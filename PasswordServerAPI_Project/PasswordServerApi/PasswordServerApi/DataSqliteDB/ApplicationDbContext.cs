﻿using Microsoft.EntityFrameworkCore;
using PasswordServerApi.DataSqliteDB.DataModels;
using PasswordServerApi.Models.Enums;
using PasswordServerApi.Security.SecurityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordServerApi.DataSqliteDB
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<EndityAbstractModelAccount> Accounts { get; set; }

		public DbSet<EndityAbstractModelPassword> Passwords { get; set; }

    }
}