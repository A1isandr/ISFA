using ISFA.MVVM.ViewModels;
using ReactiveUI;

namespace ISFA.MVVM.Views
{
	/// <summary>
	/// Логика взаимодействия для HelpView.xaml
	/// </summary>
	public partial class HelpView : ReactiveUserControl<HelpViewModel>
	{
		public HelpView()
		{
			InitializeComponent();

			ViewModel = HelpViewModel.Instance;
		}
	}
}
