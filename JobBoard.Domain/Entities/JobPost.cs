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
    public class JobPost : BaseEntity
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Location { get; set; } = default!;
        public decimal? Salary { get; set; }
        public DateTime PostedDate { get; set; } = DateTime.UtcNow;
        public JobStatus MyProperty = JobStatus.Open;

        // Navigation properties
        public EmployerProfile EmployerProfile { get; set; }
        public int EmployerProfileId { get; set; }

    }
}
