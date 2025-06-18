namespace Laendlefinder.Classes;

/**
 * @class LoginRequest
 * @brief Repräsentiert eine Login-Anfrage mit E-Mail und Passwort.
 */
public class LoginRequest
{
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