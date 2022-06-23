using System;

namespace MyMovies.BL.Model
{
    [Serializable]
    public class User
    {
        public User(string name, string password)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Имя пользователя не может быть пустым", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("Пароль пользователя не может быть пустым", nameof(password));
            }

            Name = name;
            Password = password;
        }

        public string Name { get; }
        public string Password { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
