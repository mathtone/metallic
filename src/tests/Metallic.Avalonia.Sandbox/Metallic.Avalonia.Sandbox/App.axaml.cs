using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Metallic.Avalonia.Sandbox.Views;

namespace Metallic.Avalonia.Sandbox;

public partial class App : Application {

	public override void Initialize() => AvaloniaXamlLoader.Load(this);

	public override void OnFrameworkInitializationCompleted() {
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
			desktop.MainWindow = new MainWindow {
				//DataContext = new ApplicationVm()
			};
		}
		else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform) {
			singleViewPlatform.MainView = new MainView {
				//DataContext = new ApplicationVm()
			};
		}

		base.OnFrameworkInitializationCompleted();
	}
}