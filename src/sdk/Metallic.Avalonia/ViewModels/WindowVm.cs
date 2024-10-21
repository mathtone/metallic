using System.Collections.ObjectModel;

namespace Metallic.Avalonia.ViewModels;

public class WindowVm : ControlVm {
	readonly ObservableCollection<ControlVm> childWindows = [];

	public ControlVm? ParentWindow { get; }
	public ReadOnlyObservableCollection<ControlVm> ChildWindows { get; }

	public WindowVm(WindowVm? parentWindow = null) {
		ParentWindow = parentWindow;
		ChildWindows = new(childWindows);
	}
}
