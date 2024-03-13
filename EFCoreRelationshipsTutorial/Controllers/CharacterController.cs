using EFCoreRelationshipsTutorial.DTO;
using EFCoreRelationshipsTutorial.Entities;
using EFCoreRelationshipsTutorial.Enums;
using EFCoreRelationshipsTutorial.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreRelationshipsTutorial.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CharacterController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Character")]
        public async Task<ActionResult<List<Character>>> GetCharacter()
        {
            var characters = await _context.Characters
                .Include(c => c.User)
                .Include(c => c.Weapons)
                .Include(c => c.Skills)!
                .ThenInclude(c => c.ElementType)
                .ToListAsync();
            return characters;
        }

       

        [HttpGet("GetSkill")]
        public async Task<ActionResult<List<Skill>>> GetSkill()
        {
            var skill = await _context.Skills
                .Include(c => c.Characters)
                .Include(c => c.ElementType)
                .ToListAsync();
            return skill;
        }

        [HttpGet("GetUser/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            Thread.Sleep(500);
            var user = await _context.Users.FindAsync(id);
            //return BadRequest("Erro ao buscar usuario");
            return user!;
        }

        [HttpGet("GetWeapon")]
        public async Task<ActionResult<List<Weapon>>> GetWeapon()
        {
            var weapon = await _context.Weapons
                .ToListAsync();
            return weapon;
        }

        [HttpGet("GetElement")]
        public async Task<ActionResult<List<Element>>> GetElement()
        {
            var elements = await _context.Elements
                .ToListAsync();
            return elements;
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<User>> CreateUser(CreateUserDto request)
        {

            var newUser = new User
            {
                Username = request.Username,
                Age = request.Age,
                Gender = request.Gender,
                isMale = request.isMale
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        [HttpPatch("EditUser")]
        public async Task<ActionResult<User>> EditUser(EditUserDto request)
        {
            Thread.Sleep(1500);
            var user = await _context.Users.FindAsync(request.Id);
            if (user == null)
                return BadRequest("User Not Found");

            _mapper.Map(request, user);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        [HttpPost("CreateCharacter")]
        public async Task<ActionResult<List<Character>>> CreateCharacter(CreateCharacterDto request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
                return NotFound();
            var newCharacter = new Character(request.Name, request.RpgClass, user, request.CreateSkillDto);
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

        [HttpPost("CreateElement")]
        public async Task<ActionResult<Element>> CreateElement(CreateElementDto createElement)
        {

            var newElemType = new Element
            {
                ElemType = (ElemType)createElement.ElementEnumValue
            };
            _context.Elements.Add(newElemType);
            await _context.SaveChangesAsync();
            return newElemType;
        }

        [HttpPost("CreateSkill")]
        public async Task<ActionResult<Skill>> CreateSkill(CreateSkillDto request)
        {

            var newSkill = new Skill()
            {
                Name = request.Name,
                Damage = (int)request.Damage
            };
            _context.Skills.Add(newSkill);
            await _context.SaveChangesAsync();
            return newSkill;
        }

        [HttpPost("CreateSkillAndChar")]
        public async Task<ActionResult<Skill>> CreateSkillAndChar(AddSkillAndCharDto request)
        {

            var user = _context.Users.Find(request.Char.UserId);

            Character newChar = new Character(request.Char.Name, request.Char.RpgClass, user);
            List<Character> charList = new List<Character>();
            charList.Add(newChar);

            Skill newSkill = new Skill(request.Skill.Name, (int)request.Skill.Damage, charList);


            await _context.AddAsync(newChar);
            await _context.AddAsync(newSkill);
            await _context.SaveChangesAsync();

            return newSkill;
        }

        [HttpPost("AddSkillElement")]
        public async Task<ActionResult<Skill>> AddSkillElement(AddElementDto request)
        {

            var skill = _context.Skills.Where(x => x.id == request.SkillId)
                .Include(w => w.ElementType).FirstOrDefault();

            List<Element> elemTypes = new();
            var element = _context.Elements.Find(request.ElementId);
            elemTypes.Add(element!);


            skill?.ElementType?.AddRange(elemTypes!);
            await _context.SaveChangesAsync();

            return skill!;
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

            character.Skills!.Add(skill);
            await _context.SaveChangesAsync();

            return character;
        }

        [HttpPost("SumTen")]
        public async Task<ActionResult<List<Character>>> SumTen()
        {
            var characters = await _context.Characters
                .Include(c => c.User)
                .Include(c => c.Weapons)
                .Include(c => c.Skills)!
                .ThenInclude(c => c.ElementType)
                .ToListAsync();
            foreach (var character in characters)
            {
                foreach (var skill in character.Skills)
                {
                    skill.sumTen();
                }
            }
            await _context.SaveChangesAsync();
            return characters;
        }

        [HttpPut("AddCharacterSkillElement")]
        public async Task<ActionResult<Character>> AddCharacterSkillElement(AddCharacterSkillElementDto request)
        {
            var character = await _context.Characters
                .Where(c => c.id == request.CharacterId)
                .Include(c => c.Skills)!
                .ThenInclude(c => c.ElementType)
                .FirstOrDefaultAsync();
            if (character == null)
                return NotFound("character not found!");
            List<Element> elemTypes = new();
            var element = _context.Elements.Find(request.ElementId);
            elemTypes.Add(element!);
            character.Skills?.FirstOrDefault(x => x.id == request.SkillId)?.ElementType?.AddRange(elemTypes);
            await _context.SaveChangesAsync();
            return character;
        }

        [HttpPut("EditCharacter")]
        public async Task<ActionResult<Character>> EditCharacter(EditCharacterDto editCharacterDto)
        {
            try
            {
                var character = await _context.Characters.Where(x => x.id == editCharacterDto.Id)
                        .Include(c => c.User)
                        .Include(c => c.Weapons)
                        .Include(c => c.Skills)!
                        .ThenInclude(s => s.ElementType)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
                if (character == null)
                    return NotFound("character not found!");
                _mapper.Map(editCharacterDto, character);
                _context.Update(character);
                _context.SaveChanges();
                var response = await _context.Characters.Where(x => x.id == editCharacterDto.Id)
                    .Include(c => c.User)
                    .Include(c => c.Weapons)
                    .Include(c => c.Skills)!
                    .ThenInclude(s => s.ElementType)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPut("EditCharacterFromFront")]
        public async Task<ActionResult<List<Character>>> EditCharacterFromFront(EditCharacterDtoFromFront editCharacterDto)
        {
            var character = await _context.Characters.Where(x => x.id == editCharacterDto.Id)
                .Include(c => c.User)
                .Include(c => c.Weapons)
                .Include(c => c.Skills)!
                .ThenInclude(s => s.ElementType)
                .FirstOrDefaultAsync();
            if (character == null)
                return NotFound("character not found!");
            _mapper.Map(editCharacterDto, character);
            await _context.SaveChangesAsync();

            var characters = await _context.Characters.ToListAsync();
            return characters;
        }

        [HttpPut("EditSkill")]
        public async Task<ActionResult<Skill>> EditSkill(EditSkillDto editCharacterDto)
        {
            var skill = await _context.Skills.FindAsync(editCharacterDto.Id);
            if (skill == null)
                return NotFound("skill not found!");
            skill.Name = editCharacterDto.Name;
            _context.Update(skill);
            _context.SaveChanges();
            return skill;
        }

        [HttpPost("FormFile")]
        public void FromFile(IFormFile request, string text)
        {
            var req = request.FileName;
            var t = text.Trim(); ;
        }
    }
}
