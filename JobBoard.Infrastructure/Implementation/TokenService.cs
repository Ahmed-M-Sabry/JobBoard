using JobBoard.Application.Dtos.AuthenticationDtos.Login;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.AuthenticationHepler;
using JobBoard.Infrastructure.Authentication;
using JobBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Infrastructure.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly JwtSetting _jwtSettings;
        private readonly ApplicationDbContext _context;
        public TokenService(IOptions<JwtSetting> jwtSetting,ApplicationDbContext context)
        {
            _jwtSettings = jwtSetting.Value;
            _context = context;
        }
        public string GenerateAccessToken(AuthenticatedUserDto user, IList<string> roles)
        {
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: credentials
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            
            return accessToken;
        }
        public async Task<string> GenerateRefreshToken(AuthenticatedUserDto user)
        {
            var refreshToken = GenerateRandomRefreshToken();
            _context.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                UserId = user.UserId,
                ExpiryAt = DateTime.UtcNow.AddDays(7),
            });
            await _context.SaveChangesAsync();

            return refreshToken;
        }
        public async Task<AuthenticatedUserDto> ValidateRefreshTokenAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (token == null || token.IsRevoked || token.IsExpired)
                throw new UnauthorizedAccessException();

            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new AuthenticatedUserDto
            {
                UserId = token.User.Id,
                Email = token.User.Email,
                FullName = token.User.FullName
            };
        }
        private string GenerateRandomRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        }
        public async Task LogoutAsync(string refreshTokenfromCookie)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshTokenfromCookie);
            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
                refreshToken.RevokedAt = DateTime.UtcNow;

                _context.RefreshTokens.Update(refreshToken);
                await _context.SaveChangesAsync();
            }
        }

        public DateTime GetTokenExpiration()
        {
            return DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);
        }
        //public async Task<string> ValidateRefreshTokenAsync(string RefreshToken)
        //{
        //    var refreshToken = await _context.RefreshTokens
        //        .Include(x => x.User)
        //        .FirstOrDefaultAsync(x => x.Token == RefreshToken);

        //    if (refreshToken == null)
        //    {
        //        throw new UnauthorizedAccessException("Invalid or expired refresh token");
        //    }
        //    if (refreshToken.IsRevoked)
        //    {
        //        throw new UnauthorizedAccessException(
        //            "Refresh token has been revoked.");
        //    }
        //    if (refreshToken.IsExpired)
        //    {
        //        throw new UnauthorizedAccessException(
        //            "Refresh token has expired.");
        //    }
        //    refreshToken.IsRevoked = true;
        //    refreshToken.RevokedAt = DateTime.UtcNow;
        //    _context.RefreshTokens.Update(refreshToken);
        //    await _context.SaveChangesAsync();

        //    var user = new AuthenticatedUserDto
        //    {
        //        UserId = refreshToken.User.Id,
        //        Email = refreshToken.User.Email,
        //        FullName = refreshToken.User.FullName
        //    };
        //    return await GenerateRefreshToken(user);
        //}
    }
}
