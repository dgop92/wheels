using Wheels.Domain.Entities;

namespace Wheels.WebApi.ModelAPI;

public static class Transformers
{
    public static SharedCarNetworkDTO ToSharedCarNetworkDTO(SharedCarNetwork network)
    {
        List<UserNode> passengers = network.Passengers.Select(
            node => (UserNode)node
        ).ToList();

        return new SharedCarNetworkDTO(
            network.Uuid,
            network.Destination,
            (UserNode)network.Driver,
            passengers,
            network.Edges
        );
    }
}