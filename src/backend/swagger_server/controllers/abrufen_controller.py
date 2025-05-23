import connexion
import six

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
    datum = util.deserialize_date(datum)
    return 'do some magic!'


def kategorien_get():  # noqa: E501
    """Kategorien abrufen von API

     # noqa: E501


    :rtype: None
    """
    return 'do some magic!'
