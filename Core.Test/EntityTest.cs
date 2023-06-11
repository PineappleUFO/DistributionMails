using Core.Models;

namespace Core.Test;

public class EntityTest
{
     public class EntityTests
    {
        [Fact]
        public void Equals_ReturnsTrue_WhenEntitiesHaveSameId()
        {
            // Arrange
            int id = 1;
            Entity entity1 = new TestEntity(id);
            Entity entity2 = new TestEntity(id);

            // Act
            bool result = entity1.Equals(entity2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Equals_ReturnsFalse_WhenEntitiesHaveDifferentIds()
        {
            // Arrange
            Entity entity1 = new TestEntity(1);
            Entity entity2 = new TestEntity(2);

            // Act
            bool result = entity1.Equals(entity2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Equals_ReturnsFalse_WhenComparingWithNonEntityObject()
        {
            // Arrange
            Entity entity = new TestEntity(1);
            object otherObject = new object();

            // Act
            bool result = entity.Equals(otherObject);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetHashCode_ReturnsSameHashCode_ForEntitiesWithSameId()
        {
            // Arrange
            int id = 1;
            Entity entity1 = new TestEntity(id);
            Entity entity2 = new TestEntity(id);

            // Act
            int hashCode1 = entity1.GetHashCode();
            int hashCode2 = entity2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void GetHashCode_ReturnsDifferentHashCode_ForEntitiesWithDifferentIds()
        {
            // Arrange
            Entity entity1 = new TestEntity(1);
            Entity entity2 = new TestEntity(2);

            // Act
            int hashCode1 = entity1.GetHashCode();
            int hashCode2 = entity2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }

        // Helper class for testing Entity
        private class TestEntity : Entity
        {
            public TestEntity(int id) : base(id)
            {
            }
        }
    }
}