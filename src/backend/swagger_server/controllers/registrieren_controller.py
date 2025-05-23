import connexion
import six

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

    data = {
        "firstname": body.firstname,
        "lastname": body.lastname,
        "email": body.email,
        "password": body.passwort
    }
    supabase.table("users").insert(data).execute()
    return {"status": "success"}, 201