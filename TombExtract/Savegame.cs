using System;
using System.Collections.Generic;

namespace TombExtract
{
    public enum GameMode
    {
        Normal,
        Plus,
        None
    }

    public class Savegame
    {
        public int Offset { get; set; }
        public Int32 Number { get; set; }
        public string Name { get; set; }
        public GameMode Mode { get; set; }
        public bool IsEmptySlot { get; set; }
        public int Slot { get; set; }
        public byte[] SavegameBytes { get; set; }
        public bool SaveNumberFirst { get; set; }

        public Savegame(int savegameOffset, Int32 saveNumber, string levelName, GameMode gameMode, bool saveNumberFirst = false)
        {
            Number = saveNumber;
            Name = levelName;
            Offset = savegameOffset;
            Mode = gameMode;
            IsEmptySlot = false;
            SaveNumberFirst = saveNumberFirst;
        }

        public override string ToString()
        {
            if (IsEmptySlot)
            {
                return "Empty Slot";
            }

            if (SaveNumberFirst)
            {
                return $"{Number} - {Name}{(Mode == GameMode.Plus ? "+" : "")}";
            }

            return $"{Name}{(Mode == GameMode.Plus ? "+" : "")} - {Number}";
        }
    }

    public static class LevelNames
    {
        public static readonly Dictionary<byte, string> TR1 = new Dictionary<byte, string>()
        {
            { 1,  "Caves"                       },
            { 2,  "City of Vilcabamba"          },
            { 3,  "Lost Valley"                 },
            { 4,  "Tomb of Qualopec"            },
            { 5,  "St. Francis' Folly"          },
            { 6,  "Colosseum"                   },
            { 7,  "Palace Midas"                },
            { 8,  "The Cistern"                 },
            { 9,  "Tomb of Tihocan"             },
            { 10, "City of Khamoon"             },
            { 11, "Obelisk of Khamoon"          },
            { 12, "Sanctuary of the Scion"      },
            { 13, "Natla's Mines"               },
            { 14, "Atlantis"                    },
            { 15, "The Great Pyramid"           },
            { 16, "Return to Egypt"             },
            { 17, "Temple of the Cat"           },
            { 18, "Atlantean Stronghold"        },
            { 19, "The Hive"                    },
        };

        public static readonly Dictionary<byte, string> TR2 = new Dictionary<byte, string>()
        {
            {  1, "The Great Wall"              },
            {  2, "Venice"                      },
            {  3, "Bartoli's Hideout"           },
            {  4, "Opera House"                 },
            {  5, "Offshore Rig"                },
            {  6, "Diving Area"                 },
            {  7, "40 Fathoms"                  },
            {  8, "Wreck of the Maria Doria"    },
            {  9, "Living Quarters"             },
            { 10, "The Deck"                    },
            { 11, "Tibetan Foothills"           },
            { 12, "Barkhang Monastery"          },
            { 13, "Catacombs of the Talion"     },
            { 14, "Ice Palace"                  },
            { 15, "Temple of Xian"              },
            { 16, "Floating Islands"            },
            { 17, "The Dragon's Lair"           },
            { 18, "Home Sweet Home"             },
            { 19, "The Cold War"                },
            { 20, "Fool's Gold"                 },
            { 21, "Furnace of the Gods"         },
            { 22, "Kingdom"                     },
            { 23, "Nightmare in Vegas"          },
        };

        public static readonly Dictionary<byte, string> TR3 = new Dictionary<byte, string>()
        {
            {  1, "Jungle"                      },
            {  2, "Temple Ruins"                },
            {  3, "The River Ganges"            },
            {  4, "Caves of Kaliya"             },
            {  5, "Coastal Village"             },
            {  6, "Crash Site"                  },
            {  7, "Madubu Gorge"                },
            {  8, "Temple of Puna"              },
            {  9, "Thames Wharf"                },
            { 10, "Aldwych"                     },
            { 11, "Lud's Gate"                  },
            { 12, "City"                        },
            { 13, "Nevada Desert"               },
            { 14, "High Security Compound"      },
            { 15, "Area 51"                     },
            { 16, "Antarctica"                  },
            { 17, "RX-Tech Mines"               },
            { 18, "Lost City of Tinnos"         },
            { 19, "Meteorite Cavern"            },
            { 20, "All Hallows"                 },
            { 21, "Highland Fling"              },
            { 22, "Willard's Lair"              },
            { 23, "Shakespeare Cliff"           },
            { 24, "Sleeping with the Fishes"    },
            { 25, "It's a Madhouse!"            },
            { 26, "Reunion"                     },
        };

