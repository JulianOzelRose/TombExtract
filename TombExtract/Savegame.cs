using System;

namespace TombExtract
{
    public enum GameMode
    {
        Normal,
        Plus
    }

    public class Savegame
    {
        public int Offset { get; set; }
        public Int32 Number { get; set; }
        public string Name { get; set; }
        public GameMode Mode { get; set; }

        public Savegame(int savegameOffset, Int32 saveNumber, string levelName, GameMode gameMode)
        {
            Number = saveNumber;
            Name = levelName;
            Offset = savegameOffset;
            Mode = gameMode;
        }

        public override string ToString()
        {
            string modeSuffix = Mode == GameMode.Plus ? "+" : "";
            return $"{Name}{modeSuffix} - {Number}";
        }
    }
}
