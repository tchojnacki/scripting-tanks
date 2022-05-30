from colorsys import hls_to_rgb
from random import Random
from utils.uid import CID


def assign_color(cid: CID) -> str:
    generator = Random(cid)
    red, green, blue = (int(255 * val) for val in hls_to_rgb(generator.random(), 0.5, 0.75))
    return f"#{red:02x}{green:02x}{blue:02x}"
