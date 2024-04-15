using ISFA.MVVM.ViewModels.BinaryMatrix;
using ReactiveUI;
using System.Reactive.Disposables;

namespace ISFA.MVVM.Views.BinaryMatrix
{
	/// <summary>
	/// Логика взаимодействия для StateView.xaml
	/// </summary>
	public partial class StateView : ReactiveUserControl<StateViewModel>
	{
		public StateView()
		{
			InitializeComponent();

			this.WhenActivated(disposables =>
			{
				this.OneWayBind(ViewModel,
						viewModel => viewModel.Cells,
						view => view.Row.ItemsSource)
					.DisposeWith(disposables);
			});
		}
	}
}
