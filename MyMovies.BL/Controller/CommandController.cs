using System;

namespace MyMovies.BL.Controller
{
	/// <summary>
	/// Контроллер команд.
	/// </summary>
	public class CommandController
	{
		/// <summary>
		/// Происходит при вызове команды добавления.
		/// </summary>
		public event Action Adding;

		/// <summary>
		/// Происходит при вызове команды открытия.
		/// </summary>
		public event Action Opening;

		/// <summary>
		/// Происходит при вызове команды изменения.
		/// </summary>
		public event Action Changing;

		/// <summary>
		/// Происходит при вызове команды удаления.
		/// </summary>
		public event Action Removing;

		/// <summary>
		/// Происходит при вызове команды сортировки по названию.
		/// </summary>
		public event Action OrderingByName;

		/// <summary>
		/// Происходит при вызове команды сортировки по дате добавления.
		/// </summary>
		public event Action OrderingByDate;

		/// <summary>
		/// Происходит при вызове команды поиска.
		/// </summary>
		public event Action Finding;

		/// <summary>
		/// Происходит при вызове команды выбора случайного элемента.
		/// </summary>
		public event Action SelectingRandom;

		/// <summary>
		/// Добавить.
		/// </summary>
		public void Add()
		{
			Adding?.Invoke();
		}

		/// <summary>
		/// Открыть.
		/// </summary>
		public void Open()
		{
			Opening?.Invoke();
		}

		/// <summary>
		/// Изменить.
		/// </summary>
		public void Change()
		{
			Changing?.Invoke();
		}

		/// <summary>
		/// Удалить.
		/// </summary>
		public void Remove()
		{
			Removing?.Invoke();
		}

		/// <summary>
		/// Сортировать по названию.
		/// </summary>
		public void OrderByName()
		{
			OrderingByName?.Invoke();
		}

		/// <summary>
		/// Сортировать по дате добавления.
		/// </summary>
		public void OrderByDate()
		{
			OrderingByDate?.Invoke();
		}

		/// <summary>
		/// Искать.
		/// </summary>
		public void Find()
		{
			Finding?.Invoke();
		}

		/// <summary>
		/// Выбрать случайный элемент.
		/// </summary>
		public void SelectRandom()
		{
			SelectingRandom?.Invoke();
		}
	}
}
