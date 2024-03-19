using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly MongoDBContext _context;

        public GameController(MongoDBContext context){
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Game>> Get(){
            return await _context.GetCollection<Game>("game").Find(_=>true).ToListAsync();
        }

        [HttpGet("{Email}")]
        public async Task<IActionResult> Get(string Email){

            try
            {
                   var info = await _context.GetCollection<Game>("game").Find(p=>p.Email == Email).FirstOrDefaultAsync();

                     if (info == null)
                         {
                        return NotFound();
                        }
                        return Ok(info);
            }
            catch (OperationFailedException ex)
            {
                return BadRequest(ex.Message);
            }
         
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Game data)
        {
            if (!ValidateData(data))
                return BadRequest("Invalid data");

            if (string.IsNullOrEmpty(data.Email))
                return BadRequest("Email is required");

            try
            {
                var filter = Builders<Game>.Filter.Eq(g => g.Email, data.Email);
                var updateDefinition = Builders<Game>.Update;
                var updateFields = new List<UpdateDefinition<Game>>();

                // Add fields to update
                if (!string.IsNullOrEmpty(data.Nombre))
                    updateFields.Add(updateDefinition.Set(g => g.Nombre, data.Nombre));

                if (data.CreatedAt.HasValue)
                    updateFields.Add(updateDefinition.Set(g => g.CreatedAt, data.CreatedAt));

                if (data.Level.HasValue)
                    updateFields.Add(updateDefinition.Set(g => g.Level, data.Level));

                if (data.Experience.HasValue)
                    updateFields.Add(updateDefinition.Set(g => g.Experience, data.Experience));

                if (data.Strength.HasValue)
                    updateFields.Add(updateDefinition.Set(g => g.Strength, data.Strength));

                if (!string.IsNullOrEmpty(data.CurrentDungeon))
                    updateFields.Add(updateDefinition.Set(g => g.CurrentDungeon, data.CurrentDungeon));

                if (data.CurrentFloor.HasValue)
                    updateFields.Add(updateDefinition.Set(g => g.CurrentFloor, data.CurrentFloor));

                if (data.EnemiesDefeated.HasValue)
                    updateFields.Add(updateDefinition.Set(g => g.EnemiesDefeated, data.EnemiesDefeated));

                if (data.BossesDefeated.HasValue)
                    updateFields.Add(updateDefinition.Set(g => g.BossesDefeated, data.BossesDefeated));

                if (data.Inventory != null)
                    updateFields.Add(updateDefinition.Set(g => g.Inventory, data.Inventory));

                if (data.EquippedWeapon != null)
                    updateFields.Add(updateDefinition.Set(g => g.EquippedWeapon, data.EquippedWeapon));

                if (data.EquippedArmor != null)
                    updateFields.Add(updateDefinition.Set(g => g.EquippedArmor, data.EquippedArmor));

                var update = Builders<Game>.Update.Combine(updateFields);
                var result = await _context.GetCollection<Game>("game").UpdateOneAsync(filter, update);

                if (!result.IsAcknowledged && result.ModifiedCount == 0)
                {
                    return NotFound();
                }

                return Ok();
            }
            catch (OperationFailedException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Game game){

            if (!ValidateData(game))
                 return BadRequest("Invalid data");

            try
            {
                await _context.GetCollection<Game>("game").InsertOneAsync(game);
                return CreatedAtAction(nameof(Get),new {id = game.Id},game);
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                return Conflict("Key del usuario duplicado, mal rollo");                
            }     
            catch (Exception ex)
                {
                   return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            
        }

        [HttpDelete("{Email}")]
            public async Task<IActionResult> Delete(string Email)
            {
                var result = await _context.GetCollection<Game>("game").DeleteOneAsync(p => p.Email == Email);

                if (!result.IsAcknowledged && result.DeletedCount == 0)
                {
                    return NotFound();
                }

                return Ok();
            }
    


        public bool ValidateData(Game data){
            if (string.IsNullOrWhiteSpace(data.Nombre))
            {
                return false;    
            }
            if (string.IsNullOrWhiteSpace(data.Email))
            {
                return false;
            }
            return true;
        }
    }

    [Serializable]
    internal class OperationFailedException : Exception
    {
        public OperationFailedException()
        {
        }

        public OperationFailedException(string? message) : base(message)
        {
        }

        public OperationFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OperationFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
