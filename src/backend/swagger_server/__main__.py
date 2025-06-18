import os
import connexion
from swagger_server import encoder
from supabase import create_client, Client
from dotenv import load_dotenv
from logger import logger

load_dotenv()
url: str = os.getenv("SUPABASE_URL")
key: str = os.getenv("SUPABASE_KEY")

supabase: Client = create_client(url, key)
logger.info("Supabase erfolgreich verbunden")

def main():    
    """
    Startet die Connexion-App mit Swagger/OpenAPI-Spezifikation.

    Lädt die API-Spezifikation, setzt den JSON-Encoder und startet den Server auf Port 8081.

    Returns:
        None
    """
    app = connexion.App(__name__, specification_dir='./swagger/')
    app.app.json_encoder = encoder.JSONEncoder
    app.add_api('swagger.yaml', arguments={'title': 'Events API'}, pythonic_params=True)
    app.run(port=8081)
    logger.info("Swagger API gestartet auf Port 8081")

if __name__ == '__main__':
    main()