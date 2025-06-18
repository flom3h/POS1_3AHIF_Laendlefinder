namespace Laendlefinder.Classes;

/**
 * @class Event
 * @brief Repräsentiert ein Event mit Eigenschaften wie Name, Datum, Uhrzeit, Beschreibung, Bild, Typ und zugehörigem Ort.
 */
public class Event
{
    /**
     * @property eid
     * @brief Die eindeutige ID des Events.
     */
    public int eid { get; set; }
    /**
     * @property name
     * @brief Der Name des Events.
     */
    public string name { get; set; }
    /**
     * @property date
     * @brief Das Datum des Events.
     */
    public DateTime date { get; set; }
    /**
     * @property time
     * @brief Die Uhrzeit des Events.
     */
    public TimeSpan time { get; set; }
    /**
     * @property description
     * @brief Die Beschreibung des Events.
     */
    public string description { get; set; }
    /**
     * @property picture
     * @brief Der Pfad oder die URL zum Bild des Events.
     */
    public string picture { get; set; }
    /**
     * @property type
     * @brief Der Typ des Events (als numerischer Wert).
     */
    public int type { get; set; }
    /**
     * @property Location
     * @brief Der Ort, an dem das Event stattfindet.
     */
    public Location Location { get; set; }
}