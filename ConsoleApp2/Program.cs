using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class Program
    {
        private static List<Product> _products = new List<Product>();
        static void Main(string[] args)
        {
            SeedData();
            while (true)
            {
                Console.WriteLine("\n СИСТЕМА УЧЁТА ТОВАРОВ ");
                Console.WriteLine("1. Показать все товары");
                Console.WriteLine("2. Добавить товар");
                Console.WriteLine("3. Удалить товар");
                Console.WriteLine("4. Заказать поставку товара");
                Console.WriteLine("5. Продать товар");
                Console.WriteLine("6. Поиск товаров");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите команду: ");
                string choice = Console.ReadLine();
                Console.WriteLine();
                try
                {
                    switch (choice)
                    {
                        case "1":
                            ShowAllProducts();
                            break;
                        case "2":
                            AddProductCommand();
                            break;
                        case "3":
                            DeleteProductCommand();
                            break;
                        case "4":
                            RestockProductCommand();
                            break;
                        case "5":
                            SellProductCommand();
                            break;
                        case "6":
                            SearchProductsCommand();
                            break;
                        case "0":
                            Console.WriteLine("Программа завершена.");
                            return;
                        default:
                            Console.WriteLine("Неверная команда. Попробуйте еще раз.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }
            }
        }
        private static void SeedData()
        {
            _products.Add(new Product("Смартфон", 45000, 10, Category.Electronics));
            _products.Add(new Product("Молоко", 90, 50, Category.Food));
            _products.Add(new Product("Футболка", 1500, 25, Category.Clothing));
            _products.Add(new Product("Ноутбук", 85000, 5, Category.Electronics));
            _products.Add(new Product("Книга C#", 1200, 12, Category.Books));
        }
        private static void ShowAllProducts()
        {
            if (_products.Count == 0)
            {
                Console.WriteLine("Список товаров пуст.");
                return;
            }
            foreach (var product in _products)
            {
                Console.WriteLine(product);
            }
        }
        private static void AddProductCommand()
        {
            Console.WriteLine(" ДОБАВЛЕНИЕ ТОВАРА ");
            Console.Write("Введите название товара: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Ошибка: Название не может быть пустым.");
                return;
            }
            Console.Write("Введите цену товара: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
            {
                Console.WriteLine("Ошибка: Некорректная или отрицательная цена.");
                return;
            }
            Console.Write("Введите количество товара: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
            {
                Console.WriteLine("Ошибка: Некорректное или отрицательное количество.");
                return;
            }
            Console.WriteLine("Выберите категорию:");
            foreach (var cat in Enum.GetValues(typeof(Category)))
            {
                Console.WriteLine($"{(int)cat}. {cat}");
            }
            if (!int.TryParse(Console.ReadLine(), out int catChoice) || !Enum.IsDefined(typeof(Category), catChoice))
            {
                Console.WriteLine("Ошибка: Выбрана несуществующая категория.");
                return;
            }
            Category category = (Category)catChoice;
            Product newProduct = new Product(name, price, quantity, category);
            _products.Add(newProduct);
            Console.WriteLine($"Товар успешно добавлен! Код: {newProduct.Code}");
        }
        private static void DeleteProductCommand()
        {
            Console.WriteLine(" УДАЛЕНИЕ ТОВАРА ");
            Console.Write("Введите код товара: ");
            if (!int.TryParse(Console.ReadLine(), out int code))
            {
                Console.WriteLine("Ошибка: Некорректный формат кода.");
                return;
            }
            Product product = _products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine("Товар с таким кодом не найден.");
                return;
            }
            _products.Remove(product);
            Console.WriteLine($"Товар '{product.Name}' успешно удален.");
        }
        private static void RestockProductCommand()
        {
            Console.WriteLine(" ПОСТАВКА ТОВАРА ");
            Console.Write("Введите код товара: ");
            if (!int.TryParse(Console.ReadLine(), out int code))
            {
                Console.WriteLine("Ошибка: Некорректный формат кода.");
                return;
            }
            Product product = _products.FirstOrDefault(p => p.Code == code);
            if (product == null)
            {
                Console.WriteLine("Товар с таким кодом не найден.");
                return;
            }
            Console.Write($"Количество для поставки '{product.Name}': ");
            if (!int.TryParse(Console.ReadLine(), out int amount) || amount <= 0)
            {
                Console.WriteLine("Ошибка: Количество должно быть больше нуля.");
                return;
            }
            product.SetQuantity(product.Quantity + amount);
            Console.WriteLine($"Поставка принята. Всего: {product.Quantity}");
        }
    }
