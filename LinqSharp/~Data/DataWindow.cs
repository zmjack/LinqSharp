using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public class DataWindow<T>
    {
        public DataWindow(T[] data, int index)
        {
            if (data is null) throw new ArgumentException("The data can not be null.", nameof(data));
            if (index < 0 || index >= data.Length) throw new ArgumentOutOfRangeException($"The index({index}) is out of range(0 to {data.Length - 1}).", nameof(index));

            Data = data;
            Index = index;
        }

        public T[] Data { get; }
        public int Index { get; }

        private bool IsInRange(int index) => index >= 0 && index < Data.Length;

        public T this[int offset]
        {
            get
            {
                var index = Index + offset;
                return IsInRange(index) ? Data[index] : default;
            }
        }

        public T[] this[int startOffset, int endOffset, bool useDefaultIfOverflow]
        {
            get
            {
                IEnumerable<T> GetRows()
                {
                    for (int i = startOffset; i < endOffset; i++)
                    {
                        var index = Index + i;

                        if (IsInRange(index)) yield return Data[index];
                        else if (useDefaultIfOverflow) yield return default;
                    }
                }
                return GetRows().ToArray();

            }
        }

        public T[] TakeArray(int offset, int length, bool useDefaultIfOverflow)
        {
            return this[offset, offset + length, useDefaultIfOverflow];
        }
    }
}
