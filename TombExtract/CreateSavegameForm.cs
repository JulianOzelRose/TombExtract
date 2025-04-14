using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TombExtract;

namespace TombExtract
{
    public partial class CreateSavegameForm : Form
    {
        int CURRENT_TAB;
        private const int TAB_TR1 = 0;
        private const int TAB_TR2 = 1;
        private const int TAB_TR3 = 2;
        private const int TAB_TR4 = 3;
        private const int TAB_TR5 = 4;
        private const int TAB_TR6 = 5;

        int SLOT_NUMBER;
        int SAVE_NUMBER_OFFSET;
        int savegameOffset;
        string savegamePath;

        private List<LevelInfo> levelSelectionList = new List<LevelInfo>();
        private Dictionary<byte, Dictionary<GameMode, Dictionary<string, string>>> premadeBuffers;

        private const string PLATFORM_PC = "PC";
        private const string PLATFORM_PS4_SWITCH = "PS4_SWITCH";

        private ToolStripStatusLabel slblStatus;

        private class LevelInfo
        {
            public byte Index { get; set; }
            public string Name { get; set; }

            public LevelInfo(byte index, string name)
            {
                Index = index;
                Name = name;
            }

            public override string ToString()
            {
                return $"{Name}";
            }
        }

        public CreateSavegameForm(int selectedTab, string savegamePath, int slotNumber, int savegameOffset, ToolStripStatusLabel slblStatus)
        {
            InitializeComponent();

            this.CURRENT_TAB = selectedTab;
            this.SLOT_NUMBER = slotNumber;
            this.savegamePath = savegamePath;
            this.savegameOffset = savegameOffset;
            this.slblStatus = slblStatus;

            string gameSuffix = "";

            if (CURRENT_TAB == TAB_TR1)
            {
                gameSuffix = "Tomb Raider I";
                SAVE_NUMBER_OFFSET = 0x00C;
            }
            else if (CURRENT_TAB == TAB_TR2)
            {
                gameSuffix = "Tomb Raider II";
                SAVE_NUMBER_OFFSET = 0x00C;
            }
            else if (CURRENT_TAB == TAB_TR3)
            {
                gameSuffix = "Tomb Raider III";
                SAVE_NUMBER_OFFSET = 0x00C;
            }
            else if (CURRENT_TAB == TAB_TR4)
            {
                gameSuffix = "Tomb Raider IV";
                SAVE_NUMBER_OFFSET = 0x008;
            }
            else if (CURRENT_TAB == TAB_TR5)
            {
                gameSuffix = "Tomb Raider V";
                SAVE_NUMBER_OFFSET = 0x008;
            }
            else if (CURRENT_TAB == TAB_TR6)
            {
                gameSuffix = "Tomb Raider VI";
                SAVE_NUMBER_OFFSET = 0x11C;
            }

            this.Text = $"Create Savegame - {gameSuffix}";

            cmbMode.SelectedIndex = 0;      // Default to Normal
            cmbPlatform.SelectedIndex = 0;  // Default to PC

            PopulateLevelSelectionList();
            InitializePremadeBuffers();
            EnableDropdownsConditionally();
        }

        private void PopulateLevelSelectionList()
        {
            Dictionary<byte, string> selectedDict = null;

            if (CURRENT_TAB == TAB_TR1)
            {
                selectedDict = levelNamesTR1;
            }
            else if (CURRENT_TAB == TAB_TR2)
            {
                selectedDict = levelNamesTR2;
            }
            else if (CURRENT_TAB == TAB_TR3)
            {
                selectedDict = levelNamesTR3;
            }
            else if (CURRENT_TAB == TAB_TR4)
            {
                selectedDict = levelNamesTR4;
            }
            else if (CURRENT_TAB == TAB_TR5)
            {
                selectedDict = levelNamesTR5;
            }
            else if (CURRENT_TAB == TAB_TR6)
            {
                selectedDict = levelNamesTR6;
            }

            if (selectedDict == null)
            {
                return;
            }

            levelSelectionList.Clear();

            foreach (var kvp in selectedDict)
            {
                levelSelectionList.Add(new LevelInfo(kvp.Key, kvp.Value));
            }

            // Populate ComboBox
            cmbLevel.DataSource = null;
            cmbLevel.DataSource = levelSelectionList;
            cmbLevel.DisplayMember = "Name";
            cmbLevel.ValueMember = "Index";

            cmbLevel.SelectedIndex = 0;
        }

