using Core.Models;

namespace Core.Test;

public class UserModelTest
{

    [Fact]
    public void IsHasAccessToOneLevel_ReturnsTrueForAllowedPositions()
    {
        // Arrange
        Position allowedPosition1 = new Position() { Id = 17 };
        Position allowedPosition2 = new Position { Id = 15 };
        Position allowedPosition3 = new Position { Id = 16 };
        User user = new User();
        user.Position = allowedPosition1;

        // Act
        bool hasAccessToOneLevel = user.IsHasAccessToOneLevel;

        // Assert
        Assert.True(hasAccessToOneLevel);

        // Arrange
        user.Position = allowedPosition2;

        // Act
        hasAccessToOneLevel = user.IsHasAccessToOneLevel;

        // Assert
        Assert.True(hasAccessToOneLevel);

        // Arrange
        user.Position = allowedPosition3;

        // Act
        hasAccessToOneLevel = user.IsHasAccessToOneLevel;

        // Assert
        Assert.True(hasAccessToOneLevel);
    }

    [Fact]
    public void IsHasAccessToOneLevel_ReturnsFalseForNotAllowedPositions()
    {
        // Arrange
        Position notAllowedPosition = new Position { Id = 10 };
        User user = new User();
        user.Position = notAllowedPosition;

        // Act
        bool hasAccessToOneLevel = user.IsHasAccessToOneLevel;

        // Assert
        Assert.False(hasAccessToOneLevel);
    }
}