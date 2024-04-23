using EFCoreRelationshipsTutorial.Entities;
using EFCoreRelationshipsTutorial.Enums;

namespace EFCoreRelationshipsTutorial.DTO
{
    public class CreateWeapon
    {
        public string Name { get; set; } = string.Empty;
        public int Damage { get; set; } = 10;
    }

    public class CreateSkillDto
    {
        public string? Name { get; set; } = string.Empty;
        public int? Damage { get; set; }
    }

    public class EditSkillDto
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public int? Damage { get; set; } = 10;
    }

    public class CreateCharacterDto
    {
        public string Name { get; set; } = "Character";
        public string RpgClass { get; set; } = "Knight";
        public int UserId { get; set; }
        public List<Skill>? CreateSkillDto { get; set; }

    }
    
    public class EditCharacterDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? RpgClass { get; set; }
        public int? UserId { get; set; }
        public ICollection<Skill>? Skills { get; set; }


    }

    public class EditCharacterDtoFromFront
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? RpgClass { get; set; }
    }

    public class AddCharacterSkillDto
    {
        public int CharacterId { get; set; }
        public int SkillId { get; set; }
    }

    public class AddCharacterSkillElementDto
    {
        public int CharacterId { get; set; }
        public int SkillId { get; set; }
        public int ElementId { get; set; }
    }

    public class AddElementDto
    {
        public int SkillId { get; set; }
        public int ElementId { get; set;}

    }

    public class AddSkillAndCharDto
    {
        public CreateCharacterDto Char { get; set; }
        public CreateSkillDto Skill { get; set; }

    }

    public class CreateElementDto
    {
        public int ElementEnumValue { get; set; }
    }

    public class CreateUserDto
    {
        public string? Username { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public bool? isMale { get; set; }
    }

    public class EditUserDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public bool? isMale { get; set; }
        public ElemType ElementType { get; set; }
    }

    public class AddFromBody
    {
        public IFormFile File { get; set; }
        public string Text { get; set; }
    }
}
