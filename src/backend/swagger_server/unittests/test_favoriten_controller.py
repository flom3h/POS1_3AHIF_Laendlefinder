# coding: utf-8

from __future__ import absolute_import

from flask import json
from six import BytesIO

from swagger_server.models.favoriten_body import FavoritenBody  # noqa: E501
from swagger_server.unittests import BaseTestCase


class TestFavoritenController(BaseTestCase):
    """FavoritenController integration test stubs"""

    def test_favoriten_delete(self): # favoriten_delete noch anpassen
        """Test case for favoriten_delete
    
        Event aus den Favoriten entfernen
        """
        uid = 8
        eid = 182
        response = self.client.open(
            f'/favoriten/delete/{uid}/{eid}',
            method='DELETE')
        self.assertIn(response.status_code, [200, 404],
                      'Response body is : ' + response.data.decode('utf-8'))

    def test_favoriten_delete_invalid_uid(self):
        """Test case for favoriten_delete_invalid_uid

        Ungültige Benutzer-ID beim Entfernen aus den Favoriten
        """
        uid = "invalid"
        eid = 182
        response = self.client.open(
            f'/favoriten/{uid}/{eid}',
            method='DELETE')
        self.assertEqual(response.status_code, 404,
                         'Response body is : ' + response.data.decode('utf-8'))

    def test_favoriten_get(self):
        """Test case for favoriten_get

        Favoriten eines Benutzers abrufen
        """
        uid = 8
        response = self.client.open(
            f'/favoriten/{uid}',
            method='GET')
        self.assertIn(response.status_code, [200, 404],
                      'Response body is : ' + response.data.decode('utf-8'))

    def test_favoriten_post(self):
        """Test case for favoriten_post

        Event zu den Favoriten hinzufügen
        """
        body = {"uid": 8, "eid": 182}
        response = self.client.open(
            '/favoriten',
            method='POST',
            data=json.dumps(body),
            content_type='application/json')
        self.assertIn(response.status_code, [201, 409],
                      'Response body is : ' + response.data.decode('utf-8'))
    
    def test_favoriten_post_invalid_uid(self):
        """Test case for favoriten_post_invalid_uid

        Ungültige Benutzer-ID beim Hinzufügen zu den Favoriten
        """
        body = {"uid": "invalid", "eid": 182}
        response = self.client.open(
            '/favoriten',
            method='POST',
            data=json.dumps(body),
            content_type='application/json')
        self.assertEqual(response.status_code, 400,
                         'Response body is : ' + response.data.decode('utf-8'))

    def test_is_favorit(self):
        """Test case for is_favorit

        Prüfen, ob ein Event in den Favoriten eines Benutzers ist
        """
        uid = 8
        eid = 182
        response = self.client.open(
            f'/favoriten/{uid}/{eid}',
            method='GET')
        self.assertIn(response.status_code, [200, 404],
                      'Response body is : ' + response.data.decode('utf-8'))
        
    def test_is_favorit_not_found(self):
        """Test case for is_favorit_not_found

        Prüfen, ob ein nicht existierendes Event in den Favoriten eines Benutzers ist
        """
        uid = 8
        eid = 9999
        response = self.client.open(
            f'/favoriten/{uid}/{eid}',
            method='GET')
        self.assertIn(response.status_code, [200, 404],
                      'Response body is : ' + response.data.decode('utf-8'))


if __name__ == '__main__':
    import unittest
    unittest.main()