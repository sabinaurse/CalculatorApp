using CalculatorApp;
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
public class CalculatorViewModel : INotifyPropertyChanged
{
    private string _displayText = "0";
    private double _currentResult = 0;
    private string _currentOperator = null;
    private bool _isNewNumber = true;

    private readonly CalculatorModel _calculatorModel = new();
    public ObservableCollection<double> MemoryList { get; private set; } = new(); // Stocare memorie
    private MemoryWindow _memoryWindow; // Referință la fereastra de memorie
    public string DisplayText
    {
        get => _displayText;
        set
        {
            _displayText = value;
            OnPropertyChanged(nameof(DisplayText));
        }
    }

    public ICommand NumberCommand { get; }
    public ICommand OperationCommand { get; }
    public ICommand EqualsCommand { get; }
    public ICommand ClearCommand { get; }
    public ICommand UnaryOperationCommand { get; }
    public ICommand BackspaceCommand { get; }
    public ICommand ClearEntryCommand { get; }
    public ICommand MemoryCommand { get; }
    public ICommand OpenMemoryWindowCommand { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    public CalculatorViewModel()
    {
        NumberCommand = new RelayCommand(param => AddNumber(param.ToString()));
        OperationCommand = new RelayCommand(param => SetOperation(param.ToString()));
        EqualsCommand = new RelayCommand(_ => CalculateResult());
        ClearCommand = new RelayCommand(_ => Clear());
        UnaryOperationCommand = new RelayCommand(param => ApplyUnaryOperation(param.ToString()));
        ClearEntryCommand = new RelayCommand(_ => ClearEntry());
        BackspaceCommand = new RelayCommand(_ => Backspace());
        OpenMemoryWindowCommand = new RelayCommand(_ => OpenMemoryWindow());
        MemoryCommand = new RelayCommand(param => MemoryOperation(param.ToString()));
    }

    private void AddNumber(string number)
    {
        if (_isNewNumber)
        {
            DisplayText = number;
            _isNewNumber = false;
        }
        else
        {
            DisplayText += number;
        }
    }

    private void SetOperation(string operation)
    {
        try
        {
            if (!_isNewNumber)
            {
                double inputNumber = double.Parse(DisplayText);

                if (_currentOperator != null)
                {
                    _currentResult = _calculatorModel.PerformOperation(_currentResult, inputNumber, _currentOperator);
                    DisplayText = _currentResult.ToString();
                }
                else
                {
                    _currentResult = inputNumber;
                }
            }

            _currentOperator = operation;
            _isNewNumber = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Eroare la setarea operatorului: " + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            DisplayText = "0";
        }
    }


    private void CalculateResult()
    {
        try
        {
            if (_currentOperator == null || _isNewNumber)
                return;

            double inputNumber = double.Parse(DisplayText);
            _currentResult = _calculatorModel.PerformOperation(_currentResult, inputNumber, _currentOperator);
            DisplayText = _currentResult.ToString();

            _currentOperator = null;  // Resetăm operatorul
            _isNewNumber = true;      // Următorul număr trebuie să fie nou
        }
        catch (Exception ex)
        {
            MessageBox.Show("Eroare la calcul: " + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            DisplayText = "0";
        }
    }

    private void Backspace()
    {
        if (DisplayText.Length > 1)
            DisplayText = DisplayText.Substring(0, DisplayText.Length - 1);
        else
            DisplayText = "0"; // Evită un display gol
    }


    private void ClearEntry()
    {
        DisplayText = "0";
        _isNewNumber = true;
    }

    private void Clear()
    {
        DisplayText = "0";
        _currentResult = 0;
        _currentOperator = null;
        _isNewNumber = true;
    }
    private void ApplyUnaryOperation(string operation)
    {
        try
        {
            double value = double.TryParse(DisplayText, out double result) ? result : 0;
            value = _calculatorModel.PerformUnaryOperation(value, operation);
            DisplayText = value.ToString();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Eroare la aplicarea operației unare: " + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            DisplayText = "0";
        }
    }
    private void MemoryOperation(string operation)
    {
        double value;
        switch (operation)
        {
            case "MC": // Memory Clear
                MemoryList.Clear();
                break;

            case "MS": // Memory Store
                value = double.Parse(DisplayText);
                MemoryList.Add(value);
                break;

            case "MR": // Memory Recall
                if (MemoryList.Count > 0)
                    DisplayText = MemoryList[^1].ToString();
                else
                    MessageBox.Show("Memoria este goală!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                break;

            case "M+": // Memory Add
                if (MemoryList.Count > 0)
                    MemoryList[^1] += double.Parse(DisplayText);
                else
                    MemoryList.Add(double.Parse(DisplayText));
                break;

            case "M-": // Memory Subtract
                if (MemoryList.Count > 0)
                    MemoryList[^1] -= double.Parse(DisplayText);
                else
                    MessageBox.Show("Memoria este goală!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                break;
        }
    }


    private void OpenMemoryWindow()
    {
        if (_memoryWindow == null || !_memoryWindow.IsVisible)
        {
            _memoryWindow = new MemoryWindow(MemoryList, this);
            _memoryWindow.Show();
        }
        else
        {
            _memoryWindow.Focus();
        }
    }

    public void UseMemoryValue(double value)
    {
        DisplayText = value.ToString();
        _isNewNumber = true;
    }
    protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
