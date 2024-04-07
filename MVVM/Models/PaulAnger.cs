using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ISFA.MVVM.Models;

namespace ISFA.MVVM.Models
{
    static class PaulAnger
    {
		#region Properties

		private static List<List<TransitionReactionPair>> _states = [];

		public static List<List<BinaryMatrixCell>> BinaryMatrix {get; private set;} = [];

		#endregion

		#region Methods

		public static void Calculate(List<List<TransitionReactionPair>> states)
		{
			_states = states;
			BinaryMatrix = [];

			// Строим бинарную матрицу.
			BuildBinaryMatrix();
		}

		/// <summary>
		/// Строит бинарную матрицу
		/// </summary>
		private static void BuildBinaryMatrix()
		{
			var columns = _states.Count;
			var rows = _states.First().Count;
			var undefinedSymbol = TransitionReactionPair.UndefinedSymbol;

			// Ищем явно совметсимые/несовместимые состояния.
			// Сравниваем все состояния друг с другом.
			for (int i = 0; i < columns - 1; i++)
			{
				// Заполняем i-тую строку бинарной матрицы нулями.
				BinaryMatrix.Add(Enumerable.Repeat(new BinaryMatrixCell(0), columns - (i + 1)).ToList());

				for (int j = i + 1; j < columns; j++)
				{
					var clearlyCompatibleCount = 0;

					// Проходимся по всем парам переходов/реакций в двух состояниях.
					for (int k = 0; k < rows; k++)
					{
						// Проверяем, явно несовметсимы ли два состояния.
						if (_states[i][k].Reaction != undefinedSymbol &&
							_states[j][k].Reaction != undefinedSymbol &&
							_states[i][k].Reaction != _states[j][k].Reaction)
						{
							// Если явно несовместимы - ставим ноль в бинарной матрице.
							BinaryMatrix[i][j - i - 1] = new BinaryMatrixCell(0, true);

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
						if ((_states[i][k].Transition == undefinedSymbol &&
							 _states[i][k].Reaction == undefinedSymbol) ||
							(_states[j][k].Transition == undefinedSymbol &&
							 _states[j][k].Reaction == undefinedSymbol) ||
							(_states[i][k].Transition == _states[j][k].Transition &&
							 _states[i][k].Reaction == _states[j][k].Reaction) ||
							(_states[i][k].Transition == _states[j][k].Transition &&
							 _states[i][k].Reaction == undefinedSymbol) ||
							(_states[i][k].Transition == undefinedSymbol &&
							 _states[i][k].Reaction == _states[j][k].Reaction) ||
							(_states[j][k].Transition == undefinedSymbol &&
							 _states[j][k].Reaction == _states[i][k].Reaction) ||
							(_states[j][k].Reaction == undefinedSymbol &&
							 _states[i][k].Transition == _states[j][k].Transition) ||
							(_states[i][k].Transition == undefinedSymbol &&
							 _states[j][k].Reaction == undefinedSymbol) ||
							(_states[j][k].Transition == undefinedSymbol &&
							 _states[i][k].Reaction == undefinedSymbol))
						{
							clearlyCompatibleCount++;
						}
					}

					if (clearlyCompatibleCount != rows) continue;

					// Если явно совместимы - ставим единицу в бинарной матрице.
					BinaryMatrix[i][j - i - 1] = new BinaryMatrixCell(1, false, true);
				}
			}

			// Проверяем остальные пары.
			for (int i = 0; i < BinaryMatrix.Count; i++)
			{
				for (int j = 0; j < BinaryMatrix[i].Count; j++)
				{
					if (BinaryMatrix[i][j].Value == 1 || BinaryMatrix[i][j].IsClearlyIncompatible) continue;

					MessageBox.Show($"Now checking: {i + 1} {j + 1 + i + 1}");
					BinaryMatrix[i][j] = IsCompatible(i, j) ?
						new BinaryMatrixCell(1) :
						new BinaryMatrixCell(0);
				}
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
			var state1 = _states[row];
			var state2 = _states[column + row + 1];

			if (BinaryMatrix[row][column].IsClearlyCompatible)
			{
				return true;
			}
			if (BinaryMatrix[row][column].IsClearlyIncompatible)
			{
				return false;
			}

			for (int k = 0; k < state1.Count; k++)
			{
				if (state1[k].Transition == TransitionReactionPair.UndefinedSymbol ||
				    state2[k].Transition == TransitionReactionPair.UndefinedSymbol) continue;

				if (state1[k].Transition == state2[k].Transition) continue;

				MessageBox.Show($"{row + 1} {column + 1 + row + 1} -> {state1[k].Transition} {state2[k].Transition}");
				(int newRow, int newColumn) = StateToBinaryCoords(Convert.ToInt32(state1[k].Transition) - 1, Convert.ToInt32(state2[k].Transition) - 1);
				MessageBox.Show($"Recursive checking: {state1[k].Transition} {state2[k].Transition} {IsCompatible(newRow, newColumn)}");

				return IsCompatible(newRow, newColumn);
			}

			return true;
		}

		private static (int row, int column) StateToBinaryCoords(int state1Coords, int state2Coords)
		{
			// Ensure state1Coords is always less than state2Coords for consistency.
			if (state1Coords > state2Coords)
			{
				(state1Coords, state2Coords) = (state2Coords, state1Coords);
			}

			// Calculate the row and column based on the triangular structure of the binary matrix.
			int row = state1Coords;
			int column = state2Coords - state1Coords - 1;

			return (row, column);
		}

		#endregion
	}
}
