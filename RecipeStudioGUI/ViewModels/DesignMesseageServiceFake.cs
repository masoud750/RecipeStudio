using RecipeStudioUI.Helpers;

namespace RecipeStudioUI.ViewModels
{
    internal class DesignMesseageServiceFake : IMessageService
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