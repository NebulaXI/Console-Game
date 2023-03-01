using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Models.Players
{
    public class Mage : Player
    {
        public Mage()
        {
            Strenght = 2;
            Agility = 1;
            Intelligence = 3;
            Range = 3;
            Symbol = '*';
            Name = "Mage";
        }
        public Mage(int boostStrenght, int boostAgility, int boostInteligence)
        {
            Strenght = 2 + boostStrenght;
            Agility = 1 + boostAgility;
            Intelligence = 3 + boostInteligence;
            Range = 3;
            Symbol = '*';
            Name = "Mage";
        }
    }
}
