using RPG_Game.Models.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Models.Screens
{
    public class MainScreen:Screen
    {

        public override Player OpenScreen(Player player)
        {
            Console.WriteLine("Welctome!\nPress any key to play.");
            ConsoleKeyInfo key = Console.ReadKey();
            if (key != null)
            {
                Console.Clear();
            }
            return player;
        }
    }
}
