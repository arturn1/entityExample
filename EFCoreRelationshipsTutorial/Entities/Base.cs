using static EFCoreRelationshipsTutorial.Enums.ExEnum;

namespace EFCoreRelationshipsTutorial.Entities
{
    public class Base
    {
        public int id { get; set; } = new int();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
