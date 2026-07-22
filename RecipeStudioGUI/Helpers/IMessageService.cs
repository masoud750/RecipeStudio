using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RecipeStudio.UI.Helpers
{
    public interface IMessageService
    {
      
        public void ShowMessage(string message, MessageType type, string? caption = null);
        public bool   ShowConfirmation(string message, string caption = "Confirm");
    }

    public class MessageBoxService : IMessageService
    {
        public void ShowMessage(string message, MessageType type, string? caption = null)
        {
            string finalCaption = caption ?? type.ToString().ToUpperInvariant();

            MessageBoxImage icon = MessageBoxImage.None;
            switch (type)
            {
                case MessageType.Info:
                    icon = MessageBoxImage.Information;
                    break;
                case MessageType.Warning:
                    icon = MessageBoxImage.Warning;
                    break;
                case MessageType.Error:
                    icon = MessageBoxImage.Error;
                    break;
            }

            System.Windows.MessageBox.Show(message, finalCaption,
                MessageBoxButton.OK, icon);
        }
        public bool ShowConfirmation(string message, string caption = "Confirm")
        {
            var result = System.Windows.MessageBox.Show(message, caption,
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

       
    }

    public enum MessageType
    {
        Info,
        Warning,
        Error
    }
}
