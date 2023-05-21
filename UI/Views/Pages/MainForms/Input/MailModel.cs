using System.ComponentModel;

namespace UI.Views.Pages.MainForms.Input;


//todo: перенос в Core
public class MailModel
{
    public int IdMail { get; set; }
    
    [DisplayName("Номер письма")]
    public string NumberMail { get; set; }
    [DisplayName("Дата получения")]
    public DateTime DateInput { get; set; }
    [DisplayName("Проект")]
    public string Project { get; set; }
    [DisplayName("Отправитель")]
    public string Sender { get; set; }
    [DisplayName("Тема")]
    public string Theme { get; set; }
    [DisplayName("Номер исх.")]
    public string NumberOut { get; set; }
    [DisplayName("Срок ответа")]
    public DateTime DateOut { get; set; }

    public DirectoryInfo PathFolder { get; set; }
}