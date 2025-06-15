using System.Windows;
using System.Windows.Controls;
using Laendlefinder.Collections;

namespace Laendlefinder.Pages;

public partial class ExplorePage : Page
{
    private EventCollection eventCollection = MainPage.eventCollection;
    private EventCollection filteredCollection = new();
    public static event EventHandler HomeButtonClickedNavHome;
    public static event EventHandler ExploreButtonClickedNavExplore;
    public static event EventHandler FavsButtonClickedNavFavs;
    public static event EventHandler MapButtonClickedNavMap;
    public static event EventHandler ProfileButtonClickedNavProfile;
    public ExplorePage()
    {
        InitializeComponent();
        eventCollection.Draw(EventsPanel);
    }

    private void HomeButton_Click(object sender, RoutedEventArgs e)
    {
        HomeButtonClickedNavHome?.Invoke(this, EventArgs.Empty);
    }

    private void ExploreButton_Click(object sender, RoutedEventArgs e)
    {
        ExploreButtonClickedNavExplore?.Invoke(this, EventArgs.Empty);
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

    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
        var selectedRadio = DateFilterPanel.Children.OfType<RadioButton>().FirstOrDefault(rb => rb.IsChecked == true);

        if (selectedRadio != null)
        {
            string selectedText = selectedRadio.Content.ToString();
            if (selectedText == "Heute")
            {
                filteredCollection = eventCollection.FilterByDate(DateTime.Today);
                EventsPanel.Children.Clear();
                filteredCollection.Draw(EventsPanel);
            }
            else if (selectedText == "Morgen")
            {
                filteredCollection = eventCollection.FilterByDate(DateTime.Today.AddDays(1));
                EventsPanel.Children.Clear();
                filteredCollection.Draw(EventsPanel);
            }
            else if (selectedText == "Nächste 7 Tage")
            {
                var startDate = DateTime.Today;
                var endDate = startDate.AddDays(7);
                filteredCollection = eventCollection.FilterByDateRange(startDate, endDate);
                EventsPanel.Children.Clear();
                filteredCollection.Draw(EventsPanel);
            }
            else if (selectedText == "Datum auswählen")
            {
                var miniCalendarWindow = new MiniCalendarWindow();
                if (miniCalendarWindow.ShowDialog() == true)
                {
                    var selectedDate = miniCalendarWindow.SelectedDate;
                    filteredCollection = eventCollection.FilterByDate(selectedDate);
                    EventsPanel.Children.Clear();
                    filteredCollection.Draw(EventsPanel);
                }
            }
        }
    }
}