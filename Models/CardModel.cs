using System.ComponentModel;

namespace MemoryGame.Models
{
    public class CardModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Symbol { get; set; }

        private bool _isFlipped;
        public bool IsFlipped
        {
            get => _isFlipped;
            set
            {
                _isFlipped = value;
                OnPropertyChanged(nameof(IsFlipped));
            }
        }

        private bool _isMatched;
        public bool IsMatched
        {
            get => _isMatched;
            set
            {
                _isMatched = value;
                OnPropertyChanged(nameof(IsMatched));
            }
        }

        public CardModel(int id, string symbol)
        {
            Id = id;
            Symbol = symbol;
            IsFlipped = false;
            IsMatched = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}


