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
			services.AddTransient<IAccountService, AccountService>();
			services.AddTransient<IBaseService, BaseService>();
			services.AddTransient<IPasswordService, PasswordService>();
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
						var userService = context.HttpContext.RequestServices.GetRequiredService<IAuthenticateService>();
						if (userService.IsAuthorized(context.Principal.Identity.Name))
						{
							// return unauthorized if user no longer exists
							context.Fail("Unauthorized");
						}
						return Task.CompletedTask;
					}
				};
				x.RequireHttpsMetadata = false;
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
					TermsOfService = new Uri("https://example.com/terms"),
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



		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
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
			app.UseHttpsRedirection();
			app.UseMvc();
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "PasswordServerApi");
			});
		}
	}
}
