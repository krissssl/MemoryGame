using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MemoryGame.Models;
using MemoryGame.ViewModels;

namespace MemoryGame
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CardButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is CardModel card)
            {
                if (DataContext is GameViewModel vm)
                {
                    vm.FlipCard(card);
                }
            }
        }

        private void DifficultyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is GameViewModel vm && DifficultyComboBox.SelectedItem is ComboBoxItem item)
            {
                int pairs = int.Parse(item.Tag.ToString() ?? "8");
                vm.StartNewGame(pairs);

                // Let's update the number of columns in the grid (UniformGrid)
                var grid = FindVisualChild<UniformGrid>(this);
                if (grid != null)
                {
                    grid.Columns = (int)Math.Sqrt(pairs * 2);
                }
            }
        }

        private static T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T tChild)
                    return tChild;

                var result = FindVisualChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}


