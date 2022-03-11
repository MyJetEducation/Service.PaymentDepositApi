﻿using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyJetWallet.Sdk.Service;
using Prometheus;
using Service.Core.Client.Constants;
using Service.PaymentDepositApi.Modules;
using Service.Web;
using SimpleTrading.ServiceStatusReporterConnector;

namespace Service.PaymentDepositApi
{
	public class Startup
	{
		private const string DocumentName = "paymentdeposit";
		private const string ApiName = "PaymentDepositApi";

		public void ConfigureServices(IServiceCollection services)
		{
			services.BindCodeFirstGrpc();
			services.AddHostedService<ApplicationLifetimeManager>();
			services.AddMyTelemetry(Configuration.TelemetryPrefix, Program.Settings.ZipkinUrl);
			services.SetupSwaggerDocumentation(DocumentName, ApiName);
			services.ConfigurateHeaders();
			services.AddControllers();

			//services.AddCors(options =>
			//{
			//	options.AddPolicy("CorsApi",
			//		builder => builder.WithOrigins("http://localhost")
			//			.AllowAnyHeader()
			//			.AllowAnyMethod());
			//});

			services.ConfigureAuthentication();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();

			app.UseForwardedHeaders();
			app.UseRouting();
			app.UseCors("CorsApi"); //TODO: temporary
			app.UseStaticFiles();
			app.UseMetricServer();
			app.BindServicesTree(Assembly.GetExecutingAssembly());
			app.BindIsAlive();
			app.UseOpenApi();
			app.UseAuthentication();
			app.UseAuthorization();
			app.SetupSwagger(DocumentName, ApiName);

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapGet("/", async context => await context.Response.WriteAsync("API endpoint"));
			});
		}

		public void ConfigureContainer(ContainerBuilder builder)
		{
			builder.RegisterModule<SettingsModule>();
			builder.RegisterModule<ServiceModule>();
		}
	}
}