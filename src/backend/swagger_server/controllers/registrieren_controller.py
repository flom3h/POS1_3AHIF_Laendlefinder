import connexion
import six
import bcrypt

from swagger_server.models.reg_body import RegBody  # noqa: E501
from swagger_server import util
from swagger_server.__main__ import supabase  # Supabase-Client importieren
from swagger_server.controllers.abrufen_controller import get_user_id
from logger import logger

def user_reg_post(body):  # noqa: E501
    """! Erstellt einen neuen Benutzer-Account.

    @details Legt einen neuen Benutzer in der Datenbank an, sofern die E-Mail noch nicht existiert. Das Passwort wird sicher gehasht gespeichert.

    @param body Das RegBody-Objekt mit Benutzerinformationen.
    @return tuple: Nachricht, ggf. User-ID und HTTP-Statuscode.
    """

    if connexion.request.is_json:
        body = RegBody.from_dict(connexion.request.get_json())  # noqa: E501

    logger.info(f"Registrierungsversuch für E-Mail: {body.email}")
    exists = supabase.table("User").select("*").eq("email", body.email).execute()
    if exists.data:
        logger.warning(f"Registrierung fehlgeschlagen: User {body.email} existiert bereits.")
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
    logger.info(f"User {body.email} erfolgreich registriert.")    
    return {"message": "success", "email": body.email}, 201

def get_user_data(uid):  # noqa: E501
    """! Holt die Benutzerdaten basierend auf der User-ID.

    @details Gibt Vorname, Nachname und E-Mail des Benutzers zurück.

    @param uid Die ID des Benutzers.
    @return tuple: Benutzerdaten und HTTP-Statuscode.
    """
    logger.info(f"Hole Benutzerdaten für User-ID {uid}")
    user = supabase.table("User").select("firstname, lastname, email").eq("uid", uid).execute()
    if not user.data:
        logger.warning(f"Benutzer mit ID {uid} nicht gefunden.")
        return {"message": "Benutzer nicht gefunden"}, 404
    logger.info(f"Benutzerdaten für User-ID {uid} gefunden.")
    return user.data[0], 200

def update_user_data(uid, body):  # noqa: E501
    """! Aktualisiert die Benutzerdaten.
    
    @details Überschreibt die Benutzerdaten in der Datenbank mit den neuen Werten.

    @param uid Die ID des Benutzers.
    @param body Das RegBody-Objekt mit neuen Benutzerdaten.
    @return tuple: Nachricht und HTTP-Statuscode.
    """
    if connexion.request.is_json:
        body = RegBody.from_dict(connexion.request.get_json())  # noqa: E501

    logger.info(f"Aktualisiere Benutzerdaten für User-ID {uid}")
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
    logger.info(f"Benutzerdaten für User-ID {uid} aktualisiert.")
    return {"message": "Benutzerdaten aktualisiert"}, 200