namespace Wheels.Domain.Entities;

public class NetworkEdge
{
    public NetworkNode From { get; init; }
    public NetworkNode To { get; init; }
    public PathInfo PathInfo { get; init; }

    public NetworkEdge(NetworkNode from, NetworkNode to, PathInfo pathInfo)
    {
        From = from;
        To = to;
        PathInfo = pathInfo;
    }

    public bool ContainsNode(NetworkNode node)
    {
        if (From == node || To == node)
        {
            return true;
        }
        return false;
    }
}
