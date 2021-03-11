using System;

namespace InitDta
{
    public interface DoubleLinkedList<T>
    {
        DoubleLinkedListNode<T> First { get; set; }

        DoubleLinkedListNode<T> Last { get; set; }

        void Reverse();//insert new DoubleLinkedListNode with give value at the start of the list

        void AddFirst(T value);//insert new DoubleLinkedListNode with given value at the end of the list

        void AddLast(T value);
    }
}
