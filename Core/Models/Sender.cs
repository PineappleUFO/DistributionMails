namespace Core.Models;

public class Sender:Entity
{
    public Sender()
    {
        
    }
    public Sender(int id, string name) : base(id)
    {
        Id = id;
        Name = name;
    }

    public string Name { get; set; }
}