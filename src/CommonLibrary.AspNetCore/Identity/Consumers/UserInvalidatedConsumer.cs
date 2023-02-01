using MassTransit;

namespace CommonLibrary.AspNetCore.Identity.Consumers;
//TODO: Remove this and make it UserInvalidateConsumer()
public class UserInvalidatedConsumer : IConsumer<InvalidateUser>
{
    
    private readonly ISecuroman _securoman;

    public UserInvalidatedConsumer(
        ISecuroman securoman)
    {
        _securoman = securoman;
    }

    
    public Task Consume(ConsumeContext<InvalidateUser> context)
    {
        return _securoman.RemoveUserAsync(context.Message.UserId);
    }
}