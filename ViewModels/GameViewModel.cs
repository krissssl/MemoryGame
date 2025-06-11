using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using MemoryGame.Models;
using MemoryGame.Utilities;

namespace MemoryGame.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private int _totalPairs = 8;

        private CardModel _firstFlippedCard;
        private CardModel _secondFlippedCard;
        private System.Timers.Timer _flipBackTimer;
        private System.Timers.Timer _gameTimer;

        public ObservableCollection<CardModel> Cards { get; set; }
        public ICommand FlipCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private int _moves;
        public int Moves
        {
            get => _moves;
            set
            {
                _moves = value;
                OnPropertyChanged(nameof(Moves));
            }
        }

        private int _matchedPairs;
        public int MatchedPairs
        {
            get => _matchedPairs;
            set
            {
                _matchedPairs = value;
                OnPropertyChanged(nameof(MatchedPairs));
            }
        }

        private int _elapsedSeconds;
        public int ElapsedSeconds
        {
            get => _elapsedSeconds;
            set
            {
                _elapsedSeconds = value;
                OnPropertyChanged(nameof(ElapsedSeconds));
            }
        }

        public GameViewModel()
        {
            Cards = new ObservableCollection<CardModel>();
            GenerateCards();

            FlipCommand = new RelayCommand(param =>
            {
                if (param is CardModel card)
                {
                    FlipCard(card);
                }
            });

            _flipBackTimer = new System.Timers.Timer(1000);
            _flipBackTimer.Elapsed += FlipBackCards;

            _gameTimer = new System.Timers.Timer(1000);
            _gameTimer.Elapsed += (s, e) =>
            {
                ElapsedSeconds++;
            };
            _gameTimer.Start();
        }

        private void GenerateCards()
        {
            Cards.Clear();
            var symbols = new[]
            {
                "🍎", "🍌", "🍇", "🍓", "🍍", "🥝", "🍑", "🍉",
                "🍋", "🍒", "🍏", "🍈", "🍊", "🥥", "🥭", "🍅",
                "🥕", "🍆", "🥔", "🥬", "🌽", "🥦", "🧄", "🧅",
                "🥜", "🍠", "🍞", "🧀", "🍗", "🥩", "🍤", "🍪",
                "🍩", "🍰", "🍿", "🍫", "🍯", "🥨", "🥟", "🍙"
            };

            var selectedSymbols = symbols.Take(_totalPairs).ToList();
            var allSymbols = selectedSymbols.Concat(selectedSymbols).OrderBy(x => Guid.NewGuid()).ToList();

            for (int i = 0; i < _totalPairs * 2; i++)
            {
                Cards.Add(new CardModel(i / 2, allSymbols[i]));
            }
        }

        public void FlipCard(CardModel card)
        {
            if (card.IsFlipped || card.IsMatched || _secondFlippedCard != null)
                return;

            card.IsFlipped = true;

            if (_firstFlippedCard == null)
            {
                _firstFlippedCard = card;
            }
            else
            {
                _secondFlippedCard = card;

                if (_firstFlippedCard.Symbol == _secondFlippedCard.Symbol &&
                    _firstFlippedCard != _secondFlippedCard)
                {
                    _firstFlippedCard.IsMatched = true;
                    _secondFlippedCard.IsMatched = true;

                    MatchedPairs++;

                    if (MatchedPairs == _totalPairs)
                    {
                        _gameTimer.Stop();
                        MessageBox.Show($"Congratulations! You have won for {Moves + 1} moves and {ElapsedSeconds} seconds!", "Win! 🎉");
                    }

                    ResetFlipState();
                }
                else
                {
                    _flipBackTimer.Start();
                }

                Moves++;
            }
        }

        private void FlipBackCards(object sender, ElapsedEventArgs e)
        {
            _flipBackTimer.Stop();
            App.Current.Dispatcher.Invoke(() =>
            {
                if (_firstFlippedCard != null && !_firstFlippedCard.IsMatched)
                    _firstFlippedCard.IsFlipped = false;

                if (_secondFlippedCard != null && !_secondFlippedCard.IsMatched)
                    _secondFlippedCard.IsFlipped = false;

                ResetFlipState();
            });
        }

        private void ResetFlipState()
        {
            _firstFlippedCard = null;
            _secondFlippedCard = null;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void StartNewGame(int pairCount)
        {
            _totalPairs = pairCount;
            _firstFlippedCard = null;
            _secondFlippedCard = null;
            Moves = 0;
            MatchedPairs = 0;
            ElapsedSeconds = 0;

            _gameTimer?.Stop();
            _gameTimer?.Start();

            GenerateCards();
        }
    }
}




