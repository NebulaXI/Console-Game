using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Models.Players
{
    public class Archer : Player
    {
        public Archer()
        {
            Strenght = 2;
            Agility = 4;
            Intelligence = 0;
            Range = 2;
            Symbol = '#';
            Name = "Archer";
        }

        public Archer(int boostStrenght, int boostAgility, int boostInteligence)
        {
            Strenght = 2 + boostStrenght;
            Agility = 4 + boostAgility;
            Intelligence = 0 + boostInteligence;
            Range = 2;
            Symbol = '#';
            Name = "Archer";
        }

    }
}
