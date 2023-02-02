using System.Net;
using Polly;
using Polly.Retry;

namespace CommonLibrary.ClientServices.Policies;

public class HttpClientPolicy
{
    public AsyncRetryPolicy<HttpResponseMessage> LinearHttpRetryPolicy { get; }
    public HttpClientPolicy()
    {

        LinearHttpRetryPolicy = Policy.HandleResult<HttpResponseMessage>(
            res => 
                res.StatusCode != HttpStatusCode.OK 
            && res.StatusCode != HttpStatusCode.NotFound
            && res.StatusCode != HttpStatusCode.BadRequest).WaitAndRetryAsync(3,
            (x) => TimeSpan.FromSeconds(Math.Pow(2,x)));
    }
}