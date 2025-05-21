import connexion
import six

from swagger_server.models.user_body import UserBody  # noqa: E501
from swagger_server import util


def user_login_post(body):  # noqa: E501
    """Benutzer logt sich ein

     # noqa: E501

    :param body: 
    :type body: dict | bytes

    :rtype: None
    """
    if connexion.request.is_json:
        body = UserBody.from_dict(connexion.request.get_json())  # noqa: E501
    return 'do some magic!'
