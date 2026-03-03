using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Task1.Infrastructure;
using Task1.Models;

namespace Task1.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly MyStack<string> _stack = new();
    private readonly RelayCommand _pushCommand;
    private readonly RelayCommand _popCommand;
    private readonly RelayCommand _peekCommand;
    private string _inputValue = string.Empty;
    private string _statusMessage = "Стек пуст. Добавьте первый элемент.";

    public MainWindowViewModel()
    {
        Items = new ObservableCollection<string>();

        _pushCommand = new RelayCommand(PushItem, CanPushItem);
        _popCommand = new RelayCommand(PopItem, CanReadStack);
        _peekCommand = new RelayCommand(PeekItem, CanReadStack);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<string> Items { get; }

    public string InputValue
    {
        get => _inputValue;
        set
        {
            if (_inputValue == value)
            {
                return;
            }

            _inputValue = value;
            OnPropertyChanged();
            _pushCommand.RaiseCanExecuteChanged();
        }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        private set
        {
            if (_statusMessage == value)
            {
                return;
            }

            _statusMessage = value;
            OnPropertyChanged();
        }
    }

    public int Count => _stack.Count;

    public ICommand PushCommand => _pushCommand;

    public ICommand PopCommand => _popCommand;

    public ICommand PeekCommand => _peekCommand;

    private bool CanPushItem()
    {
        return !string.IsNullOrWhiteSpace(InputValue);
    }

    private bool CanReadStack()
    {
        return _stack.Count > 0;
    }

    private void PushItem()
    {
        string valueToPush = InputValue.Trim();
        _stack.Push(valueToPush);
        InputValue = string.Empty;
        StatusMessage = $"Push: добавлен элемент '{valueToPush}'.";
        RefreshState();
    }

    private void PopItem()
    {
        try
        {
            string removed = _stack.Pop();
            StatusMessage = $"Pop: взят элемент '{removed}'.";
            RefreshState();
        }
        catch (InvalidOperationException exception)
        {
            StatusMessage = $"Ошибка Pop: {exception.Message}";
        }
    }

    private void PeekItem()
    {
        try
        {
            string top = _stack.Peek();
            StatusMessage = $"Peek: верхний элемент '{top}'.";
            RefreshState();
        }
        catch (InvalidOperationException exception)
        {
            StatusMessage = $"Ошибка Peek: {exception.Message}";
        }
    }

    private void RefreshState()
    {
        Items.Clear();

        foreach (string item in _stack.ToTopFirstList())
        {
            Items.Add(item);
        }

        OnPropertyChanged(nameof(Count));
        _popCommand.RaiseCanExecuteChanged();
        _peekCommand.RaiseCanExecuteChanged();
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
