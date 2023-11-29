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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Flappy_Bird_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double score;
        int gravity = 8;
        int speedPipe = 5;
        int speedCloud = 2;
        bool gameOver;
        int distancePipe = 140;
        Point centerPipe;
        Rect flappyBirdHitBox;

        DispatcherTimer gameTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            gameTimer.Tick += MainEventTimer;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            StartGame();
        }

        private void MainEventTimer(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score;
            flappyBirdHitBox = new Rect(Canvas.GetLeft(flappyBird) + 5, Canvas.GetTop(flappyBird) + 5, flappyBird.Width - 5, flappyBird.Height - 5);
            Canvas.SetTop(flappyBird, Canvas.GetTop(flappyBird) + gravity);

            //Se o Flappy Bird for muito pra cima ou muito pra baixo
            if (Canvas.GetTop(flappyBird) < -10 || Canvas.GetTop(flappyBird) > 510)
            {
                EndGame();
            }            

            //interando sobre todas as imagem
            foreach(var x in MyCanvas.Children.OfType<Image>()) 
            {
               
                if (x.Name.ToString().Contains("pipe"))
                { 
                    //deslocando cada cano
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - speedPipe);

                    //criando uma hitbox para o cano
                    Rect pipeHitBox = new Rect(Canvas.GetLeft(x) + 5, Canvas.GetTop(x), x.Width - 5, x.Height);

                    //verificando se o hit Box no cano entra em contato com o Flappy Bird
                    if (pipeHitBox.IntersectsWith(flappyBirdHitBox))
                    {
                        EndGame();
                    }
                }
                //movimentando a nuvem e deslocando se necessário
                if (x.Name.ToString().Contains("cloud"))
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - speedCloud);
                    if (Canvas.GetLeft(x) < -250)
                    {
                        Canvas.SetLeft(x, 570);
                    }
                }
                // verificando o centro de cada Cano
                if (x.Name.ToString().Contains("pipeTop1"))
                {
                    centerPipe = new Point(x.Width / 2, Canvas.GetTop(x) + Height + (distancePipe / 2));
                }
                if (x.Name.ToString().Contains("pipeTop2"))
                {
                    centerPipe = new Point(x.Width / 2, Canvas.GetTop(x) + Height + (distancePipe / 2));
                }
                if (x.Name.ToString().Contains("pipeTop3"))
                {
                    centerPipe = new Point(x.Width / 2, Canvas.GetTop(x) + Height + (distancePipe / 2));
                }
            }            

            //Deslocando os canos de posições
            SetWidthAndHeight(pipeTop1, pipeBottom1);
            SetWidthAndHeight(pipeTop2, pipeBottom2);
            SetWidthAndHeight(pipeTop3, pipeBottom3);
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                flappyBird.RenderTransform = new RotateTransform(-20, flappyBird.Height / 2, flappyBird.Width / 2);
                gravity = -10;
            }

            if (e.Key == Key.R && gameOver == true)
            {
                StartGame();
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                flappyBird.RenderTransform = new RotateTransform(0, flappyBird.Height / 2, flappyBird.Width / 2);
                gravity = 6;
            }
        }

        private void StartGame()
        {
            MyCanvas.Focus();
            score = 0;
            gameOver = false;
            Canvas.SetTop(flappyBird, 240);

            //Alinhamento da esquerda da imagem dos Pipes e cloud
            Canvas.SetLeft(pipeTop1, 600);
            Canvas.SetLeft(pipeTop2, 950);
            Canvas.SetLeft(pipeTop3, 1300);

            Canvas.SetLeft(pipeBottom1, 600);
            Canvas.SetLeft(pipeBottom2, 950);
            Canvas.SetLeft(pipeBottom3, 1300);

            Canvas.SetLeft(cloud1, 70);
            Canvas.SetLeft(cloud2, 570);

            //Alinhamento do Topo da imagem dos Pipes
            Canvas.SetTop(pipeTop1, PipeHeight());
            Canvas.SetTop(pipeBottom1, Canvas.GetTop(pipeTop1) + pipeTop1.Height + distancePipe);
            Canvas.SetTop(pipeTop2, PipeHeight());
            Canvas.SetTop(pipeBottom2, Canvas.GetTop(pipeTop2) + pipeTop2.Height + distancePipe);
            Canvas.SetTop(pipeTop3, PipeHeight());
            Canvas.SetTop(pipeBottom3, Canvas.GetTop(pipeTop3) + pipeTop3.Height + distancePipe);

            gameTimer.Start(); 
        }
        private void EndGame()
        {
            gameOver = true;
            gameTimer.Stop();
        }

        private int PipeHeight()
        {            
            Random rnd = new Random(DateTime.Now.Millisecond);
            return rnd.Next(-350, 0);
        }

        private void SetWidthAndHeight(Image top, Image bottom)
        {
            if (Canvas.GetLeft(top) < -100)
            {
                Canvas.SetLeft(top, 950);
                Canvas.SetTop(top, PipeHeight());
                Canvas.SetLeft(bottom, 950);
                Canvas.SetTop(bottom, Canvas.GetTop(top) + bottom.Height + distancePipe);

                score += 1;
            }
        }
    }
}
