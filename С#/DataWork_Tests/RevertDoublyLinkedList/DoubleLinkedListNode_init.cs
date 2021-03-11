using System;
using System.Collections.Generic;
using System.Text;

namespace InitDta
{
    public interface DoubleLinkedListNode<T>
    {
        T Value { get; set; }

        DoubleLinkedListNode<T> Next { get; set; }

        DoubleLinkedListNode<T> Prev { get; set; }
    }
}
