# coding: utf-8

from __future__ import absolute_import

from flask import json
from six import BytesIO

from swagger_server.models.favoriten_body import FavoritenBody  # noqa: E501
from swagger_server.test import BaseTestCase


class TestFavoritenController(BaseTestCase):
    """FavoritenController integration test stubs"""

    def test_favoriten_delete(self):
        """Test case for favoriten_delete

        Event aus den Favoriten entfernen
        """
        query_string = [('benutzername', 'benutzername_example'),
                        ('event_id', 56)]
        response = self.client.open(
            '/favoriten',
            method='DELETE',
            query_string=query_string)
        self.assert200(response,
                       'Response body is : ' + response.data.decode('utf-8'))

    def test_favoriten_get(self):
        """Test case for favoriten_get

        Favoriten eines Benutzers abrufen
        """
        query_string = [('benutzername', 'benutzername_example')]
        response = self.client.open(
            '/favoriten',
            method='GET',
            query_string=query_string)
        self.assert200(response,
                       'Response body is : ' + response.data.decode('utf-8'))

    def test_favoriten_post(self):
        """Test case for favoriten_post

        Event zu den Favoriten hinzufügen
        """
        body = FavoritenBody()
        response = self.client.open(
            '/favoriten',
            method='POST',
            data=json.dumps(body),
            content_type='application/json')
        self.assert200(response,
                       'Response body is : ' + response.data.decode('utf-8'))


if __name__ == '__main__':
    import unittest
    unittest.main()
