using RPG_Game.Models.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Models.Interfaces
{
    public interface IAttack
    {
        public int Attack(Player player,Monster monster);
    }
}
