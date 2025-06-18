using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using Laendlefinder.Classes;
using Laendlefinder.Collections;

namespace Laendlefinder.Pages;

/**
 * @class FavoritesPage
 * @brief Repräsentiert die Favoriten-Seite der Anwendung.
 * Zeigt eine Liste von favorisierten Events des aktuellen Benutzers an.
 */
public partial class FavoritesPage : Page
{
    private int uid = LoginPage.CurrentUserID == 0 ? RegisterPage.CurrentUserID : LoginPage.CurrentUserID;
    private EventCollection eventCollection = new();
    public static event EventHandler HomeButtonClickedNavHome;
    public static event EventHandler ExploreButtonClickedNavExplore;
    public static event EventHandler FavsButtonClickedNavFavs;
    public static event EventHandler MapButtonClickedNavMap;
    public static event EventHandler ProfileButtonClickedNavProfile;
    
    /**
    * Konstruktor für die FavoritesPage. Initialisiert die Komponenten und lädt die Favoriten-Events.
    */
    public FavoritesPage()
    {
        InitializeComponent();
        LoadEventsAsync();
        MainWindow.Logger.Information("FavoritesPage initialized");
    }
    
    /**
    * Lädt die Favoriten-Events asynchron vom Server und zeigt sie an.
    */
    private async void LoadEventsAsync()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                client.BaseAddress = new Uri("http://127.0.0.1:8081");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"/favoriten/{uid}");
                if (!response.IsSuccessStatusCode)
                {
                    NoFavoritesLbl.Visibility = Visibility.Visible;
                    MainWindow.Logger.Information($"Keine Favoriten für Benutzer {uid} gefunden.");
                    return;
                }

                string responseString = await response.Content.ReadAsStringAsync();
                var events = JsonSerializer.Deserialize<List<Event>>(responseString);
                
                NoFavoritesLbl.Visibility = Visibility.Collapsed;
                eventCollection.Clear();
                foreach (var ev in events)
                {
                    eventCollection.Add(ev);
                }

                eventCollection.Draw(EventsPanel);
                MainWindow.Logger.Information($"Favoriten für Benutzer {uid} geladen: {events.Count} Events gefunden.");
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
    }

    /**
    * Event-Handler für den Favs-Button. Löst das FavsButtonClickedNavFavs-Event aus.
    * Leitet zur FavoritesPage weiter.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void FavsButton_Click(object sender, RoutedEventArgs e)
    {
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
        ProfileButtonClickedNavProfile?.Invoke(this, EventArgs.Empty);
    }
}