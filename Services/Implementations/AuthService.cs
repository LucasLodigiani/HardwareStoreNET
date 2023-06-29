using AutoMapper;
using Common.Dtos;
using Common.Entities;
using Data.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        public AuthService(IMapper mapper, IUserRepository userRepository, IConfiguration config)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<string> Authenticate(LoginDto loginDto)
        {
            User? user = await _userRepository.FindUserByNameAsync(loginDto.Username);
            if (user == null)
            {
                throw new Exception("El nombre de usuario ingresado no existe actualmente.");
            }

            bool PasswordChecked = await _userRepository.CheckPasswordAsync(user.Username, loginDto.Password);

            if(PasswordChecked)
            {
                return this.GenerateToken(user);
            }
            else
            {
                throw new Exception("La contraseña ingresada es incorrecta.");
            }

        }

        public async Task<string> CreateUser(RegisterDto registerDto)
        {
            User? userExist = await _userRepository.FindUserByNameAsync(registerDto.Username);
            if (userExist != null)
            {
                throw new Exception("El nombre de usuario que intentas utilizar ya esta registrado.");
            }

            var user = _mapper.Map<User>(registerDto);

            bool createUserResult = await _userRepository.CreateUserAsync(user);
            if (createUserResult)
            {
                return user.Id.ToString();
            }
            else
            {
                throw new Exception("Ha ocurrido un error al crear el usuario.");
            }

        }

        private string GenerateToken(User user) 
        {
            //Paso 1: Crear el token
            var securityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Authentication:SecretKey"])); //Traemos la SecretKey del Json. agregar antes: using Microsoft.IdentityModel.Tokens;

            var credentials = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);

            //Los claims son datos en clave->valor que nos permite guardar data del usuario.
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.Id.ToString())); //"sub" es una key estándar que significa unique user identifier, es decir, si mandamos el id del usuario por convención lo hacemos con la key "sub".
            claimsForToken.Add(new Claim("given_name", user.Username)); //Lo mismo para given_name y family_name, son las convenciones para nombre y apellido. Ustedes pueden usar lo que quieran, pero si alguien que no conoce la app
            claimsForToken.Add(new Claim("given_email", user.Email)); //quiere usar la API por lo general lo que espera es que se estén usando estas keys.
            claimsForToken.Add(new Claim("role", user.Role)); //Debería venir del usuario

            var jwtSecurityToken = new JwtSecurityToken( //agregar using System.IdentityModel.Tokens.Jwt; Acá es donde se crea el token con toda la data que le pasamos antes.
              _config["Authentication:Issuer"],
              _config["Authentication:Audience"],
              claimsForToken,
              DateTime.UtcNow,
              DateTime.UtcNow.AddHours(1),
              credentials);

            var tokenToReturn = new JwtSecurityTokenHandler() //Pasamos el token a string
                .WriteToken(jwtSecurityToken);

            return tokenToReturn;
        }

        public Task<string> ValidateCredentials(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
