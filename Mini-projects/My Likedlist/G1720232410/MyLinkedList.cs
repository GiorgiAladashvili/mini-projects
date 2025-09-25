using System.Collections;

namespace G1720232410;

public class MyLinkedList<T> : ICollection<T>
{
	private MyLinkedListNode<T>? _first;
	private MyLinkedListNode<T>? _last;

	public void AddFirst(MyLinkedListNode<T> node)
	{
		if (_first != null)
		{
			node.Next = _first;
			_first.Previous = node;
		}
		else
		{
			_last = node;
		}
		_first = node;
	}

	public MyLinkedListNode<T> AddFirst(T value)
	{
		MyLinkedListNode<T> node = new(value);
		AddFirst(node);
		return node;
	}

	public void AddLast(MyLinkedListNode<T> node)
	{
		if (_last == null)
		{
			AddFirst(node);
		}
		else
		{
			_last.Next = node;
			node.Previous = _last;
			_last = node;
		}
	}

	public MyLinkedListNode<T> AddLast(T value)
	{
		MyLinkedListNode<T> node = new(value);
		AddLast(node);
		return node;
	}

	public void Remove(MyLinkedListNode<T> node)
	{
		Remove(node.Value);
	}

	public bool Remove(T? value)
	{
		if (_first == null)
		{
			return false;
		}

		if (_first.Value.Equals(value))
		{
			RemoveFirst();
			return true;
		}

		MyLinkedListNode<T> tmp = Find(value);
		if (tmp == null)
		{
			return false;
		}

		tmp.Previous.Next = tmp.Next;
		tmp = tmp.Next;
		tmp.Previous = tmp.Previous.Previous;

		return true;
	}

	public void RemoveFirst()
	{
		if (_first != null && _first.Next == null)
		{
			_first = null;
			_last = null;
		}
		else if (_first != null && _first.Next != null)
		{
			_first = _first.Next;
			_first.Previous = null;
		}
	}

	public void RemoveLast()
	{
		if (_first != null && _first.Next == null)
		{
			_last = null;
			_first = null;
		}
		else if (_first != null)
		{
			_last = _last.Previous;
			_last.Next = null;
		}
	}

	public void Clear()
	{
		while (_first != null)
		{
			MyLinkedListNode<T>? temp = _first.Next;
			_first = null;
			_first = temp;
		}
	}

	public bool Contains(T? value)
	{
		return Find(value) != null;
	}

	public MyLinkedListNode<T>? Find(T? value)
	{
		MyLinkedListNode<T>? tmp = _first;
		while (tmp != null)
		{
			if (tmp.Value.Equals(value))
			{
				return tmp;
			}
			tmp = tmp.Next;
		}
		return tmp;
	}

	public MyLinkedListNode<T>? FindLast(T? value)
	{

		MyLinkedListNode<T>? tmp = _last;
		while (tmp != null)
		{
			if (tmp.Value.Equals(value))
			{
				return tmp;
			}
			tmp = tmp.Previous;
		}
		return null;
	}

	public void AddAfter(MyLinkedListNode<T> node, MyLinkedListNode<T> newNode)
	{
		MyLinkedListNode<T>? tmp = node.Next;
		node.Next = newNode;
		newNode.Previous = node;
		if (tmp != null)
		{
			newNode.Next = tmp;
			tmp.Previous = newNode;
			return;
		}
		_last = newNode;
	}

	public MyLinkedListNode<T> AddAfter(MyLinkedListNode<T> node, T value)
	{
		MyLinkedListNode<T> newNode = new(value);
		AddAfter(node, newNode);

		return newNode;
	}

	public void AddBefore(MyLinkedListNode<T> node, MyLinkedListNode<T> newNode)
	{
		MyLinkedListNode<T>? tmp = node.Previous;
		node.Previous = newNode;
		newNode.Next = node;
		if (tmp != null)
		{
			newNode.Previous = tmp;
			tmp.Next = newNode;
			return;
		}
		_first = newNode;
	}

	public MyLinkedListNode<T> AddBefore(MyLinkedListNode<T> node, T value)
	{
		MyLinkedListNode<T> newNode = new MyLinkedListNode<T>(value);
		AddBefore(node, newNode);

		return newNode;
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		MyLinkedListNode<T>? tmp = _first;
		while (tmp != null && tmp.Value != null)
		{
			array[arrayIndex++] = tmp.Value;
			tmp = tmp.Next;
		}
	}

	public int Count { get; }
	public bool IsReadOnly { get; }

	public IEnumerator<T> GetEnumerator()
	{
		return new ArrayEnumerator<T>(_first);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	void ICollection<T>.Add(T item)
	{
		throw new NotImplementedException();
	}
}

public class ArrayEnumerator<T> : IEnumerator<T>
{
	private readonly MyLinkedListNode<T> _first;
	private MyLinkedListNode<T>? _current;

	public ArrayEnumerator(MyLinkedListNode<T>? node)
	{
		_first = node ?? throw new ArgumentNullException(nameof(node));
	}

	public T Current => _current.Value;

	public bool MoveNext()
	{
		if (_current == null)
		{
			_current = _first;
			return true;
		}
		if (_current.Next != null)
		{
			_current = _current.Next;
			return true;
		}
		return false;
	}

	public void Reset()
	{
		_current = null;
	}

	public void Dispose()
	{

	}

	object IEnumerator.Current => Current;
}