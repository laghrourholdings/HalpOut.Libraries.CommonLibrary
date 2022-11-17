﻿using System.Net;

namespace CommonLibrary.AspNetCore.ServiceBus;

public interface IServiceBusPayloadResponse<TOldSubject, TSubject> : 
    IServiceBusPayload<TSubject>
{
    public IServiceBusPayload<TOldSubject> InitialPayload { get; set; }
    public HttpStatusCode StatusCode {get; set;}

}