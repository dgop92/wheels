namespace Wheels.Domain.Entities;

public record Location(double Latitude, double Longitude);

public record PathInfo(double Distance, double EstimatedTime);

public enum UserType
{
    Driver,
    Passenger
}

public record User(string UserId, UserType UserType);
