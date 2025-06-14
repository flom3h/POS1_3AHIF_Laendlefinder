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
using Laendlefinder.UserControlls;

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
            MainFrame.Navigate(new LoginPage());
            //MainFrame.Navigate(new MainPage());
            LoginPage.LoginButtonClickedNavHome += DisplayHome;
            LoginPage.LoginButtonClickedNavRegister += DisplayRegister;

            RegisterPage.LoginButtonClickedNavLogin += DisplayLogin;
            RegisterPage.LoginButtonClickedNavHome += DisplayHome;
            
            MainPage.HomeButtonClickedNavHome += DisplayHome;
            MainPage.ExploreButtonClickedNavExplore += DisplayExplore;
            MainPage.CalendarButtonClickedNavCalendar += DisplayCalendar;
            MainPage.FavsButtonClickedNavFavs += DisplayFavorites;
            MainPage.MapButtonClickedNavMap += DisplayMap;
            MainPage.ProfileButtonClickedNavProfile += DisplayProfile;
            
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

            CalendarPage.HomeButtonClickedNavHome += DisplayHome;
            CalendarPage.ExploreButtonClickedNavExplore += DisplayExplore;
            CalendarPage.CalendarButtonClickedNavCalendar += DisplayCalendar;
            CalendarPage.FavsButtonClickedNavFavs += DisplayFavorites;
            CalendarPage.MapButtonClickedNavMap += DisplayMap;
            CalendarPage.ProfileButtonClickedNavProfile += DisplayProfile;
            
            MapPage.HomeButtonClickedNavHome += DisplayHome;
            MapPage.ExploreButtonClickedNavExplore += DisplayExplore;
            MapPage.CalendarButtonClickedNavCalendar += DisplayCalendar;
            MapPage.FavsButtonClickedNavFavs += DisplayFavorites;
            MapPage.MapButtonClickedNavMap += DisplayMap;
            MapPage.ProfileButtonClickedNavProfile += DisplayProfile;
            
            MoreInfoPage.HomeButtonClickedNavHome += DisplayHome;
            MoreInfoPage.ExploreButtonClickedNavExplore += DisplayExplore;
            MoreInfoPage.CalendarButtonClickedNavCalendar += DisplayCalendar;
            MoreInfoPage.FavsButtonClickedNavFavs += DisplayFavorites;
            MoreInfoPage.MapButtonClickedNavMap += DisplayMap;
            MoreInfoPage.ProfileButtonClickedNavProfile += DisplayProfile;
            
            ProfilePage.HomeButtonClickedNavHome += DisplayHome;
            ProfilePage.ExploreButtonClickedNavExplore += DisplayExplore;
            ProfilePage.CalendarButtonClickedNavCalendar += DisplayCalendar;
            ProfilePage.FavsButtonClickedNavFavs += DisplayFavorites;
            ProfilePage.MapButtonClickedNavMap += DisplayMap;
            ProfilePage.ProfileButtonClickedNavProfile += DisplayProfile;
            ProfilePage.SaveChangesButtonClickedNavHome += DisplayHome;
            ProfilePage.LogOutButtonClickedNavLogin += DisplayLogin;
            
            EventMiniViewUserControl.MoreInfoButtonClickedNavMoreInfo += DisplayMoreInfo;
            
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
        
        private void DisplayMoreInfo (object sender, System.EventArgs e)
        {
            MoreInfoPage mainPage = new MoreInfoPage();
            MainFrame.Navigate(mainPage);
        }
    }
}