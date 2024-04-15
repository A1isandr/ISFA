using ReactiveUI;

namespace ISFA.MVVM.ViewModels
{
	public class HelpViewModel : ReactiveObject
	{
		#region Singleton

		private static HelpViewModel? _instance;

		public static HelpViewModel Instance
		{
			get
			{
				_instance ??= new HelpViewModel();
				return _instance;
			}
		}

		#endregion
	}
}
