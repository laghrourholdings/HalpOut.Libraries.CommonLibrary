using AutoMapper;

namespace CommonLibrary.AspNetCore.Core.ServiceBus;

public class ServiceBusProfile : Profile
{
    public ServiceBusProfile()
    {
        //Source -> Target
        CreateMap<ServiceBusMessageReponse<object>, ServiceBusMessage>();
        CreateMap<ServiceBusPayload<object>, ServiceBusMessage>();
    }
}