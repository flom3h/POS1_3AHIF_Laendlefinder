using System.Windows;

namespace Laendlefinder.Pages;

public partial class MiniCalendarWindow : Window
{
    public DateTime SelectedDate { get; private set; }
    
    public MiniCalendarWindow()
    {
        InitializeComponent();
    }

    private void OkButton_Clicked(object sender, RoutedEventArgs e)
    {
        SelectedDate = CalendarControl.SelectedDate ?? DateTime.Now;
        DialogResult = true;
        Close();
        MainWindow.Logger.Information($"MiniCalendar geschlossen und ausgewähltes Datum: {SelectedDate.ToShortDateString()}");
    }
}