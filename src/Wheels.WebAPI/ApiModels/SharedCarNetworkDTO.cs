using Wheels.Domain.Entities;

namespace Wheels.WebApi.ModelAPI;

public class SharedCarNetworkDTO
{
    public string Uuid { get; init; }
    public NetworkNode Destination { get; init; }
    public List<UserNode> Passengers { get; init; }
    public UserNode Driver { get; init; }
    public List<NetworkEdge> Edges { get; init; }

    public SharedCarNetworkDTO(
        string uuid,
        NetworkNode destination,
        UserNode driver,
        List<UserNode> passengers,
        List<NetworkEdge> edges
    )
    {
        Destination = destination;
        Driver = driver;
        Passengers = passengers;
        Edges = edges;
        Uuid = uuid;
    }

}

