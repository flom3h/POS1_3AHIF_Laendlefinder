using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows.Controls;
using Laendlefinder.UserControlls;
using System.Linq;
using Laendlefinder.Classes;

namespace Laendlefinder.Collections;

/**
 * Durchsucht alle Events nach dem angegebenen Suchbegriff und gibt passende Events zurück.
 *
 * Die Suche erfolgt in Name, Ort, Adresse, Beschreibung und Kategorie des Events.
 *
 * @param searchTerm Der Suchbegriff, nach dem gefiltert werden soll.
 * @return Eine Liste der gefundenen Events.
 */
public class EventCollection : ObservableCollection<Event>
{
    public static EventCollection Events { get; } = new EventCollection();
    public List<Event> events { get; } = new List<Event>();
    
    /**
     * Serialisiert die Events in eine JSON-Datei.
     *
     * @param filename Der Name der Datei, in die die Events serialisiert werden.
     */
    public void Serialize(string filename)
    {
        using (StreamWriter stream = new StreamWriter(filename))
        {
            foreach (var ev in this)
            {
                string jsonStr = JsonSerializer.Serialize(ev);
                stream.WriteLine(jsonStr);
            }
            MainWindow.Logger.Information("Events serialisiert und in JSON-Datei gespeichert: " + filename);
        }
    }
    
    /**
     * Zeichnet alle Events in das angegebene Panel.
     *
     * @param panel Das Panel, in das die Events gezeichnet werden.
     */
    public void Draw(Panel panel)
    {
        panel.Children.Clear();
        foreach (var ev in this)
        {
            var eventControl = new EventMiniViewUserControl();
            eventControl.SetEventData(ev);
            panel.Children.Add(eventControl);
        }
        MainWindow.Logger.Information("Events gezeichnet und in Panel hinzugefügt.");
    }

    /// <summary>
    /// Sucht Events, die den Suchbegriff enthalten, in Name, Ort, Adresse, Beschreibung oder Kategorie.
    /// </summary>
    /// <param name="searchTerm">Der Suchbegriff.</param>
    /// <returns>Gefundene Events als EventCollection.</returns>
    public EventCollection Search(string searchTerm)
    {
        var types = EventMiniViewUserControl.Types;
        var results = new EventCollection();
        foreach (var ev in this)
        {
            if (ev == null)
            {
                MainWindow.Logger.Warning("Ein Event in der Sammlung ist null, wird uebersprungen.");
                continue;
            }
            bool matchFound = false;
            /*
             * Überprüft, ob der Eventname den Suchbegriff enthält.
             * @see Event.name
             */
            if (ev.name != null && ev.name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            {
                matchFound = true;
                MainWindow.Logger.Information($"Event durch Name gefunden: {ev.name}");
            }
            /*!
             * Überprüft, ob der Ortsname den Suchbegriff enthält.
             * \see Location.name
             */
            else if (ev.Location?.name != null && ev.Location.name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            {
                matchFound = true;
                MainWindow.Logger.Information($"Event durch Ort gefunden: {ev.Location.name}");
            }
            /// Überprüft, ob die Adresse den Suchbegriff enthält.
            else if (ev.Location?.address != null && ev.Location.address.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            {
                matchFound = true;
                MainWindow.Logger.Information($"Event durch Adresse gefunden: {ev.Location.address}");
            }
            /// Überprüft, ob die Beschreibung den Suchbegriff enthält.
            else if (ev.description != null && ev.description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            {
                matchFound = true;
                MainWindow.Logger.Information($"Event durch Beschreibung gefunden: {ev.description}");
            }
            /**
             * Überprüft, ob die Kategorie (Typ) des Events den Suchbegriff enthält.
             * @see Type.type
             */
            else if (ev.type != 0)
            {
                var type = types.FirstOrDefault(t => t.tid == ev.type);
                if (type != null && type.type.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                {
                    matchFound = true;
                    MainWindow.Logger.Information($"Event durch Kategorie gefunden: {type.type}");
                }
            }
            if (matchFound)
            {
                results.Add(ev);
                MainWindow.Logger.Information($"Event hinzugefügt: {ev.name} (EID: {ev.eid})");
            }
        }
        return results;
    }

    /*! \brief Filtert Events nach einem bestimmten Datum.
     *
     * \param date Das gewünschte Datum.
     * \return Events, die an diesem Datum stattfinden.
     */
    public EventCollection FilterByDate(DateTime date)
    {
        var results = new EventCollection();
        foreach (var ev in this)
        {
            if (ev == null || ev.date == null)
            {
                MainWindow.Logger.Warning("Ein Event in der Sammlung ist null oder hat kein Datum, wird uebersprungen.");
                continue;
            }
            if (ev.date.Date == date.Date)
            {
                MainWindow.Logger.Information($"Event am {date.ToShortDateString()} gefunden: {ev.name}");
                results.Add(ev);
            }
        }
        MainWindow.Logger.Information($"Gefundene Events am {date.ToShortDateString()}: {results.Count} Events");
        return results;
    }

    /// <summary>
    /// Filtert Events nach einem Datumsbereich.
    /// </summary>
    /// <param name="startDate">Startdatum.</param>
    /// <param name="endDate">Enddatum.</param>
    /// <returns>Events im angegebenen Zeitraum.</returns>
    public EventCollection FilterByDateRange(DateTime startDate, DateTime endDate)
    {
        var results = new EventCollection();
        foreach (var ev in this)
        {
            if (ev == null || ev.date == null)
            {
                MainWindow.Logger.Warning("Ein Event in der Sammlung ist null oder hat kein Datum, wird uebersprungen.");
                continue;
            }
            if (ev.date.Date >= startDate.Date && ev.date.Date <= endDate.Date)
            {
                results.Add(ev);
                MainWindow.Logger.Information($"Event im Zeitraum {startDate.ToShortDateString()} - {endDate.ToShortDateString()} gefunden: {ev.name}");
            }
        }
        MainWindow.Logger.Information($"Gefundene Events im Zeitraum {startDate.ToShortDateString()} - {endDate.ToShortDateString()}: {results.Count} Events");
        return results;
    }
}