import connexion
import six
from logger import logger
from swagger_server import util
from swagger_server.__main__ import supabase

def event_by_id_get(eid):  
    """
    @brief Holt ein einzelnes Event und die zugehörige Location anhand der Event-ID.
    @details Sucht das Event mit der angegebenen ID in der Datenbank und gibt es zusammen mit der zugehörigen Location zurück.

    @param eid Die Event-ID.
    @return tuple: Eventdaten inkl. Location und HTTP-Statuscode.
    """
    logger.info(f"Suche Event mit ID {eid}")
    event_resp = supabase.table("Events").select("eid, name, date, time, description, picture, type").eq("eid", eid).execute()
    if not event_resp.data:
        logger.warning(f"Event mit ID {eid} nicht gefunden.")
        return {"error": "Event nicht gefunden"}, 404

    event = event_resp.data[0]

    loc_resp = supabase.table("Location").select("lid, address, name, longitude, latitude, picture").eq("lid", eid).execute()
    location = loc_resp.data[0] if loc_resp.data else None

    event["Location"] = location
    logger.info(f"Event mit ID {eid} erfolgreich gefunden.")
    return event, 200



def events_get():  # noqa: E501
    """
    @brief Holt alle Events mit zugehöriger Location.
    @details Gibt eine Liste aller Events aus der Datenbank zurück, jeweils mit der zugehörigen Location.

    @return tuple: Liste aller Events inkl. Location und HTTP-Statuscode.
    """
    logger.info("Hole alle Events mit zugehöriger Location.")
    events = supabase.table("Events").select("*, Location(*)").execute()
    if not events.data:
        logger.warning("Keine Events gefunden.")
        return {"message": "Keine Events gefunden"}, 404
    logger.info(f"{len(events.data)} Events gefunden.")
    return events.data, 200


def kategorien_get():
    """
    @brief Holt alle Kategorien (Types) aus der Datenbank.
    @details Gibt alle verfügbaren Kategorien (Types) aus der Tabelle "Type" zurück.

    @return tuple: Liste der Kategorien und HTTP-Statuscode.
    """
    logger.info("Hole alle Kategorien aus der Datenbank.")

    types = supabase.table("Type").select("tid, type").execute()
    kategorien = [{"tid": t["tid"], "type": t["type"]} for t in types.data]
    logger.info(f"{len(kategorien)} Kategorien gefunden.")
    return kategorien, 200

from swagger_server.services.event_fetcher import fetch_and_store_events

def events_import_post():  # noqa: E501
    """
    @brief Importiert Events von der externen API und speichert sie in der Datenbank.
    @details Ruft Events von einer externen API ab, speichert sie in der Datenbank und gibt eine Erfolgs- oder Fehlermeldung zurück.

    @return tuple: Erfolgs- oder Fehlermeldung und HTTP-Statuscode.
    """
    logger.info("Importiere Events von der externen API.")
    try:
        events = fetch_and_store_events()
        logger.info(f"{len(events)} Events importiert.")
        return {"message": f"{len(events)} Events importiert."}, 201
    
    except Exception as e:
        logger.error(f"Fehler beim Importieren: {e}")
        return {"error": str(e)}, 500
    
def get_user_id(email):
    """
    @brief Holt die User-ID basierend auf der E-Mail-Adresse.
    @details Sucht die User-ID in der Datenbank anhand der angegebenen E-Mail-Adresse.

    @param email Die E-Mail-Adresse des Benutzers.
    @return tuple: User-ID und HTTP-Statuscode.
    """
    logger.info(f"Suche User-ID für E-Mail: {email}")

    result = supabase.table("User").select("uid").eq("email", email).execute()
    if result.data:
        logger.info(f"User-ID für {email} gefunden: {result.data[0]['uid']}")
        return result.data[0]["uid"], 200

    logger.warning(f"User mit E-Mail {email} nicht gefunden.")
    return None, 404