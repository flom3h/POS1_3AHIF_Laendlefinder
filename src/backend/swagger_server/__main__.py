#!/usr/bin/env python3

import os
import connexion
from swagger_server import encoder
<<<<<<< Updated upstream
from supabase import create_client, Client
from dotenv import load_dotenv

load_dotenv()
=======
#from controllers.eventbrite_controller import FetchEventbriteEvents

def init_db():
    pass
>>>>>>> Stashed changes

def main():

    url: str = os.getenv("SUPABASE_URL")
    key: str = os.getenv("SUPABASE_KEY")

    supabase: Client = create_client(url, key)
    app = connexion.App(__name__, specification_dir='./swagger/')
    app.app.json_encoder = encoder.JSONEncoder

    #app.add_url_rule('/eventbrite/fetch-events', view_func=FetchEventbriteEvents.as_view('fetch_eventbrite_events'), methods=['POST'])

    app.add_api('swagger.yaml', arguments={'title': 'Events API'}, pythonic_params=True)
<<<<<<< Updated upstream
    app.run(port=8081)
=======
    app.run(port=8080)
>>>>>>> Stashed changes

if __name__ == '__main__':
    main()
