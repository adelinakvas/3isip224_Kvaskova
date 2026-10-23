using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
    }
    internal class Program
    {
        static List<Book> books = new List<Book>();
        static int idCounter = 1;

        static void Main(string[] args)
        {
            books.Add(new Book { Id = idCounter++, Title = "Война и мир", Author = "Лев Толстой", Genre = "Роман", Year = 1869, Price = 1200 });
            books.Add(new Book { Id = idCounter++, Title = "Капитанская дочка", Author = "Александр Пушкин", Genre = "Повесть", Year = 1836, Price = 500 });
            books.Add(new Book { Id = idCounter++, Title = "Преступление и наказание", Author = "Фёдор Достоевский", Genre = "Роман", Year = 1866, Price = 800 });
            books.Add(new Book { Id = idCounter++, Title = "Мёртвые души", Author = "Николай Гоголь", Genre = "Поэма", Year = 1842, Price = 650 });
            books.Add(new Book { Id = idCounter++, Title = "Руслан и Людмила", Author = "Александр Пушкин", Genre = "Поэма", Year = 1820, Price = 450 });
            while (true)
            {
                Console.WriteLine("\nГЛАВНОЕ МЕНЮ");
                Console.WriteLine("1. Показать все книги");
                Console.WriteLine("2. Добавить книгу");
                Console.WriteLine("3. Удалить книгу по ID");
                Console.WriteLine("4. Найти книгу");
                Console.WriteLine("5. Сортировать книги");
                Console.WriteLine("6. Самая дорогая и дешёвая книга");
                Console.WriteLine("7. Группировать по авторам");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите команду: ");

                string cmd = Console.ReadLine();

                if (cmd == "0") break;

                try
                {
                    if (cmd == "1") ShowAll();
                    else if (cmd == "2") AddBook();
                    else if (cmd == "3") RemoveBook();
                    else if (cmd == "4") SearchBook();
                    else if (cmd == "5") SortBooks();
                    else if (cmd == "6") ShowMinMaxPrice();
                    else if (cmd == "7") GroupByAuthor();
                    else Console.WriteLine("Неверная команда.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
        }
        static void ShowAll()
        {
            Console.WriteLine("\nСписок всех книг");
            foreach (var b in books)
                Console.WriteLine($"[{b.Id}] {b.Author} — \"{b.Title}\" ({b.Genre}, {b.Year} г.) — {b.Price} руб.");
        }

        static void AddBook()
        {
            Console.WriteLine("\nДобавление книги");
            Console.Write("Название: "); string title = Console.ReadLine();
            Console.Write("Автор: "); string author = Console.ReadLine();
            Console.Write("Жанр: "); string genre = Console.ReadLine();
            Console.Write("Год: "); int year = int.Parse(Console.ReadLine());
            Console.Write("Цена: "); decimal price = decimal.Parse(Console.ReadLine());

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author) || year <= 0 || price <= 0)
            {
                Console.WriteLine("Ошибка! Данные не могут быть пустыми, а год и цена должны быть больше 0.");
                return;
            }

            books.Add(new Book { Id = idCounter++, Title = title, Author = author, Genre = genre, Year = year, Price = price });
            Console.WriteLine("Книга успешно добавлена.");
        }
        static void RemoveBook()
        {
            Console.WriteLine("\nУдаление книги");
            Console.Write("Введите ID для удаления: ");
            int id = int.Parse(Console.ReadLine());
            Book book = books.FirstOrDefault(b => b.Id == id);

            if (book != null) { books.Remove(book); Console.WriteLine("Книга удалена."); }
            else Console.WriteLine("Книга с таким ID не найдена.");
        }

        static void SearchBook()
        {
            Console.WriteLine("\nПоиск книг");
            Console.Write("Введите текст для поиска (по названию, автору или жанру): ");
            string q = Console.ReadLine().ToLower();

            var result = books.Where(b => b.Title.ToLower().Contains(q) || b.Author.ToLower().Contains(q) || b.Genre.ToLower().Contains(q));

            foreach (var b in result)
                Console.WriteLine($"[{b.Id}] {b.Author} — \"{b.Title}\" ({b.Genre})");
        }
        static void SortBooks()
        {
            Console.WriteLine("\n Сортировка ");
            Console.WriteLine("1. По названию");
            Console.WriteLine("2. По году издания");
            Console.Write("Выберите вариант: ");
            string mode = Console.ReadLine();

            var sorted = mode == "1" ? books.OrderBy(b => b.Title) : books.OrderBy(b => b.Year);

            foreach (var b in sorted)
                Console.WriteLine($"{b.Title} ({b.Year} г.)");
        }

        static void ShowMinMaxPrice()
        {
            if (!books.Any()) return;
            Console.WriteLine("\n Статистика цен ");
            var max = books.OrderByDescending(b => b.Price).First();
            var min = books.OrderBy(b => b.Price).First();

            Console.WriteLine($"Самая дорогая: \"{max.Title}\" ({max.Price} руб.)");
            Console.WriteLine($"Самая дешевая: \"{min.Title}\" ({min.Price} руб.)");
        }
        static void GroupByAuthor()
        {
            Console.WriteLine("\n Группировка по авторам ");
            var groups = books.GroupBy(b => b.Author);
            foreach (var g in groups)
            {
                Console.WriteLine($"Автор: {g.Key} — Книг в базе: {g.Count()}");
            }
        }
    }
}