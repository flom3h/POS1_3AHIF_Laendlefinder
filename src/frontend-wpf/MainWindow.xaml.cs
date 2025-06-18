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
using Serilog.Core;
using Serilog;

namespace Laendlefinder
{
    /**
     * @class MainWindow
     * @brief Repräsentiert das Hauptfenster der Anwendung.
     * Stellt die Navigation zwischen verschiedenen Seiten bereit und initialisiert das Logger-System.
     */
    public partial class MainWindow : Window
    {
        public static Logger Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
            .CreateLogger();
        
        /**
         * Konstruktor für das MainWindow. Initialisiert die Komponenten, Logger und setzt die Startseite.
         */
        public MainWindow()
        {
            InitializeComponent();
            Logger.Information("Programm initialized");
            MainFrame.Navigate(new LoginPage());
            LoginPage.LoginButtonClickedNavHome += DisplayHome;
            LoginPage.LoginButtonClickedNavRegister += DisplayRegister;

            RegisterPage.LoginButtonClickedNavLogin += DisplayLogin;
            RegisterPage.LoginButtonClickedNavHome += DisplayHome;
            
            MainPage.HomeButtonClickedNavHome += DisplayHome;
            MainPage.ExploreButtonClickedNavExplore += DisplayExplore;
            MainPage.FavsButtonClickedNavFavs += DisplayFavorites;
            MainPage.MapButtonClickedNavMap += DisplayMap;
            MainPage.ProfileButtonClickedNavProfile += DisplayProfile;
            
            ExplorePage.HomeButtonClickedNavHome += DisplayHome;
            ExplorePage.ExploreButtonClickedNavExplore += DisplayExplore;
            ExplorePage.FavsButtonClickedNavFavs += DisplayFavorites;
            ExplorePage.MapButtonClickedNavMap += DisplayMap;
            ExplorePage.ProfileButtonClickedNavProfile += DisplayProfile;
            
            FavoritesPage.HomeButtonClickedNavHome += DisplayHome;
            FavoritesPage.ExploreButtonClickedNavExplore += DisplayExplore;
            FavoritesPage.FavsButtonClickedNavFavs += DisplayFavorites;
            FavoritesPage.MapButtonClickedNavMap += DisplayMap;
            FavoritesPage.ProfileButtonClickedNavProfile += DisplayProfile;
            
            MapPage.HomeButtonClickedNavHome += DisplayHome;
            MapPage.ExploreButtonClickedNavExplore += DisplayExplore;
            MapPage.FavsButtonClickedNavFavs += DisplayFavorites;
            MapPage.MapButtonClickedNavMap += DisplayMap;
            MapPage.ProfileButtonClickedNavProfile += DisplayProfile;
            
            MoreInfoPage.HomeButtonClickedNavHome += DisplayHome;
            MoreInfoPage.ExploreButtonClickedNavExplore += DisplayExplore;
            MoreInfoPage.FavsButtonClickedNavFavs += DisplayFavorites;
            MoreInfoPage.MapButtonClickedNavMap += DisplayMap;
            MoreInfoPage.ProfileButtonClickedNavProfile += DisplayProfile;
            
            ProfilePage.HomeButtonClickedNavHome += DisplayHome;
            ProfilePage.ExploreButtonClickedNavExplore += DisplayExplore;
            ProfilePage.FavsButtonClickedNavFavs += DisplayFavorites;
            ProfilePage.MapButtonClickedNavMap += DisplayMap;
            ProfilePage.ProfileButtonClickedNavProfile += DisplayProfile;
            ProfilePage.SaveChangesButtonClickedNavHome += DisplayHome;
            ProfilePage.LogOutButtonClickedNavLogin += DisplayLogin;
            
            EventMiniViewUserControl.MoreInfoButtonClickedNavMoreInfo += DisplayMoreInfo;
            
        }
        
        /**
         * Event-Handler für die Anzeige der Startseite.
         * Navigiert zur MainPage und gibt eine Log-Nachricht aus.
         * @param sender Das auslösende Objekt.
         * @param e Event-Argumente.
         */
        private void DisplayHome (object sender, System.EventArgs e)
        {
            MainPage mainPage = new MainPage();
            MainFrame.Navigate(mainPage);
            Logger.Information("MainPage aufgerufen.");
        }
        
        /**
         * Event-Handler für die Anzeige der Login-Seite.
         * Navigiert zur LoginPage und gibt eine Log-Nachricht aus.
         * @param sender Das auslösende Objekt.
         * @param e Event-Argumente.
         */
        private void DisplayLogin (object sender, System.EventArgs e)
        {
            LoginPage mainPage = new LoginPage();
            MainFrame.Navigate(mainPage);
            Logger.Information("LoginPage aufgerufen.");
        }
        
        /**
         * Event-Handler für die Anzeige der Registrierungsseite.
         * Navigiert zur RegisterPage und gibt eine Log-Nachricht aus.
         * @param sender Das auslösende Objekt.
         * @param e Event-Argumente.
         */
        private void DisplayRegister (object sender, System.EventArgs e)
        {
            RegisterPage mainPage = new RegisterPage();
            MainFrame.Navigate(mainPage);
            Logger.Information("RegisterPage aufgerufen.");
        }
        
        /**
         * Event-Handler für die Anzeige der Explore-Seite.
         * Navigiert zur ExplorePage und gibt eine Log-Nachricht aus.
         * @param sender Das auslösende Objekt.
         * @param e Event-Argumente.
         */
        private void DisplayExplore (object sender, System.EventArgs e)
        {
            ExplorePage mainPage = new ExplorePage();
            MainFrame.Navigate(mainPage);
            Logger.Information("ExplorePage aufgerufen.");
        }
        
        /**
         * Event-Handler für die Anzeige der Profilseite.
         * Navigiert zur ProfilePage und gibt eine Log-Nachricht aus.
         * @param sender Das auslösende Objekt.
         * @param e Event-Argumente.
         */
        private void DisplayProfile (object sender, System.EventArgs e)
        {
            ProfilePage mainPage = new ProfilePage();
            MainFrame.Navigate(mainPage);
            Logger.Information("ProfilePage aufgerufen.");
        }
        
        /**
         * Event-Handler für die Anzeige der Favoritenseite.
         * Navigiert zur FavoritesPage und gibt eine Log-Nachricht aus.
         * @param sender Das auslösende Objekt.
         * @param e Event-Argumente.
         */
        private void DisplayFavorites (object sender, System.EventArgs e)
        {
            FavoritesPage mainPage = new FavoritesPage();
            MainFrame.Navigate(mainPage);
            Logger.Information("FavoritesPage aufgerufen.");
        }
        
        /**
         * Event-Handler für die Anzeige der Kartenseite.
         * Navigiert zur MapPage und gibt eine Log-Nachricht aus.
         * @param sender Das auslösende Objekt.
         * @param e Event-Argumente.
         */
        private void DisplayMap (object sender, System.EventArgs e)
        {
            MapPage mainPage = new MapPage();
            MainFrame.Navigate(mainPage);
            Logger.Information("MapPage aufgerufen.");
        }
        
        /**
         * Event-Handler für die Anzeige der Detailseite eines Events.
         * Navigiert zur MoreInfoPage und gibt eine Log-Nachricht aus.
         * @param sender Das auslösende Objekt.
         * @param eid Die ID des angezeigten Events.
         */
        private void DisplayMoreInfo (object sender, int eid)
        {
            MoreInfoPage mainPage = new MoreInfoPage(eid);
            MainFrame.Navigate(mainPage);
            Logger.Information($"MoreInfoPage für Event {eid} aufgerufen.");
        }
    }
}