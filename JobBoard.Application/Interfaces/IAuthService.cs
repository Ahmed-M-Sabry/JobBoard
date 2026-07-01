using JobBoard.Application.Dtos.AuthenticationDtos.Login;
using JobBoard.Application.Dtos.AuthenticationDtos.Resgister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Interfaces
{
    public interface IAuthService
    {
        Task<RegisteResponseDto> RegisterAsync(RegisterRequestDto request);

        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task<LoginResponseDto> RefreshTokensAsync(string refreshToken);
    }
}
