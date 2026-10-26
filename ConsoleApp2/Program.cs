using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public enum Rarity { Common, Rare, Epic, Legendary }

    public abstract class Item
    {
        public string Name { get; protected set; }
        public Rarity ItemRarity { get; protected set; }

        protected Item(string name, Rarity rarity)
        {
            Name = name;
            ItemRarity = rarity;
        }

        public string GetRarityPrefix()
        {
            switch (ItemRarity)
            {
                case Rarity.Common:
                    return "[Обычный]";
                case Rarity.Rare:
                    return "[Редкий]";
                case Rarity.Epic:
                    return "[Эпический]";
                case Rarity.Legendary:
                    return "[ЛЕГЕНДАРНЫЙ]";
                default:
                    return "";
            }
        }
    }

    public class Weapon : Item
    {
        public int Damage { get; private set; }
        public Weapon(string name, int damage, Rarity rarity) : base(name, rarity)
        {
            Damage = damage;
        }
    }

    public class Armor : Item
    {
        public int Defense { get; private set; }
        public Armor(string name, int defense, Rarity rarity) : base(name, rarity)
        {
            Defense = defense;
        }
    }

    public class Potion : Item
    {
        public Potion() : base("Зелье полного исцеления", Rarity.Common) { }
    }

    public abstract class Creature
    {
        public string Name { get; protected set; }
        public int MaxHp { get; protected set; }
        public int CurrentHp { get; protected set; }
        public int AttackPower { get; protected set; }
        public int DefensePower { get; protected set; }
        public bool IsFrozen { get; set; }
        public bool IsDead => CurrentHp <= 0;

        protected Creature(string name, int maxHp, int attack, int defense)
        {
            Name = name;
            MaxHp = maxHp;
            CurrentHp = maxHp;
            AttackPower = attack;
            DefensePower = defense;
            IsFrozen = false;
        }

        public void TakeDamage(int damage)
        {
            CurrentHp = Math.Max(0, CurrentHp - Math.Max(0, damage));
        }

        public void FullHeal()
        {
            CurrentHp = MaxHp;
        }
    }

    public class Player : Creature
    {
        public Weapon EquippedWeapon { get; private set; }
        public Armor EquippedArmor { get; private set; }
        public int TotalAttack => AttackPower + (EquippedWeapon != null ? EquippedWeapon.Damage : 0);
        public int TotalDefense => DefensePower + (EquippedArmor != null ? EquippedArmor.Defense : 0);

        public Player() : base("Герой", 100, 10, 5)
        {
            EquippedWeapon = new Weapon("Ржавый меч", 5, Rarity.Common);
            EquippedArmor = new Armor("Обноски путешественника", 3, Rarity.Common);
        }

        public void ChangeWeapon(Weapon w) { EquippedWeapon = w; }
        public void ChangeArmor(Armor a) { EquippedArmor = a; }
    }

    public abstract class Enemy : Creature
    {
        protected Enemy(string name, int hp, int attack, int defense) : base(name, hp, attack, defense) { }
        public abstract void AttackTarget(Player player, bool playerDefending, Random rand);

        protected void ExecuteBaseAttack(Player player, int baseDamage, bool playerDefending, Random rand)
        {
            if (playerDefending)
            {
                if (rand.NextDouble() < 0.40)
                {
                    Console.WriteLine("Вы успешно уклонились от атаки " + Name + "!");
                    return;
                }
                double blockPercent = rand.Next(70, 101) / 100.0;
                int blockedDamage = (int)(player.TotalDefense * blockPercent);
                int finalDamage = Math.Max(0, baseDamage - blockedDamage);
                Console.WriteLine("Блок поглотил " + blockedDamage + " ед. урона (" + (blockPercent * 100).ToString("F0") + "% защиты).");
                player.TakeDamage(finalDamage);
                Console.WriteLine(Name + " нанес вам " + finalDamage + " ед. урона.");
            }
            else
            {
                int finalDamage = Math.Max(1, baseDamage - player.TotalDefense);
                player.TakeDamage(finalDamage);
                Console.WriteLine(Name + " нанес вам " + finalDamage + " ед. урона.");
            }
        }
    }

    public class Goblin : Enemy
    {
        protected double CritChance;
        public Goblin(string name = "Гоблин", int hp = 30, int attack = 12, int defense = 4, double crit = 0.15)
            : base(name, hp, attack, defense)
        {
            CritChance = crit;
        }

        public override void AttackTarget(Player p, bool def, Random r)
        {
            if (r.NextDouble() < CritChance)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(Name + " наносит КРИТИЧЕСКИЙ удар!");
                Console.ResetColor();
                ExecuteBaseAttack(p, AttackPower * 2, def, r);
            }
            else
            {
                ExecuteBaseAttack(p, AttackPower, def, r);
            }
        }
    }

    public class Skeleton : Enemy
    {
        public Skeleton(string name = "Скелет", int hp = 40, int attack = 10, int defense = 6) : base(name, hp, attack, defense) { }
        public override void AttackTarget(Player p, bool def, Random r)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(Name + " полностью игнорирует вашу броню!");
            Console.ResetColor();
            if (def && r.NextDouble() < 0.40)
            {
                Console.WriteLine("Вы уклонились от атаки " + Name + "!");
            }
            else
            {
                p.TakeDamage(AttackPower);
                Console.WriteLine(Name + " нанес вам " + AttackPower + " ед. чистого урона.");
            }
        }
    }

    public class Mage : Enemy
    {
        protected double FreezeChance;
        public Mage(string name = "Маг", int hp = 25, int attack = 15, int defense = 2, double freeze = 0.20)
            : base(name, hp, attack, defense)
        {
            FreezeChance = freeze;
        }

        public override void AttackTarget(Player p, bool def, Random r)
        {
            int dmg = AttackPower;
            if (def)
            {
                if (r.NextDouble() < 0.40)
                {
                    Console.WriteLine("Вы уклонились!");
                    CheckFreeze(p, r);
                    return;
                }
                dmg = Math.Max(0, dmg - (int)(p.TotalDefense * (r.Next(70, 101) / 100.0)));
            }
            else
            {
                dmg = Math.Max(1, dmg - p.TotalDefense);
            }

            p.TakeDamage(dmg);
            Console.WriteLine(Name + " ударил магией на " + dmg + " ед. урона.");
            CheckFreeze(p, r);
        }

        private void CheckFreeze(Player p, Random r)
        {
            if (r.NextDouble() < FreezeChance)
            {
                p.IsFrozen = true;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Вы ЗАМОРОЖЕНЫ на следующий ход!");
                Console.ResetColor();
            }
        }
    }

    public class BossVVG : Goblin { public BossVVG() : base("ВВГ (Босс)", 60, 18, 5, 0.25) { } }
    public class BossKovalski : Skeleton { public BossKovalski() : base("Ковальский (Босс)", 100, 13, 8) { } }
    public class BossArchmageCpp : Mage { public BossArchmageCpp() : base("Архимаг C++ (Босс)", 45, 24, 2, 0.30) { } }
    public class BossPestovCmm : Enemy
    {
        public BossPestovCmm() : base("Пестов C-- (Босс)", 52, 18, 4) { }
        public override void AttackTarget(Player p, bool def, Random r)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(Name + " пробивает броню и замораживает воздух!");
            Console.ResetColor();
            if (def && r.NextDouble() < 0.40)
            {
                Console.WriteLine("Вы уклонились!");
            }
            else
            {
                p.TakeDamage(AttackPower);
                Console.WriteLine(Name + " нанес " + AttackPower + " ед. урона.");
            }
            if (r.NextDouble() < 0.35)
            {
                p.IsFrozen = true;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Заморозка!");
                Console.ResetColor();
            }
        }
    }

    public class GameEngine
    {
        private Player _player = new Player();
        private Random _rand = new Random();
        private int _turn = 0;

        public void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Clear();
            Console.WriteLine("=== ТЕКСТОВЫЙ РОГАЛИК ===");
            Console.WriteLine("Каждые 10 ходов - битва с БОССОМ. Нажмите любую клавишу...");
            Console.ReadKey();

            while (!_player.IsDead)
            {
                _turn++;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[ХОД " + _turn + "] HP: " + _player.CurrentHp + "/" + _player.MaxHp + " | Атк: " + _player.TotalAttack + " | Защ: " + _player.TotalDefense);
                Console.ResetColor();

                if (_turn % 10 == 0)
                {
                    SpawnBoss();
                }
                else if (_rand.Next(0, 2) == 0)
                {
                    OpenChest();
                }
                else
                {
                    EnounterEnemy();
                }

                if (!_player.IsDead)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\nНажмите клавишу для продолжения...");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nИГРА ОКОНЧЕНА! Вы продержались ходов: " + _turn);
            Console.ResetColor();
        }
        private void OpenChest()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Найден сундук!");
            Console.ResetColor();
            double roll = _rand.NextDouble();
            Rarity rarity = roll < 0.60 ? Rarity.Common : roll < 0.85 ? Rarity.Rare : roll < 0.97 ? Rarity.Epic : Rarity.Legendary;
            int bonus = 0;
            switch (rarity)
            {
                case Rarity.Rare:
                    bonus = 4;
                    break;
                case Rarity.Epic:
                    bonus = 9;
                    break;
                case Rarity.Legendary:
                    bonus = 16;
                    break;
                default:
                    bonus = 0;
                    break;
            }
            int type = _rand.Next(0, 3);
            if (type == 0)
            {
                Console.WriteLine("Вы нашли Лечебное зелье и полностью исцелились!");
                _player.FullHeal();
            }
            else if (type == 1)
            {
                Weapon nw = new Weapon("Меч Искателя", bonus + _rand.Next(3, 10), rarity);
                Console.WriteLine("Оружие: " + nw.GetRarityPrefix() + " " + nw.Name + " (+" + nw.Damage + " Атк) | Текущее: +" + _player.EquippedWeapon.Damage);
                if (AskChoice("Взять предмет?") == 1) _player.ChangeWeapon(nw);
            }
            else
            {
                Armor na = new Armor("Щит Защитника", bonus + _rand.Next(2, 7), rarity);
                Console.WriteLine("Доспех: " + na.GetRarityPrefix() + " " + na.Name + " (+" + na.Defense + " Защ) | Текущий: +" + _player.EquippedArmor.Defense);
                if (AskChoice("Взять предмет?") == 1) _player.ChangeArmor(na);
            }
        }
        private void EnounterEnemy()
        {
            Enemy enemy;
            switch (_rand.Next(0, 3))
            {
                case 0:
                    enemy = new Goblin();
                    break;
                case 1:
                    enemy = new Skeleton();
                    break;
                default:
                    enemy = new Mage();
                    break;
            }
            Battle(enemy);
        }
        private void SpawnBoss()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ВНИМАНИЕ! Появился БОСС!");
            Console.ResetColor();
            Enemy boss;
            switch (_rand.Next(0, 4))
            {
                case 0:
                    boss = new BossVVG();
                    break;
                case 1:
                    boss = new BossKovalski();
                    break;
                case 2:
                    boss = new BossArchmageCpp();
                    break;
                default:
                    boss = new BossPestovCmm();
                    break;
            }
            Battle(boss);
        }
        private void Battle(Enemy enemy)
        {
            Console.WriteLine("Битва с: " + enemy.Name + " (" + enemy.CurrentHp + " HP)");
            while (!_player.IsDead && !enemy.IsDead)
            {
                bool def = false;
                if (_player.IsFrozen)
                {
                    Console.WriteLine("Вы заморожены и пропускаете ход!");
                    _player.IsFrozen = false;
                }
                else
                {
                    int choice = AskChoice("Ваше HP: " + _player.CurrentHp + ". Выберите действие:", "Атака", "Защита");
                    if (choice == 1)
                    {
                        int dmg = Math.Max(1, _player.TotalAttack - enemy.DefensePower);
                        enemy.TakeDamage(dmg);
                        Console.WriteLine("Вы нанесли " + dmg + " урона!");
                    }
                    else
                    {
                        def = true;
                    }
                }
                if (!enemy.IsDead) enemy.AttackTarget(_player, def, _rand);
            }
            if (enemy.IsDead) Console.WriteLine("Победа над " + enemy.Name + "!");
        }
        private int AskChoice(string title, string o1 = "Взять/Да", string o2 = "Выбросить/Нет")
        {
            while (true)
            {
                Console.Write(title + " (1 - " + o1 + ", 2 - " + o2 + "): ");
                int r;
                if (int.TryParse(Console.ReadLine(), out r) && (r == 1 || r == 2)) return r;
                Console.WriteLine("Некорректный ввод!");
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            GameEngine game = new GameEngine();
            game.Run();
        }
    }
}
