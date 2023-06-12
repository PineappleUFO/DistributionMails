using System.Data;
using EF.Interfaces;
using Npgsql;

namespace Postgres.Test;

using Moq;

public class DepRepositoryTest
{
   [Fact]
        public async Task GetDepByUserId_ValidUserId_ReturnsDep()
        {
            // Arrange
            int userId = 1;
            var mockConnectionString = new Mock<IConnectionString>();
            var mockConnection = new Mock<INpgsqlConnection>(MockBehavior.Strict);
            var mockCommand = new Mock<NpgsqlCommand>(MockBehavior.Strict);
            var mockReader = new Mock<NpgsqlDataReader>(MockBehavior.Strict);

            // Set up the mock objects
            mockConnectionString.Setup(c => c.TryGetConnetion()).Returns(mockConnection.Object);
            mockConnection.Setup(c => c.OpenAsync()).Returns(Task.CompletedTask);
            mockConnection.Setup(c => c.CreateCommand()).Returns(mockCommand.Object);
            mockCommand.SetupSet(c => c.CommandText = It.IsAny<string>());
            mockCommand.Setup(c => c.ExecuteReaderAsync(It.IsAny<CancellationToken>()))
                .Callback(() => Task.FromResult(mockReader.Object));

            mockReader.Setup(r => r.ReadAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            mockReader.Setup(r => r.GetInt32(0)).Returns(1);
            mockReader.Setup(r => r.GetString(1)).Returns("DepName");
            mockReader.Setup(r => r.GetString(2)).Returns("DepAbout");

            var depRepository = new DepRepository(mockConnectionString.Object);

            // Act
            var result = await depRepository.GetDepByUserId(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("DepName", result.Name);
            Assert.Equal("DepAbout", result.About);

            // Verify the interactions with the mock objects
            mockConnectionString.Verify(c => c.TryGetConnetion(), Times.Once);
            mockConnection.Verify(c => c.OpenAsync(), Times.Once);
            mockConnection.Verify(c => c.CreateCommand(), Times.Once);
            mockCommand.VerifySet(c => c.CommandText = $"select d.dep_id, dep_name, dep_about from deps d,users u where d.dep_id=u.id_dep and u.user_id='{userId}'", Times.Once);
            mockCommand.Verify(c => c.ExecuteReaderAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockReader.Verify(r => r.ReadAsync(It.IsAny<CancellationToken>()), Times.Once);
            mockReader.Verify(r => r.GetInt32(0), Times.Once);
            mockReader.Verify(r => r.GetString(1), Times.Once);
            mockReader.Verify(r => r.GetString(2), Times.Once);
            mockConnection.Verify(c => c.CloseAsync(), Times.Once);
        }
    }
}