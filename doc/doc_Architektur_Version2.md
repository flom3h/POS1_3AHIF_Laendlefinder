# Architektur & Funktionsblöcke

## Übersicht

Das System besteht aus zwei Hauptkomponenten:
- WPF-Frontend (C#)
- Python-Backend (REST API, Flask/Connexion, Swagger/OpenAPI)

Die Kommunikation erfolgt per HTTP/REST mit JSON.

## Architekturdiagramm

- Siehe ER- und Architekturdiagramm: [LaendlefinderER.drawio.pdf](LaendlefinderER.drawio.pdf)

## Haupt-Funktionsblöcke

### Frontend (WPF)
- Eventanzeige & -suche
- Kartenansicht
- User-Authentifizierung
- Favoritenverwaltung

### Backend (Python)
- Event-API (CRUD)
- User-API (Login/Register)
- Favoriten-API
- Kategoriefilter
- Verwaltung der Datenbankzugriffe

### Datenbank
- User, Event, Category, Favorite

## Abläufe
1. User startet Frontend, meldet sich an/registriert sich
2. Frontend fragt Events/Favoriten vom Backend ab
3. Events werden auf Karte angezeigt, sind filterbar
4. Favoriten können gespeichert und angezeigt werden

## Externe Schnittstellen
- Google Maps API oder OpenStreetMaps
