using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Laendlefinder.Pages;

namespace Laendlefinder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new RegisterPage());
            MainFrame.Navigate(new CalendarPage());
            LoginPage.LoginButtonClickedNavHome += DisplayHome;
            LoginPage.LoginButtonClickedNavRegister += DisplayRegister;

            RegisterPage.LoginButtonClickedNavLogin += DisplayLogin;
            RegisterPage.LoginButtonClickedNavHome += DisplayHome;
            
            ProfilePage.HomeButtonClickedNavHome += DisplayHome;
            ProfilePage.ExploreButtonClickedNavExplore += DisplayExplore;
            ProfilePage.CalendarButtonClickedNavCalendar += DisplayCalendar;
            ProfilePage.FavsButtonClickedNavFavs += DisplayFavorites;
            ProfilePage.MapButtonClickedNavMap += DisplayMap;
            ProfilePage.ProfileButtonClickedNavProfile += DisplayProfile;
            ProfilePage.SaveChangesButtonClickedNavHome += DisplayHome;
            ProfilePage.LogOutButtonClickedNavLogin += DisplayLogin;
            
            ExplorePage.HomeButtonClickedNavHome += DisplayHome;
            ExplorePage.ExploreButtonClickedNavExplore += DisplayExplore;
            ExplorePage.CalendarButtonClickedNavCalendar += DisplayCalendar;
            ExplorePage.FavsButtonClickedNavFavs += DisplayFavorites;
            ExplorePage.MapButtonClickedNavMap += DisplayMap;
            ExplorePage.ProfileButtonClickedNavProfile += DisplayProfile;
            
            FavoritesPage.HomeButtonClickedNavHome += DisplayHome;
            FavoritesPage.ExploreButtonClickedNavExplore += DisplayExplore;
            FavoritesPage.CalendarButtonClickedNavCalendar += DisplayCalendar;
            FavoritesPage.FavsButtonClickedNavFavs += DisplayFavorites;
            FavoritesPage.MapButtonClickedNavMap += DisplayMap;
            FavoritesPage.ProfileButtonClickedNavProfile += DisplayProfile;
        }
        
        private void DisplayHome (object sender, System.EventArgs e)
        {
            MainPage mainPage = new MainPage();
            MainFrame.Navigate(mainPage);
        }
        
        private void DisplayLogin (object sender, System.EventArgs e)
        {
            LoginPage mainPage = new LoginPage();
            MainFrame.Navigate(mainPage);
        }
        
        private void DisplayRegister (object sender, System.EventArgs e)
        {
            RegisterPage mainPage = new RegisterPage();
            MainFrame.Navigate(mainPage);
        }
        
        private void DisplayCalendar (object sender, System.EventArgs e)
        {
            CalendarPage mainPage = new CalendarPage();
            MainFrame.Navigate(mainPage);
        }
        
        private void DisplayExplore (object sender, System.EventArgs e)
        {
            ExplorePage mainPage = new ExplorePage();
            MainFrame.Navigate(mainPage);
        }
        
        private void DisplayProfile (object sender, System.EventArgs e)
        {
            ProfilePage mainPage = new ProfilePage();
            MainFrame.Navigate(mainPage);
        }
        
        private void DisplayFavorites (object sender, System.EventArgs e)
        {
            FavoritesPage mainPage = new FavoritesPage();
            MainFrame.Navigate(mainPage);
        }
        
        private void DisplayMap (object sender, System.EventArgs e)
        {
            MapPage mainPage = new MapPage();
            MainFrame.Navigate(mainPage);
        }
    }
}