using Wheels.Domain.Entities;
using Wheels.Domain.Services;
using Wheels.Domain.NetworkAlgorithms;
using SharedKernel.Errors;

namespace Domain.UnitTests.NetworkAlgorithms;

[TestClass]
public class SharedCarNetworkReComputerTest
{

    private static IPathService _pathService = default!;
    private static Location[] testLocations = {
        new Location(11.010931, -74.828367),
        new Location(11.010026, -74.826358),
        new Location(11.015369, -74.823037),
        new Location(11.008807, -74.809309),
    };
    private static User driverUser = new User("1", UserType.Driver);
    private static SharedCarNetworkReComputer _sharedCarNetworkReComputer = default!;

    private static SharedCarNetwork createEmptySharedCarNetwork()
    {
        NetworkNode driver = new UserNode(testLocations[0], driverUser);
        NetworkNode destination = new NetworkNode(testLocations[1]);

        return new SharedCarNetwork(destination, driver);
    }

    [ClassInitialize]
    public static void TestFixtureSetup(TestContext context)
    {
        _pathService = new EuclideanPathService();
        _sharedCarNetworkReComputer = new SharedCarNetworkReComputer(
            _pathService, _pathService
        );
    }

    [TestMethod]
    public async Task ShouldAddNewtPassengerInEmptyNetwork()
    {
        User passengerUser = new User("2", UserType.Passenger);
        NetworkNode passenger = new UserNode(testLocations[2], passengerUser);

        SharedCarNetwork network = createEmptySharedCarNetwork();
        SharedCarNetwork newNetwork =
            await _sharedCarNetworkReComputer.AddNewPassanger(network, passenger);

        Assert.AreEqual(1, newNetwork.Passengers.Count);
        Assert.AreEqual(2, newNetwork.Edges.Count);
    }

    [TestMethod]
    public async Task ShouldAddNewtPassengerInNetwork()
    {
        User passengerUser1 = new User("2", UserType.Passenger);
        User passengerUser2 = new User("3", UserType.Passenger);
        NetworkNode passenger1 = new UserNode(testLocations[2], passengerUser1);
        NetworkNode passenger2 = new UserNode(testLocations[3], passengerUser2);

        SharedCarNetwork network = createEmptySharedCarNetwork();
        SharedCarNetwork newNetwork1 =
            await _sharedCarNetworkReComputer.AddNewPassanger(network, passenger1);
        SharedCarNetwork newNetwork2 =
            await _sharedCarNetworkReComputer.AddNewPassanger(newNetwork1, passenger2);

        Assert.AreEqual(2, newNetwork2.Passengers.Count);
        Assert.AreEqual(5, newNetwork2.Edges.Count);
    }

    [TestMethod]
    public async Task ShouldThrowErrorMaximumPassengers()
    {

        // It doesn't matter for this unit test if the passengers has the same location,
        List<NetworkNode> passengers = Enumerable.Range(2, 6).Select(
            i => (NetworkNode)new UserNode(
                testLocations[2],
                new User(i.ToString(), UserType.Passenger)
            )
        ).ToList();

        // It doesn't matter for this unit test if the network has no edges
        SharedCarNetwork newNetwork = new SharedCarNetwork(
            Guid.NewGuid().ToString(),
            new NetworkNode(testLocations[1]),
            new UserNode(testLocations[0], driverUser),
            passengers,
            new List<NetworkEdge>()
        );

        await Assert.ThrowsExceptionAsync<DomainException>(
            async () => await _sharedCarNetworkReComputer.AddNewPassanger(
                newNetwork, passengers.First()
            )
        );
    }

    [TestMethod]
    public async Task ShouldComputeOptimalPathInEmptyNetwork()
    {
        User passengerUser = new User("2", UserType.Passenger);
        NetworkNode passenger = new UserNode(testLocations[2], passengerUser);

        SharedCarNetwork network = createEmptySharedCarNetwork();
        SharedCarNetwork newNetwork =
            await _sharedCarNetworkReComputer.AddNewPassanger(network, passenger);

        List<NetworkEdge> optimalPath = newNetwork.OptimalPath;
        Assert.AreEqual(2, optimalPath.Count);

        Assert.AreEqual(optimalPath[0].From, network.Driver);
        Assert.AreEqual(optimalPath[0].To, passenger);

        Assert.AreEqual(optimalPath[1].From, passenger);
        Assert.AreEqual(optimalPath[1].To, network.Destination);
    }

    [TestMethod]
    public async Task ShouldComputeOptimalPathInNetwork()
    {
        User passengerUser1 = new User("2", UserType.Passenger);
        User passengerUser2 = new User("3", UserType.Passenger);
        NetworkNode passenger1 = new UserNode(testLocations[2], passengerUser1);
        NetworkNode passenger2 = new UserNode(testLocations[3], passengerUser2);

        SharedCarNetwork network = createEmptySharedCarNetwork();
        SharedCarNetwork newNetwork1 =
            await _sharedCarNetworkReComputer.AddNewPassanger(network, passenger1);
        SharedCarNetwork newNetwork2 =
            await _sharedCarNetworkReComputer.AddNewPassanger(newNetwork1, passenger2);

        List<NetworkEdge> optimalPath = newNetwork2.OptimalPath;
        Assert.AreEqual(3, optimalPath.Count);

        Assert.AreEqual(optimalPath[0].From, network.Driver);
        Assert.AreEqual(optimalPath[0].To, passenger1);

        Assert.AreEqual(optimalPath[1].From, passenger1);
        Assert.AreEqual(optimalPath[1].To, passenger2);

        Assert.AreEqual(optimalPath[2].From, passenger2);
        Assert.AreEqual(optimalPath[2].To, network.Destination);
    }
}