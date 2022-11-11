using Wheels.Domain.UseCases;
using Wheels.Domain.Entities;
using Wheels.Domain.Dtos;
using Wheels.Domain.Repository;
using Wheels.Domain.NetworkAlgorithms;
using Wheels.Infrastructure.Validators;
using SharedKernel.Errors;
using FluentValidation.Results;

namespace Wheels.Infrastructure.UseCases;

public class SharedCarNetworkUseCase : ISharedCarNetworkUseCase
{

    private readonly ISharedCarNetworkRepository _repository;
    private readonly SharedCarNetworkReComputer _reComputer;
    private readonly ReComputeNetworkValidator _validator;

    public SharedCarNetworkUseCase(
        ISharedCarNetworkRepository repository,
        SharedCarNetworkReComputer reComputer
    )
    {
        _repository = repository;
        _reComputer = reComputer;
        _validator = new ReComputeNetworkValidator();
    }


    public Task<SharedCarNetwork[]> GetAll()
    {
        return _repository.GetAll();
    }

    public Task<SharedCarNetwork?> GetById(string uuid)
    {
        return _repository.GetById(uuid);
    }

    public async Task<SharedCarNetwork> AddPassengerToNetwork(
        ReComputeNetworkDTO reComputeNetworkDTO
        )
    {
        ValidationResult result = await _validator.ValidateAsync(reComputeNetworkDTO);
        if (!result.IsValid)
        {
            throw new DomainException(result.ToString("~"), ErrorCode.InvalidInput);
        }

        List<NetworkNode> passengers =
            reComputeNetworkDTO
            .SharedCarNetworkDTO.Passengers.Select(
                node => (NetworkNode)node
            ).ToList();

        var sharedCarNetwork = new SharedCarNetwork(
            reComputeNetworkDTO.SharedCarNetworkDTO.Uuid,
            reComputeNetworkDTO.SharedCarNetworkDTO.Destination,
            reComputeNetworkDTO.SharedCarNetworkDTO.Driver,
            passengers,
            reComputeNetworkDTO.SharedCarNetworkDTO.Edges
        );

        var network = await _reComputer.AddNewPassanger(
            sharedCarNetwork,
            reComputeNetworkDTO.Passenger
        );

        return network;
    }

    public Task<SharedCarNetwork> RemovePassengerToNetwork(
        ReComputeNetworkDTO reComputeNetworkDTO
    )
    {
        throw new NotImplementedException();

    }

}
