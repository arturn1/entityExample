namespace EFCoreRelationshipsTutorial.DTO
{
    public class CreateWeapon
    {
        public string Name { get; set; } = string.Empty;
        public int Damage { get; set; } = 10;
    }

    public class CreateSkillDto
    {
        public string Name { get; set; } = string.Empty;
        public int Damage { get; set; } = 10;
    }

    public class CreateCharacterDto
    {
        public string Name { get; set; } = "Character";
        public string RpgClass { get; set; } = "Knight";
        public int UserId { get; set; }

    }

    public class AddCharacterSkillDto
    {
        public int CharacterId { get; set; }
        public int SkillId { get; set; }
    }

    public class AddWhatsAppDto
    {
        public int SkillId { get; set; }
        public int WhatsAppId { get; set;}

    }

    public class AddSkillAndChar
    {
        public CreateCharacterDto Char { get; set; }
        public CreateSkillDto Skill { get; set; }

    }
}
