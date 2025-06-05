from flask import Blueprint, request, jsonify
from swagger_server.controllers.abrufen_controller import fetch_events

favoriten_controller = Blueprint('favoriten', __name__)

@favoriten_controller.route('/favoriten', methods=['GET'])
def get_favoriten():
    benutzername = request.args.get('benutzername')
    # Logic to retrieve favorites for the user
    # This is a placeholder for actual implementation
    return jsonify({"message": "Favoriten abgerufen", "benutzername": benutzername})

@favoriten_controller.route('/favoriten', methods=['POST'])
def add_to_favoriten():
    data = request.get_json()
    benutzername = data.get('benutzername')
    event_id = data.get('event_id')
    # Logic to add event to favorites
    # This is a placeholder for actual implementation
    return jsonify({"message": "Event zu Favoriten hinzugefügt", "benutzername": benutzername, "event_id": event_id}), 201

@favoriten_controller.route('/favoriten', methods=['DELETE'])
def remove_from_favoriten():
    data = request.get_json()
    benutzername = data.get('benutzername')
    event_id = data.get('event_id')
    # Logic to remove event from favorites
    # This is a placeholder for actual implementation
    return jsonify({"message": "Event aus Favoriten entfernt", "benutzername": benutzername, "event_id": event_id}), 200