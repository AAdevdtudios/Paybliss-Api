﻿using Microsoft.EntityFrameworkCore;
using Paybliss.Models;

namespace Paybliss.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}