using System;

namespace TombExtract
{
    public class Savegame
    {
        public int Offset { get; set; }
        public Int32 Number { get; set; }
        public string Name { get; set; }

        public Savegame(int savegameOffset, Int32 saveNumber, string levelName)
        {
            Number = saveNumber;
            Name = levelName;
            Offset = savegameOffset;
        }

        public override string ToString()
        {
            return $"{Name} - {Number}";
        }
    }
}
