import connexion
import six
import bcrypt

from swagger_server.models.reg_body import RegBody  # noqa: E501
from swagger_server import util
from swagger_server.__main__ import supabase  # Supabase-Client importieren

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

    body.passwort = body.passwort.encode('utf-8')
    salt = bcrypt.gensalt()
    body.passwort = bcrypt.hashpw(body.passwort, salt)
    body.passwort = bcrypt.hashpw(body.passwort, salt).decode('utf-8')  # Bytes → String

    data = {
        "firstname": body.firstname,
        "lastname": body.lastname,
        "email": body.email,
        "password": body.passwort,
        "isAdmin": False
    }
    supabase.table("User").insert(data).execute()
    return {"message": "success"}, 201