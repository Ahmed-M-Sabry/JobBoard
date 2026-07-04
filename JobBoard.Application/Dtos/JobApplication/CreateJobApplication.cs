using JobBoard.Domain.Entities.Users;
using JobBoard.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Dtos.JobApplication
{
    public class CreateJobApplication
    {
        public int JobPostId { get; set; }
        public string? CVLink { get; set; }
    }
}
