namespace Core.Models;

public class Dep:Entity
{
    public string Name { get; set; }
    public string About { get; set; }
    
    public Dep(int id,string name, string about)
    {
        Id = id;
        Name = name;
        About = about;
    }

}