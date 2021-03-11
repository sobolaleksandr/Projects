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
            DoubleLinkedList<string> linkedList = new DoubleLinkedList<string>();

            List<string> testList = new List<string>{"Bob", "Bill"};

            linkedList.AddLast(testList[0]);
            Assert.Equal(1, linkedList.Count);
            Assert.True(linkedList.Contains(testList[0]));

            linkedList.AddLast(testList[1]);
            Assert.Equal(2, linkedList.Count);

            int i = 0;

            foreach (var item in linkedList)
            { 
                Assert.Equal(testList[i], item);
                i++; 
            }
        }

        [Fact]
        public void AddFirst()
        {
            DoubleLinkedList<string> linkedList = new DoubleLinkedList<string>();

            List<string> testList = new List<string> { "Bob", "Bill" };

            linkedList.AddFirst(testList[0]);
            Assert.Equal(1, linkedList.Count);
            Assert.True(linkedList.Contains(testList[0]));

            linkedList.AddFirst(testList[1]);
            Assert.Equal(2, linkedList.Count);

            int i = 1;

            foreach (var item in linkedList)
            {
                Assert.Equal(testList[i], item);
                i--;
            }
        }

        [Fact]
        public void Remove_WithItemInList_ReturnsTrue()
        {
            DoubleLinkedList<string> linkedList = new DoubleLinkedList<string>();

            List<string> testList = new List<string> { "Bob", "Bill" };

            linkedList.AddFirst(testList[0]);

            Assert.True(linkedList.Remove(testList[0]));
        }

        [Fact]
        public void Remove_WithEmptyList_ReturnsFalse()
        {
            DoubleLinkedList<string> linkedList = new DoubleLinkedList<string>();

            List<string> testList = new List<string> { "Bob", "Bill" };

            Assert.False(linkedList.Remove(testList[0]));
        }

        [Fact]
        public void Rverse()
        {
            DoubleLinkedList<string> linkedList = new DoubleLinkedList<string>();

            List<string> testList = new List<string> { "Bob", "Bill" };

            linkedList.AddLast(testList[0]);
            linkedList.AddLast(testList[1]);
            Assert.Equal(2, linkedList.Count);

            linkedList.Reverse();

            int i = 1;

            foreach (var item in linkedList)
            {
                Assert.Equal(testList[i], item);
                i--;
            }
        }
    }
}
