from os import getenv

DEVELOPMENT = bool(getenv("DEVELOPMENT"))
PORT = int(getenv("PORT", "3000"))
