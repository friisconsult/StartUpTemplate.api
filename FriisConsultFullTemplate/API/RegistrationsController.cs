using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using FriisConsultFullTemplate.Models;
using Microsoft.EntityFrameworkCore;

namespace FriisConsultFullTemplate.API
{
    /// <summary>
    /// Registrations controller.
    /// </summary>
    [Route("api/v1/[controller]"), Authorize]
    public class RegistrationsController : Controller
    {
        public RegistrationsController(DatabaseContext databaseContext)
        {
            DatabaseContext = databaseContext;
        }

        private DatabaseContext DatabaseContext { get; }


        /// <summary>
        /// Gets the registrations async.
        /// </summary>
        /// <returns>The registrations async.</returns>
        [HttpGet]
        public async Task<IActionResult> GetRegistrationsAsync()
        {
            var registrations = await DatabaseContext.Registrations.ToListAsync();

            return Ok(registrations);
        }
        /// <summary>
        /// Gets the registration async.
        /// </summary>
        /// <returns>The registration async.</returns>
        /// <param name="id">Identifier.</param>

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegistrationAsync([FromRoute] Guid id)
        {
            var registration = await DatabaseContext.Registrations.FindAsync(id);
            if (registration == null)
                return NotFound();

            return Ok(registration);
        }

        /// <summary>
        /// Posts the registration async.
        /// </summary>
        /// <returns>The registration async.</returns>
        /// <param name="registration">Registration.</param>
        [HttpPost]
        public async Task<IActionResult> PostRegistrationAsync([FromBody] Registration registration)
        {

            await DatabaseContext.Registrations.AddAsync(registration);
            await DatabaseContext.SaveChangesAsync();

            return Created(registration.Id.ToString(), registration);
        }
    }
}
