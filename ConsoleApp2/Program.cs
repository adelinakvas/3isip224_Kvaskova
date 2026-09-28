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
                Console.WriteLine("\n--- СИСТЕМА УЧЁТА ТОВАРОВ ---");
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
    }
