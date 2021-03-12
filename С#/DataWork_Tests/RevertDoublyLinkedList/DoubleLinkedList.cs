using System.Collections.Generic;
using System.Collections;

namespace RevertDoublyLinkedList
{
    public class DoubleLinkedList<T> : IEnumerable<T>  // двусвязный список
    {
        DoubleLinkedListNode<T> First; // головной/первый элемент
        DoubleLinkedListNode<T> Last; // последний/хвостовой элемент
        int count;  // количество элементов в списке

        //insert new DoubleLinkedListNode with given value at the end of the list
        public void AddLast(T value)
        {
            DoubleLinkedListNode<T> node = new DoubleLinkedListNode<T>(value);

            if (First == null)
                First = node;
            else
            {
                Last.Next = node;
                node.Prev = Last;
            }
            Last = node;
            count++;
        }

        //insert new DoubleLinkedListNode with give value at the start of the list
        public void AddFirst(T value)
        {
            DoubleLinkedListNode<T> node = new DoubleLinkedListNode<T>(value);
            DoubleLinkedListNode<T> temp = First;
            node.Next = temp;
            First = node;
            if (count == 0)
                Last = First;
            else
                temp.Prev = node;
            count++;
        }

        public int Count { get { return count; } }
        public bool IsEmpty { get { return count == 0; } }

        public bool Contains(T value)
        {
            DoubleLinkedListNode<T> current = First;
            while (current != null)
            {
                if (current.Value.Equals(value))
                    return true;
                current = current.Next;
            }
            return false;
        }

        public void Reverse()
        {
            DoubleLinkedListNode<T> temp = null;
            DoubleLinkedListNode<T> current = First;

            while (current != null)
            {
                temp = current.Prev;
                current.Prev = current.Next;
                current.Next = temp;
                current = current.Prev;
            }

            if (temp != null)
            {
                First = temp.Prev;
            }

        }


        // удаление
        public bool Remove(T value)
        {
            DoubleLinkedListNode<T> current = First;

            // поиск удаляемого узла
            while (current != null)
            {
                if (current.Value.Equals(value))
                {
                    break;
                }
                current = current.Next;
            }
            if (current != null)
            {
                // если узел не последний
                if (current.Next != null)
                {
                    current.Next.Prev = current.Prev;
                }
                else
                {
                    // если последний, переустанавливаем tail
                    Last = current.Prev;
                }

                // если узел не первый
                if (current.Prev != null)
                {
                    current.Prev.Next = current.Next;
                }
                else
                {
                    // если первый, переустанавливаем head
                    First = current.Next;
                }
                count--;
                return true;
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            DoubleLinkedListNode<T> current = First;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }
    }
}