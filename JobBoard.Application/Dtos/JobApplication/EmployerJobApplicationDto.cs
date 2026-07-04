using JobBoard.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Dtos.JobApplication
{
    public class EmployerJobApplicationDto
    {
        public int ApplicationId { get; set; }

        public int SeekerId { get; set; }

        public string ApplicantName { get; set; }
        public string ApplicantEmail { get; set; }

        public string JobTitle { get; set; }
        public int YearsOfExp { get; set; }
        public string CVLink { get; set; }

        public ApplicationStatus Status { get; set; }

        public DateTime AppliedAt { get; set; }
    }
}
