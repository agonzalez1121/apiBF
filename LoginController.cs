using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using BCrypt.Net;
using CRUDMongo.Models;
using MongoDB.Driver;

namespace CRUDMongo
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MongoDBContext _context;

        public LoginController(MongoDBContext context)
        {
            _context = context;
        }

        [HttpPost("Register")]
        public async Task Register([FromBody]LoginData data)
        {
            string hashed = BCrypt.Net.BCrypt.HashPassword(data.Password);

            var user = new LoginData
            {
                UserName = data.UserName,
                Password = hashed
            };

            var userCollection = _context.GetCollection<LoginData>("users");
            await userCollection.InsertOneAsync(user);

        }

        [HttpPost("Login")]
        public async Task<bool> Login([FromBody]LoginData data)
        {

            if (string.IsNullOrWhiteSpace(data.UserName) || string.IsNullOrWhiteSpace(data.Password))
            {
                return false;
            }
           
                   

            var player = await _context.GetCollection<LoginData>("users").Find(p => p.UserName == data.UserName).FirstOrDefaultAsync();

            if (player == null)
            {
                return false;
            }

            if (!BCrypt.Net.BCrypt.Verify(data.Password, player.Password))
            {
                return false;
            }
            return true;
            
        }

       

    }
}
