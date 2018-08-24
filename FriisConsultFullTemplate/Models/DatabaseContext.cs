// // 
// // DatabaseContext.cs
// // Author: Per Friis per.friis@friisconsult.com
// // © 2018 Per Friis Consult ApS
// // Created: 8/21/2018 8:29 PM
using System;
using Microsoft.EntityFrameworkCore;
namespace FriisConsultFullTemplate.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Registration> Registrations { get; set; }


        public DatabaseContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
