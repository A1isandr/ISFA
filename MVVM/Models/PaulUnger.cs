  using System.Data.Common;
using System.Windows;
using Windows.Media.Capture;
using static ISFA.MVVM.Models.BinaryMatrixCell;

namespace ISFA.MVVM.Models
{
	internal static class PaulUnger
    {
		#region Properties

		public static List<List<TransitionReactionPair>> States { get; private set; } = [];

		public static List<List<BinaryMatrixCell>> BinaryMatrix { get; private set; } = [];

		public static HashSet<HashSet<int>> InitialCompatibilitySets { get; private set; } = [];

		public static HashSet<HashSet<int>> CorrectMaxCovering { get; private set; } = [];

		#endregion

		#region Methods

		/// <summary>
		/// Применяет алгоритм Ангера-Пола к неполному автомату.
		/// </summary>
		/// <param name="states"></param>
		public static void Calculate(List<List<TransitionReactionPair>> states)
		{
			States = states;
			BinaryMatrix = [];
			InitialCompatibilitySets = [];

			// Строим бинарную матрицу.
			BuildBinaryMatrix();

			// Строим правильную максимальную группировку.
			BuildCorrectMaxCovering();
		}

		/// <summary>
		/// Строит бинарную матрицу
		/// </summary>
		private static void BuildBinaryMatrix()
		{
			var columns = States.Count;
			var rows = States.First().Count;
			const string undefinedSymbol = TransitionReactionPair.UndefinedSymbol;

			// Ищем явно совметсимые/несовместимые состояния.
			// Сравниваем все состояния друг с другом.
			for (int i = 0; i < columns - 1; i++)
			{
				// Добавляем i-тую строку бинарной матрицы.
				BinaryMatrix.Add(Enumerable.Repeat(new BinaryMatrixCell(), columns - (i + 1)).ToList());

				for (int j = i + 1; j < columns; j++)
				{
					var clearlyCompatibleCount = 0;

					// Проходимся по всем парам переходов/реакций в двух состояниях.
					for (int k = 0; k < rows; k++)
					{
						// Проверяем, явно ли несовместимы два состояния.
						if (States[i][k].Reaction != undefinedSymbol &&
							States[j][k].Reaction != undefinedSymbol &&
							States[i][k].Reaction != States[j][k].Reaction)
						{
							BinaryMatrix[i][j - i - 1] = new BinaryMatrixCell(0, CellState.ClearlyIncompatible);
							break;
						}

						// Проверяем, явно ли совместимы два состояния.
						// ?/? -/-
						// -/- ?/?
						// 1/x 1/x
						// 1/- 1/?
						// -/x ?/x
						// ?/x -/x
						// 1/? 1/-
						// -/? ?/-
						// ?/- -/?
						// TODO: Это выглядит просто ужасно, надо переписать. Возможно, просто вырезать.
						if ((States[i][k].Transition == undefinedSymbol &&
							 States[i][k].Reaction == undefinedSymbol) ||
							(States[j][k].Transition == undefinedSymbol &&
							 States[j][k].Reaction == undefinedSymbol) ||
							(States[i][k].Transition == States[j][k].Transition &&
							 States[i][k].Reaction == States[j][k].Reaction) ||
							(States[i][k].Transition == States[j][k].Transition &&
							 States[i][k].Reaction == undefinedSymbol) ||
							(States[i][k].Transition == undefinedSymbol &&
							 States[i][k].Reaction == States[j][k].Reaction) ||
							(States[j][k].Transition == undefinedSymbol &&
							 States[j][k].Reaction == States[i][k].Reaction) ||
							(States[j][k].Reaction == undefinedSymbol &&
							 States[i][k].Transition == States[j][k].Transition) ||
							(States[i][k].Transition == undefinedSymbol &&
							 States[j][k].Reaction == undefinedSymbol) ||
							(States[j][k].Transition == undefinedSymbol &&
							 States[i][k].Reaction == undefinedSymbol))
						{
							clearlyCompatibleCount++;
						}
					}

					if (clearlyCompatibleCount != rows) continue;

					// Если явно совместимы - ставим единицу в бинарной матрице.
					BinaryMatrix[i][j - i - 1] = new BinaryMatrixCell(1, CellState.ClearlyCompatible);
				}
			}

			// Проверяем остальные пары.
			for (int i = 0; i < BinaryMatrix.Count; i++)
			{
				for (int j = 0; j < BinaryMatrix[i].Count; j++)
				{
					if (BinaryMatrix[i][j].Value is not null) continue;

					//MessageBox.Show($"Now checking: {i + 1} {j + 1 + i + 1}");
					BinaryMatrix[i][j] = IsCompatible(i, j, []) 
						? new BinaryMatrixCell(1, CellState.Final)
						: new BinaryMatrixCell(0, CellState.Final);
				}
			}
		}

