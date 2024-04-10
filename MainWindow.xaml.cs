using System.Reactive.Disposables;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ISFA.MVVM.ViewModels;
using ISFA.MVVM.Views;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;

namespace ISFA
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
	{
		private const string WindowMaximizeIconKey = "WindowMaximizeIcon";
		private const string WindowRestoreIconKey = "WindowRestoreIcon";

		public MainWindow()
		{
			InitializeComponent();

			ViewModel = new MainWindowViewModel();

			this.WhenActivated(disposables =>
			{
				this.OneWayBind(ViewModel,
						viewModel => viewModel.IsMainViewOpen,
						view => view.MainViewExpander.IsExpanded)
					.DisposeWith(disposables);

				this.OneWayBind(ViewModel,
						viewModel => viewModel.IsHelpViewOpen,
						view => view.HelpViewExpander.IsExpanded)
					.DisposeWith(disposables);

				MainView.HelpButton
					.Events()
					.Click
					.Subscribe(e =>
					{
						ViewModel.IsMainViewOpen = false;
						ViewModel.IsHelpViewOpen = true;
					})
					.DisposeWith(disposables);

				HelpView.BackButton
					.Events()
					.Click
					.Subscribe(e =>
					{
						ViewModel.IsMainViewOpen = true;
						ViewModel.IsHelpViewOpen = false;
					})
					.DisposeWith(disposables);
			});
		}

		private void Header_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (WindowState == WindowState.Maximized)
			{
				var mouseWindowRelativeCoords = Mouse.GetPosition(this);

				Left = mouseWindowRelativeCoords.X;
				Top = mouseWindowRelativeCoords.Y - 5;

				WindowState = WindowState.Normal;
			}

			DragMove();
		}

		private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void MaxWindowButton_Click(object sender, RoutedEventArgs e)
		{
			switch (WindowState)
			{
				case WindowState.Maximized:
				{
					WindowState = WindowState.Normal;

					var resource = Application.Current.FindResource(WindowMaximizeIconKey);

					if (resource is not null)
					{
						MaxWindowButton.Content = ((GeometryDrawing)resource).Geometry;
					}

					break;
				}
				case WindowState.Normal:
				{
					WindowState = WindowState.Maximized;

					var resource = Application.Current.FindResource(WindowRestoreIconKey);

					if (resource is not null)
					{
						MaxWindowButton.Content = ((GeometryDrawing)resource).Geometry;
					}

					break;
				}
				case WindowState.Minimized:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(sender));
			}
		}

		private void MinWindowButton_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}
	}
}