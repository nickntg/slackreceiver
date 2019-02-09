using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SlackReceiver.Web.Models;
using SlackReceiver.Web.Runners;
using SlackReceiver.Web.Runners.Interfaces;

namespace SlackReceiver.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			var appSettingsSection = Configuration.GetSection("AppSettings");
			var appSettings = appSettingsSection.Get<AppSettings>();
			GlobalAppSettings.BaseBotDirectory = appSettings.BaseBotDirectory;
			GlobalAppSettings.RunTimeoutInSeconds = appSettings.RunTimeoutInSeconds;

			services.AddScoped<IRunner, PowerShellRunner>();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMvc();
		}
	}
}