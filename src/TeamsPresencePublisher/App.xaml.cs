using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Windows;
using TeamsPresencePublisher.Controls;
using TeamsPresencePublisher.Models;
using TeamsPresencePublisher.Options;
using TeamsPresencePublisher.Publishers;
using TeamsPresencePublisher.Services;

namespace TeamsPresencePublisher
{
    public partial class App : System.Windows.Application
    {
        private IServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            TraybarIcon traybarIcon = _serviceProvider.GetRequiredService<TraybarIcon>();
            traybarIcon.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            _ = services.AddHttpClient()
                        .AddSingleton<TraybarIcon>()
                        .AddSingleton<MainWindow>()
                        .AddSingleton<IMqttFactory, MqttFactory>()
                        .AddSingleton<PresenceViewModel>()
                        .AddSingleton<IMicrosoftAuthentication, MicrosoftAuthentication>()
                        .AddSingleton<IAuthenticationProvider>(
                            serviceProvider => serviceProvider.GetRequiredService<IMicrosoftAuthentication>().AuthProvider)
                        .AddSingleton<IPresenceService, PresenceService>()
                        .AddSingleton<MQTTPublisher>()
                        .AddSingleton<TeamsPresencePublisherOptions>()
                        .AddScoped<IOptionsService, OptionsService>()
                        .AddSingleton<GlobalOptions>(serviceProvider => serviceProvider.GetRequiredService<IOptionsService>().ReadSettingsAsync<GlobalOptions>().Result)
                        .AddSingleton<IEnumerable<IPublisher>>(
                            serviceProvider => new List<IPublisher>
                            {
                                serviceProvider.GetRequiredService<MQTTPublisher>()
                            });
        }
    }
}
