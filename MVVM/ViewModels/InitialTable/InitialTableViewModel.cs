using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive;
using System.Reactive.Linq;

namespace ISFA.MVVM.ViewModels.InitialTable
{
	public class InitialTableViewModel : ReactiveObject
    {
        #region Singleton

        private static InitialTableViewModel? _instance;

        public static InitialTableViewModel Instance
        {
            get
            {
                _instance ??= new InitialTableViewModel();
                return _instance;
            }
        }

        #endregion

        #region Properties

        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

        public ReactiveCommand<Unit, Unit> NewColumnCommand { get; }
        public ReactiveCommand<Unit, Unit> NewRowCommand { get; }
        public ReactiveCommand<Unit, Unit> CleanCommand { get; }

        [Reactive]
        public ObservableCollection<HeaderViewModel> ColumnHeaders { get; set; } = [];

        [Reactive]
        public ObservableCollection<HeaderViewModel> RowHeaders { get; set; } = [];

        [Reactive]
        public ObservableCollection<StateViewModel> States { get; set; } = [];

        private readonly ObservableAsPropertyHelper<bool> _isTableEmpty;
        public bool IsTableEmpty => _isTableEmpty.Value;

        #endregion

        #region Constructors

        private InitialTableViewModel()
        {
            NewColumnCommand = ReactiveCommand.CreateFromTask(AddNewColumn);

            NewRowCommand = ReactiveCommand.CreateFromTask(AddNewRow);

            CleanCommand = ReactiveCommand.CreateFromObservable<Unit, Unit>(_ =>
            {
                ColumnHeaders.Clear();
                RowHeaders.Clear();
                States.Clear();

                return Observable.Return(Unit.Default);
            });

            ColumnHeaders
                .CollectionChanged += DeleteRowOrColumn;

            RowHeaders
                .CollectionChanged += DeleteRowOrColumn;

            _isTableEmpty = this
                .WhenAnyValue(x => x.States.Count)
                .Select(columns => columns == 0)
                .ToProperty(this, x => x.IsTableEmpty);
        }

        #endregion

        #region Methods

        private string GenerateColumnLabel(int index)
        {
            return index == 0 ?
                "1" :
                $"{Convert.ToInt32(ColumnHeaders[index - 1].InitialName) + 1}";
        }

        private string GenerateRowLabel(int index)
        {
            if (index == 0) return Alphabet[0].ToString();

            char[] chars = RowHeaders[index - 1].InitialName.ToCharArray();
            bool carry = true;

            for (int i = chars.Length - 1; i >= 0; i--)
            {
                if (!carry) continue;

                int alphabetIndex = Array.IndexOf(Alphabet.ToCharArray(), chars[i]);

                if (alphabetIndex == Alphabet.Length - 1)
                {
                    chars[i] = Alphabet[0];
                }
                else
                {
                    chars[i] = Alphabet[alphabetIndex + 1];
                    carry = false;
                }
            }

            if (carry)
                return Alphabet[0] + new string(chars);

            return new string(chars);
        }

        private async Task AddNewColumn()
        {
            ColumnHeaders.Add(new HeaderViewModel(HeaderViewModel.HeaderType.Column, GenerateColumnLabel(ColumnHeaders.Count)));
            States.Add(new StateViewModel(ColumnHeaders.Last()));

            foreach (var row in RowHeaders)
            {
                States.Last().TransitionReactionPairs.Add(new TransitionReactionPairViewModel(ColumnHeaders.Last(), row));

                // Чтобы клетки красиво появлялись одна за другой.
                await Task.Delay(100 / RowHeaders.Count);
            }
        }

        private async Task AddNewRow()
        {
            RowHeaders.Add(new HeaderViewModel(HeaderViewModel.HeaderType.Row, GenerateRowLabel(RowHeaders.Count)));

            for (int i = 0; i < States.Count; i++)
            {
                States[i].TransitionReactionPairs.Add(new TransitionReactionPairViewModel(ColumnHeaders[i], RowHeaders.Last()));

                // Чтобы клетки красиво появлялись одна за другой.
                await Task.Delay(100 / ColumnHeaders.Count);
            }
        }

        private void DeleteRowOrColumn(object? source, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems is null) return;

            if (e.OldItems.Count == 0) return;

            foreach (var item in e.OldItems)
            {
                if (item is not HeaderViewModel header) continue;

                switch (header.Type)
                {
                    case HeaderViewModel.HeaderType.Column:
                        States.Remove(States.First(x => x.Header == header));
                        break;
                    case HeaderViewModel.HeaderType.Row:
                        {
                            foreach (var column in States)
                            {
                                column.TransitionReactionPairs.Remove(column.TransitionReactionPairs.First(x => x.Row == header));
                            }

                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(e));
                }
            }
        }

        #endregion
    }
}
