from typing import Any, NewType, Optional, Union, get_args
from cattr import Converter
from .create_lobby import CCreateLobbyMsg
from .enter_lobby import CEnterLobbyMsg
from .leave_lobby import CLeaveLobbyMsg

ClientMsg = Union[CCreateLobbyMsg, CEnterLobbyMsg, CLeaveLobbyMsg]

_converter = Converter()


def _class_from_tag(tag):
    match tag:
        case "c-create-lobby":
            return CCreateLobbyMsg
        case "c-enter-lobby":
            return CEnterLobbyMsg
        case "c-leave-lobby":
            return CLeaveLobbyMsg


_converter.register_structure_hook_func(
    lambda t: t.__class__ is NewType, lambda d, t: t.__call__(d)
)

_converter.register_structure_hook(
    ClientMsg,
    lambda d, _: _converter.structure(d, _class_from_tag(d["tag"]))
)


def parse_message(data: Any) -> Optional[ClientMsg]:
    result = _converter.structure(data, ClientMsg)
    return result if isinstance(result, ClientMsg) else None
