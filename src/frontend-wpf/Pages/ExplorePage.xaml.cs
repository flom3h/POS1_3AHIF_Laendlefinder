using System.Windows;
using System.Windows.Controls;
using Laendlefinder.Collections;
using Serilog.Core;

namespace Laendlefinder.Pages;

/**
 * @class ExplorePage
 * @brief Repräsentiert die Explore-Seite der Anwendung.
 * Zeigt eine Liste von Events an und ermöglicht die Navigation zu anderen Seiten.
 */
public partial class ExplorePage : Page
{
    private EventCollection eventCollection = MainPage.eventCollection;
    private EventCollection filteredCollection = new();
    public static event EventHandler HomeButtonClickedNavHome;
    public static event EventHandler ExploreButtonClickedNavExplore;
    public static event EventHandler FavsButtonClickedNavFavs;
    public static event EventHandler MapButtonClickedNavMap;
    public static event EventHandler ProfileButtonClickedNavProfile;

    /**
    * Konstruktor für die ExplorePage. Initialisiert die Komponenten und zeichnet die Events.
    */
    public ExplorePage()
    {
        InitializeComponent();
        eventCollection.Draw(EventsPanel);
        MainWindow.Logger.Information("ExplorePage initialized");
    }

    /**
    * Event-Handler für den Home-Button. Löst das HomeButtonClickedNavHome-Event aus.
    * Leitet zur HomePage weiter.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void HomeButton_Click(object sender, RoutedEventArgs e)
    {
        MainWindow.Logger.Information("Home-Button wurde geklickt, Navigation zur HomePage.");
        HomeButtonClickedNavHome?.Invoke(this, EventArgs.Empty);
    }

    /**
    * Event-Handler für den Explore-Button. Löst das ExploreButtonClickedNavExplore-Event aus.
    * Leitet zur ExplorePage weiter.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void ExploreButton_Click(object sender, RoutedEventArgs e)
    {
        MainWindow.Logger.Information("Explore-Button wurde geklickt, Navigation zur ExplorePage.");
        ExploreButtonClickedNavExplore?.Invoke(this, EventArgs.Empty);
    }

    /**
    * Event-Handler für den Favs-Button. Löst das FavsButtonClickedNavFavs-Event aus.
    * Leitet zur FavsPage weiter.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void FavsButton_Click(object sender, RoutedEventArgs e)
    {
        MainWindow.Logger.Information("Favs-Button wurde geklickt, Navigation zur FavsPage.");
        FavsButtonClickedNavFavs?.Invoke(this, EventArgs.Empty);
    }

    /**
    * Event-Handler für den Map-Button. Löst das MapButtonClickedNavMap-Event aus.
    * Leitet zur MapPage weiter.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void MapButton_Click(object sender, RoutedEventArgs e)
    {
        MainWindow.Logger.Information("Map-Button wurde geklickt, Navigation zur MapPage.");
        MapButtonClickedNavMap?.Invoke(this, EventArgs.Empty);
    }

    /**
    * Event-Handler für den Profile-Button. Löst das ProfileButtonClickedNavProfile-Event aus.
    * Leitet zur ProfilePage weiter.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void ProfileButton_Click(object sender, RoutedEventArgs e)
    {
        MainWindow.Logger.Information("Profile-Button wurde geklickt, Navigation zur ProfilePage.");
        ProfileButtonClickedNavProfile?.Invoke(this, EventArgs.Empty);
    }

    /**
    * Event-Handler für die Auswahl eines RadioButtons zur Datumsfilterung.
    * Filtert die Events je nach Auswahl und zeigt sie an.
    * @param sender Das auslösende Objekt.
    * @param e Event-Argumente.
    */
    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
        var selectedRadio = DateFilterPanel.Children.OfType<RadioButton>().FirstOrDefault(rb => rb.IsChecked == true);

        if (selectedRadio != null)
        {
            MainWindow.Logger.Information("RadioButton für Datumsauswahl wurde ausgewählt: " + selectedRadio.Content.ToString());
            string selectedText = selectedRadio.Content.ToString();
            if (selectedText == "Heute")
            {
                filteredCollection = eventCollection.FilterByDate(DateTime.Today);
                EventsPanel.Children.Clear();
                filteredCollection.Draw(EventsPanel);
                MainWindow.Logger.Information("Events für heute gefiltert und angezeigt.");
            }
            else if (selectedText == "Morgen")
            {
                filteredCollection = eventCollection.FilterByDate(DateTime.Today.AddDays(1));
                EventsPanel.Children.Clear();
                filteredCollection.Draw(EventsPanel);
                MainWindow.Logger.Information("Events für morgen gefiltert und angezeigt.");
            }
            else if (selectedText == "Nächste 7 Tage")
            {
                var startDate = DateTime.Today;
                var endDate = startDate.AddDays(7);
                filteredCollection = eventCollection.FilterByDateRange(startDate, endDate);
                EventsPanel.Children.Clear();
                filteredCollection.Draw(EventsPanel);
                MainWindow.Logger.Information("Events für die nächsten 7 Tage gefiltert und angezeigt.");
            }
            else if (selectedText == "Datum auswählen")
            {
                var miniCalendarWindow = new MiniCalendarWindow();
                MainWindow.Logger.Information("MiniCalendar geöffnet, um ein Datum auszuwählen.");
                if (miniCalendarWindow.ShowDialog() == true)
                {
                    var selectedDate = miniCalendarWindow.SelectedDate;
                    filteredCollection = eventCollection.FilterByDate(selectedDate);
                    EventsPanel.Children.Clear();
                    filteredCollection.Draw(EventsPanel);
                    MainWindow.Logger.Information($"Events für das ausgewählte Datum {selectedDate.ToShortDateString()} gefiltert und angezeigt.");
                }
            }
        }
    }
}