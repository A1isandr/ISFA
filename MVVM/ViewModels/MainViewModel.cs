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
using ISFA.MVVM.ViewModels.BinaryMatrix;
using ISFA.MVVM.ViewModels.InitialTable;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using StateViewModel = ISFA.MVVM.ViewModels.BinaryMatrix.StateViewModel;

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
		private BinaryMatrixViewModel BinaryMatrix { get; } = BinaryMatrixViewModel.Instance;

		private readonly ObservableAsPropertyHelper<bool> _isBinaryMatrixVisible;
		public bool IsBinaryMatrixVisible => _isBinaryMatrixVisible.Value;

		#endregion

		#region Constructors

		private MainViewModel()
		{
			var canCalculate = this
				.WhenAnyValue(x => x.InitialTable.IsTableEmpty, x => x.InitialTable.ColumnHeaders.Count, x => x.InitialTable.RowHeaders.Count)
				.Select(x => !x.Item1 && x.Item2 != 0 && x.Item3 != 0);

			var canClean = this
				.WhenAnyValue(x => x.InitialTable.IsTableEmpty)
				.Select(isEmpty => !isEmpty);

			CalculateCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(_ =>
			{
				// Заполняем все пустые ячейки.
				foreach (var state in InitialTable.States)
				{
					foreach (var pair in state.TransitionReactionPairs)
					{
						pair.TransitionReactionPair.Transition = pair.TransitionReactionPair.Transition == string.Empty
							? TransitionReactionPair.UndefinedSymbol
							: pair.TransitionReactionPair.Transition;

						pair.TransitionReactionPair.Reaction = pair.TransitionReactionPair.Reaction == string.Empty
							? TransitionReactionPair.UndefinedSymbol
							: pair.TransitionReactionPair.Reaction;
					}
				}

				// Проводим анализ.
				PaulAnger.Calculate
				(
					InitialTable.States
						.Select
						(
							column => column.TransitionReactionPairs
								.Select(state => state.TransitionReactionPair)
								.ToList()
						)
						.ToList()
				);

				BinaryMatrix.ColumnHeaders = new ObservableCollection<HeaderViewModel>(InitialTable.ColumnHeaders.Skip(1));
				BinaryMatrix.RowHeaders = new ObservableCollection<HeaderViewModel>(InitialTable.ColumnHeaders.Take(InitialTable.ColumnHeaders.Count - 1));

				// Заполняем бинарную матрицу.
				ObservableCollection<StateViewModel> binaryMatrix = [];

				foreach (var row in PaulAnger.BinaryMatrix)
				{
					binaryMatrix.Add(new StateViewModel 
					{
						Cells = new ObservableCollection<BinaryMatrixCellViewModel>(row
							.Select(value => new BinaryMatrixCellViewModel(value))
							.ToList())
					});
				}

				BinaryMatrix.BinaryMatrix = binaryMatrix;

				return Observable.Return(Unit.Default);
			}, canCalculate);

			CleanCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(_ =>
			{
				InitialTableViewModel.Instance.CleanCommand.Execute();
				BinaryMatrixViewModel.Instance.CleanCommand.Execute();

				return Observable.Return(Unit.Default);
			}, canClean);

			_isBinaryMatrixVisible = this
				.WhenAnyValue(x => x.BinaryMatrix.IsTableNotEmpty)
				.Select(isNotEmpty => isNotEmpty)
				.ToProperty(this, x => x.IsBinaryMatrixVisible);
		}

		#endregion
	}
}
