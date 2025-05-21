import connexion
import six

from swagger_server.models.favoriten_body import FavoritenBody  # noqa: E501
from swagger_server import util


def favoriten_delete(benutzername, event_id):  # noqa: E501
    """Event aus den Favoriten entfernen

     # noqa: E501

    :param benutzername: Der Benutzer, dessen Favoriten bearbeitet werden
    :type benutzername: str
    :param event_id: Die ID des Events, das entfernt werden soll
    :type event_id: int

    :rtype: None
    """
    return 'do some magic!'


def favoriten_get(benutzername):  # noqa: E501
    """Favoriten eines Benutzers abrufen

     # noqa: E501

    :param benutzername: Der Benutzer, dessen Favoriten abgerufen werden
    :type benutzername: str

    :rtype: None
    """
    return 'do some magic!'


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
