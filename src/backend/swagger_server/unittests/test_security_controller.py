# coding: utf-8

from __future__ import absolute_import

from flask import json
from six import BytesIO

from swagger_server.unittests import BaseTestCase
from swagger_server.controllers.security_controller import basic_auth

class TestSecurityController(BaseTestCase):
    """SecurityController integration test stubs"""

    def test_basic_auth_success(self):
        """Testet erfolgreiche Basic Authentifizierung"""
        result = basic_auth("admin", "kaffee123")
        self.assertIsInstance(result, dict)
        self.assertEqual(result["sub"], "admin")

    def test_basic_auth_wrong_password(self):
        """Testet Basic Authentifizierung mit falschem Passwort"""
        result = basic_auth("admin", "falsch")
        self.assertIsNone(result)

    def test_basic_auth_wrong_username(self):
        """Testet Basic Authentifizierung mit falschem Benutzernamen"""
        result = basic_auth("user", "kaffee123")
        self.assertIsNone(result)

    def test_basic_auth_wrong_both(self):
        """Testet Basic Authentifizierung mit falschem Benutzernamen und Passwort"""
        result = basic_auth("user", "falsch")
        self.assertIsNone(result)

