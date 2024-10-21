using Avalonia;
using Avalonia.Markup.Xaml;
using Metallic.Avalonia.ViewModels;


namespace Metallic.Avalonia;

public abstract class MvvmApplication<VM> : Application where VM : ApplicationVm {

	VM appVm;

	public override void Initialize() => AvaloniaXamlLoader.Load(this);

	public override async void OnFrameworkInitializationCompleted() {
		base.OnFrameworkInitializationCompleted();
		appVm = await CreateAppVm();
	}

	protected abstract Task<VM> CreateAppVm();
}