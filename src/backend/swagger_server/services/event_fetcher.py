# von flo
from swagger_server.__main__ import supabase
import requests
import json
from logger import logger


url = "https://v-cloud.vorarlberg.travel/api/v4/endpoints/b24513ef-acbb-4d9b-8cdc-eda44787baee?token=aed0e815dc2374d59cfc2e9f397a8653"
params = {
    "page": {
        "number": 1,
        "size": 24
    },
    "include": "image,description,address,location,dc:classification,eventSchedule,dc:additionalInformation.dc:classification.skos:inScheme,organizer,potentialAction,aggregateRating,amenityFeature,subjectOf,subjectOf.contentBlock,subjectOf.about,containsPlace,subjectOf.dcls:video",
    "sort": "proximity.occurrence",
    "section": {
        "@context": 0,
        "meta": 1
    },
    "filter": {
        "schedule": {
            "in": {
                "min": "2025-07-03T22:00:00.000Z"
            }
        },
        "search": ""
    },
    "language": "de"
}
logger.info("Fetching Events von der Vorarlberg API")

def extract_event_data(event):
    """
    Extrahiert relevante Eventdaten aus einem Event-Objekt.

    Args:
        event (dict): Das Event-Objekt aus der API.

    Returns:
        dict or None: Extrahierte Eventdaten oder None, falls keine Geo-Daten vorhanden sind.
    """
    logger.debug("Extrahiere Eventdaten für Event: %s", event.get("name", "Unbekannt"))

    types = event.get("@type", [])
    if isinstance(types, str):
        types = [types]

    location = event.get("location", {})
    if isinstance(location, list):
        location = location[0] if location else {}
    address_obj = event.get("address", {}) or location.get("address", {})

    address = address_obj.get("streetAddress", "")
    nameoflocation = location.get("name", "")
    geo = location.get("geo", {})
    if not geo:
        logger.warning("Kein Geo-Feld für Event: %s", event.get("name", "Unbekannt"))
        return None
    
    longitude = geo.get("longitude", "")
    latitude = geo.get("latitude", "")

    picture = (
        event.get("dcls:dynamicUrlPortrait")
        or event.get("thumbnailUrl")
        or event.get("contentUrl")
        or (event.get("image", [{}])[0].get("contentUrl") if isinstance(event.get("image", []), list) and event.get("image") else "")
        or "no data"
    )
    if not picture:
        picture = "no data"

    nameofevent = event.get("name", "")

    # Always try to get the actual event start date
    start_date = event.get("startDate", "")
    if not start_date:
        schedule = event.get("eventSchedule", [{}])
        if schedule and isinstance(schedule, list):
            start_date = schedule[0].get("startDate", "")
    date = start_date[:10] if start_date else "no data"
    time = start_date[11:16] if start_date else "no data"

    description = event.get("description", "")
    if not description:
        description = "no data"

    classifications = event.get("dc:classification", [])
    typeofevent = "Unbekannt"
    if classifications and isinstance(classifications, list):
        pref_labels = [c["skos:prefLabel"] for c in classifications if "skos:prefLabel" in c]
        if len(pref_labels) > 1:
            typeofevent = pref_labels[1]
        elif pref_labels:
            typeofevent = pref_labels[0]
        #print("typeofevent:", typeofevent)
        logger.debug("Typ des Events erkannt: %s", typeofevent)


    return {
        "address": address,
        "nameoflocation": nameoflocation,
        "longitude": longitude,
        "latitude": latitude,
        "picture": picture,
        "nameofevent": nameofevent,
        "date": date,
        "time": time,
        "description": description,
        "typeofevent": typeofevent
    }

def upload_to_database(events):
    """
    Lädt extrahierte Events in die Supabase-Datenbank.

    Args:
        events (list): Liste von Event-Dictionaries.

    Returns:
        None
    """
    logger.info("Starte Upload von %d Events in die Datenbank.", len(events))

    for event in events:
        existing = supabase.table("Events") \
            .select("eid") \
            .eq("name", event.get("nameofevent", "")) \
            .execute()
        if existing.data:
            logger.info("Event '%s' existiert bereits. Überspringe.", event.get("nameofevent", ""))
            continue  # Skip this event

        # 1. Insert or get Type
        type_name = event.get("typeofevent", "Unbekannt")
        type_resp = supabase.table("Type").select("tid").eq("type", type_name).execute()
        if type_resp.data:
            type_id = type_resp.data[0]["tid"]
            logger.debug("Typ '%s' existiert bereits mit ID %s.", type_name, type_id)

        else:
            type_insert = supabase.table("Type").insert({"type": type_name}).execute()
            type_id = type_insert.data[0]["tid"]
            logger.info("Neuer Typ '%s' mit ID %s angelegt.", type_name, type_id)

        event_data = {
            "name": event.get("nameofevent", ""),
            "date": event.get("date", None),
            "time": event.get("time", None),
            "description": event.get("description", ""),
            "picture": event.get("picture", ""),
            "type": type_id,
        }
        event_insert = supabase.table("Events").insert(event_data).execute()
        eid = event_insert.data[0]["eid"]
        logger.info("Event '%s' mit ID %s gespeichert.", event.get("nameofevent", ""), eid)


        # 3. Insert Location (lid = eid)
        location_data = {
            "lid": eid,
            "address": event.get("address", ""),
            "name": event.get("nameoflocation", ""),
            "longitude": float(event.get("longitude", 0) or 0),
            "latitude": float(event.get("latitude", 0) or 0),
            "picture": event.get("picture", ""),
        }
        supabase.table("Location").insert(location_data).execute()
        logger.info("Location für Event '%s' gespeichert.", event.get("nameofevent", ""))


def fetch_and_store_events():
    """
    Holt Events von der Vorarlberg API, extrahiert und speichert sie in der Datenbank.

    Returns:
        list: Liste der extrahierten Events.
    """
    logger.info("Starte das Abrufen und Speichern von Events.")
    response = requests.get(url, params=params)
    data = json.loads(response.text)
    events = data.get("@graph", [])
    logger.info("Es wurden %d Events von der API geladen.", len(events))
    # Only keep events with geo and proper data
    extracted = [e for e in (extract_event_data(event) for event in events) if e is not None]
    upload_to_database(extracted)
    logger.info("%d Events verarbeitet und gespeichert.", len(extracted))
    return extracted

# Example usage (remove or adapt for production)
if __name__ == "__main__":
    """
    Hauptfunktion zum Testen: Ruft Events ab.
    Im JSON wird sie ausgegeben
    """
    events = fetch_and_store_events()
    print(json.dumps(events, indent=2))