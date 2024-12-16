namespace SpaceBattle.Lib;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Http;
using Hwdtech;
using Hwdtech.Ioc;

public class Program
{
	public static void Main(string[] args)
	{
		new InitScopeBasedIoCImplementationCommand().Execute();
		IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",IoC.Resolve<object>("Scopes.Root")).Execute();

		SpaceBattle.Lib.ICommand strat = (SpaceBattle.Lib.ICommand)new RegisterThreadDepsStrategy().run_strategy();
		strat.Execute();

		var builder = WebApplication.CreateBuilder(args);

		builder.Services.Configure<KestrelServerOptions>(options =>
		{
			options.AllowSynchronousIO = true;
		});
		ThreadManager tm = new ThreadManager();
		builder.Services.AddSingleton<ThreadManager>(tm);
        builder.Services.AddRazorPages();
		builder.Services.AddControllers();

		var app = builder.Build();

		app.UseRouting();
		app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
		app.Use((context, next) =>
		{
			context.Request.EnableBuffering();
			return next();
		});


		app.Run();
	}
}
