using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ISFA.MVVM.ViewModels;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;

namespace ISFA.MVVM.Views
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
						viewModel => viewModel.Columns,
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
