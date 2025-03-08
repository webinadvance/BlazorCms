namespace BlazorApp2.Data.Models;
public class ResourceType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Resource> Resources { get; set; } = new();
}