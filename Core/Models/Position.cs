namespace Core.Models;

public class Position:Entity
{
    public Position(int id,string positionName)
    {
        Id = id;
        PositionName = positionName;
    }

    public string PositionName { get; set; }
    
}