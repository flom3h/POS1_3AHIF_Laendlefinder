using System.Windows.Controls;

namespace Laendlefinder.Pages;

public partial class MainPage : Page
{
    public static event EventHandler LoginButtonClickedNavHome;

    public MainPage()
    {
        InitializeComponent();
    }
}