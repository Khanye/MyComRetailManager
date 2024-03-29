﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Portal.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Portal.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly IConfiguration _config;
        private string authTokenStorageKey;

        public AuthenticationService(HttpClient httpClient,
                                     AuthenticationStateProvider authStateProvider,
                                     ILocalStorageService localStorage,
                                     IConfiguration config)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
            _config = config;
            authTokenStorageKey = _config["authTokenStorageKey"];
        }

        public async Task<AuthenticatedUserModel> Login(AuthenticationUserModel userForAuthentication)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair <string, string>("grant_type", "password"),
                new KeyValuePair <string, string>("username", userForAuthentication.Email),
                new KeyValuePair <string, string>("password", userForAuthentication.Password)
            });

            string api = _config["api "] + _config["tokenEndpoint"];
            var authResult = await _httpClient.PostAsync(api, data);
            var authContent = await authResult.Content.ReadAsStringAsync();

            if (authResult.IsSuccessStatusCode == false)
            {
                return null;
            }

            var result = JsonSerializer.Deserialize<AuthenticatedUserModel>(authContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            await _localStorage.SetItemAsync(authTokenStorageKey, result.Access_Token);

            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Access_Token);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Access_Token);

            return result;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync(authTokenStorageKey);
            ((AuthStateProvider)_authStateProvider).NotifyUserLogOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
