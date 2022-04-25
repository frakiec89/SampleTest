using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.DB
{
    internal class MyContext : DbContext
    {
        private static string _dbConnectionString =
            "Server=192.168.10.134; database = Ahtyamov_test_25_04; user id= stud ; password=stud;";


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.UseSqlServer(_dbConnectionString);
        }

        public DbSet<AgentViewModel> AgentViewModels { get; set; } 
    }
}
