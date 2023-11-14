using System.Text.Json.Serialization;

namespace EFCoreRelationshipsTutorial.Entities
{
    public class Skill : Base
    {
        public Skill(string name, int damage, Character? characters, List<WhatsApp>? whatsApps)
        {
            Name=name;
            Damage=damage;
            Characters=characters;
            WhatsApps=whatsApps;
        }

        public Skill(string name, int damage, Character characters)
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
        public Character? Characters { get; set; }

        public List<WhatsApp>? WhatsApps { get; set; }
    }
}
