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

namespace Aquanoid
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool left, right, ballDown = true, ballUp, ballLeft = true, ballRight;
        int ballspeed = 5;
        DispatcherTimer gameTime = new DispatcherTimer();
        List<Rectangle> itemsToRemove = new List<Rectangle>();
        public MainWindow()
        {
            InitializeComponent();
            gameTime.Interval = TimeSpan.FromMilliseconds(20);
            gameTime.Tick += GameEngine;
            gameTime.Start();
            myCanvas.Focus();
            DrawRectangles();
        }
        void GameEngine(object sender, EventArgs e)
        {
            Rect playerHitbox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player)-15, player.Width, player.Height);
            Rect ballHitbox = new Rect(Canvas.GetLeft(ball)+2, Canvas.GetTop(ball)+2, ball.Width-4, ball.Height-4);
            
            
            if (left)
            {
                if(Canvas.GetLeft(player) > 10) Canvas.SetLeft(player, Canvas.GetLeft(player) - 10);
            }
            if (right)
            {
                if (Canvas.GetLeft(player) < Application.Current.MainWindow.ActualWidth - player.Width - 35) Canvas.SetLeft(player, Canvas.GetLeft(player) + 10);
            }
            if (ballDown) Canvas.SetTop(ball, Canvas.GetTop(ball) + ballspeed);
            if (ballUp) Canvas.SetTop(ball, Canvas.GetTop(ball) - ballspeed);
            if (ballLeft) Canvas.SetLeft(ball, Canvas.GetLeft(ball) - ballspeed);
            if(ballRight) Canvas.SetLeft(ball, Canvas.GetLeft(ball) + ballspeed);
            if(ballHitbox.IntersectsWith(playerHitbox))
            {
                ballDown = false;
                ballUp = true;

            }
            if(Canvas.GetLeft(ball) <= 10)
            {
                ballLeft = false;
                ballRight = true;
            }
            if (Canvas.GetLeft(ball) >= Application.Current.MainWindow.ActualWidth - 35)
            {
                ballLeft = true;
                ballRight = false;
            }            
            foreach(var x in myCanvas.Children.OfType<Rectangle>())
            {
                if(x is Rectangle && (string)x.Tag == "block")
                {
                    Rect blockHitboxD = new Rect(Canvas.GetLeft(x) + 1, Canvas.GetTop(x) + 12, x.Width-2, 1);
                    Rect blockHitboxU = new Rect(Canvas.GetLeft(x) + 1, Canvas.GetTop(x), x.Width - 2, 1);
                    Rect blockHitboxL = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), 1, 13);
                    Rect blockHitboxR = new Rect(Canvas.GetLeft(x) + 14, Canvas.GetTop(x), 1, 13);
                    if(blockHitboxD.IntersectsWith(ballHitbox))
                    {
                        ballUp = false;
                        ballDown = true;
                        itemsToRemove.Add(x);                        
                    }
                    else if(blockHitboxL.IntersectsWith(ballHitbox))
                    {
                        ballLeft = true;
                        ballRight = false;
                    }
                    else if(blockHitboxR.IntersectsWith(ballHitbox))
                    {
                        ballLeft = false;
                        ballRight = true;
                    }
                }
            }
            if(Canvas.GetTop(ball) > Application.Current.MainWindow.ActualHeight - 15)
            {
                MessageBox.Show("Game Over");
                Application.Current.Shutdown();
            }
            foreach(var x in itemsToRemove)
            {
                myCanvas.Children.Remove(x);
            }

        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Left)
            {
                left = true;
            }
            if(e.Key == Key.Right)
            {
                right = true;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                left = false;
            }
            if (e.Key == Key.Right)
            {
                right = false;
            }
        }
        void DrawRectangles()
        {
            int blockWidth = 10;
            int blockHeight = 10;
            for(int i=1;i<91;i++)
            {
                Rectangle rec = new Rectangle
                {
                    Tag = "block",
                    Width = 30,
                    Height = 10,
                    Fill = Brushes.Blue
                };               
                Canvas.SetTop(rec, blockHeight);
                Canvas.SetLeft(rec, blockWidth);
                myCanvas.Children.Add(rec);
                blockWidth += 31;
                if (i % 18 == 0 && i != 0)
                {
                    blockHeight += 11;
                    blockWidth = 10;
                }
            }
        }
    }
}
