using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ISFA.MVVM.Models
{
	public class TransitionReactionPair : ReactiveObject
    {
		#region Properties

		public const string UndefinedSymbol = "—";

		[Reactive]
		public string Transition { get; set; } = string.Empty;

		[Reactive]
		public string Reaction { get; set; } = string.Empty;

		#endregion

		#region Methods

		public override string ToString()
		{
			return $"{Transition} / {Reaction}";
		}

		#endregion
	}
}
