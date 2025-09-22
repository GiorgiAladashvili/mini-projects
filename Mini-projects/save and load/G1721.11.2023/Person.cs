namespace G1721._11._2023
{
	public class Person
	{
		public int Id { get; set; }
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		//public DateTime BirthDate { get; set; }
		//public GenderType Gender { get; set; }
		//public ICollection<Person>? Children { get; set; }

		public override string ToString()
		{
			return $"{Id}, {FirstName}, {LastName}";
		}
	}

	public enum GenderType : byte
	{
		Male = 0,
		Female = 1
	}
}
