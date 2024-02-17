using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Falling_Sand
{
    public partial class MainWindow : Window
    {
        Thread mouseMoveTask;
        Thread updateTaks;
        bool mouseDown = false;

        public MainWindow()
        {
            InitializeComponent();

            updateTaks = new Thread(FallingSand);
            updateTaks.Start();

            mainCanvas.Width = Data.canvasSize;
            mainCanvas.Height = Data.canvasSize;

            for (int i = 0; i < Data.matrixSize; i++)
            {
                for (int j = 0; j < Data.matrixSize; j++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Fill = Brushes.Black;
                    rect.Width = Data.sandSize;
                    rect.Height = Data.sandSize;

                    Canvas.SetTop(rect, i * Data.sandSize);
                    Canvas.SetLeft(rect, j * Data.sandSize);
                    Data.matrix[i, j] = rect;
                    mainCanvas.Children.Add(rect);
                }
            }
        }

        private void mainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mouseMoveTask = new Thread(MouseMove);
            mouseDown = true;
            mouseMoveTask.Start();

        }

        private void mainCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mouseDown = false;
        }

        private void mainCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            updateTaks.Abort();
            if (mouseMoveTask != null)
                mouseMoveTask.Abort();
        }

        private void FallingSand()
        {
            while (true)
            {
                Dispatcher.Invoke(() =>
                {
                    for (int i = Data.matrixSize - 2; i >= 0; i--) // Start from second last row
                    {
                        for (int j = 0; j < Data.matrixSize; j++)
                        {
                            if (Data.matrix[i, j].Fill == Brushes.White)
                            {
                                // Check below
                                if (Data.matrix[i + 1, j].Fill == Brushes.Black)
                                {
                                    Data.matrix[i, j].Fill = Brushes.Black;
                                    Data.matrix[i + 1, j].Fill = Brushes.White;
                                }
                                else
                                {
                                    if (j - 1 > 0 && j + 1 < Data.matrixSize)
                                    {
                                        // Check left
                                        if (Data.matrix[i + 1, j - 1].Fill == Brushes.Black && Data.matrix[i + 1, j + 1].Fill == Brushes.White)
                                        {
                                            Data.matrix[i, j].Fill = Brushes.Black;
                                            Data.matrix[i + 1, j - 1].Fill = Brushes.White;
                                        }
                                        // Check right
                                        else if (Data.matrix[i + 1, j + 1].Fill == Brushes.Black && Data.matrix[i + 1, j - 1].Fill == Brushes.White)
                                        {
                                            Data.matrix[i, j].Fill = Brushes.Black;
                                            Data.matrix[i + 1, j + 1].Fill = Brushes.White;
                                        }
                                        // If below is occupied, go left or right
                                        else if (Data.matrix[i + 1, j - 1].Fill == Brushes.Black && Data.matrix[i + 1, j + 1].Fill == Brushes.Black)
                                        {
                                            Data.matrix[i, j].Fill = Brushes.Black;

                                            Random rand = new Random();
                                            int direction = rand.Next(2); // 0 for left, 1 for right
                                            int newJ = j + (direction == 0 ? -1 : 1);

                                            // Ensure the newJ is within bounds
                                            if (newJ >= 0 && newJ < Data.matrixSize)
                                            {
                                                Data.matrix[i + 1, newJ].Fill = Brushes.White;
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                });
                Thread.Sleep(20);
            }
        }

        private void MouseMove()
        {
            while (mouseDown)
            {
                Dispatcher.Invoke(() =>
                {
                    Point position = Mouse.GetPosition(mainCanvas);
                    Random rand = new Random();
                    int num = 4;

                    double col = position.X / Data.sandSize + rand.Next(-num, num);
                    double row = position.Y / Data.sandSize + rand.Next(-num, num);

                    if (col >= 0 && row >= 0 && col < Data.matrixSize && row < Data.matrixSize)
                    {
                        Data.matrix[(int)row, (int)col].Fill = Brushes.White;
                    }
                });
                Thread.Sleep(10);
            }
        }
    }
}
