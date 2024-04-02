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
	/// Логика взаимодействия для TableColumnView.xaml
	/// </summary>
	public partial class TableColumnView : ReactiveUserControl<TableColumnViewModel>
	{
		public TableColumnView()
		{
			InitializeComponent();

			this.WhenActivated(disposables =>
			{
				this.OneWayBind(ViewModel,
						viewModel => viewModel.States,
						view => view.Column.ItemsSource)
					.DisposeWith(disposables);
			});
		}
	}
}
