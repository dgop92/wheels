using Wheels.Domain.Entities;

namespace Wheels.Domain.Services;

public class EuclideanPathService : IPathService
{

    private readonly double AVERAGE_SPEED_KMHOURS = 20;

    private double GetDistanceInMeters(
        double longitude,
        double latitude,
        double otherLongitude,
        double otherLatitude
    )
    {
        var d1 = latitude * (Math.PI / 180.0);
        var num1 = longitude * (Math.PI / 180.0);
        var d2 = otherLatitude * (Math.PI / 180.0);
        var num2 = otherLongitude * (Math.PI / 180.0) - num1;
        var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

        return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
    }

    /// <summary>
    ///  Returns the distance in meters and the expected time in minutes
    /// </summary>
    public Task<PathInfo> GetPathInfo(Location origin, Location destination)
    {
        var distance = GetDistanceInMeters(
            origin.Longitude,
            origin.Latitude,
            destination.Longitude,
            destination.Latitude
        );
        var estimatedTimeInMinutes = (distance / 1000 / AVERAGE_SPEED_KMHOURS) * 60;
        PathInfo pf = new PathInfo(distance, estimatedTimeInMinutes);
        return Task.FromResult(pf);
    }
}