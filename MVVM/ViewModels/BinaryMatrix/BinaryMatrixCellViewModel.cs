using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ISFA.MVVM.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ISFA.MVVM.ViewModels.BinaryMatrix
{
	public class BinaryMatrixCellViewModel : ReactiveObject
	{
		#region Properties

		[Reactive] 
		public BinaryMatrixCell BinaryMatrixCell { get; set; }

		private readonly ObservableAsPropertyHelper<SolidColorBrush> _foregroundBrush;
		public SolidColorBrush ForegroundBrush => _foregroundBrush.Value;

		#endregion

		#region Constructors

		public BinaryMatrixCellViewModel(BinaryMatrixCell binaryMatrixCell)
		{
			BinaryMatrixCell = binaryMatrixCell;

			_foregroundBrush = this
				.WhenAnyValue(x => x.BinaryMatrixCell)
				.Select(cell =>
					cell.State switch
					{
						BinaryMatrixCell.CellState.ClearlyCompatible => new SolidColorBrush(Color.FromRgb(36, 191, 78)),
						BinaryMatrixCell.CellState.ClearlyIncompatible => new SolidColorBrush(Color.FromRgb(199, 30, 44)),
						_ => new SolidColorBrush(Color.FromRgb(255, 255, 255))
					})
				.ToProperty(this, x => x.ForegroundBrush);
		}

		#endregion
	}
}
