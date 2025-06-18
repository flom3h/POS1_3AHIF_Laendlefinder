# Detaillierte Beschreibung der Umsetzung

## Backend

- Generiert mit swagger-codegen, basiert auf Flask und Connexion für OpenAPI-Schnittstellen.
- Start: `pip3 install -r requirements.txt` und `python3 -m swagger_server`
- REST-Endpunkte für Events, Nutzer, Favoriten, Kategorien
- Integration mit SQL-Datenbank (z.B. SQLite oder MySQL)

## Frontend

- WPF-Projekt (C#), Multi-Page-App
- Views: Home, Explore, Kalender, Favorites, Map, Info, Login, Register
- HTTP-Requests ans Backend (REST)
- Kartenintegration über WebView/Maps-API

## Besonderheiten

- Swagger/OpenAPI für automatische API-Doku
- Cloud-Datenbankanbindung
- UI mit Screenshots im doc-Ordner dokumentiert

## Verwendete Bibliotheken

- Python: Flask, Connexion, SQLAlchemy, Swagger-Server
- C#: WPF, System.Net.Http

---

Siehe Screenshots und Diagramme im doc-Ordner zur Veranschaulichung der wichtigsten Seiten.