using System;
using System.Collections.Generic;

namespace MyMovies.BL.Model
{
    [Serializable]
    public class Movie
    {
        public Movie(string name, string originalName, int year, string director, double rating, List<Genre> genres, string watched) : 
            this(name, originalName, year, director, rating, genres, watched, DateTime.Now) { }

        public Movie(string name, string originalName, int year, string director, double rating, List<Genre> genres, string watched, DateTime dateTime)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Название фильма не может быть пустым", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(originalName))
            {
                throw new ArgumentNullException("Оригинальное название фильма не может быть пустым", nameof(originalName));
            }

            if (year <= 1800)
            {
                throw new ArgumentException("Год выхода фильма не может быть меньше или равен нулю", nameof(year));
            }

            if (string.IsNullOrWhiteSpace(director))
            {
                throw new ArgumentNullException("Имя режиссёра не может быть пустым", nameof(director));
            }

            if (rating < 0)
            {
                throw new ArgumentException("Рейтинг фильма не может быть меньше нуля", nameof(rating));
            }

            if (genres is null)
            {
                throw new ArgumentNullException("Жанр фильма не может быть пустым", nameof(genres));
            }

            if (string.IsNullOrWhiteSpace(watched))
            {
                throw new ArgumentNullException("Отметка о просмотре фильма не может быть пустой", nameof(watched));
            }

            Name = name;
            OriginalName = originalName;
            Year = year;
            Director = director;
            Rating = rating;
            Genres = genres;
            Watched = watched;
            AdditionDate = dateTime;
        }

        public string Name { get; }
        public string OriginalName { get; }
        public int Year { get; }
        public string Director { get; }
        public double Rating { get; }
        public List<Genre> Genres { get; }
        public string Watched { get; }
        public DateTime AdditionDate { get; }

        public override string ToString()
        {
            return $"{Name} ({Year})";
        }
    }
}
