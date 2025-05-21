# coding: utf-8

from __future__ import absolute_import

from flask import json
from six import BytesIO

from swagger_server.models.reg_body import RegBody  # noqa: E501
from swagger_server.test import BaseTestCase


class TestRegistrierenController(BaseTestCase):
    """RegistrierenController integration test stubs"""

    def test_user_reg_post(self):
        """Test case for user_reg_post

        Benutzer erstellt seinen Account
        """
        body = RegBody()
        response = self.client.open(
            '/registrieren',
            method='POST',
            data=json.dumps(body),
            content_type='application/json')
        self.assert200(response,
                       'Response body is : ' + response.data.decode('utf-8'))


if __name__ == '__main__':
    import unittest
    unittest.main()
