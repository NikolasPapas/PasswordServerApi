using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Service;
using System;
using System.Reflection;
using System.IO;
using PasswordServerApi.Security;
using PasswordServerApi.Security.SecurityModels;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using PasswordServerApi.DataSqliteDB;
using Microsoft.EntityFrameworkCore;
using PasswordServerApi.DataSqliteDB.DataModels;
using System.Collections.Generic;
using PasswordServerApi.Models.Enums;
using Newtonsoft.Json;
using System.Linq;
using Serilog;
using Microsoft.Extensions.Logging;
using PasswordServerApi.Extensions;
using PasswordServerApi.DataFileDb;
using PasswordServerApi.StorageLayer;
using PasswordServerApi.DTO;

namespace PasswordServerApi
{
	public class Startup
	{

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			LogInfo("Starting add services");
			services.AddTransient<ILoggingService, LoggingService>();

			services.AddTransient<IReadFileDb>(s => new ReadFileDb("C:\\PASSWORDSERVERAPI", "Accounts.txt", "Paswords.txt"));
			services.AddTransient<IApplicationFileDb, ApplicationFileDb>();
			services.AddTransient<IStorageService, StorageServiceFile>();


			//services.AddEntityFrameworkInMemoryDatabase().AddEntityFrameworkSqlite().AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=PasswordServer.db"));
			//var provider = services.BuildServiceProvider();
			//services.AddTransient<IStorageService, StorageServiceDb>();



			services.AddTransient<IAccountService, AccountService>();
			services.AddTransient<IBaseService, BaseService>();
			services.AddTransient<IPasswordService, PasswordService>();
			services.AddTransient<IExportService, ExportService>();
			services.AddTransient<IExceptionHandler, ExceptionHandler>();

			services.AddScoped<IAuthenticateService, TokenAuthenticationService>();
			services.AddScoped<IUserManagementService, UserManagementService>();


			services.Configure<TokenManagement>(Configuration.GetSection("tokenManagement"));
			var token = Configuration.GetSection("tokenManagement").Get<TokenManagement>();
			var secret = System.Text.Encoding.ASCII.GetBytes(token.Secret);
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(x =>
			{
				x.Events = new JwtBearerEvents
				{
					OnTokenValidated = context =>
					{
						var accessToken = context.SecurityToken as JwtSecurityToken;
						var userService = context.HttpContext.RequestServices.GetRequiredService<IAuthenticateService>();
						if (!userService.IsAuthorized(Guid.Parse(context.Principal.Identity.Name), accessToken.RawData))
						{
							// return unauthorized if user no longer exists
							context.Fail("Unauthorized");
						}

						return Task.CompletedTask;
					}
				};
				//x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(secret),
					ValidIssuer = Configuration["jwt:Issuer"], //token.Issuer,
					ValidAudience = Configuration["jwt:Issuer"], //token.Audience,
					ValidateIssuer = false,
					ValidateAudience = false
				};

			});


			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "PasswordServerApi",
					Description = "A simple PasswordServerApi ASP.NET Core2.2 Web API",
					TermsOfService = new Uri("http://example.com/terms"),
					Contact = new OpenApiContact
					{
						Name = "Papazian Nikolaos",
						Email = "nikolaspapazian@gmail.com",
					},
					License = new OpenApiLicense
					{
						Name = "Use under LICX",
					}
				});

				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
				LogInfo("End addingg services");
			});
		}



		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IStorageService storageService, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddSerilog();

			LogInfo("Start SetUP Database and Init");
			//storageService.Database.EnsureCreated();

			List<AccountDto> oldAccounds = storageService.GetAccountsDto().ToList();
			List<PasswordDto> oldPasswords = storageService.GetPasswordsDto().ToList();
			if (oldAccounds != null || oldAccounds.Count>0)
				oldAccounds.ForEach(accont => storageService.DeleteAccountsDto(accont));
			if (oldPasswords != null || oldPasswords.Count > 0)
				oldPasswords.ForEach(pass => storageService.DeletePasswordsDto(pass));
			FieldDatabae(storageService);
			LogInfo("Stop SetUP Database and Init");


			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
			app.UseAuthentication();
			//app.UseHttpsRedirection();
			app.UseMvc();
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "PasswordServerApi");
			});
		}

		private void FieldDatabae(IStorageService storageService)
		{
			for (int i = 105; i <= 115; i++)
			{
				var setData = GetDumyfullAccount(i);
				storageService.SetAccountsDto(setData.Item1);
				setData.Item2.ForEach(pass => storageService.SetPasswordsDto(pass));
			}
		}

		private PasswordDto GetDumyPassword(int i, int accountIndex)
		{
			return new PasswordDto()
			{
				PasswordId = Guid.NewGuid(),
				Name = "Google" + i * i,
				UserName = $"nikolaspapazian{accountIndex}@gmail.com",
				Password = $"123{ i * i}",
				LogInLink = $"google{accountIndex}.com",
				Sensitivity = Sensitivity.OnlyUser,
			};
		}

		private AccountDto GetDumyAccount(int i)
		{
			return new AccountDto()
			{
				AccountId = Guid.NewGuid(),
				FirstName = $"FirstName{i}",
				LastName = $"LastName{i}",
				UserName = $"username{i}",
				Email = $"email{i}@cite.gr",
				Role = i == 105 ? Role.Admin : i == 106 ? Role.User : Role.Viewer,
				Password = $"{i}",
				Sex = Sex.Male,
				LastLogIn = null,
				Passwords = new List<PasswordDto>(),
			};
		}

		private Tuple<AccountDto, List<PasswordDto>> GetDumyfullAccount(int i)
		{
			AccountDto account = GetDumyAccount(i);
			List<PasswordDto> passwords = new List<PasswordDto>();
			for (int dumyi = i; dumyi <= i + 5; dumyi++)
			{
				PasswordDto pass = GetDumyPassword(dumyi, i);
				passwords.Add(pass);
				account.Passwords = passwords;
			}

			return new Tuple<AccountDto, List<PasswordDto>>(account, passwords);
		}

		public void LogInfo(string message)
		{
			Log.Logger.Information($"StartUpLog: {message}");
		}


	}
}
