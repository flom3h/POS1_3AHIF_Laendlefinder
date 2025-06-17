# coding: utf-8

from __future__ import absolute_import

from flask import json
from six import BytesIO

from swagger_server.unittests import BaseTestCase
from swagger_server.services import event_fetcher


class TestEventFetcher(BaseTestCase):
    def test_extract_event_data_minimal(self):
        event = {
            "@type": "Event",
            "location": {"geo": {"longitude": 9.7, "latitude": 47.2}},
            "name": "Testevent",
            "startDate": "2025-07-10T18:00:00.000Z",
            "description": "Beschreibung",
        }
        result = event_fetcher.extract_event_data(event)
        self.assertIsInstance(result, dict)
        self.assertEqual(result["nameofevent"], "Testevent")
        self.assertEqual(result["longitude"], 9.7)
        self.assertEqual(result["latitude"], 47.2)

    def test_extract_event_data_no_geo(self):
        event = {"@type": "Event", "name": "OhneGeo"}
        result = event_fetcher.extract_event_data(event)
        self.assertIsNone(result)

    def test_fetch_and_store_events(self):
        # Diese Funktion ruft echte Daten ab und speichert sie, daher nur auf Erfolg prüfen
        events = event_fetcher.fetch_and_store_events()
        self.assertIsInstance(events, list)
        if events:
            self.assertIn("nameofevent", events[0])

