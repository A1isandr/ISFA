using System.Reactive.Disposables;
using ISFA.MVVM.ViewModels.InitialTable;
using ReactiveUI;

namespace ISFA.MVVM.Views.InitialTable
{
    /// <summary>
    /// Логика взаимодействия для TableColumnView.xaml
    /// </summary>
    public partial class StateView : ReactiveUserControl<StateViewModel>
	{
		public StateView()
		{
			InitializeComponent();

			this.WhenActivated(disposables =>
			{
				this.OneWayBind(ViewModel,
						viewModel => viewModel.TransitionReactionPairs,
						view => view.Column.ItemsSource)
					.DisposeWith(disposables);
			});
		}
	}
}
