using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using ISFA.Misc;
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
