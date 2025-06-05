from flask import Blueprint, request, jsonify

# Create a blueprint for the abrufen controller
abrufen_bp = Blueprint('abrufen', __name__)

# Sample data for events (this would typically come from a database)
events = [
    {"id": 1, "name": "Event 1", "date": "2023-10-01", "category": "Music"},
    {"id": 2, "name": "Event 2", "date": "2023-10-02", "category": "Art"},
    {"id": 3, "name": "Event 3", "date": "2023-10-03", "category": "Sports"},
]

@abrufen_bp.route('/events', methods=['GET'])
def fetch_events():
    # Get query parameters
    eventname = request.args.get('eventname')
    kategorie = request.args.get('kategorie')
    ort = request.args.get('ort')
    region = request.args.get('region')
    datum = request.args.get('datum')

    # Filter events based on query parameters
    filtered_events = events
    if eventname:
        filtered_events = [event for event in filtered_events if eventname.lower() in event['name'].lower()]
    if kategorie:
        filtered_events = [event for event in filtered_events if event['category'].lower() == kategorie.lower()]
    if datum:
        filtered_events = [event for event in filtered_events if event['date'] == datum]

    return jsonify(filtered_events), 200

@abrufen_bp.route('/events/<int:id>', methods=['GET'])
def fetch_event_by_id(id):
    # Find the event by ID
    event = next((event for event in events if event['id'] == id), None)
    if event:
        return jsonify(event), 200
    return jsonify({"message": "Event nicht gefunden"}), 404

# Register the blueprint in the app.py file
# Make sure to import and register this blueprint in your app.py
# from swagger_server.controllers.abrufen_controller import abrufen_bp
# app.register_blueprint(abrufen_bp)