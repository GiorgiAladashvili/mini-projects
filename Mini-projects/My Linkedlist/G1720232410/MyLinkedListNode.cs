namespace G1720232410;

public class MyLinkedListNode<T>
{
	public MyLinkedListNode()
	{

	}

	public MyLinkedListNode(T? value) : this()
	{
		Value = value;
	}

	public MyLinkedListNode(T? value, MyLinkedListNode<T>? next) : this(value)
	{
		Next = next;
	}

	public T? Value { get; set; }
	public MyLinkedListNode<T>? Next { get; set; }
	public MyLinkedListNode<T>? Previous {  get; set; }

	public override string? ToString() => Value?.ToString();
}