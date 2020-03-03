using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MaineCoonApi.Data {
    public class MaineCoonApiContext : DbContext {
        public MaineCoonApiContext(DbContextOptions<MaineCoonApiContext> options)
            : base(options) {
        }

        public DbSet<MaineCoonApi.Models.StudentScore> StudentScore { get; set; }

        //public DbSet<MaineCoonApi.Models.QuestRecord> QuestRecord { get; set; }
        public DbSet<MaineCoonApi.Models.User> User { get; set; }
        public DbSet<MaineCoonApi.Models.Processor> Processors { get; set; }
        public DbSet<MaineCoonApi.Models.UniversityProgram> UniversityPrograms { get; set; }
    }
}
