using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Data;
using PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Data.Interface;
using PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Model;
using PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.SignalR;
using PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Helpers;
using PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Interface;
using PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async.Repository;

namespace PCEFTPOS.WebAPI.PosCloudAPITest.RazorPages.Async
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

            services.AddSingleton<IHttpClientRepository, HttpClients>();
            services.AddSingleton<ISessionRepository, SessionRepository>();

            services.AddHostedService<BackgroundProcessor>();
            services.AddHostedService<GetTransactionProcessor>();

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN"); // Configure antiforgery service to make ajax requests from client

            // Settings
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Add my services
            services.AddSingleton<ITokenRepository, TokenRepository>();

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

            services.AddSignalR();

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
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSignalR(routes =>
            {
                routes.MapHub<NotifyHub>("/notify");
            });

            //app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
