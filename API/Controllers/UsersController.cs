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
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Repository.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : MainController
    {
        private readonly IUserRepository UserRepository;
        private readonly IMapper Mapper;
        private static readonly HttpClient Client = new HttpClient();
        private readonly FacebookAuthSettings _fbAuthSettings;

        public UsersController(IConfiguration configuration, IUserRepository userRepository, IMapper mapper, ILogger<UsersController> logger) : base(logger)
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

                user.NextPasswordUpdate = DateTime.UtcNow.AddDays(15);
                user.Password = Encript.HashValue(user.Password);

                if (await UserRepository.AddAsync(user))
                {
                    var userToReturn = Mapper.Map<UserViewDto>(user);

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
                    var newToken = TokenService.GenerateToken(_user);

                    var userView = Mapper.Map<UserViewDto>(_user);
                    return Ok(new
                    {
                        user = userView,
                        token = newToken
                    });
                }

                return Unauthorized("Usuário ou senha inválido");
            }
            catch (Exception ex)
            {
                return ErrorException(ex, nameof(Login));
            }
        }

        [HttpPost("auth-Facebook/{authToken}")]
        public async Task<IActionResult> LoginFacebook(string authToken)
        {
            var appAccessTokenResponse = await Client.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_fbAuthSettings.AppId}&client_secret={_fbAuthSettings.AppSecret}&grant_type=client_credentials");
            var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessToken>(appAccessTokenResponse);
            var userAccessTokenValidationResponse = await Client.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={appAccessToken.AccessToken}");
            var userAccessTokenValidation = JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

            if (!userAccessTokenValidation.Data.IsValid)
            {
                return BadRequest();
            }

            var userInfoResponse = await Client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={authToken}");
            var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

            User user = await UserRepository.VerifyByEmailAsync(userInfo.Email);

            if (user == null)
            {
                var appUser = new User
                {
                    Name = userInfo.Name,
                    Email = userInfo.Email
                };

                if (!await UserRepository.AddAsync(appUser))
                {
                    return BadRequest();
                }

                user = await UserRepository.VerifyByEmailAsync(userInfo.Email);
            }

            var newToken = TokenService.GenerateToken(user);

            var userView = Mapper.Map<UserViewDto>(user);
            return Ok(new
            {
                user = userView,
                token = newToken
            });
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
