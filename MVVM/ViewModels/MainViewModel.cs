using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ISFA.MVVM.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ISFA.MVVM.ViewModels
{
	public class MainViewModel : ReactiveObject
	{
		#region Singleton

		private static MainViewModel? _instance;

		public static MainViewModel Instance
		{
			get
			{
				_instance ??= new MainViewModel();
				return _instance;
			}
		}

		#endregion

		#region Properties

		public ReactiveCommand<Unit, Unit> CalculateCommand { get; }
		public ReactiveCommand<Unit, Unit> CleanCommand { get; }

		private InitialTableViewModel InitialTable { get; } = InitialTableViewModel.Instance;

		#endregion

		#region Constructors

		private MainViewModel()
		{
			var canCalculate = this
				.WhenAnyValue(x => x.InitialTable.IsTableEmpty)
				.Select(isEmpty => !isEmpty);

			var canClean = this
				.WhenAnyValue(x => x.InitialTable.IsTableEmpty)
				.Select(isEmpty => !isEmpty);

			CalculateCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(_ =>
			{
				foreach (var column in InitialTable.Columns)
				{
					foreach (var state in column.States)
					{
						state.State.Transition = state.State.Transition == string.Empty
							? StateViewModel.UndefinedSymbol
							: state.State.Transition;

						state.State.Reaction = state.State.Reaction == string.Empty
							? StateViewModel.UndefinedSymbol
							: state.State.Reaction;
					}
				}

				PaulAnger.Calculate
				(
					InitialTable.Columns
						.Select
						(
							column => column.States
								.Select(state => state.State)
								.ToList()
						)
						.ToList()
				);

				return Observable.Return(Unit.Default);
			}, canCalculate);

			CleanCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(_ =>
			{
				InitialTableViewModel.Instance.CleanCommand.Execute();
				return Observable.Return(Unit.Default);
			}, canClean);
		}

		#endregion
	}
}
