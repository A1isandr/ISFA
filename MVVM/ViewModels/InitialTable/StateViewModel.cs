using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;

namespace ISFA.MVVM.ViewModels.InitialTable
{
	public class StateViewModel(HeaderViewModel header) : ReactiveObject
    {
        #region Properties

        [Reactive]
        public ObservableCollection<TransitionReactionPairViewModel> TransitionReactionPairs { get; set; } = [];

        public HeaderViewModel Header { get; } = header;

		#endregion
	}
}
