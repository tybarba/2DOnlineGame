using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public static class AuthenticationWrapper
{
    private const int retryTimeToWait = 10000;
    public static AuthState AuthState { get; private set; } = AuthState.NotAuthenticated;
    public static async Task<AuthState> DoAuth(int maxTries = 5)
    {
        // check if authenticated already
        if (AuthState == AuthState.Authenticated) return AuthState;
        if (AuthState == AuthState.Authenticating)
        {
            Debug.LogWarning("Already authenticating!");
            await Authenticating();
            return AuthState;
        }

        await SignInAnonymouslyAsync(maxTries);

        return AuthState;
    }

    private static async Task<AuthState> Authenticating()
    {
        bool authenticating = true;
        while (authenticating)
        {
            authenticating = AuthState == AuthState.Authenticating;
            await Task.Delay(2000);
        }
        return AuthState;
    }

    private static async Task SignInAnonymouslyAsync(int maxRetries)
    {
        AuthState = AuthState.Authenticating;

        int retries = 0;
        while (AuthState == AuthState.Authenticating && retries < maxRetries)
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                {
                    AuthState = AuthState.Authenticated;
                    break;
                }
            }
            catch (AuthenticationException authException)
            {
                Debug.LogError(authException);
                AuthState = AuthState.Error;
            }
            catch (RequestFailedException requestException)
            {
                Debug.LogError(requestException);
                AuthState = AuthState.Error;
            }

            retries++;
            await Task.Delay(1000);
        }

        if (AuthState != AuthState.Authenticated)
        {
            Debug.LogWarning($"Player was not signed in successfully after {retries} retries");
            AuthState = AuthState.Timeout;
        }
    }
}

public enum AuthState { NotAuthenticated, Authenticating, Authenticated, Error, Timeout }
