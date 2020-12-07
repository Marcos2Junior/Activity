using API.Dtos;
using API.Models;
using API.Services;
using AutoMapper;
using Business.Classes;
using Domain.Entitys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Repository.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UsersController : MainController
    {
        private readonly IUserRepository UserRepository;
        private readonly IMapper Mapper;
        private readonly FacebookAuthSettings _fbAuthSettings;

        public UsersController(IConfiguration configuration, IUserRepository userRepository, IMapper mapper, ILogger<UsersController> logger) : base(logger, userRepository)
        {
            _fbAuthSettings = new FacebookAuthSettings
            {
                AppId = configuration["Authentication:Facebook:AppId"],
                AppSecret = configuration["Authentication:Facebook:AppSecret"]
            };

            UserRepository = userRepository;
            Mapper = mapper;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserInsertDto userInsertDto)
        {
            try
            {
                var user = Mapper.Map<User>(userInsertDto);

                user.NextPasswordUpdate = DateTimeOffset.UtcNow.AddDays(15).DateTime;
                user.Password = Encript.HashValue(user.Password);
                user.Date = DateTimeOffset.UtcNow.Date;

                if (await UserRepository.AddAsync(user))
                {
                    var userToReturn = Mapper.Map<UserViewDto>(user);
                    userToReturn.Date = GetDateTimeZoneUser(userToReturn.Date).Value;
                    return Created(string.Empty, userToReturn);
                }
                else
                {
                    return BadRequest("Não foi possível concluir o seu registro, por favor tente novamente em alguns instantes");
                }
            }
            catch (Exception ex)
            {
                return ErrorException(ex, nameof(Register));
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var _user = await UserRepository.VerifyAcessAsync(Encript.HashValue(loginDto.Password), loginDto.LoginUser);

                if (_user != null)
                {
                    var result = LoginResult(_user);

                    if (result.StatusCode != 200) { return result; }

                    var userView = Mapper.Map<UserViewDto>(_user);

                    userView.Date = GetDateTimeZoneUser(userView.Date).Value;
                    return Ok(new
                    {
                        user = userView,
                        token = result.Value
                    });
                }

                return Unauthorized("Usuário ou senha inválido");
            }
            catch (Exception ex)
            {
                return ErrorException(ex, nameof(Login));
            }
        }

        private async Task<ReturnLogin> VerifySocialUserAsync(string name, string email)
        {
            User user = await UserRepository.VerifyByEmailAsync(email);

            if (user == null)
            {
                var appUser = new User
                {
                    Name = name,
                    Email = email,
                    Date = DateTimeOffset.UtcNow.DateTime
                };

                if (!await UserRepository.AddAsync(appUser))
                {
                    return null;
                }

                user = await UserRepository.VerifyByEmailAsync(email);
            }

            var result = LoginResult(user);

            if (result.StatusCode != 200) { return null; }

            var userView = Mapper.Map<UserViewDto>(user);
            userView.Date = GetDateTimeZoneUser(userView.Date).Value;
            return new ReturnLogin { Token = result.Value.ToString(), UserView = userView };
        }

        private ObjectResult LoginResult(User user)
        {
            string utcHeader = HttpContext.Request?.Headers["time_zone"];

            if (string.IsNullOrEmpty(utcHeader) || !int.TryParse(utcHeader, out int utc))
            {
                return BadRequest("could not identify time zone");
            }

            utc = utc / 60 * -1;

            var newToken = TokenService.GenerateToken(user, utc.ToString());

            return new OkObjectResult(newToken);
        }

        [HttpPost("auth-facebook")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginFacebook(string authToken)
        {
            HttpClient client = new HttpClient();
            var appAccessTokenResponse = await client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_fbAuthSettings.AppId}&client_secret={_fbAuthSettings.AppSecret}&grant_type=client_credentials");
            var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
            var userAccessTokenValidationResponse = await client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={appAccessToken.AccessToken}");
            var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

            if (!userAccessTokenValidation.Data.IsValid)
            {
                return BadRequest();
            }

            var userInfoResponse = await client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={authToken}");
            var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

            var userResult = await VerifySocialUserAsync(userInfo.Name, userInfo.Email);

            return userResult != null ? StatusCode(200, userResult) : StatusCode(400);
        }

        [HttpPost("auth-amazon")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAmazon(string authToken)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var userInfoResponse = await client.GetAsync("https://api.amazon.com/user/profile");

            if (userInfoResponse.StatusCode != HttpStatusCode.OK)
            {
                return BadRequest();
            }

            var userInfo = JsonConvert.DeserializeObject<AmazonApiResponse>(await userInfoResponse.Content.ReadAsStringAsync());

            var userResult = await VerifySocialUserAsync(userInfo.Name, userInfo.Email);

            return userResult != null ? StatusCode(200, userResult) : StatusCode(400);
        }


        //[HttpPost("update-password")]
        //public async Task<IActionResult> UpdatePassword(string oldPassword, string newPassword)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
    }
}
