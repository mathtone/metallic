using Metallic.Mvvm.Reactive;
using System.Collections.ObjectModel;

namespace Metallic.Avalonia.ViewModels;

public class ApplicationVm : ReactiveViewModel {

	readonly ObservableCollection<WindowVm> appWindows = [];

	public ReadOnlyObservableCollection<WindowVm> Windows { get; }

	public ApplicationVm() {
		Windows = new(appWindows);
	}
}