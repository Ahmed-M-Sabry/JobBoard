using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<string> RegisterAsync(string email, string password, string fullName);
    }
}
