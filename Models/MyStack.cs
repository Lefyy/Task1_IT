using System;
using System.Collections.Generic;

namespace Task1.Models;

public class MyStack<T>
{
    private readonly List<T> _items = new();

    public int Count => _items.Count;

    public void Push(T item)
    {
        _items.Add(item);
    }

    public T Pop()
    {
        if (_items.Count == 0)
        {
            throw new InvalidOperationException("Stack is empty.");
        }

        T item = _items[^1];
        _items.RemoveAt(_items.Count - 1);
        return item;
    }

    public T Peek()
    {
        if (_items.Count == 0)
        {
            throw new InvalidOperationException("Stack is empty.");
        }

        return _items[^1];
    }

    public IReadOnlyList<T> ToTopFirstList()
    {
        var result = new List<T>(_items.Count);

        for (int i = _items.Count - 1; i >= 0; i--)
        {
            result.Add(_items[i]);
        }

        return result;
    }
}
