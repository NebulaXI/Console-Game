using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPG_Game.Data.Models
{
    public class Player
    {
        [Key]
        public Guid PlayerId { get; set; }
        public int Strenght { get; set; }
        public int Agility { get; set; }
        public int Intelligence { get; set; }
        public int Range { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Damage { get; set; }
        [Required]
        public string Name { get; set; }
        public char Symbol { get; set; }
        [Required]
        [Column(TypeName ="DATETIME2")]
        public DateTime Created { get; set; }
    }
}