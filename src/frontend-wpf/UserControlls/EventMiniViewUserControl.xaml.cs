using System.Net.Http;
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
    private bool _isFavorite = false;
    public static event EventHandler MoreInfoButtonClickedNavMoreInfo;

    public EventMiniViewUserControl()
    {
        InitializeComponent();
    }

    public void SetEventData(Event ev)
    {
        CheckIfFavoriteAsync(uid , ev.name);
        NameLabel.Content = ev.name;
        DateLabel.Content = ev.date.ToShortDateString();
        TimeLabel.Content = ev.time.ToString(@"hh\:mm");
        TypeButton.Content = ev.type?.ToString() ?? "Unbekannt";

        /*if (!string.IsNullOrEmpty(ev.picture))
        {
            try
            {
                EventImage.Source = new BitmapImage(new Uri(ev.picture, UriKind.Absolute));
                ImagePlaceholder.Visibility = Visibility.Collapsed;
            }
            catch
            {
                EventImage.Source = null;
                ImagePlaceholder.Visibility = Visibility.Visible;
            }
        }
        else
        {
            EventImage.Source = null;
            ImagePlaceholder.Visibility = Visibility.Visible;
        }*/
    }
    
    private void MoreButton_OnClick(object sender, RoutedEventArgs e)
    {
        MoreInfoButtonClickedNavMoreInfo?.Invoke(this, EventArgs.Empty);
    }

    private void FavButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (_isFavorite)
        {
            
        }
        else
        {
            
        }
    }

    public async void CheckIfFavoriteAsync(int uid, string eventName)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync($"http://127.0.0.1:8081/favoriten/{uid}/{eventName}");
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