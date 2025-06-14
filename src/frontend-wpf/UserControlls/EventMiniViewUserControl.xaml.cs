using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Laendlefinder.Classes;
using Laendlefinder.Pages;

namespace Laendlefinder.UserControlls;

public partial class EventMiniViewUserControl : UserControl
{
    private int uid = LoginPage.CurrentUserID == 0 ? RegisterPage.CurrentUserID : LoginPage.CurrentUserID;
    public int eid = 0;
    private bool _isFavorite = false;
    public static event EventHandler<int> MoreInfoButtonClickedNavMoreInfo; // int für eid

    public EventMiniViewUserControl()
    {
        InitializeComponent();
    }

    public void SetEventData(Event ev)
    {
        eid = (int)ev.eid;
        CheckIfFavoriteAsync(uid, eid);
        NameLabel.Content = ev.name;
        DateLabel.Content = ev.date.ToShortDateString();
        TimeLabel.Content = ev.time.ToString(@"hh\:mm");
        TypeButton.Content = ev.type?.ToString() ?? "Unbekannt";

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
    }
    
    private void MoreButton_OnClick(object sender, RoutedEventArgs e)
    {
        MoreInfoButtonClickedNavMoreInfo?.Invoke(this, eid);
        
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

    public async void CheckIfFavoriteAsync(int uid, int eid)
    {
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync($"http://127.0.0.1:8081/favoriten/{uid}/{eid}");
            if (response.IsSuccessStatusCode)
            {
                _isFavorite = true;
                FavButton.Background = Brushes.White;
                FavButton.Foreground = Brushes.Black;
            }
            else
            {
                _isFavorite = false;
                FavButton.Background = Brushes.Black;
                FavButton.Foreground = Brushes.White;
            }
        }
    }
}