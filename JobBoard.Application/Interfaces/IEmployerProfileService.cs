using JobBoard.Application.Dtos.AuthenticationDtos.User.UserData.Profile;
using JobBoard.Application.Dtos.AuthenticationDtos.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Interfaces
{
    public interface IEmployerProfileService
    {
        Task CreateAsync(string userId, CreateEmployeerProfileDto dto);
        Task<EmployerProfileDto> GetByUserIdAsync(string userId);

        //Task UpdateAsync(string userId, UpdateEmployerProfileDto dto);
    }
}
