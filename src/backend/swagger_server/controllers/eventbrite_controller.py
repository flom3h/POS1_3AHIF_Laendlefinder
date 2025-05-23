from flask_restful import Resource
from flask import request
from eventbrite import Eventbrite
from supabase import create_client, Client

EVENTBRITE_API_KEY = 'your_eventbrite_api_key'
eventbrite = Eventbrite(EVENTBRITE_API_KEY)

SUPABASE_URL = "https://esgeugesyqlszsalokzz.supabase.co/"
SUPABASE_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImVzZ2V1Z2VzeXFsc3pzYWxva3p6Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDY3NzY0ODEsImV4cCI6MjA2MjM1MjQ4MX0.e1zRQahAM85oPyJv6wFv91JMMBQ6BGersv8H632mMI4"
supabase: Client = create_client(SUPABASE_URL, SUPABASE_KEY)

def fetch_eventbrite_events():
    """Fetch events from Eventbrite API."""
    events = eventbrite.get('/events/search/', data={'location.address': 'YourLocation'})
    return events.get('events', [])

def store_events_in_supabase(events):
    """Store events in Supabase database."""
    for event in events:
        event_data = {
            'name': event.get('name', {}).get('text'),
            'date': event.get('start', {}).get('local'),
            'description': event.get('description', {}).get('text'),
            'link': event.get('url'),
            'type': event.get('type')
        }
        supabase.table('Events').insert(event_data).execute()

class FetchEventbriteEvents(Resource):
    def post(self):
        """Endpoint to manually trigger fetching and storing Eventbrite events."""
        events = fetch_eventbrite_events()
        store_events_in_supabase(events)
        return {'message': 'Events fetched and stored successfully'}, 200
