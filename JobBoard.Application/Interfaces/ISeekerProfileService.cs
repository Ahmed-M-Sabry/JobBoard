using JobBoard.Application.Dtos.AuthenticationDtos.User.UserData.Profile;
using JobBoard.Application.Dtos.AuthenticationDtos.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Interfaces
{
    public interface ISeekerProfileService
    {
        Task<BecomeSeekerDto> CreateAsync(string userId, CreateSeekerProfileDto dto);

        Task<SeekerProfileDto> GetByUserIdAsync(string userId);

        //Task UpdateAsync(string userId, UpdateSeekerProfileDto dto);
    }
}
