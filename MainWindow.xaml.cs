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

namespace SnakeWPF
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Кнопка нажата");
        }
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {

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
        private Color color;
        public List<Point> body;
        private Random rnd = new Random();
        private bool BlockCatched;
        public int lastScore;
        public int PlayMode;
        public bool Loosed;

        public Snake()
        {
            BlockCatched = false;
            body = new List<Point>();
            NewBlock = new Point();
            direction = Direction.Right;
            color = Color.DeepPink;
            element.Y = 300;
            PlayMode = 1;
            Loosed = false;
            for (int i = 0; i < 3; ++i)
            {
                element.X = 60 - i * 20;
                body.Add(element);
            }
            GenerateNewBlock();
        }

        public int GetSize() => body.Count();
        public Color GetColor() => color;

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
                        element.Y %= 580;
                    else
                        GameOver(lastScore);
                }
                if (element.Y < 0)
                {
                    if (PlayMode == 1)
                        element.Y += 590;
                    else
                        GameOver(lastScore);
                }

                if (element.X > 940)
                {
                    if (PlayMode == 1)
                        element.X %= 940;
                    else
                        GameOver(lastScore);
                }

                if (element.X < 0)
                {
                    if (PlayMode == 1)
                        element.X += 940;
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
        public void Draw()
        {
           for(int i = 0; i < body.Count; i++)
            {
                Ellipse ell = new Ellipse();
                ell.Width = 20;
                ell.Height = 20;
                //ell.Children.Add(myButton);
            }
            
            
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

        public void GenerateNewBlock()
        {
            NewBlock = new Point(rnd.Next(1, 47) * 20, rnd.Next(1, 29) * 20);
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
