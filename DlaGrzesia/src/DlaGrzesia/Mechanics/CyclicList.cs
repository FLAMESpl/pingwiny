using DlaGrzesia.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

namespace DlaGrzesia.Mechanics
{
    public class CyclicList<T>
    {
        private readonly T[] items;
        private int currentIndex = 0;

        public CyclicList(int size) : this(size, default) { }

        public CyclicList(int size, T @default)
        {
            items = new T[size];
            Array.Fill(items, @default);
        }

        public IEnumerable<T> ReadAll()
        {
            for (var i = currentIndex; i < items.Length; i++)
                yield return items[i];

            for (var i = 0; i < currentIndex; i++)
                yield return items[i];
        }

        public void Write(T item)
        {
            items[currentIndex++] = item;
            if (currentIndex == items.Length)
                currentIndex = 0;
        }

        public void Deserialize(Stream stream, Func<Stream, T> reader)
        {
            currentIndex = stream.ReadInt();
            for (var i = 0; i < items.Length; i++)
                items[i] = reader(stream);
        }

        public void Serialize(Stream stream, Action<Stream, T> writer)
        {
            stream.WriteInt(currentIndex);
            foreach (var item in items)
                writer(stream, item);
        }
    }
}
