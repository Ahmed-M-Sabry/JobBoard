using JobBoard.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Dtos.JobPost
{
    public class JobPostQueryParameters
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }
        public string? Location { get; set; }

        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }

        public JobStatus? Status { get; set; }
    }
}
