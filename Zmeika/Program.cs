using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

class Zmeika
{
    class Program
    {
        protected enum max
        {
            Ширина = 20,
            Высота_тоже = 10
        }
        protected static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                Console.SetWindowSize(20 * 3, 20 * 3);
                Console.SetBufferSize(20 * 3, 20 * 3);
                Console.SetCursorPosition(0, Convert.ToInt32(max.Высота_тоже) + 2);
                Console.WriteLine("Игра окончена! Нажмите любую клавишу, чтобы начать игру!");
            }
            else Console.WriteLine("Нажмите любую клавишу, чтобы начать игру!");
            Console.ReadKey(true);

            Console.Clear();
            print_pole(Convert.ToInt32(max.Ширина), Convert.ToInt32(max.Высота_тоже));
            new snake();
        }
        static void print_pole(int width, int height)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine(' ' + string.Join("", Enumerable.Repeat("_", width)));
            for (int i = 0; i < height; i++)
                Console.WriteLine('|' + string.Join("", Enumerable.Repeat(" ", width)) + '|');
            Console.WriteLine(' ' + string.Join("", Enumerable.Repeat("¯", width)));
        }

    }
    class Coordinate
    {
        public int x;
        public int y;
        public Coordinate() { }
        public Coordinate(int x, int y) { this.x = x; this.y = y; }
        public void set(Coordinate obj) { x = obj.x; y = obj.y; }
        public bool get(Coordinate obj) => obj.x == x && obj.y == y;
    }
    class snake : Program
    {
        private List<Coordinate> snake_;
        private string direction = "право", tmp_direction = "";
        Coordinate dot = new Coordinate();
        bool game = false;
        public snake()
        {
            snake_ = new List<Coordinate>()
            {
                new Coordinate(8,8),
                new Coordinate(7,8),
                new Coordinate(6,8)
            };
            random_dot();
            print();
        }
        private void readkey()
        {
            Stopwatch sw = new Stopwatch();
            while (game)
            {
                sw.Restart();
                while (sw.ElapsedMilliseconds <= 200)
                {
                    direction = Console.ReadKey(true).Key switch
                    {
                        ConsoleKey.W when direction != "вниз" => "вверх",
                        ConsoleKey.S when direction != "вверх" => "вниз",
                        ConsoleKey.A when direction != "право" => "лево",
                        ConsoleKey.D when direction != "лево" => "право",
                        _ => direction
                    };
                }
            }
        }
        private void print()
        {
            game = true;
            new Thread(readkey).Start();
            Coordinate tmp = new Coordinate(), result = new Coordinate();
            while (game)
            {
                tmp.set(snake_[0]);
                snake_[0] = direction switch
                {
                    "право" => tmp_direction == "лево" ? snake_[0] : new Coordinate(snake_[0].x + 1, snake_[0].y),
                    "лево" => tmp_direction == "право" ? snake_[0] : new Coordinate(snake_[0].x - 1, snake_[0].y),
                    "вверх" => tmp_direction == "вних" ? snake_[0] : new Coordinate(snake_[0].x, snake_[0].y - 1),
                    "вниз" => tmp_direction == "вверх" ? snake_[0] : new Coordinate(snake_[0].x, snake_[0].y + 1),
                    _ => snake_[0],
                };
                foreach (var a in snake_)
                    if (a != snake_[0] && a.x == snake_[0].x && a.y == snake_[0].y) game = false;

                if (snake_[0].x == 0 || snake_[0].y == 0 || snake_[0].x == Convert.ToInt32(max.Ширина)+1 || snake_[0].y == Convert.ToInt32(max.Высота_тоже)+1) game = false;
                if (dot.get(tmp))
                {
                    snake_.Add(new Coordinate(snake_[^1].x, snake_[^1].y));
                    random_dot(dot);
                }
                foreach (var a in snake_)
                {
                    Console.SetCursorPosition(a.x, a.y);
                    Console.WriteLine(' ');
                }
                foreach (var a in snake_)
                {
                    if (a != snake_[0])
                    {
                        result.set(a);
                        a.set(tmp);
                        tmp.set(result);
                    }
                }
                foreach (var a in snake_)
                {
                    Console.SetCursorPosition(a.x, a.y);
                    Console.WriteLine('#');
                }
                Console.SetCursorPosition(0, Convert.ToInt32(max.Высота_тоже) + 1);
                Thread.Sleep(200);
            }
            Console.Clear();
            Console.WriteLine("Конец игры(");
        }
        void random_dot(Coordinate old_dot = null, Coordinate fix_coord = null)
        {
            if (old_dot != null)
            {
                Console.SetCursorPosition(old_dot.x, old_dot.y);
                Console.WriteLine(" ");
            }
            if (fix_coord != null)
                dot.set(fix_coord);
            else
                dot = new Coordinate(new Random().Next(1, Convert.ToInt32(max.Ширина)), new Random().Next(1, Convert.ToInt32(max.Высота_тоже)));
            if (!snake_.Contains(dot))
            {
                Console.SetCursorPosition(dot.x, dot.y);
                Console.WriteLine('•');
                Console.SetCursorPosition(0, Convert.ToInt32(max.Высота_тоже) + 1);
            }
            else random_dot();
        }
    }
}