using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Controls;
using ISFA.MVVM.ViewModels.InitialTable;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;

namespace ISFA.MVVM.Views.InitialTable
{
	/// <summary>
	/// Логика взаимодействия для InitialTableView.xaml
	/// </summary>
	public partial class InitialTableView : ReactiveUserControl<InitialTableViewModel>
	{
		public InitialTableView()
		{
			InitializeComponent();

			ViewModel = InitialTableViewModel.Instance;

			this.WhenActivated(disposables =>
			{
				this.OneWayBind(ViewModel,
						viewModel => viewModel.States,
						view => view.Columns.ItemsSource)
					.DisposeWith(disposables);

				this.OneWayBind(ViewModel,
						viewModel => viewModel.ColumnHeaders,
						view => view.ColumnHeaders.ItemsSource)
					.DisposeWith(disposables);

				this.OneWayBind(ViewModel,
						viewModel => viewModel.RowHeaders,
						view => view.RowHeaders.ItemsSource)
					.DisposeWith(disposables);

				this.OneWayBind(ViewModel,
						viewModel => viewModel.IsTableEmpty,
						view => view.EmptyTableLabel.Visibility)
					.DisposeWith(disposables);

				this.BindCommand(ViewModel,
						viewModel => viewModel.NewColumnCommand,
						view => view.AddColumnButton)
					.DisposeWith(disposables);

				this.BindCommand(ViewModel,
						viewModel => viewModel.NewRowCommand,
						view => view.AddRowButton)
					.DisposeWith(disposables);

				AddColumnButton
					.Events()
					.Click
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(e =>
					{
						if (ViewModel.ColumnHeaders.Count == 0)
						{
							ColumnHeadersScrollViewer.ScrollToRightEnd();
						}
						else
						{
							TableScrollViewer.ScrollToRightEnd();
						}
					})
					.DisposeWith(disposables);

				AddRowButton
					.Events()
					.Click
					.ObserveOn(RxApp.MainThreadScheduler)
					.Subscribe(e =>
					{
						if (ViewModel.RowHeaders.Count == 0)
						{
							RowHeadersScrollViewer.ScrollToBottom();
						}
						else
						{
							TableScrollViewer.ScrollToBottom();
						}
					})
					.DisposeWith(disposables);
			});
		}

		private void ScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			if (sender is not ScrollViewer scrollViewer) return;

			ColumnHeadersScrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset);
			RowHeadersScrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset);
		}
	}
}
