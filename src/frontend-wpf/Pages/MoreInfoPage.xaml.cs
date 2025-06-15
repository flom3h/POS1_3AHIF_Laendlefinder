using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Text.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Laendlefinder.Classes;
using Laendlefinder.Collections;
using System.Diagnostics;
using System.Windows.Navigation;
using Microsoft.Web.WebView2.Core;
using Mapsui;
using Mapsui.Projection;
using Mapsui.Utilities;
using Mapsui.Layers;
using Mapsui.Providers;
using Mapsui.Styles;
using System.IO;
using System.Linq;
using Mapsui.Geometries;
using Mapsui; // Für BitmapRegistry

namespace Laendlefinder.Pages
{
    public partial class MoreInfoPage : Page
    {
        private static int? _markerBitmapId = null;
        private bool _isFavorite = false;
        private int uid = LoginPage.CurrentUserID == 0 ? RegisterPage.CurrentUserID : LoginPage.CurrentUserID;
        public int eid = 0;
        public static event EventHandler HomeButtonClickedNavHome;
        public static event EventHandler ExploreButtonClickedNavExplore;
        public static event EventHandler FavsButtonClickedNavFavs;
        public static event EventHandler MapButtonClickedNavMap;
        public static event EventHandler ProfileButtonClickedNavProfile;
        public int EventId;

        private BoundingBox _vorarlbergEnvelope;
        private bool _isResettingViewport = false;

        public MoreInfoPage(int eventId)
        {
            EventId = eventId;
            InitializeComponent();
            LoadEventAndInitMapAsync();
        }

        private async void LoadEventAndInitMapAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://127.0.0.1:8081");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync($"/events/{EventId}");
                    response.EnsureSuccessStatusCode();

                    string responseString = await response.Content.ReadAsStringAsync();
                    var ev = JsonSerializer.Deserialize<Event>(responseString);

                    if (ev == null || ev.Location == null)
                    {
                        MessageBox.Show("Keine Location-Daten gefunden!");
                        return;
                    }

                    eid = (int)ev.eid; // Typumwandlung wie in MapPage.xaml.cs

                    // Adresse oder Koordinaten anzeigen
                    if (!string.IsNullOrWhiteSpace(ev.Location.address))
                        LocationText.Text = ev.Location.address;
                    else
                        LocationText.Text = $"Lon: {ev.Location.longitude}, Lat: {ev.Location.latitude}";

                    // --- Karten-Logik wie in MapPage.xaml.cs ---
                    var tileLayer = OpenStreetMap.CreateTileLayer();
                    OpenStreetMapViewer.Map = new Mapsui.Map();
                    OpenStreetMapViewer.Map.Layers.Add(tileLayer);
                    double delta = 0.002;
                    var min = SphericalMercator.FromLonLat(ev.Location.longitude - delta, ev.Location.latitude - delta);
                    var max = SphericalMercator.FromLonLat(ev.Location.longitude + delta, ev.Location.latitude + delta);
                    _vorarlbergEnvelope = new BoundingBox(min.X, min.Y, max.X, max.Y);

                    OpenStreetMapViewer.Navigator.NavigateTo(_vorarlbergEnvelope, ScaleMethod.Fit, 0, null);

                    OpenStreetMapViewer.Viewport.ViewportChanged += Viewport_ViewportChanged;

                    // Marker-Bild laden und registrieren
                    if (_markerBitmapId == null)
                    {
                        var markerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gps.png");
                        if (File.Exists(markerPath))
                        {
                            using var markerStream = File.OpenRead(markerPath);
                            _markerBitmapId = BitmapRegistry.Instance.Register(markerStream);
                        }
                    }

                    // Marker setzen wie in MapPage.xaml.cs
                    var features = new List<IFeature>();
                    var point = SphericalMercator.FromLonLat(ev.Location.longitude, ev.Location.latitude);
                    var feature = new Feature { Geometry = point };
                    feature.Styles.Add(new SymbolStyle
                    {
                        SymbolScale = 0.7,
                        BitmapId = _markerBitmapId ?? 0
                    });
                    features.Add(feature);

                    var markerLayer = new MemoryLayer
                    {
                        Name = "EventMarkerLayer",
                        DataSource = new MemoryProvider(features)
                    };

                    // Vorherigen MarkerLayer entfernen, falls vorhanden
                    var oldLayer = OpenStreetMapViewer.Map.Layers.FirstOrDefault(l => l.Name == "EventMarkerLayer");
                    if (oldLayer != null)
                        OpenStreetMapViewer.Map.Layers.Remove(oldLayer);

                    OpenStreetMapViewer.Map.Layers.Add(markerLayer);
                    // --- Ende Karten-Logik ---
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Laden der Location: " + ex.Message);
                }
            }
        }

        private void Viewport_ViewportChanged(object? sender, System.EventArgs e)
        {
            if (_isResettingViewport) return;

            var extent = OpenStreetMapViewer.Viewport.Extent;
            if (extent.MinX < _vorarlbergEnvelope.MinX ||
                extent.MinY < _vorarlbergEnvelope.MinY ||
                extent.MaxX > _vorarlbergEnvelope.MaxX ||
                extent.MaxY > _vorarlbergEnvelope.MaxY)
            {
                _isResettingViewport = true;
                OpenStreetMapViewer.Navigator.NavigateTo(_vorarlbergEnvelope, ScaleMethod.Fit, 0, null);
                _isResettingViewport = false;
            }
        }

        private async void FavButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_isFavorite)
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.DeleteAsync($"http://127.0.0.1:8081/favoriten/delete/{uid}/{eid}");
                    if (response.IsSuccessStatusCode)
                    {
                        _isFavorite = false;
                        FavButton.Background = Brushes.Black;
                        FavButton.Foreground = Brushes.White;
                    }
                }
            }
            else
            {
                var favData = new FavRequest()
                {
                    uid = uid,
                    eid = eid
                };

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://127.0.0.1:8081");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string json = JsonSerializer.Serialize(favData);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("/favoriten", content);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        _isFavorite = true;
                        FavButton.Background = Brushes.White;
                        FavButton.Foreground = Brushes.Black;
                    }
                }
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            HomeButtonClickedNavHome?.Invoke(this, EventArgs.Empty);
        }

        private void ExploreButton_Click(object sender, RoutedEventArgs e)
        {
            ExploreButtonClickedNavExplore?.Invoke(this, EventArgs.Empty);
        }

        private void FavsButton_Click(object sender, RoutedEventArgs e)
        {
            FavsButtonClickedNavFavs?.Invoke(this, EventArgs.Empty);
        }

        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            MapButtonClickedNavMap?.Invoke(this, EventArgs.Empty);
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ProfileButtonClickedNavProfile?.Invoke(this, EventArgs.Empty);
        }
    }
}