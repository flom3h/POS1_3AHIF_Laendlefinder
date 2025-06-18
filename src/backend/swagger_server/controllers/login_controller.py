import connexion
import six
import bcrypt

from swagger_server.models.user_body import UserBody  # noqa: E501
from swagger_server import util
from swagger_server.__main__ import supabase  # Supabase-Client importieren
from swagger_server.controllers.abrufen_controller import get_user_id
from logger import logger

def user_login_post(body):  # noqa: E501
    """
    @brief Authentifiziert einen Benutzer anhand seiner E-Mail und seines Passworts.
    @details Prüft, ob die E-Mail existiert und das Passwort korrekt ist. Gibt bei Erfolg die User-ID zurück.

    @param body Das UserBody-Objekt mit E-Mail und Passwort.
    @return tuple: Nachricht, ggf. User-ID und HTTP-Statuscode.
    """
    if connexion.request.is_json:
        body = UserBody.from_dict(connexion.request.get_json())  # noqa: E501

    logger.info(f"Login-Versuch für E-Mail: {body.email}")

    result = supabase.table("User").select("*").eq("email", body.email).execute()

    if not result.data:
        logger.warning(f"Login fehlgeschlagen: E-Mail {body.email} nicht gefunden.")

        # Security: Same error for non-existent user and wrong password
        return {"message": "Ungültige Anmeldeinformationen"}, 401

    user = result.data[0]
    stored_hash = user["password"].encode('utf-8')  # Convert to bytes
    entered_password = body.passwort.encode('utf-8')

    if bcrypt.checkpw(entered_password, stored_hash):
        logger.info(f"Login erfolgreich für E-Mail: {body.email}")
        return {"message": "Erfolgreich angemeldet", "userID": get_user_id(body.email)[0]}, 201
    else:
        logger.warning(f"Login fehlgeschlagen: Falsches Passwort für E-Mail {body.email}")
        return {"message": "Ungültige Anmeldeinformationen"}, 401
    
    
