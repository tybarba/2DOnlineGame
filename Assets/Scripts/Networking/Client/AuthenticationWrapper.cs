using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public static class AuthenticationWrapper
{
    private const int retryTimeToWait = 1000;
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
            await Task.Delay(200);
        }
        return AuthState;
    }

    private static async Task SignInAnonymouslyAsync(int maxTries)
    {
        try
        {
            AuthState = AuthState.Authenticating;
            for (int tries = 0; tries < maxTries; tries++)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                {
                    AuthState = AuthState.Authenticated;
                    break;
                }
                // wait before trying again
                await Task.Delay(retryTimeToWait);
            }
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError(ex.Message);
            AuthState = AuthState.Error;
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError(ex.Message);
            AuthState = AuthState.Error;
        }


        if (AuthState != AuthState.Authenticating)
        {
            Debug.LogWarning($"Player was not signed in successfully after {maxTries} attempts");
            AuthState = AuthState.Timeout;
        }
    }
}

public enum AuthState { NotAuthenticated, Authenticating, Authenticated, Error, Timeout }
