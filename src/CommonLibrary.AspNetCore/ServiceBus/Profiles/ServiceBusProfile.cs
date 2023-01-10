using AutoMapper;

namespace CommonLibrary.AspNetCore.ServiceBus;

public class ServiceBusProfile : Profile
{
    public ServiceBusProfile()
    {
        //Source -> Target
        CreateMap<ServiceBusMessageReponse<object>, ServiceBusMessage>();
        CreateMap<ServiceBusPayload<object>, ServiceBusMessage>();
    }
}