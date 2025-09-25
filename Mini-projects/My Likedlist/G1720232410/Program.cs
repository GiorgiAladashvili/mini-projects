namespace G1720232410
{
	internal class Program
	{
		static void Main(string[] args)
		{
			MyLinkedList<string> linkedList = new();

			linkedList.AddFirst("5");
			linkedList.AddFirst("4");
			linkedList.AddFirst(new MyLinkedListNode<string>("3"));
			linkedList.AddFirst("1");
			linkedList.RemoveLast();
			//linkedList.Clear();
			

			foreach (var x in linkedList)
			{
				Console.WriteLine(x);
			}
		}

		static void Print(MyLinkedListNode<string>? node)
		{
			do
			{
				Console.WriteLine(node);
				node = node?.Next;
			} 
			while (node != null);
		}
	}
}