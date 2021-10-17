using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamsPresencePublisher.Options;

namespace TeamsPresencePublisher.Services
{
    public class MicrosoftAuthentication : IMicrosoftAuthentication
    {
        private const string Authority = "https://login.microsoftonline.com/common/";
        private const string RedirectUri = "http://localhost";
        private static readonly string[] s_scopes = new string[] { "User.Read", "Presence.Read" };
        private MsalCacheHelper _msalCacheHelper;
        private IPublicClientApplication _publicClientApplication;
        private GlobalOptions _globalOptions;
        TeamsPresencePublisherOptions _options;

        public IAuthenticationProvider AuthProvider { get; private set; }

        public MicrosoftAuthentication(TeamsPresencePublisherOptions options, GlobalOptions globalOptions)
        {
            _globalOptions = globalOptions;
            _options = options;
        }

        private void Init()
        {
            StorageCreationProperties storageProperties =
                new StorageCreationPropertiesBuilder("TeamsPresencePublisher.msalcache.bin", _options.CacheFolder, _globalOptions.AppId)
                .Build();

            _msalCacheHelper = MsalCacheHelper.CreateAsync(storageProperties).GetAwaiter().GetResult();

            _publicClientApplication = BuildPublicClientApplication();
            AuthProvider = new InteractiveAuthenticationProvider(_publicClientApplication, s_scopes);
        }

        public async Task<bool> SigninAsync()
        {
            try
            {
                if (_publicClientApplication == null || _publicClientApplication.AppConfig.ClientId != _globalOptions.AppId)
                    Init();
            }
            catch(Exception)
            {
                return false;
            }

            bool result = false;

            try
            {
                var builder = _publicClientApplication.AcquireTokenInteractive(s_scopes);
                _ = await builder.ExecuteAsync();

                result = true;
            }
            catch (MsalServiceException) // access_denied when user cancels login
            {
            }

            return result;
        }

        public async Task<bool> IsSignedInAsync()
        {
            try
            {
                if (_publicClientApplication == null || _publicClientApplication.AppConfig.ClientId != _globalOptions.AppId)
                    Init();
            }
            catch
            {
                return false;
            }

            IEnumerable<IAccount> accounts = await _publicClientApplication.GetAccountsAsync();
            return accounts.Any();
        }

        public async Task<string> GetUserNameAsync()
        {
            try
            {
                if (_publicClientApplication == null || _publicClientApplication.AppConfig.ClientId != _globalOptions.AppId)
                    Init();
            }
            catch
            {
                return null;
            }

            IEnumerable<IAccount> accounts = await _publicClientApplication.GetAccountsAsync();
            return accounts.FirstOrDefault()?.Username;
        }

        public async Task Signout()
        {
            if (_publicClientApplication == null)
                return;

            foreach (IAccount account in await _publicClientApplication.GetAccountsAsync())
            {
                await _publicClientApplication.RemoveAsync(account);
            }
        }

        private IPublicClientApplication BuildPublicClientApplication()
        {
            IPublicClientApplication application = PublicClientApplicationBuilder
                    .Create(_globalOptions.AppId)
                    .WithAuthority(Authority)
                    .WithRedirectUri(RedirectUri)
                    .Build();

            _msalCacheHelper.RegisterCache(application.UserTokenCache);

            return application;
        }
    }
}
