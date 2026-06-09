using JobBoard.Domain.Common;
using JobBoard.Domain.Entities.Users;
using JobBoard.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class JobApplication : BaseEntity
    {
        public int SeekerProfileId { get; set; }
        public SeekerProfile SeekerProfile { get; set; }

        public int JobPostId { get; set; }
        public JobPost JobPost { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

    }
}
