using MyMovies.BL.Controller;
using MyMovies.BL.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;

namespace MyMovies.CMD
{
	/// <summary>
	/// Интерфейс приложения.
	/// </summary>
	class Program
	{
		/// <summary>
		/// Контроллер команд.
		/// </summary>
		private static CommandController _commandController;

		/// <summary>
		/// Контроллер каталогов.
		/// </summary>
		private static CatalogController _catalogController;

		/// <summary>
		/// Контроллер таблицы.
		/// </summary>
		private static DataTableController _dataTableController;

		/// <summary>
		/// Местоположение.
		/// </summary>
		private static Location _location;

		/// <summary>
		/// Список команд.
		/// </summary>
		private static readonly Dictionary<Location, List<string>> _commands = new Dictionary<Location, List<string>>();

		/// <summary>
		/// Локализация.
		/// </summary>
		private static CultureInfo _culture = CultureInfo.CurrentCulture;
		private static ResourceManager _resourceManager = new ResourceManager("MyMovies.CMD.Languages.Messages", typeof(Program).Assembly);

		static void Main(string[] args)
		{
			int language = 0;
			while (language != 1 && language != 2)
			{
				Console.Write($"{GetLocalization("ChooseLanguage")}: \n'1' - русский, '2' - английский: ");
				if (int.TryParse(Console.ReadLine(), out language))
				{
					if (language == 1)
					{
						_culture = CultureInfo.CreateSpecificCulture("ru-ru");
					}
					else if (language == 2)
					{
						_culture = CultureInfo.CreateSpecificCulture("en-us");
					}
				}
			}
			Console.Clear();

			Console.WriteLine(GetLocalization("Greeting"));

			Console.WriteLine($"{GetLocalization("EnterUserName")}:");
			var name = Console.ReadLine();

			Console.WriteLine($"{GetLocalization("EnterUserPassword")}:");
			var password = Console.ReadLine();

			var userController = new UserController(name, password);
			_catalogController = new CatalogController(userController.CurrentUser);
			
			if (!userController.IsNewUser && password != userController.CurrentUser.Password)
			{
				while (password != userController.CurrentUser.Password)
				{
					Console.WriteLine(GetLocalization("WrongPassword"));
					password = Console.ReadLine();

				}
			}

			_commandController = new CommandController();
			_commands.Add(Location.Catalogs, new List<string>
			{
				GetLocalization("AddCatalog"),
				GetLocalization("OpenCatalog"),
				GetLocalization("ChangeCatalog"),
				GetLocalization("RemoveCatalog"),
				GetLocalization("SortingCatalogs"),
				GetLocalization("FindCatalogs"),
				GetLocalization("SelectRandomCatalog"),
				GetLocalization("Exit")
			});

			_commands.Add(Location.Movies, new List<string>
			{
				GetLocalization("AddMovie"),
				GetLocalization("OpenMovie"),
				GetLocalization("ChangeMovie"),
				GetLocalization("RemoveMovie"),
				GetLocalization("SortingMovies"),
				GetLocalization("FindMovies"),
				GetLocalization("SelectRandomMovie"),
				GetLocalization("FiltMovies"),
				GetLocalization("Exit")
			});

			_catalogController.ShowMessage += ShowMessage;

			ShowMainMenu(userController.CurrentUser.Name);

			while (true)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;				
				Console.WriteLine();
				Console.Write($"{GetLocalization("Command")}> ");
				Console.ResetColor();

				var key = Console.ReadKey();

				switch (key.Key)
				{
					case ConsoleKey.A:
						Console.WriteLine();
						_commandController.Add();
						break;

					case ConsoleKey.O:
						_commandController.Open();
						break;

					case ConsoleKey.C:
						_commandController.Change();
						break;

					case ConsoleKey.D:
						_commandController.Remove();
						break;

					case ConsoleKey.S:
						Console.WriteLine();
						Console.Write($"{GetLocalization("SortingByNameOrByDate")}: ");     

						switch (Console.ReadLine())
						{
							case "1":
								Console.Clear();
								ShowCommands();

								_commandController.OrderByName();
								break;

							case "2":
								Console.Clear();
								ShowCommands();

								_commandController.OrderByDate();
								break;

							default:
								ShowMessage(GetLocalization("WrongCommand"), false);
								break;
						}
						break;

					case ConsoleKey.P:
						Console.WriteLine();
						_commandController.Find();
						break;

					case ConsoleKey.R:
						Console.Clear();
						ShowCommands();

						_commandController.SelectRandom();
						break;

					case ConsoleKey.F:
						Console.WriteLine();

						if (_location == Location.Movies)
						{
							FiltByGenres();
						}
						else
						{
							ShowMessage(GetLocalization("UnknownCommand"), false);
						}
						break;					

					case ConsoleKey.Escape:
						_location = Location.Catalogs;
						UpdateCommands();
						GoToCurrentLocation();
						break;

					default:
						Console.WriteLine();
						ShowMessage(GetLocalization("UnknownCommand"), false);
						break;
				}
			}
		}

