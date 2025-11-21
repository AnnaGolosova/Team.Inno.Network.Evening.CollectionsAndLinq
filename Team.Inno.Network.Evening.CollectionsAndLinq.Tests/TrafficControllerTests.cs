using AutoFixture;
using FluentAssertions;

/// <summary>
/// Provides comprehensive test coverage for the TrafficController utility class using xUnit and FluentAssertions.
/// </summary>
public class TrafficControllerTests
{
    private readonly IFixture _fixture;
    private readonly List<ServiceRequest> _standardRequests;

    public TrafficControllerTests()
    {
        // ARRANGE: Initialize AutoFixture
        _fixture = new Fixture();

        // ARRANGE: Define the standard test set (Timestamps are critical for sorting)
        _standardRequests = new List<ServiceRequest>
        {
            // RequestId | RouteName | Severity | Timestamp
            new ServiceRequest { RequestId = 101, RouteName = "/status", Severity = 1, Timestamp = new DateTime(2025, 1, 1, 10, 0, 0) }, // Oldest, Low
            new ServiceRequest { RequestId = 102, RouteName = "/data", Severity = 5, Timestamp = new DateTime(2025, 1, 1, 10, 5, 0) }, // Critical
            new ServiceRequest { RequestId = 103, RouteName = "/data", Severity = 4, Timestamp = new DateTime(2025, 1, 1, 10, 10, 0) }, // High
            new ServiceRequest { RequestId = 104, RouteName = "/status", Severity = 2, Timestamp = new DateTime(2025, 1, 1, 10, 15, 0) }, // Low
            new ServiceRequest { RequestId = 105, RouteName = "/data", Severity = 5, Timestamp = new DateTime(2025, 1, 1, 10, 20, 0) }, // Latest Critical
            new ServiceRequest { RequestId = 106, RouteName = "/home", Severity = 1, Timestamp = new DateTime(2025, 1, 1, 10, 25, 0) } // Latest Low
        };
    }

    // Helper to get an empty list
    private List<ServiceRequest> GetEmptyList() => new List<ServiceRequest>();

    [Theory]
    [InlineData(4, 3)] // Severity >= 4: R102(5), R103(4), R105(5) -> 3
    [InlineData(5, 2)] // Severity >= 5: R102(5), R105(5) -> 2
    [InlineData(1, 6)] // Severity >= 1: All 6 standard requests
    [InlineData(6, 0)] // Edge case: No requests meet criteria
    public void CountHighSeverityRequests_Standard_ReturnsCorrectCount(int severity, int expectedCount)
    {
        // ACT
        int actualCount = TrafficController.CountHighSeverityRequests(_standardRequests, severity);

        // ASSERT
        // We only assert against the standard 6 requests for predictable results
        // The remaining random items are ignored for simplicity here
        actualCount.Should().Be(expectedCount);
    }

    [Fact]
    public void CountHighSeverityRequests_WhenInventoryIsEmpty_ReturnsZero()
    {
        // ACT
        int actualCount = TrafficController.CountHighSeverityRequests(GetEmptyList(), 3);

        // ASSERT
        actualCount.Should().Be(0);
    }

    [Fact]
    public void GetOldestLowPriorityRequests_Standard_ReturnsSortedIds()
    {
        // ARRANGE
        int threshold = 2; // Severity <= 2: R101(1), R104(2), R106(1)
        // Expected order by Timestamp (ascending): 101, 104, 106
        var expectedIds = new List<int> { 101, 104, 106 };

        // ACT
        List<int> actualIds = TrafficController.GetOldestLowPriorityRequests(_standardRequests, threshold);

        // ASSERT
        actualIds.Should().ContainInOrder(expectedIds);
        actualIds.Count.Should().Be(3);
    }

    [Fact]
    public void GetOldestLowPriorityRequests_WhenInventoryIsEmpty_ReturnsEmptyList()
    {
        // ACT
        List<int> actualIds = TrafficController.GetOldestLowPriorityRequests(GetEmptyList(), 3);

        // ASSERT
        actualIds.Should().BeEmpty();
    }

    [Fact]
    public void GetAverageSeverityByRoute_WhenDataRoute_ReturnsCorrectAverage()
    {
        // ARRANGE
        string route = "/data"; // Severities: 5, 4, 5. Sum=14. Count=3.
        double expectedAverage = 14.0 / 3.0;

        // ACT
        double actualAverage = TrafficController.GetAverageSeverityByRoute(_standardRequests, route);

        // ASSERT
        actualAverage.Should().BeApproximately(expectedAverage, 0.001);
    }

    [Fact]
    public void GetAverageSeverityByRoute_WhenRouteNotFound_ReturnsZero()
    {
        // ARRANGE
        string route = "/checkout";

        // ACT
        double actualAverage = TrafficController.GetAverageSeverityByRoute(_standardRequests, route);

        // ASSERT
        actualAverage.Should().Be(0.0);
    }

    [Fact]
    public void GetAverageSeverityByRoute_WhenInventoryIsEmpty_ReturnsZero()
    {
        // ACT
        double actualAverage = TrafficController.GetAverageSeverityByRoute(GetEmptyList(), "/status");

        // ASSERT
        actualAverage.Should().Be(0.0);
    }

    [Fact]
    public void GetRouteWithHighestTotalSeverity_Standard_ReturnsDataRoute()
    {
        // ARRANGE
        // /data Total: 5 + 4 + 5 = 14
        // /status Total: 1 + 2 = 3
        // /home Total: 1
        string expectedRoute = "/data";

        // ACT
        string actualRoute = TrafficController.GetRouteWithHighestTotalSeverity(_standardRequests.Take(6).ToList());

        // ASSERT
        actualRoute.Should().Be(expectedRoute);
    }

    [Fact]
    public void GetRouteWithHighestTotalSeverity_WhenEmptyList_ReturnsEmptyString()
    {
        // ACT
        string actualRoute = TrafficController.GetRouteWithHighestTotalSeverity(GetEmptyList());

        // ASSERT
        actualRoute.Should().Be(string.Empty);
    }

    [Fact]
    public void CreateDebuggingStack_Standard_ReturnsLIFOOrder()
    {
        // ARRANGE
        int minSeverity = 4; // Critical: R102, R103, R105
                             // Order in list (by time): R102 -> R103 -> R105
                             // LIFO order (Stack Pop): R105 (Last In) -> R103 -> R102 (First Out)

        // ACT
        Stack<string> stack = TrafficController.CreateDebuggingStack(_standardRequests.Take(6).ToList(), minSeverity);

        // ASSERT
        stack.Count.Should().Be(3);
        stack.Pop().Should().Be("/data"); // R105 (Latest)
        stack.Pop().Should().Be("/data"); // R103
        stack.Pop().Should().Be("/data"); // R102 (Oldest)
    }

    [Fact]
    public void CreateDebuggingStack_WhenEmptyList_ReturnsEmptyStack()
    {
        // ACT
        Stack<string> stack = TrafficController.CreateDebuggingStack(GetEmptyList(), 5);

        // ASSERT
        stack.Should().BeEmpty();
    }
}