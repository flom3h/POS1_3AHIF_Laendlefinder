using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Laendlefinder.Classes;
using Laendlefinder.Pages;

namespace Laendlefinder.UserControlls;

/*
 * @class EventMiniViewUserControl
 * @brief Repräsentiert eine Miniaturansicht eines Events.
 * Zeigt grundlegende Informationen an und ermöglicht das Hinzufügen zu Favoriten.
 */
public partial class EventMiniViewUserControl : UserControl
{
    private int uid = LoginPage.CurrentUserID == 0 ? RegisterPage.CurrentUserID : LoginPage.CurrentUserID;
    public int eid = 0;
    private bool _isFavorite = false;
    public static event EventHandler<int> MoreInfoButtonClickedNavMoreInfo;
    private const string StarOutlinePath = "M1306.181 1110.407c-28.461 20.781-40.32 57.261-29.477 91.03l166.136 511.398-435.05-316.122c-28.686-20.781-67.086-20.781-95.66 0l-435.05 316.122 166.25-511.623c10.842-33.544-1.017-70.024-29.591-90.805L178.577 794.285h537.825c35.351 0 66.523-22.701 77.365-56.245l166.25-511.51 166.136 511.397a81.155 81.155 0 0 0 77.365 56.358h537.939l-435.276 316.122Zm609.77-372.819c-10.956-33.656-42.014-56.244-77.365-56.244h-612.141l-189.064-582.1C1026.426 65.589 995.367 43 960.017 43c-35.351 0-66.523 22.588-77.365 56.245L693.475 681.344H81.335c-35.351 0-66.41 22.588-77.366 56.244-10.842 33.657 1.017 70.137 29.591 90.918l495.247 359.718-189.29 582.211c-10.842 33.657 1.017 70.137 29.704 90.918 14.23 10.39 31.059 15.586 47.661 15.586 16.829 0 33.657-5.195 47.887-15.699l495.248-359.718 495.02 359.718c28.575 20.894 67.088 20.894 95.775.113 28.574-20.781 40.433-57.261 29.59-91.03l-189.289-582.1 495.247-359.717c28.687-20.781 40.546-57.261 29.59-90.918Z";
    private const string StarFilledPath = "M1915.918 737.475c-10.955-33.543-42.014-56.131-77.364-56.131h-612.029l-189.063-582.1v-.112C1026.394 65.588 995.335 43 959.984 43c-35.237 0-66.41 22.588-77.365 56.245L693.443 681.344H81.415c-35.35 0-66.41 22.588-77.365 56.131-10.955 33.544.79 70.137 29.478 91.03l495.247 359.831-189.177 582.212c-10.955 33.657 1.13 70.25 29.817 90.918 14.23 10.278 30.946 15.487 47.66 15.487 16.716 0 33.432-5.21 47.775-15.6l495.134-359.718 495.021 359.718c28.574 20.781 67.087 20.781 95.662.113 28.687-20.668 40.658-57.261 29.703-91.03l-189.176-582.1 495.36-359.83c28.574-20.894 40.433-57.487 29.364-91.03";
    public static List<Laendlefinder.Classes.Type> Types { get; set; } = new();
    private Event? _pendingEvent;
    
    /**
     * Konstruktor für die EventMiniViewUserControl. Initialisiert die Komponenten und registriert das Loaded-Ereignis.
     */
    public EventMiniViewUserControl()
    {
        InitializeComponent();
        this.Loaded += EventMiniViewUserControl_Loaded;
    }

    private async void EventMiniViewUserControl_Loaded(object sender, RoutedEventArgs e)
    {
        // if (Types == null || Types.Count == 0)
        //     await LoadTypesAsync();
    }
    
    /**
     * Ruft den Typnamen anhand der Typ-ID ab.
     * @param typeId Die ID des Typs.
     * @return Der Name des Typs oder null, wenn nicht gefunden.
     */
    private async Task<string?> GetTypeNameById(int typeId)
    {
        await LoadTypesAsync();
    foreach (var t in Types)
    {
        Console.WriteLine($"Type in Liste: tid={t.tid}, type={t.type}");
    }
    var type = Types.FirstOrDefault(t => t.tid == typeId);
    if (type != null)
    {
        Console.WriteLine($"Gefunden: tid={type.tid}, type={type.type}");
    }
    else
    {
        Console.WriteLine("Kein passender Typ gefunden!");
    }
    return type?.type;
    MainWindow.Logger.Information($"GetTypeNameById aufgerufen mit: {typeId}, gibt zurueck type: {type?.type}");
}

    
/**
 * Lädt die Typen asynchron vom Server, wenn sie noch nicht geladen wurden.
 * Speichert die Typen in der statischen Liste Types.
 */
public static async Task LoadTypesAsync()
{
    if (Types.Count == 0)
    {
        using (HttpClient client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://127.0.0.1:8081");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("/kategorien");
            response.EnsureSuccessStatusCode();

            string responseString = await response.Content.ReadAsStringAsync();
            var types = JsonSerializer.Deserialize<List<Laendlefinder.Classes.Type>>(responseString);

            Types = types ?? new List<Laendlefinder.Classes.Type>();
        }
    }
    MainWindow.Logger.Information($"LoadTypesAsync aufgerufen, gibt zurueck: {Types.Count} Types");
}
    
