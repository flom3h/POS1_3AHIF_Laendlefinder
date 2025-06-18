namespace Laendlefinder.Classes;

/**
 * @class Type
 * @brief Repräsentiert einen Typ mit einer ID und einer Typbezeichnung.
 */
public class Type
{
    /**
     * @property tid
     * @brief Die eindeutige ID des Typs.
     */
    public int tid { get; set; }
    /**
     * @property type
     * @brief Die Bezeichnung des Typs.
     */
    public string type { get; set; } = string.Empty;
}