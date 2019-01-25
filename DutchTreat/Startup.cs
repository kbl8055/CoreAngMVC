﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DutchTreat
{
  public class Startup
  {
    private readonly IConfiguration _config;

    public Startup(IConfiguration config)
    {
      this._config = config;
    }
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<DutchContext>(cfg =>
      {
        cfg.UseSqlServer(_config.GetConnectionString("DutchConnectionString"));
      });

      services.AddTransient<IMailService, NullMailService>();
      services.AddTransient<DutchSeeder>();
      services.AddScoped<IDutchRepository, DutchRepository>();
      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to conf igure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      #region default
      //if (env.IsDevelopment())
      //{
      //    app.UseDeveloperExceptionPage();
      //}

      //app.Run(async (context) =>
      //{
      //    await context.Response.WriteAsync("Hello World!");
      //});
      #endregion
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/error");
      }

      app.UseNodeModules(env);
      app.UseStaticFiles();
      app.UseMvc(cfg =>
      {
        cfg.MapRoute("Default",
            "/{controller}/{action}/{id?}",
            new { controller = "App", Action = "Index" });
      });
    }
  }
}