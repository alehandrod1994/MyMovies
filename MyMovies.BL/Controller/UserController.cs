using MyMovies.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyMovies.BL.Controller
{
	/// <summary>
	/// Контроллер пользователя.
	/// </summary>   
	public class UserController : ControllerBase
	{
		/// <summary>
		/// Создать новый контроллер пользователя.
		/// </summary>
		/// <param name="userName"> Имя. </param>
		/// <param name="password"> Пароль. </param>
		public UserController(string userName, string password)
		{
			#region Проверка условий
			if (string.IsNullOrWhiteSpace(userName))
			{
				throw new ArgumentNullException("Имя пользователя не может быть пустым.", nameof(userName));
			}

			if (string.IsNullOrWhiteSpace(password))
			{
				throw new ArgumentNullException("Пароль пользователя не может быть пустым.", nameof(password));
			}
			#endregion

			Users = GetUsersData();

			CurrentUser = Users.SingleOrDefault(user => user.Name == userName);

			if (CurrentUser == null)
			{
				CurrentUser = new User(userName, password);
				Users.Add(CurrentUser);
				IsNewUser = true;
				Save(Users);
			}
		}

		/// <summary>
		/// Список пользователей.
		/// </summary>
		public List<User> Users { get; }

		/// <summary>
		/// Текущий пользователь.
		/// </summary>
		public User CurrentUser { get; }

		/// <summary>
		/// Возвращает значение, указывающее новый ли пользователь.
		/// </summary>
		public bool IsNewUser { get; } = false;

		/// <summary>
		/// Получить список пользователей.
		/// </summary>
		/// <returns> Пользователи приложения. </returns>
		public List<User> GetUsersData()
		{
			return Load<User>() ?? new List<User>();          
		}
	}
}
