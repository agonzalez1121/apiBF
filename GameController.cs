using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id){

            try
            {
                   var info = await _context.GetCollection<Game>("game").Find(p=>p.Email == id).FirstOrDefaultAsync();

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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Game data)
            {
                if (!ValidateData(data))
                    return BadRequest("Invalid  data");

                if (id != data.Id)
                    return BadRequest();

                try
                {
                    var result = await _context.GetCollection<Game>("Products").ReplaceOneAsync(p => p.Email == id, data);

                    if (!result.IsAcknowledged && result.ModifiedCount == 0)
                    {
                        return NotFound();
                    }

                    return NoContent();
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

        [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(string id)
            {
                var result = await _context.GetCollection<Game>("game").DeleteOneAsync(p => p.Email == id);

                if (!result.IsAcknowledged && result.DeletedCount == 0)
                {
                    return NotFound();
                }

                return NoContent();
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
