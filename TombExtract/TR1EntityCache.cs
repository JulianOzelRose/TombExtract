using System.Collections.Generic;

namespace TombExtract
{
    public class TR1Object
    {
        public int ObjectId { get; set; }
        public byte Flags00 { get; set; }
        public string Name { get; set; } = "";
    }

    public class TR1EntityCache
    {
        public static readonly Dictionary<int, int> FixedCameraCounts = new Dictionary<int, int>()
        {
            { 1,  2  },     // Caves
            { 2,  6  },     // City of Vilcabamba
            { 3,  6  },     // Lost Valley
            { 4,  10 },     // Tomb of Qualopec
            { 5,  14 },     // St. Francis' Folly
            { 6,  10 },     // Colosseum
            { 7,  7  },     // Palace Midas
            { 8,  7  },     // The Cistern
            { 9,  15 },     // Tomb of Tihocan
            { 10, 3  },     // City of Khamoon
            { 11, 8  },     // Obelisk of Khamoon
            { 12, 12 },     // Sanctuary of the Scion
            { 13, 15 },     // Natla's Mines
            { 14, 6  },     // Atlantis
            { 15, 2  },     // The Great Pyramid
            { 16, 15 },     // Return to Egypt
            { 17, 30 },     // Temple of the Cat
            { 18, 24 },     // Atlantean Stronghold
            { 19, 14 },     // The Hive
        };

        public static readonly Dictionary<int, Dictionary<int, TR1Object>> TR1ObjectsByLevel = new Dictionary<int, Dictionary<int, TR1Object>>
        {
            [1] = new Dictionary<int, TR1Object> // Caves
            {
                [0] = new TR1Object
                {
                    ObjectId = 0,
                    Flags00 = 0x79,
                },
                [1] = new TR1Object
                {
                    ObjectId = 1,
                    Flags00 = 0x01,
                },
                [2] = new TR1Object
                {
                    ObjectId = 2,
                    Flags00 = 0x01,
                },
                [3] = new TR1Object
                {
                    ObjectId = 3,
                    Flags00 = 0x01,
                },
                [4] = new TR1Object
                {
                    ObjectId = 4,
                    Flags00 = 0x01,
                },
                [5] = new TR1Object
                {
                    ObjectId = 5,
                    Flags00 = 0x01,
                },
                [6] = new TR1Object
                {
                    ObjectId = 6,
                    Flags00 = 0x79,
                },
                [7] = new TR1Object
                {
                    ObjectId = 7,
                    Flags00 = 0x7B,
                },
                [8] = new TR1Object
                {
                    ObjectId = 8,
                    Flags00 = 0x7B,
                },
                [9] = new TR1Object
                {
                    ObjectId = 9,
                    Flags00 = 0x7B,
                },
                [10] = new TR1Object
                {
                    ObjectId = 10,
                    Flags00 = 0x7B,
                },
                [11] = new TR1Object
                {
                    ObjectId = 11,
                    Flags00 = 0x7B,
                },
                [12] = new TR1Object
                {
                    ObjectId = 12,
                    Flags00 = 0x7B,
                },
                [13] = new TR1Object
                {
                    ObjectId = 13,
                    Flags00 = 0x7B,
                },
                [14] = new TR1Object
                {
                    ObjectId = 14,
                    Flags00 = 0x7B,
                },
                [15] = new TR1Object
                {
                    ObjectId = 15,
                    Flags00 = 0x7B,
                },
                [16] = new TR1Object
                {
                    ObjectId = 16,
                    Flags00 = 0x7B,
                },
                [17] = new TR1Object
                {
                    ObjectId = 17,
                    Flags00 = 0x7B,
                },
                [18] = new TR1Object
                {
                    ObjectId = 18,
                    Flags00 = 0x7B,
                },
                [19] = new TR1Object
                {
                    ObjectId = 19,
                    Flags00 = 0x7B,
                },
                [20] = new TR1Object
                {
                    ObjectId = 20,
                    Flags00 = 0x7B,
                },
                [21] = new TR1Object
                {
                    ObjectId = 21,
                    Flags00 = 0x7B,
                },
                [22] = new TR1Object
                {
                    ObjectId = 22,
                    Flags00 = 0x7B,
                },
                [23] = new TR1Object
                {
                    ObjectId = 23,
                    Flags00 = 0x7B,
                },
                [24] = new TR1Object
                {
                    ObjectId = 24,
                    Flags00 = 0x71,
                },
                [25] = new TR1Object
                {
                    ObjectId = 25,
                    Flags00 = 0x00,
                },
                [26] = new TR1Object
                {
                    ObjectId = 26,
                    Flags00 = 0x00,
                },
                [27] = new TR1Object
                {
                    ObjectId = 27,
                    Flags00 = 0x7B,
                },
                [28] = new TR1Object
                {
                    ObjectId = 28,
                    Flags00 = 0x7B,
                },
                [29] = new TR1Object
                {
                    ObjectId = 29,
                    Flags00 = 0x01,
                },
                [30] = new TR1Object
                {
                    ObjectId = 30,
                    Flags00 = 0x7B,
                },
                [31] = new TR1Object
                {
                    ObjectId = 31,
                    Flags00 = 0x7B,
                },
                [32] = new TR1Object
                {
                    ObjectId = 32,
                    Flags00 = 0x7B,
                },
                [33] = new TR1Object
                {
                    ObjectId = 33,
                    Flags00 = 0x7B,
                },
                [34] = new TR1Object
                {
                    ObjectId = 34,
                    Flags00 = 0x7B,
                },
                [35] = new TR1Object
                {
                    ObjectId = 35,
                    Flags00 = 0x69,
                },
                [36] = new TR1Object
                {
                    ObjectId = 36,
                    Flags00 = 0x61,
                },
                [37] = new TR1Object
                {
                    ObjectId = 37,
                    Flags00 = 0x00,
                },
                [38] = new TR1Object
                {
                    ObjectId = 38,
                    Flags00 = 0x68,
                },
                [39] = new TR1Object
                {
                    ObjectId = 39,
                    Flags00 = 0x21,
                },
                [40] = new TR1Object
                {
                    ObjectId = 40,
                    Flags00 = 0x21,
                },
                [41] = new TR1Object
                {
                    ObjectId = 41,
                    Flags00 = 0x00,
                },
                [42] = new TR1Object
                {
                    ObjectId = 42,
                    Flags00 = 0x60,
                },
                [43] = new TR1Object
                {
                    ObjectId = 43,
                    Flags00 = 0x68,
                },
                [44] = new TR1Object
                {
                    ObjectId = 44,
                    Flags00 = 0x60,
                },
                [45] = new TR1Object
                {
                    ObjectId = 45,
                    Flags00 = 0x60,
                },
                [46] = new TR1Object
                {
                    ObjectId = 46,
                    Flags00 = 0x20,
                },
                [47] = new TR1Object
                {
                    ObjectId = 47,
                    Flags00 = 0x68,
                },
                [48] = new TR1Object
                {
                    ObjectId = 48,
                    Flags00 = 0x68,
                },
                [49] = new TR1Object
                {
                    ObjectId = 49,
                    Flags00 = 0x68,
                },
                [50] = new TR1Object
                {
                    ObjectId = 50,
                    Flags00 = 0x68,
                },
                [51] = new TR1Object
                {
                    ObjectId = 51,
                    Flags00 = 0x68,
                },
                [52] = new TR1Object
                {
                    ObjectId = 52,
                    Flags00 = 0x68,
                },
                [53] = new TR1Object
                {
                    ObjectId = 53,
                    Flags00 = 0x68,
                },
                [54] = new TR1Object
                {
                    ObjectId = 54,
                    Flags00 = 0x68,
                },
                [55] = new TR1Object
                {
                    ObjectId = 55,
                    Flags00 = 0x61,
                },
                [56] = new TR1Object
                {
                    ObjectId = 56,
                    Flags00 = 0x60,
                },
                [57] = new TR1Object
                {
                    ObjectId = 57,
                    Flags00 = 0x61,
                },
                [58] = new TR1Object
                {
                    ObjectId = 58,
                    Flags00 = 0x61,
                },
                [59] = new TR1Object
                {
                    ObjectId = 59,
                    Flags00 = 0x61,
                },
                [60] = new TR1Object
                {
                    ObjectId = 60,
                    Flags00 = 0x61,
                },
                [61] = new TR1Object
                {
                    ObjectId = 61,
                    Flags00 = 0x60,
                },
                [62] = new TR1Object
                {
                    ObjectId = 62,
                    Flags00 = 0x60,
                },
                [63] = new TR1Object
                {
                    ObjectId = 63,
                    Flags00 = 0x60,
                },
                [64] = new TR1Object
                {
                    ObjectId = 64,
                    Flags00 = 0x60,
                },
                [65] = new TR1Object
                {
                    ObjectId = 65,
                    Flags00 = 0x60,
                },
                [66] = new TR1Object
                {
                    ObjectId = 66,
                    Flags00 = 0x60,
                },
                [67] = new TR1Object
                {
                    ObjectId = 67,
                    Flags00 = 0x00,
                },
                [68] = new TR1Object
                {
                    ObjectId = 68,
                    Flags00 = 0x01,
                },
                [69] = new TR1Object
                {
                    ObjectId = 69,
                    Flags00 = 0x01,
                },
                [70] = new TR1Object
                {
                    ObjectId = 70,
                    Flags00 = 0x01,
                },
                [71] = new TR1Object
                {
                    ObjectId = 71,
                    Flags00 = 0x01,
                },
                [72] = new TR1Object
                {
                    ObjectId = 72,
                    Flags00 = 0x01,
                },
                [73] = new TR1Object
                {
                    ObjectId = 73,
                    Flags00 = 0x00,
                },
                [74] = new TR1Object
                {
                    ObjectId = 74,
                    Flags00 = 0x20,
                },
                [75] = new TR1Object
                {
                    ObjectId = 75,
                    Flags00 = 0x20,
                },
                [76] = new TR1Object
                {
                    ObjectId = 76,
                    Flags00 = 0x20,
                },
                [77] = new TR1Object
                {
                    ObjectId = 77,
                    Flags00 = 0x00,
                },
                [78] = new TR1Object
                {
                    ObjectId = 78,
                    Flags00 = 0x00,
                },
                [79] = new TR1Object
                {
                    ObjectId = 79,
                    Flags00 = 0x00,
                },
                [80] = new TR1Object
                {
                    ObjectId = 80,
                    Flags00 = 0x00,
                },
                [81] = new TR1Object
                {
                    ObjectId = 81,
                    Flags00 = 0x01,
                },
                [82] = new TR1Object
                {
                    ObjectId = 82,
                    Flags00 = 0x01,
                },
                [83] = new TR1Object
                {
                    ObjectId = 83,
                    Flags00 = 0x21,
                },
                [84] = new TR1Object
                {
                    ObjectId = 84,
                    Flags00 = 0x21,
                },
                [85] = new TR1Object
                {
                    ObjectId = 85,
                    Flags00 = 0x21,
                },
                [86] = new TR1Object
                {
                    ObjectId = 86,
                    Flags00 = 0x21,
                },
                [87] = new TR1Object
                {
                    ObjectId = 87,
                    Flags00 = 0x21,
                },
                [88] = new TR1Object
                {
                    ObjectId = 88,
                    Flags00 = 0x20,
                },
                [89] = new TR1Object
                {
                    ObjectId = 89,
                    Flags00 = 0x21,
                },
                [90] = new TR1Object
                {
                    ObjectId = 90,
                    Flags00 = 0x21,
                },
                [91] = new TR1Object
                {
                    ObjectId = 91,
                    Flags00 = 0x21,
                },
                [92] = new TR1Object
                {
                    ObjectId = 92,
                    Flags00 = 0x20,
                },
                [93] = new TR1Object
                {
                    ObjectId = 93,
                    Flags00 = 0x21,
                },
                [94] = new TR1Object
                {
                    ObjectId = 94,
                    Flags00 = 0x21,
                },
                [95] = new TR1Object
                {
                    ObjectId = 95,
                    Flags00 = 0x01,
                },
                [96] = new TR1Object
                {
                    ObjectId = 96,
                    Flags00 = 0x01,
                },
                [97] = new TR1Object
                {
                    ObjectId = 97,
                    Flags00 = 0x01,
                },
                [98] = new TR1Object
                {
                    ObjectId = 98,
                    Flags00 = 0x00,
                },
                [99] = new TR1Object
                {
                    ObjectId = 99,
                    Flags00 = 0x01,
                },
                [100] = new TR1Object
                {
                    ObjectId = 100,
                    Flags00 = 0x01,
                },
                [101] = new TR1Object
                {
                    ObjectId = 101,
                    Flags00 = 0x01,
                },
                [102] = new TR1Object
                {
                    ObjectId = 102,
                    Flags00 = 0x01,
                },
                [103] = new TR1Object
                {
                    ObjectId = 103,
                    Flags00 = 0x01,
                },
                [104] = new TR1Object
                {
                    ObjectId = 104,
                    Flags00 = 0x01,
                },
                [105] = new TR1Object
                {
                    ObjectId = 105,
                    Flags00 = 0x01,
                },
                [106] = new TR1Object
                {
                    ObjectId = 106,
                    Flags00 = 0x01,
                },
                [107] = new TR1Object
                {
                    ObjectId = 107,
                    Flags00 = 0x00,
                },
                [108] = new TR1Object
                {
                    ObjectId = 108,
                    Flags00 = 0x01,
                },
                [109] = new TR1Object
                {
                    ObjectId = 109,
                    Flags00 = 0x01,
                },
                [110] = new TR1Object
                {
                    ObjectId = 110,
                    Flags00 = 0x20,
                },
                [111] = new TR1Object
                {
                    ObjectId = 111,
                    Flags00 = 0x20,
                },
                [112] = new TR1Object
                {
                    ObjectId = 112,
                    Flags00 = 0x20,
                },
                [113] = new TR1Object
                {
                    ObjectId = 113,
                    Flags00 = 0x20,
                },
                [114] = new TR1Object
                {
                    ObjectId = 114,
                    Flags00 = 0x00,
                },
                [115] = new TR1Object
                {
                    ObjectId = 115,
                    Flags00 = 0x00,
                },
                [116] = new TR1Object
                {
                    ObjectId = 116,
                    Flags00 = 0x00,
                },
                [117] = new TR1Object
                {
                    ObjectId = 117,
                    Flags00 = 0x00,
                },
                [118] = new TR1Object
                {
                    ObjectId = 118,
                    Flags00 = 0x20,
                },
                [119] = new TR1Object
                {
                    ObjectId = 119,
                    Flags00 = 0x20,
                },
                [120] = new TR1Object
                {
                    ObjectId = 120,
                    Flags00 = 0x20,
                },
                [121] = new TR1Object
                {
                    ObjectId = 121,
                    Flags00 = 0x20,
                },
                [122] = new TR1Object
                {
                    ObjectId = 122,
                    Flags00 = 0x20,
                },
                [123] = new TR1Object
                {
                    ObjectId = 123,
                    Flags00 = 0x20,
                },
                [124] = new TR1Object
                {
                    ObjectId = 124,
                    Flags00 = 0x20,
                },
                [125] = new TR1Object
                {
                    ObjectId = 125,
                    Flags00 = 0x20,
                },
                [126] = new TR1Object
                {
                    ObjectId = 126,
                    Flags00 = 0x20,
                },
                [127] = new TR1Object
                {
                    ObjectId = 127,
                    Flags00 = 0x00,
                },
                [128] = new TR1Object
                {
                    ObjectId = 128,
                    Flags00 = 0x00,
                },
                [129] = new TR1Object
                {
                    ObjectId = 129,
                    Flags00 = 0x20,
                },
                [130] = new TR1Object
                {
                    ObjectId = 130,
                    Flags00 = 0x20,
                },
                [131] = new TR1Object
                {
                    ObjectId = 131,
                    Flags00 = 0x20,
                },
                [132] = new TR1Object
                {
                    ObjectId = 132,
                    Flags00 = 0x20,
                },
                [133] = new TR1Object
                {
                    ObjectId = 133,
                    Flags00 = 0x00,
                },
                [134] = new TR1Object
                {
                    ObjectId = 134,
                    Flags00 = 0x00,
                },
                [135] = new TR1Object
                {
                    ObjectId = 135,
                    Flags00 = 0x00,
                },
                [136] = new TR1Object
                {
                    ObjectId = 136,
                    Flags00 = 0x00,
                },
                [137] = new TR1Object
                {
                    ObjectId = 137,
                    Flags00 = 0x20,
                },
                [138] = new TR1Object
                {
                    ObjectId = 138,
                    Flags00 = 0x20,
                },
                [139] = new TR1Object
                {
                    ObjectId = 139,
                    Flags00 = 0x20,
                },
                [140] = new TR1Object
                {
                    ObjectId = 140,
                    Flags00 = 0x20,
                },
                [141] = new TR1Object
                {
                    ObjectId = 141,
                    Flags00 = 0x20,
                },
                [142] = new TR1Object
                {
                    ObjectId = 142,
                    Flags00 = 0x20,
                },
                [143] = new TR1Object
                {
                    ObjectId = 143,
                    Flags00 = 0x21,
                },
                [144] = new TR1Object
                {
                    ObjectId = 144,
                    Flags00 = 0x20,
                },
                [145] = new TR1Object
                {
                    ObjectId = 145,
                    Flags00 = 0x20,
                },
                [146] = new TR1Object
                {
                    ObjectId = 146,
                    Flags00 = 0x20,
                },
                [147] = new TR1Object
                {
                    ObjectId = 147,
                    Flags00 = 0x60,
                },
                [148] = new TR1Object
                {
                    ObjectId = 148,
                    Flags00 = 0x00,
                },
                [149] = new TR1Object
                {
                    ObjectId = 149,
                    Flags00 = 0x00,
                },
                [150] = new TR1Object
                {
                    ObjectId = 150,
                    Flags00 = 0x01,
                },
                [151] = new TR1Object
                {
                    ObjectId = 151,
                    Flags00 = 0x00,
                },
                [152] = new TR1Object
                {
                    ObjectId = 152,
                    Flags00 = 0x00,
                },
                [153] = new TR1Object
                {
                    ObjectId = 153,
                    Flags00 = 0x01,
                },
                [154] = new TR1Object
                {
                    ObjectId = 154,
                    Flags00 = 0x00,
                },
                [155] = new TR1Object
                {
                    ObjectId = 155,
                    Flags00 = 0x01,
                },
                [156] = new TR1Object
                {
                    ObjectId = 156,
                    Flags00 = 0x01,
                },
                [157] = new TR1Object
                {
                    ObjectId = 157,
                    Flags00 = 0x00,
                },
                [158] = new TR1Object
                {
                    ObjectId = 158,
                    Flags00 = 0x01,
                },
                [159] = new TR1Object
                {
                    ObjectId = 159,
                    Flags00 = 0x00,
                },
                [160] = new TR1Object
                {
                    ObjectId = 160,
                    Flags00 = 0x01,
                },
                [161] = new TR1Object
                {
                    ObjectId = 161,
                    Flags00 = 0x00,
                },
                [162] = new TR1Object
                {
                    ObjectId = 162,
                    Flags00 = 0x60,
                },
                [163] = new TR1Object
                {
                    ObjectId = 163,
                    Flags00 = 0x00,
                },
                [164] = new TR1Object
                {
                    ObjectId = 164,
                    Flags00 = 0x01,
                },
                [165] = new TR1Object
                {
                    ObjectId = 165,
                    Flags00 = 0x00,
                },
                [166] = new TR1Object
                {
                    ObjectId = 166,
                    Flags00 = 0x01,
                },
                [167] = new TR1Object
                {
                    ObjectId = 167,
                    Flags00 = 0x00,
                },
                [168] = new TR1Object
                {
                    ObjectId = 168,
                    Flags00 = 0x01,
                },
                [169] = new TR1Object
                {
                    ObjectId = 169,
                    Flags00 = 0x01,
                },
                [170] = new TR1Object
                {
                    ObjectId = 170,
                    Flags00 = 0x20,
                },
            },
            [2] = new Dictionary<int, TR1Object> // City of Vilcabamba
            {
                [0] = new TR1Object
                {
                    ObjectId = 0,
                    Flags00 = 0x79,
                },
                [1] = new TR1Object
                {
                    ObjectId = 1,
                    Flags00 = 0x01,
                },
                [2] = new TR1Object
                {
                    ObjectId = 2,
                    Flags00 = 0x01,
                },
                [3] = new TR1Object
                {
                    ObjectId = 3,
                    Flags00 = 0x01,
                },
                [4] = new TR1Object
                {
                    ObjectId = 4,
                    Flags00 = 0x01,
                },
                [5] = new TR1Object
                {
                    ObjectId = 5,
                    Flags00 = 0x01,
                },
                [6] = new TR1Object
                {
                    ObjectId = 6,
                    Flags00 = 0x78,
                },
                [7] = new TR1Object
                {
                    ObjectId = 7,
                    Flags00 = 0x7B,
                },
                [8] = new TR1Object
                {
                    ObjectId = 8,
                    Flags00 = 0x7B,
                },
                [9] = new TR1Object
                {
                    ObjectId = 9,
                    Flags00 = 0x7B,
                },
                [10] = new TR1Object
                {
                    ObjectId = 10,
                    Flags00 = 0x7B,
                },
                [11] = new TR1Object
                {
                    ObjectId = 11,
                    Flags00 = 0x7B,
                },
                [12] = new TR1Object
                {
                    ObjectId = 12,
                    Flags00 = 0x7B,
                },
                [13] = new TR1Object
                {
                    ObjectId = 13,
                    Flags00 = 0x7B,
                },
                [14] = new TR1Object
                {
                    ObjectId = 14,
                    Flags00 = 0x7B,
                },
                [15] = new TR1Object
                {
                    ObjectId = 15,
                    Flags00 = 0x7B,
                },
                [16] = new TR1Object
                {
                    ObjectId = 16,
                    Flags00 = 0x7B,
                },
                [17] = new TR1Object
                {
                    ObjectId = 17,
                    Flags00 = 0x7B,
                },
                [18] = new TR1Object
                {
                    ObjectId = 18,
                    Flags00 = 0x7B,
                },
                [19] = new TR1Object
                {
                    ObjectId = 19,
                    Flags00 = 0x7B,
                },
                [20] = new TR1Object
                {
                    ObjectId = 20,
                    Flags00 = 0x7B,
                },
                [21] = new TR1Object
                {
                    ObjectId = 21,
                    Flags00 = 0x7B,
                },
                [22] = new TR1Object
                {
                    ObjectId = 22,
                    Flags00 = 0x7B,
                },
                [23] = new TR1Object
                {
                    ObjectId = 23,
                    Flags00 = 0x7B,
                },
                [24] = new TR1Object
                {
                    ObjectId = 24,
                    Flags00 = 0x71,
                },
                [25] = new TR1Object
                {
                    ObjectId = 25,
                    Flags00 = 0x00,
                },
                [26] = new TR1Object
                {
                    ObjectId = 26,
                    Flags00 = 0x00,
                },
                [27] = new TR1Object
                {
                    ObjectId = 27,
                    Flags00 = 0x7B,
                },
                [28] = new TR1Object
                {
                    ObjectId = 28,
                    Flags00 = 0x7B,
                },
                [29] = new TR1Object
                {
                    ObjectId = 29,
                    Flags00 = 0x01,
                },
                [30] = new TR1Object
                {
                    ObjectId = 30,
                    Flags00 = 0x7B,
                },
                [31] = new TR1Object
                {
                    ObjectId = 31,
                    Flags00 = 0x7B,
                },
                [32] = new TR1Object
                {
                    ObjectId = 32,
                    Flags00 = 0x7B,
                },
                [33] = new TR1Object
                {
                    ObjectId = 33,
                    Flags00 = 0x7B,
                },
                [34] = new TR1Object
                {
                    ObjectId = 34,
                    Flags00 = 0x7B,
                },
                [35] = new TR1Object
                {
                    ObjectId = 35,
                    Flags00 = 0x69,
                },
                [36] = new TR1Object
                {
                    ObjectId = 36,
                    Flags00 = 0x61,
                },
                [37] = new TR1Object
                {
                    ObjectId = 37,
                    Flags00 = 0x00,
                },
                [38] = new TR1Object
                {
                    ObjectId = 38,
                    Flags00 = 0x68,
                },
                [39] = new TR1Object
                {
                    ObjectId = 39,
                    Flags00 = 0x21,
                },
                [40] = new TR1Object
                {
                    ObjectId = 40,
                    Flags00 = 0x21,
                },
                [41] = new TR1Object
                {
                    ObjectId = 41,
                    Flags00 = 0x00,
                },
                [42] = new TR1Object
                {
                    ObjectId = 42,
                    Flags00 = 0x60,
                },
                [43] = new TR1Object
                {
                    ObjectId = 43,
                    Flags00 = 0x68,
                },
                [44] = new TR1Object
                {
                    ObjectId = 44,
                    Flags00 = 0x60,
                },
                [45] = new TR1Object
                {
                    ObjectId = 45,
                    Flags00 = 0x60,
                },
                [46] = new TR1Object
                {
                    ObjectId = 46,
                    Flags00 = 0x20,
                },
                [47] = new TR1Object
                {
                    ObjectId = 47,
                    Flags00 = 0x68,
                },
                [48] = new TR1Object
                {
                    ObjectId = 48,
                    Flags00 = 0x69,
                },
                [49] = new TR1Object
                {
                    ObjectId = 49,
                    Flags00 = 0x68,
                },
                [50] = new TR1Object
                {
                    ObjectId = 50,
                    Flags00 = 0x68,
                },
                [51] = new TR1Object
                {
                    ObjectId = 51,
                    Flags00 = 0x68,
                },
                [52] = new TR1Object
                {
                    ObjectId = 52,
                    Flags00 = 0x68,
                },
                [53] = new TR1Object
                {
                    ObjectId = 53,
                    Flags00 = 0x68,
                },
                [54] = new TR1Object
                {
                    ObjectId = 54,
                    Flags00 = 0x68,
                },
                [55] = new TR1Object
                {
                    ObjectId = 55,
                    Flags00 = 0x61,
                },
                [56] = new TR1Object
                {
                    ObjectId = 56,
                    Flags00 = 0x61,
                },
                [57] = new TR1Object
                {
                    ObjectId = 57,
                    Flags00 = 0x61,
                },
                [58] = new TR1Object
                {
                    ObjectId = 58,
                    Flags00 = 0x61,
                },
                [59] = new TR1Object
                {
                    ObjectId = 59,
                    Flags00 = 0x61,
                },
                [60] = new TR1Object
                {
                    ObjectId = 60,
                    Flags00 = 0x61,
                },
                [61] = new TR1Object
                {
                    ObjectId = 61,
                    Flags00 = 0x61,
                },
                [62] = new TR1Object
                {
                    ObjectId = 62,
                    Flags00 = 0x61,
                },
                [63] = new TR1Object
                {
                    ObjectId = 63,
                    Flags00 = 0x60,
                },
                [64] = new TR1Object
                {
                    ObjectId = 64,
                    Flags00 = 0x60,
                },
                [65] = new TR1Object
                {
                    ObjectId = 65,
                    Flags00 = 0x61,
                },
                [66] = new TR1Object
                {
                    ObjectId = 66,
                    Flags00 = 0x60,
                },
                [67] = new TR1Object
                {
                    ObjectId = 67,
                    Flags00 = 0x00,
                },
                [68] = new TR1Object
                {
                    ObjectId = 68,
                    Flags00 = 0x00,
                },
                [69] = new TR1Object
                {
                    ObjectId = 69,
                    Flags00 = 0x00,
                },
                [70] = new TR1Object
                {
                    ObjectId = 70,
                    Flags00 = 0x00,
                },
                [71] = new TR1Object
                {
                    ObjectId = 71,
                    Flags00 = 0x01,
                },
                [72] = new TR1Object
                {
                    ObjectId = 72,
                    Flags00 = 0x01,
                },
                [73] = new TR1Object
                {
                    ObjectId = 73,
                    Flags00 = 0x00,
                },
                [74] = new TR1Object
                {
                    ObjectId = 74,
                    Flags00 = 0x20,
                },
                [75] = new TR1Object
                {
                    ObjectId = 75,
                    Flags00 = 0x20,
                },
                [76] = new TR1Object
                {
                    ObjectId = 76,
                    Flags00 = 0x20,
                },
                [77] = new TR1Object
                {
                    ObjectId = 77,
                    Flags00 = 0x00,
                },
                [78] = new TR1Object
                {
                    ObjectId = 78,
                    Flags00 = 0x00,
                },
                [79] = new TR1Object
                {
                    ObjectId = 79,
                    Flags00 = 0x00,
                },
                [80] = new TR1Object
                {
                    ObjectId = 80,
                    Flags00 = 0x00,
                },
                [81] = new TR1Object
                {
                    ObjectId = 81,
                    Flags00 = 0x01,
                },
                [82] = new TR1Object
                {
                    ObjectId = 82,
                    Flags00 = 0x01,
                },
                [83] = new TR1Object
                {
                    ObjectId = 83,
                    Flags00 = 0x21,
                },
                [84] = new TR1Object
                {
                    ObjectId = 84,
                    Flags00 = 0x21,
                },
                [85] = new TR1Object
                {
                    ObjectId = 85,
                    Flags00 = 0x21,
                },
                [86] = new TR1Object
                {
                    ObjectId = 86,
                    Flags00 = 0x21,
                },
                [87] = new TR1Object
                {
                    ObjectId = 87,
                    Flags00 = 0x21,
                },
                [88] = new TR1Object
                {
                    ObjectId = 88,
                    Flags00 = 0x20,
                },
                [89] = new TR1Object
                {
                    ObjectId = 89,
                    Flags00 = 0x21,
                },
                [90] = new TR1Object
                {
                    ObjectId = 90,
                    Flags00 = 0x21,
                },
                [91] = new TR1Object
                {
                    ObjectId = 91,
                    Flags00 = 0x21,
                },
                [92] = new TR1Object
                {
                    ObjectId = 92,
                    Flags00 = 0x20,
                },
                [93] = new TR1Object
                {
                    ObjectId = 93,
                    Flags00 = 0x21,
                },
                [94] = new TR1Object
                {
                    ObjectId = 94,
                    Flags00 = 0x21,
                },
                [95] = new TR1Object
                {
                    ObjectId = 95,
                    Flags00 = 0x01,
                },
                [96] = new TR1Object
                {
                    ObjectId = 96,
                    Flags00 = 0x01,
                },
                [97] = new TR1Object
                {
                    ObjectId = 97,
                    Flags00 = 0x01,
                },
                [98] = new TR1Object
                {
                    ObjectId = 98,
                    Flags00 = 0x00,
                },
                [99] = new TR1Object
                {
                    ObjectId = 99,
                    Flags00 = 0x01,
                },
                [100] = new TR1Object
                {
                    ObjectId = 100,
                    Flags00 = 0x01,
                },
                [101] = new TR1Object
                {
                    ObjectId = 101,
                    Flags00 = 0x01,
                },
                [102] = new TR1Object
                {
                    ObjectId = 102,
                    Flags00 = 0x01,
                },
                [103] = new TR1Object
                {
                    ObjectId = 103,
                    Flags00 = 0x01,
                },
                [104] = new TR1Object
                {
                    ObjectId = 104,
                    Flags00 = 0x01,
                },
                [105] = new TR1Object
                {
                    ObjectId = 105,
                    Flags00 = 0x01,
                },
                [106] = new TR1Object
                {
                    ObjectId = 106,
                    Flags00 = 0x01,
                },
                [107] = new TR1Object
                {
                    ObjectId = 107,
                    Flags00 = 0x00,
                },
                [108] = new TR1Object
                {
                    ObjectId = 108,
                    Flags00 = 0x01,
                },
                [109] = new TR1Object
                {
                    ObjectId = 109,
                    Flags00 = 0x01,
                },
                [110] = new TR1Object
                {
                    ObjectId = 110,
                    Flags00 = 0x21,
                },
                [111] = new TR1Object
                {
                    ObjectId = 111,
                    Flags00 = 0x20,
                },
                [112] = new TR1Object
                {
                    ObjectId = 112,
                    Flags00 = 0x20,
                },
                [113] = new TR1Object
                {
                    ObjectId = 113,
                    Flags00 = 0x20,
                },
                [114] = new TR1Object
                {
                    ObjectId = 114,
                    Flags00 = 0x01,
                },
                [115] = new TR1Object
                {
                    ObjectId = 115,
                    Flags00 = 0x00,
                },
                [116] = new TR1Object
                {
                    ObjectId = 116,
                    Flags00 = 0x00,
                },
                [117] = new TR1Object
                {
                    ObjectId = 117,
                    Flags00 = 0x00,
                },
                [118] = new TR1Object
                {
                    ObjectId = 118,
                    Flags00 = 0x21,
                },
                [119] = new TR1Object
                {
                    ObjectId = 119,
                    Flags00 = 0x20,
                },
                [120] = new TR1Object
                {
                    ObjectId = 120,
                    Flags00 = 0x20,
                },
                [121] = new TR1Object
                {
                    ObjectId = 121,
                    Flags00 = 0x20,
                },
                [122] = new TR1Object
                {
                    ObjectId = 122,
                    Flags00 = 0x21,
                },
                [123] = new TR1Object
                {
                    ObjectId = 123,
                    Flags00 = 0x20,
                },
                [124] = new TR1Object
                {
                    ObjectId = 124,
                    Flags00 = 0x20,
                },
                [125] = new TR1Object
                {
                    ObjectId = 125,
                    Flags00 = 0x20,
                },
                [126] = new TR1Object
                {
                    ObjectId = 126,
                    Flags00 = 0x20,
                },
                [127] = new TR1Object
                {
                    ObjectId = 127,
                    Flags00 = 0x00,
                },
                [128] = new TR1Object
                {
                    ObjectId = 128,
                    Flags00 = 0x00,
                },
                [129] = new TR1Object
                {
                    ObjectId = 129,
                    Flags00 = 0x21,
                },
                [130] = new TR1Object
                {
                    ObjectId = 130,
                    Flags00 = 0x20,
                },
                [131] = new TR1Object
                {
                    ObjectId = 131,
                    Flags00 = 0x20,
                },
                [132] = new TR1Object
                {
                    ObjectId = 132,
                    Flags00 = 0x20,
                },
                [133] = new TR1Object
                {
                    ObjectId = 133,
                    Flags00 = 0x01,
                },
                [134] = new TR1Object
                {
                    ObjectId = 134,
                    Flags00 = 0x00,
                },
                [135] = new TR1Object
                {
                    ObjectId = 135,
                    Flags00 = 0x00,
                },
                [136] = new TR1Object
                {
                    ObjectId = 136,
                    Flags00 = 0x00,
                },
                [137] = new TR1Object
                {
                    ObjectId = 137,
                    Flags00 = 0x21,
                },
                [138] = new TR1Object
                {
                    ObjectId = 138,
                    Flags00 = 0x20,
                },
                [139] = new TR1Object
                {
                    ObjectId = 139,
                    Flags00 = 0x20,
                },
                [140] = new TR1Object
                {
                    ObjectId = 140,
                    Flags00 = 0x20,
                },
                [141] = new TR1Object
                {
                    ObjectId = 141,
                    Flags00 = 0x20,
                },
                [142] = new TR1Object
                {
                    ObjectId = 142,
                    Flags00 = 0x20,
                },
                [143] = new TR1Object
                {
                    ObjectId = 143,
                    Flags00 = 0x20,
                },
                [144] = new TR1Object
                {
                    ObjectId = 144,
                    Flags00 = 0x20,
                },
                [145] = new TR1Object
                {
                    ObjectId = 145,
                    Flags00 = 0x20,
                },
                [146] = new TR1Object
                {
                    ObjectId = 146,
                    Flags00 = 0x20,
                },
                [147] = new TR1Object
                {
                    ObjectId = 147,
                    Flags00 = 0x60,
                },
                [148] = new TR1Object
                {
                    ObjectId = 148,
                    Flags00 = 0x00,
                },
                [149] = new TR1Object
                {
                    ObjectId = 149,
                    Flags00 = 0x00,
                },
                [150] = new TR1Object
                {
                    ObjectId = 150,
                    Flags00 = 0x01,
                },
                [151] = new TR1Object
                {
                    ObjectId = 151,
                    Flags00 = 0x01,
                },
                [152] = new TR1Object
                {
                    ObjectId = 152,
                    Flags00 = 0x00,
                },
                [153] = new TR1Object
                {
                    ObjectId = 153,
                    Flags00 = 0x01,
                },
                [154] = new TR1Object
                {
                    ObjectId = 154,
                    Flags00 = 0x00,
                },
                [155] = new TR1Object
                {
                    ObjectId = 155,
                    Flags00 = 0x01,
                },
                [156] = new TR1Object
                {
                    ObjectId = 156,
                    Flags00 = 0x01,
                },
                [157] = new TR1Object
                {
                    ObjectId = 157,
                    Flags00 = 0x00,
                },
                [158] = new TR1Object
                {
                    ObjectId = 158,
                    Flags00 = 0x01,
                },
                [159] = new TR1Object
                {
                    ObjectId = 159,
                    Flags00 = 0x00,
                },
                [160] = new TR1Object
                {
                    ObjectId = 160,
                    Flags00 = 0x01,
                },
                [161] = new TR1Object
                {
                    ObjectId = 161,
                    Flags00 = 0x00,
                },
                [162] = new TR1Object
                {
                    ObjectId = 162,
                    Flags00 = 0x60,
                },
                [163] = new TR1Object
                {
                    ObjectId = 163,
                    Flags00 = 0x00,
                },
                [164] = new TR1Object
                {
                    ObjectId = 164,
                    Flags00 = 0x01,
                },
                [165] = new TR1Object
                {
                    ObjectId = 165,
                    Flags00 = 0x01,
                },
                [166] = new TR1Object
                {
                    ObjectId = 166,
                    Flags00 = 0x01,
                },
                [167] = new TR1Object
                {
                    ObjectId = 167,
                    Flags00 = 0x00,
                },
                [168] = new TR1Object
                {
                    ObjectId = 168,
                    Flags00 = 0x01,
                },
                [169] = new TR1Object
                {
                    ObjectId = 169,
                    Flags00 = 0x01,
                },
                [170] = new TR1Object
                {
                    ObjectId = 170,
                    Flags00 = 0x20,
                },
            },
            [3] = new Dictionary<int, TR1Object> // Lost Valley
            {
                [0] = new TR1Object
                {
                    ObjectId = 0,
                    Flags00 = 0x79,
                },
                [1] = new TR1Object
                {
                    ObjectId = 1,
                    Flags00 = 0x01,
                },
                [2] = new TR1Object
                {
                    ObjectId = 2,
                    Flags00 = 0x01,
                },
                [3] = new TR1Object
                {
                    ObjectId = 3,
                    Flags00 = 0x01,
                },
                [4] = new TR1Object
                {
                    ObjectId = 4,
                    Flags00 = 0x01,
                },
                [5] = new TR1Object
                {
                    ObjectId = 5,
                    Flags00 = 0x01,
                },
                [6] = new TR1Object
                {
                    ObjectId = 6,
                    Flags00 = 0x78,
                },
                [7] = new TR1Object
                {
                    ObjectId = 7,
                    Flags00 = 0x7B,
                },
                [8] = new TR1Object
                {
                    ObjectId = 8,
                    Flags00 = 0x7B,
                },
                [9] = new TR1Object
                {
                    ObjectId = 9,
                    Flags00 = 0x7B,
                },
                [10] = new TR1Object
                {
                    ObjectId = 10,
                    Flags00 = 0x7B,
                },
                [11] = new TR1Object
                {
                    ObjectId = 11,
                    Flags00 = 0x7B,
                },
                [12] = new TR1Object
                {
                    ObjectId = 12,
                    Flags00 = 0x7B,
                },
                [13] = new TR1Object
                {
                    ObjectId = 13,
                    Flags00 = 0x7B,
                },
                [14] = new TR1Object
                {
                    ObjectId = 14,
                    Flags00 = 0x7B,
                },
                [15] = new TR1Object
                {
                    ObjectId = 15,
                    Flags00 = 0x7B,
                },
                [16] = new TR1Object
                {
                    ObjectId = 16,
                    Flags00 = 0x7B,
                },
                [17] = new TR1Object
                {
                    ObjectId = 17,
                    Flags00 = 0x7B,
                },
                [18] = new TR1Object
                {
                    ObjectId = 18,
                    Flags00 = 0x7B,
                },
                [19] = new TR1Object
                {
                    ObjectId = 19,
                    Flags00 = 0x7B,
                },
                [20] = new TR1Object
                {
                    ObjectId = 20,
                    Flags00 = 0x7B,
                },
                [21] = new TR1Object
                {
                    ObjectId = 21,
                    Flags00 = 0x7B,
                },
                [22] = new TR1Object
                {
                    ObjectId = 22,
                    Flags00 = 0x7B,
                },
                [23] = new TR1Object
                {
                    ObjectId = 23,
                    Flags00 = 0x7B,
                },
                [24] = new TR1Object
                {
                    ObjectId = 24,
                    Flags00 = 0x71,
                },
                [25] = new TR1Object
                {
                    ObjectId = 25,
                    Flags00 = 0x00,
                },
                [26] = new TR1Object
                {
                    ObjectId = 26,
                    Flags00 = 0x00,
                },
                [27] = new TR1Object
                {
                    ObjectId = 27,
                    Flags00 = 0x7B,
                },
                [28] = new TR1Object
                {
                    ObjectId = 28,
                    Flags00 = 0x7B,
                },
                [29] = new TR1Object
                {
                    ObjectId = 29,
                    Flags00 = 0x01,
                },
                [30] = new TR1Object
                {
                    ObjectId = 30,
                    Flags00 = 0x7B,
                },
                [31] = new TR1Object
                {
                    ObjectId = 31,
                    Flags00 = 0x7B,
                },
                [32] = new TR1Object
                {
                    ObjectId = 32,
                    Flags00 = 0x7B,
                },
                [33] = new TR1Object
                {
                    ObjectId = 33,
                    Flags00 = 0x7B,
                },
                [34] = new TR1Object
                {
                    ObjectId = 34,
                    Flags00 = 0x7B,
                },
                [35] = new TR1Object
                {
                    ObjectId = 35,
                    Flags00 = 0x68,
                },
                [36] = new TR1Object
                {
                    ObjectId = 36,
                    Flags00 = 0x60,
                },
                [37] = new TR1Object
                {
                    ObjectId = 37,
                    Flags00 = 0x00,
                },
                [38] = new TR1Object
                {
                    ObjectId = 38,
                    Flags00 = 0x68,
                },
                [39] = new TR1Object
                {
                    ObjectId = 39,
                    Flags00 = 0x20,
                },
                [40] = new TR1Object
                {
                    ObjectId = 40,
                    Flags00 = 0x20,
                },
                [41] = new TR1Object
                {
                    ObjectId = 41,
                    Flags00 = 0x00,
                },
                [42] = new TR1Object
                {
                    ObjectId = 42,
                    Flags00 = 0x60,
                },
                [43] = new TR1Object
                {
                    ObjectId = 43,
                    Flags00 = 0x68,
                },
                [44] = new TR1Object
                {
                    ObjectId = 44,
                    Flags00 = 0x60,
                },
                [45] = new TR1Object
                {
                    ObjectId = 45,
                    Flags00 = 0x60,
                },
                [46] = new TR1Object
                {
                    ObjectId = 46,
                    Flags00 = 0x20,
                },
                [47] = new TR1Object
                {
                    ObjectId = 47,
                    Flags00 = 0x68,
                },
                [48] = new TR1Object
                {
                    ObjectId = 48,
                    Flags00 = 0x69,
                },
                [49] = new TR1Object
                {
                    ObjectId = 49,
                    Flags00 = 0x68,
                },
                [50] = new TR1Object
                {
                    ObjectId = 50,
                    Flags00 = 0x68,
                },
                [51] = new TR1Object
                {
                    ObjectId = 51,
                    Flags00 = 0x68,
                },
                [52] = new TR1Object
                {
                    ObjectId = 52,
                    Flags00 = 0x68,
                },
                [53] = new TR1Object
                {
                    ObjectId = 53,
                    Flags00 = 0x68,
                },
                [54] = new TR1Object
                {
                    ObjectId = 54,
                    Flags00 = 0x68,
                },
                [55] = new TR1Object
                {
                    ObjectId = 55,
                    Flags00 = 0x61,
                },
                [56] = new TR1Object
                {
                    ObjectId = 56,
                    Flags00 = 0x60,
                },
                [57] = new TR1Object
                {
                    ObjectId = 57,
                    Flags00 = 0x61,
                },
                [58] = new TR1Object
                {
                    ObjectId = 58,
                    Flags00 = 0x60,
                },
                [59] = new TR1Object
                {
                    ObjectId = 59,
                    Flags00 = 0x61,
                },
                [60] = new TR1Object
                {
                    ObjectId = 60,
                    Flags00 = 0x60,
                },
                [61] = new TR1Object
                {
                    ObjectId = 61,
                    Flags00 = 0x60,
                },
                [62] = new TR1Object
                {
                    ObjectId = 62,
                    Flags00 = 0x61,
                },
                [63] = new TR1Object
                {
                    ObjectId = 63,
                    Flags00 = 0x60,
                },
                [64] = new TR1Object
                {
                    ObjectId = 64,
                    Flags00 = 0x60,
                },
                [65] = new TR1Object
                {
                    ObjectId = 65,
                    Flags00 = 0x60,
                },
                [66] = new TR1Object
                {
                    ObjectId = 66,
                    Flags00 = 0x60,
                },
                [67] = new TR1Object
                {
                    ObjectId = 67,
                    Flags00 = 0x00,
                },
                [68] = new TR1Object
                {
                    ObjectId = 68,
                    Flags00 = 0x01,
                },
                [69] = new TR1Object
                {
                    ObjectId = 69,
                    Flags00 = 0x01,
                },
                [70] = new TR1Object
                {
                    ObjectId = 70,
                    Flags00 = 0x01,
                },
                [71] = new TR1Object
                {
                    ObjectId = 71,
                    Flags00 = 0x01,
                },
                [72] = new TR1Object
                {
                    ObjectId = 72,
                    Flags00 = 0x01,
                },
                [73] = new TR1Object
                {
                    ObjectId = 73,
                    Flags00 = 0x00,
                },
                [74] = new TR1Object
                {
                    ObjectId = 74,
                    Flags00 = 0x21,
                },
                [75] = new TR1Object
                {
                    ObjectId = 75,
                    Flags00 = 0x21,
                },
                [76] = new TR1Object
                {
                    ObjectId = 76,
                    Flags00 = 0x21,
                },
                [77] = new TR1Object
                {
                    ObjectId = 77,
                    Flags00 = 0x00,
                },
                [78] = new TR1Object
                {
                    ObjectId = 78,
                    Flags00 = 0x00,
                },
                [79] = new TR1Object
                {
                    ObjectId = 79,
                    Flags00 = 0x00,
                },
                [80] = new TR1Object
                {
                    ObjectId = 80,
                    Flags00 = 0x00,
                },
                [81] = new TR1Object
                {
                    ObjectId = 81,
                    Flags00 = 0x01,
                },
                [82] = new TR1Object
                {
                    ObjectId = 82,
                    Flags00 = 0x01,
                },
                [83] = new TR1Object
                {
                    ObjectId = 83,
                    Flags00 = 0x21,
                },
                [84] = new TR1Object
                {
                    ObjectId = 84,
                    Flags00 = 0x21,
                },
                [85] = new TR1Object
                {
                    ObjectId = 85,
                    Flags00 = 0x21,
                },
                [86] = new TR1Object
                {
                    ObjectId = 86,
                    Flags00 = 0x21,
                },
                [87] = new TR1Object
                {
                    ObjectId = 87,
                    Flags00 = 0x21,
                },
                [88] = new TR1Object
                {
                    ObjectId = 88,
                    Flags00 = 0x20,
                },
                [89] = new TR1Object
                {
                    ObjectId = 89,
                    Flags00 = 0x21,
                },
                [90] = new TR1Object
                {
                    ObjectId = 90,
                    Flags00 = 0x21,
                },
                [91] = new TR1Object
                {
                    ObjectId = 91,
                    Flags00 = 0x21,
                },
                [92] = new TR1Object
                {
                    ObjectId = 92,
                    Flags00 = 0x20,
                },
                [93] = new TR1Object
                {
                    ObjectId = 93,
                    Flags00 = 0x21,
                },
                [94] = new TR1Object
                {
                    ObjectId = 94,
                    Flags00 = 0x21,
                },
                [95] = new TR1Object
                {
                    ObjectId = 95,
                    Flags00 = 0x01,
                },
                [96] = new TR1Object
                {
                    ObjectId = 96,
                    Flags00 = 0x01,
                },
                [97] = new TR1Object
                {
                    ObjectId = 97,
                    Flags00 = 0x01,
                },
                [98] = new TR1Object
                {
                    ObjectId = 98,
                    Flags00 = 0x00,
                },
                [99] = new TR1Object
                {
                    ObjectId = 99,
                    Flags00 = 0x01,
                },
                [100] = new TR1Object
                {
                    ObjectId = 100,
                    Flags00 = 0x01,
                },
                [101] = new TR1Object
                {
                    ObjectId = 101,
                    Flags00 = 0x01,
                },
                [102] = new TR1Object
                {
                    ObjectId = 102,
                    Flags00 = 0x01,
                },
                [103] = new TR1Object
                {
                    ObjectId = 103,
                    Flags00 = 0x01,
                },
                [104] = new TR1Object
                {
                    ObjectId = 104,
                    Flags00 = 0x01,
                },
                [105] = new TR1Object
                {
                    ObjectId = 105,
                    Flags00 = 0x01,
                },
                [106] = new TR1Object
                {
                    ObjectId = 106,
                    Flags00 = 0x01,
                },
                [107] = new TR1Object
                {
                    ObjectId = 107,
                    Flags00 = 0x00,
                },
                [108] = new TR1Object
                {
                    ObjectId = 108,
                    Flags00 = 0x01,
                },
                [109] = new TR1Object
                {
                    ObjectId = 109,
                    Flags00 = 0x01,
                },
                [110] = new TR1Object
                {
                    ObjectId = 110,
                    Flags00 = 0x21,
                },
                [111] = new TR1Object
                {
                    ObjectId = 111,
                    Flags00 = 0x20,
                },
                [112] = new TR1Object
                {
                    ObjectId = 112,
                    Flags00 = 0x20,
                },
                [113] = new TR1Object
                {
                    ObjectId = 113,
                    Flags00 = 0x20,
                },
                [114] = new TR1Object
                {
                    ObjectId = 114,
                    Flags00 = 0x01,
                },
                [115] = new TR1Object
                {
                    ObjectId = 115,
                    Flags00 = 0x00,
                },
                [116] = new TR1Object
                {
                    ObjectId = 116,
                    Flags00 = 0x00,
                },
                [117] = new TR1Object
                {
                    ObjectId = 117,
                    Flags00 = 0x00,
                },
                [118] = new TR1Object
                {
                    ObjectId = 118,
                    Flags00 = 0x21,
                },
                [119] = new TR1Object
                {
                    ObjectId = 119,
                    Flags00 = 0x20,
                },
                [120] = new TR1Object
                {
                    ObjectId = 120,
                    Flags00 = 0x20,
                },
                [121] = new TR1Object
                {
                    ObjectId = 121,
                    Flags00 = 0x20,
                },
                [122] = new TR1Object
                {
                    ObjectId = 122,
                    Flags00 = 0x21,
                },
                [123] = new TR1Object
                {
                    ObjectId = 123,
                    Flags00 = 0x20,
                },
                [124] = new TR1Object
                {
                    ObjectId = 124,
                    Flags00 = 0x20,
                },
                [125] = new TR1Object
                {
                    ObjectId = 125,
                    Flags00 = 0x20,
                },
                [126] = new TR1Object
                {
                    ObjectId = 126,
                    Flags00 = 0x20,
                },
                [127] = new TR1Object
                {
                    ObjectId = 127,
                    Flags00 = 0x00,
                },
                [128] = new TR1Object
                {
                    ObjectId = 128,
                    Flags00 = 0x00,
                },
                [129] = new TR1Object
                {
                    ObjectId = 129,
                    Flags00 = 0x20,
                },
                [130] = new TR1Object
                {
                    ObjectId = 130,
                    Flags00 = 0x20,
                },
                [131] = new TR1Object
                {
                    ObjectId = 131,
                    Flags00 = 0x20,
                },
                [132] = new TR1Object
                {
                    ObjectId = 132,
                    Flags00 = 0x20,
                },
                [133] = new TR1Object
                {
                    ObjectId = 133,
                    Flags00 = 0x00,
                },
                [134] = new TR1Object
                {
                    ObjectId = 134,
                    Flags00 = 0x00,
                },
                [135] = new TR1Object
                {
                    ObjectId = 135,
                    Flags00 = 0x00,
                },
                [136] = new TR1Object
                {
                    ObjectId = 136,
                    Flags00 = 0x00,
                },
                [137] = new TR1Object
                {
                    ObjectId = 137,
                    Flags00 = 0x20,
                },
                [138] = new TR1Object
                {
                    ObjectId = 138,
                    Flags00 = 0x20,
                },
                [139] = new TR1Object
                {
                    ObjectId = 139,
                    Flags00 = 0x20,
                },
                [140] = new TR1Object
                {
                    ObjectId = 140,
                    Flags00 = 0x20,
                },
                [141] = new TR1Object
                {
                    ObjectId = 141,
                    Flags00 = 0x20,
                },
                [142] = new TR1Object
                {
                    ObjectId = 142,
                    Flags00 = 0x20,
                },
                [143] = new TR1Object
                {
                    ObjectId = 143,
                    Flags00 = 0x20,
                },
                [144] = new TR1Object
                {
                    ObjectId = 144,
                    Flags00 = 0x20,
                },
                [145] = new TR1Object
                {
                    ObjectId = 145,
                    Flags00 = 0x20,
                },
                [146] = new TR1Object
                {
                    ObjectId = 146,
                    Flags00 = 0x20,
                },
                [147] = new TR1Object
                {
                    ObjectId = 147,
                    Flags00 = 0x60,
                },
                [148] = new TR1Object
                {
                    ObjectId = 148,
                    Flags00 = 0x00,
                },
                [149] = new TR1Object
                {
                    ObjectId = 149,
                    Flags00 = 0x00,
                },
                [150] = new TR1Object
                {
                    ObjectId = 150,
                    Flags00 = 0x01,
                },
                [151] = new TR1Object
                {
                    ObjectId = 151,
                    Flags00 = 0x00,
                },
                [152] = new TR1Object
                {
                    ObjectId = 152,
                    Flags00 = 0x00,
                },
                [153] = new TR1Object
                {
                    ObjectId = 153,
                    Flags00 = 0x01,
                },
                [154] = new TR1Object
                {
                    ObjectId = 154,
                    Flags00 = 0x00,
                },
                [155] = new TR1Object
                {
                    ObjectId = 155,
                    Flags00 = 0x01,
                },
                [156] = new TR1Object
                {
                    ObjectId = 156,
                    Flags00 = 0x01,
                },
                [157] = new TR1Object
                {
                    ObjectId = 157,
                    Flags00 = 0x00,
                },
                [158] = new TR1Object
                {
                    ObjectId = 158,
                    Flags00 = 0x01,
                },
                [159] = new TR1Object
                {
                    ObjectId = 159,
                    Flags00 = 0x00,
                },
                [160] = new TR1Object
                {
                    ObjectId = 160,
                    Flags00 = 0x00,
                },
                [161] = new TR1Object
                {
                    ObjectId = 161,
                    Flags00 = 0x00,
                },
                [162] = new TR1Object
                {
                    ObjectId = 162,
                    Flags00 = 0x60,
                },
                [163] = new TR1Object
                {
                    ObjectId = 163,
                    Flags00 = 0x00,
                },
                [164] = new TR1Object
                {
                    ObjectId = 164,
                    Flags00 = 0x01,
                },
                [165] = new TR1Object
                {
                    ObjectId = 165,
                    Flags00 = 0x00,
                },
                [166] = new TR1Object
                {
                    ObjectId = 166,
                    Flags00 = 0x01,
                },
                [167] = new TR1Object
                {
                    ObjectId = 167,
                    Flags00 = 0x00,
                },
                [168] = new TR1Object
                {
                    ObjectId = 168,
                    Flags00 = 0x01,
                },
                [169] = new TR1Object
                {
                    ObjectId = 169,
                    Flags00 = 0x01,
                },
                [170] = new TR1Object
                {
                    ObjectId = 170,
                    Flags00 = 0x21,
                },
                [171] = new TR1Object
                {
                    ObjectId = 171,
                    Flags00 = 0x00,
                },
            },
            [4] = new Dictionary<int, TR1Object> // Tomb of Qualopec
            {
                [0] = new TR1Object
                {
                    ObjectId = 0,
                    Flags00 = 0x79,
                },
                [1] = new TR1Object
                {
                    ObjectId = 1,
                    Flags00 = 0x01,
                },
                [2] = new TR1Object
                {
                    ObjectId = 2,
                    Flags00 = 0x01,
                },
                [3] = new TR1Object
                {
                    ObjectId = 3,
                    Flags00 = 0x01,
                },
                [4] = new TR1Object
                {
                    ObjectId = 4,
                    Flags00 = 0x01,
                },
                [5] = new TR1Object
                {
                    ObjectId = 5,
                    Flags00 = 0x01,
                },
                [6] = new TR1Object
                {
                    ObjectId = 6,
                    Flags00 = 0x78,
                },
                [7] = new TR1Object
                {
                    ObjectId = 7,
                    Flags00 = 0x7B,
                },
                [8] = new TR1Object
                {
                    ObjectId = 8,
                    Flags00 = 0x7B,
                },
                [9] = new TR1Object
                {
                    ObjectId = 9,
                    Flags00 = 0x7B,
                },
                [10] = new TR1Object
                {
                    ObjectId = 10,
                    Flags00 = 0x7B,
                },
                [11] = new TR1Object
                {
                    ObjectId = 11,
                    Flags00 = 0x7B,
                },
                [12] = new TR1Object
                {
                    ObjectId = 12,
                    Flags00 = 0x7B,
                },
                [13] = new TR1Object
                {
                    ObjectId = 13,
                    Flags00 = 0x7B,
                },
                [14] = new TR1Object
                {
                    ObjectId = 14,
                    Flags00 = 0x7B,
                },
                [15] = new TR1Object
                {
                    ObjectId = 15,
                    Flags00 = 0x7B,
                },
                [16] = new TR1Object
                {
                    ObjectId = 16,
                    Flags00 = 0x7B,
                },
                [17] = new TR1Object
                {
                    ObjectId = 17,
                    Flags00 = 0x7B,
                },
                [18] = new TR1Object
                {
                    ObjectId = 18,
                    Flags00 = 0x7B,
                },
                [19] = new TR1Object
                {
                    ObjectId = 19,
                    Flags00 = 0x7B,
                },
                [20] = new TR1Object
                {
                    ObjectId = 20,
                    Flags00 = 0x7B,
                },
                [21] = new TR1Object
                {
                    ObjectId = 21,
                    Flags00 = 0x7B,
                },
                [22] = new TR1Object
                {
                    ObjectId = 22,
                    Flags00 = 0x7B,
                },
                [23] = new TR1Object
                {
                    ObjectId = 23,
                    Flags00 = 0x7B,
                },
                [24] = new TR1Object
                {
                    ObjectId = 24,
                    Flags00 = 0x71,
                },
                [25] = new TR1Object
                {
                    ObjectId = 25,
                    Flags00 = 0x00,
                },
                [26] = new TR1Object
                {
                    ObjectId = 26,
                    Flags00 = 0x00,
                },
                [27] = new TR1Object
                {
                    ObjectId = 27,
                    Flags00 = 0x7B,
                },
                [28] = new TR1Object
                {
                    ObjectId = 28,
                    Flags00 = 0x7B,
                },
                [29] = new TR1Object
                {
                    ObjectId = 29,
                    Flags00 = 0x01,
                },
                [30] = new TR1Object
                {
                    ObjectId = 30,
                    Flags00 = 0x7B,
                },
                [31] = new TR1Object
                {
                    ObjectId = 31,
                    Flags00 = 0x7B,
                },
                [32] = new TR1Object
                {
                    ObjectId = 32,
                    Flags00 = 0x7B,
                },
                [33] = new TR1Object
                {
                    ObjectId = 33,
                    Flags00 = 0x7B,
                },
                [34] = new TR1Object
                {
                    ObjectId = 34,
                    Flags00 = 0x7B,
                },
                [35] = new TR1Object
                {
                    ObjectId = 35,
                    Flags00 = 0x69,
                },
                [36] = new TR1Object
                {
                    ObjectId = 36,
                    Flags00 = 0x60,
                },
                [37] = new TR1Object
                {
                    ObjectId = 37,
                    Flags00 = 0x01,
                },
                [38] = new TR1Object
                {
                    ObjectId = 38,
                    Flags00 = 0x69,
                },
                [39] = new TR1Object
                {
                    ObjectId = 39,
                    Flags00 = 0x21,
                },
                [40] = new TR1Object
                {
                    ObjectId = 40,
                    Flags00 = 0x21,
                },
                [41] = new TR1Object
                {
                    ObjectId = 41,
                    Flags00 = 0x00,
                },
                [42] = new TR1Object
                {
                    ObjectId = 42,
                    Flags00 = 0x60,
                },
                [43] = new TR1Object
                {
                    ObjectId = 43,
                    Flags00 = 0x68,
                },
                [44] = new TR1Object
                {
                    ObjectId = 44,
                    Flags00 = 0x60,
                },
                [45] = new TR1Object
                {
                    ObjectId = 45,
                    Flags00 = 0x60,
                },
                [46] = new TR1Object
                {
                    ObjectId = 46,
                    Flags00 = 0x20,
                },
                [47] = new TR1Object
                {
                    ObjectId = 47,
                    Flags00 = 0x68,
                },
                [48] = new TR1Object
                {
                    ObjectId = 48,
                    Flags00 = 0x69,
                },
                [49] = new TR1Object
                {
                    ObjectId = 49,
                    Flags00 = 0x68,
                },
                [50] = new TR1Object
                {
                    ObjectId = 50,
                    Flags00 = 0x68,
                },
                [51] = new TR1Object
                {
                    ObjectId = 51,
                    Flags00 = 0x68,
                },
                [52] = new TR1Object
                {
                    ObjectId = 52,
                    Flags00 = 0x69,
                },
                [53] = new TR1Object
                {
                    ObjectId = 53,
                    Flags00 = 0x69,
                },
                [54] = new TR1Object
                {
                    ObjectId = 54,
                    Flags00 = 0x68,
                },
                [55] = new TR1Object
                {
                    ObjectId = 55,
                    Flags00 = 0x61,
                },
                [56] = new TR1Object
                {
                    ObjectId = 56,
                    Flags00 = 0x60,
                },
                [57] = new TR1Object
                {
                    ObjectId = 57,
                    Flags00 = 0x61,
                },
                [58] = new TR1Object
                {
                    ObjectId = 58,
                    Flags00 = 0x61,
                },
                [59] = new TR1Object
                {
                    ObjectId = 59,
                    Flags00 = 0x61,
                },
                [60] = new TR1Object
                {
                    ObjectId = 60,
                    Flags00 = 0x61,
                },
                [61] = new TR1Object
                {
                    ObjectId = 61,
                    Flags00 = 0x61,
                },
                [62] = new TR1Object
                {
                    ObjectId = 62,
                    Flags00 = 0x61,
                },
                [63] = new TR1Object
                {
                    ObjectId = 63,
                    Flags00 = 0x60,
                },
                [64] = new TR1Object
                {
                    ObjectId = 64,
                    Flags00 = 0x61,
                },
                [65] = new TR1Object
                {
                    ObjectId = 65,
                    Flags00 = 0x60,
                },
                [66] = new TR1Object
                {
                    ObjectId = 66,
                    Flags00 = 0x60,
                },
                [67] = new TR1Object
                {
                    ObjectId = 67,
                    Flags00 = 0x00,
                },
                [68] = new TR1Object
                {
                    ObjectId = 68,
                    Flags00 = 0x00,
                },
                [69] = new TR1Object
                {
                    ObjectId = 69,
                    Flags00 = 0x00,
                },
                [70] = new TR1Object
                {
                    ObjectId = 70,
                    Flags00 = 0x00,
                },
                [71] = new TR1Object
                {
                    ObjectId = 71,
                    Flags00 = 0x01,
                },
                [72] = new TR1Object
                {
                    ObjectId = 72,
                    Flags00 = 0x01,
                },
                [73] = new TR1Object
                {
                    ObjectId = 73,
                    Flags00 = 0x00,
                },
                [74] = new TR1Object
                {
                    ObjectId = 74,
                    Flags00 = 0x20,
                },
                [75] = new TR1Object
                {
                    ObjectId = 75,
                    Flags00 = 0x20,
                },
                [76] = new TR1Object
                {
                    ObjectId = 76,
                    Flags00 = 0x20,
                },
                [77] = new TR1Object
                {
                    ObjectId = 77,
                    Flags00 = 0x00,
                },
                [78] = new TR1Object
                {
                    ObjectId = 78,
                    Flags00 = 0x00,
                },
                [79] = new TR1Object
                {
                    ObjectId = 79,
                    Flags00 = 0x00,
                },
                [80] = new TR1Object
                {
                    ObjectId = 80,
                    Flags00 = 0x00,
                },
                [81] = new TR1Object
                {
                    ObjectId = 81,
                    Flags00 = 0x01,
                },
                [82] = new TR1Object
                {
                    ObjectId = 82,
                    Flags00 = 0x01,
                },
                [83] = new TR1Object
                {
                    ObjectId = 83,
                    Flags00 = 0x21,
                },
                [84] = new TR1Object
                {
                    ObjectId = 84,
                    Flags00 = 0x21,
                },
                [85] = new TR1Object
                {
                    ObjectId = 85,
                    Flags00 = 0x21,
                },
                [86] = new TR1Object
                {
                    ObjectId = 86,
                    Flags00 = 0x21,
                },
                [87] = new TR1Object
                {
                    ObjectId = 87,
                    Flags00 = 0x21,
                },
                [88] = new TR1Object
                {
                    ObjectId = 88,
                    Flags00 = 0x20,
                },
                [89] = new TR1Object
                {
                    ObjectId = 89,
                    Flags00 = 0x21,
                },
                [90] = new TR1Object
                {
                    ObjectId = 90,
                    Flags00 = 0x21,
                },
                [91] = new TR1Object
                {
                    ObjectId = 91,
                    Flags00 = 0x21,
                },
                [92] = new TR1Object
                {
                    ObjectId = 92,
                    Flags00 = 0x20,
                },
                [93] = new TR1Object
                {
                    ObjectId = 93,
                    Flags00 = 0x21,
                },
                [94] = new TR1Object
                {
                    ObjectId = 94,
                    Flags00 = 0x21,
                },
                [95] = new TR1Object
                {
                    ObjectId = 95,
                    Flags00 = 0x01,
                },
                [96] = new TR1Object
                {
                    ObjectId = 96,
                    Flags00 = 0x01,
                },
                [97] = new TR1Object
                {
                    ObjectId = 97,
                    Flags00 = 0x01,
                },
                [98] = new TR1Object
                {
                    ObjectId = 98,
                    Flags00 = 0x00,
                },
                [99] = new TR1Object
                {
                    ObjectId = 99,
                    Flags00 = 0x01,
                },
                [100] = new TR1Object
                {
                    ObjectId = 100,
                    Flags00 = 0x01,
                },
                [101] = new TR1Object
                {
                    ObjectId = 101,
                    Flags00 = 0x01,
                },
                [102] = new TR1Object
                {
                    ObjectId = 102,
                    Flags00 = 0x01,
                },
                [103] = new TR1Object
                {
                    ObjectId = 103,
                    Flags00 = 0x01,
                },
                [104] = new TR1Object
                {
                    ObjectId = 104,
                    Flags00 = 0x01,
                },
                [105] = new TR1Object
                {
                    ObjectId = 105,
                    Flags00 = 0x01,
                },
                [106] = new TR1Object
                {
                    ObjectId = 106,
                    Flags00 = 0x01,
                },
                [107] = new TR1Object
                {
                    ObjectId = 107,
                    Flags00 = 0x00,
                },
                [108] = new TR1Object
                {
                    ObjectId = 108,
                    Flags00 = 0x01,
                },
                [109] = new TR1Object
                {
                    ObjectId = 109,
                    Flags00 = 0x01,
                },
                [110] = new TR1Object
                {
                    ObjectId = 110,
                    Flags00 = 0x21,
                },
                [111] = new TR1Object
                {
                    ObjectId = 111,
                    Flags00 = 0x20,
                },
                [112] = new TR1Object
                {
                    ObjectId = 112,
                    Flags00 = 0x20,
                },
                [113] = new TR1Object
                {
                    ObjectId = 113,
                    Flags00 = 0x20,
                },
                [114] = new TR1Object
                {
                    ObjectId = 114,
                    Flags00 = 0x01,
                },
                [115] = new TR1Object
                {
                    ObjectId = 115,
                    Flags00 = 0x00,
                },
                [116] = new TR1Object
                {
                    ObjectId = 116,
                    Flags00 = 0x00,
                },
                [117] = new TR1Object
                {
                    ObjectId = 117,
                    Flags00 = 0x00,
                },
                [118] = new TR1Object
                {
                    ObjectId = 118,
                    Flags00 = 0x21,
                },
                [119] = new TR1Object
                {
                    ObjectId = 119,
                    Flags00 = 0x20,
                },
                [120] = new TR1Object
                {
                    ObjectId = 120,
                    Flags00 = 0x20,
                },
                [121] = new TR1Object
                {
                    ObjectId = 121,
                    Flags00 = 0x20,
                },
                [122] = new TR1Object
                {
                    ObjectId = 122,
                    Flags00 = 0x21,
                },
                [123] = new TR1Object
                {
                    ObjectId = 123,
                    Flags00 = 0x20,
                },
                [124] = new TR1Object
                {
                    ObjectId = 124,
                    Flags00 = 0x20,
                },
                [125] = new TR1Object
                {
                    ObjectId = 125,
                    Flags00 = 0x20,
                },
                [126] = new TR1Object
                {
                    ObjectId = 126,
                    Flags00 = 0x20,
                },
                [127] = new TR1Object
                {
                    ObjectId = 127,
                    Flags00 = 0x00,
                },
                [128] = new TR1Object
                {
                    ObjectId = 128,
                    Flags00 = 0x00,
                },
                [129] = new TR1Object
                {
                    ObjectId = 129,
                    Flags00 = 0x20,
                },
                [130] = new TR1Object
                {
                    ObjectId = 130,
                    Flags00 = 0x20,
                },
                [131] = new TR1Object
                {
                    ObjectId = 131,
                    Flags00 = 0x20,
                },
                [132] = new TR1Object
                {
                    ObjectId = 132,
                    Flags00 = 0x20,
                },
                [133] = new TR1Object
                {
                    ObjectId = 133,
                    Flags00 = 0x00,
                },
                [134] = new TR1Object
                {
                    ObjectId = 134,
                    Flags00 = 0x00,
                },
                [135] = new TR1Object
                {
                    ObjectId = 135,
                    Flags00 = 0x00,
                },
                [136] = new TR1Object
                {
                    ObjectId = 136,
                    Flags00 = 0x00,
                },
                [137] = new TR1Object
                {
                    ObjectId = 137,
                    Flags00 = 0x20,
                },
                [138] = new TR1Object
                {
                    ObjectId = 138,
                    Flags00 = 0x20,
                },
                [139] = new TR1Object
                {
                    ObjectId = 139,
                    Flags00 = 0x20,
                },
                [140] = new TR1Object
                {
                    ObjectId = 140,
                    Flags00 = 0x20,
                },
                [141] = new TR1Object
                {
                    ObjectId = 141,
                    Flags00 = 0x20,
                },
                [142] = new TR1Object
                {
                    ObjectId = 142,
                    Flags00 = 0x20,
                },
                [143] = new TR1Object
                {
                    ObjectId = 143,
                    Flags00 = 0x21,
                },
                [144] = new TR1Object
                {
                    ObjectId = 144,
                    Flags00 = 0x20,
                },
                [145] = new TR1Object
                {
                    ObjectId = 145,
                    Flags00 = 0x20,
                },
                [146] = new TR1Object
                {
                    ObjectId = 146,
                    Flags00 = 0x20,
                },
                [147] = new TR1Object
                {
                    ObjectId = 147,
                    Flags00 = 0x60,
                },
                [148] = new TR1Object
                {
                    ObjectId = 148,
                    Flags00 = 0x00,
                },
                [149] = new TR1Object
                {
                    ObjectId = 149,
                    Flags00 = 0x00,
                },
                [150] = new TR1Object
                {
                    ObjectId = 150,
                    Flags00 = 0x01,
                },
                [151] = new TR1Object
                {
                    ObjectId = 151,
                    Flags00 = 0x00,
                },
                [152] = new TR1Object
                {
                    ObjectId = 152,
                    Flags00 = 0x00,
                },
                [153] = new TR1Object
                {
                    ObjectId = 153,
                    Flags00 = 0x01,
                },
                [154] = new TR1Object
                {
                    ObjectId = 154,
                    Flags00 = 0x00,
                },
                [155] = new TR1Object
                {
                    ObjectId = 155,
                    Flags00 = 0x01,
                },
                [156] = new TR1Object
                {
                    ObjectId = 156,
                    Flags00 = 0x01,
                },
                [157] = new TR1Object
                {
                    ObjectId = 157,
                    Flags00 = 0x00,
                },
                [158] = new TR1Object
                {
                    ObjectId = 158,
                    Flags00 = 0x01,
                },
                [159] = new TR1Object
                {
                    ObjectId = 159,
                    Flags00 = 0x00,
                },
                [160] = new TR1Object
                {
                    ObjectId = 160,
                    Flags00 = 0x01,
                },
                [161] = new TR1Object
                {
                    ObjectId = 161,
                    Flags00 = 0x00,
                },
                [162] = new TR1Object
                {
                    ObjectId = 162,
                    Flags00 = 0x60,
                },
                [163] = new TR1Object
                {
                    ObjectId = 163,
                    Flags00 = 0x00,
                },
                [164] = new TR1Object
                {
                    ObjectId = 164,
                    Flags00 = 0x01,
                },
                [165] = new TR1Object
                {
                    ObjectId = 165,
                    Flags00 = 0x00,
                },
                [166] = new TR1Object
                {
                    ObjectId = 166,
                    Flags00 = 0x01,
                },
                [167] = new TR1Object
                {
                    ObjectId = 167,
                    Flags00 = 0x00,
                },
                [168] = new TR1Object
                {
                    ObjectId = 168,
                    Flags00 = 0x01,
                },
                [169] = new TR1Object
                {
                    ObjectId = 169,
                    Flags00 = 0x01,
                },
                [170] = new TR1Object
                {
                    ObjectId = 170,
                    Flags00 = 0x20,
                },
            },
            [5] = new Dictionary<int, TR1Object> // St. Francis' Folly
            {
                [0] = new TR1Object
                {
                    ObjectId = 0,
                    Flags00 = 0x79,
                },
                [1] = new TR1Object
                {
                    ObjectId = 1,
                    Flags00 = 0x01,
                },
                [2] = new TR1Object
                {
                    ObjectId = 2,
                    Flags00 = 0x01,
                },
                [3] = new TR1Object
                {
                    ObjectId = 3,
                    Flags00 = 0x01,
                },
                [4] = new TR1Object
                {
                    ObjectId = 4,
                    Flags00 = 0x01,
                },
                [5] = new TR1Object
                {
                    ObjectId = 5,
                    Flags00 = 0x01,
                },
                [6] = new TR1Object
                {
                    ObjectId = 6,
                    Flags00 = 0x78,
                },
                [7] = new TR1Object
                {
                    ObjectId = 7,
                    Flags00 = 0x7B,
                },
                [8] = new TR1Object
                {
                    ObjectId = 8,
                    Flags00 = 0x7B,
                },
                [9] = new TR1Object
                {
                    ObjectId = 9,
                    Flags00 = 0x7B,
                },
                [10] = new TR1Object
                {
                    ObjectId = 10,
                    Flags00 = 0x7B,
                },
                [11] = new TR1Object
                {
                    ObjectId = 11,
                    Flags00 = 0x7B,
                },
                [12] = new TR1Object
                {
                    ObjectId = 12,
                    Flags00 = 0x7B,
                },
                [13] = new TR1Object
                {
                    ObjectId = 13,
                    Flags00 = 0x7B,
                },
                [14] = new TR1Object
                {
                    ObjectId = 14,
                    Flags00 = 0x7B,
                },
                [15] = new TR1Object
                {
                    ObjectId = 15,
                    Flags00 = 0x7B,
                },
                [16] = new TR1Object
                {
                    ObjectId = 16,
                    Flags00 = 0x7B,
                },
                [17] = new TR1Object
                {
                    ObjectId = 17,
                    Flags00 = 0x7B,
                },
                [18] = new TR1Object
                {
                    ObjectId = 18,
                    Flags00 = 0x7B,
                },
                [19] = new TR1Object
                {
                    ObjectId = 19,
                    Flags00 = 0x7B,
                },
                [20] = new TR1Object
                {
                    ObjectId = 20,
                    Flags00 = 0x7B,
                },
                [21] = new TR1Object
                {
                    ObjectId = 21,
                    Flags00 = 0x7B,
                },
                [22] = new TR1Object
                {
                    ObjectId = 22,
                    Flags00 = 0x7B,
                },
                [23] = new TR1Object
                {
                    ObjectId = 23,
                    Flags00 = 0x7B,
                },
                [24] = new TR1Object
                {
                    ObjectId = 24,
                    Flags00 = 0x71,
                },
                [25] = new TR1Object
                {
                    ObjectId = 25,
                    Flags00 = 0x00,
                },
                [26] = new TR1Object
                {
                    ObjectId = 26,
                    Flags00 = 0x00,
                },
                [27] = new TR1Object
                {
                    ObjectId = 27,
                    Flags00 = 0x7B,
                },
                [28] = new TR1Object
                {
                    ObjectId = 28,
                    Flags00 = 0x7B,
                },
                [29] = new TR1Object
                {
                    ObjectId = 29,
                    Flags00 = 0x01,
                },
                [30] = new TR1Object
                {
                    ObjectId = 30,
                    Flags00 = 0x7B,
                },
                [31] = new TR1Object
                {
                    ObjectId = 31,
                    Flags00 = 0x7B,
                },
                [32] = new TR1Object
                {
                    ObjectId = 32,
                    Flags00 = 0x7B,
                },
                [33] = new TR1Object
                {
                    ObjectId = 33,
                    Flags00 = 0x7B,
                },
                [34] = new TR1Object
                {
                    ObjectId = 34,
                    Flags00 = 0x7B,
                },
                [35] = new TR1Object
                {
                    ObjectId = 35,
                    Flags00 = 0x69,
                },
                [36] = new TR1Object
                {
                    ObjectId = 36,
                    Flags00 = 0x61,
                },
                [37] = new TR1Object
                {
                    ObjectId = 37,
                    Flags00 = 0x01,
                },
                [38] = new TR1Object
                {
                    ObjectId = 38,
                    Flags00 = 0x69,
                },
                [39] = new TR1Object
                {
                    ObjectId = 39,
                    Flags00 = 0x20,
                },
                [40] = new TR1Object
                {
                    ObjectId = 40,
                    Flags00 = 0x20,
                },
                [41] = new TR1Object
                {
                    ObjectId = 41,
                    Flags00 = 0x00,
                },
                [42] = new TR1Object
                {
                    ObjectId = 42,
                    Flags00 = 0x60,
                },
                [43] = new TR1Object
                {
                    ObjectId = 43,
                    Flags00 = 0x69,
                },
                [44] = new TR1Object
                {
                    ObjectId = 44,
                    Flags00 = 0x61,
                },
                [45] = new TR1Object
                {
                    ObjectId = 45,
                    Flags00 = 0x61,
                },
                [46] = new TR1Object
                {
                    ObjectId = 46,
                    Flags00 = 0x21,
                },
                [47] = new TR1Object
                {
                    ObjectId = 47,
                    Flags00 = 0x69,
                },
                [48] = new TR1Object
                {
                    ObjectId = 48,
                    Flags00 = 0x69,
                },
                [49] = new TR1Object
                {
                    ObjectId = 49,
                    Flags00 = 0x68,
                },
                [50] = new TR1Object
                {
                    ObjectId = 50,
                    Flags00 = 0x68,
                },
                [51] = new TR1Object
                {
                    ObjectId = 51,
                    Flags00 = 0x68,
                },
                [52] = new TR1Object
                {
                    ObjectId = 52,
                    Flags00 = 0x68,
                },
                [53] = new TR1Object
                {
                    ObjectId = 53,
                    Flags00 = 0x68,
                },
                [54] = new TR1Object
                {
                    ObjectId = 54,
                    Flags00 = 0x69,
                },
                [55] = new TR1Object
                {
                    ObjectId = 55,
                    Flags00 = 0x61,
                },
                [56] = new TR1Object
                {
                    ObjectId = 56,
                    Flags00 = 0x61,
                },
                [57] = new TR1Object
                {
                    ObjectId = 57,
                    Flags00 = 0x61,
                },
                [58] = new TR1Object
                {
                    ObjectId = 58,
                    Flags00 = 0x61,
                },
                [59] = new TR1Object
                {
                    ObjectId = 59,
                    Flags00 = 0x61,
                },
                [60] = new TR1Object
                {
                    ObjectId = 60,
                    Flags00 = 0x61,
                },
                [61] = new TR1Object
                {
                    ObjectId = 61,
                    Flags00 = 0x61,
                },
                [62] = new TR1Object
                {
                    ObjectId = 62,
                    Flags00 = 0x61,
                },
                [63] = new TR1Object
                {
                    ObjectId = 63,
                    Flags00 = 0x61,
                },
                [64] = new TR1Object
                {
                    ObjectId = 64,
                    Flags00 = 0x60,
                },
                [65] = new TR1Object
                {
                    ObjectId = 65,
                    Flags00 = 0x61,
                },
                [66] = new TR1Object
                {
                    ObjectId = 66,
                    Flags00 = 0x60,
                },
                [67] = new TR1Object
                {
                    ObjectId = 67,
                    Flags00 = 0x00,
                },
                [68] = new TR1Object
                {
                    ObjectId = 68,
                    Flags00 = 0x00,
                },
                [69] = new TR1Object
                {
                    ObjectId = 69,
                    Flags00 = 0x00,
                },
                [70] = new TR1Object
                {
                    ObjectId = 70,
                    Flags00 = 0x00,
                },
                [71] = new TR1Object
                {
                    ObjectId = 71,
                    Flags00 = 0x01,
                },
                [72] = new TR1Object
                {
                    ObjectId = 72,
                    Flags00 = 0x01,
                },
                [73] = new TR1Object
                {
                    ObjectId = 73,
                    Flags00 = 0x00,
                },
                [74] = new TR1Object
                {
                    ObjectId = 74,
                    Flags00 = 0x20,
                },
                [75] = new TR1Object
                {
                    ObjectId = 75,
                    Flags00 = 0x20,
                },
                [76] = new TR1Object
                {
                    ObjectId = 76,
                    Flags00 = 0x20,
                },
                [77] = new TR1Object
                {
                    ObjectId = 77,
                    Flags00 = 0x00,
                },
                [78] = new TR1Object
                {
                    ObjectId = 78,
                    Flags00 = 0x00,
                },
                [79] = new TR1Object
                {
                    ObjectId = 79,
                    Flags00 = 0x00,
                },
                [80] = new TR1Object
                {
                    ObjectId = 80,
                    Flags00 = 0x00,
                },
                [81] = new TR1Object
                {
                    ObjectId = 81,
                    Flags00 = 0x01,
                },
                [82] = new TR1Object
                {
                    ObjectId = 82,
                    Flags00 = 0x01,
                },
                [83] = new TR1Object
                {
                    ObjectId = 83,
                    Flags00 = 0x21,
                },
                [84] = new TR1Object
                {
                    ObjectId = 84,
                    Flags00 = 0x21,
                },
                [85] = new TR1Object
                {
                    ObjectId = 85,
                    Flags00 = 0x21,
                },
                [86] = new TR1Object
                {
                    ObjectId = 86,
                    Flags00 = 0x21,
                },
                [87] = new TR1Object
                {
                    ObjectId = 87,
                    Flags00 = 0x21,
                },
                [88] = new TR1Object
                {
                    ObjectId = 88,
                    Flags00 = 0x20,
                },
                [89] = new TR1Object
                {
                    ObjectId = 89,
                    Flags00 = 0x21,
                },
                [90] = new TR1Object
                {
                    ObjectId = 90,
                    Flags00 = 0x21,
                },
                [91] = new TR1Object
                {
                    ObjectId = 91,
                    Flags00 = 0x21,
                },
                [92] = new TR1Object
                {
                    ObjectId = 92,
                    Flags00 = 0x20,
                },
                [93] = new TR1Object
                {
                    ObjectId = 93,
                    Flags00 = 0x21,
                },
                [94] = new TR1Object
                {
                    ObjectId = 94,
                    Flags00 = 0x21,
                },
                [95] = new TR1Object
                {
                    ObjectId = 95,
                    Flags00 = 0x01,
                },
                [96] = new TR1Object
                {
                    ObjectId = 96,
                    Flags00 = 0x01,
                },
                [97] = new TR1Object
                {
                    ObjectId = 97,
                    Flags00 = 0x01,
                },
                [98] = new TR1Object
                {
                    ObjectId = 98,
                    Flags00 = 0x00,
                },
                [99] = new TR1Object
                {
                    ObjectId = 99,
                    Flags00 = 0x01,
                },
                [100] = new TR1Object
                {
                    ObjectId = 100,
                    Flags00 = 0x01,
                },
                [101] = new TR1Object
                {
                    ObjectId = 101,
                    Flags00 = 0x01,
                },
                [102] = new TR1Object
                {
                    ObjectId = 102,
                    Flags00 = 0x01,
                },
                [103] = new TR1Object
                {
                    ObjectId = 103,
                    Flags00 = 0x01,
                },
                [104] = new TR1Object
                {
                    ObjectId = 104,
                    Flags00 = 0x01,
                },
                [105] = new TR1Object
                {
                    ObjectId = 105,
                    Flags00 = 0x01,
                },
                [106] = new TR1Object
                {
                    ObjectId = 106,
                    Flags00 = 0x01,
                },
                [107] = new TR1Object
                {
                    ObjectId = 107,
                    Flags00 = 0x00,
                },
                [108] = new TR1Object
                {
                    ObjectId = 108,
                    Flags00 = 0x01,
                },
                [109] = new TR1Object
                {
                    ObjectId = 109,
                    Flags00 = 0x01,
                },
                [110] = new TR1Object
                {
                    ObjectId = 110,
                    Flags00 = 0x20,
                },
                [111] = new TR1Object
                {
                    ObjectId = 111,
                    Flags00 = 0x20,
                },
                [112] = new TR1Object
                {
                    ObjectId = 112,
                    Flags00 = 0x20,
                },
                [113] = new TR1Object
                {
                    ObjectId = 113,
                    Flags00 = 0x20,
                },
                [114] = new TR1Object
                {
                    ObjectId = 114,
                    Flags00 = 0x00,
                },
                [115] = new TR1Object
                {
                    ObjectId = 115,
                    Flags00 = 0x00,
                },
                [116] = new TR1Object
                {
                    ObjectId = 116,
                    Flags00 = 0x00,
                },
                [117] = new TR1Object
                {
                    ObjectId = 117,
                    Flags00 = 0x00,
                },
                [118] = new TR1Object
                {
                    ObjectId = 118,
                    Flags00 = 0x20,
                },
                [119] = new TR1Object
                {
                    ObjectId = 119,
                    Flags00 = 0x20,
                },
                [120] = new TR1Object
                {
                    ObjectId = 120,
                    Flags00 = 0x20,
                },
                [121] = new TR1Object
                {
                    ObjectId = 121,
                    Flags00 = 0x20,
                },
                [122] = new TR1Object
                {
                    ObjectId = 122,
                    Flags00 = 0x20,
                },
                [123] = new TR1Object
                {
                    ObjectId = 123,
                    Flags00 = 0x20,
                },
                [124] = new TR1Object
                {
                    ObjectId = 124,
                    Flags00 = 0x20,
                },
                [125] = new TR1Object
                {
                    ObjectId = 125,
                    Flags00 = 0x20,
                },
                [126] = new TR1Object
                {
                    ObjectId = 126,
                    Flags00 = 0x20,
                },
                [127] = new TR1Object
                {
                    ObjectId = 127,
                    Flags00 = 0x00,
                },
                [128] = new TR1Object
                {
                    ObjectId = 128,
                    Flags00 = 0x00,
                },
                [129] = new TR1Object
                {
                    ObjectId = 129,
                    Flags00 = 0x21,
                },
                [130] = new TR1Object
                {
                    ObjectId = 130,
                    Flags00 = 0x21,
                },
                [131] = new TR1Object
                {
                    ObjectId = 131,
                    Flags00 = 0x21,
                },
                [132] = new TR1Object
                {
                    ObjectId = 132,
                    Flags00 = 0x21,
                },
                [133] = new TR1Object
                {
                    ObjectId = 133,
                    Flags00 = 0x01,
                },
                [134] = new TR1Object
                {
                    ObjectId = 134,
                    Flags00 = 0x01,
                },
                [135] = new TR1Object
                {
                    ObjectId = 135,
                    Flags00 = 0x01,
                },
                [136] = new TR1Object
                {
                    ObjectId = 136,
                    Flags00 = 0x01,
                },
                [137] = new TR1Object
                {
                    ObjectId = 137,
                    Flags00 = 0x21,
                },
                [138] = new TR1Object
                {
                    ObjectId = 138,
                    Flags00 = 0x21,
                },
                [139] = new TR1Object
                {
                    ObjectId = 139,
                    Flags00 = 0x21,
                },
                [140] = new TR1Object
                {
                    ObjectId = 140,
                    Flags00 = 0x21,
                },
                [141] = new TR1Object
                {
                    ObjectId = 141,
                    Flags00 = 0x20,
                },
                [142] = new TR1Object
                {
                    ObjectId = 142,
                    Flags00 = 0x20,
                },
                [143] = new TR1Object
                {
                    ObjectId = 143,
                    Flags00 = 0x20,
                },
                [144] = new TR1Object
                {
                    ObjectId = 144,
                    Flags00 = 0x20,
                },
                [145] = new TR1Object
                {
                    ObjectId = 145,
                    Flags00 = 0x20,
                },
            },
            [6] = new Dictionary<int, TR1Object> // Colosseum
            {
                [0] = new TR1Object
                {
                    ObjectId = 0,
                    Flags00 = 0x79,
                },
                [1] = new TR1Object
                {
                    ObjectId = 1,
                    Flags00 = 0x01,
                },
                [2] = new TR1Object
                {
                    ObjectId = 2,
                    Flags00 = 0x01,
                },
                [3] = new TR1Object
                {
                    ObjectId = 3,
                    Flags00 = 0x01,
                },
                [4] = new TR1Object
                {
                    ObjectId = 4,
                    Flags00 = 0x01,
                },
                [5] = new TR1Object
                {
                    ObjectId = 5,
                    Flags00 = 0x01,
                },
                [6] = new TR1Object
                {
                    ObjectId = 6,
                    Flags00 = 0x78,
                },
                [7] = new TR1Object
                {
                    ObjectId = 7,
                    Flags00 = 0x7B,
                },
                [8] = new TR1Object
                {
                    ObjectId = 8,
                    Flags00 = 0x7B,
                },
                [9] = new TR1Object
                {
                    ObjectId = 9,
                    Flags00 = 0x7B,
                },
                [10] = new TR1Object
                {
                    ObjectId = 10,
                    Flags00 = 0x7B,
                },
                [11] = new TR1Object
                {
                    ObjectId = 11,
                    Flags00 = 0x7B,
                },
                [12] = new TR1Object
                {
                    ObjectId = 12,
                    Flags00 = 0x7B,
                },
                [13] = new TR1Object
                {
                    ObjectId = 13,
                    Flags00 = 0x7B,
                },
                [14] = new TR1Object
                {
                    ObjectId = 14,
                    Flags00 = 0x7B,
                },
                [15] = new TR1Object
                {
                    ObjectId = 15,
                    Flags00 = 0x7B,
                },
                [16] = new TR1Object
                {
                    ObjectId = 16,
                    Flags00 = 0x7B,
                },
                [17] = new TR1Object
                {
                    ObjectId = 17,
                    Flags00 = 0x7B,
                },
                [18] = new TR1Object
                {
                    ObjectId = 18,
                    Flags00 = 0x7B,
                },
                [19] = new TR1Object
                {
                    ObjectId = 19,
                    Flags00 = 0x7B,
                },
                [20] = new TR1Object
                {
                    ObjectId = 20,
                    Flags00 = 0x7B,
                },
                [21] = new TR1Object
                {
                    ObjectId = 21,
                    Flags00 = 0x7B,
                },
                [22] = new TR1Object
                {
                    ObjectId = 22,
                    Flags00 = 0x7B,
                },
                [23] = new TR1Object
                {
                    ObjectId = 23,
                    Flags00 = 0x7B,
                },
                [24] = new TR1Object
                {
                    ObjectId = 24,
                    Flags00 = 0x71,
                },
                [25] = new TR1Object
                {
                    ObjectId = 25,
                    Flags00 = 0x00,
                },
                [26] = new TR1Object
                {
                    ObjectId = 26,
                    Flags00 = 0x00,
                },
                [27] = new TR1Object
                {
                    ObjectId = 27,
                    Flags00 = 0x7B,
                },
                [28] = new TR1Object
                {
                    ObjectId = 28,
                    Flags00 = 0x7B,
                },
                [29] = new TR1Object
                {
                    ObjectId = 29,
                    Flags00 = 0x01,
                },
                [30] = new TR1Object
                {
                    ObjectId = 30,
                    Flags00 = 0x7B,
                },
                [31] = new TR1Object
                {
                    ObjectId = 31,
                    Flags00 = 0x7B,
                },
                [32] = new TR1Object
                {
                    ObjectId = 32,
                    Flags00 = 0x7B,
                },
                [33] = new TR1Object
                {
                    ObjectId = 33,
                    Flags00 = 0x7B,
                },
                [34] = new TR1Object
                {
                    ObjectId = 34,
                    Flags00 = 0x7B,
                },
                [35] = new TR1Object
                {
                    ObjectId = 35,
                    Flags00 = 0x69,
                },
                [36] = new TR1Object
                {
                    ObjectId = 36,
                    Flags00 = 0x60,
                },
                [37] = new TR1Object
                {
                    ObjectId = 37,
                    Flags00 = 0x01,
                },
                [38] = new TR1Object
                {
                    ObjectId = 38,
                    Flags00 = 0x69,
                },
                [39] = new TR1Object
                {
                    ObjectId = 39,
                    Flags00 = 0x20,
                },
                [40] = new TR1Object
                {
                    ObjectId = 40,
                    Flags00 = 0x20,
                },
                [41] = new TR1Object
                {
                    ObjectId = 41,
                    Flags00 = 0x00,
                },
                [42] = new TR1Object
                {
                    ObjectId = 42,
                    Flags00 = 0x60,
                },
                [43] = new TR1Object
                {
                    ObjectId = 43,
                    Flags00 = 0x68,
                },
                [44] = new TR1Object
                {
                    ObjectId = 44,
                    Flags00 = 0x60,
                },
                [45] = new TR1Object
                {
                    ObjectId = 45,
                    Flags00 = 0x60,
                },
                [46] = new TR1Object
                {
                    ObjectId = 46,
                    Flags00 = 0x20,
                },
                [47] = new TR1Object
                {
                    ObjectId = 47,
                    Flags00 = 0x68,
                },
                [48] = new TR1Object
                {
                    ObjectId = 48,
                    Flags00 = 0x69,
                },
                [49] = new TR1Object
                {
                    ObjectId = 49,
                    Flags00 = 0x69,
                },
                [50] = new TR1Object
                {
                    ObjectId = 50,
                    Flags00 = 0x68,
                },
                [51] = new TR1Object
                {
                    ObjectId = 51,
                    Flags00 = 0x68,
                },
                [52] = new TR1Object
                {
                    ObjectId = 52,
                    Flags00 = 0x68,
                },
                [53] = new TR1Object
                {
                    ObjectId = 53,
                    Flags00 = 0x68,
                },
                [54] = new TR1Object
                {
                    ObjectId = 54,
                    Flags00 = 0x68,
                },
                [55] = new TR1Object
                {
                    ObjectId = 55,
                    Flags00 = 0x61,
                },
                [56] = new TR1Object
                {
                    ObjectId = 56,
                    Flags00 = 0x60,
                },
                [57] = new TR1Object
                {
                    ObjectId = 57,
                    Flags00 = 0x61,
                },
                [58] = new TR1Object
                {
                    ObjectId = 58,
                    Flags00 = 0x61,
                },
                [59] = new TR1Object
                {
                    ObjectId = 59,
                    Flags00 = 0x61,
                },
                [60] = new TR1Object
                {
                    ObjectId = 60,
                    Flags00 = 0x61,
                },
                [61] = new TR1Object
                {
                    ObjectId = 61,
                    Flags00 = 0x61,
                },
                [62] = new TR1Object
                {
                    ObjectId = 62,
                    Flags00 = 0x61,
                },
                [63] = new TR1Object
                {
                    ObjectId = 63,
                    Flags00 = 0x61,
                },
                [64] = new TR1Object
                {
                    ObjectId = 64,
                    Flags00 = 0x61,
                },
                [65] = new TR1Object
                {
                    ObjectId = 65,
                    Flags00 = 0x60,
                },
                [66] = new TR1Object
                {
                    ObjectId = 66,
                    Flags00 = 0x60,
                },
                [67] = new TR1Object
                {
                    ObjectId = 67,
                    Flags00 = 0x00,
                },
                [68] = new TR1Object
                {
                    ObjectId = 68,
                    Flags00 = 0x00,
                },
                [69] = new TR1Object
                {
                    ObjectId = 69,
                    Flags00 = 0x00,
                },
                [70] = new TR1Object
                {
                    ObjectId = 70,
                    Flags00 = 0x00,
                },
                [71] = new TR1Object
                {
                    ObjectId = 71,
                    Flags00 = 0x01,
                },
                [72] = new TR1Object
                {
                    ObjectId = 72,
                    Flags00 = 0x01,
                },
                [73] = new TR1Object
                {
                    ObjectId = 73,
                    Flags00 = 0x00,
                },
                [74] = new TR1Object
                {
                    ObjectId = 74,
                    Flags00 = 0x20,
                },
                [75] = new TR1Object
                {
                    ObjectId = 75,
                    Flags00 = 0x20,
                },
                [76] = new TR1Object
                {
                    ObjectId = 76,
                    Flags00 = 0x20,
                },
                [77] = new TR1Object
                {
                    ObjectId = 77,
                    Flags00 = 0x00,
                },
                [78] = new TR1Object
                {
                    ObjectId = 78,
                    Flags00 = 0x00,
                },
                [79] = new TR1Object
                {
                    ObjectId = 79,
                    Flags00 = 0x00,
                },
                [80] = new TR1Object
                {
                    ObjectId = 80,
                    Flags00 = 0x00,
                },
                [81] = new TR1Object
                {
                    ObjectId = 81,
                    Flags00 = 0x01,
                },
                [82] = new TR1Object
                {
                    ObjectId = 82,
                    Flags00 = 0x01,
                },
                [83] = new TR1Object
                {
                    ObjectId = 83,
                    Flags00 = 0x21,
                },
                [84] = new TR1Object
                {
                    ObjectId = 84,
                    Flags00 = 0x21,
                },
                [85] = new TR1Object
                {
                    ObjectId = 85,
                    Flags00 = 0x21,
                },
                [86] = new TR1Object
                {
                    ObjectId = 86,
                    Flags00 = 0x21,
                },
                [87] = new TR1Object
                {
                    ObjectId = 87,
                    Flags00 = 0x21,
                },
                [88] = new TR1Object
                {
                    ObjectId = 88,
                    Flags00 = 0x20,
                },
                [89] = new TR1Object
                {
                    ObjectId = 89,
                    Flags00 = 0x21,
                },
                [90] = new TR1Object
                {
                    ObjectId = 90,
                    Flags00 = 0x21,
                },
                [91] = new TR1Object
                {
                    ObjectId = 91,
                    Flags00 = 0x21,
                },
                [92] = new TR1Object
                {
                    ObjectId = 92,
                    Flags00 = 0x20,
                },
                [93] = new TR1Object
                {
                    ObjectId = 93,
                    Flags00 = 0x21,
                },
                [94] = new TR1Object
                {
                    ObjectId = 94,
                    Flags00 = 0x21,
                },
                [95] = new TR1Object
                {
                    ObjectId = 95,
                    Flags00 = 0x01,
                },
                [96] = new TR1Object
                {
                    ObjectId = 96,
                    Flags00 = 0x01,
                },
                [97] = new TR1Object
                {
                    ObjectId = 97,
                    Flags00 = 0x01,
                },
                [98] = new TR1Object
                {
                    ObjectId = 98,
                    Flags00 = 0x00,
                },
                [99] = new TR1Object
                {
                    ObjectId = 99,
                    Flags00 = 0x01,
                },
                [100] = new TR1Object
                {
                    ObjectId = 100,
                    Flags00 = 0x01,
                },
                [101] = new TR1Object
                {
                    ObjectId = 101,
                    Flags00 = 0x01,
                },
                [102] = new TR1Object
                {
                    ObjectId = 102,
                    Flags00 = 0x01,
                },
                [103] = new TR1Object
                {
                    ObjectId = 103,
                    Flags00 = 0x01,
                },
                [104] = new TR1Object
                {
                    ObjectId = 104,
                    Flags00 = 0x01,
                },
                [105] = new TR1Object
                {
                    ObjectId = 105,
                    Flags00 = 0x01,
                },
                [106] = new TR1Object
                {
                    ObjectId = 106,
                    Flags00 = 0x01,
                },
                [107] = new TR1Object
                {
                    ObjectId = 107,
                    Flags00 = 0x00,
                },
                [108] = new TR1Object
                {
                    ObjectId = 108,
                    Flags00 = 0x01,
                },
                [109] = new TR1Object
                {
                    ObjectId = 109,
                    Flags00 = 0x01,
                },
                [110] = new TR1Object
                {
                    ObjectId = 110,
                    Flags00 = 0x20,
                },
                [111] = new TR1Object
                {
                    ObjectId = 111,
                    Flags00 = 0x20,
                },
                [112] = new TR1Object
                {
                    ObjectId = 112,
                    Flags00 = 0x20,
                },
                [113] = new TR1Object
                {
                    ObjectId = 113,
                    Flags00 = 0x20,
                },
                [114] = new TR1Object
                {
                    ObjectId = 114,
                    Flags00 = 0x00,
                },
                [115] = new TR1Object
                {
                    ObjectId = 115,
                    Flags00 = 0x00,
                },
                [116] = new TR1Object
                {
                    ObjectId = 116,
                    Flags00 = 0x00,
                },
                [117] = new TR1Object
                {
                    ObjectId = 117,
                    Flags00 = 0x00,
                },
                [118] = new TR1Object
                {
                    ObjectId = 118,
                    Flags00 = 0x20,
                },
                [119] = new TR1Object
                {
                    ObjectId = 119,
                    Flags00 = 0x20,
                },
                [120] = new TR1Object
                {
                    ObjectId = 120,
                    Flags00 = 0x20,
                },
                [121] = new TR1Object
                {
                    ObjectId = 121,
                    Flags00 = 0x20,
                },
                [122] = new TR1Object
                {
                    ObjectId = 122,
                    Flags00 = 0x20,
                },
                [123] = new TR1Object
                {
                    ObjectId = 123,
                    Flags00 = 0x20,
                },
                [124] = new TR1Object
                {
                    ObjectId = 124,
                    Flags00 = 0x20,
                },
                [125] = new TR1Object
                {
                    ObjectId = 125,
                    Flags00 = 0x20,
                },
                [126] = new TR1Object
                {
                    ObjectId = 126,
                    Flags00 = 0x20,
                },
                [127] = new TR1Object
                {
                    ObjectId = 127,
                    Flags00 = 0x00,
                },
                [128] = new TR1Object
                {
                    ObjectId = 128,
                    Flags00 = 0x00,
                },
                [129] = new TR1Object
                {
                    ObjectId = 129,
                    Flags00 = 0x21,
                },
                [130] = new TR1Object
                {
                    ObjectId = 130,
                    Flags00 = 0x20,
                },
                [131] = new TR1Object
                {
                    ObjectId = 131,
                    Flags00 = 0x20,
                },
                [132] = new TR1Object
                {
                    ObjectId = 132,
                    Flags00 = 0x20,
                },
                [133] = new TR1Object
                {
                    ObjectId = 133,
                    Flags00 = 0x01,
                },
                [134] = new TR1Object
                {
                    ObjectId = 134,
                    Flags00 = 0x00,
                },
                [135] = new TR1Object
                {
                    ObjectId = 135,
                    Flags00 = 0x00,
                },
                [136] = new TR1Object
                {
                    ObjectId = 136,
                    Flags00 = 0x00,
                },
                [137] = new TR1Object
                {
                    ObjectId = 137,
                    Flags00 = 0x21,
                },
                [138] = new TR1Object
                {
                    ObjectId = 138,
                    Flags00 = 0x20,
                },
                [139] = new TR1Object
                {
                    ObjectId = 139,
                    Flags00 = 0x20,
                },
                [140] = new TR1Object
                {
                    ObjectId = 140,
                    Flags00 = 0x20,
                },
                [141] = new TR1Object
                {
                    ObjectId = 141,
                    Flags00 = 0x20,
                },
                [142] = new TR1Object
                {
                    ObjectId = 142,
                    Flags00 = 0x20,
                },
                [143] = new TR1Object
                {
                    ObjectId = 143,
                    Flags00 = 0x21,
                },
                [144] = new TR1Object
                {
                    ObjectId = 144,
                    Flags00 = 0x20,
                },
                [145] = new TR1Object
                {
                    ObjectId = 145,
                    Flags00 = 0x20,
                },
                [146] = new TR1Object
                {
                    ObjectId = 146,
                    Flags00 = 0x20,
                },
                [147] = new TR1Object
                {
                    ObjectId = 147,
                    Flags00 = 0x60,
                },
                [148] = new TR1Object
                {
                    ObjectId = 148,
                    Flags00 = 0x00,
                },
                [149] = new TR1Object
                {
                    ObjectId = 149,
                    Flags00 = 0x00,
                },
                [150] = new TR1Object
                {
                    ObjectId = 150,
                    Flags00 = 0x01,
                },
                [151] = new TR1Object
                {
                    ObjectId = 151,
                    Flags00 = 0x00,
                },
                [152] = new TR1Object
                {
                    ObjectId = 152,
                    Flags00 = 0x00,
                },
                [153] = new TR1Object
                {
                    ObjectId = 153,
                    Flags00 = 0x01,
                },
                [154] = new TR1Object
                {
                    ObjectId = 154,
                    Flags00 = 0x00,
                },
                [155] = new TR1Object
                {
                    ObjectId = 155,
                    Flags00 = 0x01,
                },
                [156] = new TR1Object
                {
                    ObjectId = 156,
                    Flags00 = 0x01,
                },
                [157] = new TR1Object
                {
                    ObjectId = 157,
                    Flags00 = 0x00,
                },
                [158] = new TR1Object
                {
                    ObjectId = 158,
                    Flags00 = 0x01,
                },
                [159] = new TR1Object
                {
                    ObjectId = 159,
                    Flags00 = 0x00,
                },
                [160] = new TR1Object
                {
                    ObjectId = 160,
                    Flags00 = 0x00,
                },
                [161] = new TR1Object
                {
                    ObjectId = 161,
                    Flags00 = 0x00,
                },
                [162] = new TR1Object
                {
                    ObjectId = 162,
                    Flags00 = 0x60,
                },
                [163] = new TR1Object
                {
                    ObjectId = 163,
                    Flags00 = 0x00,
                },
                [164] = new TR1Object
                {
                    ObjectId = 164,
                    Flags00 = 0x01,
                },
                [165] = new TR1Object
                {
                    ObjectId = 165,
                    Flags00 = 0x00,
                },
                [166] = new TR1Object
                {
                    ObjectId = 166,
                    Flags00 = 0x01,
                },
                [167] = new TR1Object
                {
                    ObjectId = 167,
                    Flags00 = 0x00,
                },
                [168] = new TR1Object
                {
                    ObjectId = 168,
                    Flags00 = 0x01,
                },
                [169] = new TR1Object
                {
                    ObjectId = 169,
                    Flags00 = 0x01,
                },
                [170] = new TR1Object
                {
                    ObjectId = 170,
                    Flags00 = 0x20,
                },
            },
            [7] = new Dictionary<int, TR1Object> // Palace Midas
            {
                [0] = new TR1Object
                {
                    ObjectId = 0,
                    Flags00 = 0x79,
                },
                [1] = new TR1Object
                {
                    ObjectId = 1,
                    Flags00 = 0x01,
                },
                [2] = new TR1Object
                {
                    ObjectId = 2,
                    Flags00 = 0x01,
                },
                [3] = new TR1Object
                {
                    ObjectId = 3,
                    Flags00 = 0x01,
                },
                [4] = new TR1Object
                {
                    ObjectId = 4,
                    Flags00 = 0x01,
                },
                [5] = new TR1Object
                {
                    ObjectId = 5,
                    Flags00 = 0x01,
                },
                [6] = new TR1Object
                {
                    ObjectId = 6,
                    Flags00 = 0x78,
                },
                [7] = new TR1Object
                {
                    ObjectId = 7,
                    Flags00 = 0x7B,
                },
                [8] = new TR1Object
                {
                    ObjectId = 8,
                    Flags00 = 0x7B,
                },
                [9] = new TR1Object
                {
                    ObjectId = 9,
                    Flags00 = 0x7B,
                },
                [10] = new TR1Object
                {
                    ObjectId = 10,
                    Flags00 = 0x7B,
                },
                [11] = new TR1Object
                {
                    ObjectId = 11,
                    Flags00 = 0x7B,
                },
                [12] = new TR1Object
                {
                    ObjectId = 12,
                    Flags00 = 0x7B,
                },
                [13] = new TR1Object
                {
                    ObjectId = 13,
                    Flags00 = 0x7B,
                },
                [14] = new TR1Object
                {
                    ObjectId = 14,
                    Flags00 = 0x7B,
                },
                [15] = new TR1Object
                {
                    ObjectId = 15,
                    Flags00 = 0x7B,
                },
                [16] = new TR1Object
                {
                    ObjectId = 16,
                    Flags00 = 0x7B,
                },
                [17] = new TR1Object
                {
                    ObjectId = 17,
                    Flags00 = 0x7B,
                },
                [18] = new TR1Object
                {
                    ObjectId = 18,
                    Flags00 = 0x7B,
                },
                [19] = new TR1Object
                {
                    ObjectId = 19,
                    Flags00 = 0x7B,
                },
                [20] = new TR1Object
                {
                    ObjectId = 20,
                    Flags00 = 0x7B,
                },
                [21] = new TR1Object
                {
                    ObjectId = 21,
                    Flags00 = 0x7B,
                },
                [22] = new TR1Object
                {
                    ObjectId = 22,
                    Flags00 = 0x7B,
                },
                [23] = new TR1Object
                {
                    ObjectId = 23,
                    Flags00 = 0x7B,
                },
                [24] = new TR1Object
                {
                    ObjectId = 24,
                    Flags00 = 0x71,
                },
                [25] = new TR1Object
                {
                    ObjectId = 25,
                    Flags00 = 0x00,
                },
                [26] = new TR1Object
                {
                    ObjectId = 26,
                    Flags00 = 0x00,
                },
                [27] = new TR1Object
                {
                    ObjectId = 27,
                    Flags00 = 0x7B,
                },
                [28] = new TR1Object
                {
                    ObjectId = 28,
                    Flags00 = 0x7B,
                },
                [29] = new TR1Object
                {
                    ObjectId = 29,
                    Flags00 = 0x01,
                },
                [30] = new TR1Object
                {
                    ObjectId = 30,
                    Flags00 = 0x7B,
                },
                [31] = new TR1Object
                {
                    ObjectId = 31,
                    Flags00 = 0x7B,
                },
                [32] = new TR1Object
                {
                    ObjectId = 32,
                    Flags00 = 0x7B,
                },
                [33] = new TR1Object
                {
                    ObjectId = 33,
                    Flags00 = 0x7B,
                },
                [34] = new TR1Object
                {
                    ObjectId = 34,
                    Flags00 = 0x7B,
                },
                [35] = new TR1Object
                {
                    ObjectId = 35,
                    Flags00 = 0x69,
                },
                [36] = new TR1Object
                {
                    ObjectId = 36,
                    Flags00 = 0x60,
                },
                [37] = new TR1Object
                {
                    ObjectId = 37,
                    Flags00 = 0x01,
                },
                [38] = new TR1Object
                {
                    ObjectId = 38,
                    Flags00 = 0x68,
                },
                [39] = new TR1Object
                {
                    ObjectId = 39,
                    Flags00 = 0x20,
                },
                [40] = new TR1Object
                {
                    ObjectId = 40,
                    Flags00 = 0x20,
                },
                [41] = new TR1Object
                {
                    ObjectId = 41,
                    Flags00 = 0x00,
                },
                [42] = new TR1Object
                {
                    ObjectId = 42,
                    Flags00 = 0x61,
                },
                [43] = new TR1Object
                {
                    ObjectId = 43,
                    Flags00 = 0x68,
                },
                [44] = new TR1Object
                {
                    ObjectId = 44,
                    Flags00 = 0x60,
                },
                [45] = new TR1Object
                {
                    ObjectId = 45,
                    Flags00 = 0x60,
                },
                [46] = new TR1Object
                {
                    ObjectId = 46,
                    Flags00 = 0x20,
                },
                [47] = new TR1Object
                {
                    ObjectId = 47,
                    Flags00 = 0x68,
                },
                [48] = new TR1Object
                {
                    ObjectId = 48,
                    Flags00 = 0x69,
                },
                [49] = new TR1Object
                {
                    ObjectId = 49,
                    Flags00 = 0x69,
                },
                [50] = new TR1Object
                {
                    ObjectId = 50,
                    Flags00 = 0x68,
                },
                [51] = new TR1Object
                {
                    ObjectId = 51,
                    Flags00 = 0x68,
                },
                [52] = new TR1Object
                {
                    ObjectId = 52,
                    Flags00 = 0x68,
                },
                [53] = new TR1Object
                {
                    ObjectId = 53,
                    Flags00 = 0x68,
                },
                [54] = new TR1Object
                {
                    ObjectId = 54,
                    Flags00 = 0x68,
                },
                [55] = new TR1Object
                {
                    ObjectId = 55,
                    Flags00 = 0x61,
                },
                [56] = new TR1Object
                {
                    ObjectId = 56,
                    Flags00 = 0x60,
                },
                [57] = new TR1Object
                {
                    ObjectId = 57,
                    Flags00 = 0x61,
                },
                [58] = new TR1Object
                {
                    ObjectId = 58,
                    Flags00 = 0x61,
                },
                [59] = new TR1Object
                {
                    ObjectId = 59,
                    Flags00 = 0x61,
                },
                [60] = new TR1Object
                {
                    ObjectId = 60,
                    Flags00 = 0x61,
                },
                [61] = new TR1Object
                {
                    ObjectId = 61,
                    Flags00 = 0x61,
                },
                [62] = new TR1Object
                {
                    ObjectId = 62,
                    Flags00 = 0x61,
                },
                [63] = new TR1Object
                {
                    ObjectId = 63,
                    Flags00 = 0x61,
                },
                [64] = new TR1Object
                {
                    ObjectId = 64,
                    Flags00 = 0x60,
                },
                [65] = new TR1Object
                {
                    ObjectId = 65,
                    Flags00 = 0x60,
                },
                [66] = new TR1Object
                {
                    ObjectId = 66,
                    Flags00 = 0x60,
                },
                [67] = new TR1Object
                {
                    ObjectId = 67,
                    Flags00 = 0x00,
                },
                [68] = new TR1Object
                {
                    ObjectId = 68,
                    Flags00 = 0x00,
                },
                [69] = new TR1Object
                {
                    ObjectId = 69,
                    Flags00 = 0x00,
                },
                [70] = new TR1Object
                {
                    ObjectId = 70,
                    Flags00 = 0x00,
                },
                [71] = new TR1Object
                {
                    ObjectId = 71,
                    Flags00 = 0x01,
                },
                [72] = new TR1Object
                {
                    ObjectId = 72,
                    Flags00 = 0x01,
                },
                [73] = new TR1Object
                {
                    ObjectId = 73,
                    Flags00 = 0x00,
                },
                [74] = new TR1Object
                {
                    ObjectId = 74,
                    Flags00 = 0x20,
                },
                [75] = new TR1Object
                {
                    ObjectId = 75,
                    Flags00 = 0x20,
                },
                [76] = new TR1Object
                {
                    ObjectId = 76,
                    Flags00 = 0x20,
                },
                [77] = new TR1Object
                {
                    ObjectId = 77,
                    Flags00 = 0x00,
                },
                [78] = new TR1Object
                {
                    ObjectId = 78,
                    Flags00 = 0x00,
                },
                [79] = new TR1Object
                {
                    ObjectId = 79,
                    Flags00 = 0x00,
                },
                [80] = new TR1Object
                {
                    ObjectId = 80,
                    Flags00 = 0x00,
                },
                [81] = new TR1Object
                {
                    ObjectId = 81,
                    Flags00 = 0x01,
                },
                [82] = new TR1Object
                {
                    ObjectId = 82,
                    Flags00 = 0x01,
                },
                [83] = new TR1Object
                {
                    ObjectId = 83,
                    Flags00 = 0x21,
                },
                [84] = new TR1Object
                {
                    ObjectId = 84,
                    Flags00 = 0x21,
                },
                [85] = new TR1Object
                {
                    ObjectId = 85,
                    Flags00 = 0x21,
                },
                [86] = new TR1Object
                {
                    ObjectId = 86,
                    Flags00 = 0x21,
                },
                [87] = new TR1Object
                {
                    ObjectId = 87,
                    Flags00 = 0x21,
                },
                [88] = new TR1Object
                {
                    ObjectId = 88,
                    Flags00 = 0x20,
                },
                [89] = new TR1Object
                {
                    ObjectId = 89,
                    Flags00 = 0x21,
                },
                [90] = new TR1Object
                {
                    ObjectId = 90,
                    Flags00 = 0x21,
                },
                [91] = new TR1Object
                {
                    ObjectId = 91,
                    Flags00 = 0x21,
                },
                [92] = new TR1Object
                {
                    ObjectId = 92,
                    Flags00 = 0x20,
                },
                [93] = new TR1Object
                {
                    ObjectId = 93,
                    Flags00 = 0x21,
                },
                [94] = new TR1Object
                {
                    ObjectId = 94,
                    Flags00 = 0x21,
                },
                [95] = new TR1Object
                {
                    ObjectId = 95,
                    Flags00 = 0x01,
                },
                [96] = new TR1Object
                {
                    ObjectId = 96,
                    Flags00 = 0x01,
                },
                [97] = new TR1Object
                {
                    ObjectId = 97,
                    Flags00 = 0x01,
                },
                [98] = new TR1Object
                {
                    ObjectId = 98,
                    Flags00 = 0x00,
                },
                [99] = new TR1Object
                {
                    ObjectId = 99,
                    Flags00 = 0x01,
                },
                [100] = new TR1Object
                {
                    ObjectId = 100,
                    Flags00 = 0x01,
                },
                [101] = new TR1Object
                {
                    ObjectId = 101,
                    Flags00 = 0x01,
                },
                [102] = new TR1Object
                {
                    ObjectId = 102,
                    Flags00 = 0x01,
                },
                [103] = new TR1Object
                {
                    ObjectId = 103,
                    Flags00 = 0x01,
                },
                [104] = new TR1Object
                {
                    ObjectId = 104,
                    Flags00 = 0x01,
                },
                [105] = new TR1Object
                {
                    ObjectId = 105,
                    Flags00 = 0x01,
                },
                [106] = new TR1Object
                {
                    ObjectId = 106,
                    Flags00 = 0x01,
                },
                [107] = new TR1Object
                {
                    ObjectId = 107,
                    Flags00 = 0x00,
                },
                [108] = new TR1Object
                {
                    ObjectId = 108,
                    Flags00 = 0x01,
                },
                [109] = new TR1Object
                {
                    ObjectId = 109,
                    Flags00 = 0x01,
                },
                [110] = new TR1Object
                {
                    ObjectId = 110,
                    Flags00 = 0x21,
                },
                [111] = new TR1Object
                {
                    ObjectId = 111,
                    Flags00 = 0x20,
                },
                [112] = new TR1Object
                {
                    ObjectId = 112,
                    Flags00 = 0x20,
                },
                [113] = new TR1Object
                {
                    ObjectId = 113,
                    Flags00 = 0x20,
                },
                [114] = new TR1Object
                {
                    ObjectId = 114,
                    Flags00 = 0x01,
                },
                [115] = new TR1Object
                {
                    ObjectId = 115,
                    Flags00 = 0x00,
                },
                [116] = new TR1Object
                {
                    ObjectId = 116,
                    Flags00 = 0x00,
                },
                [117] = new TR1Object
                {
                    ObjectId = 117,
                    Flags00 = 0x00,
                },
                [118] = new TR1Object
                {
                    ObjectId = 118,
                    Flags00 = 0x21,
                },
                [119] = new TR1Object
                {
                    ObjectId = 119,
                    Flags00 = 0x20,
                },
                [120] = new TR1Object
                {
                    ObjectId = 120,
                    Flags00 = 0x20,
                },
                [121] = new TR1Object
                {
                    ObjectId = 121,
                    Flags00 = 0x20,
                },
                [122] = new TR1Object
                {
                    ObjectId = 122,
                    Flags00 = 0x21,
                },
                [123] = new TR1Object
                {
                    ObjectId = 123,
                    Flags00 = 0x20,
                },
                [124] = new TR1Object
                {
                    ObjectId = 124,
                    Flags00 = 0x20,
                },
                [125] = new TR1Object
                {
                    ObjectId = 125,
                    Flags00 = 0x20,
                },
                [126] = new TR1Object
                {
                    ObjectId = 126,
                    Flags00 = 0x21,
                },
                [127] = new TR1Object
                {
                    ObjectId = 127,
                    Flags00 = 0x01,
                },
                [128] = new TR1Object
                {
                    ObjectId = 128,
                    Flags00 = 0x01,
                },
                [129] = new TR1Object
                {
                    ObjectId = 129,
                    Flags00 = 0x20,
                },
                [130] = new TR1Object
                {
                    ObjectId = 130,
                    Flags00 = 0x20,
                },
                [131] = new TR1Object
                {
                    ObjectId = 131,
                    Flags00 = 0x20,
                },
                [132] = new TR1Object
                {
                    ObjectId = 132,
                    Flags00 = 0x20,
                },
                [133] = new TR1Object
                {
                    ObjectId = 133,
                    Flags00 = 0x00,
                },
                [134] = new TR1Object
                {
                    ObjectId = 134,
                    Flags00 = 0x00,
                },
                [135] = new TR1Object
                {
                    ObjectId = 135,
                    Flags00 = 0x00,
                },
                [136] = new TR1Object
                {
                    ObjectId = 136,
                    Flags00 = 0x00,
                },
                [137] = new TR1Object
                {
                    ObjectId = 137,
                    Flags00 = 0x20,
                },
                [138] = new TR1Object
                {
                    ObjectId = 138,
                    Flags00 = 0x20,
                },
                [139] = new TR1Object
                {
                    ObjectId = 139,
                    Flags00 = 0x20,
                },
                [140] = new TR1Object
                {
                    ObjectId = 140,
                    Flags00 = 0x20,
                },
                [141] = new TR1Object
                {
                    ObjectId = 141,
                    Flags00 = 0x20,
                },
                [142] = new TR1Object
                {
                    ObjectId = 142,
                    Flags00 = 0x20,
                },
                [143] = new TR1Object
                {
                    ObjectId = 143,
                    Flags00 = 0x21,
                },
                [144] = new TR1Object
                {
                    ObjectId = 144,
                    Flags00 = 0x20,
                },
                [145] = new TR1Object
                {
                    ObjectId = 145,
                    Flags00 = 0x20,
                },
                [146] = new TR1Object
                {
                    ObjectId = 146,
                    Flags00 = 0x20,
                },
                [147] = new TR1Object
                {
                    ObjectId = 147,
                    Flags00 = 0x60,
                },
                [148] = new TR1Object
                {
                    ObjectId = 148,
                    Flags00 = 0x00,
                },
                [149] = new TR1Object
                {
                    ObjectId = 149,
                    Flags00 = 0x00,
                },
                [150] = new TR1Object
                {
                    ObjectId = 150,
                    Flags00 = 0x01,
                },
                [151] = new TR1Object
                {
                    ObjectId = 151,
                    Flags00 = 0x00,
                },
                [152] = new TR1Object
                {
                    ObjectId = 152,
                    Flags00 = 0x00,
                },
                [153] = new TR1Object
                {
                    ObjectId = 153,
                    Flags00 = 0x01,
                },
                [154] = new TR1Object
                {
                    ObjectId = 154,
                    Flags00 = 0x00,
                },
                [155] = new TR1Object
                {
                    ObjectId = 155,
                    Flags00 = 0x01,
                },
                [156] = new TR1Object
                {
                    ObjectId = 156,
                    Flags00 = 0x01,
                },
                [157] = new TR1Object
                {
                    ObjectId = 157,
                    Flags00 = 0x00,
                },
                [158] = new TR1Object
                {
                    ObjectId = 158,
                    Flags00 = 0x01,
                },
                [159] = new TR1Object
                {
                    ObjectId = 159,
                    Flags00 = 0x00,
                },
                [160] = new TR1Object
                {
                    ObjectId = 160,
                    Flags00 = 0x00,
                },
                [161] = new TR1Object
                {
                    ObjectId = 161,
                    Flags00 = 0x00,
                },
                [162] = new TR1Object
                {
                    ObjectId = 162,
                    Flags00 = 0x60,
                },
                [163] = new TR1Object
                {
                    ObjectId = 163,
                    Flags00 = 0x00,
                },
                [164] = new TR1Object
                {
                    ObjectId = 164,
                    Flags00 = 0x01,
                },
                [165] = new TR1Object
                {
                    ObjectId = 165,
                    Flags00 = 0x01,
                },
                [166] = new TR1Object
                {
                    ObjectId = 166,
                    Flags00 = 0x01,
                },
                [167] = new TR1Object
                {
                    ObjectId = 167,
                    Flags00 = 0x00,
                },
                [168] = new TR1Object
                {
                    ObjectId = 168,
                    Flags00 = 0x01,
                },
                [169] = new TR1Object
                {
                    ObjectId = 169,
                    Flags00 = 0x01,
                },
                [170] = new TR1Object
                {
                    ObjectId = 170,
                    Flags00 = 0x20,
                },
                [171] = new TR1Object
                {
                    ObjectId = 171,
                    Flags00 = 0x00,
                },
                [172] = new TR1Object
                {
                    ObjectId = 172,
                    Flags00 = 0x00,
                },
                [173] = new TR1Object
                {
                    ObjectId = 173,
                    Flags00 = 0x00,
                },
                [174] = new TR1Object
                {
                    ObjectId = 174,
                    Flags00 = 0x00,
                },
                [175] = new TR1Object
                {
                    ObjectId = 175,
                    Flags00 = 0x00,
                },
                [176] = new TR1Object
                {
                    ObjectId = 176,
                    Flags00 = 0x00,
                },
                [177] = new TR1Object
                {
                    ObjectId = 177,
                    Flags00 = 0x20,
                },
                [178] = new TR1Object
                {
                    ObjectId = 178,
                    Flags00 = 0x01,
                },
                [179] = new TR1Object
                {
                    ObjectId = 179,
                    Flags00 = 0x21,
                },
                [180] = new TR1Object
                {
                    ObjectId = 180,
                    Flags00 = 0x68,
                },
            },
            [8] = new Dictionary<int, TR1Object> // The Cistern
            {
                [0] = new TR1Object
                {
                    ObjectId = 0,
                    Flags00 = 0x79,
                },
                [1] = new TR1Object
                {
                    ObjectId = 1,
                    Flags00 = 0x01,
                },
                [2] = new TR1Object
                {
                    ObjectId = 2,
                    Flags00 = 0x01,
                },
                [3] = new TR1Object
                {
                    ObjectId = 3,
                    Flags00 = 0x01,
                },
                [4] = new TR1Object
                {
                    ObjectId = 4,
                    Flags00 = 0x01,
                },
                [5] = new TR1Object
                {
                    ObjectId = 5,
                    Flags00 = 0x01,
                },
                [6] = new TR1Object
                {
                    ObjectId = 6,
                    Flags00 = 0x78,
                },
                [7] = new TR1Object
                {
                    ObjectId = 7,
                    Flags00 = 0x7B,
                },
                [8] = new TR1Object
                {
                    ObjectId = 8,
                    Flags00 = 0x7B,
                },
                [9] = new TR1Object
                {
                    ObjectId = 9,
                    Flags00 = 0x7B,
                },
                [10] = new TR1Object
                {
                    ObjectId = 10,
                    Flags00 = 0x7B,
                },
                [11] = new TR1Object
                {
                    ObjectId = 11,
                    Flags00 = 0x7B,
                },
                [12] = new TR1Object
                {
                    ObjectId = 12,
                    Flags00 = 0x7B,
                },
                [13] = new TR1Object
                {
                    ObjectId = 13,
                    Flags00 = 0x7B,
                },
                [14] = new TR1Object
                {
                    ObjectId = 14,
                    Flags00 = 0x7B,
                },
                [15] = new TR1Object
                {
                    ObjectId = 15,
                    Flags00 = 0x7B,
                },
                [16] = new TR1Object
                {
                    ObjectId = 16,
                    Flags00 = 0x7B,
                },
                [17] = new TR1Object
                {
                    ObjectId = 17,
                    Flags00 = 0x7B,
                },
                [18] = new TR1Object
                {
                    ObjectId = 18,
                    Flags00 = 0x7B,
                },
                [19] = new TR1Object
                {
                    ObjectId = 19,
                    Flags00 = 0x7B,
                },
                [20] = new TR1Object
                {
                    ObjectId = 20,
                    Flags00 = 0x7B,
                },
                [21] = new TR1Object
                {
                    ObjectId = 21,
                    Flags00 = 0x7B,
                },
                [22] = new TR1Object
                {
                    ObjectId = 22,
                    Flags00 = 0x7B,
                },
                [23] = new TR1Object
                {
                    ObjectId = 23,
                    Flags00 = 0x7B,
                },
                [24] = new TR1Object
                {
                    ObjectId = 24,
                    Flags00 = 0x71,
                },
                [25] = new TR1Object
                {
                    ObjectId = 25,
                    Flags00 = 0x00,
                },
                [26] = new TR1Object
                {
                    ObjectId = 26,
                    Flags00 = 0x00,
                },
                [27] = new TR1Object
                {
                    ObjectId = 27,
                    Flags00 = 0x7B,
                },
                [28] = new TR1Object
                {
                    ObjectId = 28,
                    Flags00 = 0x7B,
                },
                [29] = new TR1Object
                {
                    ObjectId = 29,
                    Flags00 = 0x01,
                },
                [30] = new TR1Object
                {
                    ObjectId = 30,
                    Flags00 = 0x7B,
                },
                [31] = new TR1Object
                {
                    ObjectId = 31,
                    Flags00 = 0x7B,
                },
                [32] = new TR1Object
                {
                    ObjectId = 32,
                    Flags00 = 0x7B,
                },
                [33] = new TR1Object
                {
                    ObjectId = 33,
                    Flags00 = 0x7B,
                },
                [34] = new TR1Object
                {
                    ObjectId = 34,
                    Flags00 = 0x00,
                },
                [35] = new TR1Object
                {
                    ObjectId = 35,
                    Flags00 = 0x69,
                },
                [36] = new TR1Object
                {
                    ObjectId = 36,
                    Flags00 = 0x60,
                },
                [37] = new TR1Object
                {
                    ObjectId = 37,
                    Flags00 = 0x01,
                },
                [38] = new TR1Object
                {
                    ObjectId = 38,
                    Flags00 = 0x68,
                },
                [39] = new TR1Object
                {
                    ObjectId = 39,
                    Flags00 = 0x20,
                },
                [40] = new TR1Object
                {
                    ObjectId = 40,
                    Flags00 = 0x20,
                },
                [41] = new TR1Object
                {
                    ObjectId = 41,
                    Flags00 = 0x00,
                },
                [42] = new TR1Object
                {
                    ObjectId = 42,
                    Flags00 = 0x60,
                },
                [43] = new TR1Object
                {
                    ObjectId = 43,
                    Flags00 = 0x68,
                },
                [44] = new TR1Object
                {
                    ObjectId = 44,
                    Flags00 = 0x60,
                },
                [45] = new TR1Object
                {
                    ObjectId = 45,
                    Flags00 = 0x60,
                },
                [46] = new TR1Object
                {
                    ObjectId = 46,
                    Flags00 = 0x20,
                },
                [47] = new TR1Object
                {
                    ObjectId = 47,
                    Flags00 = 0x68,
                },
                [48] = new TR1Object
                {
                    ObjectId = 48,
                    Flags00 = 0x69,
                },
                [49] = new TR1Object
                {
                    ObjectId = 49,
                    Flags00 = 0x69,
                },
                [50] = new TR1Object
                {
                    ObjectId = 50,
                    Flags00 = 0x68,
                },
                [51] = new TR1Object
                {
                    ObjectId = 51,
                    Flags00 = 0x68,
                },
                [52] = new TR1Object
                {
                    ObjectId = 52,
                    Flags00 = 0x68,
                },
                [53] = new TR1Object
                {
                    ObjectId = 53,
                    Flags00 = 0x68,
                },
                [54] = new TR1Object
                {
                    ObjectId = 54,
                    Flags00 = 0x68,
                },
                [55] = new TR1Object
                {
                    ObjectId = 55,
                    Flags00 = 0x61,
                },
                [56] = new TR1Object
                {
                    ObjectId = 56,
                    Flags00 = 0x61,
                },
                [57] = new TR1Object
                {
                    ObjectId = 57,
                    Flags00 = 0x61,
                },
                [58] = new TR1Object
                {
                    ObjectId = 58,
                    Flags00 = 0x61,
                },
                [59] = new TR1Object
                {
                    ObjectId = 59,
                    Flags00 = 0x61,
                },
                [60] = new TR1Object
                {
                    ObjectId = 60,
                    Flags00 = 0x60,
                },
                [61] = new TR1Object
                {
                    ObjectId = 61,
                    Flags00 = 0x60,
                },
                [62] = new TR1Object
                {
                    ObjectId = 62,
                    Flags00 = 0x60,
                },
                [63] = new TR1Object
                {
                    ObjectId = 63,
                    Flags00 = 0x60,
                },
                [64] = new TR1Object
                {
                    ObjectId = 64,
                    Flags00 = 0x60,
                },
                [65] = new TR1Object
                {
                    ObjectId = 65,
                    Flags00 = 0x61,
                },
                [66] = new TR1Object
                {
                    ObjectId = 66,
                    Flags00 = 0x60,
                },
                [67] = new TR1Object
                {
                    ObjectId = 67,
                    Flags00 = 0x00,
                },
                [68] = new TR1Object
                {
                    ObjectId = 68,
                    Flags00 = 0x00,
                },
                [69] = new TR1Object
                {
                    ObjectId = 69,
                    Flags00 = 0x00,
                },
                [70] = new TR1Object
                {
                    ObjectId = 70,
                    Flags00 = 0x00,
                },
                [71] = new TR1Object
                {
                    ObjectId = 71,
                    Flags00 = 0x01,
                },
                [72] = new TR1Object
                {
                    ObjectId = 72,
                    Flags00 = 0x01,
                },
                [73] = new TR1Object
                {
                    ObjectId = 73,
                    Flags00 = 0x00,
                },
                [74] = new TR1Object
                {
                    ObjectId = 74,
                    Flags00 = 0x20,
                },
                [75] = new TR1Object
                {
                    ObjectId = 75,
                    Flags00 = 0x20,
                },
                [76] = new TR1Object
                {
                    ObjectId = 76,
                    Flags00 = 0x20,
                },
                [77] = new TR1Object
                {
                    ObjectId = 77,
                    Flags00 = 0x00,
                },
                [78] = new TR1Object
                {
                    ObjectId = 78,
                    Flags00 = 0x00,
                },
                [79] = new TR1Object
                {
                    ObjectId = 79,
                    Flags00 = 0x00,
                },
                [80] = new TR1Object
                {
                    ObjectId = 80,
                    Flags00 = 0x00,
                },
                [81] = new TR1Object
                {
                    ObjectId = 81,
                    Flags00 = 0x01,
                },
                [82] = new TR1Object
                {
                    ObjectId = 82,
                    Flags00 = 0x01,
                },
                [83] = new TR1Object
                {
                    ObjectId = 83,
                    Flags00 = 0x21,
                },
                [84] = new TR1Object
                {
                    ObjectId = 84,
                    Flags00 = 0x21,
                },
                [85] = new TR1Object
                {
                    ObjectId = 85,
                    Flags00 = 0x21,
                },
                [86] = new TR1Object
                {
                    ObjectId = 86,
                    Flags00 = 0x21,
                },
                [87] = new TR1Object
                {
                    ObjectId = 87,
                    Flags00 = 0x21,
                },
                [88] = new TR1Object
                {
                    ObjectId = 88,
                    Flags00 = 0x20,
                },
                [89] = new TR1Object
                {
                    ObjectId = 89,
                    Flags00 = 0x21,
                },
                [90] = new TR1Object
                {
                    ObjectId = 90,
                    Flags00 = 0x21,
                },
                [91] = new TR1Object
                {
                    ObjectId = 91,
                    Flags00 = 0x21,
                },
                [92] = new TR1Object
                {
                    ObjectId = 92,
                    Flags00 = 0x20,
                },
                [93] = new TR1Object
                {
                    ObjectId = 93,
                    Flags00 = 0x21,
                },
                [94] = new TR1Object
                {
                    ObjectId = 94,
                    Flags00 = 0x21,
                },
                [95] = new TR1Object
                {
                    ObjectId = 95,
                    Flags00 = 0x01,
                },
                [96] = new TR1Object
                {
                    ObjectId = 96,
                    Flags00 = 0x01,
                },
                [97] = new TR1Object
                {
                    ObjectId = 97,
                    Flags00 = 0x01,
                },
                [98] = new TR1Object
                {
                    ObjectId = 98,
                    Flags00 = 0x00,
                },
                [99] = new TR1Object
                {
                    ObjectId = 99,
                    Flags00 = 0x01,
                },
                [100] = new TR1Object
                {
                    ObjectId = 100,
                    Flags00 = 0x01,
                },
                [101] = new TR1Object
                {
                    ObjectId = 101,
                    Flags00 = 0x01,
                },
                [102] = new TR1Object
                {
                    ObjectId = 102,
                    Flags00 = 0x01,
                },
                [103] = new TR1Object
                {
                    ObjectId = 103,
                    Flags00 = 0x01,
                },
                [104] = new TR1Object
                {
                    ObjectId = 104,
                    Flags00 = 0x01,
                },
                [105] = new TR1Object
                {
                    ObjectId = 105,
                    Flags00 = 0x01,
                },
                [106] = new TR1Object
                {
                    ObjectId = 106,
                    Flags00 = 0x01,
                },
                [107] = new TR1Object
                {
                    ObjectId = 107,
                    Flags00 = 0x00,
                },
                [108] = new TR1Object
                {
                    ObjectId = 108,
                    Flags00 = 0x01,
                },
                [109] = new TR1Object
                {
                    ObjectId = 109,
                    Flags00 = 0x01,
                },
                [110] = new TR1Object
                {
                    ObjectId = 110,
                    Flags00 = 0x20,
                },
                [111] = new TR1Object
                {
                    ObjectId = 111,
                    Flags00 = 0x20,
                },
                [112] = new TR1Object
                {
                    ObjectId = 112,
                    Flags00 = 0x20,
                },
                [113] = new TR1Object
                {
                    ObjectId = 113,
                    Flags00 = 0x20,
                },
                [114] = new TR1Object
                {
                    ObjectId = 114,
                    Flags00 = 0x00,
                },
                [115] = new TR1Object
                {
                    ObjectId = 115,
                    Flags00 = 0x00,
                },
                [116] = new TR1Object
                {
                    ObjectId = 116,
                    Flags00 = 0x00,
                },
                [117] = new TR1Object
                {
                    ObjectId = 117,
                    Flags00 = 0x00,
                },
                [118] = new TR1Object
                {
                    ObjectId = 118,
                    Flags00 = 0x20,
                },
                [119] = new TR1Object
                {
                    ObjectId = 119,
                    Flags00 = 0x20,
                },
                [120] = new TR1Object
                {
                    ObjectId = 120,
                    Flags00 = 0x20,
                },
                [121] = new TR1Object
                {
                    ObjectId = 121,
                    Flags00 = 0x20,
                },
                [122] = new TR1Object
                {
                    ObjectId = 122,
                    Flags00 = 0x20,
                },
                [123] = new TR1Object
                {
                    ObjectId = 123,
                    Flags00 = 0x20,
                },
                [124] = new TR1Object
                {
                    ObjectId = 124,
                    Flags00 = 0x20,
                },
                [125] = new TR1Object
                {
                    ObjectId = 125,
                    Flags00 = 0x20,
                },
                [126] = new TR1Object
                {
                    ObjectId = 126,
                    Flags00 = 0x20,
                },
                [127] = new TR1Object
                {
                    ObjectId = 127,
                    Flags00 = 0x00,
                },
                [128] = new TR1Object
                {
                    ObjectId = 128,
                    Flags00 = 0x00,
                },
                [129] = new TR1Object
                {
                    ObjectId = 129,
                    Flags00 = 0x21,
                },
                [130] = new TR1Object
                {
                    ObjectId = 130,
                    Flags00 = 0x21,
                },
                [131] = new TR1Object
                {
                    ObjectId = 131,
                    Flags00 = 0x21,
                },
                [132] = new TR1Object
                {
                    ObjectId = 132,
                    Flags00 = 0x20,
                },
                [133] = new TR1Object
                {
                    ObjectId = 133,
                    Flags00 = 0x01,
                },
                [134] = new TR1Object
                {
                    ObjectId = 134,
                    Flags00 = 0x01,
                },
                [135] = new TR1Object
                {
                    ObjectId = 135,
                    Flags00 = 0x01,
                },
                [136] = new TR1Object
                {
                    ObjectId = 136,
                    Flags00 = 0x00,
                },
                [137] = new TR1Object
                {
                    ObjectId = 137,
                    Flags00 = 0x21,
                },
                [138] = new TR1Object
                {
                    ObjectId = 138,
                    Flags00 = 0x21,
                },
                [139] = new TR1Object
                {
                    ObjectId = 139,
                    Flags00 = 0x21,
                },
                [140] = new TR1Object
                {
                    ObjectId = 140,
                    Flags00 = 0x20,
                },
                [141] = new TR1Object
                {
                    ObjectId = 141,
                    Flags00 = 0x20,
                },
                [142] = new TR1Object
                {
                    ObjectId = 142,
                    Flags00 = 0x20,
                },
                [143] = new TR1Object
                {
                    ObjectId = 143,
                    Flags00 = 0x20,
                },
                [144] = new TR1Object
                {
                    ObjectId = 144,
                    Flags00 = 0x21,
                },
                [145] = new TR1Object
                {
                    ObjectId = 145,
                    Flags00 = 0x20,
                },
                [146] = new TR1Object
                {
                    ObjectId = 146,
                    Flags00 = 0x20,
                },
                [147] = new TR1Object
                {
                    ObjectId = 147,
                    Flags00 = 0x60,
                },
                [148] = new TR1Object
                {
                    ObjectId = 148,
                    Flags00 = 0x00,
                },
                [149] = new TR1Object
                {
                    ObjectId = 149,
                    Flags00 = 0x00,
                },
                [150] = new TR1Object
                {
                    ObjectId = 150,
                    Flags00 = 0x01,
                },
                [151] = new TR1Object
                {
                    ObjectId = 151,
                    Flags00 = 0x00,
                },
                [152] = new TR1Object
                {
                    ObjectId = 152,
                    Flags00 = 0x00,
                },
                [153] = new TR1Object
                {
                    ObjectId = 153,
                    Flags00 = 0x01,
                },
                [154] = new TR1Object
                {
                    ObjectId = 154,
                    Flags00 = 0x00,
                },
                [155] = new TR1Object
                {
                    ObjectId = 155,
                    Flags00 = 0x01,
                },
                [156] = new TR1Object
                {
                    ObjectId = 156,
                    Flags00 = 0x01,
                },
                [157] = new TR1Object
                {
                    ObjectId = 157,
                    Flags00 = 0x00,
                },
                [158] = new TR1Object
                {
                    ObjectId = 158,
                    Flags00 = 0x01,
                },
                [159] = new TR1Object
                {
                    ObjectId = 159,
                    Flags00 = 0x00,
                },
                [160] = new TR1Object
                {
                    ObjectId = 160,
                    Flags00 = 0x00,
                },
                [161] = new TR1Object
                {
                    ObjectId = 161,
                    Flags00 = 0x00,
                },
                [162] = new TR1Object
                {
                    ObjectId = 162,
                    Flags00 = 0x60,
                },
                [163] = new TR1Object
                {
                    ObjectId = 163,
                    Flags00 = 0x00,
                },
                [164] = new TR1Object
                {
                    ObjectId = 164,
                    Flags00 = 0x01,
                },
                [165] = new TR1Object
                {
                    ObjectId = 165,
                    Flags00 = 0x00,
                },
                [166] = new TR1Object
                {
                    ObjectId = 166,
                    Flags00 = 0x01,
                },
                [167] = new TR1Object
                {
                    ObjectId = 167,
                    Flags00 = 0x00,
                },
                [168] = new TR1Object
                {
                    ObjectId = 168,
                    Flags00 = 0x01,
                },
                [169] = new TR1Object
                {
                    ObjectId = 169,
                    Flags00 = 0x01,
                },
                [170] = new TR1Object
                {
                    ObjectId = 170,
                    Flags00 = 0x20,
                },
            },
            [9] = new Dictionary<int, TR1Object> // Tomb of Tihocan
            {
                [0] = new TR1Object
                {
                    ObjectId = 0,
                    Flags00 = 0x79,
                },
                [1] = new TR1Object
                {
                    ObjectId = 1,
                    Flags00 = 0x01,
                },
                [2] = new TR1Object
                {
                    ObjectId = 2,
                    Flags00 = 0x01,
                },
                [3] = new TR1Object
                {
                    ObjectId = 3,
                    Flags00 = 0x01,
                },
                [4] = new TR1Object
                {
                    ObjectId = 4,
                    Flags00 = 0x01,
                },
                [5] = new TR1Object
                {
                    ObjectId = 5,
                    Flags00 = 0x01,
                },
                [6] = new TR1Object
                {
                    ObjectId = 6,
                    Flags00 = 0x78,
                },
                [7] = new TR1Object
                {
                    ObjectId = 7,
                    Flags00 = 0x7B,
                },
                [8] = new TR1Object
                {
                    ObjectId = 8,
                    Flags00 = 0x7B,
                },
                [9] = new TR1Object
                {
                    ObjectId = 9,
                    Flags00 = 0x7B,
                },
                [10] = new TR1Object
                {
                    ObjectId = 10,
                    Flags00 = 0x7B,
                },
                [11] = new TR1Object
                {
                    ObjectId = 11,
                    Flags00 = 0x7B,
                },
                [12] = new TR1Object
                {
                    ObjectId = 12,
                    Flags00 = 0x7B,
                },
                [13] = new TR1Object
                {
                    ObjectId = 13,
                    Flags00 = 0x7B,
                },
                [14] = new TR1Object
                {
                    ObjectId = 14,
                    Flags00 = 0x7B,
                },
                [15] = new TR1Object
                {
                    ObjectId = 15,
                    Flags00 = 0x7B,
                },
                [16] = new TR1Object
                {
                    ObjectId = 16,
                    Flags00 = 0x7B,
                },
                [17] = new TR1Object
                {
                    ObjectId = 17,
                    Flags00 = 0x7B,
                },
                [18] = new TR1Object
                {
                    ObjectId = 18,
                    Flags00 = 0x7B,
                },
                [19] = new TR1Object
                {
                    ObjectId = 19,
                    Flags00 = 0x7B,
                },
                [20] = new TR1Object
                {
                    ObjectId = 20,
                    Flags00 = 0x7B,
                },
                [21] = new TR1Object
                {
                    ObjectId = 21,
                    Flags00 = 0x7B,
                },
                [22] = new TR1Object
                {
                    ObjectId = 22,
                    Flags00 = 0x7B,
                },
                [23] = new TR1Object
                {
                    ObjectId = 23,
                    Flags00 = 0x7B,
                },
                [24] = new TR1Object
                {
                    ObjectId = 24,
                    Flags00 = 0x71,
                },
                [25] = new TR1Object
                {
                    ObjectId = 25,
                    Flags00 = 0x00,
                },
                [26] = new TR1Object
                {
                    ObjectId = 26,
                    Flags00 = 0x00,
                },
                [27] = new TR1Object
                {
                    ObjectId = 27,
                    Flags00 = 0x7B,
                },
                [28] = new TR1Object
                {
                    ObjectId = 28,
                    Flags00 = 0x7B,
                },
                [29] = new TR1Object
                {
                    ObjectId = 29,
                    Flags00 = 0x01,
                },
                [30] = new TR1Object
                {
                    ObjectId = 30,
                    Flags00 = 0x7B,
                },
                [31] = new TR1Object
                {
                    ObjectId = 31,
                    Flags00 = 0x7B,
                },
                [32] = new TR1Object
                {
                    ObjectId = 32,
                    Flags00 = 0x7B,
                },
                [33] = new TR1Object
                {
                    ObjectId = 33,
                    Flags00 = 0x7B,
                },
                [34] = new TR1Object
                {
                    ObjectId = 34,
                    Flags00 = 0x7B,
                },
                [35] = new TR1Object
                {
                    ObjectId = 35,
                    Flags00 = 0x69,
                },
                [36] = new TR1Object
                {
                    ObjectId = 36,
                    Flags00 = 0x61,
                },
                [37] = new TR1Object
                {
                    ObjectId = 37,
                    Flags00 = 0x01,
                },
                [38] = new TR1Object
                {
                    ObjectId = 38,
                    Flags00 = 0x69,
                },
                [39] = new TR1Object
                {
                    ObjectId = 39,
                    Flags00 = 0x21,
                },
                [40] = new TR1Object
                {
                    ObjectId = 40,
                    Flags00 = 0x21,
                },
                [41] = new TR1Object
                {
                    ObjectId = 41,
                    Flags00 = 0x00,
                },
                [42] = new TR1Object
                {
                    ObjectId = 42,
                    Flags00 = 0x61,
                },
                [43] = new TR1Object
                {
                    ObjectId = 43,
                    Flags00 = 0x68,
                },
                [44] = new TR1Object
                {
                    ObjectId = 44,
                    Flags00 = 0x60,
                },
                [45] = new TR1Object
                {
                    ObjectId = 45,
                    Flags00 = 0x60,
                },
                [46] = new TR1Object
                {
                    ObjectId = 46,
                    Flags00 = 0x20,
                },
                [47] = new TR1Object
                {
                    ObjectId = 47,
                    Flags00 = 0x68,
                },
                [48] = new TR1Object
                {
                    ObjectId = 48,
                    Flags00 = 0x69,
                },
                [49] = new TR1Object
                {
                    ObjectId = 49,
                    Flags00 = 0x68,
                },
                [50] = new TR1Object
                {
                    ObjectId = 50,
                    Flags00 = 0x68,
                },
                [51] = new TR1Object
                {
                    ObjectId = 51,
                    Flags00 = 0x68,
                },
                [52] = new TR1Object
                {
                    ObjectId = 52,
                    Flags00 = 0x68,
                },
                [53] = new TR1Object
                {
                    ObjectId = 53,
                    Flags00 = 0x68,
                },
                [54] = new TR1Object
                {
                    ObjectId = 54,
                    Flags00 = 0x68,
                },
                [55] = new TR1Object
                {
                    ObjectId = 55,
                    Flags00 = 0x61,
                },
                [56] = new TR1Object
                {
                    ObjectId = 56,
                    Flags00 = 0x61,
                },
                [57] = new TR1Object
                {
                    ObjectId = 57,
                    Flags00 = 0x61,
                },
                [58] = new TR1Object
                {
                    ObjectId = 58,
                    Flags00 = 0x61,
                },
                [59] = new TR1Object
                {
                    ObjectId = 59,
                    Flags00 = 0x61,
                },
                [60] = new TR1Object
                {
                    ObjectId = 60,
                    Flags00 = 0x61,
                },
                [61] = new TR1Object
                {
                    ObjectId = 61,
                    Flags00 = 0x61,
                },
                [62] = new TR1Object
                {
                    ObjectId = 62,
                    Flags00 = 0x60,
                },
                [63] = new TR1Object
                {
                    ObjectId = 63,
                    Flags00 = 0x60,
                },
                [64] = new TR1Object
                {
                    ObjectId = 64,
                    Flags00 = 0x60,
                },
                [65] = new TR1Object
                {
                    ObjectId = 65,
                    Flags00 = 0x60,
                },
                [66] = new TR1Object
                {
                    ObjectId = 66,
                    Flags00 = 0x60,
                },
                [67] = new TR1Object
                {
                    ObjectId = 67,
                    Flags00 = 0x00,
                },
                [68] = new TR1Object
                {
                    ObjectId = 68,
                    Flags00 = 0x00,
                },
                [69] = new TR1Object
                {
                    ObjectId = 69,
                    Flags00 = 0x00,
                },
                [70] = new TR1Object
                {
                    ObjectId = 70,
                    Flags00 = 0x00,
                },
                [71] = new TR1Object
                {
                    ObjectId = 71,
                    Flags00 = 0x01,
                },
                [72] = new TR1Object
                {
                    ObjectId = 72,
                    Flags00 = 0x01,
                },
                [73] = new TR1Object
                {
                    ObjectId = 73,
                    Flags00 = 0x00,
                },
                [74] = new TR1Object
                {
                    ObjectId = 74,
                    Flags00 = 0x20,
                },
                [75] = new TR1Object
                {
                    ObjectId = 75,
                    Flags00 = 0x20,
                },
                [76] = new TR1Object
                {
                    ObjectId = 76,
                    Flags00 = 0x20,
                },
                [77] = new TR1Object
                {
                    ObjectId = 77,
                    Flags00 = 0x00,
                },
                [78] = new TR1Object
                {
                    ObjectId = 78,
                    Flags00 = 0x00,
                },
                [79] = new TR1Object
                {
                    ObjectId = 79,
                    Flags00 = 0x00,
                },
                [80] = new TR1Object
                {
                    ObjectId = 80,
                    Flags00 = 0x00,
                },
                [81] = new TR1Object
                {
                    ObjectId = 81,
                    Flags00 = 0x01,
                },
                [82] = new TR1Object
                {
                    ObjectId = 82,
                    Flags00 = 0x01,
                },
                [83] = new TR1Object
                {
                    ObjectId = 83,
                    Flags00 = 0x21,
                },
                [84] = new TR1Object
                {
                    ObjectId = 84,
                    Flags00 = 0x21,
                },
                [85] = new TR1Object
                {
                    ObjectId = 85,
                    Flags00 = 0x21,
                },
                [86] = new TR1Object
                {
                    ObjectId = 86,
                    Flags00 = 0x21,
                },
                [87] = new TR1Object
                {
                    ObjectId = 87,
                    Flags00 = 0x21,
                },
                [88] = new TR1Object
                {
                    ObjectId = 88,
                    Flags00 = 0x20,
                },
                [89] = new TR1Object
                {
                    ObjectId = 89,
                    Flags00 = 0x21,
                },
                [90] = new TR1Object
                {
                    ObjectId = 90,
                    Flags00 = 0x21,
                },
                [91] = new TR1Object
                {
                    ObjectId = 91,
                    Flags00 = 0x21,
                },
                [92] = new TR1Object
                {
                    ObjectId = 92,
                    Flags00 = 0x20,
                },
                [93] = new TR1Object
                {
                    ObjectId = 93,
                    Flags00 = 0x21,
                },
                [94] = new TR1Object
                {
                    ObjectId = 94,
                    Flags00 = 0x21,
                },
                [95] = new TR1Object
                {
                    ObjectId = 95,
                    Flags00 = 0x01,
                },
                [96] = new TR1Object
                {
                    ObjectId = 96,
                    Flags00 = 0x01,
                },
                [97] = new TR1Object
                {
                    ObjectId = 97,
                    Flags00 = 0x01,
                },
                [98] = new TR1Object
                {
                    ObjectId = 98,
                    Flags00 = 0x00,
                },
                [99] = new TR1Object
                {
                    ObjectId = 99,
                    Flags00 = 0x01,
                },
                [100] = new TR1Object
                {
                    ObjectId = 100,
                    Flags00 = 0x01,
                },
                [101] = new TR1Object
                {
                    ObjectId = 101,
                    Flags00 = 0x01,
                },
                [102] = new TR1Object
                {
                    ObjectId = 102,
                    Flags00 = 0x01,
                },
                [103] = new TR1Object
                {
                    ObjectId = 103,
                    Flags00 = 0x01,
                },
                [104] = new TR1Object
                {
                    ObjectId = 104,
                    Flags00 = 0x01,
                },
                [105] = new TR1Object
                {
                    ObjectId = 105,
                    Flags00 = 0x01,
                },
                [106] = new TR1Object
                {
                    ObjectId = 106,
                    Flags00 = 0x01,
                },
                [107] = new TR1Object
                {
                    ObjectId = 107,
                    Flags00 = 0x00,
                },
                [108] = new TR1Object
                {
                    ObjectId = 108,
                    Flags00 = 0x01,
                },
                [109] = new TR1Object
                {
                    ObjectId = 109,
                    Flags00 = 0x01,
                },
                [110] = new TR1Object
                {
                    ObjectId = 110,
                    Flags00 = 0x20,
                },
                [111] = new TR1Object
                {
                    ObjectId = 111,
                    Flags00 = 0x20,
                },
                [112] = new TR1Object
                {
                    ObjectId = 112,
                    Flags00 = 0x20,
                },
                [113] = new TR1Object
                {
                    ObjectId = 113,
                    Flags00 = 0x20,
                },
                [114] = new TR1Object
                {
                    ObjectId = 114,
                    Flags00 = 0x00,
                },
                [115] = new TR1Object
                {
                    ObjectId = 115,
                    Flags00 = 0x00,
                },
                [116] = new TR1Object
                {
                    ObjectId = 116,
                    Flags00 = 0x00,
                },
                [117] = new TR1Object
                {
                    ObjectId = 117,
                    Flags00 = 0x00,
                },
                [118] = new TR1Object
                {
                    ObjectId = 118,
                    Flags00 = 0x20,
                },
                [119] = new TR1Object
                {
                    ObjectId = 119,
                    Flags00 = 0x20,
                },
                [120] = new TR1Object
                {
                    ObjectId = 120,
                    Flags00 = 0x20,
                },
                [121] = new TR1Object
                {
                    ObjectId = 121,
                    Flags00 = 0x20,
                },
                [122] = new TR1Object
                {
                    ObjectId = 122,
                    Flags00 = 0x20,
                },
                [123] = new TR1Object
                {
                    ObjectId = 123,
                    Flags00 = 0x20,
                },
                [124] = new TR1Object
                {
                    ObjectId = 124,
                    Flags00 = 0x20,
                },
                [125] = new TR1Object
                {
                    ObjectId = 125,
                    Flags00 = 0x20,
                },
                [126] = new TR1Object
                {
                    ObjectId = 126,
                    Flags00 = 0x20,
                },
                [127] = new TR1Object
                {
                    ObjectId = 127,
                    Flags00 = 0x00,
                },
                [128] = new TR1Object
                {
                    ObjectId = 128,
                    Flags00 = 0x00,
                },
                [129] = new TR1Object
                {
                    ObjectId = 129,
                    Flags00 = 0x21,
                },
                [130] = new TR1Object
                {
                    ObjectId = 130,
                    Flags00 = 0x20,
                },
                [131] = new TR1Object
                {
                    ObjectId = 131,
                    Flags00 = 0x21,
                },
                [132] = new TR1Object
                {
                    ObjectId = 132,
                    Flags00 = 0x20,
                },
                [133] = new TR1Object
                {
                    ObjectId = 133,
                    Flags00 = 0x01,
                },
                [134] = new TR1Object
                {
                    ObjectId = 134,
                    Flags00 = 0x00,
                },
                [135] = new TR1Object
                {
                    ObjectId = 135,
                    Flags00 = 0x01,
                },
                [136] = new TR1Object
                {
                    ObjectId = 136,
                    Flags00 = 0x00,
                },
                [137] = new TR1Object
                {
                    ObjectId = 137,
                    Flags00 = 0x21,
                },
                [138] = new TR1Object
                {
                    ObjectId = 138,
                    Flags00 = 0x20,
                },
                [139] = new TR1Object
                {
                    ObjectId = 139,
                    Flags00 = 0x21,
                },
                [140] = new TR1Object
                {
                    ObjectId = 140,
                    Flags00 = 0x20,
                },
                [141] = new TR1Object
                {
                    ObjectId = 141,
                    Flags00 = 0x20,
                },
                [142] = new TR1Object
                {
                    ObjectId = 142,
                    Flags00 = 0x20,
                },
                [143] = new TR1Object
                {
                    ObjectId = 143,
                    Flags00 = 0x20,
                },
                [144] = new TR1Object
                {
                    ObjectId = 144,
                    Flags00 = 0x21,
                },
                [145] = new TR1Object
                {
                    ObjectId = 145,
                    Flags00 = 0x20,
                },
                [146] = new TR1Object
                {
                    ObjectId = 146,
                    Flags00 = 0x20,
                },
                [147] = new TR1Object
                {
                    ObjectId = 147,
                    Flags00 = 0x60,
                },
                [148] = new TR1Object
                {
                    ObjectId = 148,
                    Flags00 = 0x00,
                },
                [149] = new TR1Object
                {
                    ObjectId = 149,
                    Flags00 = 0x00,
                },
                [150] = new TR1Object
                {
                    ObjectId = 150,
                    Flags00 = 0x01,
                },
                [151] = new TR1Object
                {
                    ObjectId = 151,
                    Flags00 = 0x01,
                },
                [152] = new TR1Object
                {
                    ObjectId = 152,
                    Flags00 = 0x00,
                },
                [153] = new TR1Object
                {
                    ObjectId = 153,
                    Flags00 = 0x01,
                },
                [154] = new TR1Object
                {
                    ObjectId = 154,
                    Flags00 = 0x00,
                },
                [155] = new TR1Object
                {
                    ObjectId = 155,
                    Flags00 = 0x01,
                },
                [156] = new TR1Object
                {
                    ObjectId = 156,
                    Flags00 = 0x01,
                },
                [157] = new TR1Object
                {
                    ObjectId = 157,
                    Flags00 = 0x00,
                },
                [158] = new TR1Object
                {
                    ObjectId = 158,
                    Flags00 = 0x01,
                },
                [159] = new TR1Object
                {
                    ObjectId = 159,
                    Flags00 = 0x00,
                },
                [160] = new TR1Object
                {
                    ObjectId = 160,
                    Flags00 = 0x01,
                },
                [161] = new TR1Object
                {
                    ObjectId = 161,
                    Flags00 = 0x61,
                },
                [162] = new TR1Object
                {
                    ObjectId = 162,
                    Flags00 = 0x60,
                },
                [163] = new TR1Object
                {
                    ObjectId = 163,
                    Flags00 = 0x00,
                },
                [164] = new TR1Object
                {
                    ObjectId = 164,
                    Flags00 = 0x01,
                },
                [165] = new TR1Object
                {
                    ObjectId = 165,
                    Flags00 = 0x00,
                },
                [166] = new TR1Object
                {
                    ObjectId = 166,
                    Flags00 = 0x01,
                },
                [167] = new TR1Object
                {
                    ObjectId = 167,
                    Flags00 = 0x00,
                },
                [168] = new TR1Object
                {
                    ObjectId = 168,
                    Flags00 = 0x01,
                },
                [169] = new TR1Object
                {
                    ObjectId = 169,
                    Flags00 = 0x01,
                },
                [170] = new TR1Object
                {
                    ObjectId = 170,
                    Flags00 = 0x20,
                },
            },
            [10] = new Dictionary<int, TR1Object> // City of Khamoon
            {
                [0] = new TR1Object
                {
                    ObjectId = 0,
                    Flags00 = 0x79,
                },
                [1] = new TR1Object
                {
                    ObjectId = 1,
                    Flags00 = 0x01,
                },
                [2] = new TR1Object
                {
                    ObjectId = 2,
                    Flags00 = 0x01,
                },
                [3] = new TR1Object
                {
                    ObjectId = 3,
                    Flags00 = 0x01,
                },
                [4] = new TR1Object
                {
                    ObjectId = 4,
                    Flags00 = 0x01,
                },
                [5] = new TR1Object
                {
                    ObjectId = 5,
                    Flags00 = 0x00,
                },
                [6] = new TR1Object
                {
                    ObjectId = 6,
                    Flags00 = 0x78,
                },
                [7] = new TR1Object
                {
                    ObjectId = 7,
                    Flags00 = 0x7B,
                },
                [8] = new TR1Object
                {
                    ObjectId = 8,
                    Flags00 = 0x7B,
                },
                [9] = new TR1Object
                {
                    ObjectId = 9,
                    Flags00 = 0x7B,
                },
                [10] = new TR1Object
                {
                    ObjectId = 10,
                    Flags00 = 0x7B,
                },
                [11] = new TR1Object
                {
                    ObjectId = 11,
                    Flags00 = 0x7B,
                },
                [12] = new TR1Object
                {
                    ObjectId = 12,
                    Flags00 = 0x7B,
                },
                [13] = new TR1Object
                {
                    ObjectId = 13,
                    Flags00 = 0x7B,
                },
                [14] = new TR1Object
                {
                    ObjectId = 14,
                    Flags00 = 0x7B,
                },
                [15] = new TR1Object
                {
                    ObjectId = 15,
                    Flags00 = 0x7B,
                },
                [16] = new TR1Object
                {
                    ObjectId = 16,
                    Flags00 = 0x7B,
                },
                [17] = new TR1Object
                {
                    ObjectId = 17,
                    Flags00 = 0x7B,
                },
                [18] = new TR1Object
                {
                    ObjectId = 18,
                    Flags00 = 0x7B,
                },
                [19] = new TR1Object
                {
                    ObjectId = 19,
                    Flags00 = 0x7B,
                },
                [20] = new TR1Object
                {
                    ObjectId = 20,
                    Flags00 = 0x7B,
                },
                [21] = new TR1Object
                {
                    ObjectId = 21,
                    Flags00 = 0x7B,
                },
                [22] = new TR1Object
                {
                    ObjectId = 22,
                    Flags00 = 0x7B,
                },
                [23] = new TR1Object
                {
                    ObjectId = 23,
                    Flags00 = 0x7B,
                },
                [24] = new TR1Object
                {
                    ObjectId = 24,
                    Flags00 = 0x71,
                },
                [25] = new TR1Object
                {
                    ObjectId = 25,
                    Flags00 = 0x00,
                },
                [26] = new TR1Object
                {
                    ObjectId = 26,
                    Flags00 = 0x00,
                },
                [27] = new TR1Object
                {
                    ObjectId = 27,
                    Flags00 = 0x7B,
                },
                [28] = new TR1Object
                {
                    ObjectId = 28,
                    Flags00 = 0x7B,
                },
                [29] = new TR1Object
                {
                    ObjectId = 29,
                    Flags00 = 0x01,
                },
                [30] = new TR1Object
                {
                    ObjectId = 30,
                    Flags00 = 0x7B,
                },
                [31] = new TR1Object
                {
                    ObjectId = 31,
                    Flags00 = 0x7B,
                },
                [32] = new TR1Object
                {
                    ObjectId = 32,
                    Flags00 = 0x7B,
                },
                [33] = new TR1Object
                {
                    ObjectId = 33,
                    Flags00 = 0x7B,
                },
                [34] = new TR1Object
                {
                    ObjectId = 34,
                    Flags00 = 0x7B,
                },
                [35] = new TR1Object
                {
                    ObjectId = 35,
                    Flags00 = 0x68,
                },
                [36] = new TR1Object
                {
                    ObjectId = 36,
                    Flags00 = 0x60,
                },
                [37] = new TR1Object
                {
                    ObjectId = 37,
                    Flags00 = 0x01,
                },
                [38] = new TR1Object
                {
                    ObjectId = 38,
                    Flags00 = 0x69,
                },
                [39] = new TR1Object
                {
                    ObjectId = 39,
                    Flags00 = 0x20,
                },
                [40] = new TR1Object
                {
                    ObjectId = 40,
                    Flags00 = 0x20,
                },
                [41] = new TR1Object
                {
                    ObjectId = 41,
                    Flags00 = 0x00,
                },
                [42] = new TR1Object
                {
                    ObjectId = 42,
                    Flags00 = 0x60,
                },
                [43] = new TR1Object
                {
                    ObjectId = 43,
                    Flags00 = 0x68,
                },
                [44] = new TR1Object
                {
                    ObjectId = 44,
                    Flags00 = 0x60,
                },
                [45] = new TR1Object
                {
                    ObjectId = 45,
                    Flags00 = 0x60,
                },
                [46] = new TR1Object
                {
                    ObjectId = 46,
                    Flags00 = 0x20,
                },
                [47] = new TR1Object
                {
                    ObjectId = 47,
                    Flags00 = 0x68,
                },
                [48] = new TR1Object
                {
                    ObjectId = 48,
                    Flags00 = 0x69,
                },
                [49] = new TR1Object
                {
                    ObjectId = 49,
                    Flags00 = 0x68,
                },
                [50] = new TR1Object
                {
                    ObjectId = 50,
                    Flags00 = 0x68,
                },
                [51] = new TR1Object
                {
                    ObjectId = 51,
                    Flags00 = 0x68,
                },
                [52] = new TR1Object
                {
                    ObjectId = 52,
                    Flags00 = 0x68,
                },
                [53] = new TR1Object
                {
                    ObjectId = 53,
                    Flags00 = 0x68,
                },
                [54] = new TR1Object
                {
                    ObjectId = 54,
                    Flags00 = 0x68,
                },
                [55] = new TR1Object
                {
                    ObjectId = 55,
                    Flags00 = 0x61,
                },
                [56] = new TR1Object
                {
                    ObjectId = 56,
                    Flags00 = 0x61,
                },
                [57] = new TR1Object
                {
                    ObjectId = 57,
                    Flags00 = 0x61,
                },
                [58] = new TR1Object
                {
                    ObjectId = 58,
                    Flags00 = 0x61,
                },
                [59] = new TR1Object
                {
                    ObjectId = 59,
                    Flags00 = 0x61,
                },
                [60] = new TR1Object
                {
                    ObjectId = 60,
                    Flags00 = 0x61,
                },
                [61] = new TR1Object
                {
                    ObjectId = 61,
                    Flags00 = 0x61,
                },
                [62] = new TR1Object
                {
                    ObjectId = 62,
                    Flags00 = 0x60,
                },
                [63] = new TR1Object
                {
                    ObjectId = 63,
                    Flags00 = 0x60,
                },
                [64] = new TR1Object
                {
                    ObjectId = 64,
                    Flags00 = 0x60,
                },
                [65] = new TR1Object
                {
                    ObjectId = 65,
                    Flags00 = 0x61,
                },
                [66] = new TR1Object
                {
                    ObjectId = 66,
                    Flags00 = 0x60,
                },
                [67] = new TR1Object
                {
                    ObjectId = 67,
                    Flags00 = 0x00,
                },
                [68] = new TR1Object
                {
                    ObjectId = 68,
                    Flags00 = 0x01,
                },
                [69] = new TR1Object
                {
                    ObjectId = 69,
                    Flags00 = 0x01,
                },
                [70] = new TR1Object
                {
                    ObjectId = 70,
                    Flags00 = 0x01,
                },
                [71] = new TR1Object
                {
                    ObjectId = 71,
                    Flags00 = 0x01,
                },
                [72] = new TR1Object
                {
                    ObjectId = 72,
                    Flags00 = 0x01,
                },
                [73] = new TR1Object
                {
                    ObjectId = 73,
                    Flags00 = 0x00,
                },
                [74] = new TR1Object
                {
                    ObjectId = 74,
                    Flags00 = 0x20,
                },
                [75] = new TR1Object
                {
                    ObjectId = 75,
                    Flags00 = 0x20,
                },
                [76] = new TR1Object
                {
                    ObjectId = 76,
                    Flags00 = 0x20,
                },
                [77] = new TR1Object
                {
                    ObjectId = 77,
                    Flags00 = 0x00,
                },
                [78] = new TR1Object
                {
                    ObjectId = 78,
                    Flags00 = 0x00,
                },
                [79] = new TR1Object
                {
                    ObjectId = 79,
                    Flags00 = 0x00,
                },
                [80] = new TR1Object
                {
                    ObjectId = 80,
                    Flags00 = 0x00,
                },
                [81] = new TR1Object
                {
                    ObjectId = 81,
                    Flags00 = 0x01,
                },
                [82] = new TR1Object
                {
                    ObjectId = 82,
                    Flags00 = 0x01,
                },
                [83] = new TR1Object
                {
                    ObjectId = 83,
                    Flags00 = 0x21,
                },
                [84] = new TR1Object
                {
                    ObjectId = 84,
                    Flags00 = 0x21,
                },
                [85] = new TR1Object
                {
                    ObjectId = 85,
                    Flags00 = 0x21,
                },
                [86] = new TR1Object
                {
                    ObjectId = 86,
                    Flags00 = 0x21,
                },
                [87] = new TR1Object
                {
                    ObjectId = 87,
                    Flags00 = 0x21,
                },
                [88] = new TR1Object
                {
                    ObjectId = 88,
                    Flags00 = 0x20,
                },
                [89] = new TR1Object
                {
                    ObjectId = 89,
                    Flags00 = 0x21,
                },
                [90] = new TR1Object
                {
                    ObjectId = 90,
                    Flags00 = 0x21,
                },
                [91] = new TR1Object
                {
                    ObjectId = 91,
                    Flags00 = 0x21,
                },
                [92] = new TR1Object
                {
                    ObjectId = 92,
                    Flags00 = 0x20,
                },
                [93] = new TR1Object
                {
                    ObjectId = 93,
                    Flags00 = 0x21,
                },
                [94] = new TR1Object
                {
                    ObjectId = 94,
                    Flags00 = 0x21,
                },
                [95] = new TR1Object
                {
                    ObjectId = 95,
                    Flags00 = 0x01,
                },
                [96] = new TR1Object
                {
                    ObjectId = 96,
                    Flags00 = 0x01,
                },
                [97] = new TR1Object
                {
                    ObjectId = 97,
                    Flags00 = 0x01,
                },
                [98] = new TR1Object
                {
                    ObjectId = 98,
                    Flags00 = 0x00,
                },
                [99] = new TR1Object
                {
                    ObjectId = 99,
                    Flags00 = 0x01,
                },
                [100] = new TR1Object
                {
                    ObjectId = 100,
                    Flags00 = 0x01,
                },
                [101] = new TR1Object
                {
                    ObjectId = 101,
                    Flags00 = 0x01,
                },
                [102] = new TR1Object
                {
                    ObjectId = 102,
                    Flags00 = 0x01,
                },
                [103] = new TR1Object
                {
                    ObjectId = 103,
                    Flags00 = 0x01,
                },
                [104] = new TR1Object
                {
                    ObjectId = 104,
                    Flags00 = 0x01,
                },
                [105] = new TR1Object
                {
                    ObjectId = 105,
                    Flags00 = 0x01,
                },
                [106] = new TR1Object
                {
                    ObjectId = 106,
                    Flags00 = 0x01,
                },
                [107] = new TR1Object
                {
                    ObjectId = 107,
                    Flags00 = 0x00,
                },
                [108] = new TR1Object
                {
                    ObjectId = 108,
                    Flags00 = 0x01,
                },
                [109] = new TR1Object
                {
                    ObjectId = 109,
                    Flags00 = 0x01,
                },
                [110] = new TR1Object
                {
                    ObjectId = 110,
                    Flags00 = 0x20,
                },
                [111] = new TR1Object
                {
                    ObjectId = 111,
                    Flags00 = 0x20,
                },
                [112] = new TR1Object
                {
                    ObjectId = 112,
                    Flags00 = 0x20,
                },
                [113] = new TR1Object
                {
                    ObjectId = 113,
                    Flags00 = 0x20,
                },
                [114] = new TR1Object
                {
                    ObjectId = 114,
                    Flags00 = 0x00,
                },
                [115] = new TR1Object
                {
                    ObjectId = 115,
                    Flags00 = 0x00,
                },
                [116] = new TR1Object
                {
                    ObjectId = 116,
                    Flags00 = 0x00,
                },
                [117] = new TR1Object
                {
                    ObjectId = 117,
                    Flags00 = 0x00,
                },
                [118] = new TR1Object
                {
                    ObjectId = 118,
                    Flags00 = 0x21,
                },
                [119] = new TR1Object
                {
                    ObjectId = 119,
                    Flags00 = 0x21,
                },
                [120] = new TR1Object
                {
                    ObjectId = 120,
                    Flags00 = 0x21,
                },
                [121] = new TR1Object
                {
                    ObjectId = 121,
                    Flags00 = 0x21,
                },
                [122] = new TR1Object
                {
                    ObjectId = 122,
                    Flags00 = 0x20,
                },
                [123] = new TR1Object
                {
                    ObjectId = 123,
                    Flags00 = 0x20,
                },
                [124] = new TR1Object
                {
                    ObjectId = 124,
                    Flags00 = 0x20,
                },
                [125] = new TR1Object
                {
                    ObjectId = 125,
                    Flags00 = 0x20,
                },
                [126] = new TR1Object
                {
                    ObjectId = 126,
                    Flags00 = 0x20,
                },
                [127] = new TR1Object
                {
                    ObjectId = 127,
                    Flags00 = 0x00,
                },
                [128] = new TR1Object
                {
                    ObjectId = 128,
                    Flags00 = 0x00,
                },
                [129] = new TR1Object
                {
                    ObjectId = 129,
                    Flags00 = 0x21,
                },
                [130] = new TR1Object
                {
                    ObjectId = 130,
                    Flags00 = 0x20,
                },
                [131] = new TR1Object
                {
                    ObjectId = 131,
                    Flags00 = 0x20,
                },
                [132] = new TR1Object
                {
                    ObjectId = 132,
                    Flags00 = 0x20,
                },
                [133] = new TR1Object
                {
                    ObjectId = 133,
                    Flags00 = 0x01,
                },
                [134] = new TR1Object
                {
                    ObjectId = 134,
                    Flags00 = 0x00,
                },
                [135] = new TR1Object
                {
                    ObjectId = 135,
                    Flags00 = 0x00,
                },
                [136] = new TR1Object
                {
                    ObjectId = 136,
                    Flags00 = 0x00,
                },
                [137] = new TR1Object
                {
                    ObjectId = 137,
                    Flags00 = 0x21,
                },
                [138] = new TR1Object
                {
                    ObjectId = 138,
                    Flags00 = 0x20,
                },
                [139] = new TR1Object
                {
                    ObjectId = 139,
                    Flags00 = 0x20,
                },
                [140] = new TR1Object
                {
                    ObjectId = 140,
                    Flags00 = 0x20,
                },
                [141] = new TR1Object
                {
                    ObjectId = 141,
                    Flags00 = 0x20,
                },
                [142] = new TR1Object
                {
                    ObjectId = 142,
                    Flags00 = 0x20,
                },
                [143] = new TR1Object
                {
                    ObjectId = 143,
                    Flags00 = 0x20,
                },
                [144] = new TR1Object
                {
                    ObjectId = 144,
                    Flags00 = 0x20,
                },
                [145] = new TR1Object
                {
                    ObjectId = 145,
                    Flags00 = 0x20,
                },
                [146] = new TR1Object
                {
                    ObjectId = 146,
                    Flags00 = 0x20,
                },
                [147] = new TR1Object
                {
                    ObjectId = 147,
                    Flags00 = 0x60,
                },
                [148] = new TR1Object
                {
                    ObjectId = 148,
                    Flags00 = 0x00,
                },
                [149] = new TR1Object
                {
                    ObjectId = 149,
                    Flags00 = 0x00,
                },
                [150] = new TR1Object
                {
                    ObjectId = 150,
                    Flags00 = 0x01,
                },
                [151] = new TR1Object
                {
                    ObjectId = 151,
                    Flags00 = 0x00,
                },
                [152] = new TR1Object
                {
                    ObjectId = 152,
                    Flags00 = 0x00,
                },
                [153] = new TR1Object
                {
                    ObjectId = 153,
                    Flags00 = 0x01,
                },
                [154] = new TR1Object
                {
                    ObjectId = 154,
                    Flags00 = 0x00,
                },
                [155] = new TR1Object
                {
                    ObjectId = 155,
                    Flags00 = 0x01,
                },
                [156] = new TR1Object
                {
                    ObjectId = 156,
                    Flags00 = 0x01,
                },
                [157] = new TR1Object
                {
                    ObjectId = 157,
                    Flags00 = 0x00,
                },
                [158] = new TR1Object
                {
                    ObjectId = 158,
                    Flags00 = 0x01,
                },
                [159] = new TR1Object
                {
                    ObjectId = 159,
                    Flags00 = 0x00,
                },
                [160] = new TR1Object
                {
                    ObjectId = 160,
                    Flags00 = 0x00,
                },
                [161] = new TR1Object
                {
                    ObjectId = 161,
                    Flags00 = 0x00,
                },
                [162] = new TR1Object
                {
                    ObjectId = 162,
                    Flags00 = 0x60,
                },
                [163] = new TR1Object
                {
                    ObjectId = 163,
                    Flags00 = 0x00,
                },
                [164] = new TR1Object
                {
                    ObjectId = 164,
                    Flags00 = 0x01,
                },
                [165] = new TR1Object
                {
                    ObjectId = 165,
                    Flags00 = 0x00,
                },
                [166] = new TR1Object
                {
                    ObjectId = 166,
                    Flags00 = 0x01,
                },
                [167] = new TR1Object
                {
                    ObjectId = 167,
                    Flags00 = 0x00,
                },
                [168] = new TR1Object
                {
                    ObjectId = 168,
                    Flags00 = 0x01,
                },
                [169] = new TR1Object
                {
                    ObjectId = 169,
                    Flags00 = 0x01,
                },
                [170] = new TR1Object
                {
                    ObjectId = 170,
                    Flags00 = 0x20,
                },
            },
            [11] = new Dictionary<int, TR1Object> // Obelisk of Khamoon
            {
                [0] = new TR1Object
                {
                    ObjectId = 0,
                    Flags00 = 0x79,
                },
                [1] = new TR1Object
                {
                    ObjectId = 1,
                    Flags00 = 0x01,
                },
                [2] = new TR1Object
                {
                    ObjectId = 2,
                    Flags00 = 0x01,
                },
                [3] = new TR1Object
                {
                    ObjectId = 3,
                    Flags00 = 0x01,
                },
                [4] = new TR1Object
                {
                    ObjectId = 4,
                    Flags00 = 0x01,
                },
                [5] = new TR1Object
                {
                    ObjectId = 5,
                    Flags00 = 0x00,
                },
                [6] = new TR1Object
                {
                    ObjectId = 6,
                    Flags00 = 0x78,
                },
                [7] = new TR1Object
                {
                    ObjectId = 7,
                    Flags00 = 0x7B,
                },
                [8] = new TR1Object
                {
                    ObjectId = 8,
                    Flags00 = 0x7B,
                },
                [9] = new TR1Object
                {
                    ObjectId = 9,
                    Flags00 = 0x7B,
                },
                [10] = new TR1Object
                {
                    ObjectId = 10,
                    Flags00 = 0x7B,
                },
                [11] = new TR1Object
                {
                    ObjectId = 11,
                    Flags00 = 0x7B,
                },
                [12] = new TR1Object
                {
                    ObjectId = 12,
                    Flags00 = 0x7B,
                },
                [13] = new TR1Object
                {
                    ObjectId = 13,
                    Flags00 = 0x7B,
                },
                [14] = new TR1Object
                {
                    ObjectId = 14,
                    Flags00 = 0x7B,
                },
                [15] = new TR1Object
                {
                    ObjectId = 15,
                    Flags00 = 0x7B,
                },
                [16] = new TR1Object
                {
                    ObjectId = 16,
                    Flags00 = 0x7B,
                },
                [17] = new TR1Object
                {
                    ObjectId = 17,
                    Flags00 = 0x7B,
                },
                [18] = new TR1Object
                {
                    ObjectId = 18,
                    Flags00 = 0x7B,
                },
                [19] = new TR1Object
                {
                    ObjectId = 19,
                    Flags00 = 0x7B,
                },
                [20] = new TR1Object
                {
                    ObjectId = 20,
                    Flags00 = 0x7B,
                },
                [21] = new TR1Object
                {
                    ObjectId = 21,
                    Flags00 = 0x7B,
                },
                [22] = new TR1Object
                {
                    ObjectId = 22,
                    Flags00 = 0x7B,
                },
                [23] = new TR1Object
                {
                    ObjectId = 23,
                    Flags00 = 0x7B,
                },
                [24] = new TR1Object
                {
                    ObjectId = 24,
                    Flags00 = 0x71,
                },
                [25] = new TR1Object
                {
                    ObjectId = 25,
                    Flags00 = 0x00,
                },
                [26] = new TR1Object
                {
                    ObjectId = 26,
                    Flags00 = 0x00,
                },
                [27] = new TR1Object
                {
                    ObjectId = 27,
                    Flags00 = 0x7B,
                },
                [28] = new TR1Object
                {
                    ObjectId = 28,
                    Flags00 = 0x7B,
                },
                [29] = new TR1Object
                {
                    ObjectId = 29,
                    Flags00 = 0x01,
                },
                [30] = new TR1Object
                {
                    ObjectId = 30,
                    Flags00 = 0x7B,
                },
                [31] = new TR1Object
                {
                    ObjectId = 31,
                    Flags00 = 0x7B,
                },
                [32] = new TR1Object
                {
                    ObjectId = 32,
                    Flags00 = 0x7B,
                },
                [33] = new TR1Object
                {
                    ObjectId = 33,
                    Flags00 = 0x7B,
                },
                [34] = new TR1Object
                {
                    ObjectId = 34,
                    Flags00 = 0x7B,
                },
                [35] = new TR1Object
                {
                    ObjectId = 35,
                    Flags00 = 0x69,
                },
                [36] = new TR1Object
                {
                    ObjectId = 36,
                    Flags00 = 0x60,
                },
                [37] = new TR1Object
                {
                    ObjectId = 37,
                    Flags00 = 0x00,
                },
                [38] = new TR1Object
                {
                    ObjectId = 38,
                    Flags00 = 0x68,
                },
                [39] = new TR1Object
                {
                    ObjectId = 39,
                    Flags00 = 0x20,
                },
                [40] = new TR1Object
                {
                    ObjectId = 40,
                    Flags00 = 0x20,
                },
                [41] = new TR1Object
                {
                    ObjectId = 41,
                    Flags00 = 0x61,
                },
                [42] = new TR1Object
                {
                    ObjectId = 42,
                    Flags00 = 0x60,
                },
                [43] = new TR1Object
                {
                    ObjectId = 43,
                    Flags00 = 0x68,
                },
                [44] = new TR1Object
                {
                    ObjectId = 44,
                    Flags00 = 0x60,
                },
                [45] = new TR1Object
                {
                    ObjectId = 45,
                    Flags00 = 0x60,
                },
                [46] = new TR1Object
                {
                    ObjectId = 46,
                    Flags00 = 0x20,
                },
                [47] = new TR1Object
                {
                    ObjectId = 47,
                    Flags00 = 0x68,
                },
                [48] = new TR1Object
                {
                    ObjectId = 48,
                    Flags00 = 0x69,
                },
                [49] = new TR1Object
                {
                    ObjectId = 49,
                    Flags00 = 0x68,
                },
                [50] = new TR1Object
                {
                    ObjectId = 50,
                    Flags00 = 0x68,
                },
                [51] = new TR1Object
                {
                    ObjectId = 51,
                    Flags00 = 0x68,
                },
                [52] = new TR1Object
                {
                    ObjectId = 52,
                    Flags00 = 0x68,
                },
                [53] = new TR1Object
                {
                    ObjectId = 53,
                    Flags00 = 0x68,
                },
                [54] = new TR1Object
                {
                    ObjectId = 54,
                    Flags00 = 0x68,
                },
                [55] = new TR1Object
                {
                    ObjectId = 55,
                    Flags00 = 0x61,
                },
                [56] = new TR1Object
                {
                    ObjectId = 56,
                    Flags00 = 0x60,
                },
                [57] = new TR1Object
                {
                    ObjectId = 57,
                    Flags00 = 0x61,
                },
                [58] = new TR1Object
                {
                    ObjectId = 58,
                    Flags00 = 0x61,
                },
                [59] = new TR1Object
                {
                    ObjectId = 59,
                    Flags00 = 0x60,
                },
                [60] = new TR1Object
                {
                    ObjectId = 60,
                    Flags00 = 0x60,
                },
                [61] = new TR1Object
                {
                    ObjectId = 61,
                    Flags00 = 0x61,
                },
                [62] = new TR1Object
                {
                    ObjectId = 62,
                    Flags00 = 0x61,
                },
                [63] = new TR1Object
                {
                    ObjectId = 63,
                    Flags00 = 0x60,
                },
                [64] = new TR1Object
                {
                    ObjectId = 64,
                    Flags00 = 0x60,
                },
                [65] = new TR1Object
                {
                    ObjectId = 65,
                    Flags00 = 0x60,
                },
                [66] = new TR1Object
                {
                    ObjectId = 66,
                    Flags00 = 0x60,
                },
                [67] = new TR1Object
                {
                    ObjectId = 67,
                    Flags00 = 0x00,
                },
                [68] = new TR1Object
                {
                    ObjectId = 68,
                    Flags00 = 0x00,
                },
                [69] = new TR1Object
                {
                    ObjectId = 69,
                    Flags00 = 0x00,
                },
                [70] = new TR1Object
                {
                    ObjectId = 70,
                    Flags00 = 0x00,
                },
                [71] = new TR1Object
                {
                    ObjectId = 71,
                    Flags00 = 0x01,
                },
                [72] = new TR1Object
                {
                    ObjectId = 72,
                    Flags00 = 0x01,
                },
                [73] = new TR1Object
                {
                    ObjectId = 73,
                    Flags00 = 0x00,
                },
                [74] = new TR1Object
                {
                    ObjectId = 74,
                    Flags00 = 0x20,
                },
                [75] = new TR1Object
                {
                    ObjectId = 75,
                    Flags00 = 0x20,
                },
                [76] = new TR1Object
                {
                    ObjectId = 76,
                    Flags00 = 0x20,
                },
                [77] = new TR1Object
                {
                    ObjectId = 77,
                    Flags00 = 0x00,
                },
                [78] = new TR1Object
                {
                    ObjectId = 78,
                    Flags00 = 0x00,
                },
                [79] = new TR1Object
                {
                    ObjectId = 79,
                    Flags00 = 0x00,
                },
                [80] = new TR1Object
                {
                    ObjectId = 80,
                    Flags00 = 0x00,
                },
                [81] = new TR1Object
                {
                    ObjectId = 81,
                    Flags00 = 0x01,
                },
                [82] = new TR1Object
                {
                    ObjectId = 82,
                    Flags00 = 0x01,
                },
                [83] = new TR1Object
                {
                    ObjectId = 83,
                    Flags00 = 0x21,
                },
                [84] = new TR1Object
                {
                    ObjectId = 84,
                    Flags00 = 0x21,
                },
                [85] = new TR1Object
                {
                    ObjectId = 85,
                    Flags00 = 0x21,
                },
                [86] = new TR1Object
                {
                    ObjectId = 86,
                    Flags00 = 0x21,
                },
                [87] = new TR1Object
                {
                    ObjectId = 87,
                    Flags00 = 0x21,
                },
                [88] = new TR1Object
                {
                    ObjectId = 88,
                    Flags00 = 0x20,
                },
                [89] = new TR1Object
                {
                    ObjectId = 89,
                    Flags00 = 0x21,
                },
                [90] = new TR1Object
                {
                    ObjectId = 90,
                    Flags00 = 0x21,
                },
                [91] = new TR1Object
                {
                    ObjectId = 91,
                    Flags00 = 0x21,
                },
                [92] = new TR1Object
                {
                    ObjectId = 92,
                    Flags00 = 0x20,
                },
                [93] = new TR1Object
                {
                    ObjectId = 93,
                    Flags00 = 0x21,
                },
                [94] = new TR1Object
                {
                    ObjectId = 94,
                    Flags00 = 0x21,
                },
                [95] = new TR1Object
                {
                    ObjectId = 95,
                    Flags00 = 0x01,
                },
                [96] = new TR1Object
                {
                    ObjectId = 96,
                    Flags00 = 0x01,
                },
                [97] = new TR1Object
                {
                    ObjectId = 97,
                    Flags00 = 0x01,
                },
                [98] = new TR1Object
                {
                    ObjectId = 98,
                    Flags00 = 0x00,
                },
                [99] = new TR1Object
                {
                    ObjectId = 99,
                    Flags00 = 0x01,
                },
                [100] = new TR1Object
                {
                    ObjectId = 100,
                    Flags00 = 0x01,
                },
                [101] = new TR1Object
                {
                    ObjectId = 101,
                    Flags00 = 0x01,
                },
                [102] = new TR1Object
                {
                    ObjectId = 102,
                    Flags00 = 0x01,
                },
                [103] = new TR1Object
                {
                    ObjectId = 103,
                    Flags00 = 0x01,
                },
                [104] = new TR1Object
                {
                    ObjectId = 104,
                    Flags00 = 0x01,
                },
                [105] = new TR1Object
                {
                    ObjectId = 105,
                    Flags00 = 0x01,
                },
                [106] = new TR1Object
                {
                    ObjectId = 106,
                    Flags00 = 0x01,
                },
                [107] = new TR1Object
                {
                    ObjectId = 107,
                    Flags00 = 0x00,
                },
                [108] = new TR1Object
                {
                    ObjectId = 108,
                    Flags00 = 0x01,
                },
                [109] = new TR1Object
                {
                    ObjectId = 109,
                    Flags00 = 0x01,
                },
                [110] = new TR1Object
                {
                    ObjectId = 110,
                    Flags00 = 0x21,
                },
                [111] = new TR1Object
                {
                    ObjectId = 111,
                    Flags00 = 0x21,
                },
                [112] = new TR1Object
                {
                    ObjectId = 112,
                    Flags00 = 0x21,
                },
                [113] = new TR1Object
                {
                    ObjectId = 113,
                    Flags00 = 0x21,
                },
                [114] = new TR1Object
                {
                    ObjectId = 114,
                    Flags00 = 0x01,
                },
                [115] = new TR1Object
                {
                    ObjectId = 115,
                    Flags00 = 0x01,
                },
                [116] = new TR1Object
                {
                    ObjectId = 116,
                    Flags00 = 0x01,
                },
                [117] = new TR1Object
                {
                    ObjectId = 117,
                    Flags00 = 0x01,
                },
                [118] = new TR1Object
                {
                    ObjectId = 118,
                    Flags00 = 0x21,
                },
                [119] = new TR1Object
                {
                    ObjectId = 119,
                    Flags00 = 0x21,
                },
                [120] = new TR1Object
                {
                    ObjectId = 120,
                    Flags00 = 0x21,
                },
                [121] = new TR1Object
                {
                    ObjectId = 121,
                    Flags00 = 0x21,
                },
                [122] = new TR1Object
                {
                    ObjectId = 122,
                    Flags00 = 0x21,
                },
                [123] = new TR1Object
                {
                    ObjectId = 123,
                    Flags00 = 0x21,
                },
                [124] = new TR1Object
                {
                    ObjectId = 124,
                    Flags00 = 0x21,
                },
                [125] = new TR1Object
                {
                    ObjectId = 125,
                    Flags00 = 0x21,
                },
                [126] = new TR1Object
                {
                    ObjectId = 126,
                    Flags00 = 0x20,
                },
                [127] = new TR1Object
                {
                    ObjectId = 127,
                    Flags00 = 0x00,
                },
                [128] = new TR1Object
                {
                    ObjectId = 128,
                    Flags00 = 0x00,
                },
                [129] = new TR1Object
                {
                    ObjectId = 129,
                    Flags00 = 0x21,
                },
                [130] = new TR1Object
                {
                    ObjectId = 130,
                    Flags00 = 0x20,
                },
                [131] = new TR1Object
                {
                    ObjectId = 131,
                    Flags00 = 0x20,
                },
                [132] = new TR1Object
                {
                    ObjectId = 132,
                    Flags00 = 0x20,
                },
                [133] = new TR1Object
                {
                    ObjectId = 133,
                    Flags00 = 0x01,
                },
                [134] = new TR1Object
                {
                    ObjectId = 134,
                    Flags00 = 0x00,
                },
                [135] = new TR1Object
                {
                    ObjectId = 135,
                    Flags00 = 0x00,
                },
                [136] = new TR1Object
                {
                    ObjectId = 136,
                    Flags00 = 0x00,
                },
                [137] = new TR1Object
                {
                    ObjectId = 137,
                    Flags00 = 0x21,
                },
                [138] = new TR1Object
                {
                    ObjectId = 138,
                    Flags00 = 0x20,
                },
                [139] = new TR1Object
                {
                    ObjectId = 139,
                    Flags00 = 0x20,
                },
                [140] = new TR1Object
                {
                    ObjectId = 140,
                    Flags00 = 0x20,
                },
                [141] = new TR1Object
                {
                    ObjectId = 141,
                    Flags00 = 0x20,
                },
                [142] = new TR1Object
                {
                    ObjectId = 142,
                    Flags00 = 0x20,
                },
                [143] = new TR1Object
                {
                    ObjectId = 143,
                    Flags00 = 0x20,
                },
                [144] = new TR1Object
                {
                    ObjectId = 144,
                    Flags00 = 0x20,
                },
                [145] = new TR1Object
                {
                    ObjectId = 145,
                    Flags00 = 0x20,
                },
                [146] = new TR1Object
                {
                    ObjectId = 146,
                    Flags00 = 0x20,
                },
                [147] = new TR1Object
                {
                    ObjectId = 147,
                    Flags00 = 0x60,
                },
                [148] = new TR1Object
                {
                    ObjectId = 148,
                    Flags00 = 0x00,
                },
                [149] = new TR1Object
                {
                    ObjectId = 149,
                    Flags00 = 0x00,
                },
                [150] = new TR1Object
                {
                    ObjectId = 150,
                    Flags00 = 0x01,
                },
                [151] = new TR1Object
                {
                    ObjectId = 151,
                    Flags00 = 0x01,
                },
                [152] = new TR1Object
                {
                    ObjectId = 152,
                    Flags00 = 0x00,
                },
                [153] = new TR1Object
                {
                    ObjectId = 153,
                    Flags00 = 0x01,
                },
                [154] = new TR1Object
                {
                    ObjectId = 154,
                    Flags00 = 0x00,
                },
                [155] = new TR1Object
                {
                    ObjectId = 155,
                    Flags00 = 0x01,
                },
                [156] = new TR1Object
                {
                    ObjectId = 156,
                    Flags00 = 0x01,
                },
                [157] = new TR1Object
                {
                    ObjectId = 157,
                    Flags00 = 0x00,
                },
                [158] = new TR1Object
                {
                    ObjectId = 158,
                    Flags00 = 0x01,
                },
                [159] = new TR1Object
                {
                    ObjectId = 159,
                    Flags00 = 0x00,
                },
                [160] = new TR1Object
                {
                    ObjectId = 160,
                    Flags00 = 0x00,
                },
                [161] = new TR1Object
                {
                    ObjectId = 161,
                    Flags00 = 0x00,
                },
                [162] = new TR1Object
                {
                    ObjectId = 162,
                    Flags00 = 0x60,
                },
                [163] = new TR1Object
                {
                    ObjectId = 163,
                    Flags00 = 0x00,
                },
                [164] = new TR1Object
                {
                    ObjectId = 164,
                    Flags00 = 0x01,
                },
                [165] = new TR1Object
                {
                    ObjectId = 165,
                    Flags00 = 0x00,
                },
                [166] = new TR1Object
                {
                    ObjectId = 166,
                    Flags00 = 0x01,
                },
                [167] = new TR1Object
                {
                    ObjectId = 167,
                    Flags00 = 0x00,
                },
                [168] = new TR1Object
                {
                    ObjectId = 168,
                    Flags00 = 0x01,
                },
                [169] = new TR1Object
                {
                    ObjectId = 169,
                    Flags00 = 0x01,
                },
                [170] = new TR1Object
                {
                    ObjectId = 170,
                    Flags00 = 0x20,
                },
            },
            [12] = new Dictionary<int, TR1Object> // Sanctuary of the Scion
            {
                [0] = new TR1Object
                {
                    ObjectId = 0,
                    Flags00 = 0x79,
                },
                [1] = new TR1Object
                {
                    ObjectId = 1,
                    Flags00 = 0x01,
                },
                [2] = new TR1Object
                {
                    ObjectId = 2,
                    Flags00 = 0x01,
                },
                [3] = new TR1Object
                {
                    ObjectId = 3,
                    Flags00 = 0x01,
                },
                [4] = new TR1Object
                {
                    ObjectId = 4,
                    Flags00 = 0x01,
                },
                [5] = new TR1Object
                {
                    ObjectId = 5,
                    Flags00 = 0x01,
                },
                [6] = new TR1Object
                {
                    ObjectId = 6,
                    Flags00 = 0x78,
                },
                [7] = new TR1Object
                {
                    ObjectId = 7,
                    Flags00 = 0x7B,
                },
                [8] = new TR1Object
                {
                    ObjectId = 8,
                    Flags00 = 0x7B,
                },
                [9] = new TR1Object
                {
                    ObjectId = 9,
                    Flags00 = 0x7B,
                },
                [10] = new TR1Object
                {
                    ObjectId = 10,
                    Flags00 = 0x7B,
                },
                [11] = new TR1Object
                {
                    ObjectId = 11,
                    Flags00 = 0x7B,
                },
                [12] = new TR1Object
                {
                    ObjectId = 12,
                    Flags00 = 0x7B,
                },
                [13] = new TR1Object
                {
                    ObjectId = 13,
                    Flags00 = 0x7B,
                },
                [14] = new TR1Object
                {
                    ObjectId = 14,
                    Flags00 = 0x7B,
                },
                [15] = new TR1Object
                {
                    ObjectId = 15,
                    Flags00 = 0x7B,
                },
                [16] = new TR1Object
                {
                    ObjectId = 16,
                    Flags00 = 0x7B,
                },
                [17] = new TR1Object
                {
                    ObjectId = 17,
                    Flags00 = 0x7B,
                },
                [18] = new TR1Object
                {
                    ObjectId = 18,
                    Flags00 = 0x7B,
                },
                [19] = new TR1Object
                {
                    ObjectId = 19,
                    Flags00 = 0x7B,
                },
                [20] = new TR1Object
                {
                    ObjectId = 20,
                    Flags00 = 0x7B,
                },
                [21] = new TR1Object
                {
                    ObjectId = 21,
                    Flags00 = 0x7B,
                },
                [22] = new TR1Object
                {
                    ObjectId = 22,
                    Flags00 = 0x7B,
                },
                [23] = new TR1Object
                {
                    ObjectId = 23,
                    Flags00 = 0x7B,
                },
                [24] = new TR1Object
                {
                    ObjectId = 24,
                    Flags00 = 0x71,
                },
                [25] = new TR1Object
                {
                    ObjectId = 25,
                    Flags00 = 0x00,
                },
                [26] = new TR1Object
                {
                    ObjectId = 26,
                    Flags00 = 0x00,
                },
                [27] = new TR1Object
                {
                    ObjectId = 27,
                    Flags00 = 0x7B,
                },
                [28] = new TR1Object
                {
                    ObjectId = 28,
                    Flags00 = 0x7B,
                },
                [29] = new TR1Object
                {
                    ObjectId = 29,
                    Flags00 = 0x01,
                },
                [30] = new TR1Object
                {
                    ObjectId = 30,
                    Flags00 = 0x7B,
                },
                [31] = new TR1Object
                {
                    ObjectId = 31,
                    Flags00 = 0x7B,
                },
                [32] = new TR1Object
                {
                    ObjectId = 32,
                    Flags00 = 0x7B,
                },
                [33] = new TR1Object
                {
                    ObjectId = 33,
                    Flags00 = 0x7B,
                },
                [34] = new TR1Object
                {
                    ObjectId = 34,
                    Flags00 = 0x7B,
                },
                [35] = new TR1Object
                {
                    ObjectId = 35,
                    Flags00 = 0x68,
                },
                [36] = new TR1Object
                {
                    ObjectId = 36,
                    Flags00 = 0x60,
                },
                [37] = new TR1Object
                {
                    ObjectId = 37,
                    Flags00 = 0x00,
                },
                [38] = new TR1Object
                {
                    ObjectId = 38,
                    Flags00 = 0x68,
                },
                [39] = new TR1Object
                {
                    ObjectId = 39,
                    Flags00 = 0x20,
                },
                [40] = new TR1Object
                {
                    ObjectId = 40,
                    Flags00 = 0x20,
                },
                [41] = new TR1Object
                {
                    ObjectId = 41,
                    Flags00 = 0x00,
                },
                [42] = new TR1Object
                {
                    ObjectId = 42,
                    Flags00 = 0x61,
                },
                [43] = new TR1Object
                {
                    ObjectId = 43,
                    Flags00 = 0x68,
                },
                [44] = new TR1Object
                {
                    ObjectId = 44,
                    Flags00 = 0x60,
                },
                [45] = new TR1Object
                {
                    ObjectId = 45,
                    Flags00 = 0x60,
                },
                [46] = new TR1Object
                {
                    ObjectId = 46,
                    Flags00 = 0x20,
                },
                [47] = new TR1Object
                {
                    ObjectId = 47,
                    Flags00 = 0x68,
                },
                [48] = new TR1Object
                {
                    ObjectId = 48,
                    Flags00 = 0x69,
                },
                [49] = new TR1Object
                {
                    ObjectId = 49,
                    Flags00 = 0x68,
                },
                [50] = new TR1Object
                {
                    ObjectId = 50,
                    Flags00 = 0x68,
                },
                [51] = new TR1Object
                {
                    ObjectId = 51,
                    Flags00 = 0x68,
                },
                [52] = new TR1Object
                {
                    ObjectId = 52,
                    Flags00 = 0x68,
                },
                [53] = new TR1Object
                {
                    ObjectId = 53,
                    Flags00 = 0x68,
                },
                [54] = new TR1Object
                {
                    ObjectId = 54,
                    Flags00 = 0x68,
                },
                [55] = new TR1Object
                {
                    ObjectId = 55,
                    Flags00 = 0x61,
                },
                [56] = new TR1Object
                {
                    ObjectId = 56,
                    Flags00 = 0x61,
                },
                [57] = new TR1Object
                {
                    ObjectId = 57,
                    Flags00 = 0x61,
                },
                [58] = new TR1Object
                {
                    ObjectId = 58,
                    Flags00 = 0x60,
                },
                [59] = new TR1Object
                {
                    ObjectId = 59,
                    Flags00 = 0x61,
                },
                [60] = new TR1Object
                {
                    ObjectId = 60,
                    Flags00 = 0x61,
                },
                [61] = new TR1Object
                {
                    ObjectId = 61,
                    Flags00 = 0x61,
                },
                [62] = new TR1Object
                {
                    ObjectId = 62,
                    Flags00 = 0x61,
                },
                [63] = new TR1Object
                {
                    ObjectId = 63,
                    Flags00 = 0x60,
                },
                [64] = new TR1Object
                {
                    ObjectId = 64,
                    Flags00 = 0x60,
                },
                [65] = new TR1Object
                {
                    ObjectId = 65,
                    Flags00 = 0x60,
                },
                [66] = new TR1Object
                {
                    ObjectId = 66,
                    Flags00 = 0x60,
                },
                [67] = new TR1Object
                {
                    ObjectId = 67,
                    Flags00 = 0x00,
                },
                [68] = new TR1Object
                {
                    ObjectId = 68,
                    Flags00 = 0x00,
                },
                [69] = new TR1Object
                {
                    ObjectId = 69,
                    Flags00 = 0x00,
                },
                [70] = new TR1Object
                {
                    ObjectId = 70,
                    Flags00 = 0x00,
                },
                [71] = new TR1Object
                {
                    ObjectId = 71,
                    Flags00 = 0x01,
                },
                [72] = new TR1Object
                {
                    ObjectId = 72,
                    Flags00 = 0x01,
                },
                [73] = new TR1Object
                {
                    ObjectId = 73,
                    Flags00 = 0x00,
                },
                [74] = new TR1Object
                {
                    ObjectId = 74,
                    Flags00 = 0x20,
                },
                [75] = new TR1Object
                {
                    ObjectId = 75,
                    Flags00 = 0x20,
                },
                [76] = new TR1Object
                {
                    ObjectId = 76,
                    Flags00 = 0x20,
                },
                [77] = new TR1Object
                {
                    ObjectId = 77,
                    Flags00 = 0x00,
                },
                [78] = new TR1Object
                {
                    ObjectId = 78,
                    Flags00 = 0x00,
                },
                [79] = new TR1Object
                {
                    ObjectId = 79,
                    Flags00 = 0x00,
                },
                [80] = new TR1Object
                {
                    ObjectId = 80,
                    Flags00 = 0x00,
                },
                [81] = new TR1Object
                {
                    ObjectId = 81,
                    Flags00 = 0x01,
                },
                [82] = new TR1Object
                {
                    ObjectId = 82,
                    Flags00 = 0x01,
                },
                [83] = new TR1Object
                {
                    ObjectId = 83,
                    Flags00 = 0x21,
                },
                [84] = new TR1Object
                {
                    ObjectId = 84,
                    Flags00 = 0x21,
                },
                [85] = new TR1Object
                {
                    ObjectId = 85,
                    Flags00 = 0x21,
                },
                [86] = new TR1Object
                {
                    ObjectId = 86,
                    Flags00 = 0x21,
                },
                [87] = new TR1Object
                {
                    ObjectId = 87,
                    Flags00 = 0x21,
                },
                [88] = new TR1Object
                {
                    ObjectId = 88,
                    Flags00 = 0x20,
                },
                [89] = new TR1Object
                {
                    ObjectId = 89,
                    Flags00 = 0x21,
                },
                [90] = new TR1Object
                {
                    ObjectId = 90,
                    Flags00 = 0x21,
                },
                [91] = new TR1Object
                {
                    ObjectId = 91,
                    Flags00 = 0x21,
                },
                [92] = new TR1Object
                {
                    ObjectId = 92,
                    Flags00 = 0x20,
                },
                [93] = new TR1Object
                {
                    ObjectId = 93,
                    Flags00 = 0x21,
                },
                [94] = new TR1Object
                {
                    ObjectId = 94,
                    Flags00 = 0x21,
                },
                [95] = new TR1Object
                {
                    ObjectId = 95,
                    Flags00 = 0x01,
                },
                [96] = new TR1Object
                {
                    ObjectId = 96,
                    Flags00 = 0x01,
                },
                [97] = new TR1Object
                {
                    ObjectId = 97,
                    Flags00 = 0x01,
                },
                [98] = new TR1Object
                {
                    ObjectId = 98,
                    Flags00 = 0x00,
                },
                [99] = new TR1Object
                {
                    ObjectId = 99,
                    Flags00 = 0x01,
                },
                [100] = new TR1Object
                {
                    ObjectId = 100,
                    Flags00 = 0x01,
                },
                [101] = new TR1Object
                {
                    ObjectId = 101,
                    Flags00 = 0x01,
                },
                [102] = new TR1Object
                {
                    ObjectId = 102,
                    Flags00 = 0x01,
                },
                [103] = new TR1Object
                {
                    ObjectId = 103,
                    Flags00 = 0x01,
                },
                [104] = new TR1Object
                {
                    ObjectId = 104,
                    Flags00 = 0x01,
                },
                [105] = new TR1Object
                {
                    ObjectId = 105,
                    Flags00 = 0x01,
                },
                [106] = new TR1Object
                {
                    ObjectId = 106,
                    Flags00 = 0x01,
                },
                [107] = new TR1Object
                {
                    ObjectId = 107,
                    Flags00 = 0x00,
                },
                [108] = new TR1Object
                {
                    ObjectId = 108,
                    Flags00 = 0x01,
                },
                [109] = new TR1Object
                {
                    ObjectId = 109,
                    Flags00 = 0x01,
                },
                [110] = new TR1Object
                {
                    ObjectId = 110,
                    Flags00 = 0x21,
                },
                [111] = new TR1Object
                {
                    ObjectId = 111,
                    Flags00 = 0x21,
                },
                [112] = new TR1Object
                {
                    ObjectId = 112,
                    Flags00 = 0x20,
                },
                [113] = new TR1Object
                {
                    ObjectId = 113,
                    Flags00 = 0x20,
                },
                [114] = new TR1Object
                {
                    ObjectId = 114,
                    Flags00 = 0x01,
                },
                [115] = new TR1Object
                {
                    ObjectId = 115,
                    Flags00 = 0x01,
                },
                [116] = new TR1Object
                {
                    ObjectId = 116,
                    Flags00 = 0x00,
                },
                [117] = new TR1Object
                {
                    ObjectId = 117,
                    Flags00 = 0x00,
                },
                [118] = new TR1Object
                {
                    ObjectId = 118,
                    Flags00 = 0x21,
                },
                [119] = new TR1Object
                {
                    ObjectId = 119,
                    Flags00 = 0x21,
                },
                [120] = new TR1Object
                {
                    ObjectId = 120,
                    Flags00 = 0x20,
                },
                [121] = new TR1Object
                {
                    ObjectId = 121,
                    Flags00 = 0x20,
                },
                [122] = new TR1Object
                {
                    ObjectId = 122,
                    Flags00 = 0x21,
                },
                [123] = new TR1Object
                {
                    ObjectId = 123,
                    Flags00 = 0x21,
                },
                [124] = new TR1Object
                {
                    ObjectId = 124,
                    Flags00 = 0x20,
                },
                [125] = new TR1Object
                {
                    ObjectId = 125,
                    Flags00 = 0x20,
                },
                [126] = new TR1Object
                {
                    ObjectId = 126,
                    Flags00 = 0x20,
                },
                [127] = new TR1Object
                {
                    ObjectId = 127,
                    Flags00 = 0x00,
                },
                [128] = new TR1Object
                {
                    ObjectId = 128,
                    Flags00 = 0x00,
                },
                [129] = new TR1Object
                {
                    ObjectId = 129,
                    Flags00 = 0x21,
                },
                [130] = new TR1Object
                {
                    ObjectId = 130,
                    Flags00 = 0x20,
                },
                [131] = new TR1Object
                {
                    ObjectId = 131,
                    Flags00 = 0x20,
                },
                [132] = new TR1Object
                {
                    ObjectId = 132,
                    Flags00 = 0x20,
                },
                [133] = new TR1Object
                {
                    ObjectId = 133,
                    Flags00 = 0x01,
                },
                [134] = new TR1Object
                {
                    ObjectId = 134,
                    Flags00 = 0x00,
                },
                [135] = new TR1Object
                {
                    ObjectId = 135,
                    Flags00 = 0x00,
                },
                [136] = new TR1Object
                {
                    ObjectId = 136,
                    Flags00 = 0x00,
                },
                [137] = new TR1Object
                {
                    ObjectId = 137,
                    Flags00 = 0x21,
                },
                [138] = new TR1Object
                {
                    ObjectId = 138,
                    Flags00 = 0x20,
                },
                [139] = new TR1Object
                {
                    ObjectId = 139,
                    Flags00 = 0x20,
                },
                [140] = new TR1Object
                {
                    ObjectId = 140,
                    Flags00 = 0x20,
                },
                [141] = new TR1Object
                {
                    ObjectId = 141,
                    Flags00 = 0x20,
                },
                [142] = new TR1Object
                {
                    ObjectId = 142,
                    Flags00 = 0x20,
                },
                [143] = new TR1Object
                {
                    ObjectId = 143,
                    Flags00 = 0x21,
                },
                [144] = new TR1Object
                {
                    ObjectId = 144,
                    Flags00 = 0x20,
                },
            },
            [13] = new Dictionary<int, TR1Object> // Natla's Mines
            {
                [0] = new TR1Object
                {
                    ObjectId = 0,
                    Flags00 = 0x79,
                },
                [1] = new TR1Object
                {
                    ObjectId = 1,
                    Flags00 = 0x01,
                },
                [2] = new TR1Object
                {
                    ObjectId = 2,
                    Flags00 = 0x01,
                },
                [3] = new TR1Object
                {
                    ObjectId = 3,
                    Flags00 = 0x01,
                },
                [4] = new TR1Object
                {
                    ObjectId = 4,
                    Flags00 = 0x01,
                },
                [5] = new TR1Object
                {
                    ObjectId = 5,
                    Flags00 = 0x00,
                },
                [6] = new TR1Object
                {
                    ObjectId = 6,
                    Flags00 = 0x78,
                },
                [7] = new TR1Object
                {
                    ObjectId = 7,
                    Flags00 = 0x7B,
                },
                [8] = new TR1Object
                {
                    ObjectId = 8,
                    Flags00 = 0x7B,
                },
                [9] = new TR1Object
                {
                    ObjectId = 9,
                    Flags00 = 0x7B,
                },
                [10] = new TR1Object
                {
                    ObjectId = 10,
                    Flags00 = 0x7B,
                },
                [11] = new TR1Object
                {
                    ObjectId = 11,
                    Flags00 = 0x7B,
                },
                [12] = new TR1Object
                {
                    ObjectId = 12,
                    Flags00 = 0x7B,
                },
                [13] = new TR1Object
                {
                    ObjectId = 13,
                    Flags00 = 0x7B,
                },
                [14] = new TR1Object
                {
                    ObjectId = 14,
                    Flags00 = 0x7B,
                },
                [15] = new TR1Object
                {
                    ObjectId = 15,
                    Flags00 = 0x7B,
                },
                [16] = new TR1Object
                {
                    ObjectId = 16,
                    Flags00 = 0x7B,
                },
                [17] = new TR1Object
                {
                    ObjectId = 17,
                    Flags00 = 0x7B,
                },
                [18] = new TR1Object
                {
                    ObjectId = 18,
                    Flags00 = 0x7B,
                },
                [19] = new TR1Object
                {
                    ObjectId = 19,
                    Flags00 = 0x7B,
                },
                [20] = new TR1Object
                {
                    ObjectId = 20,
                    Flags00 = 0x7B,
                },
                [21] = new TR1Object
                {
                    ObjectId = 21,
                    Flags00 = 0x7B,
                },
                [22] = new TR1Object
                {
                    ObjectId = 22,
                    Flags00 = 0x7B,
                },
                [23] = new TR1Object
                {
                    ObjectId = 23,
                    Flags00 = 0x7B,
                },
                [24] = new TR1Object
                {
                    ObjectId = 24,
                    Flags00 = 0x71,
                },
                [25] = new TR1Object
                {
                    ObjectId = 25,
                    Flags00 = 0x00,
                },
                [26] = new TR1Object
                {
                    ObjectId = 26,
                    Flags00 = 0x00,
                },
                [27] = new TR1Object
                {
                    ObjectId = 27,
                    Flags00 = 0x7B,
                },
                [28] = new TR1Object
                {
                    ObjectId = 28,
                    Flags00 = 0x7B,
                },
                [29] = new TR1Object
                {
                    ObjectId = 29,
                    Flags00 = 0x01,
                },
                [30] = new TR1Object
                {
                    ObjectId = 30,
                    Flags00 = 0x7B,
                },
                [31] = new TR1Object
                {
                    ObjectId = 31,
                    Flags00 = 0x7B,
                },
                [32] = new TR1Object
                {
                    ObjectId = 32,
                    Flags00 = 0x7B,
                },
                [33] = new TR1Object
                {
                    ObjectId = 33,
                    Flags00 = 0x7B,
                },
                [34] = new TR1Object
                {
                    ObjectId = 34,
                    Flags00 = 0x7B,
                },
                [35] = new TR1Object
                {
                    ObjectId = 35,
                    Flags00 = 0x69,
                },
                [36] = new TR1Object
                {
                    ObjectId = 36,
                    Flags00 = 0x60,
                },
                [37] = new TR1Object
                {
                    ObjectId = 37,
                    Flags00 = 0x00,
                },
                [38] = new TR1Object
                {
                    ObjectId = 38,
                    Flags00 = 0x69,
                },
                [39] = new TR1Object
                {
                    ObjectId = 39,
                    Flags00 = 0x20,
                },
                [40] = new TR1Object
                {
                    ObjectId = 40,
                    Flags00 = 0x20,
                },
                [41] = new TR1Object
                {
                    ObjectId = 41,
                    Flags00 = 0x00,
                },
                [42] = new TR1Object
                {
                    ObjectId = 42,
                    Flags00 = 0x60,
                },
                [43] = new TR1Object
                {
                    ObjectId = 43,
                    Flags00 = 0x68,
                },
                [44] = new TR1Object
                {
                    ObjectId = 44,
                    Flags00 = 0x60,
                },
                [45] = new TR1Object
                {
                    ObjectId = 45,
                    Flags00 = 0x60,
                },
                [46] = new TR1Object
                {
                    ObjectId = 46,
                    Flags00 = 0x20,
                },
                [47] = new TR1Object
                {
                    ObjectId = 47,
                    Flags00 = 0x69,
                },
                [48] = new TR1Object
                {
                    ObjectId = 48,
                    Flags00 = 0x69,
                },
                [49] = new TR1Object
                {
                    ObjectId = 49,
                    Flags00 = 0x69,
                },
                [50] = new TR1Object
                {
                    ObjectId = 50,
                    Flags00 = 0x69,
                },
                [51] = new TR1Object
                {
                    ObjectId = 51,
                    Flags00 = 0x69,
                },
                [52] = new TR1Object
                {
                    ObjectId = 52,
                    Flags00 = 0x68,
                },
                [53] = new TR1Object
                {
                    ObjectId = 53,
                    Flags00 = 0x68,
                },
                [54] = new TR1Object
                {
                    ObjectId = 54,
                    Flags00 = 0x68,
                },
                [55] = new TR1Object
                {
                    ObjectId = 55,
                    Flags00 = 0x61,
                },
                [56] = new TR1Object
                {
                    ObjectId = 56,
                    Flags00 = 0x60,
                },
                [57] = new TR1Object
                {
                    ObjectId = 57,
                    Flags00 = 0x61,
                },
                [58] = new TR1Object
                {
                    ObjectId = 58,
                    Flags00 = 0x61,
                },
                [59] = new TR1Object
                {
                    ObjectId = 59,
                    Flags00 = 0x61,
                },
                [60] = new TR1Object
                {
                    ObjectId = 60,
                    Flags00 = 0x61,
                },
                [61] = new TR1Object
                {
                    ObjectId = 61,
                    Flags00 = 0x60,
                },
                [62] = new TR1Object
                {
                    ObjectId = 62,
                    Flags00 = 0x60,
                },
                [63] = new TR1Object
                {
                    ObjectId = 63,
                    Flags00 = 0x60,
                },
                [64] = new TR1Object
                {
                    ObjectId = 64,
                    Flags00 = 0x60,
                },
                [65] = new TR1Object
                {
                    ObjectId = 65,
                    Flags00 = 0x60,
                },
                [66] = new TR1Object
                {
                    ObjectId = 66,
                    Flags00 = 0x61,
                },
                [67] = new TR1Object
                {
                    ObjectId = 67,
                    Flags00 = 0x00,
                },
                [68] = new TR1Object
                {
                    ObjectId = 68,
                    Flags00 = 0x00,
                },
                [69] = new TR1Object
                {
                    ObjectId = 69,
                    Flags00 = 0x00,
                },
                [70] = new TR1Object
                {
                    ObjectId = 70,
                    Flags00 = 0x00,
                },
                [71] = new TR1Object
                {
                    ObjectId = 71,
                    Flags00 = 0x01,
                },
                [72] = new TR1Object
                {
                    ObjectId = 72,
                    Flags00 = 0x01,
                },
                [73] = new TR1Object
                {
                    ObjectId = 73,
                    Flags00 = 0x00,
                },
                [74] = new TR1Object
                {
                    ObjectId = 74,
                    Flags00 = 0x20,
                },
                [75] = new TR1Object
                {
                    ObjectId = 75,
                    Flags00 = 0x20,
                },
                [76] = new TR1Object
                {
                    ObjectId = 76,
                    Flags00 = 0x20,
                },
                [77] = new TR1Object
                {
                    ObjectId = 77,
                    Flags00 = 0x00,
                },
                [78] = new TR1Object
                {
                    ObjectId = 78,
                    Flags00 = 0x00,
                },
                [79] = new TR1Object
                {
                    ObjectId = 79,
                    Flags00 = 0x00,
                },
                [80] = new TR1Object
                {
                    ObjectId = 80,
                    Flags00 = 0x00,
                },
                [81] = new TR1Object
                {
                    ObjectId = 81,
                    Flags00 = 0x01,
                },
                [82] = new TR1Object
                {
                    ObjectId = 82,
                    Flags00 = 0x01,
                },
                [83] = new TR1Object
                {
                    ObjectId = 83,
                    Flags00 = 0x21,
                },
                [84] = new TR1Object
                {
                    ObjectId = 84,
                    Flags00 = 0x21,
                },
                [85] = new TR1Object
                {
                    ObjectId = 85,
                    Flags00 = 0x21,
                },
                [86] = new TR1Object
                {
                    ObjectId = 86,
                    Flags00 = 0x21,
                },
                [87] = new TR1Object
                {
                    ObjectId = 87,
                    Flags00 = 0x21,
                },
                [88] = new TR1Object
                {
                    ObjectId = 88,
                    Flags00 = 0x20,
                },
                [89] = new TR1Object
                {
                    ObjectId = 89,
                    Flags00 = 0x21,
                },
                [90] = new TR1Object
                {
                    ObjectId = 90,
                    Flags00 = 0x21,
                },
                [91] = new TR1Object
                {
                    ObjectId = 91,
                    Flags00 = 0x21,
                },
                [92] = new TR1Object
                {
                    ObjectId = 92,
                    Flags00 = 0x20,
                },
                [93] = new TR1Object
                {
                    ObjectId = 93,
                    Flags00 = 0x21,
                },
                [94] = new TR1Object
                {
                    ObjectId = 94,
                    Flags00 = 0x21,
                },
                [95] = new TR1Object
                {
                    ObjectId = 95,
                    Flags00 = 0x01,
                },
                [96] = new TR1Object
                {
                    ObjectId = 96,
                    Flags00 = 0x01,
                },
                [97] = new TR1Object
                {
                    ObjectId = 97,
                    Flags00 = 0x01,
                },
                [98] = new TR1Object
                {
                    ObjectId = 98,
                    Flags00 = 0x00,
                },
                [99] = new TR1Object
                {
                    ObjectId = 99,
                    Flags00 = 0x01,
                },
                [100] = new TR1Object
                {
                    ObjectId = 100,
                    Flags00 = 0x01,
                },
                [101] = new TR1Object
                {
                    ObjectId = 101,
                    Flags00 = 0x01,
                },
                [102] = new TR1Object
                {
                    ObjectId = 102,
                    Flags00 = 0x01,
                },
                [103] = new TR1Object
                {
                    ObjectId = 103,
                    Flags00 = 0x01,
                },
                [104] = new TR1Object
                {
                    ObjectId = 104,
                    Flags00 = 0x01,
                },
                [105] = new TR1Object
                {
                    ObjectId = 105,
                    Flags00 = 0x01,
                },
                [106] = new TR1Object
                {
                    ObjectId = 106,
                    Flags00 = 0x01,
                },
                [107] = new TR1Object
                {
                    ObjectId = 107,
                    Flags00 = 0x00,
                },
                [108] = new TR1Object
                {
                    ObjectId = 108,
                    Flags00 = 0x01,
                },
                [109] = new TR1Object
                {
                    ObjectId = 109,
                    Flags00 = 0x01,
                },
                [110] = new TR1Object
                {
                    ObjectId = 110,
                    Flags00 = 0x21,
                },
                [111] = new TR1Object
                {
                    ObjectId = 111,
                    Flags00 = 0x21,
                },
                [112] = new TR1Object
                {
                    ObjectId = 112,
                    Flags00 = 0x20,
                },
                [113] = new TR1Object
                {
                    ObjectId = 113,
                    Flags00 = 0x20,
                },
                [114] = new TR1Object
                {
                    ObjectId = 114,
                    Flags00 = 0x01,
                },
                [115] = new TR1Object
                {
                    ObjectId = 115,
                    Flags00 = 0x01,
                },
                [116] = new TR1Object
                {
                    ObjectId = 116,
                    Flags00 = 0x00,
                },
                [117] = new TR1Object
                {
                    ObjectId = 117,
                    Flags00 = 0x00,
                },
                [118] = new TR1Object
                {
                    ObjectId = 118,
                    Flags00 = 0x21,
                },
                [119] = new TR1Object
                {
                    ObjectId = 119,
                    Flags00 = 0x21,
                },
                [120] = new TR1Object
                {
                    ObjectId = 120,
                    Flags00 = 0x20,
                },
                [121] = new TR1Object
                {
                    ObjectId = 121,
                    Flags00 = 0x20,
                },
                [122] = new TR1Object
                {
                    ObjectId = 122,
                    Flags00 = 0x21,
                },
                [123] = new TR1Object
                {
                    ObjectId = 123,
                    Flags00 = 0x21,
                },
                [124] = new TR1Object
                {
                    ObjectId = 124,
                    Flags00 = 0x20,
                },
                [125] = new TR1Object
                {
                    ObjectId = 125,
                    Flags00 = 0x20,
                },
                [126] = new TR1Object
                {
                    ObjectId = 126,
                    Flags00 = 0x20,
                },
                [127] = new TR1Object
                {
                    ObjectId = 127,
                    Flags00 = 0x00,
                },
                [128] = new TR1Object
                {
                    ObjectId = 128,
                    Flags00 = 0x00,
                },
                [129] = new TR1Object
                {
                    ObjectId = 129,
                    Flags00 = 0x21,
                },
                [130] = new TR1Object
                {
                    ObjectId = 130,
                    Flags00 = 0x20,
                },
                [131] = new TR1Object
                {
                    ObjectId = 131,
                    Flags00 = 0x20,
                },
                [132] = new TR1Object
                {
                    ObjectId = 132,
                    Flags00 = 0x20,
                },
                [133] = new TR1Object
                {
                    ObjectId = 133,
                    Flags00 = 0x01,
                },
                [134] = new TR1Object
                {
                    ObjectId = 134,
                    Flags00 = 0x00,
                },
                [135] = new TR1Object
                {
                    ObjectId = 135,
                    Flags00 = 0x00,
                },
                [136] = new TR1Object
                {
                    ObjectId = 136,
                    Flags00 = 0x00,
                },
                [137] = new TR1Object
                {
                    ObjectId = 137,
                    Flags00 = 0x21,
                },
                [138] = new TR1Object
                {
                    ObjectId = 138,
                    Flags00 = 0x20,
                },
                [139] = new TR1Object
                {
                    ObjectId = 139,
                    Flags00 = 0x20,
                },
                [140] = new TR1Object
                {
                    ObjectId = 140,
                    Flags00 = 0x20,
                },
                [141] = new TR1Object
                {
                    ObjectId = 141,
                    Flags00 = 0x20,
                },
                [142] = new TR1Object
                {
                    ObjectId = 142,
                    Flags00 = 0x20,
                },
                [143] = new TR1Object
                {
                    ObjectId = 143,
                    Flags00 = 0x20,
                },
                [144] = new TR1Object
                {
                    ObjectId = 144,
                    Flags00 = 0x20,
                },
                [145] = new TR1Object
                {
                    ObjectId = 145,
                    Flags00 = 0x20,
                },
                [146] = new TR1Object
                {
                    ObjectId = 146,
                    Flags00 = 0x20,
                },
                [147] = new TR1Object
                {
                    ObjectId = 147,
                    Flags00 = 0x60,
                },
                [148] = new TR1Object
                {
                    ObjectId = 148,
                    Flags00 = 0x00,
                },
                [149] = new TR1Object
                {
                    ObjectId = 149,
                    Flags00 = 0x00,
                },
                [150] = new TR1Object
                {
                    ObjectId = 150,
                    Flags00 = 0x01,
                },
                [151] = new TR1Object
                {
                    ObjectId = 151,
                    Flags00 = 0x00,
                },
                [152] = new TR1Object
                {
                    ObjectId = 152,
                    Flags00 = 0x00,
                },
                [153] = new TR1Object
                {
                    ObjectId = 153,
                    Flags00 = 0x01,
                },
                [154] = new TR1Object
                {
                    ObjectId = 154,
                    Flags00 = 0x00,
                },
                [155] = new TR1Object
                {
                    ObjectId = 155,
                    Flags00 = 0x01,
                },
                [156] = new TR1Object
                {
                    ObjectId = 156,
                    Flags00 = 0x01,
                },
                [157] = new TR1Object
                {
                    ObjectId = 157,
                    Flags00 = 0x00,
                },
                [158] = new TR1Object
                {
                    ObjectId = 158,
                    Flags00 = 0x01,
                },
                [159] = new TR1Object
                {
                    ObjectId = 159,
                    Flags00 = 0x00,
                },
                [160] = new TR1Object
                {
                    ObjectId = 160,
                    Flags00 = 0x00,
                },
                [161] = new TR1Object
                {
                    ObjectId = 161,
                    Flags00 = 0x00,
                },
                [162] = new TR1Object
                {
                    ObjectId = 162,
                    Flags00 = 0x61,
                },
                [163] = new TR1Object
                {
                    ObjectId = 163,
                    Flags00 = 0x00,
                },
                [164] = new TR1Object
                {
                    ObjectId = 164,
                    Flags00 = 0x01,
                },
                [165] = new TR1Object
                {
                    ObjectId = 165,
                    Flags00 = 0x00,
                },
                [166] = new TR1Object
                {
                    ObjectId = 166,
                    Flags00 = 0x01,
                },
                [167] = new TR1Object
                {
                    ObjectId = 167,
                    Flags00 = 0x00,
                },
                [168] = new TR1Object
                {
                    ObjectId = 168,
                    Flags00 = 0x01,
                },
                [169] = new TR1Object
                {
                    ObjectId = 169,
                    Flags00 = 0x01,
                },
                [170] = new TR1Object
                {
                    ObjectId = 170,
                    Flags00 = 0x21,
                },
                [171] = new TR1Object
                {
                    ObjectId = 171,
                    Flags00 = 0x00,
                },
                [172] = new TR1Object
                {
                    ObjectId = 172,
                    Flags00 = 0x00,
                },
                [173] = new TR1Object
                {
                    ObjectId = 173,
                    Flags00 = 0x00,
                },
                [174] = new TR1Object
                {
                    ObjectId = 174,
                    Flags00 = 0x00,
                },
                [175] = new TR1Object
                {
                    ObjectId = 175,
                    Flags00 = 0x00,
                },
                [176] = new TR1Object
                {
                    ObjectId = 176,
                    Flags00 = 0x01,
                },
                [177] = new TR1Object
                {
                    ObjectId = 177,
                    Flags00 = 0x21,
                },
                [178] = new TR1Object
                {
                    ObjectId = 178,
                    Flags00 = 0x01,
                },
                [179] = new TR1Object
                {
                    ObjectId = 179,
                    Flags00 = 0x20,
                },
                [180] = new TR1Object
                {
                    ObjectId = 180,
                    Flags00 = 0x68,
                },
                [181] = new TR1Object
                {
                    ObjectId = 181,
                    Flags00 = 0x00,
                },
                [182] = new TR1Object
                {
                    ObjectId = 182,
                    Flags00 = 0x69,
                },
                [183] = new TR1Object
                {
                    ObjectId = 183,
                    Flags00 = 0x20,
                },
            },
            [14] = new Dictionary<int, TR1Object> // Atlantis
            {
                [0] = new TR1Object
                {
                    ObjectId = 0,
                    Flags00 = 0x79,
                },
                [1] = new TR1Object
                {
                    ObjectId = 1,
                    Flags00 = 0x01,
                },
                [2] = new TR1Object
                {
                    ObjectId = 2,
                    Flags00 = 0x01,
                },
                [3] = new TR1Object
                {
                    ObjectId = 3,
                    Flags00 = 0x01,
                },
                [4] = new TR1Object
                {
                    ObjectId = 4,
                    Flags00 = 0x01,
                },
                [5] = new TR1Object
                {
                    ObjectId = 5,
                    Flags00 = 0x01,
                },
                [6] = new TR1Object
                {
                    ObjectId = 6,
                    Flags00 = 0x79,
                },
                [7] = new TR1Object
                {
                    ObjectId = 7,
                    Flags00 = 0x7B,
                },
                [8] = new TR1Object
                {
                    ObjectId = 8,
                    Flags00 = 0x7B,
                },
                [9] = new TR1Object
                {
                    ObjectId = 9,
                    Flags00 = 0x7B,
                },
                [10] = new TR1Object
                {
                    ObjectId = 10,
                    Flags00 = 0x7B,
                },
                [11] = new TR1Object
                {
                    ObjectId = 11,
                    Flags00 = 0x7B,
                },
                [12] = new TR1Object
                {
                    ObjectId = 12,
                    Flags00 = 0x7B,
                },
                [13] = new TR1Object
                {
                    ObjectId = 13,
                    Flags00 = 0x7B,
                },
                [14] = new TR1Object
                {
                    ObjectId = 14,
                    Flags00 = 0x7B,
                },
                [15] = new TR1Object
                {
                    ObjectId = 15,
                    Flags00 = 0x7B,
                },
                [16] = new TR1Object
                {
                    ObjectId = 16,
                    Flags00 = 0x7B,
                },
                [17] = new TR1Object
                {
                    ObjectId = 17,
                    Flags00 = 0x7B,
                },
                [18] = new TR1Object
                {
                    ObjectId = 18,
                    Flags00 = 0x7B,
                },
                [19] = new TR1Object
                {
                    ObjectId = 19,
                    Flags00 = 0x7B,
                },
                [20] = new TR1Object
                {
                    ObjectId = 20,
                    Flags00 = 0x7B,
                },
                [21] = new TR1Object
                {
                    ObjectId = 21,
                    Flags00 = 0x7B,
                },
                [22] = new TR1Object
                {
                    ObjectId = 22,
                    Flags00 = 0x7B,
                },
                [23] = new TR1Object
                {
                    ObjectId = 23,
                    Flags00 = 0x7B,
                },
                [24] = new TR1Object
                {
                    ObjectId = 24,
                    Flags00 = 0x71,
                },
                [25] = new TR1Object
                {
                    ObjectId = 25,
                    Flags00 = 0x00,
                },
                [26] = new TR1Object
                {
                    ObjectId = 26,
                    Flags00 = 0x00,
                },
                [27] = new TR1Object
                {
                    ObjectId = 27,
                    Flags00 = 0x7B,
                },
                [28] = new TR1Object
                {
                    ObjectId = 28,
                    Flags00 = 0x7B,
                },
                [29] = new TR1Object
                {
                    ObjectId = 29,
                    Flags00 = 0x01,
                },
                [30] = new TR1Object
                {
                    ObjectId = 30,
                    Flags00 = 0x7B,
                },
                [31] = new TR1Object
                {
                    ObjectId = 31,
                    Flags00 = 0x7B,
                },
                [32] = new TR1Object
                {
                    ObjectId = 32,
                    Flags00 = 0x7B,
                },
                [33] = new TR1Object
                {
                    ObjectId = 33,
                    Flags00 = 0x7B,
                },
                [34] = new TR1Object
                {
                    ObjectId = 34,
                    Flags00 = 0x7B,
                },
                [35] = new TR1Object
                {
                    ObjectId = 35,
                    Flags00 = 0x68,
                },
                [36] = new TR1Object
                {
                    ObjectId = 36,
                    Flags00 = 0x60,
                },
                [37] = new TR1Object
                {
                    ObjectId = 37,
                    Flags00 = 0x01,
                },
                [38] = new TR1Object
                {
                    ObjectId = 38,
                    Flags00 = 0x69,
                },
                [39] = new TR1Object
                {
                    ObjectId = 39,
                    Flags00 = 0x21,
                },
                [40] = new TR1Object
                {
                    ObjectId = 40,
                    Flags00 = 0x21,
                },
                [41] = new TR1Object
                {
                    ObjectId = 41,
                    Flags00 = 0x00,
                },
                [42] = new TR1Object
                {
                    ObjectId = 42,
                    Flags00 = 0x61,
                },
                [43] = new TR1Object
                {
                    ObjectId = 43,
                    Flags00 = 0x68,
                },
                [44] = new TR1Object
                {
                    ObjectId = 44,
                    Flags00 = 0x60,
                },
                [45] = new TR1Object
                {
                    ObjectId = 45,
                    Flags00 = 0x60,
                },
                [46] = new TR1Object
                {
                    ObjectId = 46,
                    Flags00 = 0x20,
                },
                [47] = new TR1Object
                {
                    ObjectId = 47,
                    Flags00 = 0x68,
                },
                [48] = new TR1Object
                {
                    ObjectId = 48,
                    Flags00 = 0x69,
                },
                [49] = new TR1Object
                {
                    ObjectId = 49,
                    Flags00 = 0x68,
                },
                [50] = new TR1Object
                {
                    ObjectId = 50,
                    Flags00 = 0x68,
                },
                [51] = new TR1Object
                {
                    ObjectId = 51,
                    Flags00 = 0x68,
                },
                [52] = new TR1Object
                {
                    ObjectId = 52,
                    Flags00 = 0x68,
                },
                [53] = new TR1Object
                {
                    ObjectId = 53,
                    Flags00 = 0x68,
                },
                [54] = new TR1Object
                {
                    ObjectId = 54,
                    Flags00 = 0x68,
                },
                [55] = new TR1Object
                {
                    ObjectId = 55,
                    Flags00 = 0x61,
                },
                [56] = new TR1Object
                {
                    ObjectId = 56,
                    Flags00 = 0x61,
                },
                [57] = new TR1Object
                {
                    ObjectId = 57,
                    Flags00 = 0x60,
                },
                [58] = new TR1Object
                {
                    ObjectId = 58,
                    Flags00 = 0x60,
                },
                [59] = new TR1Object
                {
                    ObjectId = 59,
                    Flags00 = 0x61,
                },
                [60] = new TR1Object
                {
                    ObjectId = 60,
                    Flags00 = 0x61,
                },
                [61] = new TR1Object
                {
                    ObjectId = 61,
                    Flags00 = 0x61,
                },
                [62] = new TR1Object
                {
                    ObjectId = 62,
                    Flags00 = 0x61,
                },
                [63] = new TR1Object
                {
                    ObjectId = 63,
                    Flags00 = 0x61,
                },
                [64] = new TR1Object
                {
                    ObjectId = 64,
                    Flags00 = 0x61,
                },
                [65] = new TR1Object
                {
                    ObjectId = 65,
                    Flags00 = 0x61,
                },
                [66] = new TR1Object
                {
                    ObjectId = 66,
                    Flags00 = 0x61,
                },
                [67] = new TR1Object
                {
                    ObjectId = 67,
                    Flags00 = 0x00,
                },
                [68] = new TR1Object
                {
                    ObjectId = 68,
                    Flags00 = 0x00,
                },
                [69] = new TR1Object
                {
                    ObjectId = 69,
                    Flags00 = 0x00,
                },
                [70] = new TR1Object
                {
                    ObjectId = 70,
                    Flags00 = 0x00,
                },
                [71] = new TR1Object
                {
                    ObjectId = 71,
                    Flags00 = 0x01,
                },
                [72] = new TR1Object
                {
                    ObjectId = 72,
                    Flags00 = 0x01,
                },
                [73] = new TR1Object
                {
                    ObjectId = 73,
                    Flags00 = 0x00,
                },
                [74] = new TR1Object
                {
                    ObjectId = 74,
                    Flags00 = 0x20,
                },
                [75] = new TR1Object
                {
                    ObjectId = 75,
                    Flags00 = 0x20,
                },
                [76] = new TR1Object
                {
                    ObjectId = 76,
                    Flags00 = 0x20,
                },
                [77] = new TR1Object
                {
                    ObjectId = 77,
                    Flags00 = 0x00,
                },
                [78] = new TR1Object
                {
                    ObjectId = 78,
                    Flags00 = 0x00,
                },
                [79] = new TR1Object
                {
                    ObjectId = 79,
                    Flags00 = 0x00,
                },
                [80] = new TR1Object
                {
                    ObjectId = 80,
                    Flags00 = 0x00,
                },
                [81] = new TR1Object
                {
                    ObjectId = 81,
                    Flags00 = 0x01,
                },
                [82] = new TR1Object
                {
                    ObjectId = 82,
                    Flags00 = 0x01,
                },
                [83] = new TR1Object
                {
                    ObjectId = 83,
                    Flags00 = 0x21,
                },
                [84] = new TR1Object
                {
                    ObjectId = 84,
                    Flags00 = 0x21,
                },
                [85] = new TR1Object
                {
                    ObjectId = 85,
                    Flags00 = 0x21,
                },
                [86] = new TR1Object
                {
                    ObjectId = 86,
                    Flags00 = 0x21,
                },
                [87] = new TR1Object
                {
                    ObjectId = 87,
                    Flags00 = 0x21,
                },
                [88] = new TR1Object
                {
                    ObjectId = 88,
                    Flags00 = 0x20,
                },
                [89] = new TR1Object
                {
                    ObjectId = 89,
                    Flags00 = 0x21,
                },
                [90] = new TR1Object
                {
                    ObjectId = 90,
                    Flags00 = 0x21,
                },
                [91] = new TR1Object
                {
                    ObjectId = 91,
                    Flags00 = 0x21,
                },
                [92] = new TR1Object
                {
                    ObjectId = 92,
                    Flags00 = 0x20,
                },
                [93] = new TR1Object
                {
                    ObjectId = 93,
                    Flags00 = 0x21,
                },
                [94] = new TR1Object
                {
                    ObjectId = 94,
                    Flags00 = 0x21,
                },
                [95] = new TR1Object
                {
                    ObjectId = 95,
                    Flags00 = 0x01,
                },
                [96] = new TR1Object
                {
                    ObjectId = 96,
                    Flags00 = 0x01,
                },
                [97] = new TR1Object
                {
                    ObjectId = 97,
                    Flags00 = 0x01,
                },
                [98] = new TR1Object
                {
                    ObjectId = 98,
                    Flags00 = 0x00,
                },
                [99] = new TR1Object
                {
                    ObjectId = 99,
                    Flags00 = 0x01,
                },
                [100] = new TR1Object
                {
                    ObjectId = 100,
                    Flags00 = 0x01,
                },
                [101] = new TR1Object
                {
                    ObjectId = 101,
                    Flags00 = 0x01,
                },
                [102] = new TR1Object
                {
                    ObjectId = 102,
                    Flags00 = 0x01,
                },
                [103] = new TR1Object
                {
                    ObjectId = 103,
                    Flags00 = 0x01,
                },
                [104] = new TR1Object
                {
                    ObjectId = 104,
                    Flags00 = 0x01,
                },
                [105] = new TR1Object
                {
                    ObjectId = 105,
                    Flags00 = 0x01,
                },
                [106] = new TR1Object
                {
                    ObjectId = 106,
                    Flags00 = 0x01,
                },
                [107] = new TR1Object
                {
                    ObjectId = 107,
                    Flags00 = 0x00,
                },
                [108] = new TR1Object
                {
                    ObjectId = 108,
                    Flags00 = 0x01,
                },
                [109] = new TR1Object
                {
                    ObjectId = 109,
                    Flags00 = 0x01,
                },
                [110] = new TR1Object
                {
                    ObjectId = 110,
                    Flags00 = 0x20,
                },
                [111] = new TR1Object
                {
                    ObjectId = 111,
                    Flags00 = 0x20,
                },
                [112] = new TR1Object
                {
                    ObjectId = 112,
                    Flags00 = 0x20,
                },
                [113] = new TR1Object
                {
                    ObjectId = 113,
                    Flags00 = 0x20,
                },
                [114] = new TR1Object
                {
                    ObjectId = 114,
                    Flags00 = 0x00,
                },
                [115] = new TR1Object
                {
                    ObjectId = 115,
                    Flags00 = 0x00,
                },
                [116] = new TR1Object
                {
                    ObjectId = 116,
                    Flags00 = 0x00,
                },
                [117] = new TR1Object
                {
                    ObjectId = 117,
                    Flags00 = 0x00,
                },
                [118] = new TR1Object
                {
                    ObjectId = 118,
                    Flags00 = 0x20,
                },
                [119] = new TR1Object
                {
                    ObjectId = 119,
                    Flags00 = 0x20,
                },
                [120] = new TR1Object
                {
                    ObjectId = 120,
                    Flags00 = 0x20,
                },
                [121] = new TR1Object
                {
                    ObjectId = 121,
                    Flags00 = 0x20,
                },
                [122] = new TR1Object
                {
                    ObjectId = 122,
                    Flags00 = 0x20,
                },
                [123] = new TR1Object
                {
                    ObjectId = 123,
                    Flags00 = 0x20,
                },
                [124] = new TR1Object
                {
                    ObjectId = 124,
                    Flags00 = 0x20,
                },
                [125] = new TR1Object
                {
                    ObjectId = 125,
                    Flags00 = 0x20,
                },
                [126] = new TR1Object
                {
                    ObjectId = 126,
                    Flags00 = 0x20,
                },
                [127] = new TR1Object
                {
                    ObjectId = 127,
                    Flags00 = 0x00,
                },
                [128] = new TR1Object
                {
                    ObjectId = 128,
                    Flags00 = 0x00,
                },
                [129] = new TR1Object
                {
                    ObjectId = 129,
                    Flags00 = 0x20,
                },
                [130] = new TR1Object
                {
                    ObjectId = 130,
                    Flags00 = 0x20,
                },
                [131] = new TR1Object
                {
                    ObjectId = 131,
                    Flags00 = 0x20,
                },
                [132] = new TR1Object
                {
                    ObjectId = 132,
                    Flags00 = 0x20,
                },
                [133] = new TR1Object
                {
                    ObjectId = 133,
                    Flags00 = 0x00,
                },
                [134] = new TR1Object
                {
                    ObjectId = 134,
                    Flags00 = 0x00,
                },
                [135] = new TR1Object
                {
                    ObjectId = 135,
                    Flags00 = 0x00,
                },
                [136] = new TR1Object
                {
                    ObjectId = 136,
                    Flags00 = 0x00,
                },
                [137] = new TR1Object
                {
                    ObjectId = 137,
                    Flags00 = 0x20,
                },
                [138] = new TR1Object
                {
                    ObjectId = 138,
                    Flags00 = 0x20,
                },
                [139] = new TR1Object
                {
                    ObjectId = 139,
                    Flags00 = 0x20,
                },
                [140] = new TR1Object
                {
                    ObjectId = 140,
                    Flags00 = 0x20,
                },
                [141] = new TR1Object
                {
                    ObjectId = 141,
                    Flags00 = 0x20,
                },
                [142] = new TR1Object
                {
                    ObjectId = 142,
                    Flags00 = 0x20,
                },
                [143] = new TR1Object
                {
                    ObjectId = 143,
                    Flags00 = 0x20,
                },
                [144] = new TR1Object
                {
                    ObjectId = 144,
                    Flags00 = 0x20,
                },
                [145] = new TR1Object
                {
                    ObjectId = 145,
                    Flags00 = 0x20,
                },
                [146] = new TR1Object
                {
                    ObjectId = 146,
                    Flags00 = 0x21,
                },
                [147] = new TR1Object
                {
                    ObjectId = 147,
                    Flags00 = 0x61,
                },
                [148] = new TR1Object
                {
                    ObjectId = 148,
                    Flags00 = 0x00,
                },
                [149] = new TR1Object
                {
                    ObjectId = 149,
                    Flags00 = 0x00,
                },
                [150] = new TR1Object
                {
                    ObjectId = 150,
                    Flags00 = 0x01,
                },
                [151] = new TR1Object
                {
                    ObjectId = 151,
                    Flags00 = 0x01,
                },
                [152] = new TR1Object
                {
                    ObjectId = 152,
                    Flags00 = 0x00,
                },
                [153] = new TR1Object
                {
                    ObjectId = 153,
                    Flags00 = 0x01,
                },
                [154] = new TR1Object
                {
                    ObjectId = 154,
                    Flags00 = 0x00,
                },
                [155] = new TR1Object
                {
                    ObjectId = 155,
                    Flags00 = 0x01,
                },
                [156] = new TR1Object
                {
                    ObjectId = 156,
                    Flags00 = 0x01,
                },
                [157] = new TR1Object
                {
                    ObjectId = 157,
                    Flags00 = 0x00,
                },
                [158] = new TR1Object
                {
                    ObjectId = 158,
                    Flags00 = 0x01,
                },
                [159] = new TR1Object
                {
                    ObjectId = 159,
                    Flags00 = 0x00,
                },
                [160] = new TR1Object
                {
                    ObjectId = 160,
                    Flags00 = 0x01,
                },
                [161] = new TR1Object
                {
                    ObjectId = 161,
                    Flags00 = 0x00,
                },
                [162] = new TR1Object
                {
                    ObjectId = 162,
                    Flags00 = 0x60,
                },
                [163] = new TR1Object
                {
                    ObjectId = 163,
                    Flags00 = 0x61,
                },
                [164] = new TR1Object
                {
                    ObjectId = 164,
                    Flags00 = 0x01,
                },
                [165] = new TR1Object
                {
                    ObjectId = 165,
                    Flags00 = 0x00,
                },
                [166] = new TR1Object
                {
                    ObjectId = 166,
                    Flags00 = 0x01,
                },
                [167] = new TR1Object
                {
                    ObjectId = 167,
                    Flags00 = 0x00,
                },
                [168] = new TR1Object
                {
                    ObjectId = 168,
                    Flags00 = 0x01,
                },
                [169] = new TR1Object
                {
                    ObjectId = 169,
                    Flags00 = 0x01,
                },
                [170] = new TR1Object
                {
                    ObjectId = 170,
                    Flags00 = 0x20,
                },
                [171] = new TR1Object
                {
                    ObjectId = 171,
                    Flags00 = 0x00,
                },
                [172] = new TR1Object
                {
                    ObjectId = 172,
                    Flags00 = 0x01,
                },
                [173] = new TR1Object
                {
                    ObjectId = 173,
                    Flags00 = 0x01,
                },
                [174] = new TR1Object
                {
                    ObjectId = 174,
                    Flags00 = 0x00,
                },
                [175] = new TR1Object
                {
                    ObjectId = 175,
                    Flags00 = 0x00,
                },
                [176] = new TR1Object
                {
                    ObjectId = 176,
                    Flags00 = 0x01,
                },
                [177] = new TR1Object
                {
                    ObjectId = 177,
                    Flags00 = 0x21,
                },
                [178] = new TR1Object
                {
                    ObjectId = 178,
                    Flags00 = 0x01,
                },
                [179] = new TR1Object
                {
                    ObjectId = 179,
                    Flags00 = 0x20,
                },
                [180] = new TR1Object
                {
                    ObjectId = 180,
                    Flags00 = 0x69,
                },
                [181] = new TR1Object
                {
                    ObjectId = 181,
                    Flags00 = 0x61,
                },
                [182] = new TR1Object
                {
                    ObjectId = 182,
                    Flags00 = 0x68,
                },
            },
            [15] = new Dictionary<int, TR1Object> // The Great Pyramid
            {
                [0] = new TR1Object
                {
                    ObjectId = 0,
                    Flags00 = 0x79,
                },
                [1] = new TR1Object
                {
                    ObjectId = 1,
                    Flags00 = 0x01,
                },
                [2] = new TR1Object
                {
                    ObjectId = 2,
                    Flags00 = 0x01,
                },
                [3] = new TR1Object
                {
                    ObjectId = 3,
                    Flags00 = 0x01,
                },
                [4] = new TR1Object
                {
                    ObjectId = 4,
                    Flags00 = 0x01,
                },
                [5] = new TR1Object
                {
                    ObjectId = 5,
                    Flags00 = 0x01,
                },
                [6] = new TR1Object
                {
                    ObjectId = 6,
                    Flags00 = 0x78,
                },
                [7] = new TR1Object
                {
                    ObjectId = 7,
                    Flags00 = 0x7B,
                },
                [8] = new TR1Object
                {
                    ObjectId = 8,
                    Flags00 = 0x7B,
                },
                [9] = new TR1Object
                {
                    ObjectId = 9,
                    Flags00 = 0x7B,
                },
                [10] = new TR1Object
                {
                    ObjectId = 10,
                    Flags00 = 0x7B,
                },
                [11] = new TR1Object
                {
                    ObjectId = 11,
                    Flags00 = 0x7B,
                },
                [12] = new TR1Object
                {
                    ObjectId = 12,
                    Flags00 = 0x7B,
                },
                [13] = new TR1Object
                {
                    ObjectId = 13,
                    Flags00 = 0x7B,
                },
                [14] = new TR1Object
                {
                    ObjectId = 14,
                    Flags00 = 0x7B,
                },
                [15] = new TR1Object
                {
                    ObjectId = 15,
                    Flags00 = 0x7B,
                },
                [16] = new TR1Object
                {
                    ObjectId = 16,
                    Flags00 = 0x7B,
                },
                [17] = new TR1Object
                {
                    ObjectId = 17,
                    Flags00 = 0x7B,
                },
                [18] = new TR1Object
                {
                    ObjectId = 18,
                    Flags00 = 0x7B,
                },
                [19] = new TR1Object
                {
                    ObjectId = 19,
                    Flags00 = 0x7B,
                },
                [20] = new TR1Object
                {
                    ObjectId = 20,
                    Flags00 = 0x7B,
                },
                [21] = new TR1Object
                {
                    ObjectId = 21,
                    Flags00 = 0x7B,
                },
                [22] = new TR1Object
                {
                    ObjectId = 22,
                    Flags00 = 0x7B,
                },
                [23] = new TR1Object
                {
                    ObjectId = 23,
                    Flags00 = 0x7B,
                },
                [24] = new TR1Object
                {
                    ObjectId = 24,
                    Flags00 = 0x71,
                },
                [25] = new TR1Object
                {
                    ObjectId = 25,
                    Flags00 = 0x00,
                },
                [26] = new TR1Object
                {
                    ObjectId = 26,
                    Flags00 = 0x00,
                },
                [27] = new TR1Object
                {
                    ObjectId = 27,
                    Flags00 = 0x7B,
                },
                [28] = new TR1Object
                {
                    ObjectId = 28,
                    Flags00 = 0x7B,
                },
                [29] = new TR1Object
                {
                    ObjectId = 29,
                    Flags00 = 0x01,
                },
                [30] = new TR1Object
                {
                    ObjectId = 30,
                    Flags00 = 0x7B,
                },
                [31] = new TR1Object
                {
                    ObjectId = 31,
                    Flags00 = 0x7B,
                },
                [32] = new TR1Object
                {
                    ObjectId = 32,
                    Flags00 = 0x7B,
                },
                [33] = new TR1Object
                {
                    ObjectId = 33,
                    Flags00 = 0x7B,
                },
                [34] = new TR1Object
                {
                    ObjectId = 34,
                    Flags00 = 0x7B,
                },
                [35] = new TR1Object
                {
                    ObjectId = 35,
                    Flags00 = 0x69,
                },
                [36] = new TR1Object
                {
                    ObjectId = 36,
                    Flags00 = 0x61,
                },
                [37] = new TR1Object
                {
                    ObjectId = 37,
                    Flags00 = 0x01,
                },
                [38] = new TR1Object
                {
                    ObjectId = 38,
                    Flags00 = 0x69,
                },
                [39] = new TR1Object
                {
                    ObjectId = 39,
                    Flags00 = 0x21,
                },
                [40] = new TR1Object
                {
                    ObjectId = 40,
                    Flags00 = 0x21,
                },
                [41] = new TR1Object
                {
                    ObjectId = 41,
                    Flags00 = 0x00,
                },
                [42] = new TR1Object
                {
                    ObjectId = 42,
                    Flags00 = 0x61,
                },
                [43] = new TR1Object
                {
                    ObjectId = 43,
                    Flags00 = 0x68,
                },
                [44] = new TR1Object
                {
                    ObjectId = 44,
                    Flags00 = 0x60,
                },
                [45] = new TR1Object
                {
                    ObjectId = 45,
                    Flags00 = 0x60,
                },
                [46] = new TR1Object
                {
                    ObjectId = 46,
                    Flags00 = 0x21,
                },
                [47] = new TR1Object
                {
                    ObjectId = 47,
                    Flags00 = 0x68,
                },
                [48] = new TR1Object
                {
                    ObjectId = 48,
                    Flags00 = 0x69,
                },
                [49] = new TR1Object
                {
                    ObjectId = 49,
                    Flags00 = 0x68,
                },
                [50] = new TR1Object
                {
                    ObjectId = 50,
                    Flags00 = 0x68,
                },
                [51] = new TR1Object
                {
                    ObjectId = 51,
                    Flags00 = 0x68,
                },
                [52] = new TR1Object
                {
                    ObjectId = 52,
                    Flags00 = 0x68,
                },
                [53] = new TR1Object
                {
                    ObjectId = 53,
                    Flags00 = 0x69,
                },
                [54] = new TR1Object
                {
                    ObjectId = 54,
                    Flags00 = 0x68,
                },
                [55] = new TR1Object
                {
                    ObjectId = 55,
                    Flags00 = 0x61,
                },
                [56] = new TR1Object
                {
                    ObjectId = 56,
                    Flags00 = 0x60,
                },
                [57] = new TR1Object
                {
                    ObjectId = 57,
                    Flags00 = 0x61,
                },
                [58] = new TR1Object
                {
                    ObjectId = 58,
                    Flags00 = 0x61,
                },
                [59] = new TR1Object
                {
                    ObjectId = 59,
                    Flags00 = 0x61,
                },
                [60] = new TR1Object
                {
                    ObjectId = 60,
                    Flags00 = 0x60,
                },
                [61] = new TR1Object
                {
                    ObjectId = 61,
                    Flags00 = 0x60,
                },
                [62] = new TR1Object
                {
                    ObjectId = 62,
                    Flags00 = 0x60,
                },
                [63] = new TR1Object
                {
                    ObjectId = 63,
                    Flags00 = 0x60,
                },
                [64] = new TR1Object
                {
                    ObjectId = 64,
                    Flags00 = 0x60,
                },
                [65] = new TR1Object
                {
                    ObjectId = 65,
                    Flags00 = 0x61,
                },
                [66] = new TR1Object
                {
                    ObjectId = 66,
                    Flags00 = 0x61,
                },
                [67] = new TR1Object
                {
                    ObjectId = 67,
                    Flags00 = 0x00,
                },
                [68] = new TR1Object
                {
                    ObjectId = 68,
                    Flags00 = 0x00,
                },
                [69] = new TR1Object
                {
                    ObjectId = 69,
                    Flags00 = 0x00,
                },
                [70] = new TR1Object
                {
                    ObjectId = 70,
                    Flags00 = 0x00,
                },
                [71] = new TR1Object
                {
                    ObjectId = 71,
                    Flags00 = 0x01,
                },
                [72] = new TR1Object
                {
                    ObjectId = 72,
                    Flags00 = 0x01,
                },
                [73] = new TR1Object
                {
                    ObjectId = 73,
                    Flags00 = 0x00,
                },
                [74] = new TR1Object
                {
                    ObjectId = 74,
                    Flags00 = 0x20,
                },
                [75] = new TR1Object
                {
                    ObjectId = 75,
                    Flags00 = 0x20,
                },
                [76] = new TR1Object
                {
                    ObjectId = 76,
                    Flags00 = 0x20,
                },
                [77] = new TR1Object
                {
                    ObjectId = 77,
                    Flags00 = 0x00,
                },
                [78] = new TR1Object
                {
                    ObjectId = 78,
                    Flags00 = 0x00,
                },
                [79] = new TR1Object
                {
                    ObjectId = 79,
                    Flags00 = 0x00,
                },
                [80] = new TR1Object
                {
                    ObjectId = 80,
                    Flags00 = 0x00,
                },
                [81] = new TR1Object
                {
                    ObjectId = 81,
                    Flags00 = 0x01,
                },
                [82] = new TR1Object
                {
                    ObjectId = 82,
                    Flags00 = 0x01,
                },
                [83] = new TR1Object
                {
                    ObjectId = 83,
                    Flags00 = 0x21,
                },
                [84] = new TR1Object
                {
                    ObjectId = 84,
                    Flags00 = 0x21,
                },
                [85] = new TR1Object
                {
                    ObjectId = 85,
                    Flags00 = 0x21,
                },
                [86] = new TR1Object
                {
                    ObjectId = 86,
                    Flags00 = 0x21,
                },
                [87] = new TR1Object
                {
                    ObjectId = 87,
                    Flags00 = 0x21,
                },
                [88] = new TR1Object
                {
                    ObjectId = 88,
                    Flags00 = 0x20,
                },
                [89] = new TR1Object
                {
                    ObjectId = 89,
                    Flags00 = 0x21,
                },
                [90] = new TR1Object
                {
                    ObjectId = 90,
                    Flags00 = 0x21,
                },
                [91] = new TR1Object
                {
                    ObjectId = 91,
                    Flags00 = 0x21,
                },
                [92] = new TR1Object
                {
                    ObjectId = 92,
                    Flags00 = 0x20,
                },
                [93] = new TR1Object
                {
                    ObjectId = 93,
                    Flags00 = 0x21,
                },
                [94] = new TR1Object
                {
                    ObjectId = 94,
                    Flags00 = 0x21,
                },
                [95] = new TR1Object
                {
                    ObjectId = 95,
                    Flags00 = 0x01,
                },
                [96] = new TR1Object
                {
                    ObjectId = 96,
                    Flags00 = 0x01,
                },
                [97] = new TR1Object
                {
                    ObjectId = 97,
                    Flags00 = 0x01,
                },
                [98] = new TR1Object
                {
                    ObjectId = 98,
                    Flags00 = 0x00,
                },
                [99] = new TR1Object
                {
                    ObjectId = 99,
                    Flags00 = 0x01,
                },
                [100] = new TR1Object
                {
                    ObjectId = 100,
                    Flags00 = 0x01,
                },
                [101] = new TR1Object
                {
                    ObjectId = 101,
                    Flags00 = 0x01,
                },
                [102] = new TR1Object
                {
                    ObjectId = 102,
                    Flags00 = 0x01,
                },
                [103] = new TR1Object
                {
                    ObjectId = 103,
                    Flags00 = 0x01,
                },
                [104] = new TR1Object
                {
                    ObjectId = 104,
                    Flags00 = 0x01,
                },
                [105] = new TR1Object
                {
                    ObjectId = 105,
                    Flags00 = 0x01,
                },
                [106] = new TR1Object
                {
                    ObjectId = 106,
                    Flags00 = 0x01,
                },
                [107] = new TR1Object
                {
                    ObjectId = 107,
                    Flags00 = 0x00,
                },
                [108] = new TR1Object
                {
                    ObjectId = 108,
                    Flags00 = 0x01,
                },
                [109] = new TR1Object
                {
                    ObjectId = 109,
                    Flags00 = 0x01,
                },
                [110] = new TR1Object
                {
                    ObjectId = 110,
                    Flags00 = 0x20,
                },
                [111] = new TR1Object
                {
                    ObjectId = 111,
                    Flags00 = 0x20,
                },
                [112] = new TR1Object
                {
                    ObjectId = 112,
                    Flags00 = 0x20,
                },
                [113] = new TR1Object
                {
                    ObjectId = 113,
                    Flags00 = 0x20,
                },
                [114] = new TR1Object
                {
                    ObjectId = 114,
                    Flags00 = 0x00,
                },
                [115] = new TR1Object
                {
                    ObjectId = 115,
                    Flags00 = 0x00,
                },
                [116] = new TR1Object
                {
                    ObjectId = 116,
                    Flags00 = 0x00,
                },
                [117] = new TR1Object
                {
                    ObjectId = 117,
                    Flags00 = 0x00,
                },
                [118] = new TR1Object
                {
                    ObjectId = 118,
                    Flags00 = 0x20,
                },
                [119] = new TR1Object
                {
                    ObjectId = 119,
                    Flags00 = 0x20,
                },
                [120] = new TR1Object
                {
                    ObjectId = 120,
                    Flags00 = 0x20,
                },
                [121] = new TR1Object
                {
                    ObjectId = 121,
                    Flags00 = 0x20,
                },
                [122] = new TR1Object
                {
                    ObjectId = 122,
                    Flags00 = 0x20,
                },
                [123] = new TR1Object
                {
                    ObjectId = 123,
                    Flags00 = 0x20,
                },
                [124] = new TR1Object
                {
                    ObjectId = 124,
                    Flags00 = 0x20,
                },
                [125] = new TR1Object
                {
                    ObjectId = 125,
                    Flags00 = 0x20,
                },
                [126] = new TR1Object
                {
                    ObjectId = 126,
                    Flags00 = 0x20,
                },
                [127] = new TR1Object
                {
                    ObjectId = 127,
                    Flags00 = 0x00,
                },
                [128] = new TR1Object
                {
                    ObjectId = 128,
                    Flags00 = 0x00,
                },
                [129] = new TR1Object
                {
                    ObjectId = 129,
                    Flags00 = 0x20,
                },
                [130] = new TR1Object
                {
                    ObjectId = 130,
                    Flags00 = 0x20,
                },
                [131] = new TR1Object
                {
                    ObjectId = 131,
                    Flags00 = 0x20,
                },
                [132] = new TR1Object
                {
                    ObjectId = 132,
                    Flags00 = 0x20,
                },
                [133] = new TR1Object
                {
                    ObjectId = 133,
                    Flags00 = 0x00,
                },
                [134] = new TR1Object
                {
                    ObjectId = 134,
                    Flags00 = 0x00,
                },
                [135] = new TR1Object
                {
                    ObjectId = 135,
                    Flags00 = 0x00,
                },
                [136] = new TR1Object
                {
                    ObjectId = 136,
                    Flags00 = 0x00,
                },
                [137] = new TR1Object
                {
                    ObjectId = 137,
                    Flags00 = 0x20,
                },
                [138] = new TR1Object
                {
                    ObjectId = 138,
                    Flags00 = 0x20,
                },
                [139] = new TR1Object
                {
                    ObjectId = 139,
                    Flags00 = 0x20,
                },
                [140] = new TR1Object
                {
                    ObjectId = 140,
                    Flags00 = 0x20,
                },
                [141] = new TR1Object
                {
                    ObjectId = 141,
                    Flags00 = 0x20,
                },
                [142] = new TR1Object
                {
                    ObjectId = 142,
                    Flags00 = 0x20,
                },
                [143] = new TR1Object
                {
                    ObjectId = 143,
                    Flags00 = 0x20,
                },
                [144] = new TR1Object
                {
                    ObjectId = 144,
                    Flags00 = 0x20,
                },
                [145] = new TR1Object
                {
                    ObjectId = 145,
                    Flags00 = 0x21,
                },
                [146] = new TR1Object
                {
                    ObjectId = 146,
                    Flags00 = 0x20,
                },
                [147] = new TR1Object
                {
                    ObjectId = 147,
                    Flags00 = 0x61,
                },
                [148] = new TR1Object
                {
                    ObjectId = 148,
                    Flags00 = 0x00,
                },
                [149] = new TR1Object
                {
                    ObjectId = 149,
                    Flags00 = 0x00,
                },
                [150] = new TR1Object
                {
                    ObjectId = 150,
                    Flags00 = 0x01,
                },
                [151] = new TR1Object
                {
                    ObjectId = 151,
                    Flags00 = 0x01,
                },
                [152] = new TR1Object
                {
                    ObjectId = 152,
                    Flags00 = 0x00,
                },
                [153] = new TR1Object
                {
                    ObjectId = 153,
                    Flags00 = 0x01,
                },
                [154] = new TR1Object
                {
                    ObjectId = 154,
                    Flags00 = 0x00,
                },
                [155] = new TR1Object
                {
                    ObjectId = 155,
                    Flags00 = 0x01,
                },
                [156] = new TR1Object
                {
                    ObjectId = 156,
                    Flags00 = 0x01,
                },
                [157] = new TR1Object
                {
                    ObjectId = 157,
                    Flags00 = 0x00,
                },
                [158] = new TR1Object
                {
                    ObjectId = 158,
                    Flags00 = 0x01,
                },
                [159] = new TR1Object
                {
                    ObjectId = 159,
                    Flags00 = 0x00,
                },
                [160] = new TR1Object
                {
                    ObjectId = 160,
                    Flags00 = 0x01,
                },
                [161] = new TR1Object
                {
                    ObjectId = 161,
                    Flags00 = 0x00,
                },
                [162] = new TR1Object
                {
                    ObjectId = 162,
                    Flags00 = 0x60,
                },
                [163] = new TR1Object
                {
                    ObjectId = 163,
                    Flags00 = 0x00,
                },
                [164] = new TR1Object
                {
                    ObjectId = 164,
                    Flags00 = 0x01,
                },
                [165] = new TR1Object
                {
                    ObjectId = 165,
                    Flags00 = 0x00,
                },
                [166] = new TR1Object
                {
                    ObjectId = 166,
                    Flags00 = 0x01,
                },
                [167] = new TR1Object
                {
                    ObjectId = 167,
                    Flags00 = 0x00,
                },
                [168] = new TR1Object
                {
                    ObjectId = 168,
                    Flags00 = 0x01,
                },
                [169] = new TR1Object
                {
                    ObjectId = 169,
                    Flags00 = 0x01,
                },
                [170] = new TR1Object
                {
                    ObjectId = 170,
                    Flags00 = 0x20,
                },
                [171] = new TR1Object
                {
                    ObjectId = 171,
                    Flags00 = 0x00,
                },
                [172] = new TR1Object
                {
                    ObjectId = 172,
                    Flags00 = 0x01,
                },
                [173] = new TR1Object
                {
                    ObjectId = 173,
                    Flags00 = 0x01,
                },
                [174] = new TR1Object
                {
                    ObjectId = 174,
                    Flags00 = 0x00,
                },
                [175] = new TR1Object
                {
                    ObjectId = 175,
                    Flags00 = 0x00,
                },
                [176] = new TR1Object
                {
                    ObjectId = 176,
                    Flags00 = 0x01,
                },
                [177] = new TR1Object
                {
                    ObjectId = 177,
                    Flags00 = 0x21,
                },
                [178] = new TR1Object
                {
                    ObjectId = 178,
                    Flags00 = 0x01,
                },
                [179] = new TR1Object
                {
                    ObjectId = 179,
                    Flags00 = 0x21,
                },
                [180] = new TR1Object
                {
                    ObjectId = 180,
                    Flags00 = 0x69,
                },
                [181] = new TR1Object
                {
                    ObjectId = 181,
                    Flags00 = 0x61,
                },
                [182] = new TR1Object
                {
                    ObjectId = 182,
                    Flags00 = 0x68,
                },
                [183] = new TR1Object
                {
                    ObjectId = 183,
                    Flags00 = 0x21,
                },
            },
            [16] = new Dictionary<int, TR1Object> // Return to Egypt
            {
                [0] = new TR1Object
                {
                    ObjectId = 0,
                    Flags00 = 0x79,
                },
                [1] = new TR1Object
                {
                    ObjectId = 1,
                    Flags00 = 0x01,
                },
                [2] = new TR1Object
                {
                    ObjectId = 2,
                    Flags00 = 0x01,
                },
                [3] = new TR1Object
                {
                    ObjectId = 3,
                    Flags00 = 0x01,
                },
                [4] = new TR1Object
                {
                    ObjectId = 4,
                    Flags00 = 0x01,
                },
                [5] = new TR1Object
                {
                    ObjectId = 5,
                    Flags00 = 0x00,
                },
                [6] = new TR1Object
                {
                    ObjectId = 6,
                    Flags00 = 0x78,
                },
                [7] = new TR1Object
                {
                    ObjectId = 7,
                    Flags00 = 0x7B,
                },
                [8] = new TR1Object
                {
                    ObjectId = 8,
                    Flags00 = 0x7B,
                },
                [9] = new TR1Object
                {
                    ObjectId = 9,
                    Flags00 = 0x7B,
                },
                [10] = new TR1Object
                {
                    ObjectId = 10,
                    Flags00 = 0x7B,
                },
                [11] = new TR1Object
                {
                    ObjectId = 11,
                    Flags00 = 0x7B,
                },
                [12] = new TR1Object
                {
                    ObjectId = 12,
                    Flags00 = 0x7B,
                },
                [13] = new TR1Object
                {
                    ObjectId = 13,
                    Flags00 = 0x7B,
                },
                [14] = new TR1Object
                {
                    ObjectId = 14,
                    Flags00 = 0x7B,
                },
                [15] = new TR1Object
                {
                    ObjectId = 15,
                    Flags00 = 0x7B,
                },
                [16] = new TR1Object
                {
                    ObjectId = 16,
                    Flags00 = 0x7B,
                },
                [17] = new TR1Object
                {
                    ObjectId = 17,
                    Flags00 = 0x7B,
                },
                [18] = new TR1Object
                {
                    ObjectId = 18,
                    Flags00 = 0x7B,
                },
                [19] = new TR1Object
                {
                    ObjectId = 19,
                    Flags00 = 0x7B,
                },
                [20] = new TR1Object
                {
                    ObjectId = 20,
                    Flags00 = 0x7B,
                },
                [21] = new TR1Object
                {
                    ObjectId = 21,
                    Flags00 = 0x7B,
                },
                [22] = new TR1Object
                {
                    ObjectId = 22,
                    Flags00 = 0x7B,
                },
                [23] = new TR1Object
                {
                    ObjectId = 23,
                    Flags00 = 0x7B,
                },
                [24] = new TR1Object
                {
                    ObjectId = 24,
                    Flags00 = 0x71,
                },
                [25] = new TR1Object
                {
                    ObjectId = 25,
                    Flags00 = 0x00,
                },
                [26] = new TR1Object
                {
                    ObjectId = 26,
                    Flags00 = 0x00,
                },
                [27] = new TR1Object
                {
                    ObjectId = 27,
                    Flags00 = 0x7B,
                },
                [28] = new TR1Object
                {
                    ObjectId = 28,
                    Flags00 = 0x7B,
                },
                [29] = new TR1Object
                {
                    ObjectId = 29,
                    Flags00 = 0x01,
                },
                [30] = new TR1Object
                {
                    ObjectId = 30,
                    Flags00 = 0x7B,
                },
                [31] = new TR1Object
                {
                    ObjectId = 31,
                    Flags00 = 0x7B,
                },
                [32] = new TR1Object
                {
                    ObjectId = 32,
                    Flags00 = 0x7B,
                },
                [33] = new TR1Object
                {
                    ObjectId = 33,
                    Flags00 = 0x7B,
                },
                [34] = new TR1Object
                {
                    ObjectId = 34,
                    Flags00 = 0x7B,
                },
                [35] = new TR1Object
                {
                    ObjectId = 35,
                    Flags00 = 0x69,
                },
                [36] = new TR1Object
                {
                    ObjectId = 36,
                    Flags00 = 0x60,
                },
                [37] = new TR1Object
                {
                    ObjectId = 37,
                    Flags00 = 0x01,
                },
                [38] = new TR1Object
                {
                    ObjectId = 38,
                    Flags00 = 0x69,
                },
                [39] = new TR1Object
                {
                    ObjectId = 39,
                    Flags00 = 0x20,
                },
                [40] = new TR1Object
                {
                    ObjectId = 40,
                    Flags00 = 0x20,
                },
                [41] = new TR1Object
                {
                    ObjectId = 41,
                    Flags00 = 0x00,
                },
                [42] = new TR1Object
                {
                    ObjectId = 42,
                    Flags00 = 0x60,
                },
                [43] = new TR1Object
                {
                    ObjectId = 43,
                    Flags00 = 0x68,
                },
                [44] = new TR1Object
                {
                    ObjectId = 44,
                    Flags00 = 0x60,
                },
                [45] = new TR1Object
                {
                    ObjectId = 45,
                    Flags00 = 0x60,
                },
                [46] = new TR1Object
                {
                    ObjectId = 46,
                    Flags00 = 0x20,
                },
                [47] = new TR1Object
                {
                    ObjectId = 47,
                    Flags00 = 0x68,
                },
                [48] = new TR1Object
                {
                    ObjectId = 48,
                    Flags00 = 0x69,
                },
                [49] = new TR1Object
                {
                    ObjectId = 49,
                    Flags00 = 0x68,
                },
                [50] = new TR1Object
                {
                    ObjectId = 50,
                    Flags00 = 0x68,
                },
                [51] = new TR1Object
                {
                    ObjectId = 51,
                    Flags00 = 0x68,
                },
                [52] = new TR1Object
                {
                    ObjectId = 52,
                    Flags00 = 0x68,
                },
                [53] = new TR1Object
                {
                    ObjectId = 53,
                    Flags00 = 0x68,
                },
                [54] = new TR1Object
                {
                    ObjectId = 54,
                    Flags00 = 0x68,
                },
                [55] = new TR1Object
                {
                    ObjectId = 55,
                    Flags00 = 0x61,
                },
                [56] = new TR1Object
                {
                    ObjectId = 56,
                    Flags00 = 0x61,
                },
                [57] = new TR1Object
                {
                    ObjectId = 57,
                    Flags00 = 0x61,
                },
                [58] = new TR1Object
                {
                    ObjectId = 58,
                    Flags00 = 0x61,
                },
                [59] = new TR1Object
                {
                    ObjectId = 59,
                    Flags00 = 0x61,
                },
                [60] = new TR1Object
                {
                    ObjectId = 60,
                    Flags00 = 0x61,
                },
                [61] = new TR1Object
                {
                    ObjectId = 61,
                    Flags00 = 0x61,
                },
                [62] = new TR1Object
                {
                    ObjectId = 62,
                    Flags00 = 0x60,
                },
                [63] = new TR1Object
                {
                    ObjectId = 63,
                    Flags00 = 0x60,
                },
                [64] = new TR1Object
                {
                    ObjectId = 64,
                    Flags00 = 0x60,
                },
                [65] = new TR1Object
                {
                    ObjectId = 65,
                    Flags00 = 0x61,
                },
                [66] = new TR1Object
                {
                    ObjectId = 66,
                    Flags00 = 0x60,
                },
                [67] = new TR1Object
                {
                    ObjectId = 67,
                    Flags00 = 0x00,
                },
                [68] = new TR1Object
                {
                    ObjectId = 68,
                    Flags00 = 0x01,
                },
                [69] = new TR1Object
                {
                    ObjectId = 69,
                    Flags00 = 0x01,
                },
                [70] = new TR1Object
                {
                    ObjectId = 70,
                    Flags00 = 0x01,
                },
                [71] = new TR1Object
                {
                    ObjectId = 71,
                    Flags00 = 0x01,
                },
                [72] = new TR1Object
                {
                    ObjectId = 72,
                    Flags00 = 0x01,
                },
                [73] = new TR1Object
                {
                    ObjectId = 73,
                    Flags00 = 0x00,
                },
                [74] = new TR1Object
                {
                    ObjectId = 74,
                    Flags00 = 0x20,
                },
                [75] = new TR1Object
                {
                    ObjectId = 75,
                    Flags00 = 0x20,
                },
                [76] = new TR1Object
                {
                    ObjectId = 76,
                    Flags00 = 0x20,
                },
                [77] = new TR1Object
                {
                    ObjectId = 77,
                    Flags00 = 0x00,
                },
                [78] = new TR1Object
                {
                    ObjectId = 78,
                    Flags00 = 0x00,
                },
                [79] = new TR1Object
                {
                    ObjectId = 79,
                    Flags00 = 0x00,
                },
                [80] = new TR1Object
                {
                    ObjectId = 80,
                    Flags00 = 0x00,
                },
                [81] = new TR1Object
                {
                    ObjectId = 81,
                    Flags00 = 0x01,
                },
                [82] = new TR1Object
                {
                    ObjectId = 82,
                    Flags00 = 0x01,
                },
                [83] = new TR1Object
                {
                    ObjectId = 83,
                    Flags00 = 0x21,
                },
                [84] = new TR1Object
                {
                    ObjectId = 84,
                    Flags00 = 0x21,
                },
                [85] = new TR1Object
                {
                    ObjectId = 85,
                    Flags00 = 0x21,
                },
                [86] = new TR1Object
                {
                    ObjectId = 86,
                    Flags00 = 0x21,
                },
                [87] = new TR1Object
                {
                    ObjectId = 87,
                    Flags00 = 0x21,
                },
                [88] = new TR1Object
                {
                    ObjectId = 88,
                    Flags00 = 0x20,
                },
                [89] = new TR1Object
                {
                    ObjectId = 89,
                    Flags00 = 0x21,
                },
                [90] = new TR1Object
                {
                    ObjectId = 90,
                    Flags00 = 0x21,
                },
                [91] = new TR1Object
                {
                    ObjectId = 91,
                    Flags00 = 0x21,
                },
                [92] = new TR1Object
                {
                    ObjectId = 92,
                    Flags00 = 0x20,
                },
                [93] = new TR1Object
                {
                    ObjectId = 93,
                    Flags00 = 0x21,
                },
                [94] = new TR1Object
                {
                    ObjectId = 94,
                    Flags00 = 0x21,
                },
                [95] = new TR1Object
                {
                    ObjectId = 95,
                    Flags00 = 0x01,
                },
                [96] = new TR1Object
                {
                    ObjectId = 96,
                    Flags00 = 0x01,
                },
                [97] = new TR1Object
                {
                    ObjectId = 97,
                    Flags00 = 0x01,
                },
                [98] = new TR1Object
                {
                    ObjectId = 98,
                    Flags00 = 0x00,
                },
                [99] = new TR1Object
                {
                    ObjectId = 99,
                    Flags00 = 0x01,
                },
                [100] = new TR1Object
                {
                    ObjectId = 100,
                    Flags00 = 0x01,
                },
                [101] = new TR1Object
                {
                    ObjectId = 101,
                    Flags00 = 0x01,
                },
                [102] = new TR1Object
                {
                    ObjectId = 102,
                    Flags00 = 0x01,
                },
                [103] = new TR1Object
                {
                    ObjectId = 103,
                    Flags00 = 0x01,
                },
                [104] = new TR1Object
                {
                    ObjectId = 104,
                    Flags00 = 0x01,
                },
                [105] = new TR1Object
                {
                    ObjectId = 105,
                    Flags00 = 0x01,
                },
                [106] = new TR1Object
                {
                    ObjectId = 106,
                    Flags00 = 0x01,
                },
                [107] = new TR1Object
                {
                    ObjectId = 107,
                    Flags00 = 0x00,
                },
                [108] = new TR1Object
                {
                    ObjectId = 108,
                    Flags00 = 0x01,
                },
                [109] = new TR1Object
                {
                    ObjectId = 109,
                    Flags00 = 0x01,
                },
                [110] = new TR1Object
                {
                    ObjectId = 110,
                    Flags00 = 0x20,
                },
                [111] = new TR1Object
                {
                    ObjectId = 111,
                    Flags00 = 0x20,
                },
                [112] = new TR1Object
                {
                    ObjectId = 112,
                    Flags00 = 0x20,
                },
                [113] = new TR1Object
                {
                    ObjectId = 113,
                    Flags00 = 0x20,
                },
                [114] = new TR1Object
                {
                    ObjectId = 114,
                    Flags00 = 0x00,
                },
                [115] = new TR1Object
                {
                    ObjectId = 115,
                    Flags00 = 0x00,
                },
                [116] = new TR1Object
                {
                    ObjectId = 116,
                    Flags00 = 0x00,
                },
                [117] = new TR1Object
                {
                    ObjectId = 117,
                    Flags00 = 0x00,
                },
                [118] = new TR1Object
                {
                    ObjectId = 118,
                    Flags00 = 0x21,
                },
                [119] = new TR1Object
                {
                    ObjectId = 119,
                    Flags00 = 0x21,
                },
                [120] = new TR1Object
                {
                    ObjectId = 120,
                    Flags00 = 0x21,
                },
                [121] = new TR1Object
                {
                    ObjectId = 121,
                    Flags00 = 0x21,
                },
                [122] = new TR1Object
                {
                    ObjectId = 122,
                    Flags00 = 0x20,
                },
                [123] = new TR1Object
                {
                    ObjectId = 123,
                    Flags00 = 0x20,
                },
                [124] = new TR1Object
                {
                    ObjectId = 124,
                    Flags00 = 0x20,
                },
                [125] = new TR1Object
                {
                    ObjectId = 125,
                    Flags00 = 0x20,
                },
                [126] = new TR1Object
                {
                    ObjectId = 126,
                    Flags00 = 0x20,
                },
                [127] = new TR1Object
                {
                    ObjectId = 127,
                    Flags00 = 0x00,
                },
                [128] = new TR1Object
                {
                    ObjectId = 128,
                    Flags00 = 0x00,
                },
                [129] = new TR1Object
                {
                    ObjectId = 129,
                    Flags00 = 0x21,
                },
                [130] = new TR1Object
                {
                    ObjectId = 130,
                    Flags00 = 0x20,
                },
                [131] = new TR1Object
                {
                    ObjectId = 131,
                    Flags00 = 0x20,
                },
                [132] = new TR1Object
                {
                    ObjectId = 132,
                    Flags00 = 0x20,
                },
                [133] = new TR1Object
                {
                    ObjectId = 133,
                    Flags00 = 0x01,
                },
                [134] = new TR1Object
                {
                    ObjectId = 134,
                    Flags00 = 0x00,
                },
                [135] = new TR1Object
                {
                    ObjectId = 135,
                    Flags00 = 0x00,
                },
                [136] = new TR1Object
                {
                    ObjectId = 136,
                    Flags00 = 0x00,
                },
                [137] = new TR1Object
                {
                    ObjectId = 137,
                    Flags00 = 0x21,
                },
                [138] = new TR1Object
                {
                    ObjectId = 138,
                    Flags00 = 0x20,
                },
                [139] = new TR1Object
                {
                    ObjectId = 139,
                    Flags00 = 0x20,
                },
                [140] = new TR1Object
                {
                    ObjectId = 140,
                    Flags00 = 0x20,
                },
                [141] = new TR1Object
                {
                    ObjectId = 141,
                    Flags00 = 0x20,
                },
                [142] = new TR1Object
                {
                    ObjectId = 142,
                    Flags00 = 0x20,
                },
                [143] = new TR1Object
                {
                    ObjectId = 143,
                    Flags00 = 0x20,
                },
                [144] = new TR1Object
                {
                    ObjectId = 144,
                    Flags00 = 0x20,
                },
                [145] = new TR1Object
                {
                    ObjectId = 145,
                    Flags00 = 0x20,
                },
                [146] = new TR1Object
                {
                    ObjectId = 146,
                    Flags00 = 0x20,
                },
                [147] = new TR1Object
                {
                    ObjectId = 147,
                    Flags00 = 0x60,
                },
                [148] = new TR1Object
                {
                    ObjectId = 148,
                    Flags00 = 0x00,
                },
                [149] = new TR1Object
                {
                    ObjectId = 149,
                    Flags00 = 0x00,
                },
                [150] = new TR1Object
                {
                    ObjectId = 150,
                    Flags00 = 0x01,
                },
                [151] = new TR1Object
                {
                    ObjectId = 151,
                    Flags00 = 0x00,
                },
                [152] = new TR1Object
                {
                    ObjectId = 152,
                    Flags00 = 0x00,
                },
                [153] = new TR1Object
                {
                    ObjectId = 153,
                    Flags00 = 0x01,
                },
                [154] = new TR1Object
                {
                    ObjectId = 154,
                    Flags00 = 0x00,
                },
                [155] = new TR1Object
                {
                    ObjectId = 155,
                    Flags00 = 0x01,
                },
                [156] = new TR1Object
                {
                    ObjectId = 156,
                    Flags00 = 0x01,
                },
                [157] = new TR1Object
                {
                    ObjectId = 157,
                    Flags00 = 0x00,
                },
                [158] = new TR1Object
                {
                    ObjectId = 158,
                    Flags00 = 0x01,
                },
                [159] = new TR1Object
                {
                    ObjectId = 159,
                    Flags00 = 0x00,
                },
                [160] = new TR1Object
                {
                    ObjectId = 160,
                    Flags00 = 0x00,
                },
                [161] = new TR1Object
                {
                    ObjectId = 161,
                    Flags00 = 0x00,
                },
                [162] = new TR1Object
                {
                    ObjectId = 162,
                    Flags00 = 0x60,
                },
                [163] = new TR1Object
                {
                    ObjectId = 163,
                    Flags00 = 0x00,
                },
                [164] = new TR1Object
                {
                    ObjectId = 164,
                    Flags00 = 0x01,
                },
                [165] = new TR1Object
                {
                    ObjectId = 165,
                    Flags00 = 0x00,
                },
                [166] = new TR1Object
                {
                    ObjectId = 166,
                    Flags00 = 0x01,
                },
                [167] = new TR1Object
                {
                    ObjectId = 167,
                    Flags00 = 0x00,
                },
                [168] = new TR1Object
                {
                    ObjectId = 168,
                    Flags00 = 0x01,
                },
                [169] = new TR1Object
                {
                    ObjectId = 169,
                    Flags00 = 0x01,
                },
                [170] = new TR1Object
                {
                    ObjectId = 170,
                    Flags00 = 0x20,
                },
            },
            [17] = new Dictionary<int, TR1Object> // Temple of the Cat
            {
                [0] = new TR1Object
                {
                    ObjectId = 0,
                    Flags00 = 0x79,
                },
                [1] = new TR1Object
                {
                    ObjectId = 1,
                    Flags00 = 0x01,
                },
                [2] = new TR1Object
                {
                    ObjectId = 2,
                    Flags00 = 0x01,
                },
                [3] = new TR1Object
                {
                    ObjectId = 3,
                    Flags00 = 0x01,
                },
                [4] = new TR1Object
                {
                    ObjectId = 4,
                    Flags00 = 0x01,
                },
                [5] = new TR1Object
                {
                    ObjectId = 5,
                    Flags00 = 0x00,
                },
                [6] = new TR1Object
                {
                    ObjectId = 6,
                    Flags00 = 0x78,
                },
                [7] = new TR1Object
                {
                    ObjectId = 7,
                    Flags00 = 0x7B,
                },
                [8] = new TR1Object
                {
                    ObjectId = 8,
                    Flags00 = 0x7B,
                },
                [9] = new TR1Object
                {
                    ObjectId = 9,
                    Flags00 = 0x7B,
                },
                [10] = new TR1Object
                {
                    ObjectId = 10,
                    Flags00 = 0x7B,
                },
                [11] = new TR1Object
                {
                    ObjectId = 11,
                    Flags00 = 0x7B,
                },
                [12] = new TR1Object
                {
                    ObjectId = 12,
                    Flags00 = 0x7B,
                },
                [13] = new TR1Object
                {
                    ObjectId = 13,
                    Flags00 = 0x7B,
                },
                [14] = new TR1Object
                {
                    ObjectId = 14,
                    Flags00 = 0x7B,
                },
                [15] = new TR1Object
                {
                    ObjectId = 15,
                    Flags00 = 0x7B,
                },
                [16] = new TR1Object
                {
                    ObjectId = 16,
                    Flags00 = 0x7B,
                },
                [17] = new TR1Object
                {
                    ObjectId = 17,
                    Flags00 = 0x7B,
                },
                [18] = new TR1Object
                {
                    ObjectId = 18,
                    Flags00 = 0x7B,
                },
                [19] = new TR1Object
                {
                    ObjectId = 19,
                    Flags00 = 0x7B,
                },
                [20] = new TR1Object
                {
                    ObjectId = 20,
                    Flags00 = 0x7B,
                },
                [21] = new TR1Object
                {
                    ObjectId = 21,
                    Flags00 = 0x7B,
                },
                [22] = new TR1Object
                {
                    ObjectId = 22,
                    Flags00 = 0x7B,
                },
                [23] = new TR1Object
                {
                    ObjectId = 23,
                    Flags00 = 0x7B,
                },
                [24] = new TR1Object
                {
                    ObjectId = 24,
                    Flags00 = 0x71,
                },
                [25] = new TR1Object
                {
                    ObjectId = 25,
                    Flags00 = 0x00,
                },
                [26] = new TR1Object
                {
                    ObjectId = 26,
                    Flags00 = 0x00,
                },
                [27] = new TR1Object
                {
                    ObjectId = 27,
                    Flags00 = 0x7B,
                },
                [28] = new TR1Object
                {
                    ObjectId = 28,
                    Flags00 = 0x7B,
                },
                [29] = new TR1Object
                {
                    ObjectId = 29,
                    Flags00 = 0x01,
                },
                [30] = new TR1Object
                {
                    ObjectId = 30,
                    Flags00 = 0x7B,
                },
                [31] = new TR1Object
                {
                    ObjectId = 31,
                    Flags00 = 0x7B,
                },
                [32] = new TR1Object
                {
                    ObjectId = 32,
                    Flags00 = 0x7B,
                },
                [33] = new TR1Object
                {
                    ObjectId = 33,
                    Flags00 = 0x7B,
                },
                [34] = new TR1Object
                {
                    ObjectId = 34,
                    Flags00 = 0x7B,
                },
                [35] = new TR1Object
                {
                    ObjectId = 35,
                    Flags00 = 0x68,
                },
                [36] = new TR1Object
                {
                    ObjectId = 36,
                    Flags00 = 0x60,
                },
                [37] = new TR1Object
                {
                    ObjectId = 37,
                    Flags00 = 0x01,
                },
                [38] = new TR1Object
                {
                    ObjectId = 38,
                    Flags00 = 0x69,
                },
                [39] = new TR1Object
                {
                    ObjectId = 39,
                    Flags00 = 0x20,
                },
                [40] = new TR1Object
                {
                    ObjectId = 40,
                    Flags00 = 0x20,
                },
                [41] = new TR1Object
                {
                    ObjectId = 41,
                    Flags00 = 0x00,
                },
                [42] = new TR1Object
                {
                    ObjectId = 42,
                    Flags00 = 0x60,
                },
                [43] = new TR1Object
                {
                    ObjectId = 43,
                    Flags00 = 0x68,
                },
                [44] = new TR1Object
                {
                    ObjectId = 44,
                    Flags00 = 0x60,
                },
                [45] = new TR1Object
                {
                    ObjectId = 45,
                    Flags00 = 0x60,
                },
                [46] = new TR1Object
                {
                    ObjectId = 46,
                    Flags00 = 0x20,
                },
                [47] = new TR1Object
                {
                    ObjectId = 47,
                    Flags00 = 0x68,
                },
                [48] = new TR1Object
                {
                    ObjectId = 48,
                    Flags00 = 0x69,
                },
                [49] = new TR1Object
                {
                    ObjectId = 49,
                    Flags00 = 0x68,
                },
                [50] = new TR1Object
                {
                    ObjectId = 50,
                    Flags00 = 0x68,
                },
                [51] = new TR1Object
                {
                    ObjectId = 51,
                    Flags00 = 0x68,
                },
                [52] = new TR1Object
                {
                    ObjectId = 52,
                    Flags00 = 0x68,
                },
                [53] = new TR1Object
                {
                    ObjectId = 53,
                    Flags00 = 0x68,
                },
                [54] = new TR1Object
                {
                    ObjectId = 54,
                    Flags00 = 0x68,
                },
                [55] = new TR1Object
                {
                    ObjectId = 55,
                    Flags00 = 0x61,
                },
                [56] = new TR1Object
                {
                    ObjectId = 56,
                    Flags00 = 0x61,
                },
                [57] = new TR1Object
                {
                    ObjectId = 57,
                    Flags00 = 0x61,
                },
                [58] = new TR1Object
                {
                    ObjectId = 58,
                    Flags00 = 0x61,
                },
                [59] = new TR1Object
                {
                    ObjectId = 59,
                    Flags00 = 0x61,
                },
                [60] = new TR1Object
                {
                    ObjectId = 60,
                    Flags00 = 0x61,
                },
                [61] = new TR1Object
                {
                    ObjectId = 61,
                    Flags00 = 0x61,
                },
                [62] = new TR1Object
                {
                    ObjectId = 62,
                    Flags00 = 0x60,
                },
                [63] = new TR1Object
                {
                    ObjectId = 63,
                    Flags00 = 0x60,
                },
                [64] = new TR1Object
                {
                    ObjectId = 64,
                    Flags00 = 0x60,
                },
                [65] = new TR1Object
                {
                    ObjectId = 65,
                    Flags00 = 0x61,
                },
                [66] = new TR1Object
                {
                    ObjectId = 66,
                    Flags00 = 0x60,
                },
                [67] = new TR1Object
                {
                    ObjectId = 67,
                    Flags00 = 0x00,
                },
                [68] = new TR1Object
                {
                    ObjectId = 68,
                    Flags00 = 0x01,
                },
                [69] = new TR1Object
                {
                    ObjectId = 69,
                    Flags00 = 0x01,
                },
                [70] = new TR1Object
                {
                    ObjectId = 70,
                    Flags00 = 0x01,
                },
                [71] = new TR1Object
                {
                    ObjectId = 71,
                    Flags00 = 0x01,
                },
                [72] = new TR1Object
                {
                    ObjectId = 72,
                    Flags00 = 0x01,
                },
                [73] = new TR1Object
                {
                    ObjectId = 73,
                    Flags00 = 0x00,
                },
                [74] = new TR1Object
                {
                    ObjectId = 74,
                    Flags00 = 0x20,
                },
                [75] = new TR1Object
                {
                    ObjectId = 75,
                    Flags00 = 0x20,
                },
                [76] = new TR1Object
                {
                    ObjectId = 76,
                    Flags00 = 0x20,
                },
                [77] = new TR1Object
                {
                    ObjectId = 77,
                    Flags00 = 0x00,
                },
                [78] = new TR1Object
                {
                    ObjectId = 78,
                    Flags00 = 0x00,
                },
                [79] = new TR1Object
                {
                    ObjectId = 79,
                    Flags00 = 0x00,
                },
                [80] = new TR1Object
                {
                    ObjectId = 80,
                    Flags00 = 0x00,
                },
                [81] = new TR1Object
                {
                    ObjectId = 81,
                    Flags00 = 0x01,
                },
                [82] = new TR1Object
                {
                    ObjectId = 82,
                    Flags00 = 0x01,
                },
                [83] = new TR1Object
                {
                    ObjectId = 83,
                    Flags00 = 0x21,
                },
                [84] = new TR1Object
                {
                    ObjectId = 84,
                    Flags00 = 0x21,
                },
                [85] = new TR1Object
                {
                    ObjectId = 85,
                    Flags00 = 0x21,
                },
                [86] = new TR1Object
                {
                    ObjectId = 86,
                    Flags00 = 0x21,
                },
                [87] = new TR1Object
                {
                    ObjectId = 87,
                    Flags00 = 0x21,
                },
                [88] = new TR1Object
                {
                    ObjectId = 88,
                    Flags00 = 0x20,
                },
                [89] = new TR1Object
                {
                    ObjectId = 89,
                    Flags00 = 0x21,
                },
                [90] = new TR1Object
                {
                    ObjectId = 90,
                    Flags00 = 0x21,
                },
                [91] = new TR1Object
                {
                    ObjectId = 91,
                    Flags00 = 0x21,
                },
                [92] = new TR1Object
                {
                    ObjectId = 92,
                    Flags00 = 0x20,
                },
                [93] = new TR1Object
                {
                    ObjectId = 93,
                    Flags00 = 0x21,
                },
                [94] = new TR1Object
                {
                    ObjectId = 94,
                    Flags00 = 0x21,
                },
                [95] = new TR1Object
                {
                    ObjectId = 95,
                    Flags00 = 0x01,
                },
                [96] = new TR1Object
                {
                    ObjectId = 96,
                    Flags00 = 0x01,
                },
                [97] = new TR1Object
                {
                    ObjectId = 97,
                    Flags00 = 0x01,
                },
                [98] = new TR1Object
                {
                    ObjectId = 98,
                    Flags00 = 0x00,
                },
                [99] = new TR1Object
                {
                    ObjectId = 99,
                    Flags00 = 0x01,
                },
                [100] = new TR1Object
                {
                    ObjectId = 100,
                    Flags00 = 0x01,
                },
                [101] = new TR1Object
                {
                    ObjectId = 101,
                    Flags00 = 0x01,
                },
                [102] = new TR1Object
                {
                    ObjectId = 102,
                    Flags00 = 0x01,
                },
                [103] = new TR1Object
                {
                    ObjectId = 103,
                    Flags00 = 0x01,
                },
                [104] = new TR1Object
                {
                    ObjectId = 104,
                    Flags00 = 0x01,
                },
                [105] = new TR1Object
                {
                    ObjectId = 105,
                    Flags00 = 0x01,
                },
                [106] = new TR1Object
                {
                    ObjectId = 106,
                    Flags00 = 0x01,
                },
                [107] = new TR1Object
                {
                    ObjectId = 107,
                    Flags00 = 0x00,
                },
                [108] = new TR1Object
                {
                    ObjectId = 108,
                    Flags00 = 0x01,
                },
                [109] = new TR1Object
                {
                    ObjectId = 109,
                    Flags00 = 0x01,
                },
                [110] = new TR1Object
                {
                    ObjectId = 110,
                    Flags00 = 0x20,
                },
                [111] = new TR1Object
                {
                    ObjectId = 111,
                    Flags00 = 0x20,
                },
                [112] = new TR1Object
                {
                    ObjectId = 112,
                    Flags00 = 0x20,
                },
                [113] = new TR1Object
                {
                    ObjectId = 113,
                    Flags00 = 0x20,
                },
                [114] = new TR1Object
                {
                    ObjectId = 114,
                    Flags00 = 0x00,
                },
                [115] = new TR1Object
                {
                    ObjectId = 115,
                    Flags00 = 0x00,
                },
                [116] = new TR1Object
                {
                    ObjectId = 116,
                    Flags00 = 0x00,
                },
                [117] = new TR1Object
                {
                    ObjectId = 117,
                    Flags00 = 0x00,
                },
                [118] = new TR1Object
                {
                    ObjectId = 118,
                    Flags00 = 0x21,
                },
                [119] = new TR1Object
                {
                    ObjectId = 119,
                    Flags00 = 0x21,
                },
                [120] = new TR1Object
                {
                    ObjectId = 120,
                    Flags00 = 0x21,
                },
                [121] = new TR1Object
                {
                    ObjectId = 121,
                    Flags00 = 0x21,
                },
                [122] = new TR1Object
                {
                    ObjectId = 122,
                    Flags00 = 0x20,
                },
                [123] = new TR1Object
                {
                    ObjectId = 123,
                    Flags00 = 0x20,
                },
                [124] = new TR1Object
                {
                    ObjectId = 124,
                    Flags00 = 0x20,
                },
                [125] = new TR1Object
                {
                    ObjectId = 125,
                    Flags00 = 0x20,
                },
                [126] = new TR1Object
                {
                    ObjectId = 126,
                    Flags00 = 0x20,
                },
                [127] = new TR1Object
                {
                    ObjectId = 127,
                    Flags00 = 0x00,
                },
                [128] = new TR1Object
                {
                    ObjectId = 128,
                    Flags00 = 0x00,
                },
                [129] = new TR1Object
                {
                    ObjectId = 129,
                    Flags00 = 0x21,
                },
                [130] = new TR1Object
                {
                    ObjectId = 130,
                    Flags00 = 0x20,
                },
                [131] = new TR1Object
                {
                    ObjectId = 131,
                    Flags00 = 0x20,
                },
                [132] = new TR1Object
                {
                    ObjectId = 132,
                    Flags00 = 0x20,
                },
                [133] = new TR1Object
                {
                    ObjectId = 133,
                    Flags00 = 0x01,
                },
                [134] = new TR1Object
                {
                    ObjectId = 134,
                    Flags00 = 0x00,
                },
                [135] = new TR1Object
                {
                    ObjectId = 135,
                    Flags00 = 0x00,
                },
                [136] = new TR1Object
                {
                    ObjectId = 136,
                    Flags00 = 0x00,
                },
                [137] = new TR1Object
                {
                    ObjectId = 137,
                    Flags00 = 0x21,
                },
                [138] = new TR1Object
                {
                    ObjectId = 138,
                    Flags00 = 0x20,
                },
                [139] = new TR1Object
                {
                    ObjectId = 139,
                    Flags00 = 0x20,
                },
                [140] = new TR1Object
                {
                    ObjectId = 140,
                    Flags00 = 0x20,
                },
                [141] = new TR1Object
                {
                    ObjectId = 141,
                    Flags00 = 0x20,
                },
                [142] = new TR1Object
                {
                    ObjectId = 142,
                    Flags00 = 0x20,
                },
                [143] = new TR1Object
                {
                    ObjectId = 143,
                    Flags00 = 0x20,
                },
                [144] = new TR1Object
                {
                    ObjectId = 144,
                    Flags00 = 0x20,
                },
                [145] = new TR1Object
                {
                    ObjectId = 145,
                    Flags00 = 0x20,
                },
                [146] = new TR1Object
                {
                    ObjectId = 146,
                    Flags00 = 0x20,
                },
                [147] = new TR1Object
                {
                    ObjectId = 147,
                    Flags00 = 0x60,
                },
                [148] = new TR1Object
                {
                    ObjectId = 148,
                    Flags00 = 0x00,
                },
                [149] = new TR1Object
                {
                    ObjectId = 149,
                    Flags00 = 0x00,
                },
                [150] = new TR1Object
                {
                    ObjectId = 150,
                    Flags00 = 0x01,
                },
                [151] = new TR1Object
                {
                    ObjectId = 151,
                    Flags00 = 0x00,
                },
                [152] = new TR1Object
                {
                    ObjectId = 152,
                    Flags00 = 0x00,
                },
                [153] = new TR1Object
                {
                    ObjectId = 153,
                    Flags00 = 0x01,
                },
                [154] = new TR1Object
                {
                    ObjectId = 154,
                    Flags00 = 0x00,
                },
                [155] = new TR1Object
                {
                    ObjectId = 155,
                    Flags00 = 0x01,
                },
                [156] = new TR1Object
                {
                    ObjectId = 156,
                    Flags00 = 0x01,
                },
                [157] = new TR1Object
                {
                    ObjectId = 157,
                    Flags00 = 0x00,
                },
                [158] = new TR1Object
                {
                    ObjectId = 158,
                    Flags00 = 0x01,
                },
                [159] = new TR1Object
                {
                    ObjectId = 159,
                    Flags00 = 0x00,
                },
                [160] = new TR1Object
                {
                    ObjectId = 160,
                    Flags00 = 0x00,
                },
                [161] = new TR1Object
                {
                    ObjectId = 161,
                    Flags00 = 0x00,
                },
                [162] = new TR1Object
                {
                    ObjectId = 162,
                    Flags00 = 0x60,
                },
                [163] = new TR1Object
                {
                    ObjectId = 163,
                    Flags00 = 0x00,
                },
                [164] = new TR1Object
                {
                    ObjectId = 164,
                    Flags00 = 0x01,
                },
                [165] = new TR1Object
                {
                    ObjectId = 165,
                    Flags00 = 0x00,
                },
                [166] = new TR1Object
                {
                    ObjectId = 166,
                    Flags00 = 0x01,
                },
                [167] = new TR1Object
                {
                    ObjectId = 167,
                    Flags00 = 0x00,
                },
                [168] = new TR1Object
                {
                    ObjectId = 168,
                    Flags00 = 0x01,
                },
                [169] = new TR1Object
                {
                    ObjectId = 169,
                    Flags00 = 0x01,
                },
                [170] = new TR1Object
                {
                    ObjectId = 170,
                    Flags00 = 0x20,
                },
            },
            [18] = new Dictionary<int, TR1Object> // Atlantean Stronghold
            {
                [0] = new TR1Object
                {
                    ObjectId = 0,
                    Flags00 = 0x79,
                },
                [1] = new TR1Object
                {
                    ObjectId = 1,
                    Flags00 = 0x01,
                },
                [2] = new TR1Object
                {
                    ObjectId = 2,
                    Flags00 = 0x01,
                },
                [3] = new TR1Object
                {
                    ObjectId = 3,
                    Flags00 = 0x01,
                },
                [4] = new TR1Object
                {
                    ObjectId = 4,
                    Flags00 = 0x01,
                },
                [5] = new TR1Object
                {
                    ObjectId = 5,
                    Flags00 = 0x01,
                },
                [6] = new TR1Object
                {
                    ObjectId = 6,
                    Flags00 = 0x79,
                },
                [7] = new TR1Object
                {
                    ObjectId = 7,
                    Flags00 = 0x7B,
                },
                [8] = new TR1Object
                {
                    ObjectId = 8,
                    Flags00 = 0x7B,
                },
                [9] = new TR1Object
                {
                    ObjectId = 9,
                    Flags00 = 0x7B,
                },
                [10] = new TR1Object
                {
                    ObjectId = 10,
                    Flags00 = 0x7B,
                },
                [11] = new TR1Object
                {
                    ObjectId = 11,
                    Flags00 = 0x7B,
                },
                [12] = new TR1Object
                {
                    ObjectId = 12,
                    Flags00 = 0x7B,
                },
                [13] = new TR1Object
                {
                    ObjectId = 13,
                    Flags00 = 0x7B,
                },
                [14] = new TR1Object
                {
                    ObjectId = 14,
                    Flags00 = 0x7B,
                },
                [15] = new TR1Object
                {
                    ObjectId = 15,
                    Flags00 = 0x7B,
                },
                [16] = new TR1Object
                {
                    ObjectId = 16,
                    Flags00 = 0x7B,
                },
                [17] = new TR1Object
                {
                    ObjectId = 17,
                    Flags00 = 0x7B,
                },
                [18] = new TR1Object
                {
                    ObjectId = 18,
                    Flags00 = 0x7B,
                },
                [19] = new TR1Object
                {
                    ObjectId = 19,
                    Flags00 = 0x7B,
                },
                [20] = new TR1Object
                {
                    ObjectId = 20,
                    Flags00 = 0x7B,
                },
                [21] = new TR1Object
                {
                    ObjectId = 21,
                    Flags00 = 0x7B,
                },
                [22] = new TR1Object
                {
                    ObjectId = 22,
                    Flags00 = 0x7B,
                },
                [23] = new TR1Object
                {
                    ObjectId = 23,
                    Flags00 = 0x7B,
                },
                [24] = new TR1Object
                {
                    ObjectId = 24,
                    Flags00 = 0x71,
                },
                [25] = new TR1Object
                {
                    ObjectId = 25,
                    Flags00 = 0x00,
                },
                [26] = new TR1Object
                {
                    ObjectId = 26,
                    Flags00 = 0x00,
                },
                [27] = new TR1Object
                {
                    ObjectId = 27,
                    Flags00 = 0x00,
                },
                [28] = new TR1Object
                {
                    ObjectId = 28,
                    Flags00 = 0x00,
                },
                [29] = new TR1Object
                {
                    ObjectId = 29,
                    Flags00 = 0x00,
                },
                [30] = new TR1Object
                {
                    ObjectId = 30,
                    Flags00 = 0x00,
                },
                [31] = new TR1Object
                {
                    ObjectId = 31,
                    Flags00 = 0x00,
                },
                [32] = new TR1Object
                {
                    ObjectId = 32,
                    Flags00 = 0x00,
                },
                [33] = new TR1Object
                {
                    ObjectId = 33,
                    Flags00 = 0x00,
                },
                [34] = new TR1Object
                {
                    ObjectId = 34,
                    Flags00 = 0x00,
                },
                [35] = new TR1Object
                {
                    ObjectId = 35,
                    Flags00 = 0x68,
                },
                [36] = new TR1Object
                {
                    ObjectId = 36,
                    Flags00 = 0x60,
                },
                [37] = new TR1Object
                {
                    ObjectId = 37,
                    Flags00 = 0x01,
                },
                [38] = new TR1Object
                {
                    ObjectId = 38,
                    Flags00 = 0x69,
                },
                [39] = new TR1Object
                {
                    ObjectId = 39,
                    Flags00 = 0x21,
                },
                [40] = new TR1Object
                {
                    ObjectId = 40,
                    Flags00 = 0x21,
                },
                [41] = new TR1Object
                {
                    ObjectId = 41,
                    Flags00 = 0x00,
                },
                [42] = new TR1Object
                {
                    ObjectId = 42,
                    Flags00 = 0x61,
                },
                [43] = new TR1Object
                {
                    ObjectId = 43,
                    Flags00 = 0x68,
                },
                [44] = new TR1Object
                {
                    ObjectId = 44,
                    Flags00 = 0x60,
                },
                [45] = new TR1Object
                {
                    ObjectId = 45,
                    Flags00 = 0x60,
                },
                [46] = new TR1Object
                {
                    ObjectId = 46,
                    Flags00 = 0x20,
                },
                [47] = new TR1Object
                {
                    ObjectId = 47,
                    Flags00 = 0x68,
                },
                [48] = new TR1Object
                {
                    ObjectId = 48,
                    Flags00 = 0x69,
                },
                [49] = new TR1Object
                {
                    ObjectId = 49,
                    Flags00 = 0x68,
                },
                [50] = new TR1Object
                {
                    ObjectId = 50,
                    Flags00 = 0x68,
                },
                [51] = new TR1Object
                {
                    ObjectId = 51,
                    Flags00 = 0x68,
                },
                [52] = new TR1Object
                {
                    ObjectId = 52,
                    Flags00 = 0x68,
                },
                [53] = new TR1Object
                {
                    ObjectId = 53,
                    Flags00 = 0x68,
                },
                [54] = new TR1Object
                {
                    ObjectId = 54,
                    Flags00 = 0x68,
                },
                [55] = new TR1Object
                {
                    ObjectId = 55,
                    Flags00 = 0x61,
                },
                [56] = new TR1Object
                {
                    ObjectId = 56,
                    Flags00 = 0x61,
                },
                [57] = new TR1Object
                {
                    ObjectId = 57,
                    Flags00 = 0x60,
                },
                [58] = new TR1Object
                {
                    ObjectId = 58,
                    Flags00 = 0x60,
                },
                [59] = new TR1Object
                {
                    ObjectId = 59,
                    Flags00 = 0x61,
                },
                [60] = new TR1Object
                {
                    ObjectId = 60,
                    Flags00 = 0x61,
                },
                [61] = new TR1Object
                {
                    ObjectId = 61,
                    Flags00 = 0x61,
                },
                [62] = new TR1Object
                {
                    ObjectId = 62,
                    Flags00 = 0x61,
                },
                [63] = new TR1Object
                {
                    ObjectId = 63,
                    Flags00 = 0x61,
                },
                [64] = new TR1Object
                {
                    ObjectId = 64,
                    Flags00 = 0x61,
                },
                [65] = new TR1Object
                {
                    ObjectId = 65,
                    Flags00 = 0x61,
                },
                [66] = new TR1Object
                {
                    ObjectId = 66,
                    Flags00 = 0x61,
                },
                [67] = new TR1Object
                {
                    ObjectId = 67,
                    Flags00 = 0x00,
                },
                [68] = new TR1Object
                {
                    ObjectId = 68,
                    Flags00 = 0x00,
                },
                [69] = new TR1Object
                {
                    ObjectId = 69,
                    Flags00 = 0x00,
                },
                [70] = new TR1Object
                {
                    ObjectId = 70,
                    Flags00 = 0x00,
                },
                [71] = new TR1Object
                {
                    ObjectId = 71,
                    Flags00 = 0x01,
                },
                [72] = new TR1Object
                {
                    ObjectId = 72,
                    Flags00 = 0x01,
                },
                [73] = new TR1Object
                {
                    ObjectId = 73,
                    Flags00 = 0x00,
                },
                [74] = new TR1Object
                {
                    ObjectId = 74,
                    Flags00 = 0x20,
                },
                [75] = new TR1Object
                {
                    ObjectId = 75,
                    Flags00 = 0x20,
                },
                [76] = new TR1Object
                {
                    ObjectId = 76,
                    Flags00 = 0x20,
                },
                [77] = new TR1Object
                {
                    ObjectId = 77,
                    Flags00 = 0x00,
                },
                [78] = new TR1Object
                {
                    ObjectId = 78,
                    Flags00 = 0x00,
                },
                [79] = new TR1Object
                {
                    ObjectId = 79,
                    Flags00 = 0x00,
                },
                [80] = new TR1Object
                {
                    ObjectId = 80,
                    Flags00 = 0x00,
                },
                [81] = new TR1Object
                {
                    ObjectId = 81,
                    Flags00 = 0x01,
                },
                [82] = new TR1Object
                {
                    ObjectId = 82,
                    Flags00 = 0x01,
                },
                [83] = new TR1Object
                {
                    ObjectId = 83,
                    Flags00 = 0x21,
                },
                [84] = new TR1Object
                {
                    ObjectId = 84,
                    Flags00 = 0x21,
                },
                [85] = new TR1Object
                {
                    ObjectId = 85,
                    Flags00 = 0x21,
                },
                [86] = new TR1Object
                {
                    ObjectId = 86,
                    Flags00 = 0x21,
                },
                [87] = new TR1Object
                {
                    ObjectId = 87,
                    Flags00 = 0x21,
                },
                [88] = new TR1Object
                {
                    ObjectId = 88,
                    Flags00 = 0x20,
                },
                [89] = new TR1Object
                {
                    ObjectId = 89,
                    Flags00 = 0x21,
                },
                [90] = new TR1Object
                {
                    ObjectId = 90,
                    Flags00 = 0x21,
                },
                [91] = new TR1Object
                {
                    ObjectId = 91,
                    Flags00 = 0x21,
                },
                [92] = new TR1Object
                {
                    ObjectId = 92,
                    Flags00 = 0x20,
                },
                [93] = new TR1Object
                {
                    ObjectId = 93,
                    Flags00 = 0x21,
                },
                [94] = new TR1Object
                {
                    ObjectId = 94,
                    Flags00 = 0x21,
                },
                [95] = new TR1Object
                {
                    ObjectId = 95,
                    Flags00 = 0x01,
                },
                [96] = new TR1Object
                {
                    ObjectId = 96,
                    Flags00 = 0x01,
                },
                [97] = new TR1Object
                {
                    ObjectId = 97,
                    Flags00 = 0x01,
                },
                [98] = new TR1Object
                {
                    ObjectId = 98,
                    Flags00 = 0x00,
                },
                [99] = new TR1Object
                {
                    ObjectId = 99,
                    Flags00 = 0x01,
                },
                [100] = new TR1Object
                {
                    ObjectId = 100,
                    Flags00 = 0x01,
                },
                [101] = new TR1Object
                {
                    ObjectId = 101,
                    Flags00 = 0x01,
                },
                [102] = new TR1Object
                {
                    ObjectId = 102,
                    Flags00 = 0x01,
                },
                [103] = new TR1Object
                {
                    ObjectId = 103,
                    Flags00 = 0x01,
                },
                [104] = new TR1Object
                {
                    ObjectId = 104,
                    Flags00 = 0x01,
                },
                [105] = new TR1Object
                {
                    ObjectId = 105,
                    Flags00 = 0x01,
                },
                [106] = new TR1Object
                {
                    ObjectId = 106,
                    Flags00 = 0x01,
                },
                [107] = new TR1Object
                {
                    ObjectId = 107,
                    Flags00 = 0x00,
                },
                [108] = new TR1Object
                {
                    ObjectId = 108,
                    Flags00 = 0x01,
                },
                [109] = new TR1Object
                {
                    ObjectId = 109,
                    Flags00 = 0x01,
                },
                [110] = new TR1Object
                {
                    ObjectId = 110,
                    Flags00 = 0x20,
                },
                [111] = new TR1Object
                {
                    ObjectId = 111,
                    Flags00 = 0x20,
                },
                [112] = new TR1Object
                {
                    ObjectId = 112,
                    Flags00 = 0x20,
                },
                [113] = new TR1Object
                {
                    ObjectId = 113,
                    Flags00 = 0x20,
                },
                [114] = new TR1Object
                {
                    ObjectId = 114,
                    Flags00 = 0x00,
                },
                [115] = new TR1Object
                {
                    ObjectId = 115,
                    Flags00 = 0x00,
                },
                [116] = new TR1Object
                {
                    ObjectId = 116,
                    Flags00 = 0x00,
                },
                [117] = new TR1Object
                {
                    ObjectId = 117,
                    Flags00 = 0x00,
                },
                [118] = new TR1Object
                {
                    ObjectId = 118,
                    Flags00 = 0x20,
                },
                [119] = new TR1Object
                {
                    ObjectId = 119,
                    Flags00 = 0x20,
                },
                [120] = new TR1Object
                {
                    ObjectId = 120,
                    Flags00 = 0x20,
                },
                [121] = new TR1Object
                {
                    ObjectId = 121,
                    Flags00 = 0x20,
                },
                [122] = new TR1Object
                {
                    ObjectId = 122,
                    Flags00 = 0x20,
                },
                [123] = new TR1Object
                {
                    ObjectId = 123,
                    Flags00 = 0x20,
                },
                [124] = new TR1Object
                {
                    ObjectId = 124,
                    Flags00 = 0x20,
                },
                [125] = new TR1Object
                {
                    ObjectId = 125,
                    Flags00 = 0x20,
                },
                [126] = new TR1Object
                {
                    ObjectId = 126,
                    Flags00 = 0x20,
                },
                [127] = new TR1Object
                {
                    ObjectId = 127,
                    Flags00 = 0x00,
                },
                [128] = new TR1Object
                {
                    ObjectId = 128,
                    Flags00 = 0x00,
                },
                [129] = new TR1Object
                {
                    ObjectId = 129,
                    Flags00 = 0x20,
                },
                [130] = new TR1Object
                {
                    ObjectId = 130,
                    Flags00 = 0x20,
                },
                [131] = new TR1Object
                {
                    ObjectId = 131,
                    Flags00 = 0x20,
                },
                [132] = new TR1Object
                {
                    ObjectId = 132,
                    Flags00 = 0x20,
                },
                [133] = new TR1Object
                {
                    ObjectId = 133,
                    Flags00 = 0x00,
                },
                [134] = new TR1Object
                {
                    ObjectId = 134,
                    Flags00 = 0x00,
                },
                [135] = new TR1Object
                {
                    ObjectId = 135,
                    Flags00 = 0x00,
                },
                [136] = new TR1Object
                {
                    ObjectId = 136,
                    Flags00 = 0x00,
                },
                [137] = new TR1Object
                {
                    ObjectId = 137,
                    Flags00 = 0x20,
                },
                [138] = new TR1Object
                {
                    ObjectId = 138,
                    Flags00 = 0x20,
                },
                [139] = new TR1Object
                {
                    ObjectId = 139,
                    Flags00 = 0x20,
                },
                [140] = new TR1Object
                {
                    ObjectId = 140,
                    Flags00 = 0x20,
                },
                [141] = new TR1Object
                {
                    ObjectId = 141,
                    Flags00 = 0x20,
                },
                [142] = new TR1Object
                {
                    ObjectId = 142,
                    Flags00 = 0x20,
                },
                [143] = new TR1Object
                {
                    ObjectId = 143,
                    Flags00 = 0x20,
                },
                [144] = new TR1Object
                {
                    ObjectId = 144,
                    Flags00 = 0x20,
                },
                [145] = new TR1Object
                {
                    ObjectId = 145,
                    Flags00 = 0x20,
                },
                [146] = new TR1Object
                {
                    ObjectId = 146,
                    Flags00 = 0x21,
                },
                [147] = new TR1Object
                {
                    ObjectId = 147,
                    Flags00 = 0x61,
                },
                [148] = new TR1Object
                {
                    ObjectId = 148,
                    Flags00 = 0x00,
                },
                [149] = new TR1Object
                {
                    ObjectId = 149,
                    Flags00 = 0x00,
                },
                [150] = new TR1Object
                {
                    ObjectId = 150,
                    Flags00 = 0x01,
                },
                [151] = new TR1Object
                {
                    ObjectId = 151,
                    Flags00 = 0x01,
                },
                [152] = new TR1Object
                {
                    ObjectId = 152,
                    Flags00 = 0x00,
                },
                [153] = new TR1Object
                {
                    ObjectId = 153,
                    Flags00 = 0x01,
                },
                [154] = new TR1Object
                {
                    ObjectId = 154,
                    Flags00 = 0x00,
                },
                [155] = new TR1Object
                {
                    ObjectId = 155,
                    Flags00 = 0x01,
                },
                [156] = new TR1Object
                {
                    ObjectId = 156,
                    Flags00 = 0x01,
                },
                [157] = new TR1Object
                {
                    ObjectId = 157,
                    Flags00 = 0x00,
                },
                [158] = new TR1Object
                {
                    ObjectId = 158,
                    Flags00 = 0x01,
                },
                [159] = new TR1Object
                {
                    ObjectId = 159,
                    Flags00 = 0x00,
                },
                [160] = new TR1Object
                {
                    ObjectId = 160,
                    Flags00 = 0x01,
                },
                [161] = new TR1Object
                {
                    ObjectId = 161,
                    Flags00 = 0x00,
                },
                [162] = new TR1Object
                {
                    ObjectId = 162,
                    Flags00 = 0x60,
                },
                [163] = new TR1Object
                {
                    ObjectId = 163,
                    Flags00 = 0x61,
                },
                [164] = new TR1Object
                {
                    ObjectId = 164,
                    Flags00 = 0x01,
                },
                [165] = new TR1Object
                {
                    ObjectId = 165,
                    Flags00 = 0x00,
                },
                [166] = new TR1Object
                {
                    ObjectId = 166,
                    Flags00 = 0x01,
                },
                [167] = new TR1Object
                {
                    ObjectId = 167,
                    Flags00 = 0x00,
                },
                [168] = new TR1Object
                {
                    ObjectId = 168,
                    Flags00 = 0x01,
                },
                [169] = new TR1Object
                {
                    ObjectId = 169,
                    Flags00 = 0x01,
                },
                [170] = new TR1Object
                {
                    ObjectId = 170,
                    Flags00 = 0x20,
                },
                [171] = new TR1Object
                {
                    ObjectId = 171,
                    Flags00 = 0x00,
                },
                [172] = new TR1Object
                {
                    ObjectId = 172,
                    Flags00 = 0x01,
                },
                [173] = new TR1Object
                {
                    ObjectId = 173,
                    Flags00 = 0x01,
                },
                [174] = new TR1Object
                {
                    ObjectId = 174,
                    Flags00 = 0x00,
                },
                [175] = new TR1Object
                {
                    ObjectId = 175,
                    Flags00 = 0x00,
                },
                [176] = new TR1Object
                {
                    ObjectId = 176,
                    Flags00 = 0x01,
                },
                [177] = new TR1Object
                {
                    ObjectId = 177,
                    Flags00 = 0x21,
                },
                [178] = new TR1Object
                {
                    ObjectId = 178,
                    Flags00 = 0x01,
                },
                [179] = new TR1Object
                {
                    ObjectId = 179,
                    Flags00 = 0x20,
                },
                [180] = new TR1Object
                {
                    ObjectId = 180,
                    Flags00 = 0x69,
                },
                [181] = new TR1Object
                {
                    ObjectId = 181,
                    Flags00 = 0x61,
                },
                [182] = new TR1Object
                {
                    ObjectId = 182,
                    Flags00 = 0x68,
                },
            },
            [19] = new Dictionary<int, TR1Object> // The Hive
            {
                [0] = new TR1Object
                {
                    ObjectId = 0,
                    Flags00 = 0x79,
                },
                [1] = new TR1Object
                {
                    ObjectId = 1,
                    Flags00 = 0x01,
                },
                [2] = new TR1Object
                {
                    ObjectId = 2,
                    Flags00 = 0x01,
                },
                [3] = new TR1Object
                {
                    ObjectId = 3,
                    Flags00 = 0x01,
                },
                [4] = new TR1Object
                {
                    ObjectId = 4,
                    Flags00 = 0x01,
                },
                [5] = new TR1Object
                {
                    ObjectId = 5,
                    Flags00 = 0x01,
                },
                [6] = new TR1Object
                {
                    ObjectId = 6,
                    Flags00 = 0x79,
                },
                [7] = new TR1Object
                {
                    ObjectId = 7,
                    Flags00 = 0x7B,
                },
                [8] = new TR1Object
                {
                    ObjectId = 8,
                    Flags00 = 0x7B,
                },
                [9] = new TR1Object
                {
                    ObjectId = 9,
                    Flags00 = 0x7B,
                },
                [10] = new TR1Object
                {
                    ObjectId = 10,
                    Flags00 = 0x7B,
                },
                [11] = new TR1Object
                {
                    ObjectId = 11,
                    Flags00 = 0x7B,
                },
                [12] = new TR1Object
                {
                    ObjectId = 12,
                    Flags00 = 0x7B,
                },
                [13] = new TR1Object
                {
                    ObjectId = 13,
                    Flags00 = 0x7B,
                },
                [14] = new TR1Object
                {
                    ObjectId = 14,
                    Flags00 = 0x7B,
                },
                [15] = new TR1Object
                {
                    ObjectId = 15,
                    Flags00 = 0x7B,
                },
                [16] = new TR1Object
                {
                    ObjectId = 16,
                    Flags00 = 0x7B,
                },
                [17] = new TR1Object
                {
                    ObjectId = 17,
                    Flags00 = 0x7B,
                },
                [18] = new TR1Object
                {
                    ObjectId = 18,
                    Flags00 = 0x7B,
                },
                [19] = new TR1Object
                {
                    ObjectId = 19,
                    Flags00 = 0x7B,
                },
                [20] = new TR1Object
                {
                    ObjectId = 20,
                    Flags00 = 0x7B,
                },
                [21] = new TR1Object
                {
                    ObjectId = 21,
                    Flags00 = 0x7B,
                },
                [22] = new TR1Object
                {
                    ObjectId = 22,
                    Flags00 = 0x7B,
                },
                [23] = new TR1Object
                {
                    ObjectId = 23,
                    Flags00 = 0x7B,
                },
                [24] = new TR1Object
                {
                    ObjectId = 24,
                    Flags00 = 0x71,
                },
                [25] = new TR1Object
                {
                    ObjectId = 25,
                    Flags00 = 0x00,
                },
                [26] = new TR1Object
                {
                    ObjectId = 26,
                    Flags00 = 0x00,
                },
                [27] = new TR1Object
                {
                    ObjectId = 27,
                    Flags00 = 0x00,
                },
                [28] = new TR1Object
                {
                    ObjectId = 28,
                    Flags00 = 0x00,
                },
                [29] = new TR1Object
                {
                    ObjectId = 29,
                    Flags00 = 0x00,
                },
                [30] = new TR1Object
                {
                    ObjectId = 30,
                    Flags00 = 0x00,
                },
                [31] = new TR1Object
                {
                    ObjectId = 31,
                    Flags00 = 0x00,
                },
                [32] = new TR1Object
                {
                    ObjectId = 32,
                    Flags00 = 0x00,
                },
                [33] = new TR1Object
                {
                    ObjectId = 33,
                    Flags00 = 0x00,
                },
                [34] = new TR1Object
                {
                    ObjectId = 34,
                    Flags00 = 0x00,
                },
                [35] = new TR1Object
                {
                    ObjectId = 35,
                    Flags00 = 0x68,
                },
                [36] = new TR1Object
                {
                    ObjectId = 36,
                    Flags00 = 0x60,
                },
                [37] = new TR1Object
                {
                    ObjectId = 37,
                    Flags00 = 0x01,
                },
                [38] = new TR1Object
                {
                    ObjectId = 38,
                    Flags00 = 0x69,
                },
                [39] = new TR1Object
                {
                    ObjectId = 39,
                    Flags00 = 0x21,
                },
                [40] = new TR1Object
                {
                    ObjectId = 40,
                    Flags00 = 0x21,
                },
                [41] = new TR1Object
                {
                    ObjectId = 41,
                    Flags00 = 0x00,
                },
                [42] = new TR1Object
                {
                    ObjectId = 42,
                    Flags00 = 0x61,
                },
                [43] = new TR1Object
                {
                    ObjectId = 43,
                    Flags00 = 0x68,
                },
                [44] = new TR1Object
                {
                    ObjectId = 44,
                    Flags00 = 0x60,
                },
                [45] = new TR1Object
                {
                    ObjectId = 45,
                    Flags00 = 0x60,
                },
                [46] = new TR1Object
                {
                    ObjectId = 46,
                    Flags00 = 0x20,
                },
                [47] = new TR1Object
                {
                    ObjectId = 47,
                    Flags00 = 0x68,
                },
                [48] = new TR1Object
                {
                    ObjectId = 48,
                    Flags00 = 0x69,
                },
                [49] = new TR1Object
                {
                    ObjectId = 49,
                    Flags00 = 0x68,
                },
                [50] = new TR1Object
                {
                    ObjectId = 50,
                    Flags00 = 0x68,
                },
                [51] = new TR1Object
                {
                    ObjectId = 51,
                    Flags00 = 0x68,
                },
                [52] = new TR1Object
                {
                    ObjectId = 52,
                    Flags00 = 0x68,
                },
                [53] = new TR1Object
                {
                    ObjectId = 53,
                    Flags00 = 0x68,
                },
                [54] = new TR1Object
                {
                    ObjectId = 54,
                    Flags00 = 0x68,
                },
                [55] = new TR1Object
                {
                    ObjectId = 55,
                    Flags00 = 0x61,
                },
                [56] = new TR1Object
                {
                    ObjectId = 56,
                    Flags00 = 0x61,
                },
                [57] = new TR1Object
                {
                    ObjectId = 57,
                    Flags00 = 0x60,
                },
                [58] = new TR1Object
                {
                    ObjectId = 58,
                    Flags00 = 0x60,
                },
                [59] = new TR1Object
                {
                    ObjectId = 59,
                    Flags00 = 0x61,
                },
                [60] = new TR1Object
                {
                    ObjectId = 60,
                    Flags00 = 0x61,
                },
                [61] = new TR1Object
                {
                    ObjectId = 61,
                    Flags00 = 0x61,
                },
                [62] = new TR1Object
                {
                    ObjectId = 62,
                    Flags00 = 0x61,
                },
                [63] = new TR1Object
                {
                    ObjectId = 63,
                    Flags00 = 0x61,
                },
                [64] = new TR1Object
                {
                    ObjectId = 64,
                    Flags00 = 0x61,
                },
                [65] = new TR1Object
                {
                    ObjectId = 65,
                    Flags00 = 0x61,
                },
                [66] = new TR1Object
                {
                    ObjectId = 66,
                    Flags00 = 0x61,
                },
                [67] = new TR1Object
                {
                    ObjectId = 67,
                    Flags00 = 0x00,
                },
                [68] = new TR1Object
                {
                    ObjectId = 68,
                    Flags00 = 0x00,
                },
                [69] = new TR1Object
                {
                    ObjectId = 69,
                    Flags00 = 0x00,
                },
                [70] = new TR1Object
                {
                    ObjectId = 70,
                    Flags00 = 0x00,
                },
                [71] = new TR1Object
                {
                    ObjectId = 71,
                    Flags00 = 0x01,
                },
                [72] = new TR1Object
                {
                    ObjectId = 72,
                    Flags00 = 0x01,
                },
                [73] = new TR1Object
                {
                    ObjectId = 73,
                    Flags00 = 0x00,
                },
                [74] = new TR1Object
                {
                    ObjectId = 74,
                    Flags00 = 0x20,
                },
                [75] = new TR1Object
                {
                    ObjectId = 75,
                    Flags00 = 0x20,
                },
                [76] = new TR1Object
                {
                    ObjectId = 76,
                    Flags00 = 0x20,
                },
                [77] = new TR1Object
                {
                    ObjectId = 77,
                    Flags00 = 0x00,
                },
                [78] = new TR1Object
                {
                    ObjectId = 78,
                    Flags00 = 0x00,
                },
                [79] = new TR1Object
                {
                    ObjectId = 79,
                    Flags00 = 0x00,
                },
                [80] = new TR1Object
                {
                    ObjectId = 80,
                    Flags00 = 0x00,
                },
                [81] = new TR1Object
                {
                    ObjectId = 81,
                    Flags00 = 0x01,
                },
                [82] = new TR1Object
                {
                    ObjectId = 82,
                    Flags00 = 0x01,
                },
                [83] = new TR1Object
                {
                    ObjectId = 83,
                    Flags00 = 0x21,
                },
                [84] = new TR1Object
                {
                    ObjectId = 84,
                    Flags00 = 0x21,
                },
                [85] = new TR1Object
                {
                    ObjectId = 85,
                    Flags00 = 0x21,
                },
                [86] = new TR1Object
                {
                    ObjectId = 86,
                    Flags00 = 0x21,
                },
                [87] = new TR1Object
                {
                    ObjectId = 87,
                    Flags00 = 0x21,
                },
                [88] = new TR1Object
                {
                    ObjectId = 88,
                    Flags00 = 0x20,
                },
                [89] = new TR1Object
                {
                    ObjectId = 89,
                    Flags00 = 0x21,
                },
                [90] = new TR1Object
                {
                    ObjectId = 90,
                    Flags00 = 0x21,
                },
                [91] = new TR1Object
                {
                    ObjectId = 91,
                    Flags00 = 0x21,
                },
                [92] = new TR1Object
                {
                    ObjectId = 92,
                    Flags00 = 0x20,
                },
                [93] = new TR1Object
                {
                    ObjectId = 93,
                    Flags00 = 0x21,
                },
                [94] = new TR1Object
                {
                    ObjectId = 94,
                    Flags00 = 0x21,
                },
                [95] = new TR1Object
                {
                    ObjectId = 95,
                    Flags00 = 0x01,
                },
                [96] = new TR1Object
                {
                    ObjectId = 96,
                    Flags00 = 0x01,
                },
                [97] = new TR1Object
                {
                    ObjectId = 97,
                    Flags00 = 0x01,
                },
                [98] = new TR1Object
                {
                    ObjectId = 98,
                    Flags00 = 0x00,
                },
                [99] = new TR1Object
                {
                    ObjectId = 99,
                    Flags00 = 0x01,
                },
                [100] = new TR1Object
                {
                    ObjectId = 100,
                    Flags00 = 0x01,
                },
                [101] = new TR1Object
                {
                    ObjectId = 101,
                    Flags00 = 0x01,
                },
                [102] = new TR1Object
                {
                    ObjectId = 102,
                    Flags00 = 0x01,
                },
                [103] = new TR1Object
                {
                    ObjectId = 103,
                    Flags00 = 0x01,
                },
                [104] = new TR1Object
                {
                    ObjectId = 104,
                    Flags00 = 0x01,
                },
                [105] = new TR1Object
                {
                    ObjectId = 105,
                    Flags00 = 0x01,
                },
                [106] = new TR1Object
                {
                    ObjectId = 106,
                    Flags00 = 0x01,
                },
                [107] = new TR1Object
                {
                    ObjectId = 107,
                    Flags00 = 0x00,
                },
                [108] = new TR1Object
                {
                    ObjectId = 108,
                    Flags00 = 0x01,
                },
                [109] = new TR1Object
                {
                    ObjectId = 109,
                    Flags00 = 0x01,
                },
                [110] = new TR1Object
                {
                    ObjectId = 110,
                    Flags00 = 0x20,
                },
                [111] = new TR1Object
                {
                    ObjectId = 111,
                    Flags00 = 0x20,
                },
                [112] = new TR1Object
                {
                    ObjectId = 112,
                    Flags00 = 0x20,
                },
                [113] = new TR1Object
                {
                    ObjectId = 113,
                    Flags00 = 0x20,
                },
                [114] = new TR1Object
                {
                    ObjectId = 114,
                    Flags00 = 0x00,
                },
                [115] = new TR1Object
                {
                    ObjectId = 115,
                    Flags00 = 0x00,
                },
                [116] = new TR1Object
                {
                    ObjectId = 116,
                    Flags00 = 0x00,
                },
                [117] = new TR1Object
                {
                    ObjectId = 117,
                    Flags00 = 0x00,
                },
                [118] = new TR1Object
                {
                    ObjectId = 118,
                    Flags00 = 0x20,
                },
                [119] = new TR1Object
                {
                    ObjectId = 119,
                    Flags00 = 0x20,
                },
                [120] = new TR1Object
                {
                    ObjectId = 120,
                    Flags00 = 0x20,
                },
                [121] = new TR1Object
                {
                    ObjectId = 121,
                    Flags00 = 0x20,
                },
                [122] = new TR1Object
                {
                    ObjectId = 122,
                    Flags00 = 0x20,
                },
                [123] = new TR1Object
                {
                    ObjectId = 123,
                    Flags00 = 0x20,
                },
                [124] = new TR1Object
                {
                    ObjectId = 124,
                    Flags00 = 0x20,
                },
                [125] = new TR1Object
                {
                    ObjectId = 125,
                    Flags00 = 0x20,
                },
                [126] = new TR1Object
                {
                    ObjectId = 126,
                    Flags00 = 0x20,
                },
                [127] = new TR1Object
                {
                    ObjectId = 127,
                    Flags00 = 0x00,
                },
                [128] = new TR1Object
                {
                    ObjectId = 128,
                    Flags00 = 0x00,
                },
                [129] = new TR1Object
                {
                    ObjectId = 129,
                    Flags00 = 0x20,
                },
                [130] = new TR1Object
                {
                    ObjectId = 130,
                    Flags00 = 0x20,
                },
                [131] = new TR1Object
                {
                    ObjectId = 131,
                    Flags00 = 0x20,
                },
                [132] = new TR1Object
                {
                    ObjectId = 132,
                    Flags00 = 0x20,
                },
                [133] = new TR1Object
                {
                    ObjectId = 133,
                    Flags00 = 0x00,
                },
                [134] = new TR1Object
                {
                    ObjectId = 134,
                    Flags00 = 0x00,
                },
                [135] = new TR1Object
                {
                    ObjectId = 135,
                    Flags00 = 0x00,
                },
                [136] = new TR1Object
                {
                    ObjectId = 136,
                    Flags00 = 0x00,
                },
                [137] = new TR1Object
                {
                    ObjectId = 137,
                    Flags00 = 0x20,
                },
                [138] = new TR1Object
                {
                    ObjectId = 138,
                    Flags00 = 0x20,
                },
                [139] = new TR1Object
                {
                    ObjectId = 139,
                    Flags00 = 0x20,
                },
                [140] = new TR1Object
                {
                    ObjectId = 140,
                    Flags00 = 0x20,
                },
                [141] = new TR1Object
                {
                    ObjectId = 141,
                    Flags00 = 0x20,
                },
                [142] = new TR1Object
                {
                    ObjectId = 142,
                    Flags00 = 0x20,
                },
                [143] = new TR1Object
                {
                    ObjectId = 143,
                    Flags00 = 0x20,
                },
                [144] = new TR1Object
                {
                    ObjectId = 144,
                    Flags00 = 0x20,
                },
                [145] = new TR1Object
                {
                    ObjectId = 145,
                    Flags00 = 0x20,
                },
                [146] = new TR1Object
                {
                    ObjectId = 146,
                    Flags00 = 0x21,
                },
                [147] = new TR1Object
                {
                    ObjectId = 147,
                    Flags00 = 0x61,
                },
                [148] = new TR1Object
                {
                    ObjectId = 148,
                    Flags00 = 0x00,
                },
                [149] = new TR1Object
                {
                    ObjectId = 149,
                    Flags00 = 0x00,
                },
                [150] = new TR1Object
                {
                    ObjectId = 150,
                    Flags00 = 0x01,
                },
                [151] = new TR1Object
                {
                    ObjectId = 151,
                    Flags00 = 0x01,
                },
                [152] = new TR1Object
                {
                    ObjectId = 152,
                    Flags00 = 0x00,
                },
                [153] = new TR1Object
                {
                    ObjectId = 153,
                    Flags00 = 0x01,
                },
                [154] = new TR1Object
                {
                    ObjectId = 154,
                    Flags00 = 0x00,
                },
                [155] = new TR1Object
                {
                    ObjectId = 155,
                    Flags00 = 0x01,
                },
                [156] = new TR1Object
                {
                    ObjectId = 156,
                    Flags00 = 0x01,
                },
                [157] = new TR1Object
                {
                    ObjectId = 157,
                    Flags00 = 0x00,
                },
                [158] = new TR1Object
                {
                    ObjectId = 158,
                    Flags00 = 0x01,
                },
                [159] = new TR1Object
                {
                    ObjectId = 159,
                    Flags00 = 0x00,
                },
                [160] = new TR1Object
                {
                    ObjectId = 160,
                    Flags00 = 0x01,
                },
                [161] = new TR1Object
                {
                    ObjectId = 161,
                    Flags00 = 0x00,
                },
                [162] = new TR1Object
                {
                    ObjectId = 162,
                    Flags00 = 0x60,
                },
                [163] = new TR1Object
                {
                    ObjectId = 163,
                    Flags00 = 0x61,
                },
                [164] = new TR1Object
                {
                    ObjectId = 164,
                    Flags00 = 0x01,
                },
                [165] = new TR1Object
                {
                    ObjectId = 165,
                    Flags00 = 0x00,
                },
                [166] = new TR1Object
                {
                    ObjectId = 166,
                    Flags00 = 0x01,
                },
                [167] = new TR1Object
                {
                    ObjectId = 167,
                    Flags00 = 0x00,
                },
                [168] = new TR1Object
                {
                    ObjectId = 168,
                    Flags00 = 0x01,
                },
                [169] = new TR1Object
                {
                    ObjectId = 169,
                    Flags00 = 0x01,
                },
                [170] = new TR1Object
                {
                    ObjectId = 170,
                    Flags00 = 0x20,
                },
                [171] = new TR1Object
                {
                    ObjectId = 171,
                    Flags00 = 0x00,
                },
                [172] = new TR1Object
                {
                    ObjectId = 172,
                    Flags00 = 0x01,
                },
                [173] = new TR1Object
                {
                    ObjectId = 173,
                    Flags00 = 0x01,
                },
                [174] = new TR1Object
                {
                    ObjectId = 174,
                    Flags00 = 0x00,
                },
                [175] = new TR1Object
                {
                    ObjectId = 175,
                    Flags00 = 0x00,
                },
                [176] = new TR1Object
                {
                    ObjectId = 176,
                    Flags00 = 0x01,
                },
                [177] = new TR1Object
                {
                    ObjectId = 177,
                    Flags00 = 0x21,
                },
                [178] = new TR1Object
                {
                    ObjectId = 178,
                    Flags00 = 0x01,
                },
                [179] = new TR1Object
                {
                    ObjectId = 179,
                    Flags00 = 0x20,
                },
                [180] = new TR1Object
                {
                    ObjectId = 180,
                    Flags00 = 0x69,
                },
                [181] = new TR1Object
                {
                    ObjectId = 181,
                    Flags00 = 0x61,
                },
                [182] = new TR1Object
                {
                    ObjectId = 182,
                    Flags00 = 0x68,
                },
            },
        };
    }
}
