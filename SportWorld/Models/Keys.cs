using System;
namespace SportWorld
{
    public class Keys
    {
        public string Map { get; set; }

        public string Weather { get; set; }

        public Keys(string map, string weather)
        {
            Map = map;
            Weather = weather;
        }
    }
}
