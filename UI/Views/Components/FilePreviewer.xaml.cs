using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UI.Helpers;

namespace UI.Views.Components;


public partial class FilePreviewer : ContentView
{

    public static readonly BindableProperty FilePathProperty =
        BindableProperty.Create (nameof(FilePath), typeof(string), typeof(FilePreviewer), string.Empty);
    
    
    public string FilePath
    {
        get => (string)GetValue(FilePathProperty);
        set
        {
            SetValue(FilePathProperty, value); 
            BindingContext = new FileViewerViewModel(FilePath);
        }
    }

    public FilePreviewer()
    {
        InitializeComponent();
     
    }


    
}