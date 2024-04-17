using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SUT23_UserAPI.Models;
using SUT23_UserAPI.Services;

namespace SUT23_UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepository _userrepo;

        public UserController(IUserRepository uRepo)
        {
            _userrepo = uRepo;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_userrepo.GetAllUsers());
        }

        [HttpGet("id")]
        public IActionResult GetSingleUser(int id)
        {
            var u = _userrepo.GetUser(id);
            if(u != null)
            {
                return Ok(u);
            }
            return NotFound($"User with ID {id} not found...");
        }

        [HttpDelete("id")]
        public IActionResult DeleteUser(int id)
        {
            var userToDelete = _userrepo.GetUser(id);

            if (userToDelete != null)
            {
                _userrepo.DeleteUser(userToDelete);
                return Ok();
            }
            return NotFound($"User with ID {id} not found...");
        }

        [HttpPost]
        public IActionResult AddNewUser(User newUser)
        {
            _userrepo.AddUser(newUser);

            return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + newUser.ID, newUser);
        }

        [HttpPut("{id}")]
        public IActionResult EditUser(int id, User userToUpdate)
        {
            var existuser = _userrepo.GetUser(id);
            if(existuser != null)
            {
                userToUpdate.ID = existuser.ID;
                _userrepo.Update(userToUpdate);
            }
            return Ok();
        }
    }
}
