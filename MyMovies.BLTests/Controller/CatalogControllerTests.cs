using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMovies.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyMovies.BL.Controller.Tests
{
    [TestClass()]
    public class CatalogControllerTests
    {
        [TestMethod()]
        public void AddCatalogTest()
        {
            // Arrange
            var userName = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();
            var catalogName = Guid.NewGuid().ToString();
            var userController = new UserController(userName, password);
            var catalogController = new CatalogController(userController.CurrentUser);

            // Act
            catalogController.AddCatalog(catalogName);

            // Assert
            Assert.AreEqual(catalogName, catalogController.Catalogs.Last().Name);
        }

        [TestMethod()]
        public void ChangeCatalogTest()
        {
            // Arrange
            var userName = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();
            var catalogName = Guid.NewGuid().ToString();
            var newCatalogName = Guid.NewGuid().ToString();
            var userController = new UserController(userName, password);
            var catalogController = new CatalogController(userController.CurrentUser);

            // Act
            catalogController.AddCatalog(catalogName);
            var catalogIndex = catalogController.Catalogs.Count - 1;
            catalogController.ChangeCatalog(catalogIndex, newCatalogName);

            // Assert
            Assert.AreEqual(newCatalogName, catalogController.Catalogs.Last().Name);
        }

        [TestMethod()]
        public void RemoveCatalogTest()
        {
            // Arrange
            var userName = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();
            var catalogName = Guid.NewGuid().ToString();
            var userController = new UserController(userName, password);
            var catalogController = new CatalogController(userController.CurrentUser);

            // Act
            catalogController.AddCatalog(catalogName);
            var catalogCount = catalogController.Catalogs.Count;
            var catalogIndex = catalogCount - 1;
            catalogController.RemoveCatalog(catalogIndex);

            // Assert
            Assert.AreEqual(catalogCount, catalogController.Catalogs.Count + 1);
        }

        [TestMethod()]
        public void FindCatalogsTest()
        {
            // Arrange
            var userName = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();
            var catalogName = Guid.NewGuid().ToString();
            var userController = new UserController(userName, password);
            var catalogController = new CatalogController(userController.CurrentUser);

            // Act
            catalogController.AddCatalog(catalogName);
            List<Catalog> catalogs = catalogController.FindCatalogs(catalogName);

            // Assert
            Assert.IsNotNull(catalogs);
        }

        [TestMethod()]
        public void AddMovieTest()
        {
            // Arrange
            var rnd = new Random();

            var userName = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();

            var catalogName = Guid.NewGuid().ToString();
            var movieName = Guid.NewGuid().ToString();
            var movieOriginalName = Guid.NewGuid().ToString();
            var movieYear = rnd.Next(1800, DateTime.Now.Year);
            var movieDirector = Guid.NewGuid().ToString();
            var movieRating = rnd.Next(0, 10);
            var movieGenres = new List<Genre> { new Genre(Guid.NewGuid().ToString()) };
            var movieWatched = "да";
            var movieAdditionDate = DateTime.Now;
            var movie = new Movie(movieName, movieOriginalName, movieYear, movieDirector, movieRating, movieGenres, movieWatched, movieAdditionDate);

            var userController = new UserController(userName, password);
            var catalogController = new CatalogController(userController.CurrentUser);

            // Act
            catalogController.AddCatalog(catalogName);
            catalogController.CurrentCatalog = catalogController.Catalogs.Last();
            catalogController.AddMovie(movie);

            // Assert
            Assert.AreEqual(movieName, catalogController.CurrentCatalog.Movies.Last().Name);
            Assert.AreEqual(movieOriginalName, catalogController.CurrentCatalog.Movies.Last().OriginalName);
            Assert.AreEqual(movieYear, catalogController.CurrentCatalog.Movies.Last().Year);
            Assert.AreEqual(movieDirector, catalogController.CurrentCatalog.Movies.Last().Director);
            Assert.AreEqual(movieRating, catalogController.CurrentCatalog.Movies.Last().Rating);
            Assert.AreEqual(movieGenres.Last().Name, catalogController.CurrentCatalog.Movies.Last().Genres.Last().Name);
            Assert.AreEqual(movieWatched, catalogController.CurrentCatalog.Movies.Last().Watched);
            Assert.AreEqual(movieAdditionDate, catalogController.CurrentCatalog.Movies.Last().AdditionDate);
        }

        [TestMethod()]
        public void ChangeMovieTest()
        {
            // Arrange
            var rnd = new Random();

            var userName = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();

            var catalogName = Guid.NewGuid().ToString();
            var movieName = Guid.NewGuid().ToString();
            var movieOriginalName = Guid.NewGuid().ToString();
            var movieYear = rnd.Next(1800, DateTime.Now.Year);
            var movieDirector = Guid.NewGuid().ToString();
            var movieRating = rnd.Next(0, 10);
            var movieGenres = new List<Genre> { new Genre(Guid.NewGuid().ToString()) };
            var movieWatched = "да";
            var movieAdditionDate = DateTime.Now;
            var movie = new Movie(movieName, movieOriginalName, movieYear, movieDirector, movieRating, movieGenres, movieWatched, movieAdditionDate);

            var userController = new UserController(userName, password);
            var catalogController = new CatalogController(userController.CurrentUser);

            // Act
            catalogController.AddCatalog(catalogName);
            catalogController.CurrentCatalog = catalogController.Catalogs.Last();
            catalogController.AddMovie(movie);
            var movieIndex = catalogController.CurrentCatalog.Movies.Count - 1;
            catalogController.ChangeMovie(movieIndex, movie);

            // Assert
            Assert.AreEqual(movieName, catalogController.CurrentCatalog.Movies.Last().Name);
            Assert.AreEqual(movieOriginalName, catalogController.CurrentCatalog.Movies.Last().OriginalName);
            Assert.AreEqual(movieYear, catalogController.CurrentCatalog.Movies.Last().Year);
            Assert.AreEqual(movieDirector, catalogController.CurrentCatalog.Movies.Last().Director);
            Assert.AreEqual(movieRating, catalogController.CurrentCatalog.Movies.Last().Rating);
            Assert.AreEqual(movieGenres.Last().Name, catalogController.CurrentCatalog.Movies.Last().Genres.Last().Name);
            Assert.AreEqual(movieWatched, catalogController.CurrentCatalog.Movies.Last().Watched);
            Assert.AreEqual(movieAdditionDate, catalogController.CurrentCatalog.Movies.Last().AdditionDate);
        }

        [TestMethod()]
        public void RemoveMovieTest()
        {
            // Arrange
            var rnd = new Random();

            var userName = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();

            var catalogName = Guid.NewGuid().ToString();
            var movieName = Guid.NewGuid().ToString();
            var movieOriginalName = Guid.NewGuid().ToString();
            var movieYear = rnd.Next(1800, DateTime.Now.Year);
            var movieDirector = Guid.NewGuid().ToString();
            var movieRating = rnd.Next(0, 10);
            var movieGenres = new List<Genre> { new Genre(Guid.NewGuid().ToString()) };
            var movieWatched = "да";
            var movieAdditionDate = DateTime.Now;
            var movie = new Movie(movieName, movieOriginalName, movieYear, movieDirector, movieRating, movieGenres, movieWatched, movieAdditionDate);

            var userController = new UserController(userName, password);
            var catalogController = new CatalogController(userController.CurrentUser);

            // Act
            catalogController.AddCatalog(catalogName);
            catalogController.CurrentCatalog = catalogController.Catalogs.Last();
            catalogController.AddMovie(movie);
            var movieCount = catalogController.CurrentCatalog.Movies.Count;
            var movieIndex = movieCount - 1;
            catalogController.RemoveMovie(movieIndex);

            // Assert
            Assert.AreEqual(movieCount, catalogController.CurrentCatalog.Movies.Count + 1);
        }    

        [TestMethod()]
        public void FiltByGenresTest()
        {
            // Arrange
            var rnd = new Random();

            var userName = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();

            var catalogName = Guid.NewGuid().ToString();
            var movieName = Guid.NewGuid().ToString();
            var movieOriginalName = Guid.NewGuid().ToString();
            var movieYear = rnd.Next(1800, DateTime.Now.Year);
            var movieDirector = Guid.NewGuid().ToString();
            var movieRating = rnd.Next(0, 10);
            var movieGenres = new List<Genre> { new Genre(Guid.NewGuid().ToString()) };
            var movieWatched = "да";
            var movieAdditionDate = DateTime.Now;
            var movie = new Movie(movieName, movieOriginalName, movieYear, movieDirector, movieRating, movieGenres, movieWatched, movieAdditionDate);

            var userController = new UserController(userName, password);
            var catalogController = new CatalogController(userController.CurrentUser);

            // Act
            catalogController.AddCatalog(catalogName);
            catalogController.CurrentCatalog = catalogController.Catalogs.Last();
            catalogController.AddMovie(movie);

            List<Movie> movies = catalogController.FiltByGenres(movieGenres);

            // Assert
            Assert.AreEqual(movies.Last().Genres, catalogController.CurrentCatalog.Movies.Last().Genres);
        }

        [TestMethod()]
        public void FindMoviesTest()
        {
            // Arrange
            var rnd = new Random();

            var userName = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();

            var catalogName = Guid.NewGuid().ToString();
            var movieName = Guid.NewGuid().ToString();
            var movieOriginalName = Guid.NewGuid().ToString();
            var movieYear = rnd.Next(1800, DateTime.Now.Year);
            var movieDirector = Guid.NewGuid().ToString();
            var movieRating = rnd.Next(0, 10);
            var movieGenres = new List<Genre> { new Genre(Guid.NewGuid().ToString()) };
            var movieWatched = "да";
            var movieAdditionDate = DateTime.Now;          
            var movie = new Movie(movieName, movieOriginalName, movieYear, movieDirector, movieRating, movieGenres, movieWatched, movieAdditionDate);
            
            var userController = new UserController(userName, password);
            var catalogController = new CatalogController(userController.CurrentUser);

            // Act
            catalogController.AddCatalog(catalogName);
            catalogController.CurrentCatalog = catalogController.Catalogs.Last();
            catalogController.AddMovie(movie);

            List<Movie> movies = catalogController.FindMovies(movieName);

            // Assert
            Assert.IsNotNull(movies);
        }

        [TestMethod()]
        public void GetAllGenresTest()
        {
            // Arrange
            var userName = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();
            var userController = new UserController(userName, password);
            var catalogController = new CatalogController(userController.CurrentUser);

            // Act
            List<Genre> genres = catalogController.GetAllGenres();

            // Assert
            Assert.IsNotNull(genres);
        }

        [TestMethod()]
        public void GetAllMoviesTest()
        {
            // Arrange
            var userName = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();
            var userController = new UserController(userName, password);
            var catalogController = new CatalogController(userController.CurrentUser);

            // Act
            List<Movie> movies = catalogController.GetAllMovies();

            // Assert
            Assert.IsNotNull(movies);
        }

        [TestMethod()]
        public void GetAllCatalogsTest()
        {
            // Arrange
            var userName = Guid.NewGuid().ToString();
            var password = Guid.NewGuid().ToString();
            var userController = new UserController(userName, password);
            var catalogController = new CatalogController(userController.CurrentUser);

            // Act
            List<Catalog> catalogs = catalogController.GetAllCatalogs();

            // Assert
            Assert.IsNotNull(catalogs);
        }
    }
}