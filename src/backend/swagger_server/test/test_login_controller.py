# coding: utf-8

from __future__ import absolute_import

from flask import json
from six import BytesIO

from swagger_server.models.user_body import UserBody  # noqa: E501
from swagger_server.test import BaseTestCase


class TestLoginController(BaseTestCase):
    """LoginController integration test stubs"""

    def test_user_login_post(self):
        """Test case for user_login_post

        Benutzer logt sich ein
        """
        body = UserBody()
        response = self.client.open(
            '/login',
            method='POST',
            data=json.dumps(body),
            content_type='application/json')
        self.assert200(response,
                       'Response body is : ' + response.data.decode('utf-8'))


if __name__ == '__main__':
    import unittest
    unittest.main()