		/// <summary>
		/// Показать главное меню.
		/// </summary>
		/// <param name="userName"> Имя пользователя. </param>
		private static void ShowMainMenu(string userName)
		{
			_location = Location.Catalogs;
			UpdateCommands();

			ShowMessage($"{GetLocalization("Welcome")}, {userName}!", true);		
		}

		/// <summary>
		/// Показать пронумерованный список элементов.
		/// </summary>
		/// <param name="items"> Список элементов. </param>
		private static void ShowNumberedList<T>(List<T> items)
		{
			if (items.Count > 0)
			{
				for (int i = 1; i <= items.Count; i++)
				{
					Console.WriteLine($"{i}. { items[i - 1]}");
				}				
			}
			else
			{
				Console.WriteLine(GetLocalization("Empty"));
			}
		}

		/// <summary>
		/// Показать список элементов.
		/// </summary>
		/// <param name="items"> Список элементов. </param>
		private static void ShowList<T>(List<T> items)
		{
			if (items.Count > 0)
			{
				for (int i = 1; i <= items.Count; i++)
				{
					Console.WriteLine($"{ items[i - 1]}");
				}
			}
			else
			{
				Console.WriteLine(GetLocalization("Empty"));
			}
		}

		/// <summary>
		/// Показать информацию.
		/// </summary>
		/// <param name="list"> Список информации. </param>
		/// <param name="name"> Название информации. </param>
		private static void ShowInformation<T>(List<T> list, string name)
		{
			Console.Write(name);

			for (int i = 0; i < list.Count; i++)
			{
				if (i < list.Count - 1)
				{
					Console.Write($"{list[i]}, ");
				}
				else
				{
					Console.WriteLine($"{list[i]}");
				}
			}
		}

