using MyMovies.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyMovies.BL.Controller
{
    /// <summary>
    /// Контроллер пользователя.
    /// </summary>   
    public class UserController : ControllerBase
    {
        public UserController(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException("Имя пользователя не может быть пустым", nameof(userName));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("Пароль пользователя не может быть пустым", nameof(password));
            }

            Users = GetUsersData();

            CurrentUser = Users.SingleOrDefault(user => user.Name == userName);

            if (CurrentUser == null)
            {
                CurrentUser = new User(userName, password);
                Users.Add(CurrentUser);
                IsNewUser = true;
                Save();
            }
        }

        public List<User> Users { get; }
        public User CurrentUser { get; }
        public bool IsNewUser { get; } = false;

        public List<User> GetUsersData()
        {
            return Load<User>() ?? new List<User>();          
        }

        public void Save()
        {
            Save(Users);
        }
    }
}
