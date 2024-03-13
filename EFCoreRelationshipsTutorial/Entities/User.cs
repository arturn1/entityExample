using EFCoreRelationshipsTutorial.Enums;

namespace EFCoreRelationshipsTutorial.Entities
{
    public class User : Base
    {
        public string? Username { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public bool? isMale { get; set; }
        public ElemType ElementType { get; set; }

    }
}
