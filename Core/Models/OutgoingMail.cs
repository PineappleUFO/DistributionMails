namespace Core.Models;

public class OutgoingMail:Entity
{

    public OutgoingMail()
    {
        
    }
    public OutgoingMail(int id, string number, DateTime dateExport, DateTime dateAnswer, string theme, string text) : base(id)
    {
        Id = id;
        Number = number;
        DateExport = dateExport;
        DateAnswer = dateAnswer;
        Theme = theme;
        Text = text;
    }

    public string Number { get; set; }
    public DateTime DateExport { get; set; }
    public DateTime? DateAnswer { get; set; }
    public string Theme { get; set; }
    public string Text { get; set; }
    public Sender Sender { get; set; }
    public DirectoryInfo PathFolder { get; set; }
    public Project Project { get; set; }
}