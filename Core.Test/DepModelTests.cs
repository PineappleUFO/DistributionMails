using Core.Models;

namespace Core.Test
{
    public class DepModelTests
    {
        [Fact]
        public void Constructor_SetsPropertiesCorrectly()
        {
            // Arrange
            int id = 1;
            string name = "Department 1";
            string about = "About Department 1";

            // Act
            Dep dep = new Dep(id, name, about);

            // Assert
            Assert.Equal(id, dep.Id);
            Assert.Equal(name, dep.Name);
            Assert.Equal(about, dep.About);
        }

        [Fact]
        public void Name_Property_CanBeSetAndRetrieved()
        {
            // Arrange
            string name = "Department 1";
            Dep dep = new Dep(1, "", "");

            // Act
            dep.Name = name;

            // Assert
            Assert.Equal(name, dep.Name);
        }

        [Fact]
        public void About_Property_CanBeSetAndRetrieved()
        {
            // Arrange
            string about = "About Department 1";
            Dep dep = new Dep(1, "", "");

            // Act
            dep.About = about;

            // Assert
            Assert.Equal(about, dep.About);
        }
    }
}
