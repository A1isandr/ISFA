using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ISFA.MVVM.Models
{
    public class State : ReactiveObject
    {
        [Reactive]
	    public string Transition { get; set; } = string.Empty;

        [Reactive]
	    public string Reaction { get; set; } = string.Empty;
    }
}
