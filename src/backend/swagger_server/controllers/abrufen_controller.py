import connexion
import six

from swagger_server import util
from swagger_server.__main__ import supabase

def event_by_id_get(eid):  
    event_resp = supabase.table("Events").select("eid, name, date, time, description, picture, type").eq("eid", eid).execute()
    if not event_resp.data:
        return {"error": "Event nicht gefunden"}, 404
    event = event_resp.data[0]

    loc_resp = supabase.table("Location").select("lid, address, name, longitude, latitude, picture").eq("lid", eid).execute()
    location = loc_resp.data[0] if loc_resp.data else None

    event["Location"] = location
    return event, 200



def events_get():  # noqa: E501
    """Alle Events mit zugehöriger Location abrufen

    :rtype: None
    """
    # Holt alle Events und die zugehörige Location (1:1 über gleiche ID)
    events = supabase.table("Events").select("*, Location(*)").execute()
    if not events.data:
        return {"message": "Keine Events gefunden"}, 404
    return events.data, 200


def kategorien_get():
    """Kategorien abrufen von API"""
    # Hole alle Kategorien (Types) aus der Tabelle "Type"
    types = supabase.table("Type").select("tid, type").execute()
    kategorien = [{"tid": t["tid"], "type": t["type"]} for t in types.data]
    return kategorien, 200

from swagger_server.services.event_fetcher import fetch_and_store_events

def events_import_post():  # noqa: E501
    """Importiert Events von der externen API und speichert sie in der Datenbank

    :rtype: None
    """
    try:
        events = fetch_and_store_events()
        return {"message": f"{len(events)} Events importiert."}, 201
    except Exception as e:
        return {"error": str(e)}, 500
    
def get_user_id(email):
    """Holt die User-ID basierend auf der E-Mail-Adresse

    :param email: Die E-Mail-Adresse des Benutzers
    :type email: str

    :rtype: int
    """
    result = supabase.table("User").select("uid").eq("email", email).execute()
    if result.data:
        return result.data[0]["uid"], 200
    return None, 404