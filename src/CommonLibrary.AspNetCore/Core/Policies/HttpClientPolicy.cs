using System.Net;
using Polly;
using Polly.Retry;

namespace CommonLibrary.AspNetCore.Core.Policies;

public class HttpClientPolicy
{
    /*public AsyncRetryPolicy<HttpResponseMessage> LinearHttpRetryPolicy { get; }
    public AsyncRetryPolicy<HttpResponseMessage> LinHttpRetryPolicy { get; }
    public AsyncRetryPolicy<HttpResponseMessage> ImmediateHttpRetryPolicy { get; }
    public HttpClientPolicy()
    {
        ImmediateHttpRetryPolicy =
            Policy.HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode).RetryAsync(5);
        
        LinHttpRetryPolicy = Policy.HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode).WaitAndRetryAsync(5,
            (x) => TimeSpan.FromSeconds(3));
        
        LinearHttpRetryPolicy = Policy.HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode).WaitAndRetryAsync(5,
            (x) => TimeSpan.FromSeconds(Math.Pow(2,x)));
    }*/
    
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