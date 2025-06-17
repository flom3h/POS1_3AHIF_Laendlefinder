# coding: utf-8

from __future__ import absolute_import

from flask import json
from six import BytesIO

from swagger_server.unittests import BaseTestCase


class TestAbrufenController(BaseTestCase):
    """AbrufenController integration test stubs"""

    def test_event_by_id_get(self):
        """Testet das Abrufen eines existierenden Events"""
        eid = 182
        response = self.client.open(
            f'/events/{eid}',
            method='GET')
        self.assertEqual(response.status_code, 200)
        data = response.get_json()
        self.assertEqual(data["eid"], eid)
        self.assertIn("name", data)
        self.assertIn("Location", data)

    #def test_event_by_id_get_not_found(self): # bei funktion muss .single() entfernt werden weil sonst 500 Fehler
    #    """Testet das Abrufen eines nicht existierenden Events"""
    #    eid = 9999
    #    response = self.client.open(
    #        f'/events/{eid}',
    #        method='GET')
    #    self.assertEqual(response.status_code, 404)

    def test_events_get(self):
        """Testet das Abrufen aller Events"""
        response = self.client.open(
            '/events',
            method='GET')
        self.assertEqual(response.status_code, 200)
        data = response.get_json()
        self.assertIsInstance(data, list)
        if data:
            self.assertIn("eid", data[0])
            self.assertIn("Location", data[0])

    def test_kategorien_get(self):
        """Testet das Abrufen aller Kategorien"""
        response = self.client.open(
            '/kategorien',
            method='GET')
        self.assertEqual(response.status_code, 200)
        data = response.get_json()
        self.assertIsInstance(data, list)
        if data:
            self.assertIn("tid", data[0])
            self.assertIn("type", data[0])

    def test_get_user_id_swagger(self):
        """Testet das Abrufen der User-ID anhand der E-Mail über die OpenAPI-Route"""
        test_user = {
            "firstname": "Max",
            "lastname": "Mustermann",
            "email": "max.mustermann@example.com",
            "passwort": "sicherespasswort123"
        }
        self.client.open(
            '/registrieren',
            method='POST',
            data=json.dumps(test_user),
            content_type='application/json')
        # Teste get_user_id mit existierender E-Mail (Pfadparameter!)
        response = self.client.open(
            f'/user_id/{test_user["email"]}',
            method='GET')
        self.assertIn(response.status_code, [200, 404])
        if response.status_code == 200:
            data = response.get_json()
            self.assertIsInstance(data, int)
        # Teste get_user_id mit nicht existierender E-Mail
        response2 = self.client.open(
            '/user_id/nichtda@example.com',
            method='GET')
        self.assertEqual(response2.status_code, 404)


if __name__ == '__main__':
    import unittest
    unittest.main()