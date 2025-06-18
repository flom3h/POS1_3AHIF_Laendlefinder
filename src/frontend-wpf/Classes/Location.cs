namespace Laendlefinder.Classes;

/**
 * @class Location
 * @brief Repräsentiert einen Ort mit Name, Adresse, Koordinaten und Bild.
 */
public class Location
{
    /**
     * @property lid
     * @brief Die eindeutige ID des Ortes.
     */
    public int lid { get; set; }
    /**
     * @property name
     * @brief Der Name des Ortes.
     */
    public string name { get; set; }
    /**
     * @property address
     * @brief Die Adresse des Ortes.
     */
    public string address { get; set; }
    /**
     * @property latitude
     * @brief Der Breitengrad des Ortes.
     */
    public double latitude { get; set; }
    /**
     * @property longitude
     * @brief Der Längengrad des Ortes.
     */
    public double longitude { get; set; }
    /**
     * @property picture
     * @brief Der Pfad oder die URL zum Bild des Ortes.
     */
    public string picture { get; set; }
}