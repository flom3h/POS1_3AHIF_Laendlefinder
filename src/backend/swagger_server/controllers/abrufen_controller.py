import connexion
import six
import requests
import json
from datetime import datetime
from swagger_server import util
from swagger_server.__main__ import supabase

def event_by_id_get(id):  # noqa: E501
    """Details zu einem einzelnen Event abrufen

     # noqa: E501

    :param id: Die ID des Events
    :type id: int

    :rtype: None
    """
    return 'do some magic!'


def events_get(eventname=None, kategorie=None, ort=None, region=None, datum=None):  # noqa: E501
    """Events abrufen (mit optionalen Filtern)

     # noqa: E501

    :param eventname: Filtere Events nach Eventnamen
    :type eventname: str
    :param kategorie: Filtere Events nach Kategorie
    :type kategorie: str
    :param ort: Filtere Events nach Ort
    :type ort: str
    :param region: Filtere Events nach Region
    :type region: str
    :param datum: Filtere Events nach Datum (Format YYYY-MM-DD)
    :type datum: str

    :rtype: None
    """
    

def kategorien_get():  # noqa: E501
    """Kategorien abrufen von API"""

    url = "https://v-cloud.vorarlberg.travel/api/v4/concept_schemes/caa60bb5-9885-4595-ba88-3fe898ba08eb/concepts"
    params = {
        "token": "9c0bfbfad78d3b4d25bfa70b47497d9c",
        "language": "de"
    }

    try:
        
        antwort = requests.get(url, params=params)

        # Prüfen, ob die Antwort einen HTTP-Fehlercode enthält (z. B. 404, 500)
        antwort.raise_for_status()

        # Die Antwort als JSON-Daten (also als Python-Dictionary) interpretieren
        daten = antwort.json()

        # Gelöst mit chatgpt
        # Eine Liste von Kategorien erstellen: Aus jedem Eintrag im "@graph"-Feld
        # wird der Wert des Schlüssels "skos:prefLabel" entnommen, falls vorhanden.
        # Falls der Schlüssel fehlt, wird "Unbenannt" als Standardwert verwendet.
        kategorien_liste = []
        for eintrag in daten.get("@graph", []):
            bezeichnung = eintrag.get("skos:prefLabel", "Unbenannt")
            kategorien_liste.append(bezeichnung)

        return kategorien_liste

    except requests.RequestException as fehler:
        # Wenn ein Fehler bei der Anfrage auftritt, gib ein Dictionary mit der Fehlermeldung zurück
        return {"fehler": str(fehler)}
