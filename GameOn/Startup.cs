using GameOn.Web.Entities;
using GameOn.Web.Services;
using GameOn.Web.Services.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameOn.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
            });


            services.AddDbContext<GameOnContext>(options => { options.UseSqlServer(Configuration.GetConnectionString("GameOn")); });

            services.AddScoped<IMatchResultEntryService, MatchResultEntryService>();
            services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<IPlayerRegistrationService, PlayerRegistrationService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IRankHistoryLoggingService, RankHistoryLoggingService>();
            services.AddScoped<ITimelineService, TimelineService>();
            services.AddScoped<IRatingHelper, DurrantEloRatingHelper>();

            services.AddIdentity<Player, IdentityRole>()
                    .AddEntityFrameworkStores<GameOnContext>()
                    .AddDefaultUI(UIFramework.Bootstrap4)
                    .AddDefaultTokenProviders();

            services.AddControllersWithViews()
                .AddNewtonsoftJson();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            //if (env.IsDevelopment())
            //{
            //    //app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
