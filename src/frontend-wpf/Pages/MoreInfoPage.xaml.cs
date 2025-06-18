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
using System.Windows.Shapes;
using Laendlefinder.Classes;
using Laendlefinder.Collections;
namespace Laendlefinder.Pages;
using Microsoft.Web.WebView2.Core;

/**
 * @class MoreInfoPage
 * @brief Repräsentiert die Detailseite eines Events.
 * Zeigt Informationen zu einem Event an, einschließlich Name, Datum, Uhrzeit, Ort, Beschreibung und Bild.
 * Ermöglicht das Hinzufügen oder Entfernen des Events aus den Favoriten.
 */
public partial class MoreInfoPage : Page
{
    private bool _isFavorite = false;
    private int uid = LoginPage.CurrentUserID == 0 ? RegisterPage.CurrentUserID : LoginPage.CurrentUserID;
    public int eid = 0;
    private const string StarOutlinePath = "M1306.181 1110.407c-28.461 20.781-40.32 57.261-29.477 91.03l166.136 511.398-435.05-316.122c-28.686-20.781-67.086-20.781-95.66 0l-435.05 316.122 166.25-511.623c10.842-33.544-1.017-70.024-29.591-90.805L178.577 794.285h537.825c35.351 0 66.523-22.701 77.365-56.245l166.25-511.51 166.136 511.397a81.155 81.155 0 0 0 77.365 56.358h537.939l-435.276 316.122Zm609.77-372.819c-10.956-33.656-42.014-56.244-77.365-56.244h-612.141l-189.064-582.1C1026.426 65.589 995.367 43 960.017 43c-35.351 0-66.523 22.588-77.365 56.245L693.475 681.344H81.335c-35.351 0-66.41 22.588-77.366 56.244-10.842 33.657 1.017 70.137 29.591 90.918l495.247 359.718-189.29 582.211c-10.842 33.657 1.017 70.137 29.704 90.918 14.23 10.39 31.059 15.586 47.661 15.586 16.829 0 33.657-5.195 47.887-15.699l495.248-359.718 495.02 359.718c28.575 20.894 67.088 20.894 95.775.113 28.574-20.781 40.433-57.261 29.59-91.03l-189.289-582.1 495.247-359.717c28.687-20.781 40.546-57.261 29.59-90.918Z";
    private const string StarFilledPath = "M1915.918 737.475c-10.955-33.543-42.014-56.131-77.364-56.131h-612.029l-189.063-582.1v-.112C1026.394 65.588 995.335 43 959.984 43c-35.237 0-66.41 22.588-77.365 56.245L693.443 681.344H81.415c-35.35 0-66.41 22.588-77.365 56.131-10.955 33.544.79 70.137 29.478 91.03l495.247 359.831-189.177 582.212c-10.955 33.657 1.13 70.25 29.817 90.918 14.23 10.278 30.946 15.487 47.66 15.487 16.716 0 33.432-5.21 47.775-15.6l495.134-359.718 495.021 359.718c28.574 20.781 67.087 20.781 95.662.113 28.687-20.668 40.658-57.261 29.703-91.03l-189.176-582.1 495.36-359.83c28.574-20.894 40.433-57.487 29.364-91.03";

    public static event EventHandler HomeButtonClickedNavHome;
    public static event EventHandler ExploreButtonClickedNavExplore;
    public static event EventHandler CalendarButtonClickedNavCalendar;
    public static event EventHandler FavsButtonClickedNavFavs;
    public static event EventHandler MapButtonClickedNavMap;
    public static event EventHandler ProfileButtonClickedNavProfile;
    public int EventId;
    /**
    * Konstruktor für die MoreInfoPage. Initialisiert die Komponenten und prüft, ob das Event ein Favorit ist.
    * @param eventId Die ID des anzuzeigenden Events.
    */
    public MoreInfoPage(int eventId)
    {
        EventId = eventId;
        InitializeComponent();
        MainWindow.Logger.Information("InfoPage initialized");
        CheckIfFavoriteAsync(uid, EventId);

        LoadEventData();
    }
    /**
    * Lädt die Eventdaten vom Server und zeigt sie auf der Seite an.
    */
    private async void LoadEventData()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                var response = await client.GetAsync($"http://127.0.0.1:8081/events/{EventId}");
            
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var ev = JsonSerializer.Deserialize<Event>(json);
                    
