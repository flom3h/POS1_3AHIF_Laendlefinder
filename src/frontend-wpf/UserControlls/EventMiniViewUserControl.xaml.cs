using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Laendlefinder.Classes;

namespace Laendlefinder.UserControlls;

public partial class EventMiniViewUserControl : UserControl
{
    public EventMiniViewUserControl()
    {
        InitializeComponent();
    }

    public void SetEventData(Event ev)
    {
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
        throw new NotImplementedException();
    }
}