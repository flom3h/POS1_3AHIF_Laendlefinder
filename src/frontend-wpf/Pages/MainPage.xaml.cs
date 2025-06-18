using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Laendlefinder.Classes;
using Laendlefinder.Collections;
using Serilog.Core;

namespace Laendlefinder.Pages;

/**
 * @class MainPage
 * @brief Repräsentiert die Hauptseite der Anwendung.
 * Zeigt eine Liste von Events an und ermöglicht die Navigation zu anderen Seiten.
 */
public partial class MainPage : Page
{
    public static EventCollection eventCollection = new();
    private EventCollection filteredCollection = new();
    public static event EventHandler HomeButtonClickedNavHome;
    public static event EventHandler ExploreButtonClickedNavExplore;
    public static event EventHandler FavsButtonClickedNavFavs;
    public static event EventHandler MapButtonClickedNavMap;
    public static event EventHandler ProfileButtonClickedNavProfile;

    /**
    * Konstruktor für die MainPage. Initialisiert die Komponenten und lädt die Events.
    */
    public MainPage()
    {
        InitializeComponent();
        /*StartBackendServer();
        Task.Delay(16000);*/
        LoadEventsAsync();
        MainWindow.Logger.Information("FavoritesPage initialized");
    }
    
    /*private void StartBackendServer()
    {
        string backendDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\src\backend");
        string scriptPath = System.IO.Path.Combine(backendDir, "start_server.py");

        if (!System.IO.Directory.Exists(backendDir))
        {
            MessageBox.Show("Backend-Verzeichnis nicht gefunden: " + backendDir);
            return;
        }
        if (!System.IO.File.Exists(scriptPath))
        {
            MessageBox.Show("Backend-Skript nicht gefunden: " + scriptPath);
            return;
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = "python.exe",
            Arguments = $"\"{scriptPath}\"",
            WorkingDirectory = backendDir,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        Process.Start(startInfo);
    }*/

    /**
    * Lädt die Events asynchron vom Server und zeigt sie an.
    */
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

                eventCollection.Clear();
                foreach (var ev in events)
                {
                    eventCollection.Add(ev);
                }

                eventCollection.Draw(EventsPanel);
                MainWindow.Logger.Information("Events erfolgreich geladen und angezeigt.");
            }
            catch (Exception ex)
            {
                MainWindow.Logger.Error("Fehler beim Laden der Events: " + ex.Message);
                MessageBox.Show("Fehler beim Laden der Events: " + ex.Message);
            }
        }
    }
    
    /**
    * Event-Handler für den Home-Button. Löst das HomeButtonClickedNavHome-Event aus.
    * Leitet zur HomePage weiter.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void HomeButton_Click(object sender, RoutedEventArgs e)
    {
        HomeButtonClickedNavHome?.Invoke(this, EventArgs.Empty);
        MainWindow.Logger.Information("HomeButton geklickt, Navigation zur Startseite.");
    }

    /**
    * Event-Handler für den Explore-Button. Löst das ExploreButtonClickedNavExplore-Event aus.
    * Leitet zur ExplorePage weiter.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void ExploreButton_Click(object sender, RoutedEventArgs e)
    {
        ExploreButtonClickedNavExplore?.Invoke(this, EventArgs.Empty);
        MainWindow.Logger.Information("ExploreButton geklickt, Navigation zur Erkundungsseite.");
    }

    /**
    * Event-Handler für den Favs-Button. Löst das FavsButtonClickedNavFavs-Event aus.
    * Leitet zur Favoritenseite weiter.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void FavsButton_Click(object sender, RoutedEventArgs e)
    {
        FavsButtonClickedNavFavs?.Invoke(this, EventArgs.Empty);
        MainWindow.Logger.Information("FavsButton geklickt, Navigation zur Favoritenseite.");
    }

    /**
    * Event-Handler für den Map-Button. Löst das MapButtonClickedNavMap-Event aus.
    * Leitet zur Kartenseite weiter.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void MapButton_Click(object sender, RoutedEventArgs e)
    {
        MapButtonClickedNavMap?.Invoke(this, EventArgs.Empty);
        MainWindow.Logger.Information("MapButton geklickt, Navigation zur Kartenseite.");
    }

    /**
    * Event-Handler für den Profile-Button. Löst das ProfileButtonClickedNavProfile-Event aus.
    * Leitet zur Profilseite weiter.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void ProfileButton_Click(object sender, RoutedEventArgs e)
    {
        ProfileButtonClickedNavProfile?.Invoke(this, EventArgs.Empty);
        MainWindow.Logger.Information("ProfileButton geklickt, Navigation zur Profilseite.");
    }

    /**
    * Event-Handler für den Search-Button. Sucht nach Events anhand des eingegebenen Suchtextes und zeigt die Ergebnisse an.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        string searchText = SearchBox.Text.Trim();
        if (string.IsNullOrEmpty(searchText))
        {
            eventCollection.Draw(EventsPanel);
        }
        filteredCollection = eventCollection.Search(searchText);
        EventsPanel.Children.Clear();
        filteredCollection.Draw(EventsPanel);
        MainWindow.Logger.Information($"Suchergebnisse für '{searchText}' angezeigt.");
    }
}