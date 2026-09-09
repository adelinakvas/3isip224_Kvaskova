using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int count = 0;
            while (true)
            {
                Console.Write("Введите количество операций (от 2 до 40): ");
                if (int.TryParse(Console.ReadLine(), out count) && count >= 2 && count <= 40)
                {
                    break;
                }
                Console.WriteLine("Ошибка! Нужно ввести число от 2 до 40.");
            }
            string[] names = new string[count];
            double[] prices = new double[count];
            Console.WriteLine("\nВводите траты по шаблону: (Название услуги или товара; Количество денег)");
            Console.WriteLine("Пример: (Влажные салфетки \"Лента\"; 235)");
            for (int i = 0; i < count; i++)
            {
                while (true)
                {
                    Console.Write($"Запись {i + 1}: ");
                    string input = Console.ReadLine()?.Trim();

                    try
                    {
                        if (string.IsNullOrEmpty(input) || !input.StartsWith("(") || !input.EndsWith(")") || !input.Contains(";"))
                        {
                            throw new Exception();
                        }
                        if (string.IsNullOrEmpty(input) || !input.StartsWith("(") || !input.EndsWith(")") || !input.Contains(";"))
                        {
                            throw new Exception();
                        }
                        string content = input.Substring(1, input.Length - 2);
                        int separatorIndex = content.LastIndexOf(';');
                        string name = content.Substring(0, separatorIndex).Trim();
                        string amountStr = content.Substring(separatorIndex + 1).Trim();
                        if (double.TryParse(amountStr, out double amount) && amount >= 0)
                        {
                            names[i] = name;
                            prices[i] = amount;
                            break;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Неверный формат! Повторите ввод строго по шаблону.");
                    }
                }
            }
            while (true)
            {
                Console.WriteLine("\n МЕНЮ ");
                Console.WriteLine("1. Вывод данных");
                Console.WriteLine("2. Статистика (среднее, максимальное, минимальное, сумма)");
                Console.WriteLine("3. Сортировка по цене (пузырьковая сортировка)");
                Console.WriteLine("4. Конвертация валюты");
                Console.WriteLine("5. Поиск по названию");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите пункт меню: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        PrintExpenses(names, prices);
                        break;

                    case "2":
                        ShowStatistics(prices);
                        break;

                    case "3":
                        BubbleSort(names, prices);
                        Console.WriteLine("Данные отсортированы по возрастанию цены.");
                        PrintExpenses(names, prices);
                        break;

                    case "4":
                        ConvertCurrency(names, prices);
                        break;

                    case "5":
                        SearchByName(names, prices);
                        break;

                    case "0":
                        Console.WriteLine("Программа завершена.");
                        return;

                    default:
                        Console.WriteLine("Неверный пункт меню, попробуйте еще раз.");
                        break;
                }
            }
        }
    }
    
}
   
