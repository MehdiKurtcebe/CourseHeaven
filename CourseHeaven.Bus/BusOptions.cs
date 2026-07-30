namespace CourseHeaven.Bus;

public class BusOptions
{
    public required string Address { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required int Port { get; set; }
}