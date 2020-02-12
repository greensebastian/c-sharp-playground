using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Playground.Models.User;
using Playground.Repository.Timeline;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Playground.Controllers
{
    public class AuthenticationController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly TimelineRepository _timelineRepository;
        private readonly string RegistrationKey;

        public AuthenticationController(UserService userService, TimelineRepository timelineRepository, IConfiguration configuration)
        {
            _userService = userService;
            _timelineRepository = timelineRepository;
            RegistrationKey = configuration.GetValue<string>("RegistrationKey");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserRequestModel model)
        {
            if (string.IsNullOrEmpty(model.RegistrationKey) || !model.RegistrationKey.Equals(RegistrationKey, StringComparison.InvariantCulture))
                return StatusCode((int)HttpStatusCode.Unauthorized, "No valid registration key provided");
            var username = model.Username;
            var password = model.Password;
            bool succeeded;
            try
            {
                succeeded = await _userService.Register(username, password);
            }catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            return succeeded ? StatusCode((int)HttpStatusCode.OK) : StatusCode((int)HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserRequestModel model)
        {
            var username = model.Username;
            var password = model.Password;
            bool succeeded;
            try
            {
                succeeded = await _userService.Login(username, password);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            return succeeded ? StatusCode((int)HttpStatusCode.OK) : StatusCode((int)HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _userService.Logout();
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            return StatusCode((int)HttpStatusCode.OK);
        }

        // Retrieve currently logged in user
        [HttpGet]
        [ActionName("Me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var user = await _userService.CurrentUser();
            return StatusCode((int)HttpStatusCode.OK, $"Logged in as: {user.UserName}");
        }

        // Remove currently logged in user
        [HttpDelete]
        [ActionName("Me")]
        [Authorize]
        public async Task<IActionResult> DeleteMe(string password)
        {
            if (string.IsNullOrEmpty(password))
                return StatusCode((int)HttpStatusCode.BadRequest, "Password must be provided when attempting to delete own user");
            try
            {
                await _timelineRepository.RemoveTimelineData(await _userService.CurrentUser());
                await _userService.LogoutAndDeleteCurrent(password);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Test()
        {
            var user = await _userService.CurrentUser();
            return StatusCode((int)HttpStatusCode.OK, $"Authorized successfully as: {user.UserName}");
        }
    }
}
