using ISFA.MVVM.Models;
using ISFA.MVVM.ViewModels.BinaryMatrix;
using ISFA.MVVM.ViewModels.InitialTable;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Windows;
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
				.WhenAnyValue(x => x.InitialTable.IsTableEmpty, 
					x => x.InitialTable.ColumnHeaders.Count, 
					x => x.InitialTable.RowHeaders.Count)
				.Select(x => !x.Item1 && x.Item2 != 0 && x.Item3 != 0);

			var canClean = this
				.WhenAnyValue(x => x.InitialTable.IsTableEmpty)
				.Select(isEmpty => !isEmpty);

			CalculateCommand = ReactiveCommand.CreateFromTask(PaulUngerMethod, canCalculate);

			CalculateCommand.ThrownExceptions
				.Subscribe(ex => MessageBox.Show($"{ex.Message}\n{ex.InnerException}\n{ex.StackTrace}"));

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

		#region Methods

		private async Task PaulUngerMethod()
		{
			BinaryMatrix.InitialCompatibilitySets = [];
			BinaryMatrix.MaxCoverCompatibilitySets = [];

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

			try
			{
				// Проводим анализ.
				await Task.Run(() => PaulUnger.Calculate
				(
					InitialTable.States
						.Select
						(
							column => column.TransitionReactionPairs
								.Select(state => state.TransitionReactionPair)
								.ToList()
						)
						.ToList()
				));
			}
			catch (Exception ex)
			{
				MessageBox.Show($"{ex.Message}");
				return;
			}

			BinaryMatrix.ColumnHeaders = new ObservableCollection<HeaderViewModel>(InitialTable.ColumnHeaders.Skip(1));
			BinaryMatrix.RowHeaders = new ObservableCollection<HeaderViewModel>(InitialTable.ColumnHeaders.Take(InitialTable.ColumnHeaders.Count - 1));

			// Заполняем бинарную матрицу.
			ObservableCollection<StateViewModel> binaryMatrix = [];

			foreach (var row in PaulUnger.BinaryMatrix)
			{
				binaryMatrix.Add(new StateViewModel
				{
					Cells = new ObservableCollection<BinaryMatrixCellViewModel>(row
						.Select(value => new BinaryMatrixCellViewModel(value))
						.ToList())
				});
			}

			BinaryMatrix.BinaryMatrix = binaryMatrix;

			// Заполняем совместимые множества.
            foreach (var compatibilitySet in PaulUnger.InitialCompatibilitySets)
            {
				StringBuilder sb = new("{");
				sb.Append(string.Join(", ", compatibilitySet));
				sb.Append('}');

                BinaryMatrix.InitialCompatibilitySets.Add(sb.ToString());
            }

            // Заполняем совместимые множества.
            foreach (var compatibilitySet in PaulUnger.CorrectMaxCovering)
            {
	            StringBuilder sb = new("{");
	            sb.Append(string.Join(", ", compatibilitySet));
	            sb.Append('}');

	            BinaryMatrix.MaxCoverCompatibilitySets.Add(sb.ToString());
            }
		}

		#endregion
	}
}
