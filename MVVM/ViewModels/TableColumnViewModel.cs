using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ISFA.MVVM.ViewModels
{
	public class TableColumnViewModel : ReactiveObject
	{
		#region Properties

		[Reactive]
		public ObservableCollection<StateViewModel> States { get; set; } = [];

		public HeaderViewModel Header { get; }

		private readonly ObservableAsPropertyHelper<bool> _isColumnFullyFilled;
		public bool IsColumnFullyFilled => _isColumnFullyFilled.Value;

		#endregion

		#region Constructors

		public TableColumnViewModel(HeaderViewModel header)
		{
			Header = header;

			_isColumnFullyFilled = this
				.WhenAnyValue(x => x.States)
				.Select(states => states.All(state =>
					state.State.Transition != string.Empty && state.State.Reaction != string.Empty))
				.ToProperty(this, x => x.IsColumnFullyFilled);
		}

		#endregion
	}
}
