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
                Assert.Equal("����� ���������", dep.Name);
                Assert.Equal("�������� �� ���������� ��������� � �������� ����������������� ���������", dep.About);
            }

        
    }
}