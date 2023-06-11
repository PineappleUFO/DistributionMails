using Core.Models;

namespace Core.Test;

public class ProjectModelTest
{
    [Fact]
    public void Constructor_WithParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        int id = 1;
        string name = "Project 1";
        string color = "Blue";

        // Act
        Project project = new Project(id, name, color);

        // Assert
        Assert.Equal(id, project.Id);
        Assert.Equal(name, project.Name);
        Assert.Equal(color, project.Color);
    }

    [Fact]
    public void Constructor_WithoutParameters_SetsPropertiesToDefaultValues()
    {
        // Arrange & Act
        Project project = new Project();

        // Assert
        Assert.Equal(0, project.Id); // Assuming 0 is the default value for Id
        Assert.Null(project.Name);
        Assert.Null(project.Color);
    }

    [Fact]
    public void Name_Property_CanBeSetAndRetrieved()
    {
        // Arrange
        string name = "Project 1";
        Project project = new Project();

        // Act
        project.Name = name;

        // Assert
        Assert.Equal(name, project.Name);
    }

    [Fact]
    public void Color_Property_CanBeSetAndRetrieved()
    {
        // Arrange
        string color = "Blue";
        Project project = new Project();

        // Act
        project.Color = color;

        // Assert
        Assert.Equal(color, project.Color);
    }
}
