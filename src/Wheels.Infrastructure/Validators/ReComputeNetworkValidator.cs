using FluentValidation;
using Wheels.Domain.Dtos;
using Wheels.Domain.Entities;

namespace Wheels.Infrastructure.Validators;
public class ReComputeNetworkValidator : AbstractValidator<ReComputeNetworkDTO>
{
    public ReComputeNetworkValidator()
    {
        RuleFor(network => network.Passenger).NotNull().SetValidator(
            new UserNodeValidator()
        );
        RuleFor(network => network.SharedCarNetworkDTO).NotNull().SetValidator(
            new SharedCarNetworkDTOValidator()
        );
    }
}

public class SharedCarNetworkDTOValidator : AbstractValidator<SharedCarNetworkDTO>
{
    public SharedCarNetworkDTOValidator()
    {
        RuleFor(network => network.Destination).NotNull().SetValidator(
            new NetworkNodeValidator()
        );
        RuleFor(network => network.Driver).NotNull().SetValidator(
            new UserNodeValidator()
        );
        RuleFor(network => network.Uuid).NotEmpty();
        RuleForEach(network => network.Passengers).SetValidator(
            new UserNodeValidator()
        );
        RuleForEach(network => network.Edges).SetValidator(
            new NetworkEdgeValidator()
        );
    }
}

public class NetworkEdgeValidator : AbstractValidator<NetworkEdge>
{
    public NetworkEdgeValidator()
    {
        RuleFor(edge => edge.From).NotNull().SetValidator(new NetworkNodeValidator());
        RuleFor(edge => edge.To).NotNull().SetValidator(new NetworkNodeValidator());
        RuleFor(edge => edge.PathInfo).NotNull().SetValidator(new PathInfoValidator());

    }
}


public class UserNodeValidator : AbstractValidator<UserNode>
{
    public UserNodeValidator()
    {
        RuleFor(userNode => userNode.User).NotNull().SetValidator(new UserValidator());
        RuleFor(userNode => userNode.Location).NotNull().SetValidator(
            new LocationValidator()
        );
        RuleFor(userNode => userNode.uuid).NotEmpty();
    }
}

public class NetworkNodeValidator : AbstractValidator<NetworkNode>
{
    public NetworkNodeValidator()
    {
        RuleFor(userNode => userNode.Location).NotNull().SetValidator(
            new LocationValidator()
        );
        RuleFor(networkNode => networkNode.uuid).NotEmpty();
    }
}

public class LocationValidator : AbstractValidator<Location>
{
    public LocationValidator()
    {
        RuleFor(location => location.Latitude).NotNull();
        RuleFor(location => location.Longitude).NotNull();
    }
}

public class PathInfoValidator : AbstractValidator<PathInfo>
{
    public PathInfoValidator()
    {
        RuleFor(pathInfo => pathInfo.Distance).NotNull();
        RuleFor(pathInfo => pathInfo.EstimatedTime).NotNull();
    }
}

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.UserId).NotEmpty();
        RuleFor(user => user.UserType).NotNull();
    }
}