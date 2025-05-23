#!/usr/bin/env python3

import connexion
import sqlite3
from swagger_server import encoder


def init_db():
    pass
    

def main():
    app = connexion.App(__name__, specification_dir='./swagger/')
    app.app.json_encoder = encoder.JSONEncoder
    app.add_api('swagger.yaml', arguments={'title': 'Events API'}, pythonic_params=True)
    app.run(port=8080)


if __name__ == '__main__':
    main()
