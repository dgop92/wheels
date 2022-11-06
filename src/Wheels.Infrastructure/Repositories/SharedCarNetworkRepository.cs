using Wheels.Domain.Entities;
using Wheels.Domain.Repository;

namespace Wheels.Infrastructure.Repository;

public static class DefaultCarNetworks
{

    public static SharedCarNetwork GetSharedCarNetwork1()
    {
        NetworkNode driver = new NetworkNode(
            new Location(10.9976739, -74.8172191),
            new User("e2795eca-456", UserType.Driver)
        );
        NetworkNode destination = new NetworkNode(
            new Location(10.995412, -74.791462)
        );

        return new SharedCarNetwork(
            destination,
            driver
        );
    }

    public static SharedCarNetwork GetSharedCarNetwork2()
    {
        NetworkNode driver = new NetworkNode(
            new Location(10.9976739, -74.8172191),
            new User("e2795eca-456", UserType.Driver)
        );
        NetworkNode destination = new NetworkNode(
            new Location(11.017229, -74.850513)
        );

        return new SharedCarNetwork(
            destination,
            driver
        );
    }

}

public class SharedCarNetworkMockRepository : ISharedCarNetworkRepository
{

    private readonly SharedCarNetwork[] _networks = {
        DefaultCarNetworks.GetSharedCarNetwork1(),
        DefaultCarNetworks.GetSharedCarNetwork2(),
    };

    public SharedCarNetwork[] GetAll()
    {
        return _networks;
    }

    public SharedCarNetwork? GetSharedCarNetworkById(string uuid)
    {
        return _networks.FirstOrDefault(n => n.Uuid == uuid);
    }
}