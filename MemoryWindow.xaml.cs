using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CalculatorApp
{
    public partial class MemoryWindow : Window
    {
        private readonly CalculatorViewModel _viewModel;

        public MemoryWindow(ObservableCollection<double> memoryList, CalculatorViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel; // Legăm fereastra la ViewModel
        }

        private void MemoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MemoryListBox.SelectedItem != null)
            {
                _viewModel.UseMemoryValue((double)MemoryListBox.SelectedItem);
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
