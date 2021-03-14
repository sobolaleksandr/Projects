# Задание
1. Написать функцию, которая преобразует строку с римским числом в целое (иными словами, написать тело функции public int RomanToInt(string s)). Римское число не больше 3000.
2. Проверить сбалансированность скобочной структуры в произвольном выражении ((1+3)()(4+(3-5)))
3. Реализовать двусвязный список и написать функцию, переворачивающую его, т.е. изменяющую порядок элементов на обратный.
public interface DoubleLinkedListNode<T>
{
T Value { get; set; }
DoubleLinkedNode<T> Next { get; set; }
DoubleLinkedNode<T> Prev { get; set; }
}
public interface DoubleLinkedList<T>
{
DoubleLinkedNode<T> First { get; set; }
DoubleLinkedNode<T> Last { get; set; }
void Reverse();
//insert new DoubleLinkedListNode with given value at the start of the list
void AddFirst(T value);
//insert new DoubleLinkedListNode with given value at the end of the list
void AddLast(T value);
}

Все решения должны быть сопровождены юнит-тестами
