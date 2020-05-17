using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using TaraBot.Extention;
using TaraBot.Service.Implementations;
using TaraBot.Service.Interface;
using TaraBot.Service.Settings;

namespace TaraBot
{
    public class Startup
    {
        private readonly IConfiguration Configuration;
        private AzureSpeechToTextSettign AzureSpeechToText => Configuration.GetSection("AzureSpeechToText").Get<AzureSpeechToTextSettign>();
        private TelegramBotSettings TelegramBotSettings => Configuration.GetSection("TelegramBotSettings").Get<TelegramBotSettings>();
        private ZamzarSettings ZamzarSettings => Configuration.GetSection("ZamzarSettings").Get<ZamzarSettings>();
        private SmallTalkSettings SmallTalkSettings => Configuration.GetSection("SmallTalkSettings").Get<SmallTalkSettings>();

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IBaseService, BaseService>();
            services.AddTransient<IVoiceTranscriberService, VoiceTranscriberService>();
            services.AddTransient<ISmallTalkService, SmallTalkService>();

            services.AddSingleton(AzureSpeechToText);
            services.AddSingleton(TelegramBotSettings);
            services.AddSingleton(ZamzarSettings);
            services.AddSingleton(SmallTalkSettings);

            services
                .AddTelegramBotClient(TelegramBotSettings)
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
