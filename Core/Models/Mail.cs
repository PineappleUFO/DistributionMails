using System.ComponentModel;
using System.Runtime;

namespace Core.Models;

public class Mail:Entity
{
    public string Number { get; set; }
    public DateTime DateInput { get; set; }
    public DateTime? DateAnswer { get; set; }
    public string Theme { get; set; }
    public string Text { get; set; }
    public User? Responsible { get; set; }
    public Project Project { get; set; }
    public Sender Sender { get; set; }
    public OutgoingMail OutgoingMail { get; set; }

    //выполнено ли письмо
    public bool IsMailDone { get; set; }

    public MailType MailType { get; set; }

    public DirectoryInfo PathFolder { get; set; }

    public Mail()
    {
        
    }
    public Mail(int id, string number, DateTime dateInput, DateTime? dateAnswer, string theme, User? responsible, Project project, Sender sender, OutgoingMail outgoingMail) : base(id)
    {
        Id = id;
        Number = number;
        DateInput = dateInput;
        DateAnswer = dateAnswer;
        Theme = theme;
        Responsible = responsible;
        Project = project;
        Sender = sender;
        OutgoingMail = outgoingMail;
    }


    
}