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

public partial class MainPage : Page
{
    public static EventCollection eventCollection = new();
    private EventCollection filteredCollection = new();
    public static event EventHandler HomeButtonClickedNavHome;
    public static event EventHandler ExploreButtonClickedNavExplore;
    public static event EventHandler FavsButtonClickedNavFavs;
    public static event EventHandler MapButtonClickedNavMap;
    public static event EventHandler ProfileButtonClickedNavProfile;
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
    
    private void HomeButton_Click(object sender, RoutedEventArgs e)
    {
        HomeButtonClickedNavHome?.Invoke(this, EventArgs.Empty);
        MainWindow.Logger.Information("HomeButton geklickt, Navigation zur Startseite.");
    }

    private void ExploreButton_Click(object sender, RoutedEventArgs e)
    {
        ExploreButtonClickedNavExplore?.Invoke(this, EventArgs.Empty);
        MainWindow.Logger.Information("ExploreButton geklickt, Navigation zur Erkundungsseite.");
    }

    private void FavsButton_Click(object sender, RoutedEventArgs e)
    {
        FavsButtonClickedNavFavs?.Invoke(this, EventArgs.Empty);
        MainWindow.Logger.Information("FavsButton geklickt, Navigation zur Favoritenseite.");
    }

    private void MapButton_Click(object sender, RoutedEventArgs e)
    {
        MapButtonClickedNavMap?.Invoke(this, EventArgs.Empty);
        MainWindow.Logger.Information("MapButton geklickt, Navigation zur Kartenseite.");
    }

    private void ProfileButton_Click(object sender, RoutedEventArgs e)
    {
        ProfileButtonClickedNavProfile?.Invoke(this, EventArgs.Empty);
        MainWindow.Logger.Information("ProfileButton geklickt, Navigation zur Profilseite.");
    }

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