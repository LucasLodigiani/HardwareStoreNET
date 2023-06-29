using Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> ValidateCredentials(string username, string password);

        Task<string> Authenticate(LoginDto user);

        Task<string> CreateUser(RegisterDto user);
    }
}
