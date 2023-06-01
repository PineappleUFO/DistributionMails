using Core.Models;
using System.Data;


namespace Postgres.Test
{
    public class DepRepositoryTest
    {
        public interface IDatabaseConnection
        {
            IDbConnection GetConnection();
        }

            private DepRepository depRepository;
        //todo: доделать тесты
            public DepRepositoryTest()
            {
            depRepository = null; 
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