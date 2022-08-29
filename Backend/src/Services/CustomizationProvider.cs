using Backend.Domain;
using Backend.Utils;

namespace Backend.Services;

public class CustomizationProvider : ICustomizationProvider
{
    private readonly static Random Rand = new();

    public string AssignDisplayName()
        => $"{AnimalAdjectives[Rand.Next(AnimalAdjectives.Count)]} {AnimalNames[Rand.Next(AnimalNames.Count)]}";

    public TankColors AssignTankColors()
    {
        var hue = Rand.NextDouble() * 360;
        var (r, g, b) = ColorUtils.HslToRgb(hue, 0.75, 0.5);
        var color = ColorUtils.RgbToString(r, g, b);

        return new()
        {
            TankColor = color,
            TurretColor = color
        };
    }

    private static readonly IReadOnlyList<string> AnimalNames = new List<string>()
    {
        "Aardvark",
        "Albatross",
        "Alligator",
        "Alpaca",
        "Ant",
        "Anteater",
        "Antelope",
        "Ape",
        "Armadillo",
        "Baboon",
        "Badger",
        "Barracuda",
        "Bat",
        "Bear",
        "Beaver",
        "Bee",
        "Beetle",
        "Binturong",
        "Bird",
        "Bison",
        "Bluebird",
        "Boar",
        "Bobcat",
        "Budgerigar",
        "Buffalo",
        "Butterfly",
        "Camel",
        "Capybara",
        "Caracal",
        "Caribou",
        "Cassowary",
        "Cat",
        "Caterpillar",
        "Chamois",
        "Cheetah",
        "Chicken",
        "Chimpanzee",
        "Chinchilla",
        "Chough",
        "Coati",
        "Cobra",
        "Cockroach",
        "Cod",
        "Cormorant",
        "Cougar",
        "Coyote",
        "Crab",
        "Crane",
        "Cricket",
        "Crocodile",
        "Crow",
        "Cuckoo",
        "Curlew",
        "Deer",
        "Dhole",
        "Dingo",
        "Dinosaur",
        "Dog",
        "Dogfish",
        "Dolphin",
        "Donkey",
        "Dove",
        "Dragonfly",
        "Duck",
        "Dugong",
        "Dunlin",
        "Eagle",
        "Echidna",
        "Eel",
        "Eland",
        "Elephant",
        "Elk",
        "Emu",
        "Falcon",
        "Ferret",
        "Finch",
        "Fish",
        "Fisher",
        "Flamingo",
        "Fly",
        "Fossa",
        "Fox",
        "Frog",
        "Gaur",
        "Gazelle",
        "Gecko",
        "Genet",
        "Gerbil",
        "Giraffe",
        "Gnat",
        "Gnu",
        "Goat",
        "Goldfinch",
        "Goosander",
        "Goose",
        "Gorilla",
        "Goshawk",
        "Grasshopper",
        "Grouse",
        "Guanaco",
        "Gull",
        "Hamster",
        "Hare",
        "Hawk",
        "Hedgehog",
        "Heron",
        "Herring",
        "Hippopotamus",
        "Hoatzin",
        "Hoopoe",
        "Hornet",
        "Horse",
        "Hummingbird",
        "Hyena",
        "Ibex",
        "Ibis",
        "Iguana",
        "Impala",
        "Jackal",
        "Jaguar",
        "Jay",
        "Jellyfish",
        "Junglefowl",
        "Kangaroo",
        "Kingbird",
        "Kingfisher",
        "Kinkajou",
        "Kite",
        "Koala",
        "Kodkod",
        "Kookaburra",
        "Kouprey",
        "Kowari",
        "Langur",
        "Lapwing",
        "Lark",
        "Lechwe",
        "Lemur",
        "Leopard",
        "Lion",
        "Lizard",
        "Llama",
        "Lobster",
        "Locust",
        "Loris",
        "Louse",
        "Lynx",
        "Lyrebird",
        "Macaque",
        "Macaw",
        "Magpie",
        "Mallard",
        "Mammoth",
        "Manatee",
        "Mandrill",
        "Marmoset",
        "Marmot",
        "Meerkat",
        "Mink",
        "Mole",
        "Mongoose",
        "Monkey",
        "Moose",
        "Mosquito",
        "Mouse",
        "Myna",
        "Narwhal",
        "Newt",
        "Nightingale",
        "Nilgai",
        "Ocelot",
        "Octopus",
        "Okapi",
        "Olingo",
        "Opossum",
        "Orangutan",
        "Oryx",
        "Ostrich",
        "Otter",
        "Ox",
        "Owl",
        "Oyster",
        "Panther",
        "Parrot",
        "Panda",
        "Partridge",
        "Peafowl",
        "Pelican",
        "Penguin",
        "Pheasant",
        "Pig",
        "Pigeon",
        "Pika",
        "Pony",
        "Porcupine",
        "Porpoise",
        "Pug",
        "Quail",
        "Quelea",
        "Quetzal",
        "Rabbit",
        "Raccoon",
        "Ram",
        "Rat",
        "Raven",
        "Reindeer",
        "Rhea",
        "Rhinoceros",
        "Rook",
        "Saki",
        "Salamander",
        "Salmon",
        "Sandpiper",
        "Sardine",
        "Sassaby",
        "Seahorse",
        "Seal",
        "Serval",
        "Shark",
        "Sheep",
        "Shrew",
        "Shrike",
        "Siamang",
        "Skink",
        "Skipper",
        "Skunk",
        "Sloth",
        "Snail",
        "Snake",
        "Spider",
        "Spoonbill",
        "Squid",
        "Squirrel",
        "Starfish",
        "Starling",
        "Stilt",
        "Swan",
        "Tamarin",
        "Tapir",
        "Tarsier",
        "Termite",
        "Thrush",
        "Tiger",
        "Toad",
        "Topi",
        "Toucan",
        "Turaco",
        "Turkey",
        "Turtle",
        "Vicuña",
        "Vinegaroon",
        "Viper",
        "Vulture",
        "Wallaby",
        "Walrus",
        "Wasp",
        "Waxwing",
        "Weasel",
        "Whale",
        "Wildebeest",
        "Wobbegong",
        "Wolf",
        "Wolverine",
        "Wombat",
        "Woodpecker",
        "Worm",
        "Wren",
        "Yak",
        "Zebra"
    };

    private static readonly IReadOnlyList<string> AnimalAdjectives = new List<string>
    {
        "Adaptive",
        "Adventurous",
        "Ambitious",
        "Amusing",
        "Balanced",
        "Brave",
        "Bright",
        "Calm",
        "Capable",
        "Charming",
        "Clever",
        "Courageous",
        "Creative",
        "Decisive",
        "Determined",
        "Enthusiastic",
        "Faithful",
        "Fearless",
        "Friendly",
        "Funny",
        "Generous",
        "Gentle",
        "Good",
        "Honest",
        "Humorous",
        "Independent",
        "Intelligent",
        "Kind",
        "Loyal",
        "Modest",
        "Nice",
        "Optimistic",
        "Patient",
        "Polite",
        "Powerful",
        "Reliable",
        "Sincere",
        "Tough"
    };
}
