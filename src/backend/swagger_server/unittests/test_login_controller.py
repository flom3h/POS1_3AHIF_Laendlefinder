# coding: utf-8

from __future__ import absolute_import

from flask import json
from six import BytesIO

from swagger_server.models.user_body import UserBody  # noqa: E501
from swagger_server.unittests import BaseTestCase
from swagger_server.__main__ import supabase  # Supabase-Client importieren



class TestLoginController(BaseTestCase):
    """LoginController integration test stubs"""

    def setUp(self):
        super().setUp()
        self.test_user = {
            "firstname": "Max",
            "lastname": "Mustermann",
            "email": "max.mustermann@example.com",
            "passwort": "sicherespasswort123"
        }
        # Registriere den User, falls noch nicht vorhanden
        self.client.open(
            '/registrieren',
            method='POST',
            data=json.dumps(self.test_user),
            content_type='application/json'
        )

    def test_user_login_post_success(self):
        """Erfolgreicher Login"""
        login_body = {
            "email": self.test_user["email"],
            "passwort": self.test_user["passwort"]
        }
        response = self.client.open(
            '/login',
            method='POST',
            data=json.dumps(login_body),
            content_type='application/json')
        self.assertEqual(response.status_code, 201)
        data = response.get_json()
        self.assertEqual(data["message"], "Erfolgreich angemeldet")

    def test_user_login_post_wrong_password(self):
        """Login mit falschem Passwort"""
        login_body = {
            "email": self.test_user["email"],
            "passwort": "falschespasswort"
        }
        response = self.client.open(
            '/login',
            method='POST',
            data=json.dumps(login_body),
            content_type='application/json')
        self.assertEqual(response.status_code, 401)
        data = response.get_json()
        self.assertEqual(data["message"], "Ungültige Anmeldeinformationen")

    def test_user_login_post_nonexistent_user(self):
        """Login mit nicht existierender E-Mail"""
        login_body = {
            "email": "nichtda@example.com",
            "passwort": "irgendwas"
        }
        response = self.client.open(
            '/login',
            method='POST',
            data=json.dumps(login_body),
            content_type='application/json')
        self.assertEqual(response.status_code, 401)
        data = response.get_json()
        self.assertEqual(data["message"], "Ungültige Anmeldeinformationen")

    def delete_test_user(self):
        """Löscht den Testbenutzer nach den Tests"""
        result = supabase.table("User").delete().eq("email", self.test_user["email"]).execute()
        if result.status_code != 204:
            print("Fehler beim Löschen des Testbenutzers:", result.data)


if __name__ == '__main__':
    import unittest
    unittest.main()
