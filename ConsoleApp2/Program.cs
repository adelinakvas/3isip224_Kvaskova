using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class TextStats
    {
        public string SourceText { get; set; }
        public int WordCount { get; set; }
        public int SentenceCount { get; set; }
        public int VowelCount { get; set; }
        public int ConsonantCount { get; set; }
        public string ShortestWord { get; set; }
        public string LongestWord { get; set; }
        public Dictionary<char, int> LetterFrequency { get; set; } = new Dictionary<char, int>();
    }
    internal class Program
    {
        static List<TextStats> history = new List<TextStats>();
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== АНАЛИЗАТОР ТЕКСТА ===");
                Console.WriteLine("1. Вставить новый текст для анализа");
                Console.WriteLine("2. Посмотреть историю статистики");
                Console.WriteLine("3. Выйти");
                Console.Write("\nВыберите действие: ");
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    AnalyzeNewText();
                }
                else if (choice == "2")
                {
                    ShowHistory();
                }
                else if (choice == "3")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Неверный ввод. Нажмите любую клавишу для повтора...");
                    Console.ReadKey();
                }
            }
        }
        static void AnalyzeNewText()
        {
            Console.Clear();
            string fullText = "";
            while (true)
            {
                Console.WriteLine("Вставьте/напишите ваш текст и нажмите ENTER:\n");
                string input = Console.ReadLine();
                if (input != null)
                {
                    fullText = input.Trim();
                }
                if (fullText.Length >= 100)
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"\nОшибка: вставлено всего {fullText.Length} символов. Нужно не менее 100.");
                    Console.WriteLine("Нажмите любую клавишу, чтобы попробовать еще раз...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            TextStats stats = new TextStats { SourceText = fullText };
            char[] delimiters = new char[] { ' ', '.', ',', '!', '?', '-', ';', ':', '\r', '\n', '(', ')', '"' };
            string[] words = fullText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            stats.WordCount = words.Length;
            if (words.Length > 0)
            {
                stats.ShortestWord = words[0];
                stats.LongestWord = words[0];
                foreach (string word in words)
                {
                    if (word.Length < stats.ShortestWord.Length)
                        stats.ShortestWord = word;

                    if (word.Length > stats.LongestWord.Length)
                        stats.LongestWord = word;
                }
            }
            string vowelsSet = "аеёиоуыэюяaeiouy";
            string consonantsSet = "бвгджзйклмнпрстфхцчшщbcdfghjklmnpqrstvwxyz";
            for (int i = 0; i < fullText.Length; i++)
            {
                char c = fullText[i];
                char lowerC = char.ToLower(c);

                if (c == '.' || c == '!' || c == '?')
                {
                    if (i == fullText.Length - 1 || (fullText[i + 1] != '.' && fullText[i + 1] != '!' && fullText[i + 1] != '?'))
                    {
                        stats.SentenceCount++;
                    }
                }
                if (char.IsLetter(c))
                {
                    if (stats.LetterFrequency.ContainsKey(lowerC))
                        stats.LetterFrequency[lowerC]++;
                    else
                        stats.LetterFrequency[lowerC] = 1;

                    if (vowelsSet.Contains(lowerC.ToString()))
                        stats.VowelCount++;
                    else if (consonantsSet.Contains(lowerC.ToString()))
                        stats.ConsonantCount++;
                }
            }
            if (stats.SentenceCount == 0 && fullText.Length > 0)
            {
                stats.SentenceCount = 1;
            }
            history.Add(stats);
            PrintStats(stats);
            Console.WriteLine("\nНажмите любую клавишу, чтобы вернуться в меню...");
            Console.ReadKey();
        }
        static void ShowHistory()
        {
            Console.Clear();
            Console.WriteLine("=== ИСТОРИЯ ПРОШЛЫХ ТЕКСТОВ ===\n");

            if (history.Count == 0)
            {
                Console.WriteLine("История пуста. Вы еще не анализировали тексты.");
            }
            else
            {
                for (int i = 0; i < history.Count; i++)
                {
                    Console.WriteLine($"--- Текст #{i + 1} ---");

                    string preview = history[i].SourceText.Length > 50
                        ? history[i].SourceText.Substring(0, 50) + "..."
                        : history[i].SourceText;
                    Console.WriteLine($"Превью: \"{preview}\"");

                    PrintStats(history[i]);
                    Console.WriteLine(new string('-', 30));
                }
            }
            Console.WriteLine("\nНажмите любую клавишу, чтобы вернуться в меню...");
            Console.ReadKey();
        }
        static void PrintStats(TextStats stats)
        {
            Console.WriteLine($"\nКоличество слов: {stats.WordCount}");
            Console.WriteLine($"Количество предложений: {stats.SentenceCount}");
            Console.WriteLine($"Количество гласных букв: {stats.VowelCount}");
            Console.WriteLine($"Количество согласных букв: {stats.ConsonantCount}");
            Console.WriteLine($"Самое короткое слово: \"{stats.ShortestWord}\"");
            Console.WriteLine($"Самое длинное слово: \"{stats.LongestWord}\"");

            Console.WriteLine("Статистика частоты букв:");
            foreach (var pair in stats.LetterFrequency)
            {
                Console.WriteLine($"  Буква '{pair.Key}': встретилась {pair.Value} раз(а)");
            }
        }
    }
}