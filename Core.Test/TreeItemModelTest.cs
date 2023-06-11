using Core.Models;

namespace Core.Test;

public class TreeItemModelTest
{
    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        string name = "Test TreeItem";
        bool isExpanded = true;
        DateTime? deadline = DateTime.Now.AddDays(7);
        User user = new User();
        Status status = new Status();
        string prefixStatus = "$";
        string prefixStatusColor = "Red";
        int mailId = 1;
        int upId = 2;
        bool isResponsible = true;
        bool isReplying = false;
        string resolution = "Completed";
        string log = "Some log";
        DateTime? dateAdd = DateTime.Now;

        // Act
        TreeItem treeItem = new TreeItem(name);
        treeItem.IsExpanded = isExpanded;
        treeItem.Deadline = deadline;
        treeItem.User = user;
        treeItem.Status = status;
        treeItem.PrefixStatus = prefixStatus;
        treeItem.PrefixStatusColor = prefixStatusColor;
        treeItem.MailId = mailId;
        treeItem.UpId = upId;
        treeItem.IsResponsible = isResponsible;
        treeItem.IsReplying = isReplying;
        treeItem.Resolution = resolution;
        treeItem.Log = log;
        treeItem.DateAdd = dateAdd;

        // Assert
        Assert.Equal(name, treeItem.Name);
        Assert.Equal(isExpanded, treeItem.IsExpanded);
        Assert.Equal(deadline, treeItem.Deadline);
        Assert.Equal(user, treeItem.User);
        Assert.Equal(status, treeItem.Status);
        Assert.Equal(prefixStatus, treeItem.PrefixStatus);
        Assert.Equal(prefixStatusColor, treeItem.PrefixStatusColor);
        Assert.Equal(mailId, treeItem.MailId);
        Assert.Equal(upId, treeItem.UpId);
        Assert.Equal(isResponsible, treeItem.IsResponsible);
        Assert.Equal(isReplying, treeItem.IsReplying);
        Assert.Equal(resolution, treeItem.Resolution);
        Assert.Equal(log, treeItem.Log);
        Assert.Equal(dateAdd, treeItem.DateAdd);
    }
}