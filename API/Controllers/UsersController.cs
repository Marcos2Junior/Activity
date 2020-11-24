using API.Dtos;
using API.Services;
using AutoMapper;
using Business.Classes;
using Domain.Entitys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using Repository.Interfaces;
using Repository.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : MainController
    {
        private readonly IUserRepository UserRepository;
        private readonly IMapper Mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper, ILogger<UsersController> logger) : base(logger)
        {
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
    }
}
