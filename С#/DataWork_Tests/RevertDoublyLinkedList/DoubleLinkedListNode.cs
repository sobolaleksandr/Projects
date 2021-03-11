using System;
using System.Collections.Generic;
using System.Text;

namespace RevertDoublyLinkedList
{
    public class DoubleLinkedListNode<T>
    {
        public DoubleLinkedListNode(T value)
        {
            Value = value;
        }
        public T Value { get; set; }
        public DoubleLinkedListNode<T> Prev { get; set; }
        public DoubleLinkedListNode<T> Next { get; set; }
    }
}