        public static readonly Dictionary<byte, string> TR4 = new Dictionary<byte, string>()
        {
            {  1, "Angkor Wat"                  },
            {  2, "Race for the Iris"           },
            {  3, "The Tomb of Seth"            },
            {  4, "Burial Chambers"             },
            {  5, "Valley of the Kings"         },
            {  6, "KV5"                         },
            {  7, "Temple of Karnak"            },
            {  8, "The Great Hypostyle Hall"    },
            {  9, "Sacred Lake"                 },
            { 11, "Tomb of Semerkhet"           },
            { 12, "Guardian of Semerkhet"       },
            { 13, "Desert Railroad"             },
            { 14, "Alexandria"                  },
            { 15, "Coastal Ruins"               },
            { 16, "Pharos, Temple of Isis"      },
            { 17, "Cleopatra's Palaces"         },
            { 18, "Catacombs"                   },
            { 19, "Temple of Poseidon"          },
            { 20, "The Lost Library"            },
            { 21, "Hall of Demetrius"           },
            { 22, "City of the Dead"            },
            { 23, "Trenches"                    },
            { 24, "Chambers of Tulun"           },
            { 25, "Street Bazaar"               },
            { 26, "Citadel Gate"                },
            { 27, "Citadel"                     },
            { 28, "The Sphinx Complex"          },
            { 30, "Underneath the Sphinx"       },
            { 31, "Menkaure's Pyramid"          },
            { 32, "Inside Menkaure's Pyramid"   },
            { 33, "The Mastabas"                },
            { 34, "The Great Pyramid"           },
            { 35, "Khufu's Queens Pyramids"     },
            { 36, "Inside the Great Pyramid"    },
            { 37, "Temple of Horus"             },
            { 38, "Temple of Horus"             },
            { 40, "The Times Exclusive"         },
        };

        public static readonly Dictionary<byte, string> TR5 = new Dictionary<byte, string>()
        {
            {  1, "Streets of Rome"             },
            {  2, "Trajan's Markets"            },
            {  3, "The Colosseum"               },
            {  4, "The Base"                    },
            {  5, "The Submarine"               },
            {  6, "Deepsea Dive"                },
            {  7, "Sinking Submarine"           },
            {  8, "Gallows Tree"                },
            {  9, "Labyrinth"                   },
            { 10, "Old Mill"                    },
            { 11, "The 13th Floor"              },
            { 12, "Escape with the Iris"        },
            { 14, "Red Alert!"                  },
        };

        public static readonly Dictionary<byte, string> TR6 = new Dictionary<byte, string>()
        {
            {  0, "Parisian Back Streets"       },
            {  1, "Derelict Apartment Block"    },
            {  2, "Margot Carvier's Apartment"  },
            {  3, "Industrial Roof Tops"        },
            {  4, "Parisian Ghetto"             },
            {  5, "Parisian Ghetto"             },
            {  6, "Parisian Ghetto"             },
            {  7, "The Serpent Rouge"           },
            {  8, "Rennes' Pawnshop"            },
            {  9, "Willowtree Herbalist"        },
            { 10, "St. Aicard's Church"         },
            { 11, "Café Metro"                  },
            { 12, "St. Aicard's Graveyard"      },
            { 13, "Bouchard's Hideout"          },
            { 14, "Louvre Storm Drains"         },
            { 15, "Louvre Galleries"            },
            { 16, "Galleries Under Siege"       },
            { 17, "Tomb of Ancients"            },
            { 18, "The Archaeological Dig"      },
            { 19, "Von Croy's Apartment"        },
            { 20, "The Monstrum Crimescene"     },
            { 21, "The Strahov Fortress"        },
            { 22, "The Bio-Research Facility"   },
            { 23, "Aquatic Research Area"       },
            { 24, "The Sanitarium"              },
            { 25, "Maximum Containment Area"    },
            { 26, "The Vault of Trophies"       },
            { 27, "Boaz Returns"                },
            { 28, "Eckhardt's Lab"              },
            { 29, "The Lost Domain"             },
            { 30, "The Hall of Seasons"         },
            { 31, "Neptune's Hall"              },
            { 32, "Wrath of the Beast"          },
            { 33, "The Sanctuary of Flame"      },
            { 34, "The Breath of Hades"         },
        };
    }
}
