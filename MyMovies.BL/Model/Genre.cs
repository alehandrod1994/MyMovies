using System;

namespace MyMovies.BL.Model
{
	/// <summary>
	/// Жанр.
	/// </summary>
	
	[Serializable]
	public class Genre
	{
		/// <summary>
		/// Создать новый жанр.
		/// </summary>
		/// <param name="name"> Название. </param>
		public Genre(string name)
		{
			Name = name ?? throw new ArgumentNullException("Жанр фильма не может быть пустым.", nameof(name));			
		}

		/// <summary>
		/// Название.
		/// </summary>
		public string Name { get; }

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			return Name == ((Genre)obj).Name;
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}
	}
}
