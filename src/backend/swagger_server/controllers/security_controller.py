def basic_auth(username, password, required_scopes=None):
    if username == "admin" and password == "kaffee123":
        return {"sub": username}
    return None
    