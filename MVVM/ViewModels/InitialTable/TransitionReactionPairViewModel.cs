using ISFA.MVVM.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ISFA.MVVM.ViewModels.InitialTable
{
	public class TransitionReactionPairViewModel(HeaderViewModel column, HeaderViewModel row) : ReactiveObject
    {
        #region Properties

        public HeaderViewModel Column { get; } = column;

        public HeaderViewModel Row { get; } = row;

        [Reactive]
        public TransitionReactionPair TransitionReactionPair { get; set; } = new();

        #endregion
    }
}
