using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
