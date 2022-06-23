using System;

namespace MyMovies.BL.Controller
{
	public class CommandController
	{
		public event Action Adding;
		public event Action Opening;
		public event Action Updating;
		public event Action Removing;
		public event Action OrderingByName;
		public event Action OrderingByDate;
		public event Action Finding;

		public void Add()
		{
			Adding?.Invoke();
		}

		public void Open()
		{
			Opening?.Invoke();
		}

		public void Update()
		{
			Updating?.Invoke();
		}

		public void Remove()
		{
			Removing?.Invoke();
		}

		public void OrderByName()
		{
			OrderingByName?.Invoke();
		}

		public void OrderByDate()
		{
			OrderingByDate?.Invoke();
		}

		public void Find()
		{
			Finding?.Invoke();
		}
	}
}
