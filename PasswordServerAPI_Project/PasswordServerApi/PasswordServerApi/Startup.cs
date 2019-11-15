using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using PasswordServerApi.Security.SecurityModels;
using System.Collections.Generic;
using PasswordServerApi.Models.Enums;
using System.Linq;
using Serilog;
using Microsoft.Extensions.Logging;
using PasswordServerApi.StorageLayer;
using PasswordServerApi.DTO;
using PasswordServerApi.Utilitys;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Security;
using PasswordServerApi.Service;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace PasswordServerApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			LogInfo("Starting add services");
			InstallService(services, Configuration, "File");
			//services.InstallService(services,Configuration,"SQlLite");
			InstallSecurity(services, Configuration);
			InstallSwagger(services, Configuration);
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
			LogInfo("End addingg services");
		}

		#region Register Service

		public void InstallService(IServiceCollection services, IConfiguration configuration, string database)
		{
			services.AddTransient<ILoggingService, LoggingService>();

			if (database == "File")
			{
				services.AddFilleDB(configuration);
			}
			else if (database == "SQlLite")
			{
				services.AddSQLLiteDb(configuration);
			}

			services.AddTransient<IAccountService, AccountService>();
			services.AddTransient<IBaseService, BaseService>();
			services.AddTransient<IPasswordService, PasswordService>();
			services.AddTransient<IExportService, ExportService>();
			services.AddTransient<IExceptionHandler, ExceptionHandler>();
			services.AddScoped<IAuthenticateService, TokenAuthenticationService>();
			services.AddScoped<IUserManagementService, UserManagementService>();
		}

		public void InstallSecurity(IServiceCollection services, IConfiguration Configuration)
		{
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
		}

		public void InstallSwagger(IServiceCollection services, IConfiguration Configuration)
		{
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

			});

		}

		#endregion

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IStorageService storageService, ILoggerFactory loggerFactory)
		{
			app.Use(async (context, next) =>
			{
				Stopwatch timeCounter = new Stopwatch();
				timeCounter.Start();
				//DateTime request = DateTime.Now;
				//Log.Logger.Information($"MyMidlware: Now:{DateTime.Now} Request start: {request}");

				context.Response.OnStarting(() =>
				{
					timeCounter.Stop();
					context.Response.Headers.Add("TimeResponce", $"{timeCounter.Elapsed.TotalMilliseconds}");
					return Task.FromResult(0);
				});

				await next();
				//Log.Logger.Information($"MyMidlware: Now :{DateTime.Now} Request start: {request}");
			});

			loggerFactory.AddSerilog();
			SetStartUpData(storageService);

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

		#region   SetStartUpData

		private void SetStartUpData(IStorageService storageService)
		{
			LogInfo("Start SetUP Database and Init");
			List<AccountDto> oldAccounds = storageService.GetAccounts().ToList();
			List<PasswordDto> oldPasswords = storageService.GetPasswords().ToList();
			if (oldAccounds != null || oldAccounds.Count > 0)
				oldAccounds.ForEach(accont => storageService.DeleteAccount(accont));
			if (oldPasswords != null || oldPasswords.Count > 0)
				oldPasswords.ForEach(pass => storageService.DeletePassword(pass));
			FieldDatabae(storageService);
			LogInfo("Stop SetUP Database and Init");
		}


		private void FieldDatabae(IStorageService storageService)
		{
			for (int i = 105; i <= 115; i++)
			{
				var setData = GetDumyfullAccount(i);
				storageService.SetAccount(setData.Item1);
				setData.Item2.ForEach(pass => storageService.SetPassword(pass));
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

		#endregion

		#region helpers 

		public void LogInfo(string message)
		{
			Log.Logger.Information($"StartUpLog: {message}");
		}

		#endregion

	}
}
