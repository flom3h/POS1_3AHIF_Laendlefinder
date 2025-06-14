import connexion
import six

from swagger_server.models.favoriten_body import FavoritenBody  # noqa: E501
from swagger_server import util
from flask import jsonify
from swagger_server.__main__ import supabase  # Supabase-Client importieren


def favoriten_delete(uid, eid):  # noqa: E501
    """Event aus den Favoriten entfernen

     # noqa: E501

    :param uid: Die ID des Benutzers, dessen Favoriten bearbeitet werden
    :type uid: int
    :param eid: Die ID des Events, das entfernt werden soll
    :type eid: int

    :rtype: None
    """
    supabase.table("Favourites").delete().eq("uid", uid).eq("eid", eid).execute()
    return {"message": "Event erfolgreich aus den Favoriten entfernt"}, 200


def favoriten_get(uid):  # noqa: E501
    """Favoriten eines Benutzers abrufen

     # noqa: E501

    :param uid: Die ID des Benutzers, dessen Favoriten abgerufen werden
    :type uid: int

    :rtype: None
    """
    EventIDs = supabase.table("Favourites").select("eid").eq("uid", uid).execute()
    if not EventIDs.data:
        return {"message": "Keine Favoriten gefunden"}, 404
    event_ids = [event["eid"] for event in EventIDs.data]
    events = supabase.table("Events").select("*").in_("eid", event_ids).execute()
    if not events.data:
        return {"message": "Keine Events gefunden"}, 404
    return jsonify(events.data), 200

def is_favorit(uid, eid): # noqa: E501
    """Prüfen, ob ein Event in den Favoriten eines Benutzers ist

     # noqa: E501
    :param uid: Die ID des Benutzers, dessen Favoriten geprüft werden
    :type uid: int
    :param eid: Die ID des Events, das geprüft werden soll
    :type eid: int
    :rtype: None
    """
    Favorit = supabase.table("Favourites").select("eid").eq("uid", uid).eq("eid", eid).execute()
    if not Favorit.data:
        return {"message": "Event ist nicht in den Favoriten"}, 404
    return {"message": "Event ist in den Favoriten"}, 200

def favoriten_post(body):  # noqa: E501
    """Event zu den Favoriten hinzufügen

     # noqa: E501

    :param body: 
    :type body: dict | bytes

    :rtype: None
    """
    if connexion.request.is_json:
        body = FavoritenBody.from_dict(connexion.request.get_json())  # noqa: E501
    supabase.table("Favourites").insert({"uid": body.uid, "eid": body.eid}).execute()
    return {"message": "Event erfolgreich zu den Favoriten hinzugefügt"}, 201