                    MainWindow.Logger.Information("EventDaten geladen");

                    Dispatcher.Invoke(() => {
                        NameLabel.Text = ev.name ?? "Name unbekannt";
                        DateText.Text = "- " + ev.date.ToString("dddd, dd.MM.yyyy", new CultureInfo("de-DE")) ?? "Kein Datum verfügbar";
                        if (ev.time == TimeSpan.Zero)
                            TimeText.Text = "- Ganzer Tag";
                        else
                            TimeText.Text = "- " + ev.time.ToString(@"hh\:mm") + " Uhr";                        LocationText.Text = "- " + ev.Location.name + ", "+ ev.Location.address ?? "Keine Adresse verfügbar"; 
                        if (!string.IsNullOrEmpty(ev.picture))
                        {
                            try
                            {
                                EventImage.Source = new BitmapImage(new Uri(ev.picture, UriKind.Absolute));
                                ImagePlaceholder.Visibility = Visibility.Collapsed;
                                EventImage.Visibility = Visibility.Visible;
                            }
                            catch
                            {
                                EventImage.Source = null;
                                ImagePlaceholder.Visibility = Visibility.Visible;
                                EventImage.Visibility = Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            EventImage.Source = null;
                            ImagePlaceholder.Visibility = Visibility.Visible;
                            EventImage.Visibility = Visibility.Collapsed;
                        }
                        
                        string description = string.IsNullOrWhiteSpace(ev.description) || ev.description.Trim().ToLower() == "no data"
                            ? "Keine Beschreibung verfügbar."
                            : ev.description;

                        DescriptionWebView.EnsureCoreWebView2Async().ContinueWith(_ =>
                        {
                            DescriptionWebView.Dispatcher.Invoke(() =>
                            {
                                var html = $"<html><body style='font-family:sans-serif;font-size:14px'>{description}</body></html>";
                                DescriptionWebView.NavigateToString(html);
                            });
                        });
                        double lat = ev.Location.latitude;
                        double lng = ev.Location.longitude;

                        LoadGoogleMaps(lat, lng);
                        
                    });
                }
                else
                {
                    MainWindow.Logger.Error($"Fehler beim Laden der Event Daten für Event {EventId}: {response.StatusCode}");
                    MessageBox.Show($"Fehler: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MainWindow.Logger.Error(ex, $"Fehler beim Laden der Event Daten für Event {EventId}");
                MessageBox.Show($"Fehler: {ex.Message}");
            }
        }
    }
    /**
    * Lädt die Google Maps Ansicht für die angegebenen Koordinaten.
    * @param latitude Die Breite des Standorts.
    * @param longitude Die Länge des Standorts.
    */
    private async void LoadGoogleMaps(double latitude, double longitude)
    {
        await GoogleMapBrowser.EnsureCoreWebView2Async();
        MainWindow.Logger.Information("GoogleMaps gestartet");
        string mapsUrl = $"https://www.google.com/maps/search/?api=1&query={latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}";
    
        GoogleMapBrowser.CoreWebView2.Navigate(mapsUrl);
    }
    /**
    * Prüft, ob das Event ein Favorit des aktuellen Benutzers ist.
    */
    private async void CheckIfFavorite()
    {
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync($"http://127.0.0.1:8081/favoriten/check/{uid}/{EventId}");
            if (response.IsSuccessStatusCode)
            {
                _isFavorite = true;
                SetFavIcon(true);
            }
            else
            {
                _isFavorite = false;
                SetFavIcon(false);
            }
        }
    }
    /**
    * Setzt das Favoriten-Icon entsprechend dem Favoritenstatus des Events.
    * @param isFavorite Der Favoritenstatus.
    */
    private void SetFavIcon(bool isFavorite)
    {
        if (FavButton.Content is Viewbox viewbox && viewbox.Child is Path path)
        {
            path.Data = Geometry.Parse(isFavorite ? StarFilledPath : StarOutlinePath);
        }
    }
    /**
    * Event-Handler für den Klick auf den Favoriten-Button.
    * Fügt das Event zu den Favoriten hinzu oder entfernt es aus den Favoriten.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private async void FavButton_OnClick(object sender, RoutedEventArgs e)
    {
        MainWindow.Logger.Information($"Favoriten-Button geklickt für Event von Benutzer");
        if (_isFavorite)
        {
            // DELETE
            using (HttpClient client = new HttpClient())
            {
                var response = await client.DeleteAsync($"http://127.0.0.1:8081/favoriten/delete/{uid}/{EventId}");
                if (response.IsSuccessStatusCode)
                {
                    MainWindow.Logger.Information($"Event {EventId} wurde aus den Favoriten entfernt.");
                    _isFavorite = false;
                    SetFavIcon(false);
                }
            }
        }
        else
        {
            // POST
            var favData = new FavRequest() { uid = uid, eid = EventId };
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://127.0.0.1:8081");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string json = JsonSerializer.Serialize(favData);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("/favoriten", content);
                if (response.IsSuccessStatusCode)
                {
                    MainWindow.Logger.Information($"Event {EventId} wurde zu den Favoriten hinzugefügt.");
                    _isFavorite = true;
                    SetFavIcon(true);
                }
            }
        }
    }
    /**
    * Prüft asynchron, ob das Event ein Favorit des aktuellen Benutzers ist.
    * @param userId Die User-ID.
    * @param eventId Die Event-ID.
    */
    public async void CheckIfFavoriteAsync(int uid, int eid)
    {
        MainWindow.Logger.Information($"Prüfe, ob Event ein Favorit für Benutzer ist.");
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync($"http://127.0.0.1:8081/favoriten/{uid}/{eid}");
            if (response.IsSuccessStatusCode)
            {
                MainWindow.Logger.Information($"Event {eid} ist ein Favorit für Benutzer {uid}.");
                _isFavorite = true;
                FavButton.Foreground = Brushes.Black;
            }
            else
            {
                MainWindow.Logger.Information($"Event {eid} ist kein Favorit für Benutzer {uid}.");
                _isFavorite = false;
                FavButton.Foreground = Brushes.White;
            }
            SetFavIcon(_isFavorite);
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
        MainWindow.Logger.Information("HomeButton geklickt, Navigation zur Startseite.");
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
        MainWindow.Logger.Information("ExploreButton geklickt, Navigation zur Erkundungsseite.");
        ExploreButtonClickedNavExplore?.Invoke(this, EventArgs.Empty);
    }
    /**
    * Event-Handler für den Kalender-Button. Löst das CalendarButtonClickedNavCalendar-Event aus.
    * Leitet zur Kalenderseite weiter.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void CalndarButton_Click(object sender, RoutedEventArgs e)
    {
        MainWindow.Logger.Information("KalenderButton geklickt, Navigation zur Kalenderseite.");
        CalendarButtonClickedNavCalendar?.Invoke(this, EventArgs.Empty);
    }
    /**
    * Event-Handler für den Favs-Button. Löst das FavsButtonClickedNavFavs-Event aus.
    * Leitet zur FavsPage weiter.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void FavsButton_Click(object sender, RoutedEventArgs e)
    {
        MainWindow.Logger.Information("FavsButton geklickt, Navigation zur Favoritenseite.");
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
        MainWindow.Logger.Information("MapButton geklickt, Navigation zur Kartenseite.");
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
        MainWindow.Logger.Information("ProfileButton geklickt, Navigation zur Profilseite.");
        ProfileButtonClickedNavProfile?.Invoke(this, EventArgs.Empty);
    }
}