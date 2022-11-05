using Wheels.Domain.Entities;

namespace Wheels.Domain.Services;

public interface IPathService
{
    Task<PathInfo> GetPathInfo(Location origin, Location destination);
}