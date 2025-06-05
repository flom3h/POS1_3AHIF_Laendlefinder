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
<<<<<<< Updated upstream
            MainFrame.Navigate(new RegisterPage());
=======
            MainFrame.Navigate(new CalendarPage());
>>>>>>> Stashed changes
            LoginPage.LoginButtonClickedNavHome += DisplayHome;
            LoginPage.LoginButtonClickedNavRegister += DisplayRegister;

            RegisterPage.LoginButtonClickedNavLogin += DisplayLogin;
            RegisterPage.LoginButtonClickedNavHome += DisplayHome;
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
    }
}