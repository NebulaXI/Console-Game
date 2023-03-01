using RPG_Game.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Game.Models.Players
{
    public class Monster : IAttack
    {
        public int Strenght { get; set; }
        public int Agility { get; set; }
        public int Intelligence { get; set; }
        public int Range { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Damage { get; set; }
        public string Name { get; set; }
        public char Symbol { get; }
        public Dictionary<string,int> Position { get; set; }=new Dictionary<string, int>();
        public Monster()
        {
            Random r = new Random();
            Strenght = r.Next(1, 3);
            Agility = r.Next(1, 3);
            Intelligence = r.Next(1, 3);
            Range = 1;
            Symbol = '$';//'◙';
            Name = "Monster";
            Position=CreatePosition();
        }

        private Dictionary<string, int> CreatePosition()
        {
            Random r = new Random();
            int monsterRow = r.Next(1, 10);
            int monsterCol = r.Next(1, 10);
            Dictionary<string, int> returnDic = new Dictionary<string, int>
            {
                { "monsterRow", monsterRow },
                { "monsterCol", monsterCol }
            };
            return returnDic;
        }
        public void Setup()
        {
            Health = Strenght * 5;
            Mana = Intelligence * 3;
            Damage = Agility * 2;

        }

        public int Attack(Player player,Monster monster)
        {
            player.Health -= monster.Damage;
            return player.Health;
        }
    }
}
