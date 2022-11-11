using Wheels.Domain.Entities;
using Wheels.Domain.Dtos;

namespace Wheels.Domain.UseCases;

public interface ISharedCarNetworkUseCase
{

    public Task<SharedCarNetwork[]> GetAll();
    public Task<SharedCarNetwork?> GetById(string uuid);
    public Task<SharedCarNetwork> AddPassengerToNetwork(
        ReComputeNetworkDTO reComputeNetworkDTO
    );
    public Task<SharedCarNetwork> RemovePassengerToNetwork(
        ReComputeNetworkDTO reComputeNetworkDTO
    );
}