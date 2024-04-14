using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ISFA.MVVM.ViewModels.BinaryMatrix;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;
using static ISFA.MVVM.ViewModels.BinaryMatrix.BinaryMatrixViewModel;

namespace ISFA.MVVM.Views.BinaryMatrix
{
    /// <summary>
    /// Логика взаимодействия для BinaryMatrixView.xaml
    /// </summary>
    public partial class BinaryMatrixView : ReactiveUserControl<BinaryMatrixViewModel>
	{
		private const double MinBlocksColumnWidth = 100;

		public BinaryMatrixView()
		{
			InitializeComponent();

			ViewModel = Instance;

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

				this.OneWayBind(ViewModel,
						viewModel => viewModel.InitialCompatibilitySets,
						view => view.InitialBlocks.ItemsSource)
					.DisposeWith(disposables);

				this.OneWayBind(ViewModel,
						viewModel => viewModel.MaxCoverCompatibilitySets,
						view => view.MaxCoverBlocks.ItemsSource)
					.DisposeWith(disposables);

				ExpandBlocksButton
					.Events()
					.Click
					.Subscribe(e =>
					{
						switch (ViewModel.State)
						{
							case ExpandState.Default:
								TableColumn.Width = new GridLength(0, GridUnitType.Pixel);
								ViewModel.State = ExpandState.BlocksExpanded;
								break;
							case ExpandState.TableExpanded:
								BlocksColumn.MinWidth = MinBlocksColumnWidth;
								BlocksColumn.Width = new GridLength(1, GridUnitType.Star);
								ViewModel.State = ExpandState.Default;
								break;
							case ExpandState.BlocksExpanded:
							default:
								break;
						}
					})
					.DisposeWith(disposables);

				ExpandTableButton
					.Events()
					.Click
					.Subscribe(e =>
					{
						switch (ViewModel.State)
						{
							case ExpandState.Default:
								BlocksColumn.MinWidth = 0;
								BlocksColumn.Width = new GridLength(0, GridUnitType.Pixel);
								ViewModel.State = ExpandState.TableExpanded;
								break;
							case ExpandState.BlocksExpanded:
								TableColumn.Width = new GridLength(1, GridUnitType.Star);
								ViewModel.State = ExpandState.Default;
								break;
							case ExpandState.TableExpanded:
							default:
								break;
						}
					})
					.DisposeWith(disposables);

				TableScrollViewer
					.Events()
					.ScrollChanged
					.Subscribe(e =>
					{
						if (e.Source is not ScrollViewer scrollViewer) return;

						ColumnHeadersScrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset);
						RowHeadersScrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset);
						InitialBlocksScrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset);
					})
					.DisposeWith(disposables);

				InitialBlocksScrollViewer
					.Events()
					.ScrollChanged
					.Subscribe(e =>
					{
						if (e.Source is not ScrollViewer scrollViewer) return;

						RowHeadersScrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset);
					})
					.DisposeWith(disposables);
			});
		}
	}
}
