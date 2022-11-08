namespace Wheels.Domain.Entities;

public class NetworkNode
{
    public string uuid { get; init; }

    public Location Location { get; init; }

    public NetworkNode(Location location)
    {
        Location = location;
        uuid = Guid.NewGuid().ToString();
    }

    //Override equals method to compare two NetworkNodes using uuid
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        NetworkNode node = (NetworkNode)obj;
        return uuid == node.uuid;
    }

    public override int GetHashCode()
    {
        return uuid.GetHashCode();
    }
}

