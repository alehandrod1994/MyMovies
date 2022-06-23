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
		private readonly User _user;

		public CatalogController(User user)
		{
			_user = user ?? throw new ArgumentNullException("Пользователь не может быть пустым", nameof(user));

			Movies = GetAllMovies();
			Catalogs = GetAllCatalogs();
			Genres = GetAllGenres();
		}

		public Movie CurrentMovie { get; set; }
		public Catalog CurrentCatalog { get; set; }

		public List<Movie> Movies { get; set; }
		public List<Catalog> Catalogs { get; set; }
		public List<Genre> Genres { get; set; }

		public event Action<string, bool> ShowMessage;

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

		public void ChangeCatalog(int index, string newCatalogName)
		{
			Catalogs[index].Name = newCatalogName;
			Save(Catalogs);

			string message = "Каталог успешно переименован.";
			ShowMessage?.Invoke(message, true);
		}

		public void RemoveCatalog(int index)
		{
			Catalogs.RemoveAt(index);
			Save(Catalogs);

			string message = "Каталог успешно удалён.";
			ShowMessage?.Invoke(message, true);
		}

		public void OrderByNameCatalogs()
		{
			Catalogs = Catalogs.OrderBy(catalog => catalog.Name).ToList();
		}

		public void OrderByDateCatalogs()
		{
			Catalogs = Catalogs.OrderBy(catalog => catalog.AdditionDate).ToList();
		}

		public List<Catalog> FindCatalogs(string catalogName)
		{
			return Catalogs.FindAll(c => c.Name.Contains(catalogName));
		}

		public void AddMovie(Movie movie)
		{
			var movieFound = Movies.SingleOrDefault(m => m.Name == movie.Name && m.OriginalName == movie.OriginalName && m.Year == movie.Year);

			if (movieFound == null)
			{
				Movies.Add(movie);
				Save(Movies);
			}

			foreach(var genre in movie.Genres)
			{
				var genreFound = Genres.SingleOrDefault(g => g.Name == genre.Name);

				if (genreFound == null)
				{
					Genres.Add(genre);
					Save(Genres);
				}
			}

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

		public void ChangeMovie(int index, Movie movie)
		{
			CurrentCatalog.Movies[index] = movie;
			Save(Catalogs);

			string message = "Фильм успешно изменён.";
			ShowMessage?.Invoke(message, true);
		}

		public void RemoveMovie(int index)
		{
			CurrentCatalog.Movies.RemoveAt(index);
			Save(Catalogs);

			string message = "Фильм успешно удалён.";
			ShowMessage?.Invoke(message, true);
		}

		public void OrderByNameMovies()
		{
			CurrentCatalog.Movies = CurrentCatalog.Movies.OrderBy(film => film.Name).ToList();
		}

		public void OrderByDateMovies()
		{
			CurrentCatalog.Movies = CurrentCatalog.Movies.OrderBy(film => film.AdditionDate).ToList();
		}

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

		public List<Movie> FindMovies(string movieName)
		{
			return CurrentCatalog.Movies.FindAll(m => m.Name.Contains(movieName));
		}

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
				new Genre("криминал")
			};
		}

		public List<Movie> GetAllMovies()
		{
			return Load<Movie>() ?? new List<Movie>();
		}

		public List<Catalog> GetAllCatalogs()
		{
			return Load<Catalog>().Where(c => c.User.Name == _user.Name).ToList() ?? new List<Catalog>();
		}
	}
}