        private readonly Dictionary<byte, string> levelNamesTR1 = new Dictionary<byte, string>()
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

        private readonly Dictionary<byte, string> levelNamesTR2 = new Dictionary<byte, string>()
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

        private readonly Dictionary<byte, string> levelNamesTR3 = new Dictionary<byte, string>()
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

        private readonly Dictionary<byte, string> levelNamesTR4 = new Dictionary<byte, string>()
        {
            {  1, "Angkor Wat"                      },
            {  2, "Race for the Iris"               },
            {  3, "The Tomb of Seth"                },
            {  4, "Burial Chambers"                 },
            {  5, "Valley of the Kings"             },
            {  6, "KV5"                             },
            {  7, "Temple of Karnak"                },
            {  8, "The Great Hypostyle Hall"        },
            {  9, "Sacred Lake"                     },
            { 11, "Tomb of Semerkhet"               },
            { 12, "Guardian of Semerkhet"           },
            { 13, "Desert Railroad"                 },
            { 14, "Alexandria"                      },
            { 15, "Coastal Ruins"                   },
            { 16, "Pharos, Temple of Isis"          },
            { 17, "Cleopatra's Palaces"             },
            { 18, "Catacombs"                       },
            { 19, "Temple of Poseidon"              },
            { 20, "The Lost Library"                },
            { 21, "Hall of Demetrius"               },
            { 22, "City of the Dead"                },
            { 23, "Trenches"                        },
            { 24, "Chambers of Tulun"               },
            { 25, "Street Bazaar"                   },
            { 26, "Citadel Gate"                    },
            { 27, "Citadel"                         },
            { 28, "The Sphinx Complex"              },
            { 30, "Underneath the Sphinx"           },
            { 31, "Menkaure's Pyramid"              },
            { 32, "Inside Menkaure's Pyramid"       },
            { 33, "The Mastabas"                    },
            { 34, "The Great Pyramid"               },
            { 35, "Khufu's Queens Pyramids"         },
            { 36, "Inside the Great Pyramid"        },
            { 37, "Temple of Horus"                 },
            { 38, "Temple of Horus"                 },
            { 39, "The Times Office"                },
            { 40, "The Times Exclusive"             },
        };

        private readonly Dictionary<byte, string> levelNamesTR5 = new Dictionary<byte, string>()
        {
            {  1, "Streets of Rome"                      },
            {  2, "Trajan's Markets"                     },
            {  3, "The Colosseum"                        },
            {  4, "The Base"                             },
            {  5, "The Submarine"                        },
            {  6, "Deepsea Dive"                         },
            {  7, "Sinking Submarine"                    },
            {  8, "Gallows Tree"                         },
            {  9, "Labyrinth"                            },
            { 10, "Old Mill"                             },
            { 11, "The 13th Floor"                       },
            { 12, "Escape with the Iris"                 },
            { 14, "Red Alert!"                           },
        };

        private readonly Dictionary<byte, string> levelNamesTR6 = new Dictionary<byte, string>()
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

