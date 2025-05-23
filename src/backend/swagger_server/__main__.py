#!/usr/bin/env python3

import os
import connexion
from swagger_server import encoder
from supabase import create_client, Client
from dotenv import load_dotenv

load_dotenv()

def main():

    url: str = os.getenv("SUPABASE_URL")
    key: str = os.getenv("SUPABASE_KEY")

    supabase: Client = create_client(url, key)
    app = connexion.App(__name__, specification_dir='./swagger/')
    app.app.json_encoder = encoder.JSONEncoder
    app.add_api('swagger.yaml', arguments={'title': 'Events API'}, pythonic_params=True)
    app.run(port=8081)

if __name__ == '__main__':
    main()
