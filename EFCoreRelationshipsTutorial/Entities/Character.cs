using EFCoreRelationshipsTutorial.DTO;
using System.Text.Json.Serialization;

namespace EFCoreRelationshipsTutorial.Entities
{
    public class Character : Base
    {
        public Character(string name, string rpgClass, User user, List<Weapon>? weapons, ICollection<Skill>? skills)
        {
            Name=name;
            RpgClass=rpgClass;
            User=user;
            Weapons=weapons;
            Skills=skills;
        }

        public Character(string name, string rpgClass, User user, ICollection<Skill> CreateSkillDto)
        {
            Name =name;
            RpgClass=rpgClass;
            User=user;
            Skills =CreateSkillDto;
        }

        public Character(string name, string rpgClass, User user)
        {
            Name =name;
            RpgClass=rpgClass;
            User=user;
        }

        public Character()
        {

        }

        public string? Name { get; set; } = string.Empty;
        public string? RpgClass { get; set; } = "Knight";
        public User? User { get; set; }
        public List<Weapon>? Weapons { get; set; }
        public ICollection<Skill>? Skills { get; set; }

    }
}
