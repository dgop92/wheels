namespace Wheels.Domain.Entities;

public class UserNode : NetworkNode
{

    public User User { get; init; }

    public UserNode(Location location, User user) : base(location)
    {
        User = user;
    }
}

