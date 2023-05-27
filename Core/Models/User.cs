﻿namespace Core.Models;

public class User:Entity
{
    public string Family { get; private set; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string Login { get; private set; }
    public byte[] Photo { get; private set; }
    public string Phone { get; private set; }
    //public int DepartmentId { get; private set; }
    //public int PositionId { get; private set; }
    
    public User(string family, string name, string surname, string login, byte[] photo, string phone)
    {
        Family = family;
        Name = name;
        Surname = surname;
        Login = login;
        Photo = photo;
        Phone = phone;
        //DepartmentId = departmentId;
        //PositionId = positionId;
    }
}