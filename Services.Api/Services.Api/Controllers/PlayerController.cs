using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Api.Context;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Services.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PlayerController : Controller
    {
        #region Fields
        private readonly PlayerContext _context;
        #endregion

        #region Ctor
        public PlayerController(PlayerContext context)
        {
            _context = context;

            if (!_context.Players.Any())
            {
                // Create a new player if the collection is empty
                _context.Players.Add(new Models.Player { Name = "Default Player", Points = 0 });
                _context.SaveChanges();
            }
        }
        #endregion

        #region Public Methods
        // GET: api/<controller>/<action>
        [HttpGet]
        public async Task<string> GetAllPlayers()
        {
            return await Task.FromResult(JsonConvert.SerializeObject(_context.Players.ToList()))
                .ConfigureAwait(false);
        }

        // GET api/<controller>/<action>5
        [HttpGet]
        public async Task<string> GetPlayerById(int id)
        {
            var result = _context.Players.Find(id);
            if (result != null)
            {
                return await Task.FromResult(JsonConvert.SerializeObject(result));
            }

            return await Task.FromResult(JsonConvert.SerializeObject(new JObject(
                new JProperty("Error", "Player Not Found")
                )));
        }

        [HttpGet]
        public async Task<string> CreateNewPlayer(string name)
        {
            var result = new Models.Player
            {
                Name = name,
                Points = 0
            };

            _context.Players.Add(result);
            _context.SaveChanges();

            return await Task.FromResult(JsonConvert.SerializeObject(result));
        }

        [HttpGet]
        public async Task<string> UpdatePlayer(int id, string name, int points)
        {
            var result = _context.Players.Find(id);
            if (result != null)
            {
                result.Name = name;
                result.Points = points;

                _context.Players.Update(result);
                _context.SaveChanges();

                return await Task.FromResult(JsonConvert.SerializeObject(result));
            }

            return await Task.FromResult(JsonConvert.SerializeObject(new JObject(
                new JProperty("Error", "Player Not Found")
                )));
        }
        #endregion
    }
}
