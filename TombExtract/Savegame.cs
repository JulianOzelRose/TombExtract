using System;

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

        public Savegame(int savegameOffset, Int32 saveNumber, string levelName, GameMode gameMode)
        {
            Number = saveNumber;
            Name = levelName;
            Offset = savegameOffset;
            Mode = gameMode;
            IsEmptySlot = false;
        }

        public override string ToString()
        {
            return !IsEmptySlot ? $"{Name}{(Mode == GameMode.Plus ? "+" : "")} - {Number}" : "Empty Slot";
        }
    }
}
