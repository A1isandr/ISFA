using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ISFA.MVVM.Models.BinaryMatrixCell;

namespace ISFA.MVVM.Models
{
	public class BinaryMatrixCell(byte? value = null, CellState state = CellState.Initialized)
	{
		#region Properties

		public byte? Value { get; set; } = value;

		public CellState State { get; set; } = state;

		public enum CellState
		{
			Initialized,
			ClearlyIncompatible,
			ClearlyCompatible,
			Final
		}

		#endregion
	}
}
