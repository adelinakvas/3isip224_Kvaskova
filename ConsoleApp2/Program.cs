using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class Program
    {
        public enum Category
        {
            Electronics = 1,
            Food,
            Clothing,
            Books
        }
        public class Product
        {
            private static int _nextId = 1000;
            public int Code { get; private set; }
            public string Name { get; private set; }
            public decimal Price { get; private set; }
            public int Quantity { get; private set; }
            public Category ProductCategory { get; private set; }
            public bool IsInStock => Quantity > 0;
            public Product(string name, decimal price, int quantity, Category category)
            {
                SetName(name);
                SetPrice(price);
                SetQuantity(quantity);
                ProductCategory = category;
                Code = _nextId++; 
            }
            public void SetName(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Название товара не может быть пустым.");
                Name = name.Trim();
            }
            public void SetPrice(decimal price)
            {
                if (price < 0)
                    throw new ArgumentException("Цена не может быть отрицательной.");
                Price = price;
            }
            public void SetQuantity(int quantity)
            {
                if (quantity < 0)
                    throw new ArgumentException("Количество не может быть отрицательным.");
                Quantity = quantity;
            }
            public override string ToString()
            {
                return $"[Код: {Code}] {Name} | Категория: {ProductCategory} | Цена: {Price:F2} руб. | Кол-во: {Quantity} шт. | В наличии: {(IsInStock ? "Да" : "Нет")}";
            }
        }
        static void Main(string[] args)
        {
        }
    }
}
