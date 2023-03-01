using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Models.Players;

namespace RPG_Game.Models.Interfaces
{
    public interface ICreateable
    {
        public Player CreateDefaultPlayer(char input, Player player);
        public Player CreateCustomPlayer(char input, int boostStrenghtPoints, int boostAgilityPoints, int boostIntelligencePoints, Player player);

        public void Setup();
    }
}
