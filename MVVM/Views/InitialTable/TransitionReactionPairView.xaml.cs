using System.Reactive.Disposables;
using ISFA.MVVM.Models;
using ISFA.MVVM.ViewModels.InitialTable;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;
using TextBox = System.Windows.Controls.TextBox;

namespace ISFA.MVVM.Views.InitialTable
{
	/// <summary>
	/// Логика взаимодействия для StateView.xaml
	/// </summary>
	public partial class TransitionReactionPairView : ReactiveUserControl<TransitionReactionPairViewModel>
	{
		public TransitionReactionPairView()
		{
			InitializeComponent();

			this.WhenActivated(disposables =>
			{
				this.Bind(ViewModel,
						viewModel => viewModel.TransitionReactionPair.Transition,
						view => view.Transition.Text)
					.DisposeWith(disposables);

				this.Bind(ViewModel,
						viewModel => viewModel.TransitionReactionPair.Reaction,
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
							textBox.Text = TransitionReactionPair.UndefinedSymbol;
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
							textBox.Text = TransitionReactionPair.UndefinedSymbol;
						}
					})
					.DisposeWith(disposables);
			});
		}
	}
}
