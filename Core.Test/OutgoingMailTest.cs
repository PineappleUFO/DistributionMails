using Core.Models;

namespace Core.Test;

public class OutgoingMailTest
{
    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        int id = 1;
        string number = "123";
        DateTime dateExport = DateTime.Now;
        DateTime dateAnswer = DateTime.Now.AddDays(1);
        string theme = "Test Outgoing Mail";
        string text = "This is a test outgoing mail";
        Sender sender = new Sender();
        DirectoryInfo pathFolder = new DirectoryInfo(".");
        Project project = new Project();

        // Act
        OutgoingMail outgoingMail = new OutgoingMail(id, number, dateExport, dateAnswer, theme, text);
        outgoingMail.Sender = sender;
        outgoingMail.PathFolder = pathFolder;
        outgoingMail.Project = project;

        // Assert
        Assert.Equal(id, outgoingMail.Id);
        Assert.Equal(number, outgoingMail.Number);
        Assert.Equal(dateExport, outgoingMail.DateExport);
        Assert.Equal(dateAnswer, outgoingMail.DateAnswer);
        Assert.Equal(theme, outgoingMail.Theme);
        Assert.Equal(text, outgoingMail.Text);
        Assert.Equal(sender, outgoingMail.Sender);
        Assert.Equal(pathFolder, outgoingMail.PathFolder);
        Assert.Equal(project, outgoingMail.Project);
    }
}