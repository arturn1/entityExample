using static EFCoreRelationshipsTutorial.Enums.ExEnum;

namespace EFCoreRelationshipsTutorial.Entities
{
    public class Telegram : Base
    {
        public CallType TypeCall { get; set; }
    }
}
