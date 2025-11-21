/// <summary>
/// Represents a service request received by the system, often used for API logging or message queuing.
/// </summary>
public class ServiceRequest
{
    /// <summary>
    /// Gets or sets the unique ID of the request (for Dictionary lookup).
    /// </summary>
    public int RequestId { get; set; }

    /// <summary>
    /// Gets or sets the endpoint/route name (e.g., "/api/status", "/api/data").
    /// </summary>
    public string RouteName { get; set; }

    /// <summary>
    /// Gets or sets the severity level of the request or associated alert (1=Low, 5=Critical).
    /// </summary>
    public int Severity { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the request was logged.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Returns a string representation of the request.
    /// </summary>
    public override string ToString() => $"[ID:{RequestId}] {RouteName} (Severity: {Severity})";
}