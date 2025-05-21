import connexion
import six

from swagger_server.models.reg_body import RegBody  # noqa: E501
from swagger_server import util


def user_reg_post(body):  # noqa: E501
    """Benutzer erstellt seinen Account

     # noqa: E501

    :param body: 
    :type body: dict | bytes

    :rtype: None
    """
    if connexion.request.is_json:
        body = RegBody.from_dict(connexion.request.get_json())  # noqa: E501
    return 'do some magic!'
