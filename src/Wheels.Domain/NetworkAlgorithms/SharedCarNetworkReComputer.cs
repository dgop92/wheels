using Wheels.Domain.Entities;
using Wheels.Domain.Services;
using SharedKernel.Utils;

namespace Wheels.Domain.Application.NetworkAlgorithms;

public class SharedCarNetworkReComputer
{

    private IPathService _pathService;
    /*
    Main path service may fail, so we use a backup path service.
    (e.g. Directions API may fail because in some locations there is not enough data 
    to compute a path, so we use Euclidian distance that always works)
    */
    private IPathService _fallBackPathService;

    public SharedCarNetworkReComputer(
        IPathService pathService,
        IPathService fallBackPathService
    )
    {
        _pathService = pathService;
        _fallBackPathService = fallBackPathService;
    }

    public async Task<SharedCarNetwork> AddNewPassanger(
        SharedCarNetwork sharedCarNetwork,
        NetworkNode newPassanger
    )
    {
        // A shallow copy is enough because edges and nodes are immutable
        List<NetworkEdge> edges = new List<NetworkEdge>(sharedCarNetwork.Edges);
        List<NetworkNode> nodes = new List<NetworkNode>(sharedCarNetwork.Passengers);

        // Connect each passannger to the new user
        foreach (var node in nodes)
        {
            var networkEdge = await CreateNetworkEdge(
                newPassanger,
                node
            );
            edges.Add(networkEdge);
        }

        NetworkEdge destinationEdge = await CreateNetworkEdge(
            newPassanger,
            sharedCarNetwork.Destination
        );
        NetworkEdge driverEdge = await CreateNetworkEdge(
            newPassanger,
            sharedCarNetwork.Driver
        );

        edges.Add(destinationEdge);
        edges.Add(driverEdge);

        nodes.Add(newPassanger);

        SharedCarNetwork newNetwork = new SharedCarNetwork(
            sharedCarNetwork.Uuid,
            sharedCarNetwork.Destination,
            sharedCarNetwork.Driver,
            nodes,
            edges
        );

        List<NetworkEdge> optimalPath = ComputeOptimalPath(newNetwork);
        newNetwork.OptimalPath = optimalPath;
        return newNetwork;
    }

    private List<NetworkEdge> ComputeOptimalPath(
        SharedCarNetwork sharedCarNetwork)
    {
        IEnumerable<IEnumerable<NetworkNode>> pathPermutations =
            sharedCarNetwork.Passengers.Permute();
        List<NetworkEdge> optimalPath = new List<NetworkEdge>();
        double globalMinDistance = Double.MaxValue;

        foreach (var currentPath in pathPermutations)
        {
            List<NetworkEdge> possiblePath = new List<NetworkEdge>();
            int n = currentPath.Count();
            NetworkEdge firstEdge = sharedCarNetwork.GetEdge(
                sharedCarNetwork.Driver,
                currentPath.First()
            );
            possiblePath.Add(firstEdge);

            for (int i = 0; i < n - 1; i++)
            {
                NetworkEdge edge = sharedCarNetwork.GetEdge(
                    currentPath.ElementAt(i),
                    currentPath.ElementAt(i + 1)
                );
                possiblePath.Add(edge);
            }

            NetworkEdge lastEdge = sharedCarNetwork.GetEdge(
                currentPath.Last(),
                sharedCarNetwork.Destination
            );
            possiblePath.Add(lastEdge);

            double currentDistance = possiblePath.Sum(e => e.PathInfo.Distance);
            if (currentDistance < globalMinDistance)
            {
                globalMinDistance = currentDistance;
                optimalPath = possiblePath;
            }
        }

        return optimalPath;
    }

    private async Task<NetworkEdge> CreateNetworkEdge(
        NetworkNode node1,
        NetworkNode node2
    )
    {
        PathInfo pathInfo;
        try
        {
            pathInfo = await _pathService.GetPathInfo(
                node1.Location, node2.Location
            );
        }
        catch (System.Exception)
        {
            pathInfo = await _fallBackPathService.GetPathInfo(
                node1.Location, node2.Location
            );
        }

        return new NetworkEdge(node1, node2, pathInfo);
    }
}
