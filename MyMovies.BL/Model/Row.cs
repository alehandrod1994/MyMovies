using System;

namespace MyMovies.BL.Model
{
    public class Row
    {
        public Row(int columnCount)
        {
            if (columnCount < 1)
            {
                throw new ArgumentException("Количество полей не может быть меньше 1", nameof(columnCount));
            }

            Cells = new string[columnCount];
        }

        public string[] Cells { get; set; }
    }
}
