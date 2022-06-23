using System;
using System.Collections.Generic;
using System.Linq;

namespace MyMovies.BL.Model
{
    [Serializable]
    public class Catalog
    {
        public Catalog(string name, User user)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Задано неверное имя каталога", nameof(name));
            }

            if (user is null)
            {
                throw new ArgumentNullException("Задано неверное имя пользователя", nameof(user));
            }

            Name = name;
            Movies = new List<Movie>();
            User = user;
            AdditionDate = DateTime.Now;
        }

        public string Name { get; set; }
        public List<Movie> Movies { get; set; }
        public User User { get; }
        public DateTime AdditionDate { get; }

        public string Add(Movie movie)
        {
            var movieFounded = Movies.SingleOrDefault(m => m.Name == movie.Name && m.OriginalName == movie.OriginalName && m.Year == movie.Year);

            string message = "";

            if (movieFounded == null)
            {
                Movies.Add(movie);
                message = "Фильм успешно добавлен.";
            }
            else
            {
                message = "Такой фильм уже есть.";
            }

            return message;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
