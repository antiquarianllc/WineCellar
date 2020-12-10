using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace WineCellar
{
    using Npgsql;

    using WineCellar.Entities;

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
            var builder = new NpgsqlConnectionStringBuilder( );
            builder.ConnectionString = Configuration.GetConnectionString( "PostgreSqlConnection" );
            builder.Username = Configuration[ "UserID" ];
            builder.Password = Configuration[ "Password" ];

            services.AddDbContext<WineCellarDBContext>( opt => opt.UseNpgsql( builder.ConnectionString ) );

            services.AddControllers();

            // 2020/12/10 - Add Swashbuckle service to auto-generate API documentation.
            services.AddSwaggerGen( options =>
            {
                options.SwaggerDoc( "v1", new OpenApiInfo { Title = "WineCellar API", Version = "v1" } );
            } );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WineCellarDBContext wcConext)
        {
            // 2020/10/21 - Move applying migrations to only happen in 'Developement' environment.
            //--wcConext.Database.Migrate( );

            if ( env.IsDevelopment( ) )
            {
                wcConext.Database.Migrate( );
                app.UseDeveloperExceptionPage( );
            }

            app.UseHttpsRedirection( );

            app.UseRouting( );

            app.UseAuthorization( );

            // 2020/12/10 - Add Swashbuckle service to auto-generate API documentation.
            app.UseSwagger( );
            app.UseSwaggerUI( options => 
            {
                options.SwaggerEndpoint( "/swagger/v1/swagger.json", "WineCellar API v1" );
            } );

            app.UseEndpoints( endpoints =>
            {
                 endpoints.MapControllers( );
            } );
        }
    }
}
