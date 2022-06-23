using MyMovies.BL.Model;
using System.Collections.Generic;
using System.Linq;

namespace MyMovies.BL.Controller
{
    public class DataTableController
    {
        public DataTableController(string[] columnsNames)
        {
            DataTable = new DataTable(columnsNames);
        }

        public DataTable DataTable { get; set; }

        public void SetMoviesData(List<Movie> movies)
        {
            DataTable.Rows.Clear();

            for (int i = 0; i < movies.Count; i++)
            {
                var row = new Row(DataTable.ColumnCount);

                row.Cells[0] = (i + 1).ToString();
                row.Cells[1] = movies[i].Name;
                row.Cells[2] = movies[i].OriginalName;
                row.Cells[3] = movies[i].Year.ToString();
                row.Cells[4] = movies[i].Director;
                row.Cells[5] = movies[i].Rating.ToString();

                if (movies[i].Genres.Count > 1)
                {
                    row.Cells[6] = $"{movies[i].Genres[0].Name}, {movies[i].Genres[1].Name}...";
                }
                else
                {
                    row.Cells[6] = movies[i].Genres[0].Name;
                }

                row.Cells[7] = movies[i].Watched;
                row.Cells[8] = movies[i].AdditionDate.ToShortDateString();

                DataTable.Rows.Add(row);
            }
        }

        public int[] GetMaxLengthMoviesProperties(List<Movie> movies)
        {
            int nameLength = movies.Max(m => m.Name.Length);
            int originalNameLength = movies.Max(m => m.OriginalName.Length);
            int directorLength = movies.Max(m => m.Director.Length);
            int watchedLength = movies.Max(m => m.Watched.Length);

            int genresLength = 0;
            foreach (var movie in movies)
            {
                if (movie.Genres.Count > 1)
                {
                    genresLength = CalculateLengthWidth(movie.Genres[0].Name.Length + movie.Genres[1].Name.Length + 5, genresLength);
                }
                else
                {
                    genresLength = CalculateLengthWidth(movie.Genres[0].Name.Length, genresLength);
                }
            }

            int numberLength = 5;
            int yearLength = 4;
            int ratingLength = 3;
            int additionDateCount = 10;

            int[] length = new int[]
            {
                numberLength,
                nameLength,
                originalNameLength,
                yearLength,
                directorLength,
                ratingLength,
                genresLength,
                watchedLength,
                additionDateCount
            };         

            return length;
        }

        private int CalculateLengthWidth(int columnLength, int length)
        {
            if (length < columnLength)
            {
                length = columnLength;
            }

            return length;
        }
    }

}
