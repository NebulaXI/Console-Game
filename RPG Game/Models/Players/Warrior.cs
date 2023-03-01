using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Models.Players
{
    public class Warrior : Player
    {
        public Warrior()
        {
            Strenght = 3;
            Agility = 3;
            Intelligence = 0;
            Range = 1;
            Symbol = '@';
            Name = "Warrior";
        }
        public Warrior(int boostStrenght, int boostAgility, int boostInteligence)
        {
            Strenght = 3 + boostStrenght;
            Agility = 3 + boostAgility;
            Intelligence = 0 + boostInteligence;
            Range = 1;
            Symbol = '@';
            Name = "Warrior";
        }
    }
}
