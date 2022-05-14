from hashlib import md5
from random import getrandbits
from typing import NewType, Any


def _get_uid(token: Any = None) -> str:
    if token is None:
        token = getrandbits(32)
    return md5(str(token).encode()).hexdigest()


CID = NewType("CID", str)


def get_cid(token: Any = None) -> CID:
    return CID("CID$" + _get_uid(token))


LID = NewType("LID", str)


def get_lid(token: Any = None) -> LID:
    return LID("LID$" + _get_uid(token))


EID = NewType("EID", str)


def get_eid(token: Any = None) -> EID:
    return EID("EID$" + _get_uid(token))
