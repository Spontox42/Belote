using System.Collections.Generic;

namespace Belote
{
    public class Player
    {
        public Player(string name)
        {
            Name = name;
            Points = new Dictionary<int, int>();
        }

        public string Name { get; set; }
        public int PointsDixDeDer { get; set; }
        public int PointsAnnonce { get; set; }
        public int PointsBeloteRebelote { get; set; }
        public Dictionary<int, int> Points { get; set; }
    }
}