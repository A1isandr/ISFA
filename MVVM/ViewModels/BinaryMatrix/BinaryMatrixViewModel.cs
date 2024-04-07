using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ISFA.MVVM.ViewModels.BinaryMatrix
{
    public class BinaryMatrixViewModel : ReactiveObject
    {
        #region Singleton

        private static BinaryMatrixViewModel? _instance;

        public static BinaryMatrixViewModel Instance
        {
            get
            {
                _instance ??= new BinaryMatrixViewModel();
                return _instance;
            }
        }

        #endregion

        #region Properties

        public ReactiveCommand<Unit, Unit> CleanCommand { get; }

        [Reactive]
        public ObservableCollection<HeaderViewModel> ColumnHeaders { get; set; } = [];

        [Reactive]
        public ObservableCollection<HeaderViewModel> RowHeaders { get; set; } = [];

        [Reactive]
        public ObservableCollection<StateViewModel> BinaryMatrix { get; set; } = [];

        private readonly ObservableAsPropertyHelper<bool> _isTableNotEmpty;
        public bool IsTableNotEmpty => _isTableNotEmpty.Value;

		#endregion

		#region Constructors

		private BinaryMatrixViewModel()
		{
			CleanCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(_ =>
			{
				ColumnHeaders.Clear();
				RowHeaders.Clear();
				BinaryMatrix.Clear();

				return Observable.Return(Unit.Default);
			});

			_isTableNotEmpty = this
				.WhenAnyValue(x => x.BinaryMatrix.Count)
				.Select(columns => columns != 0)
				.ToProperty(this, x => x.IsTableNotEmpty);
		}

        #endregion
    }
}
