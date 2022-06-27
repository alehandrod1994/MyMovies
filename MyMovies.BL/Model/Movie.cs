using System;
using System.Collections.Generic;

namespace MyMovies.BL.Model
{
	/// <summary>
	/// Фильм.
	/// </summary>

	[Serializable]
	public class Movie
	{
		public Movie(string name, string originalName, int year, string director, double rating, List<Genre> genres, string watched) : 
			this(name, originalName, year, director, rating, genres, watched, DateTime.Now) { }

		/// <summary>
		/// Создать новый фильм.
		/// </summary>
		/// <param name="name"> Название. </param>
		/// <param name="originalName"> Оригинальное название. </param>
		/// <param name="year"> Год. </param>
		/// <param name="director"> Режиссёр. </param>
		/// <param name="rating"> Рейтинг. </param>
		/// <param name="genres"> Жанры. </param>
		/// <param name="watched"> Отметка о просмотре. </param>
		/// <param name="dateTime"> Дата добавления. </param>
		public Movie(string name, string originalName, int year, string director, double rating, List<Genre> genres, string watched, DateTime dateTime)
		{
			#region Проверка условий
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("Название фильма не может быть пустым.", nameof(name));
			}

			if (string.IsNullOrWhiteSpace(originalName))
			{
				throw new ArgumentNullException("Оригинальное название фильма не может быть пустым.", nameof(originalName));
			}

			if (year <= 1800)
			{
				throw new ArgumentException("Год выхода фильма не может быть меньше или равен нулю.", nameof(year));
			}

			if (string.IsNullOrWhiteSpace(director))
			{
				throw new ArgumentNullException("Имя режиссёра не может быть пустым.", nameof(director));
			}

			if (rating < 0)
			{
				throw new ArgumentException("Рейтинг фильма не может быть меньше нуля.", nameof(rating));
			}

			if (genres is null)
			{
				throw new ArgumentNullException("Жанр фильма не может быть пустым.", nameof(genres));
			}

			if (string.IsNullOrWhiteSpace(watched))
			{
				throw new ArgumentNullException("Отметка о просмотре фильма не может быть пустой.", nameof(watched));
			}
			#endregion

			Name = name;
			OriginalName = originalName;
			Year = year;
			Director = director;
			Rating = rating;
			Genres = genres;
			Watched = watched;
			AdditionDate = dateTime;
		}

		/// <summary>
		/// Название.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Оригинальное название.
		/// </summary>
		public string OriginalName { get; }

		/// <summary>
		/// Год.
		/// </summary>
		public int Year { get; }

		/// <summary>
		/// Режиссёр.
		/// </summary>
		public string Director { get; }

		/// <summary>
		/// Рейтинг.
		/// </summary>
		public double Rating { get; }

		/// <summary>
		/// Жанры.
		/// </summary>
		public List<Genre> Genres { get; }

		/// <summary>
		/// Отметка о просмотре.
		/// </summary>
		public string Watched { get; }

		/// <summary>
		/// Дата добавления.
		/// </summary>
		public DateTime AdditionDate { get; }

		public override string ToString()
		{
			return $"{Name} ({Year})";
		}
	}
}
