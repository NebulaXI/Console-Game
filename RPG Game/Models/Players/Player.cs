using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_Game.Models.Interfaces;

namespace RPG_Game.Models.Players
{
    public class Player:ICreateable,IAttack
    {
        public int Strenght { get; set; }
        public int Agility { get; set; }
        public int Intelligence { get; set; }
        public int Range { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Damage { get; set; }
        public string Name { get; set; }
        public char Symbol { get; set; }

        public int Attack(Player player,Monster monster)
        {
            monster.Health -= player.Damage;
            return monster.Health;
        }

        public Player CreateCustomPlayer(char input, int boostStrenghtPoints, int boostAgilityPoints, int boostIntelligencePoints,Player player)
        {
            switch (input)
            {
                case '1':
                    player = new Warrior(boostStrenghtPoints, boostAgilityPoints, boostIntelligencePoints);
                    break;
                case '2':
                    player = new Archer(boostStrenghtPoints, boostAgilityPoints, boostIntelligencePoints);
                    break;
                case '3':
                    player = new Mage(boostStrenghtPoints, boostAgilityPoints, boostIntelligencePoints);
                    break;
            }
            return player;
        }

        public Player CreateDefaultPlayer(char input,Player player)
        {
            switch (input)
            {
                case '1':
                    player = new Warrior();
                    break;
                case '2':
                    player = new Archer();
                    break;
                case '3':
                    player = new Mage();
                    break;
            }
            return player;
        }

        public void Setup()
        {
            Health = Strenght * 5;
            Mana = Intelligence * 3;
            Damage = Agility * 2;

        }
    }
}
