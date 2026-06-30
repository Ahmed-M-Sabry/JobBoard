using JobBoard.Application.Dtos.AuthenticationDtos;
using JobBoard.Application.Interfaces;
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

        public AuthService(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var userId = await _identityService.RegisterAsync(request.FullName, request.Email, request.Password);

            return new AuthResponseDto
            {
                UserId = userId,
                Email = request.Email,
                FullName = request.FullName,
            };       
        }
    }
}
