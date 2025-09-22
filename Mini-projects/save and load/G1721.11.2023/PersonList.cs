namespace G1721._11._2023
{
	public class PersonList : List<Person>
	{
		//dafikrdit kidev saidan sheidzleba dairgves Id-is unikalurobis cesi da gamoascoret
		public new void Add(Person item)
		{
			foreach (var person in this)
			{
				if (person.Id == item.Id)
				{
					throw new ArgumentException($"{item.Id} already exists!");
				}
			}
			base.Add(item);
		}

		public void Load(string filePath)
		{
			//daceret es tkven
		}

		public void Save(string filePath)
		{
			FileStream stream = new(filePath, FileMode.Create);
			BinaryWriter writer = new(stream);

			try
			{
				foreach (var person in this)
				{
					writer.Write(person.Id);
					writer.Write(person.FirstName);
					writer.Write(person.LastName);
				}
			}
			finally
			{
				writer.Close();
			}
		}
	}
}
