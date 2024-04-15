using ISFA.MVVM.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace ISFA.MVVM.Views
{
	/// <summary>
	/// Логика взаимодействия для MainView.xaml
	/// </summary>
	public partial class MainView : ReactiveUserControl<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();

            ViewModel = MainViewModel.Instance;

            this.WhenActivated(disposables =>
            {
	            this.BindCommand(ViewModel,
			            viewModel => viewModel.CalculateCommand,
			            view => view.CalculateButton)
		            .DisposeWith(disposables);

				this.BindCommand(ViewModel,
			            viewModel => viewModel.CleanCommand,
			            view => view.CleanButton)
		            .DisposeWith(disposables);

				this.OneWayBind(ViewModel,
						viewModel => viewModel.IsBinaryMatrixVisible,
						view => view.ResultsStackPanel.Visibility)
					.DisposeWith(disposables);
			});
        }
    }
}
