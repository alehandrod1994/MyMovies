using MyMovies.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyMovies.BL.Controller
{
	/// <summary>
	/// Контроллер каталога фильмов.
	/// </summary>
	public class CatalogController : ControllerBase
	{
		/// <summary>
		/// Пользователь.
		/// </summary>
		private readonly User _user;

		/// <summary>
		/// Создать новый контроллер каталога.
		/// </summary>
		/// <param name="user"> Пользователь. </param>
		public CatalogController(User user)
		{
			_user = user ?? throw new ArgumentNullException("Пользователь не может быть пустым.", nameof(user));

			Movies = GetAllMovies();
			Catalogs = GetAllCatalogs();
			Genres = GetAllGenres();
		}

		/// <summary>
		/// Текущий фильм.
		/// </summary>
		public Movie CurrentMovie { get; set; }

		/// <summary>
		/// Текущий каталог.
		/// </summary>
		public Catalog CurrentCatalog { get; set; }

		/// <summary>
		/// Полный список фильмов.
		/// </summary>
		public List<Movie> Movies { get; set; }

		/// <summary>
		/// Полный список каталогов.
		/// </summary>
		public List<Catalog> Catalogs { get; set; }

		/// <summary>
		/// Полный список жанров.
		/// </summary>
		public List<Genre> Genres { get; set; }

		/// <summary>
		/// Показать сообщение.
		/// </summary>
		public event Action<string, bool> ShowMessage;

		/// <summary>
		/// Добавить новый каталог.
		/// </summary>
		public void AddCatalog(string catalogName)
		{
			var catalogFound = Catalogs.FirstOrDefault(c => c.Name == catalogName);

			string message = "";
			bool success;

			if (catalogFound == null)
			{
				var catalog = new Catalog(catalogName, _user);
				Catalogs.Add(catalog);
				Save(Catalogs);

				message = "Каталог успешно добавлен.";
				success = true;
			}
			else
			{
				message = "Такой каталог уже есть.";
				success = false;
			}

			ShowMessage?.Invoke(message, success);
		}

		/// <summary>
		/// Изменить данные каталога.
		/// </summary>
		/// <param name="index"> Номер каталога. </param>
		/// <param name="newCatalogName"> Новое название для каталога. </param>
		public void ChangeCatalog(int index, string newCatalogName)
		{
			var catalogFound = Catalogs.FirstOrDefault(c => c.Name == newCatalogName);

			string message = "";
			bool success;

			if (catalogFound == null)
			{
				Catalogs[index].Name = newCatalogName;
				Save(Catalogs);

				message = "Каталог успешно переименован.";
				success = true;
			}
			else
			{
				message = "Такой каталог уже есть.";
				success = false;
			}

			ShowMessage?.Invoke(message, success);
		}

		/// <summary>
		/// Удалить каталог.
		/// </summary>
		/// <param name="index"> Номер каталога. </param>
		public void RemoveCatalog(int index)
		{
			Catalogs.RemoveAt(index);
			Save(Catalogs);

			string message = "Каталог успешно удалён.";
			ShowMessage?.Invoke(message, true);
		}

		/// <summary>
		/// Сортировать каталоги по названию.
		/// </summary>
		public void OrderByNameCatalogs()
		{
			Catalogs = Catalogs.OrderBy(catalog => catalog.Name).ToList();
		}

		/// <summary>
		/// Сортировать каталоги по дате добавления.
		/// </summary>
		public void OrderByDateCatalogs()
		{
			Catalogs = Catalogs.OrderBy(catalog => catalog.AdditionDate).ToList();
		}

		/// <summary>
		/// Поиск каталогов.
		/// </summary>
		/// <returns> Список каталогов. </returns>
		public List<Catalog> FindCatalogs(string catalogName)
		{
			return Catalogs.FindAll(c => c.Name.Contains(catalogName));
		}

		/// <summary>
		/// Добавить фильм в текущий каталог.
		/// </summary>
		/// <param name="movie"> Фильм. </param>
		public void AddMovie(Movie movie)
		{
			var movieFound = Movies.SingleOrDefault(m => m.Name == movie.Name && m.OriginalName == movie.OriginalName && m.Year == movie.Year);

			if (movieFound == null)
			{
				Movies.Add(movie);
				Save(Movies);
			}

			CheckGenres(movie);

			movieFound = CurrentCatalog.Movies.SingleOrDefault(m => m.Name == movie.Name && m.OriginalName == movie.OriginalName && m.Year == movie.Year);

			string message = "";
			bool success;

			if (movieFound == null)
			{
				CurrentCatalog.Movies.Add(movie);
				Save(Catalogs);
				message = "Фильм успешно добавлен в каталог.";
				success = true;
			}
			else
			{
				message = "Такой фильм уже есть в каталоге.";
				success = false;
			}

			ShowMessage?.Invoke(message, success);
		}

		/// <summary>
		/// Изменить данные фильма.
		/// </summary>
		/// <param name="index"> Номер фильма. </param>
		/// <param name="movie"> Новые данные фильма. </param>
		public void ChangeMovie(int index, Movie movie)
		{
			CheckGenres(movie);

			CurrentCatalog.Movies[index] = movie;
			Save(Catalogs);

			string message = "Фильм успешно изменён.";
			ShowMessage?.Invoke(message, true);
		}

		/// <summary>
		/// Удалить фильм.
		/// </summary>
		/// <param name="index"> Номер фильма. </param>
		public void RemoveMovie(int index)
		{
			CurrentCatalog.Movies.RemoveAt(index);
			Save(Catalogs);

			string message = "Фильм успешно удалён.";
			ShowMessage?.Invoke(message, true);
		}

		/// <summary>
		/// Сортировать фильмы по названию.
		/// </summary>
		public void OrderByNameMovies()
		{
			CurrentCatalog.Movies = CurrentCatalog.Movies.OrderBy(film => film.Name).ToList();
		}

		/// <summary>
		/// Сортировать фильмы по дате добавления.
		/// </summary>
		public void OrderByDateMovies()
		{
			CurrentCatalog.Movies = CurrentCatalog.Movies.OrderBy(film => film.AdditionDate).ToList();
		}

		/// <summary>
		/// Поиск фильмов по названию.
		/// </summary>
		/// <param name="movieName"> Название фильма. </param>
		/// <returns> Список фильмов. </returns>
		public List<Movie> FindMovies(string movieName)
		{
			return CurrentCatalog.Movies.FindAll(m => m.Name.Contains(movieName));
		}

		/// <summary>
		/// Фильтровать фильмы по жанру.
		/// </summary>
		/// <param name="genres"> Список жанров. </param>
		/// <returns> Список фильмов. </returns>
		public List<Movie> FiltByGenres(List<Genre> genres)
		{
			var movies = new List<Movie>();

			for (int i = 0; i < CurrentCatalog.Movies.Count; i++)
			{
				List<Genre> intersectGenres = CurrentCatalog.Movies[i].Genres.Intersect(genres).ToList();

				if (intersectGenres != null)
				{
					if (intersectGenres.Count == genres.Count)
					{
						movies.Add(CurrentCatalog.Movies[i]);
					}
				}

			}

			return movies;
		}		

		/// <summary>
		/// Получить список всех жанров.
		/// </summary>
		/// <returns> Список жанров. </returns>
		public List<Genre> GetAllGenres()
		{
			var genres = Load<Genre>();

			return genres.Count > 0 ? genres : new List<Genre>
			{
				new Genre("боевик"),
				new Genre("военный"),
				new Genre("драма"),
				new Genre("история"),
				new Genre("комедия"),
				new Genre("мультфильм"),
				new Genre("приключения"),
				new Genre("семейный"),
				new Genre("сериал"),
				new Genre("триллер"),
				new Genre("ужасы"),
				new Genre("фантастика"),
				new Genre("фэнтези"),
				new Genre("криминал"),
				new Genre("детектив")
			};
		}

		/// <summary>
		/// Проверяет есть ли у фильма жанры, которых нет в общем списке жанров. Если есть, добавляет их.
		/// </summary>
		/// <param name="movieName"> Фильм. </param>
		private void CheckGenres(Movie movie)
		{
			foreach (var genre in movie.Genres)
			{
				var genreFound = Genres.SingleOrDefault(g => g.Name == genre.Name);

				if (genreFound == null)
				{
					Genres.Add(genre);
					Save(Genres);
				}
			}
		}

		/// <summary>
		/// Получить список всех фильмов.
		/// </summary>
		/// <returns> Список фильмов. </returns>
		public List<Movie> GetAllMovies()
		{
			return Load<Movie>() ?? new List<Movie>();
		}

		/// <summary>
		/// Получить список каталогов текущего пользователя.
		/// </summary>
		/// <returns> Список каталогов. </returns>
		public List<Catalog> GetAllCatalogs()
		{
			return Load<Catalog>().Where(c => c.User.Name == _user.Name).ToList() ?? new List<Catalog>();
		}
	}
}
