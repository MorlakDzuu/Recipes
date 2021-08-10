using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Domain.User;
using Infastructure.Repostitory;
using Application.Service;
using Infastructure;
using Microsoft.EntityFrameworkCore;
using Domain.Recipe;
using Domain.Tag;
using Domain.label;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Extranet.Api.Auth;
using Microsoft.AspNetCore.Http;

namespace Extranet.API
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services )
        {
            services.AddControllersWithViews();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddScoped<IRecipeService, RecipeService>();

            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITagService, TagService>();

            services.AddScoped<ILabelRepository, LabelRepository>();

            services.AddScoped<IPasswordService, PasswordService>();

            services.AddDbContext<ApplicationContext>( options => options.UseNpgsql( Configuration.GetSection( "ConnectionString" ).Value ) );
            services.AddScoped<IUnitOfWork>( sp => sp.GetService<ApplicationContext>() );

            services.AddAuthentication( auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            } )
                    .AddJwtBearer( options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.SaveToken = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = AuthOptions.ISSUER,
                            ValidateAudience = true,
                            ValidAudience = AuthOptions.AUDIENCE,
                            ValidateLifetime = true,
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true,
                        };
                    } );

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles( configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            } );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
        {
            if ( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler( "/Error" );
            }

            app.UseStaticFiles();
            if ( !env.IsDevelopment() )
            {
                app.UseSpaStaticFiles();
            }

            app.UseEndpoints( endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}" );
            } );

            app.UseRouting();
            app.UseSession();

            app.Use( async ( context, next ) =>
            {
                var JWToken = context.Session.GetString( "JWToken" );
                if ( !string.IsNullOrEmpty( JWToken ) )
                {
                    context.Request.Headers.Add( "Authorization", "Bearer " + JWToken );
                }
                await next();
            } );

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseSpa( spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if ( env.IsDevelopment() )
                {
                    spa.UseAngularCliServer( npmScript: "start" );
                }
            } );
        }
    }
}
