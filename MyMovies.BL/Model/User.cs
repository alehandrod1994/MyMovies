using System;

namespace MyMovies.BL.Model
{
	/// <summary>
	/// Пользователь.
	/// </summary>

	[Serializable]
	public class User
	{
		/// <summary>
		/// Создать нового пользователя.
		/// </summary>
		/// <param name="name"> Имя. </param>
		/// <param name="password"> Пароль. </param>
		public User(string name, string password)
		{
			#region Проверка условий
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("Имя пользователя не может быть пустым.", nameof(name));
			}

			if (string.IsNullOrWhiteSpace(password))
			{
				throw new ArgumentNullException("Пароль пользователя не может быть пустым.", nameof(password));
			}
			#endregion

			Name = name;
			Password = password;
		}

		/// <summary>
		/// Имя.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Пароль.
		/// </summary>
		public string Password { get; }

		public override string ToString()
		{
			return Name;
		}
	}
}
