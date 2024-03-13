using EFCoreRelationshipsTutorial.Enums;
using System.Text.Json.Serialization;

namespace EFCoreRelationshipsTutorial.Entities
{
    public class Skill : Base
    {
        public Skill(string name, int damage, List<Character>? characters, List<Element>? elemType)
        {
            Name=name;
            Damage=damage;
            Characters=characters;
            ElementType=elemType;
        }

        public Skill(string name, int damage, List<Character?> characters)
        {
            Name=name;
            Damage=damage;
            Characters=characters;
        }

        public Skill()
        {
            
        }

        public string Name { get; set; } = string.Empty;
        public int Damage { get; set; }
        [JsonIgnore]
        public List<Character>? Characters { get; set; }
        public List<Element>? ElementType { get; set; } = new List<Element>();

        public void sumTen()
        {
            this.Damage++;
        } 
    }
}