        private void InitializePremadeBuffers()
        {
            if (CURRENT_TAB == TAB_TR1)
            {
                premadeBuffers = new Dictionary<byte, Dictionary<GameMode, Dictionary<string, string>>>()
                {
                    { 1, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR1_CAVES_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR1_CAVES_NORMAL_PS4_SWITCH.bin" }
                                }
                            }
                            // Add GameMode.Plus
                        }
                    },
                    { 2, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR1_CITY_OF_VILCABAMBA_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR1_CITY_OF_VILCABAMBA_NORMAL_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 3, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR1_LOST_VALLEY_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR1_LOST_VALLEY_NORMAL_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 4, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR1_TOMB_OF_QUALOPEC_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR1_TOMB_OF_QUALOPEC_NORMAL_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 5, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR1_ST_FRANCIS_FOLLY_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR1_ST_FRANCIS_FOLLY_NORMAL_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 6, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR1_COLOSSEUM_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR1_COLOSSEUM_NORMAL_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 7, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR1_PALACE_MIDAS_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR1_PALACE_MIDAS_NORMAL_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 8, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR1_THE_CISTERN_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR1_THE_CISTERN_NORMAL_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 9, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR1_TOMB_OF_TIHOCAN_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR1_TOMB_OF_TIHOCAN_NORMAL_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 10, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR1_CITY_OF_KHAMOON_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR1_CITY_OF_KHAMOON_NORMAL_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 11, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR1_OBELISK_OF_KHAMOON_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR1_OBELISK_OF_KHAMOON_NORMAL_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 12, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR1_SANCTUARY_OF_THE_SCION_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR1_SANCTUARY_OF_THE_SCION_NORMAL_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 13, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR1_NATLAS_MINES_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR1_NATLAS_MINES_NORMAL_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 14, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR1_ATLANTIS_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR1_ATLANTIS_NORMAL_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 15, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR1_THE_GREAT_PYRAMID_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR1_THE_GREAT_PYRAMID_NORMAL_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 16, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR1_RETURN_TO_EGYPT_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR1_RETURN_TO_EGYPT_NORMAL_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 17, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR1_TEMPLE_OF_THE_CAT_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR1_TEMPLE_OF_THE_CAT_NORMAL_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 18, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR1_ATLANTEAN_STRONGHOLD_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR1_ATLANTEAN_STRONGHOLD_NORMAL_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 19, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR1_THE_HIVE_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR1_THE_HIVE_NORMAL_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                };
            }
            else if (CURRENT_TAB == TAB_TR2)
            {
                premadeBuffers = new Dictionary<byte, Dictionary<GameMode, Dictionary<string, string>>>()
                {
                    { 1, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_THE_GREAT_WALL_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_THE_GREAT_WALL_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_THE_GREAT_WALL_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_THE_GREAT_WALL_NGPLUS_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 2, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_VENICE_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_VENICE_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_VENICE_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_VENICE_NGPLUS_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 3, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_BARTOLIS_HIDEOUT_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_BARTOLIS_HIDEOUT_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_BARTOLIS_HIDEOUT_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_BARTOLIS_HIDEOUT_NGPLUS_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 4, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_OPERA_HOUSE_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_OPERA_HOUSE_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_OPERA_HOUSE_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_OPERA_HOUSE_NGPLUS_PS4_SWITCH.bin" }
                                }
                            }
                        }
                    },
                    { 5, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_OFFSHORE_RIG_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_OFFSHORE_RIG_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_OFFSHORE_RIG_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_OFFSHORE_RIG_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 6, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_DIVING_AREA_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_DIVING_AREA_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_DIVING_AREA_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_DIVING_AREA_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 7, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_40_FATHOMS_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_40_FATHOMS_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_40_FATHOMS_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_40_FATHOMS_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 8, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_WRECK_OF_THE_MARIA_DORIA_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_WRECK_OF_THE_MARIA_DORIA_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_WRECK_OF_THE_MARIA_DORIA_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_WRECK_OF_THE_MARIA_DORIA_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 9, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_LIVING_QUARTERS_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_LIVING_QUARTERS_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_LIVING_QUARTERS_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_LIVING_QUARTERS_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 10, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_THE_DECK_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_THE_DECK_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_THE_DECK_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_THE_DECK_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 11, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_TIBETAN_FOOTHILLS_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_TIBETAN_FOOTHILLS_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_TIBETAN_FOOTHILLS_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_TIBETAN_FOOTHILLS_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 12, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_BARKHANG_MONASTERY_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_BARKHANG_MONASTERY_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_BARKHANG_MONASTERY_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_BARKHANG_MONASTERY_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 13, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_CATACOMBS_OF_THE_TALION_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_CATACOMBS_OF_THE_TALION_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_CATACOMBS_OF_THE_TALION_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_CATACOMBS_OF_THE_TALION_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 14, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_ICE_PALACE_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_ICE_PALACE_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_ICE_PALACE_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_ICE_PALACE_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 15, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_TEMPLE_OF_XIAN_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_TEMPLE_OF_XIAN_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_TEMPLE_OF_XIAN_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_TEMPLE_OF_XIAN_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 16, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_FLOATING_ISLANDS_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_FLOATING_ISLANDS_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_FLOATING_ISLANDS_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_FLOATING_ISLANDS_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 17, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_THE_DRAGONS_LAIR_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_THE_DRAGONS_LAIR_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_THE_DRAGONS_LAIR_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_THE_DRAGONS_LAIR_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 18, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_HOME_SWEET_HOME_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_HOME_SWEET_HOME_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_HOME_SWEET_HOME_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_HOME_SWEET_HOME_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 19, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_THE_COLD_WAR_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_THE_COLD_WAR_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 20, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_FOOLS_GOLD_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_FOOLS_GOLD_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 21, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_FURNACE_OF_THE_GODS_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_FURNACE_OF_THE_GODS_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 22, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_KINGDOM_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_KINGDOM_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 23, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR2_NIGHTMARE_IN_VEGAS_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR2_NIGHTMARE_IN_VEGAS_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                };
            }
            else if (CURRENT_TAB == TAB_TR3)
            {
                premadeBuffers = new Dictionary<byte, Dictionary<GameMode, Dictionary<string, string>>>()
                {
                    { 1, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_JUNGLE_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_JUNGLE_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_JUNGLE_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_JUNGLE_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 2, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_TEMPLE_RUINS_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_TEMPLE_RUINS_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_TEMPLE_RUINS_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_TEMPLE_RUINS_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 3, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_THE_RIVER_GANGES_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_THE_RIVER_GANGES_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_THE_RIVER_GANGES_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_THE_RIVER_GANGES_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 4, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_CAVES_OF_KALIYA_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_CAVES_OF_KALIYA_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_CAVES_OF_KALIYA_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_CAVES_OF_KALIYA_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 5, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_COASTAL_VILLAGE_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_COASTAL_VILLAGE_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_COASTAL_VILLAGE_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_COASTAL_VILLAGE_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 6, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_CRASH_SITE_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_CRASH_SITE_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_CRASH_SITE_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_CRASH_SITE_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 7, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_MADUBU_GORGE_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_MADUBU_GORGE_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_MADUBU_GORGE_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_MADUBU_GORGE_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 8, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_TEMPLE_OF_PUNA_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_TEMPLE_OF_PUNA_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_TEMPLE_OF_PUNA_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_TEMPLE_OF_PUNA_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 9, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_THAMES_WHARF_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_THAMES_WHARF_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_THAMES_WHARF_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_THAMES_WHARF_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 10, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_ALDWYCH_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_ALDWYCH_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_ALDWYCH_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_ALDWYCH_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 11, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_LUDS_GATE_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_LUDS_GATE_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_LUDS_GATE_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_LUDS_GATE_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 12, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_CITY_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_CITY_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_CITY_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_CITY_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 13, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_NEVADA_DESERT_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_NEVADA_DESERT_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_NEVADA_DESERT_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_NEVADA_DESERT_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 14, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_HIGH_SECURITY_COMPOUND_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_HIGH_SECURITY_COMPOUND_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_HIGH_SECURITY_COMPOUND_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_HIGH_SECURITY_COMPOUND_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 15, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_AREA_51_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_AREA_51_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_AREA_51_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_AREA_51_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 16, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_ANTARCTICA_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_ANTARCTICA_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_ANTARCTICA_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_ANTARCTICA_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 17, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_RX_TECH_MINES_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_RX_TECH_MINES_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_RX_TECH_MINES_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_RX_TECH_MINES_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 18, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_LOST_CITY_OF_TINNOS_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_LOST_CITY_OF_TINNOS_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_LOST_CITY_OF_TINNOS_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_LOST_CITY_OF_TINNOS_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 19, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_METEORITE_CAVERN_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_METEORITE_CAVERN_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                            { GameMode.Plus, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_METEORITE_CAVERN_NGPLUS_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_METEORITE_CAVERN_NGPLUS_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 20, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_ALL_HALLOWS_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_ALL_HALLOWS_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 21, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_HIGHLAND_FLING_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_HIGHLAND_FLING_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 22, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_WILLARDS_LAIR_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_WILLARDS_LAIR_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 23, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_SHAKESPEARE_CLIFF_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_SHAKESPEARE_CLIFF_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 24, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_SLEEPING_WITH_THE_FISHES_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_SLEEPING_WITH_THE_FISHES_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 25, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_ITS_A_MADHOUSE_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_ITS_A_MADHOUSE_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                    { 26, new Dictionary<GameMode, Dictionary<string, string>>()
                        {
                            { GameMode.Normal, new Dictionary<string, string>()
                                {
                                    { PLATFORM_PC, "TombExtract.Resources.PremadeSavegames.TR3_REUNION_NORMAL_PC.bin" },
                                    { PLATFORM_PS4_SWITCH, "TombExtract.Resources.PremadeSavegames.TR3_REUNION_NORMAL_PS4_SWITCH.bin" }
                                }
                            },
                        }
                    },
                };
            }
            else if (CURRENT_TAB == TAB_TR4)
            {
                premadeBuffers = new Dictionary<byte, Dictionary<GameMode, Dictionary<string, string>>>()
                {
                    // TODO: Add TR4 buffers
                };
            }
            else if (CURRENT_TAB == TAB_TR5)
            {
                premadeBuffers = new Dictionary<byte, Dictionary<GameMode, Dictionary<string, string>>>()
                {
                    // TODO: Add TR5 buffers
                };
            }
            else if (CURRENT_TAB == TAB_TR6)
            {
                premadeBuffers = new Dictionary<byte, Dictionary<GameMode, Dictionary<string, string>>>()
                {
                    // TODO: Add TR6 buffers
                };
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private byte[] LoadPremadeBuffer(string resourceName)
        {
            using (var stream = typeof(CreateSavegameForm).Assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    throw new Exception($"Missing embedded resource: {resourceName}");

                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedLevel = (LevelInfo)cmbLevel.SelectedItem;
                if (selectedLevel == null)
                {
                    MessageBox.Show("Please select a level.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                GameMode selectedMode = (GameMode)cmbMode.SelectedIndex;

                // Resolve platform string based on selection
                string selectedPlatform;
                switch (cmbPlatform.SelectedIndex)
                {
                    case 0:
                        selectedPlatform = PLATFORM_PC;
                        break;
                    case 1:
                    case 2:
                        selectedPlatform = PLATFORM_PS4_SWITCH;
                        break;
                    default:
                        selectedPlatform = PLATFORM_PC;
                        break;
                }

                // Declarations using correct nested types
                Dictionary<GameMode, Dictionary<string, string>> modeDict;
                Dictionary<string, string> platformDict;
                string resourceName;

                if (!premadeBuffers.TryGetValue(selectedLevel.Index, out modeDict) ||
                    !modeDict.TryGetValue(selectedMode, out platformDict) ||
                    !platformDict.TryGetValue(selectedPlatform, out resourceName))
                {
                    MessageBox.Show("Premade savegame buffer not found for this level/mode/platform.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Load the premade save buffer
                byte[] buffer = LoadPremadeBuffer(resourceName);

                // Write the save buffer to the correct offset
                using (var fs = new FileStream(savegamePath, FileMode.Open, FileAccess.Write))
                {
                    // Write the main buffer
                    fs.Seek(savegameOffset, SeekOrigin.Begin);
                    fs.Write(buffer, 0, buffer.Length);

                    // Inject the save number as Int32 at known offset
                    fs.Seek(savegameOffset + SAVE_NUMBER_OFFSET, SeekOrigin.Begin);
                    fs.Write(BitConverter.GetBytes((int)nudSaveNumber.Value), 0, 4);
                }

                MessageBox.Show("Savegame created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create savegame:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnableDropdownsConditionally()
        {
            if (cmbLevel.SelectedItem is LevelInfo selectedLevel &&
                premadeBuffers.TryGetValue(selectedLevel.Index, out var modeDict))
            {
                cmbMode.Enabled = modeDict.Count > 1;

                GameMode selectedMode = (GameMode)cmbMode.SelectedIndex;

                if (modeDict.TryGetValue(selectedMode, out var platformDict))
                {
                    cmbPlatform.Enabled = platformDict.Count > 1;

                    // If current selection is out of range, reset it
                    if (cmbPlatform.SelectedIndex < 0 || cmbPlatform.SelectedIndex >= cmbPlatform.Items.Count)
                    {
                        cmbPlatform.SelectedIndex = 0;
                    }
                }
                else
                {
                    cmbPlatform.Enabled = false;
                }
            }
            else
            {
                cmbMode.Enabled = false;
                cmbPlatform.Enabled = false;
            }
        }

        private void cmbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Reset Platform/Mode
            cmbMode.SelectedIndex = 0;
            cmbPlatform.SelectedIndex = 0;

            EnableDropdownsConditionally();
        }

        private void cmbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLevel.SelectedItem is LevelInfo selectedLevel &&
                premadeBuffers.TryGetValue(selectedLevel.Index, out var modeDict) &&
                modeDict.TryGetValue((GameMode)cmbMode.SelectedIndex, out var platformDict))
            {
                cmbPlatform.SelectedIndex = 0;
            }

            EnableDropdownsConditionally();
        }
    }
}
