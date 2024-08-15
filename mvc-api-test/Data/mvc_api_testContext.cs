using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using mvc_api_test.Models;

namespace mvc_api_test.Data
{
    public class mvc_api_testContext : DbContext
    {
        public mvc_api_testContext (DbContextOptions<mvc_api_testContext> options)
            : base(options)
        {
        }

        public DbSet<mvc_api_test.Models.Mission> Mission { get; set; } = default!;
    }
}
