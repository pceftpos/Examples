using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Data;
using PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Data.Interface;
using PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Helpers;
using PCEFTPOS.WebAPI.PosCloudAPITest.Angular.Async.Model;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.Angular
{
    public class Startup
    {
        readonly bool DEBUG_AUTH = false;  // Use this flag to enable AAD auth in development environments (otherwise only enabled in Production/Staging)

        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // Settings
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
    
            services.AddSingleton<INotificationRepository, NotificationRepository>();
            services.AddSingleton<IHttpClientRepository, HttpClients>();
            services.AddSingleton<ISessionRepository, SessionRepository>();

            services.AddHostedService<BackgroundProcessor>();
            services.AddHostedService<GetTransactionProcessor>();

            if (DEBUG_AUTH || !HostingEnvironment.IsDevelopment())
            {
                services.AddAuthentication()
                .AddJwtBearer("Bearer", o =>
                {
                    o.RequireHttpsMetadata = !HostingEnvironment.IsDevelopment(); // This should only ever be disabled in dev

                    o.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["JwtSettings:SignKey"])),

                        ValidateAudience = true,
                        ValidAudience = Configuration["JwtSettings:Audience"],

                        ValidateIssuer = true,
                        ValidIssuer = Configuration["JwtSettings:Issuer"],

                        TokenDecryptionKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["JwtSettings:EncryptKey"])),

                        ValidateLifetime = true,
                        ClockSkew = System.TimeSpan.Zero // remove delay of token when expire
                    };
                });
            }
            
            services.AddAuthorization(c =>
            {
                c.AddPolicy("AuthenticatedPosNotifyUser", policy =>
                {
                    if (DEBUG_AUTH || !HostingEnvironment.IsDevelopment())
                    {
                        policy.AddAuthenticationSchemes("Bearer");
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim("PosNotify");
                    }
                    else
                    {
                        policy.RequireAssertion(ctx => { return true; });
                    }
                });
            });
            
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
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
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc();

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
