using EFCoreRelationshipsTutorial.DTO;
using EFCoreRelationshipsTutorial.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreRelationshipsTutorial.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly DataContext _context;

        public CharacterController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("Character")]
        public async Task<ActionResult<List<Character>>> GetCharacter()
        {
            var characters = await _context.Characters
                .Include(c => c.User)
                .Include(c => c.Weapons)
                .Include(c => c.Skills)
                .ThenInclude(s => s.WhatsApps)
                .ToListAsync();

            return characters;
        }

        [HttpGet("Skill")]
        public async Task<ActionResult<List<Skill>>> GetSkill()
        {
            var skill = await _context.Skills
                .Include(c => c.Characters)
                .ToListAsync();

            return skill;
        }

        [HttpGet("User")]
        public async Task<ActionResult<List<User>>> GetUser()
        {
            var users = await _context.Users
                .ToListAsync();

            return users;
        }

        [HttpGet("Weapon")]
        public async Task<ActionResult<List<Weapon>>> GetWeapon()
        {
            var weapon = await _context.Weapons
                .ToListAsync();

            return weapon;
        }

        [HttpPost("User")]
        public async Task<ActionResult<User>> CreateUser()
        {

            var newUser = new User
            {
                Username = "Artur"
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        [HttpPost("Character")]
        public async Task<ActionResult<List<Character>>> CreateCharacter(CreateCharacterDto request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
                return NotFound();

            var newCharacter = new Character(request.Name, request.RpgClass, user);



            _context.Characters.Add(newCharacter);
            await _context.SaveChangesAsync();

            return await GetCharacter();
        }

        [HttpPost("CreateWeapon")]
        public async Task<ActionResult<Weapon>> CreateWeapon(CreateWeapon request)
        {

            var newWeapon = new Weapon
            {
                Name = request.Name,
                Damage = request.Damage,
            };

            _context.Weapons.Add(newWeapon);
            await _context.SaveChangesAsync();

            return newWeapon;
        }

        [HttpPost("CreateWhatsApp")]
        public async Task<ActionResult<WhatsApp>> CreateWhatsApp()
        {

            var newWhatsApp = new WhatsApp
            {
                TypeCall = Enums.ExEnum.CallType.tl
            };

            _context.WhatsApps.Add(newWhatsApp);
            await _context.SaveChangesAsync();

            return newWhatsApp;
        }

        [HttpPost("CreateSkill")]
        public async Task<ActionResult<Skill>> CreateSkill(CreateSkillDto request)
        {

            var newSkill = new Skill()
            {
                Name = request.Name,
                Damage = request.Damage
            };

            _context.Skills.Add(newSkill);
            await _context.SaveChangesAsync();

            return newSkill;
        }

        [HttpPost("CreateSkillAndChar")]
        public async Task<ActionResult<Skill>> CreateSkillAndChar(AddSkillAndChar request)
        {

            var user = _context.Users.Find(request.Char.UserId);

            Character newChar = new Character(request.Char.Name, request.Char.RpgClass, user);

            Skill newSkill = new Skill(request.Skill.Name, request.Skill.Damage, newChar);


            _context.Add(newChar);
            _context.Add(newSkill);
            _context.SaveChanges();

            return newSkill;
        }

        [HttpPost("AddWhatsApp")]
        public async Task<ActionResult<Skill>> AddWhatsApp(AddWhatsAppDto request)
        {

            var skill = _context.Skills.Where(x => x.id == request.SkillId)
                .Include(w => w.WhatsApps).FirstOrDefault();

            var whatsApp = _context.WhatsApps.Find(request.WhatsAppId);


            skill?.WhatsApps?.Add(whatsApp);
            await _context.SaveChangesAsync();

            return skill;
        }

        [HttpPost("AddCharacterSkill")]
        public async Task<ActionResult<Character>> AddCharacterSkill(AddCharacterSkillDto request)
        {
            var character = await _context.Characters
                .Where(c => c.id == request.CharacterId)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync();
            if (character == null)
                return NotFound();

            var skill = await _context.Skills.FindAsync(request.SkillId);
            if (skill == null)
                return NotFound();

            character.Skills.Add(skill);
            await _context.SaveChangesAsync();

            return character;
        }
    }
}
