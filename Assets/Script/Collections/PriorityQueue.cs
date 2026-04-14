using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    private IComparer<T> comparer;
    public PriorityQueue(IComparer<T> comparer)
    {
        this.comparer = comparer;
    }

    private readonly List<T> heap;
    public int Count => heap.Count;

    public bool Enqueue(T element)
    {
        heap.Add(element);

        int index = heap.Count - 1;

        while (index > 0)
        {
            int parent = (index - 1) / 2;

            if (comparer.Compare(heap[index], heap[parent]) >= 0)
            {
                return true;
            }

            (heap[index], heap[parent]) = (heap[parent], heap[index]);
            index = parent;
        }

        return false;
    }

    public bool TryDequeue(out T element)
    {
        if (heap.Count == 0)
        {
            element = default;
            return false;
        }

        element = heap[0];

        int heapCount = heap.Count;
        heap[0] = heap[heapCount - 1];
        heap.RemoveAt(heapCount - 1);

        int index = 0;
        --heapCount;

        while (true)
        {
            int left = 2 * index + 1;
            int right = 2 * index + 2;
            int current = index;

            if (left < heapCount && comparer.Compare(heap[left], heap[current]) < 0)
            {
                current = left;
            }
            if (right < heapCount && comparer.Compare(heap[right], heap[current]) < 0)
            {
                current = right;
            }
            if (current == index)
            {
                return true;
            }

            (heap[index], heap[current]) = (heap[current], heap[index]);
            index = current;
        }
    }
}
