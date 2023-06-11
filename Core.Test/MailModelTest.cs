using Core.Models;

namespace Core.Test;

public class MailModelTest
{
    public class MailTests
    {
        [Fact]
        public void Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            int id = 1;
            string number = "123";
            DateTime dateInput = DateTime.Now;
            DateTime? dateAnswer = DateTime.Now.AddDays(1);
            string theme = "Test Mail";
            User responsible = new User();
            Project project = new Project();
            Sender sender = new Sender();
            OutgoingMail outgoingMail = new OutgoingMail();

            // Act
            Mail mail = new Mail(id, number, dateInput, dateAnswer, theme, responsible, project, sender, outgoingMail);

            // Assert
            Assert.Equal(id, mail.Id);
            Assert.Equal(number, mail.Number);
            Assert.Equal(dateInput, mail.DateInput);
            Assert.Equal(dateAnswer, mail.DateAnswer);
            Assert.Equal(theme, mail.Theme);
            Assert.Equal(responsible, mail.Responsible);
            Assert.Equal(project, mail.Project);
            Assert.Equal(sender, mail.Sender);
            Assert.Equal(outgoingMail, mail.OutgoingMail);
        }

        [Fact]
        public void IsMailDone_CanBeSetAndRetrieved()
        {
            // Arrange
            Mail mail = new Mail();

            // Act
            mail.IsMailDone = true;

            // Assert
            Assert.True(mail.IsMailDone);
        }
    }
}