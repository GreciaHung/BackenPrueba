using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using API.Core;
using API.Models;
using System.Transactions;
using API.Repositories;

namespace API.Controllers
{
   
    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private UserRepository userRepository = new UserRepository();

        [Authorize]
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
      
            return Ok(userRepository.GetAll()
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.EmailAddress,
                    u.UserName,
                })
            );
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            var u = await userRepository.GetOne(id);
            if (u == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.EmailAddress,
                u.UserName,
            });
        }


        [HttpPut]
        [Route("")]
        public async Task<IHttpActionResult> PutUser(Models.UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userRepository.GetOne(model.Id);
            if(user == null)
            {
                return NotFound();
            }


            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.UserName;
            user.EmailAddress = model.EmailAddress;
            if (model.Password != null)
            {
                user.Salt = PasswordHasherService.GenerateSalt();
                user.Hash = PasswordHasherService.HashPassword(model.Password, user.Salt);
            }
        
            
            
            try
            {
                await userRepository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!userRepository.UserExists(model.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> PostUser(Models.UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            var user = new User();
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.UserName;
            user.EmailAddress = model.EmailAddress;
            user.Salt = PasswordHasherService.GenerateSalt();
            user.Hash = PasswordHasherService.HashPassword(model.Password, user.Salt);

            userRepository.Add(user);

            await userRepository.SaveAsync();

            return Created(Request.RequestUri + user.Id.ToString(), user);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {

            //Terminar
            await userRepository.SaveAsync();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                userRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}