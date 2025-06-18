import connexion
import six

from swagger_server.models.favoriten_body import FavoritenBody  # noqa: E501
from swagger_server import util
from flask import jsonify
from swagger_server.__main__ import supabase  # Supabase-Client importieren
from logger import logger

def favoriten_delete(uid, eid):  # noqa: E501
    """! Entfernt ein Event aus den Favoriten eines Benutzers.

    @details Löscht den Eintrag aus der Tabelle "Favourites" für den angegebenen Benutzer und das Event.

    @param uid Die ID des Benutzers, dessen Favoriten bearbeitet werden.
    @param eid Die ID des Events, das entfernt werden soll.
    @return tuple: Nachricht und HTTP-Statuscode.
    """
    
    logger.info(f"Entferne Event {eid} aus Favoriten von User {uid}")
    result = supabase.table("Favourites").delete().eq("uid", uid).eq("eid", eid).execute()
    if not result.data:
        logger.warning(f"Event {eid} war nicht in den Favoriten von User {uid}")
        return {"message": "Event ist nicht in den Favoriten"}, 404
    
    logger.info(f"Event {eid} erfolgreich aus Favoriten von User {uid} entfernt")
    return {"message": "Event erfolgreich aus den Favoriten entfernt"}, 200


def favoriten_get(uid):  # noqa: E501
    """! Gibt die Favoriten eines Benutzers zurück.

    @details Holt alle Events, die als Favoriten für den angegebenen Benutzer gespeichert sind.

    @param uid Die ID des Benutzers, dessen Favoriten abgerufen werden.
    @return tuple: Events als JSON und HTTP-Statuscode.
    """
    logger.info(f"Rufe Favoriten für User {uid} ab")
    EventIDs = supabase.table("Favourites").select("eid").eq("uid", uid).execute()
    if not EventIDs.data:
        logger.warning(f"Keine Favoriten für User {uid} gefunden")
        return {"message": "Keine Favoriten gefunden"}, 404
    event_ids = [event["eid"] for event in EventIDs.data]
    events = supabase.table("Events").select("*").in_("eid", event_ids).execute()
    if not events.data:
        logger.warning(f"Keine Events für Favoriten von User {uid} gefunden")
        return {"message": "Keine Events gefunden"}, 404
    
    logger.info(f"{len(events.data)} Favoriten für User {uid} gefunden")
    return jsonify(events.data), 200

def is_favorit(uid, eid): # noqa: E501
    """! Prüft, ob ein Event in den Favoriten eines Benutzers ist.

    @details Sucht in der Tabelle "Favourites" nach einem Eintrag für den Benutzer und das Event.

    @param uid Die ID des Benutzers.
    @param eid Die ID des Events.
    @return tuple: Nachricht und HTTP-Statuscode.
    """

    logger.info(f"Prüfe, ob Event {eid} in Favoriten von User {uid} ist")
    Favorit = supabase.table("Favourites").select("eid").eq("uid", uid).eq("eid", eid).execute()
    if not Favorit.data:
        logger.info(f"Event {eid} ist nicht in den Favoriten von User {uid}")
        return {"message": "Event ist nicht in den Favoriten"}, 404
    logger.info(f"Event {eid} ist in den Favoriten von User {uid}")
    return {"message": "Event ist in den Favoriten"}, 200

def favoriten_post(body):  # noqa: E501
    """! Fügt ein Event zu den Favoriten eines Benutzers hinzu.
    
    @details Legt einen neuen Eintrag in der Tabelle "Favourites" für den Benutzer und das Event an.

    @param body FavoritenBody mit uid und eid.
    @return tuple: Nachricht und HTTP-Statuscode.
    """
    if connexion.request.is_json:
        body = FavoritenBody.from_dict(connexion.request.get_json())  # noqa: E501

    logger.info(f"Füge Event {body.eid} zu Favoriten von User {body.uid} hinzu")
    supabase.table("Favourites").insert({"uid": body.uid, "eid": body.eid}).execute()
    logger.info(f"Event {body.eid} erfolgreich zu Favoriten von User {body.uid} hinzugefügt")
    return {"message": "Event erfolgreich zu den Favoriten hinzugefügt"}, 201
