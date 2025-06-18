using System.Windows;

namespace Laendlefinder.Pages;

/**
 * @class MiniCalendarWindow
 * @brief Repräsentiert ein kleines Kalenderfenster zur Datumsauswahl.
 * Ermöglicht die Auswahl eines Datums und gibt dieses zurück.
 */
public partial class MiniCalendarWindow : Window
{
    public DateTime SelectedDate { get; private set; }
    
    /**
    * Konstruktor für das MiniCalendarWindow. Initialisiert die Komponenten und das Logging.
    */
    public MiniCalendarWindow()
    {
        InitializeComponent();
        MainWindow.Logger.Information("MiniCalendarWindow initialized");
    }

    /**
    * Event-Handler für den OK-Button.
    * Setzt das ausgewählte Datum, schließt das Fenster und gibt DialogResult zurück.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void OkButton_Clicked(object sender, RoutedEventArgs e)
    {
        SelectedDate = CalendarControl.SelectedDate ?? DateTime.Now;
        DialogResult = true;
        Close();
        MainWindow.Logger.Information($"MiniCalendar geschlossen und ausgewähltes Datum: {SelectedDate.ToShortDateString()}");
    }
}