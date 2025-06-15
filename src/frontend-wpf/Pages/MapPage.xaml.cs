using System.Windows;
using System.Windows.Controls;
using Mapsui.UI.Wpf;
using Mapsui.Layers;
using Mapsui.Utilities;
using Mapsui.Projection;
using Mapsui;
using Mapsui.Geometries;
using Mapsui.Styles;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Laendlefinder.Classes;
using Laendlefinder.Collections;
using Mapsui.Providers;
using System.Linq;

namespace Laendlefinder.Pages;

public partial class MapPage : Page
{
    public static event EventHandler HomeButtonClickedNavHome;
    public static event EventHandler ExploreButtonClickedNavExplore;
    public static event EventHandler CalendarButtonClickedNavCalendar;
    public static event EventHandler FavsButtonClickedNavFavs;
    public static event EventHandler MapButtonClickedNavMap;
    public static event EventHandler ProfileButtonClickedNavProfile;
    private readonly BoundingBox _vorarlbergEnvelope;
    private int? _markerBitmapId = null;

    public MapPage()
    {
        InitializeComponent();
        var tileLayer = OpenStreetMap.CreateTileLayer();
        MapView.Map?.Layers.Add(tileLayer);

        var min = SphericalMercator.FromLonLat(9.60, 46.88);
        var max = SphericalMercator.FromLonLat(10.23, 47.60);
        _vorarlbergEnvelope = new BoundingBox(min.X, min.Y, max.X, max.Y);

        MapView.Navigator.NavigateTo(_vorarlbergEnvelope, ScaleMethod.Fit, 0, null);

        MapView.Viewport.ViewportChanged += Viewport_ViewportChanged;

        LoadEventsAsync();
        MapView.Info += MapView_Info;
    }

    private bool _isResettingViewport = false;

    private void MapView_Info(object sender, Mapsui.UI.MapInfoEventArgs e)
    {
        if (e.MapInfo?.Feature != null && e.MapInfo.Layer?.Name == "EventMarkerLayer")
        {
            var eventName = e.MapInfo.Feature["EventName"] as string;
            var ev = EventCollection.Events.FirstOrDefault(ev => ev.name == eventName);
            if (ev != null)
            {
                var moreInfoPage = new MoreInfoPage((int)ev.eid);
                NavigationService?.Navigate(moreInfoPage);
            }
        } 
    }

    private void Viewport_ViewportChanged(object? sender, System.EventArgs e)
    {
        if (_isResettingViewport) return;

        var extent = MapView.Viewport.Extent;
        if (extent.MinX < _vorarlbergEnvelope.MinX ||
            extent.MinY < _vorarlbergEnvelope.MinY ||
            extent.MaxX > _vorarlbergEnvelope.MaxX ||
            extent.MaxY > _vorarlbergEnvelope.MaxY)
        {
            _isResettingViewport = true;
            MapView.Navigator.NavigateTo(_vorarlbergEnvelope, ScaleMethod.Fit, 0, null);
            _isResettingViewport = false;
        }
    }

    private async void LoadEventsAsync()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                client.BaseAddress = new Uri("http://127.0.0.1:8081");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("/events");
                response.EnsureSuccessStatusCode();

                string responseString = await response.Content.ReadAsStringAsync();
                var events = JsonSerializer.Deserialize<List<Event>>(responseString);

                EventCollection.Events.Clear();
                foreach (var ev in events)
                {
                    EventCollection.Events.Add(ev);
                }

                // Events auf der Map anzeigen
                ShowEventLocationsOnMap(EventCollection.Events);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Events: " + ex.Message);
            }
        }
    }

    public void ShowEventLocationsOnMap(IEnumerable<Event> events)
    {
        var features = new List<IFeature>();
        if (events == null)
        {
            MessageBox.Show("Events ist null!");
            return;
        }

        if (!events.Any())
        {
            MessageBox.Show("Keine Events gefunden!");
            return;
        }

        // Bild nur einmal laden und registrieren
        if (_markerBitmapId == null)
        {
            var markerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gps.png");
            if (!File.Exists(markerPath))
            {
                MessageBox.Show("gps.png nicht gefunden im Ausgabeverzeichnis!");
                return;
            }
            using var markerStream = File.OpenRead(markerPath);
            _markerBitmapId = BitmapRegistry.Instance.Register(markerStream);
        }

        foreach (var ev in events)
        {
            var point = SphericalMercator.FromLonLat(ev.Location.longitude, ev.Location.latitude);
            var feature = new Feature { Geometry = point };
            feature["EventName"] = ev.name;

            feature.Styles.Add(new SymbolStyle
            {
                SymbolScale = 0.7,
                BitmapId = _markerBitmapId.Value
            });
            features.Add(feature);
        }

        var markerLayer = new MemoryLayer
        {
            Name = "EventMarkerLayer",
            DataSource = new MemoryProvider(features)
        };

        var oldLayer = MapView.Map.Layers.FirstOrDefault(l => l.Name == "EventMarkerLayer");
        if (oldLayer != null)
            MapView.Map.Layers.Remove(oldLayer);

        MapView.Map.Layers.Add(markerLayer);
    }

    private IEnumerable<Event> LoadEvents()
    {
        return EventCollection.Events;
    }

    private void HomeButton_Click(object sender, RoutedEventArgs e)
    {
        HomeButtonClickedNavHome?.Invoke(this, EventArgs.Empty);
    }

    private void ExploreButton_Click(object sender, RoutedEventArgs e)
    {
        ExploreButtonClickedNavExplore?.Invoke(this, EventArgs.Empty);
    }

    private void CalndarButton_Click(object sender, RoutedEventArgs e)
    {
        CalendarButtonClickedNavCalendar?.Invoke(this, EventArgs.Empty);
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