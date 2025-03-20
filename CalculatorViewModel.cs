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
        UnaryOperationCommand = new RelayCommand(param => PerformUnaryOperation(param.ToString()));
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
        if (!_isNewNumber)
        {
            double inputNumber = double.Parse(DisplayText);

            if (_currentOperator != null)
            {
                // Executăm operația anterioară și salvăm rezultatul intermediar
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

    private void CalculateResult()
    {
        if (_currentOperator != null && !_isNewNumber)
        {
            double inputNumber = double.Parse(DisplayText);
            _currentResult = _calculatorModel.PerformOperation(_currentResult, inputNumber, _currentOperator);
            DisplayText = _currentResult.ToString();
            _currentOperator = null; // Resetăm operatorul pentru a permite o nouă operație
        }

        _isNewNumber = true;
    }
    private void Backspace()
    {
        if (DisplayText.Length > 1)
            DisplayText = DisplayText.Substring(0, DisplayText.Length - 1);
        else
            DisplayText = "0";
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
    private void PerformUnaryOperation(string operation)
    {
        try
        {
            double value = double.Parse(DisplayText);
            double result = _calculatorModel.PerformUnaryOperation(value, operation);
            DisplayText = result.ToString();
        }
        catch (Exception ex)
        {
            DisplayText = "Eroare";
        }
        _isNewNumber = true;
    }
    private void MemoryOperation(string operation)
    {
        double value = double.Parse(DisplayText);

        switch (operation)
        {
            case "MC": // Memory Clear
                MemoryList.Clear();
                break;

            case "MS": // Memory Store (Adaugă în memorie)
                MemoryList.Add(value);
                break;

            case "MR": // Memory Recall (Ultima valoare din memorie)
                if (MemoryList.Count > 0)
                    DisplayText = MemoryList[^1].ToString();
                break;

            case "M+": // Memory Add
                if (MemoryList.Count > 0)
                {
                    MemoryList[^1] += value;
                }
                else
                {
                    MemoryList.Add(value);
                }
                break;

            case "M-": // Memory Subtract
                if (MemoryList.Count > 0)
                {
                    MemoryList[^1] -= value;
                }
                else
                {
                    MemoryList.Add(-value);
                }
                break;

            case "M>": // Deschide fereastra de memorie
                OpenMemoryWindow();
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
