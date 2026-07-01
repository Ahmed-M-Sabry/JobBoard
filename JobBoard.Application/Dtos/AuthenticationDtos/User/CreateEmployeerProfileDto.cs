using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Dtos.AuthenticationDtos.UserData
{
    public class CreateEmployeerProfileDto
    {
        public string CompanyName { get; set; } = default!;
        public string Location { get; set; }
        public string Description { get; set; }
    }
}
