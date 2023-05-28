using System.Drawing;

namespace Core.Models;

public class Project:Entity
{
    public Project(int id, string name, Color color) : base(id)
    {
        Id = id;
        Name = name;
        Color = color;
    }

    public Project()
    {
        
    }
    public string Name { get; set; }
    public Color Color { get; set; }
    
}