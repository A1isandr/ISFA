using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ISFA.MVVM.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

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
