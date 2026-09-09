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
                Console.WriteLine("3. Сортировка по цене");
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
        static void PrintExpenses(string[] names, double[] prices)
        {
            Console.WriteLine("Список трат:");
            for (int i = 0; i < names.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {names[i]} — {prices[i]} руб.");
            }
        }
        static void ShowStatistics(double[] prices)
        {
            double sum = 0;
            double min = prices[0];
            double max = prices[0];
            foreach (double price in prices)
            {
                sum += price;
                if (price < min) min = price;
                if (price > max) max = price;
            }
            double average = sum / prices.Length;
            Console.WriteLine("--- Статистика ---");
            Console.WriteLine($"Сумма: {sum} руб.");
            Console.WriteLine($"Среднее: {average:F2} руб.");
            Console.WriteLine($"Минимальное: {min} руб.");
            Console.WriteLine($"Максимальное: {max} руб.");
        }
        static void BubbleSort(string[] names, double[] prices)
        {
            for (int i = 0; i < prices.Length - 1; i++)
            {
                for (int j = 0; j < prices.Length - i - 1; j++)
                {
                    if (prices[j] > prices[j + 1])
                    {
                        double tempPrice = prices[j];
                        prices[j] = prices[j + 1];
                        prices[j + 1] = tempPrice;
                        string tempName = names[j];
                        names[j] = names[j + 1];
                        names[j + 1] = tempName;
                    }
                }
            }
        }
        static void ConvertCurrency(string[] names, double[] prices)
        {
            Console.WriteLine("Выберите валюту для конвертации из рублей:");
            Console.WriteLine("1. USD (Доллар)");
            Console.WriteLine("2. EUR (Евро)");
            Console.WriteLine("3. Ввести свой курс вручную");
            Console.Write("Выбор: ");
            string choice = Console.ReadLine();
            double rate = 0;
            if (choice == "1") rate = 85.7;
            else if (choice == "2") rate = 99.8;
            else if (choice == "3")
            {
                Console.Write("Введите курс (сколько рублей в 1 единице чужой валюты): ");
                double.TryParse(Console.ReadLine(), out rate);
            }
            if (rate <= 0)
            {
                Console.WriteLine("Некорректный курс.");
                return;
            }
            Console.WriteLine("\nРезультат конвертации:");
            for (int i = 0; i < prices.Length; i++)
            {
                double converted = prices[i] / rate;
                Console.WriteLine($"{names[i]} — {converted:F2} ед. вал. (по курсу {rate})");
            }
        }
        static void SearchByName(string[] names, double[] prices)
        {
            Console.Write("Введите название товара или услуги для поиска: ");
            string query = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(query)) return;
            bool found = false;
            Console.WriteLine("\nРезультаты поиска:");
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i].ToLower().Contains(query.ToLower()))
                {
                    Console.WriteLine($"{names[i]} — {prices[i]} руб.");
                    found = true;
                }
            }
            if (!found)
            {
                Console.WriteLine("Ничего не найдено.");
            }
        }
    }
}
   
