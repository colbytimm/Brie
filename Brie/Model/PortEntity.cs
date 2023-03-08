namespace Brie.Model;

public record PortEntity
{
    public string? Pid { get; set; }
    public string? Port { get; set; }
    public string? User { get; set; }
    public string? Command { get; set; }

    public override string ToString()
    {
        return $"Port: {Port} ({Pid})\n  User: {User}, Command: {Command}";
    }
}