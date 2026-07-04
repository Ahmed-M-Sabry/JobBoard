using JobBoard.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Dtos.JobPost
{
    public class EmployerJobApplicationCountDto
    {
        public int Id { get; set; }
        public string JobPostName { get; set; }
        public int ApplicationsCount { get; set; }

    }
}
