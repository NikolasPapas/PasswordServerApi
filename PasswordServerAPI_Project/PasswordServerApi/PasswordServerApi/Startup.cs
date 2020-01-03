using PasswordServerApi.Security.SecurityModels;
using PasswordServerApi.Models.Enums;
using PasswordServerApi.StorageLayer;
using PasswordServerApi.DTO;
using PasswordServerApi.Utilitys;
using PasswordServerApi.Interfaces;
using PasswordServerApi.Security;
using PasswordServerApi.Service;
using PasswordServerApi.Models.DTO;
using PasswordServerApi.Utilitys.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;

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
			InstallService(services, Configuration);
			//services.InstallService(services,Configuration,"SQlLite");
			InstallSecurity(services, Configuration);
			InstallSwagger(services, Configuration);
			services.AddRazorPages();
            services.AddCors();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            LogInfo("End addingg services");
		}

		#region Register Service

		public void InstallService(IServiceCollection services, IConfiguration configuration)
		{
			services.AddTransient<ILoggingService, LoggingService>();
			services.AddScoped<IConfigurationManager>(s => new ConfigurationManager(configuration));
			var configurationManager = services.BuildServiceProvider().GetService<IConfigurationManager>();

			if (configurationManager.GetString(null, "Db") == "File")
			{
				services.AddFilleDB(configurationManager);
			}
			else if (configurationManager.GetString(null, "Db") == "SQlLite")
			{
				services.AddSQLLiteDb(configurationManager);
			}

			services.AddTransient<IAccountService, AccountService>();
			services.AddTransient<IBaseService, BaseService>();
			services.AddTransient<IPasswordService, PasswordService>();
			services.AddTransient<IExportService, ExportService>();
			services.AddTransient<IExceptionHandler, ExceptionHandler>();
			services.AddScoped<IAuthenticateService, TokenAuthenticationService>();
			services.AddScoped<IUserManagementService, UserManagementService>();
			services.AddScoped<IPasswordHasher<AccountDto>, PasswordHasher<AccountDto>>();
			//services.AddSingleton<HttpClient, HttpClient>();
			//var serviceProvider = services.BuildServiceProvider();
			//var httpClientservice = serviceProvider.GetService<HttpClient>();
			//string[] controllers = new string[2];
			//controllers[0] = "isHacked";
			//controllers[1] = "getHackTimes";
			services.AddHttpClient<IPassswordHackScanner, PassswordHackScannerAPI>();
			//services.AddScoped<IPassswordHackScanner>(s => new PassswordHackScannerAPI(httpClientservice,"http://localhost:51360/api/hackScanner/", controllers));
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IStorageService storageService, ILoggerFactory loggerFactory)
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
            //app.UseMvc();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
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
			List<LoginTokenDto> oldTokens = storageService.GetTokens();
			if (oldAccounds != null || oldAccounds.Count > 0)
				oldAccounds.ForEach(accont => storageService.DeleteAccount(accont));
			if (oldPasswords != null || oldPasswords.Count > 0)
				oldPasswords.ForEach(pass => storageService.DeletePassword(pass));
			if (oldTokens != null || oldTokens.Count > 0)
				oldTokens.ForEach(pass => storageService.DeleteToken(pass));
			FieldDatabae(storageService);
			LogInfo("Stop SetUP Database and Init");
		}


		private void FieldDatabae(IStorageService storageService)
		{
			for (int i = 105; i <= 115; i++)
			{
				var setData = GetDumyfullAccount(i);
				storageService.SetAccount(setData.Item1, setData.Item1.Password);
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
