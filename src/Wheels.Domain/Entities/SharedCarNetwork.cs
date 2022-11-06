using SharedKernel.Errors;

namespace Wheels.Domain.Entities;

public class SharedCarNetwork
{
    public string Uuid { get; init; }
    public NetworkNode Destination { get; init; }
    public List<NetworkNode> Passengers { get; init; }
    public NetworkNode Driver { get; init; }
    public List<NetworkEdge> Edges { get; init; }
    public List<NetworkEdge> OptimalPath { get; set; }

    public SharedCarNetwork(
        NetworkNode destination,
        NetworkNode driver
    )
    {
        Destination = destination;
        Driver = driver;
        Passengers = new List<NetworkNode>();
        Edges = new List<NetworkEdge>();
        OptimalPath = new List<NetworkEdge>();
        Uuid = Guid.NewGuid().ToString();
    }

    public SharedCarNetwork(
        string uuid,
        NetworkNode destination,
        NetworkNode driver,
        List<NetworkNode> passengers,
        List<NetworkEdge> edges
    )
    {
        Destination = destination;
        Driver = driver;
        Passengers = passengers;
        Edges = edges;
        OptimalPath = new List<NetworkEdge>();
        Uuid = uuid;
    }

    public List<NetworkNode> GetNeighbors(NetworkNode node)
    {
        List<NetworkNode> neighbors = new List<NetworkNode>();
        foreach (NetworkEdge edge in Edges)
        {
            if (edge.ContainsNode(node))
            {
                neighbors.Add(edge.To);
            }
        }
        return neighbors;
    }

    public int GetTotalNodes()
    {
        return Passengers.Count + 2;
    }

    public NetworkEdge GetEdge(NetworkNode from, NetworkNode to)
    {
        foreach (NetworkEdge edge in Edges)
        {
            if (edge.ContainsNode(from) && edge.ContainsNode(to))
            {
                // to retrieve and edge taking into account the direction
                return new NetworkEdge(
                    from,
                    to,
                    edge.PathInfo
                );
            }
        }
        throw new DomainException(
            $"edge between {from.uuid} and {to.uuid} not found",
            ErrorCode.ApplicationIntegrityError
        );
    }

}

