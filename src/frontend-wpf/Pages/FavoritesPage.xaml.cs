using System.Windows;
using System.Windows.Controls;

namespace Laendlefinder.Pages;

public partial class FavoritesPage : Page
{
    public static event EventHandler HomeButtonClickedNavHome;
    public static event EventHandler ExploreButtonClickedNavExplore;
    public static event EventHandler CalendarButtonClickedNavCalendar;
    public static event EventHandler FavsButtonClickedNavFavs;
    public static event EventHandler MapButtonClickedNavMap;
    public static event EventHandler ProfileButtonClickedNavProfile;
    
    public FavoritesPage()
    {
        InitializeComponent();
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