namespace SpaceBattle.Lib;

using CoreWCF;
using CoreWCF.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

internal sealed class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddServiceModelWebServices(o =>
        {
            o.Title = "Space Battle API";
            o.Version = "1";
            o.Description = "API of Space Battle game.";
		});
	}

    public void Configure(IApplicationBuilder app)
    {
        app.UseServiceModel(builder =>
        {
			builder.AddService<WebApi>();
            builder.AddServiceWebEndpoint<WebApi, IWebApi>(new WebHttpBinding
            {
                MaxReceivedMessageSize = 5242880,
                MaxBufferSize = 65536,
			},
			"api",
			behavior =>
            {
                behavior.HelpEnabled = true;
                behavior.AutomaticFormatSelectionEnabled = true;
            });
		});
    }
}

