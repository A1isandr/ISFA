using System.Reactive.Disposables;
using ISFA.MVVM.ViewModels.BinaryMatrix;
using ReactiveUI;

namespace ISFA.MVVM.Views.BinaryMatrix
{
	/// <summary>
	/// Логика взаимодействия для BinaryMatrixCell.xaml
	/// </summary>
	public partial class BinaryMatrixCellView : ReactiveUserControl<BinaryMatrixCellViewModel>
	{
		public BinaryMatrixCellView()
		{
			InitializeComponent();

			this.WhenActivated(disposables =>
			{
				this.OneWayBind(ViewModel,
						viewModel => viewModel.BinaryMatrixCell.Value,
						view => view.Value.Content)
					.DisposeWith(disposables);

				this.OneWayBind(ViewModel,
						viewModel => viewModel.ForegroundBrush,
						view => view.Value.Foreground)
					.DisposeWith(disposables);
			});
		}
	}
}
