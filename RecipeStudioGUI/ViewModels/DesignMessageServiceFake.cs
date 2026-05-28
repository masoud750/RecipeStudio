using RecipeStudio.UI.Helpers;

namespace RecipeStudio.UI.ViewModels
{
    internal class DesignMessageServiceFake : IMessageService
    {
        public bool ShowConfirmation(string message, string caption = "Confirm")
        {
            throw new NotImplementedException();
        }

        public void ShowMessage(string message, MessageType type, string? caption = null)
        {
            throw new NotImplementedException();
        }
    }
}