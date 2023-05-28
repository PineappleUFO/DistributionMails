

namespace Core.Models;

public class Project:Entity
{
    public Project(int id, string name, string color) : base(id)
    {
        Id = id;
        Name = name;
        Color = color;
    }

    public Project()
    {
        
    }
    public string Name { get; set; }
    public string Color { get; set; }
    
}