using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using ISFA.MVVM.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ISFA.MVVM.ViewModels
{
    public class StateViewModel(HeaderViewModel column, HeaderViewModel row) : ReactiveObject
    {
		#region Properties

		public static readonly string UndefinedSymbol = "—";

		public HeaderViewModel Column { get; } = column;

		public HeaderViewModel Row { get; } = row;

		[Reactive]
		public State State { get; set; } = new();

		#endregion
	}
}
