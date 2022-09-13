using EFCore.DataBaseFirst.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.DataBaseFirst.DataAccessLayer
{
    public class AppDbContext : DbContext
    {
       
        public DbSet<Products> Products { get; set; }


        public AppDbContext()
        {

        }

        //Dbden okuma 1.Yol
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        //Dbden okuma 2.Yol
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DbContextInitializer.Configuration.GetConnectionString("SqlCon"));
        }
    }
}
