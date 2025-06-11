from flask import Flask, request, jsonify
from swagger_server.controllers.abrufen_controller import fetch_events

def user_login_post():
    data = request.get_json()
    email = data.get('email')
    password = data.get('passwort')

    # Here you would typically validate the user's credentials
    # For demonstration, let's assume a successful login
    if email and password:  # Replace with actual validation logic
        return jsonify({"message": "Login successful", "email": email}), 201
    else:
        return jsonify({"message": "Invalid credentials"}), 404