using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebSocketChat.Core;
using WebSocketChat.Core.SocketManager;
using WebSocketChat.SocketManager;

namespace WebSocketChat
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ConnectionManager>();
            services.AddSingleton<SocketHandler, WebSocketMessageHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets();
            app.Map("/ws", x => x.UseMiddleware<SocketMiddleware>(services.GetService<SocketHandler>()));
            app.UseStaticFiles();
        }
    }
}