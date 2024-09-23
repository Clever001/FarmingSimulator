using FarmingClasses.Plants;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FarmingClasses.Collections;

public class Garden<T> : ICollection<T>, IEnumerable<T> where T : Plant {
    private int _count;
    private Node<T>? _root;
    private Node<T>? _end;

    public Garden() {
        _count = 0;
        _root = null;
        _end = null;
    }

    public int Count => _count;

    public bool IsReadOnly => false;

    public void AddFirst(T item) {
        _root = new Node<T>(item, next: _root);
        _count++;
        if (_count == 1) _end = _root;
    }

    public void AddLast(T item) {
        if (_end is null) { // Is empty.
            _root = new Node<T>(item, next: null);
            _end = _root;
        }
        else {
            _end.Next = new Node<T>(item, next: null);
            _end = _end.Next;
        }
        _count++;
    }

    public void Add(T item) => AddLast(item);

    public void Clear() {
        _root = null;
        _end = null;
        _count = 0;
    }

    public bool Contains(T item) {
        for (Node<T>? tmp = _root; tmp != null; tmp = tmp.Next) {
            if (tmp.Current.Equals(item)) return true;
        }
        return false;
    }

    public void CopyTo(T[] array, int arrayIndex) {
        ArgumentNullException.ThrowIfNull(array);
        if (arrayIndex < 0 || arrayIndex >= array.Length) throw new ArgumentOutOfRangeException(nameof(array));
        if (array.Length - arrayIndex < _count) throw new ArgumentException("Недостаточно места в массиве.");

        foreach (T item in this) {
            array[arrayIndex++] = (T)item.Clone(); // Должны ли элементы массива быть клонами?
        }
        //for (Node<T>? tmp = _root; tmp != null; tmp = tmp.Next) {
        //    array[arrayIndex++] = (T)tmp.Current.Clone();
        //}
    }

    //public void Sort() => Sort(delegate (T x, T y) {
    //    return x.Name.CompareTo(y.Name);
    //});

    public void Sort() => Sort((x, y) => x.Name.CompareTo(y.Name));

    //public delegate int Comparison(T x, T y);
    public void Sort(Func<T, T, int> compare) {
        if (_count < 2) return;

        var (first, second) = SplitList();

        first.Sort(compare);
        second.Sort(compare);

        Clear();
        _count = 0;

        using var enumerator1 = first.GetEnumerator();
        using var enumerator2 = second.GetEnumerator();

        bool available1 = enumerator1.MoveNext(), available2 = enumerator2.MoveNext();
        while (available1 || available2) {
            if (!available1) {
                do {
                    AddLast(enumerator2.Current);
                } while (enumerator2.MoveNext());
                break;
            }
            if (!available2) {
                do {
                    AddLast(enumerator1.Current);

                } while (enumerator1.MoveNext());
                break;
            }
            int res = compare(enumerator1.Current, enumerator2.Current);
            if (res < 0) {
                AddLast(enumerator1.Current);
                available1 = enumerator1.MoveNext();
            }
            else {
                AddLast(enumerator2.Current);
                available2 = enumerator2.MoveNext();
            }
        }
    }

    private (Garden<T>, Garden<T>) SplitList() {
        if (_root is null) { // _count == 0
            return (new Garden<T>(), new Garden<T>());
        }
        else if (_count == 1) {
            return (new Garden<T>(), new Garden<T> { _root.Current });
        }

        Node<T>? fast = _root, slow = _root;
        Garden<T> first = new();
        while (fast is not null && fast.Next is not null) {
            fast = fast.Next.Next;
            first.AddLast(slow.Current);
            slow = slow.Next!;
        }

        Garden<T> second = new();
        for (; slow is not null; slow = slow.Next) {
            second.AddLast(slow.Current);
        }

        return (first, second);
    }

    public IEnumerator<T> GetEnumerator() {
        for (Node<T>? tmp = _root; tmp != null; tmp = tmp.Next) {
            yield return tmp.Current;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool Remove(T item) {
        if (_root is null) return false;

        if (_root.Current.Equals(item)) {
            if (_root.Equals(_end)) _end = null; // List contains only one element.
            _root = _root.Next;
            _count--;
            return true;
        }

        Node<T>? prev = _root;
        Node<T>? cur = _root.Next;
        while (cur is not null) {
            if (cur.Current.Equals(item)) {
                if (cur.Equals(_end)) _end = prev; // Check if found element is last.
                prev.Next = cur.Next;
                _count--;
                return true;
            }
            prev = cur;
            cur = cur.Next;
        }
        return false;
    }


    /// <summary>
    /// Удаляет все элементы, удовлетворяющие условию предиката.
    /// </summary>
    /// <param name="predicate">Предикат.</param>
    /// <param name="onRemove">Делегат, служащий для логирования.</param>
    public void RemoveIf(Predicate<T> predicate, Action<T>? onRemove = null) {
        if (_root is null) return;
        if (predicate(_root.Current)) {
            if (onRemove is not null) onRemove(_root.Current);
            if (_root.Equals(_end)) _end = null; // List contains only one element.
            _root = _root.Next;
            _count--;
            return;
        }

        Node<T>? prev = _root;
        Node<T>? cur = _root.Next;
        while (cur is not null) {
            if (predicate(cur.Current)) {
                if (onRemove is not null) onRemove(cur.Current);
                if (cur.Equals(_end)) _end = prev; // Check if found element is last.
                prev.Next = cur.Next;
                cur = cur.Next;
                _count--;
                continue;
            }
            prev = cur;
            cur = cur.Next;
        }
        return;
    }
}
