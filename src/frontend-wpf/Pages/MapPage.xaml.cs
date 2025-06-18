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

/**
* @class MapPage
* @brief Repräsentiert die Karten-Seite der Anwendung.
* Zeigt eine Karte mit Events an und ermöglicht die Navigation zu anderen Seiten.
*/
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

        /**
        * Konstruktor für die MapPage. Initialisiert die Komponenten, Karte und Events.
        */
        public MapPage()
        {
            InitializeComponent();
            MainWindow.Logger.Information("MapPage initialized");
            
            var tileLayer = OpenStreetMap.CreateTileLayer();
            MapView.Map?.Layers.Add(tileLayer);
            
            var min = SphericalMercator.FromLonLat(9.60, 46.88);
            var max = SphericalMercator.FromLonLat(10.23, 47.60);
            _vorarlbergEnvelope = new BoundingBox(min.X, min.Y, max.X, max.Y);
            
            MapView.MouseMove += MapView_MouseMove;
            MapView.MouseLeftButtonUp += MapView_MouseLeftButtonUp;
            
            MapView.Loaded += (sender, e) => 
            {
                MapView.Navigator.NavigateTo(_vorarlbergEnvelope, ScaleMethod.Fit, 0);
            };

            LoadEventsAsync();
        }
        
        /**
        * Event-Handler für Mausbewegungen über der Karte.
        * Ändert den Cursor, wenn sich die Maus über einem Event-Marker befindet.
        * @param sender Das auslösende Objekt.
        * @param e Maus-Event-Argumente.
        */
        private void MapView_MouseMove(object sender, MouseEventArgs e)
        {
            MainWindow.Logger.Information("MapView_MouseMove: Maus bewegt sich über der Karte.");
            var wpfPoint = e.GetPosition(MapView);
            var mapsuiPoint = new Mapsui.Geometries.Point(wpfPoint.X, wpfPoint.Y);
            var info = MapView.GetMapInfo(mapsuiPoint);
            
            MapView.Cursor = (info?.Feature != null && info.Layer?.Name == "EventMarkerLayer") ? Cursors.Hand : Cursors.Arrow;
            
        }

        /**
        * Event-Handler für Mausklicks auf der Karte.
        * Öffnet die Detailansicht eines Events, wenn ein Marker angeklickt wird.
        * @param sender Das auslösende Objekt.
        * @param e Maus-Event-Argumente.
        */
        private void MapView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var wpfPoint = e.GetPosition(MapView);
            var mapsuiPoint = new Mapsui.Geometries.Point(wpfPoint.X, wpfPoint.Y);
            var info = MapView.GetMapInfo(mapsuiPoint);

            if (info?.Feature == null || info.Layer?.Name != "EventMarkerLayer")
            {
                MainWindow.Logger.Information("MapView_MouseLeftButtonUp: Kein Event-Marker angeklickt.");
                return;
            }
            
            var eventIdObj = info.Feature["EventId"];
            if (eventIdObj == null)
            {
                MainWindow.Logger.Error("MapView_MouseLeftButtonUp: EventID wurde nicht gefunden");
                return;
            }

            int eventId;
            try
            {
                eventId = Convert.ToInt32(eventIdObj);
            }
            catch
            {
                MainWindow.Logger.Error("MapView_MouseLeftButtonUp: Falsche EventID");
                MessageBox.Show("Ungültige Event-ID.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            var moreInfoPage = new MoreInfoPage(eventId);
            if (NavigationService != null)
                NavigationService.Navigate(moreInfoPage);
            else
            {
                MainWindow.Logger.Error("MapView_MouseLeftButtonUp: NavigationService ist nicht verfügbar.");
                MessageBox.Show("NavigationService ist nicht verfügbar.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);   
            }
        }

        /**
        * Lädt die Events asynchron vom Server und zeigt sie auf der Karte an.
        */
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
                MainWindow.Logger.Information("Events auf Karte geladen");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Events: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /**
        * Zeigt die Event-Locations als Marker auf der Karte an.
        * @param events Die anzuzeigenden Events.
        */
        private void ShowEventLocationsOnMap(IEnumerable<Event> events)
        {
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
            MainWindow.Logger.Information("Marker hinzugefügt");
            MapView.Refresh();
        }

        /**
        * Event-Handler für den Home-Button. Löst das HomeButtonClickedNavHome-Event aus.
        * Leitet zur HomePage weiter.
        * @param sender Das auslösende Objekt.
        * @param e Event-Argumente.
        */
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Logger.Information("Home-Button wurde geklickt, Navigation zur HomePage.");
            HomeButtonClickedNavHome?.Invoke(this, EventArgs.Empty);
        }

        /**
        * Event-Handler für den Explore-Button. Löst das ExploreButtonClickedNavExplore-Event aus.
        * Leitet zur ExplorePage weiter.
        * @param sender Das auslösende Objekt.
        * @param e Event-Argumente.
        */
        private void ExploreButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Logger.Information("Explore-Button wurde geklickt, Navigation zur ExplorePage.");
            ExploreButtonClickedNavExplore?.Invoke(this, EventArgs.Empty);
        }

        /**
        * Event-Handler für den Favs-Button. Löst das FavsButtonClickedNavFavs-Event aus.
        * Leitet zur FavsPage weiter.
        * @param sender Das auslösende Objekt.
        * @param e Event-Argumente.
        */
        private void FavsButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Logger.Information("Favs-Button wurde geklickt, Navigation zur FavsPage.");
            FavsButtonClickedNavFavs?.Invoke(this, EventArgs.Empty);
        }

        /**
        * Event-Handler für den Map-Button. Löst das MapButtonClickedNavMap-Event aus.
        * Leitet zur MapPage weiter.
        * @param sender Das auslösende Objekt.
        * @param e Event-Argumente.
        */
        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Logger.Information("Map-Button wurde geklickt, Navigation zur MapPage.");
            MapButtonClickedNavMap?.Invoke(this, EventArgs.Empty);
        }

        /**
        * Event-Handler für den Profile-Button. Löst das ProfileButtonClickedNavProfile-Event aus.
        * Leitet zur ProfilePage weiter.
        * @param sender Das auslösende Objekt.
        * @param e Event-Argumente.
        */
        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Logger.Information("Profile-Button wurde geklickt, Navigation zur ProfilePage.");
            ProfileButtonClickedNavProfile?.Invoke(this, EventArgs.Empty);
        } 
    }
}
