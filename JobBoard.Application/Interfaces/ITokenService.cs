using JobBoard.Application.Dtos.AuthenticationDtos.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(
            AuthenticatedUserDto user,
            IList<string> roles);

        Task<string> GenerateRefreshToken(AuthenticatedUserDto user);

        //Task<string> ValidateRefreshTokenAsync(string RefreshToken);
        Task<AuthenticatedUserDto> ValidateRefreshTokenAsync(string refreshToken);

        Task LogoutAsync(string refreshTokenfromCookie);


    }
}
