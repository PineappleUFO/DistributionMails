﻿namespace Core.Models;

public class User:Entity
{
    public string Family { get; set; }
    public string Name { get;  set; }
    public string Surname { get;  set; }
    public string Login { get;  set; }
    public byte[]? Photo { get;  set; }
    public string Phone { get;  set; }
    public Dep? Department { get;  set; }
    public Position? Position { get;  set; }

    public string Inicials { get; set; }

    public User()
    {
        
    }
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