		/// <summary>
		/// Показать команды.
		/// </summary>
		private static void ShowCommands()
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"{GetLocalization("Commands")}:");

			List<string> commands = _commands[_location];
			foreach (var command in commands)
			{
				Console.WriteLine(command);

			}			

			MultiplySymbol('_', Console.WindowWidth);
			Console.WriteLine();
			Console.ResetColor();			
		}

		/// <summary>
		/// Показать сообщение.
		/// </summary>
		/// <param name="message"> Текст сообщения. </param>
		/// <param name="success"> Отметка об успешном выполнении действия, о котором оповещает данное сообщение. </param>
		private static void ShowMessage(string message, bool success)
		{
			if (success)
			{
				Console.ForegroundColor = ConsoleColor.Green;
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
			}

			Console.WriteLine(message);
			Console.ResetColor();

			Thread.Sleep(1000);

			GoToCurrentLocation();
		}

		/// <summary>
		/// Размножить символ.
		/// </summary>
		/// <param name="symbol"> Символ. </param>
		/// <param name="length"> Длина. </param>
		private static void MultiplySymbol(char symbol, int length)
		{
			for (int i = 0; i < length; i++)
			{
				Console.Write(symbol);
			}
		}

		/// <summary>
		/// Начертить таблицу фильмов.
		/// </summary>
		private static void DrawMoviesTable()
		{
			if (_catalogController.CurrentCatalog.Movies.Count > 0)
			{
				string[] columnsNames = new string[]
				{
					GetLocalization("MovieNumber"),
					GetLocalization("MovieName"),
					GetLocalization("MovieOriginalName"),
					GetLocalization("MovieYear"),
					GetLocalization("MovieDirector"),
					GetLocalization("MovieRating"),
					GetLocalization("MovieGenres"),
					GetLocalization("MovieWatched"),
					GetLocalization("MovieDate"),
				};

				_dataTableController = new DataTableController(columnsNames);
				_dataTableController.SetMoviesData(_catalogController.CurrentCatalog.Movies);

				int[] columnsLength = _dataTableController.GetMaxLengthMoviesProperties(_catalogController.CurrentCatalog.Movies);
				int tableWidth = Console.WindowWidth - 1;

				DrawColumns(_dataTableController.DataTable.ColumnsNames, columnsLength, tableWidth);

				for (int i = 0; i < _catalogController.CurrentCatalog.Movies.Count; i++)
				{
					DrawCells(_dataTableController.DataTable.Rows[i].Cells, columnsLength);
				}
			}
			else
			{
				Console.WriteLine(GetLocalization("Empty"));
			}			
		}

		/// <summary>
		/// Начертить названия столбцов в таблице.
		/// </summary>
		/// <param name="columns"> Названия столбцов. </param>
		/// <param name="columnsLength"> Длина столбцов. </param>
		/// <param name="tableWidth"> Ширина таблицы. </param>
		private static void DrawColumns(string[] columns, int[] columnsLength, int tableWidth)
		{
			MultiplySymbol('-', tableWidth);
			Console.WriteLine();

			DrawCells(columns, columnsLength);
			
			MultiplySymbol('-', tableWidth);
			Console.WriteLine();
		}

		/// <summary>
		/// Начертить ячейки в таблице.
		/// </summary>
		/// <param name="cells"> Ячейки. </param>
		/// <param name="columnsLength"> Длина столбцов. </param>
		private static void DrawCells(string[] cells, int[] columnsLength)
		{
			int countSpace;

			for (int i = 0; i < _dataTableController.DataTable.ColumnCount; i++)
			{
				Console.Write($"|{cells[i]}");

				if (cells[i].Length < columnsLength[i])
				{
					countSpace = columnsLength[i] - cells[i].Length;
					MultiplySymbol(' ', countSpace);
				}
			}

			Console.WriteLine("|");
		}

		/// <summary>
		/// Добавить каталог.
		/// </summary>
		private static void AddCatalog()
		{			
			Console.Write($"{GetLocalization("EnterCatalogName")}: ");
			string newCatalogName = Console.ReadLine();
			_catalogController.AddCatalog(newCatalogName);
		}

		/// <summary>
		/// Открыть каталог.
		/// </summary>
		private static void OpenCatalog()
		{
			int index = ParseIntCommand($"{GetLocalization("EnterCatalogNumberToOpen")}: ", _catalogController.Catalogs.Count);
			if (index == 0) return;

			_catalogController.CurrentCatalog = _catalogController.Catalogs[index - 1];

			_location = Location.Movies;
			UpdateCommands();
			GoToCurrentLocation();				
		}

		/// <summary>
		/// Изменить каталог.
		/// </summary>
		private static void ChangeCatalog()
		{
			int index = ParseIntCommand($"{GetLocalization("EnterCatalogNumberToChange")}: ", _catalogController.Catalogs.Count);
			if (index == 0) return;

			Console.Write($"Введите новое имя для каталога {_catalogController.Catalogs[index - 1].Name}: ");
			string newCatalogName = Console.ReadLine();
			_catalogController.ChangeCatalog(index - 1, newCatalogName);
		}

		/// <summary>
		/// Удалить каталог.
		/// </summary>
		private static void RemoveCatalog()
		{
			int index = ParseIntCommand($"{GetLocalization("EnterCatalogNumberToRemove")}: ", _catalogController.Catalogs.Count);
			if (index == 0) return;

			_catalogController.RemoveCatalog(index - 1);
		}

		/// <summary>
		/// Сортировать каталоги по названию.
		/// </summary>
		private static void OrderByNameCatalogs()
		{
			_catalogController.OrderByNameCatalogs();

			ShowNumberedList(_catalogController.Catalogs);
		}

		/// <summary>
		/// Сортировать каталоги по дате добавления.
		/// </summary>
		private static void OrderByDateCatalogs()
		{
			_catalogController.OrderByDateCatalogs();

			ShowNumberedList(_catalogController.Catalogs);
		}

		/// <summary>
		/// Поиск каталогов.
		/// </summary>
		private static void FindCatalogs()
		{
			Console.Write($"{GetLocalization("EnterCatalogNameToFind")}: ");
			var catalogs = _catalogController.FindCatalogs(Console.ReadLine());

			Console.Clear();
			ShowCommands();
			ShowNumberedList(catalogs);
		}

		/// <summary>
		/// Выбрать случайный каталог.
		/// </summary>
		private static void SelectRandomCatalog()
		{
			int index = _catalogController.SelectRandomCatalog();
			Console.WriteLine($"{index + 1}. {_catalogController.Catalogs[index]}");
		}


		/// <summary>
		/// Добавить фильм.
		/// </summary>
		private static void AddMovie()
		{
			Console.Write($"{GetLocalization("EnterMovieName")}: ");
			string name = Console.ReadLine();

			Console.Write($"{GetLocalization("EnterMovieOriginalName")}: ");
			string originalName = Console.ReadLine();

			Console.Write($"{GetLocalization("EnterMovieYear")}: ");
			int year = ParseInt(GetLocalization("Year"));

			Console.Write($"{GetLocalization("EnterMovieDirector")}: ");
			string director = Console.ReadLine();

			Console.Write($"{GetLocalization("EnterMovieRating")}: ");
			double rating = ParseDouble(GetLocalization("Rating"));

			ShowNumberedList(_catalogController.Genres);
			Console.Write($"{GetLocalization("EnterMovieGenres")}: ");
			var genres = new List<Genre>();
			genres.AddRange(ParseGenres());

			Console.Write($"{GetLocalization("EnterMovieWatched")}: ");
			string watched = Console.ReadLine();

			var movie = new Movie(name, originalName, year, director, rating, genres, watched);
			_catalogController.AddMovie(movie);
		}

		/// <summary>
		/// Открыть фильм.
		/// </summary>
		private static void OpenMovie()
		{
			int index = ParseIntCommand($"{GetLocalization("EnterMovieNumberToOpen")}: ", _catalogController.CurrentCatalog.Movies.Count);
			if (index == 0) return;

			_catalogController.CurrentMovie = _catalogController.CurrentCatalog.Movies[index - 1];

			Console.WriteLine($"{GetLocalization("MovieName")}: {_catalogController.CurrentMovie.Name}");
			Console.WriteLine($"{GetLocalization("MovieOriginalName")}: {_catalogController.CurrentMovie.OriginalName}");
			Console.WriteLine($"{GetLocalization("MovieYear")}: {_catalogController.CurrentMovie.Year}");
			Console.WriteLine($"{GetLocalization("MovieDirector")}: {_catalogController.CurrentMovie.Director}");
			Console.WriteLine($"{GetLocalization("MovieRating")}: {_catalogController.CurrentMovie.Rating}");
			ShowInformation(_catalogController.CurrentMovie.Genres, $"{GetLocalization("MovieGenres")}: ");
			Console.WriteLine($"{GetLocalization("MovieWatched")}: {_catalogController.CurrentMovie.Watched}");

			Console.WriteLine($"\n{GetLocalization("PressAnyKey")}");
			Console.ReadKey();
			GoToCurrentLocation();
		}

		/// <summary>
		/// Изменить фильм.
		/// </summary>
		private static void ChangeMovie()
		{
			int index = ParseIntCommand($"{GetLocalization("EnterMovieNumberToChange")}: ", _catalogController.CurrentCatalog.Movies.Count);
			if (index == 0) return;

			List<Genre> gg = _catalogController.CurrentCatalog.Movies[0].Genres;

			_catalogController.CurrentMovie = _catalogController.CurrentCatalog.Movies[index - 1];

			Console.WriteLine($"1. {GetLocalization("MovieName")}: {_catalogController.CurrentMovie.Name}");
			Console.WriteLine($"2. {GetLocalization("MovieOriginalName")}: {_catalogController.CurrentMovie.OriginalName}");
			Console.WriteLine($"3. {GetLocalization("MovieYear")}: {_catalogController.CurrentMovie.Year}");
			Console.WriteLine($"4. {GetLocalization("MovieDirector")}: {_catalogController.CurrentMovie.Director}");
			Console.WriteLine($"5. {GetLocalization("MovieRating")}: {_catalogController.CurrentMovie.Rating}");
			ShowInformation(_catalogController.CurrentMovie.Genres, $"6. {GetLocalization("MovieGenres")}: ");
			Console.WriteLine($"7. {GetLocalization("MovieWatched")}: {_catalogController.CurrentMovie.Watched}");

			string name = _catalogController.CurrentMovie.Name;
			string originalName = _catalogController.CurrentMovie.OriginalName;
			int year = _catalogController.CurrentMovie.Year;
			string director = _catalogController.CurrentMovie.Director;
			double rating = _catalogController.CurrentMovie.Rating;
			List<Genre> genres = _catalogController.CurrentMovie.Genres;
			string watched = _catalogController.CurrentMovie.Watched;
			DateTime additionDate = _catalogController.CurrentMovie.AdditionDate;

			while (true)
			{
				Console.Write($"{GetLocalization("EnterColumnNumber")}: ");
				int fieldNumber = ParseInt(GetLocalization("Movie"));

				switch (fieldNumber)
				{
					case 1:
						Console.Write($"{GetLocalization("EnterMovieName")}: ");
						name = Console.ReadLine();
						break;

					case 2:
						Console.Write($"{GetLocalization("EnterMovieOriginalName")}: ");
						originalName = Console.ReadLine();
						break;

					case 3:
						Console.Write($"{GetLocalization("EnterMovieYear")}: ");
						year = ParseInt(GetLocalization("Year"));
						break;

					case 4:
						Console.Write($"{GetLocalization("EnterMovieDirector")}: ");
						director = Console.ReadLine();
						break;

					case 5:
						Console.Write($"{GetLocalization("EnterMovieRating")}: ");
						rating = ParseDouble(GetLocalization("Rating"));
						break;

					case 6:
						ShowList(_catalogController.Genres);
						Console.Write($"{GetLocalization("EnterMovieGenres")}: ");						
						genres.Clear();
						genres.AddRange(ParseGenres());
						break;

					case 7:
						Console.Write($"{GetLocalization("EnterMovieWatched")}: ");
						watched = Console.ReadLine();
						break;

					default:						
						ShowMessage(GetLocalization("ColumnNotFound"), false);
						return;
				}

				while (true)
				{
					Console.Write($"{GetLocalization("SaveChanges")}: ");
					string result = Console.ReadLine();

					if (result.Equals(GetLocalization("Yes"), StringComparison.CurrentCultureIgnoreCase))
					{
						var movie = new Movie(name, originalName, year, director, rating, genres, watched, additionDate);
						_catalogController.ChangeMovie(index - 1, movie);

						return;
					}
					else if (result.Equals(GetLocalization("No"), StringComparison.CurrentCultureIgnoreCase)) break;
				}
			}			
		}

		/// <summary>
		/// Удалить фильм.
		/// </summary>
		private static void RemoveMovie()
		{
			int index = ParseIntCommand($"{GetLocalization("EnterMovieNumberToRemove")}: ", _catalogController.CurrentCatalog.Movies.Count);
			if (index == 0) return;

			_catalogController.RemoveMovie(index - 1);
		}

		/// <summary>
		/// Сортировать фильмы по названию.
		/// </summary>
		private static void OrderByNameMovies()
		{
			_catalogController.OrderByNameMovies();

			ShowNumberedList(_catalogController.CurrentCatalog.Movies);
		}

		/// <summary>
		/// Сортировать фильмы по дате добавления.
		/// </summary>
		private static void OrderByDateMovies()
		{
			_catalogController.OrderByDateMovies();

			ShowNumberedList(_catalogController.CurrentCatalog.Movies);
		}

		/// <summary>
		/// Поиск фильмов по названию.
		/// </summary>
		private static void FindMovies()
		{
			Console.Write($"{GetLocalization("EnterMovieNameToFind")}: ");
			var movies = _catalogController.FindMovies(Console.ReadLine());

			Console.Clear();
			ShowCommands();
			ShowNumberedList(movies);
		}

		/// <summary>
		/// Выбрать случайный фильм.
		/// </summary>
		private static void SelectRandomMovie()
		{
			int index = _catalogController.SelectRandomMovie();
			Console.WriteLine($"{index + 1}. {_catalogController.CurrentCatalog.Movies[index]}");
		}

		/// <summary>
		/// Фильтровать фильмы по жанру.
		/// </summary>
		public static void FiltByGenres()
		{
			ShowList(_catalogController.Genres);
			Console.Write($"{GetLocalization("EnterMovieGenres")}: ");			
			var genres = new List<Genre>();
			genres.AddRange(ParseGenres());

			var movies = _catalogController.FiltByGenres(genres);

			Console.Clear();
			ShowCommands();

			ShowNumberedList(movies);
		}

		/// <summary>
		/// Перевести строку в целое число.
		/// </summary>
		/// <param name="name"> Название элемента. </param>
		/// <returns> Результат перевода. </returns>
		private static int ParseInt(string name)
		{
			if (int.TryParse(Console.ReadLine(), out int result))
			{
				return result;
			}
			else
			{
				Console.WriteLine($"{name} {GetLocalization("NotFound")}");
				GoToCurrentLocation();
				return 0;
			}
		}

		/// <summary>
		/// Перевести строку в вещественное число.
		/// </summary>
		/// <param name="name"> Название элемента. </param>
		/// <returns> Результат перевода. </returns>
		private static double ParseDouble(string name)
		{
			if (double.TryParse(Console.ReadLine(), out double result))
			{
				return result;
			}
			else
			{
				Console.WriteLine($"{GetLocalization("WrongFormat")} {name}.");
				GoToCurrentLocation();
				return 0;
			}
		}

		/// <summary>
		/// Перевести жанры в корректный список.
		/// </summary>
		/// <returns> Результат перевода. </returns>
		private static IEnumerable<Genre> ParseGenres()
		{
			string genre = Console.ReadLine();

			for (int i = 0; i < genre.Length; i++)
			{
				if (!(genre[i] >= 'а' && genre[i] <= 'я' || genre[i] == ','))
				{
					genre = genre.Remove(i, 1);

					i--;
				}
			}

			string[] genres = genre.ToLower().Split(',');

			foreach(var g in genres)
			{
				yield return new Genre(g);
			}
		}

		/// <summary>
		/// Перевести строку в целое число для использования команды.
		/// </summary>
		/// <param name="message"> Сообщение для конкретной команды. </param>
		/// <param name="itemsCount"> Количество элементов. </param>
		/// <returns> Результат перевода. </returns>
		private static int ParseIntCommand(string message, int itemsCount)
		{
			Console.Write($"\n{message}");
			if (int.TryParse(Console.ReadLine(), out int result) && result > 0 && result <= itemsCount)
			{
				return result;
			}
			else
			{
				ShowMessage(GetLocalization("NotFound"), false);
				return 0;
			}
		}

		/// <summary>
		/// Получить локализированный текст в соответствии с выбранным языком.
		/// </summary>
		/// <param name="sourceText"> Исходный текст. </param>
		/// <returns> Локализированный текст. </returns>
		private static string GetLocalization(string sourceText)
		{
			return _resourceManager.GetString(sourceText, _culture);

		}

		/// <summary>
		/// Перейти в текущее местоположение.
		/// </summary>
		private static void GoToCurrentLocation()
		{
			Console.Clear();

			ShowCommands();		

			switch (_location)
			{
				case Location.Catalogs:
					ShowNumberedList(_catalogController.Catalogs);
					break;

				case Location.Movies:
					Console.WriteLine($"{_catalogController.CurrentCatalog.Name}:");
					DrawMoviesTable();
					break;
			}
		}

		/// <summary>
		/// Обновить команды.
		/// </summary>
		private static void UpdateCommands()
		{
			_commandController = new CommandController();

			if (_location == Location.Catalogs)
			{
				_commandController.Adding += AddCatalog;
				_commandController.Opening += OpenCatalog;
				_commandController.Changing += ChangeCatalog;
				_commandController.Removing += RemoveCatalog;
				_commandController.OrderingByName += OrderByNameCatalogs;
				_commandController.OrderingByDate += OrderByDateCatalogs;
				_commandController.Finding += FindCatalogs;
				_commandController.SelectingRandom += SelectRandomCatalog;
			}
			else
			{
				_commandController.Adding += AddMovie;
				_commandController.Opening += OpenMovie;
				_commandController.Changing += ChangeMovie;
				_commandController.Removing += RemoveMovie;
				_commandController.OrderingByName += OrderByNameMovies;
				_commandController.OrderingByDate += OrderByDateMovies;
				_commandController.Finding += FindMovies;
				_commandController.SelectingRandom += SelectRandomMovie;
			}
		}
	}
}
