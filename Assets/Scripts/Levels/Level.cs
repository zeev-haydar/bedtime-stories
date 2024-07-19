using System.Collections;
using System.Collections.Generic;
using Managers;


namespace Levels
{
    public class Level
    {
        // Start is called before the first frame update
        public List<Wave> waves;
        public Level(List<Wave> waves)
        {
            this.waves = waves;
        }
    }
}

