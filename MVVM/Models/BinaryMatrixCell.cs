using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISFA.MVVM.Models
{
	public class BinaryMatrixCell(byte value, bool isClearlyIncompatible = false, bool isClearlyCompatible = false)
	{
		#region Properties

		public byte Value { get; set; } = value;

		public bool IsClearlyIncompatible { get; set; } = isClearlyIncompatible;

		public bool IsClearlyCompatible { get; set; } = isClearlyCompatible;

		#endregion
	}
}
