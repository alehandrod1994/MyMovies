using MyMovies.BL.Model;
using System;
using System.Collections.Generic;

namespace MyMovies.BL
{
	/// <summary>
	/// Таблица.
	/// </summary>
	public class DataTable
	{
		/// <summary>
		/// Создать новую таблицу.
		/// </summary>
		/// <param name="columnsNames"> Названия столбцов. </param>
		public DataTable(string[] columnsNames)
		{
			ColumnsNames = columnsNames ?? throw new ArgumentNullException("Названия столбцов не могут быть пустыми.", nameof(columnsNames));
						 
			Rows = new List<Row>();            
		}

		/// <summary>
		/// Названия столбцов.
		/// </summary>
		public string[] ColumnsNames { get; }

		/// <summary>
		/// Количество столбцов.
		/// </summary>
		public int ColumnCount => ColumnsNames.Length;

		/// <summary>
		/// Строки.
		/// </summary>
		public List<Row> Rows { get; set; }
	}
}
