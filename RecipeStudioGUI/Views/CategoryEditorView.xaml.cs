using RecipeStudio.UI.ViewModels;
using System.Windows;


namespace RecipeStudio.UI.Views
{
    /// <summary>
    /// Interaction logic for CategoryEditorView.xaml
    /// </summary>
    public partial class CategoryEditorView : Window
    {
        public CategoryEditorView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;

        }

        private void OnDataContextChanged(object sender, 
                                        DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is IValidatable validatable)
            {
                // Subscribe to event
                validatable.Done += (s, args) =>
                {                 
                        Close();                    
                };
            }
        }
    }
}
