using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Playground.Models.User;
using Playground.Repository.Timeline;
using Playground.Models.Dto;

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
        public async Task<IActionResult> Register([FromForm]RegisterUserRequestModel model, [FromBody]RegisterUserRequestModel bodyModel)
        {
            if (IsInvalid(model)) model = bodyModel;
            if (IsInvalid(model)) return StatusCode((int)HttpStatusCode.BadRequest);
            if (string.IsNullOrEmpty(model.RegistrationKey) || !model.RegistrationKey.Equals(RegistrationKey, StringComparison.InvariantCulture))
                return StatusCode((int)HttpStatusCode.Unauthorized, "No valid registration key provided");

            try
            {
                var (result, user) = await _userService.Register(model.Username, model.Password, model.Email, model.SignIn);
                var response = new RegisterUserResponseModel(user);
                if (result.Succeeded) return StatusCode((int)HttpStatusCode.OK, response);
                else return StatusCode((int)HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm]UserRequestModel model, [FromBody]UserRequestModel bodyModel)
        {
            if (IsInvalid(model)) model = bodyModel;
            if (IsInvalid(model)) return StatusCode((int)HttpStatusCode.BadRequest);
            var username = model.Username;
            var password = model.Password;

            try
            {
                var user = await _userService.Login(username, password);
                var response = new UserResponseModel(user);
                if (user != null) return StatusCode((int)HttpStatusCode.OK, response);
                else return StatusCode((int)HttpStatusCode.Unauthorized);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
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
            return StatusCode((int)HttpStatusCode.OK, new UserResponseModel(user));
        }

        // Remove currently logged in user
        [HttpDelete]
        [ActionName("Me")]
        [Authorize]
        public async Task<IActionResult> DeleteMe([FromForm]UserRequestModel model, [FromBody]UserRequestModel bodyModel)
        {
            string password = model?.Password;
            if (string.IsNullOrEmpty(password)) password = bodyModel?.Password;
            if (string.IsNullOrEmpty(password))
                return StatusCode((int)HttpStatusCode.BadRequest, "Password must be provided when attempting to delete own user");
            try
            {
                if (!await _userService.CorrectPassword(password))
                    return StatusCode((int)HttpStatusCode.Unauthorized, "Password does not match current user");
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

        private bool IsInvalid(UserRequestModel model)
        {
            return model == null || string.IsNullOrEmpty(model.Username);
        }
    }
}
