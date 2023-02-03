using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace CommonLibrary.ClientServices.Identity;

public class SecuromanCookieHandler : DelegatingHandler
{

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Console.WriteLine("Called!");
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            return await base.SendAsync(request, cancellationToken);
    }
    
}