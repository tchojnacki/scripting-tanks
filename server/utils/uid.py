from hashlib import md5
from random import getrandbits
from typing import NewType


def _get_uid(token: any = None) -> str:
    if token is None:
        token = getrandbits(32)
    return md5(str(token).encode()).hexdigest()


CID = NewType("CID", str)


def get_cid(token: any = None) -> CID:
    return CID("CID$" + _get_uid(token))


LID = NewType("LID", str)


def get_lid(token: any = None) -> LID:
    return LID("LID$" + _get_uid(token))
