# von flo
from swagger_server.__main__ import supabase
import requests
import json

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

def extract_event_data(event):
    types = event.get("@type", [])
    if isinstance(types, str):
        types = [types]

    is_event = "Event" in types or "dcls:Event" in types
    is_place = "Place" in types or "TouristAttraction" in types

    location = event.get("location", {})
    if isinstance(location, list):
        location = location[0] if location else {}
    address_obj = event.get("address", {}) or location.get("address", {})

    address = address_obj.get("streetAddress", "")
    nameoflocation = location.get("name", "")
    geo = location.get("geo", {})
    longitude = geo.get("longitude", "")
    latitude = geo.get("latitude", "")

    picture = (
        event.get("dcls:dynamicUrlPortrait")
        or event.get("thumbnailUrl")
        or event.get("contentUrl")
        or ""
    )

    linktoevent = event.get("url", "")
    # Save external id for reference, but do not use as DB PK
    external_id = event.get("@id", event.get("id", ""))
    nameofevent = event.get("name", "")

    start_date = ""
    time = ""
    if is_event:
        start_date = event.get("startDate", "")
        if not start_date:
            schedule = event.get("eventSchedule", [{}])[0]
            start_date = schedule.get("startDate", "")
        date = start_date[:10] if start_date else ""
        time = start_date[11:16] if start_date else ""
    else:
        date = ""

    description = event.get("description", "")

    typeofevent = ", ".join(types) if isinstance(types, list) else str(types)

    return {
        "address": address,
        "nameoflocation": nameoflocation,
        "longitude": longitude,
        "latitude": latitude,
        "picture": picture,
        "linktoevent": linktoevent,
        "external_id": external_id,  # Only for reference, not as PK
        "nameofevent": nameofevent,
        "date": date,
        "time": time,
        "description": description,
        "typeofevent": typeofevent
    }

def upload_to_database(events):
    for event in events:
        # 1. Insert or get Type
        type_name = event.get("typeofevent", "Unbekannt")
        type_resp = supabase.table("Type").select("tid").eq("type", type_name).execute()
        if type_resp.data:
            type_id = type_resp.data[0]["tid"]
        else:
            type_insert = supabase.table("Type").insert({"type": type_name}).execute()
            type_id = type_insert.data[0]["tid"]

        event_data = {
            "name": event.get("nameofevent", ""),
            "date": event.get("date", None),
            "time": event.get("time", None),
            "description": event.get("description", ""),
            "link": event.get("linktoevent", ""),
            "picture": event.get("picture", ""),
            "type": type_id
            # "external_id": event.get("external_id", None)
        }
        event_insert = supabase.table("Events").insert(event_data).execute()
        eid = event_insert.data[0]["eid"]

        # 3. Insert Location (lid = eid)
        location_data = {
            "lid": eid,
            "address": event.get("address", ""),
            "name": event.get("nameoflocation", ""),
            "longitude": float(event.get("longitude", 0) or 0),
            "latitude": float(event.get("latitude", 0) or 0),
            "picture": event.get("picture", ""),
            "link": event.get("linktoevent", "")
        }
        supabase.table("Location").insert(location_data).execute()

def fetch_and_store_events():
    response = requests.get(url, params=params)
    data = json.loads(response.text)
    events = data.get("@graph", [])
    extracted = [extract_event_data(event) for event in events]
    # Upload to database (implement this function as needed)
    upload_to_database(extracted)
    return extracted

# Example usage (remove or adapt for production)
if __name__ == "__main__":
    events = fetch_and_store_events()
    print(json.dumps(events, indent=2))