using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ISFA.MVVM.ViewModels.InitialTable;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ISFA.MVVM.ViewModels
{
    public class HeaderViewModel : ReactiveObject
    {
		#region Properties

		public ReactiveCommand<Unit, Unit> DeleteHeaderCommand { get; }

		public string InitialName { get; }

		[Reactive] 
		public string Name { get; set; }

		public HeaderType Type { get; init; }

		public enum HeaderType
		{
			Column,
			Row
		}

		#endregion

		#region Constructor

		public HeaderViewModel(HeaderType type, string name)
		{
			InitialName = name;
			Name = name;
			Type = type;

			DeleteHeaderCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(_ =>
			{
				switch (Type)
				{
					case HeaderType.Column:
						InitialTableViewModel.Instance.ColumnHeaders.Remove(this);
						break;
					case HeaderType.Row:
						InitialTableViewModel.Instance.RowHeaders.Remove(this);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				return Observable.Return(Unit.Default);
			});
		}

		#endregion
	}
}
