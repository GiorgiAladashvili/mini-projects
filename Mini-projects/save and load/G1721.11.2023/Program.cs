namespace G1721._11._2023
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Person person1 = new()
			{
				Id = 1,
				FirstName = "Khareba",
				LastName = "Kavtaradze",
				//BirthDate = new DateTime(2003, 4, 7),
				//Gender = GenderType.Male
			};
			Person person2 = new()
			{
				Id = 2,
				FirstName = "Nini",
				LastName = "Avakian",
				//BirthDate = new DateTime(1992, 11, 11),
				//Gender = GenderType.Female
			};
			PersonList personList = new();
			personList.Add(person1);
			personList.Add(person2);
			personList.Save(@"C:\G10\Textin.txt");
			foreach (var person in personList)
			{
				Console.WriteLine(person);
			}

			//PersonList personList = new();
			//personList.Load(@"C:\G10\Textin.txt");
			//foreach (var person in personList)
			//{
			//	Console.WriteLine(person);
			//}
		}
	}
}