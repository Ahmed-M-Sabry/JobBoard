using JobBoard.Application.AuthenticationDtos.UserData;
using JobBoard.Application.Dtos.AuthenticationDtos.Login;
using JobBoard.Application.Dtos.AuthenticationDtos.Resgister;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.AuthenticationHepler;
using JobBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Infrastructure.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;
        private readonly JwtSetting _jwtSettings;
        public AuthService(IIdentityService identityService
            , ITokenService tokenService
            , IOptions<JwtSetting> jwtSettings)
        {
            _identityService = identityService;
            _tokenService = tokenService;
            _jwtSettings = jwtSettings.Value;
        }
        public async Task<RegisteResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var userId = await _identityService.RegisterAsync(request.Email, request.Password, request.FullName);

            return new RegisteResponseDto
            {
                UserId = userId,
                Email = request.Email,
                FullName = request.FullName,
            };
        }
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _identityService.ValidateUserAsync(request.Email, request.Password);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var roles = await _identityService.GetRolesAsync(user.UserId);

            var token = _tokenService.GenerateAccessToken(user, roles);

            var refreshToken = await _tokenService.GenerateRefreshToken(user);

            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

            var response = new LoginResponseDto
            {
                AccessToken = token,
                RefreshToken = refreshToken, // Implement refresh token
                ExpiresAt = expiresAt // Set expiration time
            };
            return response;
        }
        public async Task<LoginResponseDto> RefreshTokensAsync(string refreshToken)
        {
            var user = await _tokenService.ValidateRefreshTokenAsync(refreshToken);

            var roles = await _identityService.GetRolesAsync(user.UserId);

            var accessToken = _tokenService.GenerateAccessToken(user, roles);
            var newRefreshToken = await _tokenService.GenerateRefreshToken(user);

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes)
            };
        }

        public async Task<UserData> GetUserData(string userId)
        {
            var user = await _identityService.GetUserByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            return new UserData
            {
                FullName = user.FullName,
                Email = user.Email,
                Roles = user.Roles
            };
        }
    }
}
