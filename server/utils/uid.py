from hashlib import md5
from random import getrandbits


def get_uid(token: any = None) -> str:
    if token is None:
        token = getrandbits(32)
    return md5(str(token).encode()).hexdigest()
