using marketmanagement.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace marketmanagement.data
{
    internal class marketdbcontext : Microsoft.EntityFrameworkCore.DbContext
    {
        public Microsoft.EntityFrameworkCore.DbSet<category> categories { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<stock> stocks { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<sale> sales { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<saleitem> saleitems { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<product> products { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<supplier> suppliers { get; set; }
        protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=localsupermarket_db;Trusted_Connection=True;");
        }
    }
}
