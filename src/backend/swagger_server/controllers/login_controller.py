import connexion
import six

from swagger_server.models.user_body import UserBody  # noqa: E501
from swagger_server import util
from swagger_server.__main__ import supabase  # Supabase-Client importieren
import bcrypt
from swagger_server.controllers.abrufen_controller import get_user_id


def user_login_post(body):  # noqa: E501
    """Benutzer logt sich ein

     # noqa: E501

    :param body: 
    :type body: dict | bytes

    :rtype: None
    """
    if connexion.request.is_json:
        body = UserBody.from_dict(connexion.request.get_json())  # noqa: E501


    result = supabase.table("User").select("*").eq("email", body.email).execute()

    if not result.data:
        # Security: Same error for non-existent user and wrong password
        return {"message": "Ungültige Anmeldeinformationen"}, 401

    user = result.data[0]
    stored_hash = user["password"].encode('utf-8')  # Convert to bytes
    entered_password = body.passwort.encode('utf-8')

    if bcrypt.checkpw(entered_password, stored_hash):
        return {"message": "Erfolgreich angemeldet", "userID": get_user_id(body.email)[0]}, 201
    else:
        return {"message": "Ungültige Anmeldeinformationen"}, 401
    
    
