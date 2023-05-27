namespace Core.Models;

/// <summary>
/// Представление базовой сущности доменной модели
/// </summary>
public abstract class Entity
{
    public int Id { get; }
    
    protected Entity() { }

    protected Entity(int id)
    {
        Id = id;
    }
    // Методы сравнения сущностей по их идентификатору
    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}