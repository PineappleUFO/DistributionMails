using Core.Models;
using EF.Repositories;
using System.Data;

namespace Postgres.Test
{
    public interface IDatabaseConnection
    {
        IDbConnection GetConnection();
    }
    public class DepRepositoryTest
    {
        private DepRepository depRepository;

        public DepRepositoryTest()
        {
            depRepository = new DepRepository("Host=localhost;Database=MailerAdmin;Username='zakharovdb';Password='zakharovdb'");
        }

        [Fact]
        public async Task GetDepByUserId_ValidUserId_ReturnsDep()
        {
            // Arrange
            int userId = 123;

            // Act
            Dep? dep = await depRepository.GetDepByUserId(userId);

            // Assert
            Assert.NotNull(dep);
            Assert.Equal(6, dep.Id);
            Assert.Equal("Отдел испытаний", dep.Name);
            Assert.Equal("Отвечает за проведение испытаний и проверку работоспособности продуктов", dep.About);
        }

    }
}