		/// <summary>
		/// Строит правильную максимальную группировку.
		/// </summary>
		private static void BuildCorrectMaxCovering()
		{
			// Составляем множества совместимости на основе бинарной матрицы.
			for (int i = 0; i < BinaryMatrix.Count; i++)
			{
				InitialCompatibilitySets.Add([i + 1]);
				
				for (int j = 0; j < BinaryMatrix[i].Count; j++)
				{
					if (BinaryMatrix[i][j].Value == 1)
					{
						InitialCompatibilitySets.Last().Add(j + 1 + i + 1);
					}
				}
			}

			CorrectMaxCovering = [..InitialCompatibilitySets];

			HashSet<HashSet<HashSet<int>>> incompatibleSets = [];

			// Проверяем множества на наличие несовместимых состояний.
			foreach (var set in CorrectMaxCovering)
			{
				incompatibleSets.Add(FindIncompatibleStates(set));
			}

			// Дробление множеств
			for (int i = 0; i < CorrectMaxCovering.Count; i++)
			{
				var currentSet = CorrectMaxCovering.ElementAt(i);
				var incompatibleSubsets = incompatibleSets.ElementAt(i);

				if (incompatibleSubsets.Count == 0) continue;

				// Разбиваем множество на подмножества, исключая несовместимые пары
				List<HashSet<int>> newSubsets = new List<HashSet<int>>();
				foreach (var incompatiblePair in incompatibleSubsets)
				{
					foreach (var elementToRemove in incompatiblePair)
					{
						var newSubset = new HashSet<int>(currentSet);
						newSubset.Remove(elementToRemove);
						newSubsets.Add(newSubset);
					}
				}

				// Заменяем исходное множество новыми подмножествами
				CorrectMaxCovering.Remove(currentSet);
				CorrectMaxCovering.UnionWith(newSubsets);
			}

			// Поглощение множеств
			bool changesMade;
			do
			{
				changesMade = false;
				for (int i = 0; i < CorrectMaxCovering.Count; i++)
				{
					for (int j = i + 1; j < CorrectMaxCovering.Count; j++)
					{
						var set1 = CorrectMaxCovering.ElementAt(i);
						var set2 = CorrectMaxCovering.ElementAt(j);

						if (set1.IsSubsetOf(set2))
						{
							CorrectMaxCovering.Remove(set1);
							changesMade = true;
							break;
						}
						else if (set2.IsSubsetOf(set1))
						{
							CorrectMaxCovering.Remove(set2);
							changesMade = true;
							break;
						}
					}

					if (changesMade) break; // Restart outer loop after a change
				}
			} while (changesMade);
		}

		/// <summary>
		/// Поиск несовместимых состояний в блоках совместимости.
		/// </summary>
		/// <param name="set"></param>
		/// <returns></returns>
		private static HashSet<HashSet<int>> FindIncompatibleStates(HashSet<int> set)
		{
			HashSet<HashSet<int>> incompatibleSets = [];

			//MessageBox.Show($"Checking: {string.Join(", ", set)}");

			// Находим все множества, которые можно вынести из блока.
			for (int i = 1; i < set.Count - 1; i++)
			{
				incompatibleSets.Add([]);

				for (int j = i + 1; j < set.Count; j++)
				{
					(int row, int column) = StateToBinaryMatrixCoords(set.ElementAt(i), set.ElementAt(j));
					if (BinaryMatrix[row][column].Value == 1) continue;

					incompatibleSets.Last().Add( set.ElementAt(j));
				}
			}

			return incompatibleSets;
	    }

		/// <summary>
		/// Проверяет совместимость двух состояний.
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <param name="chain"></param>
		/// <returns></returns>
		private static bool IsCompatible(int row, int column, HashSet<(int, int)> chain)
		{
			if (BinaryMatrix[row][column].Value == 1) return true;
			if (BinaryMatrix[row][column].State == CellState.ClearlyIncompatible) return false;

			// Если образовалась петля - возвращаем true;
			if (chain.Contains((row, column))) return true;

			var state1 = States[row];
			var state2 = States[column + row + 1];

			// Проходимся по всем парам переходов/реакций в двух состояниях.
			for (int k = 0; k < state1.Count; k++)
			{
				// Если переходы не определены - пропускаем.
				if (state1[k].Transition == TransitionReactionPair.UndefinedSymbol ||
				    state2[k].Transition == TransitionReactionPair.UndefinedSymbol) continue;

				// Если переходы совпадают - пропускаем.
				if (state1[k].Transition == state2[k].Transition) continue;

				//MessageBox.Show($"{row + 1} {column + 1 + row + 1} -> {state1[k].Transition} {state2[k].Transition}");

				(int newRow, int newColumn) = StateToBinaryMatrixCoords(Convert.ToInt32(state1[k].Transition), Convert.ToInt32(state2[k].Transition));
				chain.Add((row, column));

				if (!IsCompatible(newRow, newColumn, chain))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Преобразует координаты в формат бинарной матрицы.
		/// </summary>
		/// <param name="state1Coords"></param>
		/// <param name="state2Coords"></param>
		/// <returns></returns>
		private static (int row, int column) StateToBinaryMatrixCoords(int state1Coords, int state2Coords)
        {
            state1Coords--;
			state2Coords--;

			// Проверяем, чтобы state1Coords было меньше state2Coords.
			if (state1Coords > state2Coords)
			{
				(state1Coords, state2Coords) = (state2Coords, state1Coords);
			}

			// Расчитываем координаты в бинарной матрице.
			int row = state1Coords;
			int column = state2Coords - state1Coords - 1;

			return (row, column);
		}

		#endregion
	}
}
