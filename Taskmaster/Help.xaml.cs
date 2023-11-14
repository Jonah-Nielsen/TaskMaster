using Taskmaster.ViewModel;

namespace Taskmaster;

public partial class Help : ContentPage
{
	public Help(HelpViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}