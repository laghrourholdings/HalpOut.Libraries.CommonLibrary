using Polly;
using Polly.Retry;

namespace CommonLibrary.AspNetCore.Core.Policies;

public class HttpClientPolicy
{
    public AsyncRetryPolicy<HttpResponseMessage> LinearHttpRetryPolicy { get; }
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
    }
}