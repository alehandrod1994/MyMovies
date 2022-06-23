using System;

namespace MyMovies.BL.Model
{
    [Serializable]
    public class Genre
    {
        public Genre(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Жанр фильма не может быть пустым", nameof(name));
            }

            Name = name;
        }

        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return Name == ((Genre)obj).Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
