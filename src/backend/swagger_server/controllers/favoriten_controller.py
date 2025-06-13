import connexion
import six

from swagger_server.models.favoriten_body import FavoritenBody  # noqa: E501
from swagger_server import util
from flask import jsonify
from swagger_server.__main__ import supabase  # Supabase-Client importieren


def favoriten_delete(uid, event_id):  # noqa: E501
    """Event aus den Favoriten entfernen

     # noqa: E501

    :param uid: Die ID des Benutzers, dessen Favoriten bearbeitet werden
    :type uid: int
    :param event_id: Die ID des Events, das entfernt werden soll
    :type event_id: int

    :rtype: None
    """
    return 'do some magic!'


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


def favoriten_post(body):  # noqa: E501
    """Event zu den Favoriten hinzufügen

     # noqa: E501

    :param body: 
    :type body: dict | bytes

    :rtype: None
    """
    if connexion.request.is_json:
        body = FavoritenBody.from_dict(connexion.request.get_json())  # noqa: E501
    return 'do some magic!'
