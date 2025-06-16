using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Mapsui;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Providers;
using Mapsui.Projection;
using Mapsui.Styles;
using Mapsui.UI.Wpf;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Laendlefinder.Classes;
using Laendlefinder.Collections;
using Mapsui.Utilities;

namespace Laendlefinder.Pages
{
    public partial class MapPage : Page
    {
        public static event EventHandler HomeButtonClickedNavHome;
        public static event EventHandler ExploreButtonClickedNavExplore;
        public static event EventHandler FavsButtonClickedNavFavs;
        public static event EventHandler MapButtonClickedNavMap;
        public static event EventHandler ProfileButtonClickedNavProfile;

        private readonly BoundingBox _vorarlbergEnvelope;

        public MapPage()
        {
            InitializeComponent();

            // Basiskarte hinzufügen
            var tileLayer = OpenStreetMap.CreateTileLayer();
            MapView.Map?.Layers.Add(tileLayer);

            // Viewport auf Vorarlberg setzen
            var min = SphericalMercator.FromLonLat(9.60, 46.88);
            var max = SphericalMercator.FromLonLat(10.23, 47.60);
            _vorarlbergEnvelope = new BoundingBox(min.X, min.Y, max.X, max.Y);
    
            // Events & Cursor-Handling
            MapView.MouseMove += MapView_MouseMove;
            MapView.MouseLeftButtonUp += MapView_MouseLeftButtonUp;

            // Initiales Zoomen nach Laden der MapView
            MapView.Loaded += (sender, e) => 
            {
                MapView.Navigator.NavigateTo(_vorarlbergEnvelope, ScaleMethod.Fit, 0); // 0 = keine Animation
            };

            // Events laden
            LoadEventsAsync();
        }

        // Cursor ändern, wenn über einem Event-Marker
        private void MapView_MouseMove(object sender, MouseEventArgs e)
        {
            // WPF-Point holen
            var wpfPoint = e.GetPosition(MapView);
            // In Mapsui-Point umwandeln
            var mapsuiPoint = new Mapsui.Geometries.Point(wpfPoint.X, wpfPoint.Y);
            var info = MapView.GetMapInfo(mapsuiPoint);

            MapView.Cursor = (info?.Feature != null && info.Layer?.Name == "EventMarkerLayer")
                ? Cursors.Hand
                : Cursors.Arrow;
        }

        // Linksklick auf einen Marker navigiert zur MoreInfoPage
        private void MapView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // WPF-Point holen
            var wpfPoint = e.GetPosition(MapView);
            // In Mapsui-Point umwandeln
            var mapsuiPoint = new Mapsui.Geometries.Point(wpfPoint.X, wpfPoint.Y);
            var info = MapView.GetMapInfo(mapsuiPoint);

            if (info?.Feature == null || info.Layer?.Name != "EventMarkerLayer")
                return;

            // EventId auslesen
            var eventIdObj = info.Feature["EventId"];
            if (eventIdObj == null) return;

            int eventId;
            try
            {
                eventId = Convert.ToInt32(eventIdObj);
            }
            catch
            {
                MessageBox.Show("Ungültige Event-ID.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Navigation zur Detailseite
            var moreInfoPage = new MoreInfoPage(eventId);
            if (NavigationService != null)
                NavigationService.Navigate(moreInfoPage);
            else
                MessageBox.Show("NavigationService ist nicht verfügbar.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        // Asynchrones Laden der Events aus der API
        private async void LoadEventsAsync()
        {
            using var client = new HttpClient { BaseAddress = new Uri("http://127.0.0.1:8081") };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var response = await client.GetAsync("/events");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var events = JsonSerializer.Deserialize<List<Event>>(json);

                EventCollection.Events.Clear();
                if (events != null)
                {
                    foreach (var ev in events)
                        EventCollection.Events.Add(ev);
                }

                ShowEventLocationsOnMap(EventCollection.Events);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Events: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Anzeige der Event-Marker auf der Karte
        private void ShowEventLocationsOnMap(IEnumerable<Event> events)
        {
            // Alten Layer entfernen
            var existingLayer = MapView.Map.Layers.FirstOrDefault(l => l.Name == "EventMarkerLayer");
            if (existingLayer != null)
                MapView.Map.Layers.Remove(existingLayer);

            if (events == null || !events.Any())
            {
                MapView.Refresh();
                return;
            }

            var features = new List<IFeature>();
            foreach (var ev in events)
            {
                if (ev.Location == null) continue;

                var point = SphericalMercator.FromLonLat(ev.Location.longitude, ev.Location.latitude);
                var feature = new Feature { Geometry = point };
                feature["EventId"] = ev.eid;
                feature.Styles.Add(new SymbolStyle
                {
                    SymbolType = SymbolType.Ellipse,
                    Fill = new Brush(Color.Red),
                    Outline = new Pen(Color.White, 2),
                    SymbolScale = 0.7f
                });
                features.Add(feature);
            }

            var markerLayer = new MemoryLayer
            {
                Name = "EventMarkerLayer",
                DataSource = new MemoryProvider(features),
                IsMapInfoLayer = true
            };

            MapView.Map.Layers.Add(markerLayer);
            MapView.Refresh();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e) => HomeButtonClickedNavHome?.Invoke(this, EventArgs.Empty);
        private void ExploreButton_Click(object sender, RoutedEventArgs e) => ExploreButtonClickedNavExplore?.Invoke(this, EventArgs.Empty);
        private void FavsButton_Click(object sender, RoutedEventArgs e) => FavsButtonClickedNavFavs?.Invoke(this, EventArgs.Empty);
        private void MapButton_Click(object sender, RoutedEventArgs e) { /* Bereits auf Map */ }
        private void ProfileButton_Click(object sender, RoutedEventArgs e) => ProfileButtonClickedNavProfile?.Invoke(this, EventArgs.Empty);
    }
}
