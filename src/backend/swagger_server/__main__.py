#!/usr/bin/env python3

import connexion
from swagger_server import encoder
from supabase import create_client, Client

SUPABASE_URL = "https://esgeugesyqlszsalokzz.supabase.co"
SUPABASE_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImVzZ2V1Z2VzeXFsc3pzYWxva3p6Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDY3NzY0ODEsImV4cCI6MjA2MjM1MjQ4MX0.e1zRQahAM85oPyJv6wFv91JMMBQ6BGersv8H632mMI4"
supabase: Client = None

def init_db():
    global supabase
    supabase = create_client(SUPABASE_URL, SUPABASE_KEY)
    print("Verbindung zur Supabase-Datenbank hergestellt!")

def main():
    init_db()
    app = connexion.App(__name__, specification_dir='./swagger/')
    app.app.json_encoder = encoder.JSONEncoder
    app.add_api('swagger.yaml', arguments={'title': 'Events API'}, pythonic_params=True)
    app.run(port=8080)

if __name__ == '__main__':
    main()
