using System;
using System.Collections;
using System.Collections.Generic;

namespace Suhock.X32.Types.Sets;

public class BitSet : ISet<int>
{
    public int Bits { get; private set; } = 0;

    public BitSet(int maxValue)
    {
        MaxValue = maxValue;
    }

    public BitSet(int maxValue, int bits)
    {
        if (bits >> maxValue != 0)
        {
            throw new ArgumentOutOfRangeException(nameof(bits), bits, "Bit set out of range");
        }

        MaxValue = maxValue;
        Bits = bits;
    }

    public BitSet(int maxValue, ICollection<int> items)
    {
        if (items is null)
        {
            throw new ArgumentNullException(nameof(items));
        }

        MaxValue = maxValue;

        foreach (var item in items)
        {
            Add(item);
        }
    }

    public int MaxValue { get; }

    public int Count
    {
        get
        {
            var count = 0;
            var value = Bits;

            while (value != 0)
            {
                if ((value & 1) == 1)
                {
                    count++;
                }

                value >>= 1;
            }

            return count;
        }
    }

    public bool IsReadOnly => false;

    public bool Add(int item)
    {
        if (item < 1 || item > MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(item), item, "Must be between 1 and " + MaxValue);
        }

        if (Contains(item))
        {
            return false;
        }

        Bits |= 1 << item - 1;

        return true;
    }

    public void Clear()
    {
        Bits = 0;
    }

    public bool Contains(int item)
    {
        return item >= 1 && item <= MaxValue && (Bits >> item - 1 & 1) == 1;
    }

    public void CopyTo(int[] array, int arrayIndex)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        if (array.Length - arrayIndex < Count)
        {
            throw new ArgumentOutOfRangeException(nameof(array), "Array too small");
        }

        var i = arrayIndex;

        foreach (var item in this)
        {
            array[i++] = item;
        }
    }

    public void ExceptWith(IEnumerable<int> other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        foreach (var item in other)
        {
            Remove(item);
        }
    }

    public IEnumerator<int> GetEnumerator()
    {
        return new BitwiseSetEnumerator(this);
    }

    public void IntersectWith(IEnumerable<int> other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        Bits &= GetValueFromCollection(other);
    }

    public bool IsProperSubsetOf(IEnumerable<int> other)
    {
        return IsSubsetOf(other) && !SetEquals(other);
    }

    public bool IsProperSupersetOf(IEnumerable<int> other)
    {
        return IsSupersetOf(other) && !SetEquals(other);
    }

    public bool IsSubsetOf(IEnumerable<int> other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        return (Bits & ~GetValueFromCollection(other)) == 0;
    }

    public bool IsSupersetOf(IEnumerable<int> other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        return (GetValueFromCollection(other) & ~Bits) == 0;
    }

    public bool Overlaps(IEnumerable<int> other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        foreach (var item in other)
        {
            if (Contains(item))
            {
                return true;
            }
        }

        return false;
    }

    public bool Remove(int item)
    {
        if (!Contains(item))
        {
            return false;
        }

        Bits &= ~(1 << item - 1);

        return true;
    }

    public bool SetEquals(IEnumerable<int> other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        return Bits == GetValueFromCollection(other);
    }

    public void SymmetricExceptWith(IEnumerable<int> other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        Bits ^= GetValueFromCollection(other);
    }

    public void UnionWith(IEnumerable<int> other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        foreach (var item in other)
        {
            Add(item);
        }
    }

    void ICollection<int>.Add(int item)
    {
        Add(item);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private bool IsInRange(int item)
    {
        return item >= 1 && item <= MaxValue;
    }

    private int GetValueFromCollection(IEnumerable<int> items)
    {
        if (items.GetType() == typeof(BitSet))
        {
            return ((BitSet)items).Bits;
        }

        var value = 0;

        foreach (var item in items)
        {
            if (!IsInRange(item))
            {
                throw new ArgumentOutOfRangeException(nameof(items), item, "Must be between 1 and " + MaxValue);
            }

            value |= 1 << item - 1;
        }

        return value;
    }
}

#pragma warning disable CA1063 // Implement IDisposable Correctly
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
public class BitwiseSetEnumerator : IEnumerator<int>
{
    private readonly BitSet _set;

    private int _currentIndex;

    public BitwiseSetEnumerator(BitSet set)
    {
        _set = set;
        Reset();
    }

    public int Current { get; private set; }

    object IEnumerator.Current => Current;

    public void Dispose() { }

    public bool MoveNext()
    {
        if (++_currentIndex > _set.MaxValue || _set.Bits >> _currentIndex == 0)
        {
            return false;
        }

        while ((_set.Bits >> _currentIndex & 1) == 0)
        {
            ++_currentIndex;
        }

        Current = _currentIndex + 1;

        return true;
    }

    public void Reset()
    {
        _currentIndex = -1;
    }
}