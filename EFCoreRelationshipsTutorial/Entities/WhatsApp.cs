using static EFCoreRelationshipsTutorial.Enums.ExEnum;

namespace EFCoreRelationshipsTutorial.Entities
{
    public class WhatsApp : Base
    {
        public CallType TypeCall { get; set; }
    }
}
