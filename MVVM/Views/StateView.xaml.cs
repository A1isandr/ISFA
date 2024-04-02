using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ISFA.MVVM.ViewModels;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;
using TextBox = System.Windows.Controls.TextBox;

namespace ISFA.MVVM.Views
{
	/// <summary>
	/// Логика взаимодействия для StateView.xaml
	/// </summary>
	public partial class StateView : ReactiveUserControl<StateViewModel>
	{
		public StateView()
		{
			InitializeComponent();

			this.WhenActivated(disposables =>
			{
				this.Bind(ViewModel,
						viewModel => viewModel.State.Transition,
						view => view.Transition.Text)
					.DisposeWith(disposables);

				this.Bind(ViewModel,
						viewModel => viewModel.State.Reaction,
						view => view.Reaction.Text)
					.DisposeWith(disposables);

				Transition
					.Events()
					.LostFocus
					.Subscribe(e =>
					{
						var textBox = (TextBox)e.OriginalSource;

						if (textBox.Text.Length == 0)
						{
							textBox.Text = StateViewModel.UndefinedSymbol;
						}
					})
					.DisposeWith(disposables);

				Reaction
					.Events()
					.LostFocus
					.Subscribe(e =>
					{
						var textBox = (TextBox)e.OriginalSource;

						if (textBox.Text.Length == 0)
						{
							textBox.Text = StateViewModel.UndefinedSymbol;
						}
					})
					.DisposeWith(disposables);
			});
		}
	}
}
