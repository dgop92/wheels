using Wheels.Domain.Entities;

namespace Wheels.Domain.Repository;

public interface ISharedCarNetworkRepository
{
    public Task<SharedCarNetwork[]> GetAll();
    public Task<SharedCarNetwork?> GetById(string uuid);
}