namespace Laendlefinder.Classes;

/**
 * @class RegRequest
 * @brief Repräsentiert eine Registrierungsanfrage mit Vorname, Nachname, E-Mail und Passwort.
 */
public class RegRequest
{
    /**
     * @property firstname
     * @brief Der Vorname des Benutzers.
     */
    public string firstname { get; set; }
    /**
     * @property lastname
     * @brief Der Nachname des Benutzers.
     */
    public string lastname { get; set; }
    /**
     * @property email
     * @brief Die E-Mail-Adresse des Benutzers.
     */
    public string email { get; set; }
    /**
     * @property passwort
     * @brief Das Passwort des Benutzers.
     */
    public string passwort { get; set; }
}