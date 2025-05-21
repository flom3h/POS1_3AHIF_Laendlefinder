# coding: utf-8

from __future__ import absolute_import

from flask import json
from six import BytesIO

from swagger_server.test import BaseTestCase


class TestAbrufenController(BaseTestCase):
    """AbrufenController integration test stubs"""

    def test_event_by_id_get(self):
        """Test case for event_by_id_get

        Details zu einem einzelnen Event abrufen
        """
        response = self.client.open(
            '/events/{id}'.format(id=56),
            method='GET')
        self.assert200(response,
                       'Response body is : ' + response.data.decode('utf-8'))

    def test_events_get(self):
        """Test case for events_get

        Events abrufen (mit optionalen Filtern)
        """
        query_string = [('eventname', 'eventname_example'),
                        ('kategorie', 'kategorie_example'),
                        ('ort', 'ort_example'),
                        ('region', 'region_example'),
                        ('datum', '2013-10-20')]
        response = self.client.open(
            '/events',
            method='GET',
            query_string=query_string)
        self.assert200(response,
                       'Response body is : ' + response.data.decode('utf-8'))

    def test_kategorien_get(self):
        """Test case for kategorien_get

        Kategorien abrufen von API
        """
        response = self.client.open(
            '/kategorien',
            method='GET')
        self.assert200(response,
                       'Response body is : ' + response.data.decode('utf-8'))


if __name__ == '__main__':
    import unittest
    unittest.main()
