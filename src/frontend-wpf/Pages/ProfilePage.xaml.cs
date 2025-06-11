using System.Windows;
using System.Windows.Controls;

namespace Laendlefinder.Pages;

public partial class ProfilePage : Page
{
    private bool passwordVisible = false;
    public static event EventHandler HomeButtonClickedNavHome;
    public static event EventHandler ExploreButtonClickedNavExplore;
    public static event EventHandler CalendarButtonClickedNavCalendar;
    public static event EventHandler FavsButtonClickedNavFavs;
    public static event EventHandler MapButtonClickedNavMap;
    public static event EventHandler ProfileButtonClickedNavProfile;
    public static event EventHandler SaveChangesButtonClickedNavHome;
    public static event EventHandler LogOutButtonClickedNavLogin;
    public ProfilePage()
    {
        InitializeComponent();
        PasswordBox.Visibility = Visibility.Visible;
        PlainPasswordBox.Visibility = Visibility.Collapsed;
    }
    
    private void ChangePasswordVisibility_Click(object sender, RoutedEventArgs e)
    {
        passwordVisible = !passwordVisible;

        if (passwordVisible)
        {
            PlainPasswordBox.Text = PasswordBox.Password;
            PasswordBox.Visibility = Visibility.Collapsed;
            PlainPasswordBox.Visibility = Visibility.Visible;
        }
        else
        {
            PasswordBox.Password = PlainPasswordBox.Text;
            PasswordBox.Visibility = Visibility.Visible;
            PlainPasswordBox.Visibility = Visibility.Collapsed;
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

    private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void LogOutButton_Click(object sender, RoutedEventArgs e)
    {
        LogOutButtonClickedNavLogin?.Invoke(this, EventArgs.Empty);
    }
}