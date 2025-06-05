from flask import Blueprint, request, jsonify
from swagger_server.models import User  # Assuming you have a User model defined
from swagger_server import db  # Assuming you have a database instance

registrieren_controller = Blueprint('registrieren', __name__)

@registrieren_controller.route('/registrieren', methods=['POST'])
def register_user():
    data = request.get_json()
    
    if not data or 'firstname' not in data or 'lastname' not in data or 'email' not in data or 'passwort' not in data:
        return jsonify({"message": "Missing required fields"}), 400

    new_user = User(
        firstname=data['firstname'],
        lastname=data['lastname'],
        email=data['email'],
        passwort=data['passwort']
    )

    db.session.add(new_user)
    db.session.commit()

    return jsonify({"message": "User registered successfully"}), 201