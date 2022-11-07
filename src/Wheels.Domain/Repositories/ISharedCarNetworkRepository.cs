using Wheels.Domain.Entities;

namespace Wheels.Domain.Repository;

public interface ISharedCarNetworkRepository
{
    public SharedCarNetwork[] GetAll();
    public SharedCarNetwork? GetById(string uuid);
}