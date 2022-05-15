from typing import Any, NewType, Optional, Union, get_args
from cattr import Converter
from .create_lobby import CCreateLobbyMsg
from .enter_lobby import CEnterLobbyMsg
from .leave_lobby import CLeaveLobbyMsg
from .start_game import CStartGameMsg
from .set_input_axes import CSetInputAxesMsg


ClientMsg = Union[CCreateLobbyMsg, CEnterLobbyMsg, CLeaveLobbyMsg, CSetInputAxesMsg, CStartGameMsg]

_converter = Converter()


def _class_from_tag(tag):
    for msg_type in get_args(ClientMsg):
        if msg_type.tag == tag:
            return msg_type


_converter.register_structure_hook_func(
    lambda t: t.__class__ is NewType, lambda d, t: t.__call__(d)
)

_converter.register_structure_hook(
    ClientMsg,
    lambda d, _: _converter.structure(d, _class_from_tag(d["tag"]))
)


def parse_message(data: Any) -> Optional[ClientMsg]:
    try:
        result = _converter.structure(data, ClientMsg)
        return result if isinstance(result, ClientMsg) else None
    except:  # pylint: disable=bare-except
        return None
