using BethanysPieShop.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanysPieShop
{
    public class Startup
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                );


            services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<AppDbContext>();

            //Mock repositories using hard coded data and memory
            /*services.AddScoped<IPieRepository, MockPieRepository>();
            services.AddScoped<ICategoryRepository, MockCategoryRepository>();*/

            //Repositories using DB via EF Core
            services.AddScoped<IPieRepository, PieRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            //Invoke GetCart() method in the Shopping cart class
            //When user visits site i.e sends a request,
            //create a scoped shopping cart using GetCart() method
            //Check if cartId is already in the session,
            //else create one and a new shopping cart, link both and return latter
            //Since it is scoped, all interactions with that same shopping cart
            //within that same request, will use that same shopping cart
            services.AddScoped<ShoppingCart>(sp => ShoppingCart.GetCart(sp));

            //To be uses in ShoppingCart class
            services.AddHttpContextAccessor();

            //Bring in session support
            services.AddSession();

            services.AddControllersWithViews(); //Add MVC, services.AddMvc(); would also work still

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection(); //Redirect htpp requests to https
            app.UseStaticFiles(); //so images, css, js, etc files can be used
                                  //checks wwwroot directory for static files
                                  // this check can be changed

            app.UseSession(); //This has to be befoe UseRouting()
            app.UseRouting(); //Enables MVC routing in our app

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //Name of controller
                //Then name of action
                //Third parameter {id?} is optional(nullable)

                endpoints.MapRazorPages();
            });

        }
    }
}
