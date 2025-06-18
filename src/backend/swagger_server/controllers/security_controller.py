def basic_auth(username, password, required_scopes=None):
    """! Führt eine einfache Benutzername/Passwort-Authentifizierung durch.
    
    @details Gibt ein Dictionary mit dem Benutzernamen zurück, wenn die Authentifizierung erfolgreich ist.

    @param username Der Benutzername.
    @param password Das Passwort.
    @param required_scopes (optional) Benötigte Berechtigungen (wird hier ignoriert).
    @return dict mit "sub" (Benutzername) bei Erfolg, sonst None.
    """
    if username == "admin" and password == "kaffee123":
        return {"sub": username}
    return None
    