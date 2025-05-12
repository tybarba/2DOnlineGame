using System.Threading.Tasks;
using Unity.Services.Authentication;

public static class AuthenticationWrapper
{
    private const int retryTimeToWait = 1000;
    public static AuthState AuthState {get; private set;} = AuthState.NotAuthenticated;
    public static async Task<AuthState> DoAuth(int maxTries = 5){
        // check if authenticated already
        if (AuthState == AuthState.Authenticated) return AuthState;

        AuthState = AuthState.Authenticating;
        for(int tries = 0; tries < maxTries; tries++){
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            if(AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized){
                AuthState = AuthState.Authenticated;
                break;
            }
            // wait before trying again
            await Task.Delay(retryTimeToWait);
        }

        if(AuthState != AuthState.Authenticated) AuthState = AuthState.Error;

        return AuthState;
    }
}

public enum AuthState {NotAuthenticated, Authenticating, Authenticated, Error, Timeout}
