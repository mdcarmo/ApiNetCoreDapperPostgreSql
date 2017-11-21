using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using Serilog.Core;

namespace WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "ToDo API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Marcelo Dias", Email = "marc29dias@yahoo.com.br", Url = "www.marcdias.com.br" },
                    License = new License { Name = "Use under LICX", Url = "https://example.com/license" }
                });
                
                // Set the comments path for the Swagger JSON and UI.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "WebApi.xml");
                c.IncludeXmlComments(xmlPath);
            });

            services.AddSingleton<IConfiguration>(Configuration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            //Para configurar um aplicativo para exibir uma página que mostra informações 
            //detalhadas sobre exceções, 
            //instale o Microsoft.AspNetCore.Diagnostics NuGet do pacote e adicione uma 
            //linha para o configurar método na classe de inicialização:
            if (env.IsDevelopment())
            {
                //Habilitar a página de exceção de desenvolvedor somente quando o aplicativo 
                //estiver em execução no ambiente de desenvolvimento. 
                app.UseDeveloperExceptionPage();
            }
            
            //Configurações do Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            
            
            app.UseMvc();

            //ao rodar a api a tela do swagger abre automaticamente
            app.Run(context => {
                context.Response.Redirect("/swagger");
                return Task.CompletedTask;
            });

            loggerFactory.AddConsole(Configuration.GetSection("Logging")); //log levels set in your configuration
            loggerFactory.AddDebug(); //does all log levels
        }
    }
}
