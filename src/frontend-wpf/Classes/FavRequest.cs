namespace Laendlefinder.Classes;

/**
 * @class FavRequest
 * @brief Repräsentiert eine Anfrage, um ein Event als Favorit für einen Benutzer zu markieren.
 */
public class FavRequest
{
    /**
     * @property uid
     * @brief Die Benutzer-ID.
     */
    public int uid { get; set; }
    /**
     * @property eid
     * @brief Die Event-ID.
     */
    public int eid { get; set; }
}