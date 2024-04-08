using System.Windows;
using static ISFA.MVVM.Models.BinaryMatrixCell;

namespace ISFA.MVVM.Models
{
	internal static class PaulAnger
    {
		#region Properties

		public static List<List<TransitionReactionPair>> States { get; private set; } = [];

		public static List<List<BinaryMatrixCell>> BinaryMatrix { get; private set; } = [];

		public static HashSet<HashSet<int>> CompatibilitySets { get; private set; } = [];

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
			CompatibilitySets = [];

			// Строим бинарную матрицу.
			BuildBinaryMatrix();

			// Строим правильную группировку.
			//BuildCorrectCovering();
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
				// Заполняем i-тую строку бинарной матрицы нулями.
				BinaryMatrix.Add(Enumerable.Repeat(new BinaryMatrixCell(), columns - (i + 1)).ToList());

				for (int j = i + 1; j < columns; j++)
				{
					var clearlyCompatibleCount = 0;

					// Проходимся по всем парам переходов/реакций в двух состояниях.
					for (int k = 0; k < rows; k++)
					{
						// Проверяем, явно несовметсимы ли два состояния.
						if (States[i][k].Reaction != undefinedSymbol &&
							States[j][k].Reaction != undefinedSymbol &&
							States[i][k].Reaction != States[j][k].Reaction)
						{
							// Если явно несовместимы - ставим ноль в бинарной матрице.
							BinaryMatrix[i][j - i - 1] = new BinaryMatrixCell(0, CellState.ClearlyIncompatible);

							break;
						}

						// Проверяем, явно совместимы ли два состояния.
						// ?/? -/-
						// -/- ?/?
						// 1/x 1/x
						// 1/- 1/?
						// -/x ?/x
						// ?/x -/x
						// 1/? 1/-
						// -/? ?/-
						// ?/- -/?
						// TODO: Это выглядит просто ужасно, надо переписать.
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
					BinaryMatrix[i][j] = IsCompatible(i, j) 
						? new BinaryMatrixCell(1, CellState.Final)
						: new BinaryMatrixCell(0, CellState.Final);
				}
			}
		}

		/// <summary>
		/// Строит правильную группировку.
		/// </summary>
		private static void BuildCorrectCovering()
		{
			// Составляем множества совместимости на основе бинарной матрицы.
			for (int i = 0; i < BinaryMatrix.Count; i++)
			{
				CompatibilitySets.Add([i]);

				for (int j = 0; j < BinaryMatrix[i].Count; j++)
				{
					if (BinaryMatrix[i][j].Value == 1)
					{
						CompatibilitySets.Last().Add(j);
					}
				}
			}

			// Проверяем множества на наличие несовместимых состояний.
			foreach (var set in CompatibilitySets)
			{
				FindIncompatibleStates(set);
			}
		}

		/// <summary>
		/// Поиск несовместимых состояний.
		/// </summary>
		/// <param name="set"></param>
		/// <returns></returns>
		private static HashSet<int> FindIncompatibleStates(HashSet<int> set)
		{
			// TODO: Имплементировать логику для поиска несовместимых состояний.
			HashSet<int> incompatibleStatesSet = [];
			HashSet<int> compatibleStatesSet = [];

			for (int j = 1; j < set.Count; j++)
			{
				for (int k = j + 1; k < set.Count; k++)
				{
					if (BinaryMatrix[set.ElementAt(j)][set.ElementAt(k)].Value == 0)
					{
						incompatibleStatesSet.Add(set.ElementAt(j));
					}
				}
			}

			var newIncompatibleStatesSet = FindIncompatibleStates(incompatibleStatesSet);

			if (newIncompatibleStatesSet.Count == 0)
			{

			}
		}

		/// <summary>
		/// Проверяет совместимость двух состояний.
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <returns></returns>
		private static bool IsCompatible(int row, int column)
		{
			// TODO: Рассмотреть возможные петли.

			var state1 = States[row];
			var state2 = States[column + row + 1];

			if (BinaryMatrix[row][column].State == CellState.ClearlyCompatible)
			{
				return true;
			}
			if (BinaryMatrix[row][column].State == CellState.ClearlyIncompatible)
			{
				return false;
			}

			for (int k = 0; k < state1.Count; k++)
			{
				// Если переходы не определены - пропускаем.
				if (state1[k].Transition == TransitionReactionPair.UndefinedSymbol ||
				    state2[k].Transition == TransitionReactionPair.UndefinedSymbol) continue;

				// Если переходы совпадают - пропускаем.
				if (state1[k].Transition == state2[k].Transition) continue;

				//MessageBox.Show($"{row + 1} {column + 1 + row + 1} -> {state1[k].Transition} {state2[k].Transition}");

				(int newRow, int newColumn) = StateToBinaryMatrixCoords(Convert.ToInt32(state1[k].Transition) - 1, Convert.ToInt32(state2[k].Transition) - 1);

				// Если состояние переходит в само себя - пропускаем, чтобы не образовалась петля.
				if ((newRow == row && newColumn == column) ||
				    (newRow == column && newColumn == row)) continue;

				//MessageBox.Show($"Recursive checking: {state1[k].Transition} {state2[k].Transition} {IsCompatible(newRow, newColumn)}");

				// Проверяем совместимость двух состояний.
				return IsCompatible(newRow, newColumn);
			}

			return true;
		}

		private static (int row, int column) StateToBinaryMatrixCoords(int state1Coords, int state2Coords)
		{
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
