using Wheels.Domain.Entities;

namespace Wheels.Domain.Dtos;

public class ReComputeNetworkDTO
{
    public SharedCarNetworkDTO SharedCarNetworkDTO { get; init; }

    public UserNode Passenger { get; init; }

    public ReComputeNetworkDTO(
        SharedCarNetworkDTO sharedCarNetworkDTO,
        UserNode passenger
    )
    {
        SharedCarNetworkDTO = sharedCarNetworkDTO;
        Passenger = passenger;
    }

}

