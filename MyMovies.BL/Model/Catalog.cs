using System;
using System.Collections.Generic;

namespace MyMovies.BL.Model
{
	/// <summary>
	/// Каталог.
	/// </summary>

	[Serializable]
	public class Catalog
	{
		/// <summary>
		/// Создать новый каталог.
		/// </summary>
		/// <param name="name"> Название. </param>
		/// <param name="user"> Пользователь. </param>
		public Catalog(string name, User user)
		{
			#region Проверка условий
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("Название каталога не может быть пустым.", nameof(name));
			}

			if (user is null)
			{
				throw new ArgumentNullException("Пользователь не может быть пустым.", nameof(user));
			}
			#endregion

			Name = name;
			Movies = new List<Movie>();
			User = user;
			AdditionDate = DateTime.Now;
		}

		/// <summary>
		/// Название.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Список фильмов в каталоге.
		/// </summary>
		public List<Movie> Movies { get; set; }

		/// <summary>
		/// Владелец каталога.
		/// </summary>
		public User User { get; }

		/// <summary>
		/// Дата добавления.
		/// </summary>
		public DateTime AdditionDate { get; }
		
		public override string ToString()
		{
			return Name;
		}
	}
}
