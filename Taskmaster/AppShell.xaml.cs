namespace Taskmaster;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(Help), typeof(Help));

        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
    }
}
