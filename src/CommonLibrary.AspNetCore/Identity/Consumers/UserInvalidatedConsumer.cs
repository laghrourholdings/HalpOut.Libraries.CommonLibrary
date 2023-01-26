using MassTransit;

namespace CommonLibrary.AspNetCore.Identity.Consumers;
//TODO: Remove this and make it UserInvalidateConsumer()
public class UserInvalidatedConsumer : IConsumer<InvalidateUser>
{
    
    private readonly ISecuromanService _securomanService;

    public UserInvalidatedConsumer(
        ISecuromanService securomanService)
    {
        _securomanService = securomanService;
    }

    
    public Task Consume(ConsumeContext<InvalidateUser> context)
    {
        return _securomanService.RemoveUserAsync(context.Message.UserId);
    }
}