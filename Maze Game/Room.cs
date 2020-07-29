using System.Collections.Generic;

namespace Maze_Game
{
    public class Room
    {
        public int ID { get; set; }
        public List<Passage> Passages { get; set; }
        public uint WealthLeft { get; set; }
        public uint NumberOfPassages { get; set; }
        public uint MaxNumberOfPassages { get; set; }
        public List<Direction> AvaiableDirections { get; set; }
        public List<Direction> NotAvaiableDirections { get; set; }
        public Treasure Treasure { get; set; }
        public Threat Threat { get; set; }
        public bool FirstTime { get; set; }
    }

    public class Passage
    {
        public bool IsExit { get; set; }
        public Room Destination { get; set; }
        public Direction Direction { get; set; }
    }
}