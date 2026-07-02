using JobBoard.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Dtos.JobPost
{
    public class EditJobPost
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string CompanyLocation { get; set; } = default!;
        public decimal? Salary { get; set; }
        public byte? JobPostStatus { get; set; }

    }
}
