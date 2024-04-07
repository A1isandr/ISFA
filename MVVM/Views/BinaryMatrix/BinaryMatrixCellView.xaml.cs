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
using ISFA.MVVM.ViewModels.BinaryMatrix;
using ReactiveUI;

namespace ISFA.MVVM.Views
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
