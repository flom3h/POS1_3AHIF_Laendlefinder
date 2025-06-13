using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using Laendlefinder.Classes;
using Laendlefinder.Collections;

namespace Laendlefinder.Pages;

public partial class FavoritesPage : Page
{
    private int uid = LoginPage.CurrentUserID == 0 ? RegisterPage.CurrentUserID : LoginPage.CurrentUserID;
    private EventCollection eventCollection = new();
    public static event EventHandler HomeButtonClickedNavHome;
    public static event EventHandler ExploreButtonClickedNavExplore;
    public static event EventHandler CalendarButtonClickedNavCalendar;
    public static event EventHandler FavsButtonClickedNavFavs;
    public static event EventHandler MapButtonClickedNavMap;
    public static event EventHandler ProfileButtonClickedNavProfile;
    
    public FavoritesPage()
    {
        InitializeComponent();
        LoadEventsAsync();
    }
    
    private async void LoadEventsAsync()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                client.BaseAddress = new Uri("http://127.0.0.1:8081");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"/favoriten/{uid}");
                response.EnsureSuccessStatusCode();

                string responseString = await response.Content.ReadAsStringAsync();
                var events = JsonSerializer.Deserialize<List<Event>>(responseString);

                eventCollection.Clear();
                foreach (var ev in events)
                {
                    eventCollection.Add(ev);
                }

                eventCollection.Draw(EventsPanel);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Events: " + ex.Message);
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