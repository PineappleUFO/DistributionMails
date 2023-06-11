using Core.Models;

namespace Core.Test;

public class ChatModelTest
{
    public class ChatTests
    {
        [Fact]
        public void Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            User user = new User();
            DateTime messageDate = DateTime.Now;
            string message = "Hello, world!";

            // Act
            Chat chat = new Chat
            {
                User = user,
                MessageDate = messageDate,
                Message = message
            };

            // Assert
            Assert.Equal(user, chat.User);
            Assert.Equal(messageDate, chat.MessageDate);
            Assert.Equal(message, chat.Message);
        }
    }
}