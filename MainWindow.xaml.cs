using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SnakeWPF
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random rnd = new Random();
        private Snake snake = new Snake();
        private int NewBlockAppearsInSec;
        private int InitTimerIntervalValue = 1000;
        private int LevelValue = 1;
        private int ScoreValue;
        private DispatcherTimer timer = new DispatcherTimer();
        
        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(InitTimerIntervalValue);
            timer.Start();
        }
        private void timer_Tick(object sender, EventArgs e)
        {

            if (!snake.IsBlockCatched())
                snake.CatchBlock();
            if (snake.IsBlockCatched())
            {
                ScoreValue += LevelValue * 10;
                if ((snake.body.Count() % 5) == 0)
                {
                    if (this.timer.Interval.Milliseconds > 50)
                        this.timer.Interval.Subtract(TimeSpan.FromMilliseconds(50));
                    ++LevelValue;
                }
                Score.Content = $"{Score}";
                Level.Content = $"{Level}";
               
                snake.GenerateNewBlock();
            }
            snake.PlayMode = GetPlayMode();
            snake.lastScore = ScoreValue;
            snake.Move();
            if (snake.Loosed)
            {
                this.timer.Stop();
            }
            Draw();

        }
        public int GetPlayMode() => (Level1.TabIndex == 0 ? 2 : 1);

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            snake = new Snake();
            LevelValue = 1;
            ScoreValue = 0;
            timer.Interval = TimeSpan.FromMilliseconds(InitTimerIntervalValue);
            timer.Start();
        }
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (snake.Loosed)
                return;
            Button button = (Button)sender;
            if (PauseButton.Content == "Pause")
            {
                PauseButton.Content = "Continue";
                timer.Stop();
            }
            else if (PauseButton.Content == "Continue")
            {
                PauseButton.Content = "Pause";
                timer.Start();
            }
        }
            private void canvas_KeyDown(object sender, KeyEventArgs e)
            {
                switch (e.Key)
                {
                case Key.W:
                    if (snake.direction != Direction.Down)
                        snake.direction = Direction.Up;
                    return;
                case Key.S:
                    if (snake.direction != Direction.Up)
                        snake.direction = Direction.Down;
                    return;
                case Key.A:
                    if (snake.direction != Direction.Right)
                        snake.direction = Direction.Left;
                    return;
                case Key.D:
                    if (snake.direction != Direction.Left)
                        snake.direction = Direction.Right;
                    return;
            }
        }
        public void Draw()
        {
            canvas.Children.Clear();
            for (int i = 0; i < snake.body.Count; i++)
            {
                Ellipse ell = new Ellipse();
                ell.Width = 20;
                ell.Height = 20;
                ell.Fill = new SolidColorBrush(Colors.DarkGray);
                ell.Uid = $"{i}";
                Canvas.SetLeft(ell, snake.body[i].X);
                Canvas.SetTop(ell, snake.body[i].Y);
                canvas.Children.Add(ell);
            }
            if (!snake.BlockCatched)
            {
                Ellipse ell = new Ellipse();
                ell.Width = 20;
                ell.Height = 20;
                ell.Fill = new SolidColorBrush(Colors.DarkGray);
                Canvas.SetLeft(ell, snake.NewBlock.X);
                Canvas.SetTop(ell, snake.NewBlock.Y);
                canvas.Children.Add(ell);
            }
            if (snake.PlayMode == 2)
                Borders.BorderThickness = new Thickness(2, 2, 2, 2);
            else
                Borders.BorderThickness = new Thickness(0, 0, 0, 0); ;
            
            /* Bitmap bmp = new Bitmap(panel.Width, panel.Height);
            panel.BackgroundImage = (Image)bmp;
            panel.BackgroundImageLayout = ImageLayout.None;

            Graphics g = Graphics.FromImage(bmp);
            Pen myPen = new Pen(Color.DeepPink, 10);
            for (int i = 0; i < body.Count(); ++i)
            {
                g.DrawEllipse(myPen, body[i].X, body[i].Y, 11, 10);
            }
            if (!BlockCatched)
            {
                g.DrawEllipse(myPen, NewBlock.X, NewBlock.Y, 11, 10);
            }
            if (PlayMode == 2)
            {
                myPen.Color = Color.DimGray;
                g.DrawLine(myPen, 1, 1, 939, 1);// 639);
                g.DrawLine(myPen, 1, 1, 1, 579);
                g.DrawLine(myPen, 1, 1, 939, 1);
                g.DrawLine(myPen, 939, 1, 939, 579);
                g.DrawLine(myPen, 1, 579, 939, 579);
            }
            myPen.Dispose();
            g.Dispose();*/
        }

        private void g1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    if (snake.direction != Direction.Down)
                        snake.direction = Direction.Up;
                    return;
                case Key.S:
                    if (snake.direction != Direction.Up)
                        snake.direction = Direction.Down;
                    return;
                case Key.A:
                    if (snake.direction != Direction.Right)
                        snake.direction = Direction.Left;
                    return;
                case Key.D:
                    if (snake.direction != Direction.Left)
                        snake.direction = Direction.Right;
                    return;
            }
        }

        private void Level1_Click(object sender, RoutedEventArgs e)
        {
            snake.PlayMode = 1;
        }

        private void Level2_Click(object sender, RoutedEventArgs e)
        {
            snake.PlayMode = 2;
        }
    }

    public enum Direction
    {
        Up = 1,
        Down,
        Left,
        Right
    }

    class Snake
    {
        public Direction direction;
        private Point element = new Point();
        public Point NewBlock;
        public List<Point> body;
        private Random rnd = new Random();
        public bool BlockCatched;
        public int lastScore;
        public int PlayMode;
        public bool Loosed;

        public Snake()
        {
            BlockCatched = false;
            body = new List<Point>();
            NewBlock = new Point();
            direction = Direction.Right;
            element.Y = 275;
            PlayMode = 1;
            Loosed = false;
            for (int i = 0; i < 3; ++i)
            {
                element.X = 60 - i * 20;
                body.Add(element);
            }
            GenerateNewBlock();
        }

        public void Move(Point p = new Point())
        {
            if (Loosed)
                return;
            if ((p.X == 0) && (p.Y == 0))
            {
                element = body[0];
                switch (direction)
                {
                    case Direction.Down:
                        element.Y += 20;
                        break;
                    case Direction.Left:
                        element.X -= 20;
                        break;
                    case Direction.Right:
                        element.X += 20;
                        break;
                    case Direction.Up:
                        element.Y -= 20;
                        break;
                }

                if (element.Y > 580)
                {
                    if (PlayMode == 1)
                        element.Y %= 550;
                    else
                        GameOver(lastScore);
                }
                if (element.Y < 0)
                {
                    if (PlayMode == 1)
                        element.Y += 550;
                    else
                        GameOver(lastScore);
                }

                if (element.X > 940)
                {
                    if (PlayMode == 1)
                        element.X %= 950;
                    else
                        GameOver(lastScore);
                }

                if (element.X < 0)
                {
                    if (PlayMode == 1)
                        element.X += 950;
                    else
                        GameOver(lastScore);
                }
                if (body.Any(x => x.Equals(element)))
                    GameOver(lastScore);
                if (Loosed)
                    return;
                body.RemoveAt(body.Count - 1);
            }
            else
            {
                element = p;
            }
            body.Insert(0, element);
        }
        

        public void GenerateNewBlock()
        {
            NewBlock = new Point(rnd.Next(1, 47) * 20, rnd.Next(1, 27) * 20);
            BlockCatched = false;
        }

        public bool IsBlockCatched() => BlockCatched;
        public void CatchBlock()
        {
            switch (direction)
            {
                case Direction.Down:
                case Direction.Up:
                    if ((body[0].X == NewBlock.X) && (Math.Abs(NewBlock.Y - body[0].Y) < 22))
                    {
                        Move(NewBlock);
                        BlockCatched = true;
                    }
                    break;
                case Direction.Left:
                case Direction.Right:
                    if ((body[0].Y == NewBlock.Y) && (Math.Abs(body[0].X - NewBlock.X) < 22))
                    {
                        Move(NewBlock);
                        BlockCatched = true;
                    }
                    break;
            }
            if (BlockCatched)
                NewBlock = new Point();

        }
        private void GameOver(int score)
        {
            Loosed = true;
            //System.Windows.Forms.MessageBox.Show($"Game Over\nYour Score: {score}");
        }

        static Predicate<Point> IsSnakePart(Point newPoint)
        {
            return delegate (Point p)
            {
                return p.Equals(newPoint);
            };
        }
    }

    }
    
