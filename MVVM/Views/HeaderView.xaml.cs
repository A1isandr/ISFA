using ISFA.MVVM.ViewModels;
using ReactiveMarbles.ObservableEvents;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace ISFA.MVVM.Views
{
	/// <summary>
	/// Логика взаимодействия для HeaderView.xaml
	/// </summary>
	public partial class HeaderView : ReactiveUserControl<HeaderViewModel>
    {
        public HeaderView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
	            this.Bind(ViewModel,
			            viewmodel => viewmodel.Name,
			            view => view.NameTextBox.Text)
		            .DisposeWith(disposables);

	            this.BindCommand(ViewModel,
		            viewmodel => viewmodel.DeleteHeaderCommand,
		            view => view.DeleteButton);

	            this
		            .Events()
		            .MouseEnter
		            .Subscribe(e =>
		            {
			            DeleteButton.Visibility = Visibility.Visible;
		            })
		            .DisposeWith(disposables);

	            this
		            .Events()
		            .MouseLeave
		            .Subscribe(e =>
		            {
			            DeleteButton.Visibility = Visibility.Hidden;
		            })
		            .DisposeWith(disposables);

	            NameTextBox
		            .Events()
		            .LostFocus
		            .Subscribe(e =>
		            {
						var textBox = (TextBox)e.OriginalSource;

						if (textBox.Text.Length == 0)
						{
							textBox.Text = ViewModel!.InitialName;
						}
		            });
            });
        }
    }
}
