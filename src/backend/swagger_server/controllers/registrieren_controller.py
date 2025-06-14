import connexion
import six
import bcrypt

from swagger_server.models.reg_body import RegBody  # noqa: E501
from swagger_server import util
from swagger_server.__main__ import supabase  # Supabase-Client importieren
from swagger_server.controllers.abrufen_controller import get_user_id

def user_reg_post(body):  # noqa: E501
    """Benutzer erstellt seinen Account

     # noqa: E501

    :param body: 
    :type body: dict | bytes

    :rtype: None
    """
    if connexion.request.is_json:
        body = RegBody.from_dict(connexion.request.get_json())  # noqa: E501

    exists = supabase.table("User").select("*").eq("email", body.email).execute()
    if exists.data:
        return {"message": "User existiert bereits"}, 400

    raw_password = body.passwort.encode('utf-8')
    salt = bcrypt.gensalt()
    hashed_password = bcrypt.hashpw(raw_password, salt)
    body.passwort = hashed_password.decode('utf-8')  
    
    data = {
        "firstname": body.firstname,
        "lastname": body.lastname,
        "email": body.email,
        "password": body.passwort,
        "isAdmin": False
    }
    supabase.table("User").insert(data).execute()
    return {"message": "success", "userID": get_user_id(body.email)[0]}, 201

def get_user_data(uid):  # noqa: E501
    """Holt die Benutzerdaten basierend auf der User-ID

    :param uid: Die ID des Benutzers
    :type uid: int

    :rtype: None
    """
    user = supabase.table("User").select("firstname, lastname, email").eq("uid", uid).execute()
    if not user.data:
        return {"message": "Benutzer nicht gefunden"}, 404
    return user.data[0], 200

def update_user_data(uid, body):  # noqa: E501
    """Aktualisiert die Benutzerdaten

    :param uid: Die ID des Benutzers
    :type uid: int
    :param body: Die neuen Benutzerdaten
    :type body: dict | bytes

    :rtype: None
    """
    if connexion.request.is_json:
        body = RegBody.from_dict(connexion.request.get_json())  # noqa: E501

    raw_password = body.passwort.encode('utf-8')
    salt = bcrypt.gensalt()
    hashed_password = bcrypt.hashpw(raw_password, salt)
    body.passwort = hashed_password.decode('utf-8')

    data = {
        "firstname": body.firstname,
        "lastname": body.lastname,
        "email": body.email,
        "password": body.passwort,
        "isAdmin": False
    }
    
    supabase.table("User").update(data).eq("uid", uid).execute()
    return {"message": "Benutzerdaten aktualisiert"}, 200