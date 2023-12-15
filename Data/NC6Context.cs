using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NC6.Models;

namespace NC6.Data
{
    public class NC6Context : DbContext
    {
        public NC6Context (DbContextOptions<NC6Context> options)
            : base(options)
        {
        }

        public DbSet<NC6.Models.Group> Group { get; set; } = default!;

        public DbSet<NC6.Models.Student>? Student { get; set; }

        public DbSet<NC6.Models.Faculty>? Faculty { get; set; }
    }
}
