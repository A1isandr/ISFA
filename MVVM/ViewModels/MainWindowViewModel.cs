using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ISFA.MVVM.ViewModels
{
	public class MainWindowViewModel : ReactiveObject
	{
		#region Properties

		[Reactive]
		public bool IsMainViewOpen { get; set; } = true;

		[Reactive]
		public bool IsHelpViewOpen { get; set; }

		#endregion
	}
}
