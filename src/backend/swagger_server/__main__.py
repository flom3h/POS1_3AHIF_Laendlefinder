#!/usr/bin/env python3

import os
import connexion
from swagger_server import encoder
<<<<<<< Updated upstream
from supabase import create_client, Client
from dotenv import load_dotenv

load_dotenv()
<<<<<<< HEAD
url: str = os.getenv("SUPABASE_URL")
key: str = os.getenv("SUPABASE_KEY")
=======
=======
#from controllers.eventbrite_controller import FetchEventbriteEvents

def init_db():
    pass
>>>>>>> Stashed changes
>>>>>>> 0121bca266003501f5cc22c2e1a9a6db5b91c8df

supabase: Client = create_client(url, key)
def main():

    
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
