namespace Core.Models;

public class User:Entity
{
    public string Family { get; private set; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string Login { get; private set; }
    public byte[]? Photo { get; private set; }
    public string Phone { get; private set; }
    public Dep? Department { get; private set; }
    public Position? Position { get; private set; }
    
    public User(int id, string family, string name, string surname, string login, byte[]? photo, string phone, Dep? dep, Position? position)
    {
        Id = id;
        Family = family;
        Name = name;
        Surname = surname;
        Login = login;
        Photo = photo;
        Phone = phone;
        Department = dep;
        Position = position;
    }
}