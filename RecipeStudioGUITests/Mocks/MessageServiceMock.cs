using Microsoft.Extensions.Logging;
using RecipeStudio.UI.Helpers;

namespace RecipeStudioUI.Tests.Mocks
{
    internal class MessageServiceMock : IMessageService
    {
        public bool ShowConfirmation(string message, string caption = "Confirm")
        {
            return ConfirmationState;
        }

        public bool ConfirmationState { get; set; } 

        public void ShowMessage(string message, MessageType type, string? caption = null)
        {
            Console.WriteLine("Fake: " + message);
           
        }
    }

   
}
