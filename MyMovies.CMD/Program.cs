using MyMovies.BL.Controller;
using MyMovies.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MyMovies.CMD
{
	class Program
	{
		private static CommandController _commandController;
		private static CatalogController _catalogController;
		private static DataTableController _dataTableController;

		private static Location _location;

		private static readonly Dictionary<Location, List<string>> _commands = new Dictionary<Location, List<string>>();

		static void Main(string[] args)
		{
			Console.WriteLine("Вас приветствует приложение MyMovies");

			Console.WriteLine("Введите имя пользователя");
			var name = Console.ReadLine();

			Console.WriteLine("Введите пароль пользователя");
			var password = Console.ReadLine();

			var userController = new UserController(name, password);
			_catalogController = new CatalogController(userController.CurrentUser);
			_commandController = new CommandController();

			_commands.Add(Location.Catalogs, new List<string>
			{
				"A - СОЗДАТЬ каталог",
				"O - ОТКРЫТЬ каталог",
				"C - ИЗМЕНИТЬ каталог",
				"D - УДАЛИТЬ каталог",
				"S - СОРТИРОВАТЬ каталоги",
				"P - ИСКАТЬ каталог",
				"ESC - ВЫХОД в главное меню"
			});

			_commands.Add(Location.Movies, new List<string>
			{
				"A - СОЗДАТЬ фильм",
				"O - ОТКРЫТЬ фильм",
				"C - ИЗМЕНИТЬ фильм",
				"D - УДАЛИТЬ фильм",
				"S - СОРТИРОВАТЬ фильмы",
				"P - ИСКАТЬ фильм",
				"F - ФИЛЬТРОВАТЬ фильмы по жанру",
				"ESC - ВЫХОД в главное меню"
			});

			_catalogController.ShowMessage += ShowMessage;

			if (!userController.IsNewUser && password != userController.CurrentUser.Password)
			{
				while (password != userController.CurrentUser.Password)
				{
					Console.WriteLine("Неверный пароль. Введите ещё раз.");
					password = Console.ReadLine();

				}
			}

			ShowMainMenu(userController);

			while (true)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;				
				Console.WriteLine();
				Console.Write("Команда> ");
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
						_commandController.Update();
						break;

					case ConsoleKey.D:
						_commandController.Remove();
						break;

					case ConsoleKey.S:
						Console.WriteLine();
						Console.Write("Введите '1', чтобы сортировать по имени, '2' - сортировать по дате: ");     

						switch (Console.ReadLine())
						{
							case "1":
								_commandController.OrderByName();
								break;

							case "2":
								_commandController.OrderByDate();
								break;

							default:
								ShowMessage("Неверная команда.", false);
								break;
						}
						break;

					case ConsoleKey.F:
						if (_location == Location.Movies)
						{
							Console.WriteLine();
							FiltByGenres();
						}
						else
						{
							ShowMessage("Неизвестная команда.", false);
						}

						break;

					case ConsoleKey.P:
						Console.WriteLine();
						_commandController.Find();
						break;

					case ConsoleKey.Escape:
						_location = Location.Catalogs;
						UpdateCommands();
						GoToCurrentLocation();
						break;

					default:
						Console.WriteLine();
						ShowMessage("Неизвестная команда.", false);
						break;
				}
			}
		}

		private static void ShowMainMenu(UserController userController)
		{
			_location = Location.Catalogs;
			UpdateCommands();

			ShowMessage($"Добро пожаловать, {userController.CurrentUser}!", true);		
		}

		private static void ShowList<T>(List<T> list)
		{
			if (list.Count > 0)
			{
				for (int i = 1; i <= list.Count; i++)
				{
					Console.WriteLine($"{i}. { list[i - 1]}");
				}				
			}
			else
			{
				Console.WriteLine("Пусто...");
			}
		}

		private static void ShowCommands()
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("КОМАНДЫ:");

			List<string> commands = _commands[_location];
			foreach (var command in commands)
			{
				Console.WriteLine(command);

			}			

			MultiplySymbol('_', Console.WindowWidth);
			Console.WriteLine();
			Console.ResetColor();			
		}

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

		private static void MultiplySymbol(char symbol, int length)
		{
			for (int i = 0; i < length; i++)
			{
				Console.Write(symbol);
			}
		}

		private static void DrawMoviesTable()
		{
			if (_catalogController.CurrentCatalog.Movies.Count > 0)
			{
				string[] columnsNames = new string[]
				{
					"Номер",
					"Название",
					"Оригинальное название",
					"Год",
					"Режиссёр",
					"Рейтинг",
					"Жанр",
					"Просмотрен",
					"Дата"
				};

				_dataTableController = new DataTableController(columnsNames);
				_dataTableController.SetMoviesData(_catalogController.CurrentCatalog.Movies);

				int[] columnsLength = _dataTableController.GetMaxLengthMoviesProperties(_catalogController.CurrentCatalog.Movies);
				int tableWidth = columnsLength.Sum() + _dataTableController.DataTable.ColumnCount + 1;

				DrawColumns(_dataTableController.DataTable.ColumnsNames, columnsLength, tableWidth);

				for (int i = 0; i < _catalogController.CurrentCatalog.Movies.Count; i++)
				{
					DrawCells(_dataTableController.DataTable.Rows[i].Cells, columnsLength);
				}
			}
			else
			{
				Console.WriteLine("Пусто...");
			}			
		}

		private static void DrawColumns(string[] columns, int[] columnsLength, int tableWidth)
		{
			MultiplySymbol('-', tableWidth);
			Console.WriteLine();

			DrawCells(columns, columnsLength);
			
			MultiplySymbol('-', tableWidth);
			Console.WriteLine();
		}

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

		private static void AddCatalog()
		{			
			Console.Write("Введите название для нового каталога: ");
			string newCatalogName = Console.ReadLine();
			_catalogController.AddCatalog(newCatalogName);
		}

		private static void OpenCatalog()
		{
			int index = ParseIntCommand("Введите номер каталога, который хотите открыть: ", _catalogController.Catalogs.Count);
			if (index == 0) return;

			_catalogController.CurrentCatalog = _catalogController.Catalogs[index - 1];

			_location = Location.Movies;
			UpdateCommands();
			GoToCurrentLocation();				
		}

		private static void UpdateCatalog()
		{
			int index = ParseIntCommand("Введите номер каталога, который хотите переименовать: ", _catalogController.Catalogs.Count);
			if (index == 0) return;

			Console.Write($"Введите новое имя для каталога {_catalogController.Catalogs[index - 1].Name}: ");
			string newCatalogName = Console.ReadLine();
			_catalogController.ChangeCatalog(index - 1, newCatalogName);
		}

		private static void RemoveCatalog()
		{
			int index = ParseIntCommand("Введите номер каталога, который хотите удалить: ", _catalogController.Catalogs.Count);
			if (index == 0) return;

			_catalogController.RemoveCatalog(index - 1);
		}

		private static void OrderByNameCatalogs()
		{
			_catalogController.OrderByNameCatalogs();

			ShowList(_catalogController.Catalogs);
		}

		private static void OrderByDateCatalogs()
		{
			_catalogController.OrderByDateCatalogs();

			ShowList(_catalogController.Catalogs);
		}

		private static void FindCatalogs()
		{
			Console.WriteLine("Введите название каталога, который нужно найти: ");
			var catalogs = _catalogController.FindCatalogs(Console.ReadLine());

			ShowList(catalogs);
		}

		private static void AddMovie()
		{
			Console.Write("Введите название фильма: ");
			string name = Console.ReadLine();

			Console.Write("Введите оригинальное название: ");
			string originalName = Console.ReadLine();

			Console.Write("Введите год: ");
			int year = ParseInt("год");

			Console.Write("Введите режиссёра: ");
			string director = Console.ReadLine();

			Console.Write("Введите рейтинг (разделительный знак - запятая): ");
			double rating = ParseDouble("рейтинг");

			ShowList(_catalogController.Genres);
			Console.Write("Введите жанры через запятую: ");			
			var genres = new List<Genre>();
			genres.AddRange(ParseGenres());

			Console.Write("Введите просмотрен ли фильм (да/нет/частично): ");
			string watched = Console.ReadLine();

			var movie = new Movie(name, originalName, year, director, rating, genres, watched);
			_catalogController.AddMovie(movie);
		}

		private static void OpenMovie()
		{
			int index = ParseIntCommand("Введите номер фильма, который хотите открыть: ", _catalogController.CurrentCatalog.Movies.Count);
			if (index == 0) return;

			_catalogController.CurrentMovie = _catalogController.CurrentCatalog.Movies[index - 1];

			Console.WriteLine($"Название: {_catalogController.CurrentMovie.Name}");
			Console.WriteLine($"Оригинальное название: {_catalogController.CurrentMovie.OriginalName}");
			Console.WriteLine($"Год: {_catalogController.CurrentMovie.Year}");
			Console.WriteLine($"Режиссёр: {_catalogController.CurrentMovie.Director}");
			Console.WriteLine($"Рейтинг: {_catalogController.CurrentMovie.Rating}");
			ShowInformation(_catalogController.CurrentMovie.Genres, "Жанр: ");
			Console.WriteLine($"Просмотрен: {_catalogController.CurrentMovie.Watched}");

			Console.WriteLine("\nНажмите любую клавишу для возврата к списку фильмов...");
			Console.ReadKey();
			GoToCurrentLocation();
		}

		private static void UpdateMovie()
		{
			int index = ParseIntCommand("Введите номер фильма, который хотите изменить: ", _catalogController.CurrentCatalog.Movies.Count);
			if (index == 0) return;

			_catalogController.CurrentMovie = _catalogController.CurrentCatalog.Movies[index - 1];

			Console.WriteLine($"1. Название: {_catalogController.CurrentMovie.Name}");
			Console.WriteLine($"2. Оригинальное название: {_catalogController.CurrentMovie.OriginalName}");
			Console.WriteLine($"3. Год: {_catalogController.CurrentMovie.Year}");
			Console.WriteLine($"4. Режиссёр: {_catalogController.CurrentMovie.Director}");
			Console.WriteLine($"5. Рейтинг: {_catalogController.CurrentMovie.Rating}");
			ShowInformation(_catalogController.CurrentMovie.Genres, "6. Жанр: ");
			Console.WriteLine($"7. Просмотрен: {_catalogController.CurrentMovie.Watched}");

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
				Console.Write($"Введите номер поля: ");
				int fieldNumber = ParseInt("Фильм");

				switch (fieldNumber)
				{
					case 0:
						return;

					case 1:
						Console.Write($"Введите название {_catalogController.CurrentMovie.Name}: ");
						name = Console.ReadLine();
						break;

					case 2:
						Console.Write($"Введите оригинальное название {_catalogController.CurrentMovie.OriginalName}: ");
						originalName = Console.ReadLine();
						break;

					case 3:
						Console.Write($"Введите год {_catalogController.CurrentMovie.Year}: ");
						year = ParseInt(Console.ReadLine());
						break;

					case 4:
						Console.Write($"Введите режиссёра {_catalogController.CurrentMovie.Director}: ");
						director = Console.ReadLine();
						break;

					case 5:
						Console.Write($"Введите рейтинг {_catalogController.CurrentMovie.Rating}: ");
						rating = ParseInt(Console.ReadLine());
						break;

					case 6:
						ShowList(_catalogController.Genres);
						Console.Write("Введите жанры через запятую: ");						
						genres.Clear();
						genres.AddRange(ParseGenres());
						break;

					case 7:
						Console.Write("Введите просмотрен ли фильм (да/нет/частично): ");
						watched = Console.ReadLine();
						break;

					default:
						Console.WriteLine();
						ShowMessage("Поле не найдено.", false);
						break;
				}

				Console.Write("Введите '1', чтобы сохранить изменения, или любую другую клавишу, чтобы продолжить переименование: ");
				if (ParseInt("Фильм") == 1) break;
			}

			var movie = new Movie(name, originalName, year, director, rating, genres, watched, additionDate);
			_catalogController.ChangeMovie(index - 1, movie);
		}

		private static void RemoveMovie()
		{
			int index = ParseIntCommand("Введите номер фильма, который хотите удалить: ", _catalogController.CurrentCatalog.Movies.Count);
			if (index == 0) return;

			_catalogController.RemoveMovie(index - 1);
		}

		private static void OrderByNameMovies()
		{
			_catalogController.OrderByNameMovies();

			ShowList(_catalogController.CurrentCatalog.Movies);
		}

		private static void OrderByDateMovies()
		{
			_catalogController.OrderByDateMovies();

			ShowList(_catalogController.CurrentCatalog.Movies);
		}

		private static void FindMovies()
		{
			Console.Write("Введите название фильма, который нужно найти: ");
			var movies = _catalogController.FindMovies(Console.ReadLine());

			ShowList(movies);
		}

		public static void FiltByGenres()
		{
			ShowList(_catalogController.Genres);
			Console.Write("Введите жанры из списка через запятую: ");			
			var genres = new List<Genre>();
			genres.AddRange(ParseGenres());

			var movies = _catalogController.FiltByGenres(genres);

			Console.Clear();
			ShowCommands();

			Console.WriteLine("Фильмы по Вашему запросу:");
			ShowList(movies);
		}

		private static int ParseInt(string name)
		{
			if (int.TryParse(Console.ReadLine(), out int result))
			{
				return result;
			}
			else
			{
				Console.WriteLine($"{name} не найден");
				GoToCurrentLocation();
				return 0;
			}
		}

		private static double ParseDouble(string name)
		{
			if (double.TryParse(Console.ReadLine(), out double result))
			{
				return result;
			}
			else
			{
				Console.WriteLine($"Неверный формат поля {name}");
				GoToCurrentLocation();
				return 0;
			}
		}

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

		private static int ParseIntCommand(string message, int listLength)
		{
			Console.Write($"\n{message}");
			if (int.TryParse(Console.ReadLine(), out int result) && result > 0 && result <= listLength)
			{
				return result;
			}
			else
			{
				ShowMessage("Не найдено.", false);
				return 0;
			}
		}

		private static void GoToCurrentLocation()
		{
			Console.Clear();

			ShowCommands();		

			switch (_location)
			{
				case Location.Catalogs:
					ShowList(_catalogController.Catalogs);
					break;

				case Location.Movies:
					Console.WriteLine($"{_catalogController.CurrentCatalog.Name}:");
					DrawMoviesTable();
					break;
			}
		}

		private static void UpdateCommands()
		{
			_commandController = new CommandController();

			if (_location == Location.Catalogs)
			{
				_commandController.Adding += AddCatalog;
				_commandController.Opening += OpenCatalog;
				_commandController.Updating += UpdateCatalog;
				_commandController.Removing += RemoveCatalog;
				_commandController.OrderingByName += OrderByNameCatalogs;
				_commandController.OrderingByDate += OrderByDateCatalogs;
				_commandController.Finding += FindCatalogs;
			}
			else
			{
				_commandController.Adding += AddMovie;
				_commandController.Opening += OpenMovie;
				_commandController.Updating += UpdateMovie;
				_commandController.Removing += RemoveMovie;
				_commandController.OrderingByName += OrderByNameMovies;
				_commandController.OrderingByDate += OrderByDateMovies;
				_commandController.Finding += FindMovies;
			}
		}
	}
}
