using System;
using System.Collections.Generic;
using Xunit;

namespace RevertDoublyLinkedList.Tests
{
    public class DoubleLinkedListTests
    {
        [Fact]
        public void AddLast()
        {
            DoubleLinkedList<int> linkedList = new DoubleLinkedList<int>();

            linkedList.AddLast(1);
            Assert.Equal(1, linkedList.Count);
            Assert.True(linkedList.Contains(1));

            linkedList.AddLast(2);
            Assert.Equal(2, linkedList.Count);

            int i = 1;

            foreach (var item in linkedList)
            { 
                Assert.Equal(i, item);
                i++; 
            }
        }

        [Fact]
        public void AddFirst()
        {
            DoubleLinkedList<int> linkedList = new DoubleLinkedList<int>();

            linkedList.AddFirst(1);
            Assert.Equal(1, linkedList.Count);
            Assert.True(linkedList.Contains(1));

            linkedList.AddFirst(2);
            Assert.Equal(2, linkedList.Count);

            int i = 2;

            foreach (var item in linkedList)
            {
                Assert.Equal(i, item);
                i--;
            }
        }

        [Fact]
        public void Remove_WithItemInList_ReturnsTrue()
        {
            DoubleLinkedList<int> linkedList = new DoubleLinkedList<int>();

            linkedList.AddFirst(1);

            Assert.True(linkedList.Remove(1));
        }

        [Fact]
        public void Remove_WithEmptyList_ReturnsFalse()
        {
            DoubleLinkedList<int> linkedList = new DoubleLinkedList<int>();

            Assert.False(linkedList.Remove(1));
        }

        [Fact]
        public void Reverse_WithNonEmptyList()
        {
            DoubleLinkedList<int> linkedList = new DoubleLinkedList<int>();

            int j = 0;
            for (; j <= 10; j++)
                linkedList.AddLast(j);

            linkedList.Reverse();

            j = 10;
            foreach (var item in linkedList)
            {
                Assert.Equal(j, item);
                j--;
            }
        }

        [Fact]
        public void Reverse_WithEmptyList()
        {
            DoubleLinkedList<int> linkedList = new DoubleLinkedList<int>();

            linkedList.Reverse();

            Assert.Equal(0, linkedList.Count);
        }

        [Fact]
        public void Reverse_WithSinlgeElementInList()
        {
            DoubleLinkedList<int> linkedList = new DoubleLinkedList<int>();

            linkedList.AddFirst(1);

            linkedList.Reverse();

            Assert.Equal(1, linkedList.Count);

            foreach (var item in linkedList)
                Assert.Equal(1, item);
        }
    }
}