    /**
     * Setzt die Event-Daten in die UI-Elemente.
     * @param ev Das Event-Objekt, dessen Daten angezeigt werden sollen.
     */
    public async void SetEventData(Event ev)
    {
        eid = ev.eid;
        Console.WriteLine($"SetEventData: ev.type={ev.type} ({ev.type.GetType()})");
        CheckIfFavoriteAsync(uid, eid);
        NameLabel.Content = ev.name;
        DateLabel.Content = ev.date.ToShortDateString();
        if (ev.time == TimeSpan.Zero)
            TimeLabel.Content = "Ganztägig";
        else
            TimeLabel.Content = ev.time.ToString(@"hh\:mm") + " Uhr";
        var typeName = await GetTypeNameById(ev.type);
        TypeButton.Content = typeName ?? "Unbekannt";
        MainWindow.Logger.Information("Daten von einem Event gesetzt: " +
                                      $"Name: {ev.name}, Datum: {ev.date.ToShortDateString()}, " +
                                      $"Zeit: {ev.time.ToString(@"hh\:mm")}, Typ: {typeName ?? "Unbekannt"}");

        if (!string.IsNullOrEmpty(ev.picture))
        {
            try
            {
                EventImage.Source = new BitmapImage(new Uri(ev.picture, UriKind.Absolute));
                ImagePlaceholder.Visibility = Visibility.Collapsed;
                EventImage.Visibility = Visibility.Visible;
                MainWindow.Logger.Information($"Event-Bild geladen: {ev.picture}");
            }
            catch
            {
                EventImage.Source = null;
                ImagePlaceholder.Visibility = Visibility.Visible;
                EventImage.Visibility = Visibility.Collapsed;
                MainWindow.Logger.Error($"Fehler beim Laden des Event-Bildes: {ev.picture}");
            }
        }
        else
        {
            EventImage.Source = null;
            ImagePlaceholder.Visibility = Visibility.Visible;
            EventImage.Visibility = Visibility.Collapsed;
            MainWindow.Logger.Information("Kein Event-Bild vorhanden, Platzhalter angezeigt.");
        }
    }
    
    /**
     * Event-Handler für den More-Button. Löst das MoreInfoButtonClickedNavMoreInfo-Event aus.
     * Leitet zur MoreInfoPage weiter und übergibt die Event-ID.
     * @param sender Das auslösende Objekt.
     * @param e Event-Argumente.
     */
    private void MoreButton_OnClick(object sender, RoutedEventArgs e)
    {
        MoreInfoButtonClickedNavMoreInfo?.Invoke(this, eid);
        MainWindow.Logger.Information("MoreButton geklickt, Navigation zur MoreInfoPage.");
    }
    
    /**
     * Event-Handler für den Favoriten-Button. Fügt das Event zu den Favoriten hinzu oder entfernt es.
     * @param sender Das auslösende Objekt.
     * @param e Event-Argumente.
     */
    private async void FavButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (_isFavorite)
        {// DELETE
            using (HttpClient client = new HttpClient())
            {
                var response = await client.DeleteAsync($"http://127.0.0.1:8081/favoriten/delete/{uid}/{eid}");
                if (response.IsSuccessStatusCode)
                {
                    _isFavorite = false;
                    FavButton.Foreground = Brushes.White;
                    SetFavIcon(_isFavorite);
                    MainWindow.Logger.Information($"Event {eid} wurde aus Favoriten entfernt.");
                }
            }
        }
        else
        {// POST
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
                    FavButton.Foreground = Brushes.Black;
                    SetFavIcon(_isFavorite);
                    MainWindow.Logger.Information($"Event {eid} wurde zu Favoriten hinzugefügt.");
                }
            }
            
        }
    }
    
    /**
     * Setzt das FavIcon basierend auf dem Favoritenstatus.
     * @param isFavorite Gibt an, ob das Event favorisiert ist.
     */
    private void SetFavIcon(bool isFavorite)
    {
        if (FavButton.Content is Viewbox viewbox && viewbox.Child is Path path)
        {
            path.Data = Geometry.Parse(isFavorite ? StarFilledPath : StarOutlinePath);
            MainWindow.Logger.Information($"FavIcon gesetzt: {isFavorite}");
        }
    }
    
    /**
     * Überprüft, ob das Event in den Favoriten des Benutzers ist.
     * @param uid Die Benutzer-ID.
     * @param eid Die Event-ID.
     */
    public async void CheckIfFavoriteAsync(int uid, int eid)
    {
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync($"http://127.0.0.1:8081/favoriten/{uid}/{eid}");
            if (response.IsSuccessStatusCode)
            {
                _isFavorite = true;
                FavButton.Foreground = Brushes.Black;
            }
            else
            {
                _isFavorite = false;
                FavButton.Foreground = Brushes.White;
            }
            SetFavIcon(_isFavorite);
            MainWindow.Logger.Information($"CheckIfFavoriteAsync aufgerufen: uid={uid}, eid={eid}, isFavorite={_isFavorite}");
        }
    }
}
