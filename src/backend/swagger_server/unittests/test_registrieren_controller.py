# coding: utf-8

from __future__ import absolute_import

from flask import json
from six import BytesIO

from swagger_server.models.reg_body import RegBody  # noqa: E501
from swagger_server.unittests import BaseTestCase
from swagger_server.__main__ import supabase  # Supabase-Client importieren


class TestRegistrierenController(BaseTestCase):
    """RegistrierenController integration test stubs"""

    def setUp(self):
        super().setUp()
        self.test_user = {
            "firstname": "Max",
            "lastname": "Mustermann",
            "email": "max.mustermann@example.com",
            "passwort": "sicherespasswort123"
        }

    def test_user_reg_post(self):
        """Benutzer erstellt seinen Account"""
        response = self.client.open(
            '/registrieren',
            method='POST',
            data=json.dumps(self.test_user),
            content_type='application/json')
        self.assertIn(response.status_code, [201, 400],
                      'Response body is : ' + response.data.decode('utf-8'))
        # Bei erfolgreicher Registrierung: User darf nicht nochmal registriert werden
        response2 = self.client.open(
            '/registrieren',
            method='POST',
            data=json.dumps(self.test_user),
            content_type='application/json')
        self.assertEqual(response2.status_code, 400)

    def test_get_user_data(self):
        """Benutzerdaten abrufen"""
        # Registriere User (falls nicht vorhanden)
        reg_response = self.client.open(
            '/registrieren',
            method='POST',
            data=json.dumps(self.test_user),
            content_type='application/json')
        # Hole UID aus Antwort, egal ob 201 oder 400
        if reg_response.status_code == 201:
            uid = reg_response.get_json().get("userID")
        else:
            # UID aus Datenbank holen, falls User schon existiert
            user = supabase.table("User").select("uid").eq("email", self.test_user["email"]).execute()
            uid = user.data[0]["uid"] if user.data else None
        self.assertIsNotNone(uid, "UID konnte nicht ermittelt werden.")
        response = self.client.open(
            f'/user/{uid}',
            method='GET')
        self.assertIn(response.status_code, [200, 404],
                      'Response body is : ' + response.data.decode('utf-8'))

    def test_get_user_data_not_found(self):
        """Abrufen eines nicht existierenden Users"""
        response = self.client.open(
            '/user/999',
            method='GET')
        self.assertEqual(response.status_code, 404)

    def test_update_user_data(self):
        """Benutzerdaten aktualisieren"""
        # Registriere User (falls nicht vorhanden)
        reg_response = self.client.open(
            '/registrieren',
            method='POST',
            data=json.dumps(self.test_user),
            content_type='application/json')
        if reg_response.status_code == 201:
            uid = reg_response.get_json().get("userID")
        else:
            user = supabase.table("User").select("uid").eq("email", self.test_user["email"]).execute()
            uid = user.data[0]["uid"] if user.data else None
        self.assertIsNotNone(uid, "UID konnte nicht ermittelt werden.")
        update_body = {
            "firstname": "Moritz",
            "lastname": "Musterfrau",
            "email": "moritz.musterfrau@example.com",
            "passwort": "neuespasswort456"
        }
        response = self.client.open(
            f'/update/{uid}',
            method='POST',
            data=json.dumps(update_body),
            content_type='application/json')
        self.assertEqual(response.status_code, 200)

if __name__ == '__main__':
    import unittest
    unittest.main()
