using EFCoreRelationshipsTutorial.Enums;
using System.Text.Json.Serialization;

namespace EFCoreRelationshipsTutorial.Entities
{
    public class Weapon : Base
    {
        public string Name { get; set; } = string.Empty;
        public int Damage { get; set; } = 10;
        
    }
}
