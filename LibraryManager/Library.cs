using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManager
{
    public class Library
    {
        private List<Book> books = new List<Book>();
        private int nextId = 1;

        public Library()
        {
            // Добавляем тестовые книги
            AddBook("Война и мир", "Лев Толстой", 1869, "Роман");
            AddBook("Преступление и наказание", "Федор Достоевский", 1866, "Роман");
            AddBook("Мастер и Маргарита", "Михаил Булгаков", 1967, "Фантастика");
            AddBook("Евгений Онегин", "Александр Пушкин", 1833, "Поэзия");
            AddBook("451 градус по Фаренгейту", "Рэй Брэдбери", 1953, "Фантастика");
            AddBook("Анна Каренина", "Лев Толстой", 1877, "Роман");
        }

        public void AddBook(string title, string author, int year, string genre)
        {
            books.Add(new Book
            {
                Id = nextId++,
                Title = title,
                Author = author,
                Year = year,
                Genre = genre,
                Status = "Available"
            });
        }

        public List<Book> GetAllBooks()
        {
            return books.ToList();
        }

        public void UpdateBook(int id, string title, string author, int year, string genre)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                book.Title = title;
                book.Author = author;
                book.Year = year;
                book.Genre = genre;
            }
        }

        public void DeleteBook(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                books.Remove(book);
            }
        }

        public void ChangeStatus(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                book.Status = book.Status == "Available" ? "Issued" : "Available";
            }
        }

        public List<Book> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return books.ToList();

            return books.Where(b =>
                b.Title.ToLower().Contains(query.ToLower()) ||
                b.Author.ToLower().Contains(query.ToLower())
            ).ToList();
        }

        public List<Book> FilterByGenre(string genre)
        {
            if (string.IsNullOrWhiteSpace(genre) || genre == "Все жанры")
                return books.ToList();

            return books.Where(b => b.Genre.ToLower() == genre.ToLower()).ToList();
        }

        public List<Book> FilterByStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status) || status == "Все")
                return books.ToList();

            return books.Where(b => b.Status == status).ToList();
        }

        public List<Book> SortBooks(string sortBy)
        {
            switch (sortBy)
            {
                case "Название":
                    return books.OrderBy(b => b.Title).ToList();
                case "Автор":
                    return books.OrderBy(b => b.Author).ToList();
                case "Год":
                    return books.OrderBy(b => b.Year).ToList();
                default:
                    return books.ToList();
            }
        }

        public List<string> GetAllGenres()
        {
            return books.Select(b => b.Genre).Distinct().OrderBy(g => g).ToList();
        }
    }
}