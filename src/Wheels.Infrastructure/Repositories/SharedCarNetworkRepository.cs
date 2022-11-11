using Wheels.Domain.Entities;
using Wheels.Domain.Repository;

namespace Wheels.Infrastructure.Repository;

public static class DefaultCarNetworks
{

    public static SharedCarNetwork GetSharedCarNetwork1()
    {
        NetworkNode driver = new UserNode(
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
        NetworkNode driver = new UserNode(
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

    private static readonly SharedCarNetwork[] _networks = {
        DefaultCarNetworks.GetSharedCarNetwork1(),
        DefaultCarNetworks.GetSharedCarNetwork2(),
    };

    public Task<SharedCarNetwork[]> GetAll()
    {
        return Task.FromResult(_networks);
    }

    public Task<SharedCarNetwork?> GetById(string uuid)
    {
        return Task.FromResult(_networks.FirstOrDefault(n => n.Uuid == uuid));
    }
}