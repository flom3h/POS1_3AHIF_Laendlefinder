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
using Laendlefinder.Classes;
using Laendlefinder.Collections;
namespace Laendlefinder.Pages;
using Microsoft.Web.WebView2.Core;

public partial class MoreInfoPage : Page
{
    private bool _isFavorite = false;
    private int uid = LoginPage.CurrentUserID == 0 ? RegisterPage.CurrentUserID : LoginPage.CurrentUserID;
    public int eid = 0;
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
                        NameLabel.Content = ev.name ?? "Name unbekannt";
                        DateText.Text = ev.date.ToShortDateString() ?? "Kein Datum verfügbar";
                        TimeText.Text = ev.time.ToString(@"hh\:mm") ?? "Keine Uhrzeit verfügbar";
                        LocationText.Text = ev.location.name + ", "+ ev.location.address ?? "Keine Adresse verfügbar"; 
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
                        DescriptionWebView.EnsureCoreWebView2Async().ContinueWith(_ =>
                        {
                            DescriptionWebView.Dispatcher.Invoke(() =>
                            {
                                var html = $"<html><body style='font-family:sans-serif;font-size:14px'>{ev.description}</body></html>";
                                DescriptionWebView.NavigateToString(html);
                            });
                        });
                        double lat = ev.location.latitude;
                        double lng = ev.location.longitude;

                        LoadGoogleMaps(lat, lng);



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
    private async void LoadGoogleMaps(double latitude, double longitude)
    {
        await GoogleMapBrowser.EnsureCoreWebView2Async();

        string mapsUrl = $"https://www.google.com/maps/search/?api=1&query={latitude.ToString(CultureInfo.InvariantCulture)},{longitude.ToString(CultureInfo.InvariantCulture)}";

        GoogleMapBrowser.CoreWebView2.Navigate(mapsUrl);
    }
    
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
                    FavButton.Background = Brushes.Black;
                    FavButton.Foreground = Brushes.White;
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
                    FavButton.Background = Brushes.White;
                    FavButton.Foreground = Brushes.Black;
                }
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