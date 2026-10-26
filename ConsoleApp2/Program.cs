using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public abstract class Person
    {
        private string name;
        private int age;
        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Имя не может быть пустым.");
                name = value;
            }
        }
        public int Age
        {
            get => age;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Возраст должен быть больше нуля.");
                age = value;
            }
        }
        protected Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
        public virtual string GetInfo()
        {
            return $"Имя: {Name}, Возраст: {Age}";
        }
    }
    public class Student : Person
    {
        public List<Course> EnrolledCourses { get; private set; } = new List<Course>();

        public Student(string name, int age) : base(name, age) { }

        public override string GetInfo()
        {
            return $"[Студент] {base.GetInfo()}, Курсов: {EnrolledCourses.Count}";
        }
    }
    public class Teacher : Person
    {
        public List<Course> TaughtCourses { get; private set; } = new List<Course>();

        public Teacher(string name, int age) : base(name, age) { }

        public override string GetInfo()
        {
            return $"[Преподаватель] {base.GetInfo()}, Ведет курсов: {TaughtCourses.Count}";
        }
    }
    public class Course
    {
        private string title;
        public string Title
        {
            get => title;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название курса не может быть пустым.");
                title = value;
            }
        }
        public Teacher Instructor { get; set; }
        public List<Student> EnrolledStudents { get; private set; } = new List<Student>();
        public Course(string title, Teacher instructor)
        {
            Title = title;
            Instructor = instructor ?? throw new ArgumentNullException(nameof(instructor), "У курса должен быть преподаватель.");
            instructor.TaughtCourses.Add(this);
        }
    }
    internal class Program
    {
        static List<Student> students = new List<Student>();
        static List<Teacher> teachers = new List<Teacher>();
        static List<Course> courses = new List<Course>();
        static void Main(string[] args)
        {
            InitializeData();
            bool running = true;
            while (running)
            {
                Console.WriteLine("\n СИСТЕМА УПРАВЛЕНИЯ УНИВЕРСИТЕТОМ ");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Добавить преподавателя");
                Console.WriteLine("3. Создать курс");
                Console.WriteLine("4. Записать студента на курс");
                Console.WriteLine("5. Просмотр всех студентов");
                Console.WriteLine("6. Просмотр всех преподавателей");
                Console.WriteLine("7. Просмотр всех курсов (детально)");
                Console.WriteLine("8. Просмотр курсов конкретного студента");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");
                string choice = Console.ReadLine();
                Console.WriteLine();
                try
                {
                    switch (choice)
                    {
                        case "1": AddStudent(); break;
                        case "2": AddTeacher(); break;
                        case "3": CreateCourse(); break;
                        case "4": RegisterStudentToCourse(); break;
                        case "5": ShowAllStudents(); break;
                        case "6": ShowAllTeachers(); break;
                        case "7": ShowAllCourses(); break;
                        case "8": ShowStudentCourses(); break;
                        case "0": running = false; break;
                        default: Console.WriteLine("Неверный пункт меню. Попробуйте снова."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message} Действие отменено.");
                }
            }
        }
        static void InitializeData()
        {
            Teacher t1 = new Teacher("Профессор Иванов", 45);
            teachers.Add(t1);
            Student s1 = new Student("Алексей", 20);
            students.Add(s1);
            courses.Add(new Course("Программирование на C#", t1));
        }
        static void AddStudent()
        {
            Console.Write("Введите имя студента: ");
            string name = Console.ReadLine();
            Console.Write("Введите возраст студента: ");
            if (!int.TryParse(Console.ReadLine(), out int age)) throw new ArgumentException("Возраст должен быть числом.");

            students.Add(new Student(name, age));
            Console.WriteLine("Студент успешно добавлен!");
        }
        static void AddTeacher()
        {
            Console.Write("Введите имя преподавателя: ");
            string name = Console.ReadLine();
            Console.Write("Введите возраст преподавателя: ");
            if (!int.TryParse(Console.ReadLine(), out int age)) throw new ArgumentException("Возраст должен быть числом.");
            teachers.Add(new Teacher(name, age));
            Console.WriteLine("Преподаватель успешно добавлен!");
        }
        static void CreateCourse()
        {
            if (!teachers.Any())
            {
                Console.WriteLine("Сначала добавьте хотя бы одного преподавателя!");
                return;
            }
            Console.Write("Введите название курса: ");
            string title = Console.ReadLine();

            Console.WriteLine("Выберите преподавателя для курса:");
            for (int i = 0; i < teachers.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {teachers[i].Name}");
            }
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > teachers.Count)
                throw new ArgumentException("Неверный выбор преподавателя.");
            Teacher selectedTeacher = teachers[index - 1];
            courses.Add(new Course(title, selectedTeacher));
            Console.WriteLine("Курс успешно создан!");
        }
        static void RegisterStudentToCourse()
        {
            if (!students.Any() || !courses.Any())
            {
                Console.WriteLine("В системе должны быть и студенты, и курсы!");
                return;
            }
            Console.WriteLine("Выберите студента:");
            for (int i = 0; i < students.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {students[i].Name}");
            }
            if (!int.TryParse(Console.ReadLine(), out int sIdx) || sIdx < 1 || sIdx > students.Count)
                throw new ArgumentException("Неверный выбор студента.");
            Student student = students[sIdx - 1];
            Console.WriteLine("Выберите курс:");
            for (int i = 0; i < courses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {courses[i].Title}");
            }
            if (!int.TryParse(Console.ReadLine(), out int cIdx) || cIdx < 1 || cIdx > courses.Count)
                throw new ArgumentException("Неверный выбор курса.");
            Course course = courses[cIdx - 1];
            if (course.EnrolledStudents.Contains(student))
            {
                Console.WriteLine("Студент уже записан на этот курс.");
                return;
            }
            course.EnrolledStudents.Add(student);
            student.EnrolledCourses.Add(course);
            Console.WriteLine($"Студент {student.Name} успешно записан на курс '{course.Title}'!");
        }
        static void ShowAllStudents()
        {
            Console.WriteLine(" Список всех студентов ");
            if (!students.Any()) Console.WriteLine("Студентов пока нет.");
            foreach (var student in students)
            {
                Console.WriteLine(student.GetInfo());
            }
        }
        static void ShowAllTeachers()
        {
            Console.WriteLine(" Список всех преподавателей ");
            if (!teachers.Any()) Console.WriteLine("Преподавателей пока нет.");
            foreach (var teacher in teachers)
            {
                Console.WriteLine(teacher.GetInfo());
            }
        }
        static void ShowAllCourses()
        {
            Console.WriteLine(" Детальный список курсов ");
            if (!courses.Any()) Console.WriteLine("Курсов пока нет.");
            foreach (var course in courses)
            {
                Console.WriteLine($"Курс: '{course.Title}' | Преподаватель: {course.Instructor.Name}");
                Console.WriteLine($"  Записанные студенты ({course.EnrolledStudents.Count}):");
                foreach (var student in course.EnrolledStudents)
                {
                    Console.WriteLine($"  - {student.Name} (Возраст: {student.Age})");
                }
            }
        }
        static void ShowStudentCourses()
        {
            if (!students.Any())
            {
                Console.WriteLine("Студентов в системе нет.");
                return;
            }
            Console.WriteLine("Выберите студента для просмотра его курсов:");
            for (int i = 0; i < students.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {students[i].Name}");
            }
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > students.Count)
                throw new ArgumentException("Неверный выбор.");
            Student student = students[index - 1];
            Console.WriteLine($"Курсы, на которые записан {student.Name}:");
            if (!student.EnrolledCourses.Any()) Console.WriteLine("  Нет активных курсов.");
            var courseTitles = student.EnrolledCourses.Select(c => c.Title);
            foreach (var title in courseTitles)
            {
                Console.WriteLine($"  - {title}");
            }
        }
    }
}