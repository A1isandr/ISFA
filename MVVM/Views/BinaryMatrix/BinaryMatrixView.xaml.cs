using System.Reactive.Disposables;
using System.Windows.Controls;
using ISFA.MVVM.ViewModels.BinaryMatrix;
using ReactiveUI;

namespace ISFA.MVVM.Views.BinaryMatrix
{
    /// <summary>
    /// Логика взаимодействия для BinaryMatrixView.xaml
    /// </summary>
    public partial class BinaryMatrixView : ReactiveUserControl<BinaryMatrixViewModel>
	{
		public BinaryMatrixView()
		{
			InitializeComponent();

			ViewModel = BinaryMatrixViewModel.Instance;

			this.WhenActivated(disposables =>
			{
				this.OneWayBind(ViewModel,
						viewModel => viewModel.ColumnHeaders,
						view => view.ColumnHeaders.ItemsSource)
					.DisposeWith(disposables);

				this.OneWayBind(ViewModel,
						viewModel => viewModel.RowHeaders,
						view => view.RowHeaders.ItemsSource)
					.DisposeWith(disposables);

				this.OneWayBind(ViewModel,
						viewModel => viewModel.BinaryMatrix,
						view => view.Columns.ItemsSource)
					.DisposeWith(disposables);
			});
		}

		private void TableScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			if (sender is not ScrollViewer scrollViewer) return;

			ColumnHeadersScrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset);
			RowHeadersScrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset);
		}
	}
}
