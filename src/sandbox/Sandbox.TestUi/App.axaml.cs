using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Hosting;
using Sandbox.TestUi.ViewModels;
using Sandbox.TestUi.Views;

namespace Sandbox.TestUi {

	

	public partial class App : Application {
		public override void Initialize() {
			AvaloniaXamlLoader.Load(this);
		}
		
		public override void OnFrameworkInitializationCompleted() {
			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
				desktop.MainWindow = new MainWindow {
					DataContext = new MainWindowViewModel(),
				};
			}

			base.OnFrameworkInitializationCompleted();
		}
	}
}