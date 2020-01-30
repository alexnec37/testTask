using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace TestApplicationSIBERS.Dialog
{
    public class DialogService : IDialogService
    {
        public void ShowMessage(string title, string message)
        {
            MessageBoxResult resultMB = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
