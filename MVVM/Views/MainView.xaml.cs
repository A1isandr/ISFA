using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
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
using ReactiveUI;

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
						view => view.BinaryMatrix.Visibility)
					.DisposeWith(disposables);
            });
        }
    }
}
