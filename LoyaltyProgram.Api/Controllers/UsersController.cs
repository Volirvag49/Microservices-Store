using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LoyaltyProgram.Api.Infrastructure.EF.Context;
using LoyaltyProgram.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoyaltyProgram.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/Users")]
    public class UsersController : Controller
    {
        private readonly LoyaltyProgramContext _loyaltyContext;

        public UsersController(LoyaltyProgramContext loyaltyContext)
        {
            _loyaltyContext = loyaltyContext;
        }


        [HttpGet]
        [Route("/users/all")]
        [ProducesResponseType(typeof(IEnumerable<LoyaltyProgramUser>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var user = await _loyaltyContext.LoyaltyProgramUser
                .Include(u => u.Settings)
                .ToListAsync();

            if (user.Count != 0)
            {
                return Ok(user);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("/users/{userId}")]
        [ProducesResponseType(typeof(LoyaltyProgramUser), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int userId)
        {
            var user = await _loyaltyContext.LoyaltyProgramUser
                .Include(u => u.Settings)
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("/users/register")]
        [ProducesResponseType(typeof(LoyaltyProgramUser), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RegisterUser([FromBody]LoyaltyProgramUser newUser)
        {
            if (newUser != null)
            {
                await _loyaltyContext.LoyaltyProgramUser.AddAsync(newUser);

                await _loyaltyContext.SaveChangesAsync();
                var createdUserUrl = this.Request.Host +"/users/" + newUser.Id;

                return Created(createdUserUrl, newUser);
            }

            return BadRequest("Ошибка при регистрации пользователя");
        }

        [HttpPut]
        [Route("/users/update")]
        [ProducesResponseType(typeof(LoyaltyProgramUser), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateUserAsync([FromBody]LoyaltyProgramUser user)
        {
            if (user == null)
            {
                return BadRequest("Требуется пользователь");
            }

            var usersSettings = new List<LoyaltyProgramSettings>();
            usersSettings = user.Settings.ToList();

            _loyaltyContext.LoyaltyProgramUser.Update(user);      

            await _loyaltyContext.SaveChangesAsync();

            return Ok(user);

        }
    }
}