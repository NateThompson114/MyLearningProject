using System.Net;

namespace InterceptorOAuthToNTLM.Handlers;

public class NTLMHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var credential = new NetworkCredential("username", "password", "domain");
        var cache = new CredentialCache
        {
            { new Uri("YourServiceURL"), "NTLM", credential }
        };

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("NTLM");
        return base.SendAsync(request, cancellationToken);
    }
}