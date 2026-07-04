using JobBoard.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Dtos.JobApplication
{
    public class JobApplicationDto
    {
        public int Id { get; set; }
        public int SeekerId { get; set; }
        public int JobPostId { get; set; }
        public string CVLink { get; set; }
        public ApplicationStatus Status { get; set; }
    }
}
