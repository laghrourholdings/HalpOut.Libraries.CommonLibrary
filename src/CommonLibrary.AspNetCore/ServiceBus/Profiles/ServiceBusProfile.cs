using AutoMapper;
using CommonLibrary.AspNetCore.ServiceBus.Implementations;

namespace CommonLibrary.AspNetCore.ServiceBus;

public class ServiceBusProfile : Profile
{
    public ServiceBusProfile()
    {
        //Source -> Target
        CreateMap<IiObjectServiceBusMessageResponse, ServiceBusMessage>();
        CreateMap<IIObjectServiceBusRequest, ServiceBusMessage>();
        CreateMap<ServiceBusMessageReponse<object>, ServiceBusMessage>();
        CreateMap<ServiceBusRequest<object>, ServiceBusMessage>();
    }
}