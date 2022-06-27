using System;

namespace MyMovies.BL.Model
{
	/// <summary>
	/// Строка в таблице.
	/// </summary>
	public class Row
	{
		/// <summary>
		/// Создать новую строку.
		/// </summary>
		/// <param name="columnCount"> Количество столбцов. </param>
		public Row(int columnCount)
		{
			if (columnCount < 1)
			{
				throw new ArgumentException("Количество столбцов не может быть меньше 1.", nameof(columnCount));
			}

			Cells = new string[columnCount];
		}

		/// <summary>
		/// Ячейки.
		/// </summary>
		public string[] Cells { get; set; }
	}
}
