using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Text.Json;
using System.Collections.Generic;
using Laendlefinder.Classes;
using Laendlefinder.Collections;
namespace Laendlefinder.Pages;

public partial class MoreInfoPage : Page
{
    public static event EventHandler HomeButtonClickedNavHome;
    public static event EventHandler ExploreButtonClickedNavExplore;
    public static event EventHandler CalendarButtonClickedNavCalendar;
    public static event EventHandler FavsButtonClickedNavFavs;
    public static event EventHandler MapButtonClickedNavMap;
    public static event EventHandler ProfileButtonClickedNavProfile;
    public int EventId;
    public MoreInfoPage(int eventId)
    {
        EventId = eventId;
        InitializeComponent();
        LoadEventData();
    }
    
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

                    Dispatcher.Invoke(() => {
                        NameLabel.Content = ev.name;
                        DateText.Text = ev.date.ToShortDateString();
                        TimeText.Text = ev.time.ToString(@"hh\:mm");
                        //LocationTextBlock.Text = ev.location; // Ort
                        DescriptionText.Text = ev.description;
                    
                        // Google Maps (bei Bedarf aktivieren)
                        // var mapsUrl = $"https://www.google.com/maps/embed/v1/place?key=DEIN_API_KEY&q={ev.location}";
                        // GoogleMapBrowser.Navigate(new Uri(mapsUrl));
                    });
                }
                else
                {
                    MessageBox.Show($"Fehler: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler: {ex.Message}");
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