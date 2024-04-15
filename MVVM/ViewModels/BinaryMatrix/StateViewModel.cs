using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;

namespace ISFA.MVVM.ViewModels.BinaryMatrix
{
	public class StateViewModel : ReactiveObject
    {
		#region Properties

		[Reactive]
	    public ObservableCollection<BinaryMatrixCellViewModel> Cells { get; set; } = [];

	    #endregion

    }
}
