import connexion
import six

from swagger_server import util
from swagger_server.__main__ import supabase

def event_by_id_get(eid):  
    event_resp = supabase.table("Events").select("eid, name, date, time, description, picture, type").eq("eid", eid).single().execute()
    if not event_resp.data:
        return {"error": "Event nicht gefunden"}, 404

    event = event_resp.data

    loc_resp = supabase.table("Location").select("lid, address, name, longitude, latitude, picture").eq("lid", eid).single().execute()
    location = loc_resp.data if loc_resp.data else None

    event["Location"] = location
    return event, 200



def events_get(eventname=None, kategorie=None, ort=None, region=None, datum=None):  # noqa: E501
    """Alle Events mit zugehöriger Location abrufen

    :rtype: None
    """
    # Holt alle Events und die zugehörige Location (1:1 über gleiche ID)
    events = supabase.table("Events").select("*, Location(*)").execute()
    if not events.data:
        return {"message": "Keine Events gefunden"}, 404
    return events.data, 200

    #query = supabase.table("Events").select("eid, name, date, time, description, picture, type")
    #
    #if eventname:
    #    query = query.ilike("name", f"%{eventname}%")
    #if datum:
    #    query = query.eq("date", datum)
    #if kategorie:
    #    # Hole type_id aus Type-Tabelle
    #    type_resp = supabase.table("Type").select("tid").eq("type", kategorie).execute()
    #    if type_resp.data:
    #        type_id = type_resp.data[0]["tid"]
    #        query = query.eq("type", type_id)
    #    else:
    #        return []
    ## Für ort/region: Hole alle passenden Event-IDs aus Location und filtere dann
    #if ort or region:
    #    loc_query = supabase.table("Location").select("lid, address, name, longitude, latitude, picture")
    #    if ort:
    #        loc_query = loc_query.ilike("address", f"%{ort}%")
    #    if region:
    #        loc_query = loc_query.ilike("address", f"%{region}%")
    #    loc_resp = loc_query.execute()
    #    lids = [loc["lid"] for loc in loc_resp.data]
    #    if lids:
    #        query = query.in_("eid", lids)
    #    else:
    #        return []
    #
    #response = query.execute()
    #return response.data
    


def kategorien_get():  # noqa: E501
    """Kategorien abrufen von API

     # noqa: E501


    :rtype: None
    """
    return 'do some magic!'

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