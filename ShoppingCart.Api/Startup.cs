using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Api.Client;
using ShoppingCart.Api.Client.Interfaces;
using ShoppingCart.Api.Infrastructure.EF.Context;
using ShoppingCart.Api.Infrastructure.EventFeed;
using ShoppingCart.Api.Infrastructure.EventFeed.Interfaces;
using System;
using System.Reflection;

namespace ShoppingCart.Api
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

            services.AddMvc().AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddDbContext<CartContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionString"],
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.
                MigrationsAssembly(
                    typeof(Startup).
                     GetTypeInfo().
                      Assembly.
                       GetName().Name);

                    //Configuring Connection Resiliency:
                    sqlOptions.
                        EnableRetryOnFailure(maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);

                });

                // Changing default behavior when client evaluation occurs to throw.
                // Default in EFCore would be to log warning when client evaluation is done.
                options.ConfigureWarnings(warnings => warnings.Throw(
                    RelationalEventId.QueryClientEvaluationWarning));

            });

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "ShoppingCart.Api",
                    Version = "v1",
                    Description = "ShoppingCart.Api",
                    TermsOfService = "Terms Of Service"
                });
            });

            services.AddTransient<IProductCatalogueClient, ProductCatalogueClient>();
            services.AddTransient<IEventStore, EventStore>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger()
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShoppingCart.Api V1");
            });
        }
    }
 
}
