using MyMovies.BL.Controller;
using MyMovies.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

		static void Main(string[] args)
		{
			Console.WriteLine("Вас приветствует приложение MyMovies!");

			Console.WriteLine("Введите имя пользователя:");
			var name = Console.ReadLine();

			Console.WriteLine("Введите пароль пользователя:");
			var password = Console.ReadLine();

			var userController = new UserController(name, password);
			_catalogController = new CatalogController(userController.CurrentUser);
			_commandController = new CommandController();

			_commands.Add(Location.Catalogs, new List<string>
			{
				"A - ДОБАВИТЬ каталог",
				"O - ОТКРЫТЬ каталог",
				"C - ИЗМЕНИТЬ каталог",
				"D - УДАЛИТЬ каталог",
				"S - СОРТИРОВАТЬ каталоги",
				"P - ИСКАТЬ каталог",
				"R - ВЫБРАТЬ СЛУЧАЙНЫЙ каталог",
				"ESC - ВЫХОД в главное меню"
			});

			_commands.Add(Location.Movies, new List<string>
			{
				"A - ДОБАВИТЬ фильм",
				"O - ОТКРЫТЬ фильм",
				"C - ИЗМЕНИТЬ фильм",
				"D - УДАЛИТЬ фильм",
				"S - СОРТИРОВАТЬ фильмы",
				"P - ИСКАТЬ фильм",
				"R - ВЫБРАТЬ СЛУЧАЙНЫЙ фильм",
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

			ShowMainMenu(userController.CurrentUser.Name);

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
						_commandController.Change();
						break;

					case ConsoleKey.D:
						_commandController.Remove();
						break;

					case ConsoleKey.S:
						Console.WriteLine();
						Console.Write("Введите '1', чтобы сортировать по названию, '2' - сортировать по дате: ");     

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

		/// <summary>
		/// Показать главное меню.
		/// </summary>
		/// <param name="userName"> Имя пользователя. </param>
		private static void ShowMainMenu(string userName)
		{
			_location = Location.Catalogs;
			UpdateCommands();

			ShowMessage($"Добро пожаловать, {userName}!", true);		
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
				Console.WriteLine("Пусто...");
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
				Console.WriteLine("Пусто...");
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
				int tableWidth = Console.WindowWidth - 1;

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
			Console.Write("Введите название для нового каталога: ");
			string newCatalogName = Console.ReadLine();
			_catalogController.AddCatalog(newCatalogName);
		}

		/// <summary>
		/// Открыть каталог.
		/// </summary>
		private static void OpenCatalog()
		{
			int index = ParseIntCommand("Введите номер каталога, который хотите открыть: ", _catalogController.Catalogs.Count);
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
			int index = ParseIntCommand("Введите номер каталога, который хотите переименовать: ", _catalogController.Catalogs.Count);
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
			int index = ParseIntCommand("Введите номер каталога, который хотите удалить: ", _catalogController.Catalogs.Count);
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
			Console.WriteLine("Введите название каталога, который нужно найти: ");
			var catalogs = _catalogController.FindCatalogs(Console.ReadLine());

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

			ShowNumberedList(_catalogController.Genres);
			Console.Write("Введите жанры через запятую: ");			
			var genres = new List<Genre>();
			genres.AddRange(ParseGenres());

			Console.Write("Введите просмотрен ли фильм (да/нет/частично): ");
			string watched = Console.ReadLine();

			var movie = new Movie(name, originalName, year, director, rating, genres, watched);
			_catalogController.AddMovie(movie);
		}

		/// <summary>
		/// Открыть фильм.
		/// </summary>
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

		/// <summary>
		/// Изменить фильм.
		/// </summary>
		private static void ChangeMovie()
		{
			int index = ParseIntCommand("Введите номер фильма, который хотите изменить: ", _catalogController.CurrentCatalog.Movies.Count);
			if (index == 0) return;

			List<Genre> gg = _catalogController.CurrentCatalog.Movies[0].Genres;

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
						Console.Write($"Введите название: ");
						name = Console.ReadLine();
						break;

					case 2:
						Console.Write($"Введите оригинальное название: ");
						originalName = Console.ReadLine();
						break;

					case 3:
						Console.Write($"Введите год: ");
						year = ParseInt("год");
						break;

					case 4:
						Console.Write($"Введите режиссёра: ");
						director = Console.ReadLine();
						break;

					case 5:
						Console.Write($"Введите рейтинг: ");
						rating = ParseDouble("рейтинг");
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

				while (true)
				{
					Console.Write("Сохранить? Введите 'да', чтобы сохранить изменения, 'нет' - продолжить редактирование: ");
					string result = Console.ReadLine();

					if (result.Equals("да", StringComparison.CurrentCultureIgnoreCase))
					{
						var movie = new Movie(name, originalName, year, director, rating, genres, watched, additionDate);
						_catalogController.ChangeMovie(index - 1, movie);

						return;
					}
					else if (result.Equals("нет", StringComparison.CurrentCultureIgnoreCase)) break;
				}
			}			
		}

		/// <summary>
		/// Удалить фильм.
		/// </summary>
		private static void RemoveMovie()
		{
			int index = ParseIntCommand("Введите номер фильма, который хотите удалить: ", _catalogController.CurrentCatalog.Movies.Count);
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
			Console.Write("Введите название фильма, который нужно найти: ");
			var movies = _catalogController.FindMovies(Console.ReadLine());

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
			Console.Write("Введите жанры из списка через запятую: ");			
			var genres = new List<Genre>();
			genres.AddRange(ParseGenres());

			var movies = _catalogController.FiltByGenres(genres);

			Console.Clear();
			ShowCommands();

			Console.WriteLine("Фильмы по Вашему запросу:");
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
				Console.WriteLine($"{name} не найден.");
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
				Console.WriteLine($"Неверный формат поля {name}.");
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
				ShowMessage("Не найдено.", false);
				return 0;
			}
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
