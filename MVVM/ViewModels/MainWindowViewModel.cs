using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ISFA.MVVM.ViewModels
{
	public class MainWindowViewModel : ReactiveObject
	{
		#region Properties

		[Reactive]
		public bool IsMainViewOpen { get; set; } = true;

		[Reactive]
		public bool IsHelpViewOpen { get; set; }

		#endregion
	}
}
