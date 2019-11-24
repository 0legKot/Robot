﻿using System;
using System.Collections.Generic;
using System.IO;
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

namespace RTOS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string path = "..\\Program.txt";
        const string log = "..\\Log.txt";
        const string human = "..\\Human.csv";
        const string interruptionProgramsDir = "..\\..\\InterruptionHandling/";
        Executor executor;
        public MainWindow()
        {
            InitializeComponent();
            SituationInfo.mainWindow = this;
            string[] humanTxt = File.ReadLines(human).ToArray();
            for (int i = 0; i < 44; i++)
            {
                string[] elements = humanTxt[i].Split(';');
                for (int j = 0; j < 13; j++)
                {
                    SituationInfo.humanMap[j + 1, i] = int.Parse(elements[j] == "" ? "0" : elements[j]);
                    if (SituationInfo.humanMap[j + 1, i] != 0)
                    {
                        SolidColorBrush brush = Brushes.Yellow;
                        switch (SituationInfo.humanMap[j + 1, i])
                        {
                            case 2: brush = Brushes.Red; 
                                break;
                            case 3:
                                brush = Brushes.Black;
                                break;
                        }
                        Rectangle myRect = new Rectangle
                        {
                            Stroke = brush,
                            Fill = brush,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Center,
                            Height = 20,
                            Width = 20
                        };
                        Canvas.SetLeft(myRect,  i * 20);
                        Canvas.SetTop(myRect, j * 20);
                        MainCanvas.Children.Add(myRect);
                    }
                }
            }
            SituationInfo.SetHandX(54);
            SituationInfo.SetHandY(1);

            var program = File.ReadAllText(path);
            File.WriteAllText(log, "");
            Program.Document.Blocks.Clear();
            Program.Document.Blocks.Add(new Paragraph(new Run(program)));
            program.Substring(1, program.Length - 2).GetBlocks(out _).Execute();
            Executed.Document.Blocks.Clear();
        }

        private string[] GetParsedCommands()
        {
            return File.ReadAllText(log).Split('\n').Select(x=>x.Trim()).Where(x=>x!="").ToArray();
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            executor = new Executor(GetParsedCommands(), TimeSpan.FromSeconds(1), Executed, this);
            executor.Start();
        }

        private void BtnFire_Click(object sender, RoutedEventArgs e)
        {
            ImgFire1.Visibility = Visibility.Visible;
            ImgFire2.Visibility = Visibility.Visible;
            ImgFire3.Visibility = Visibility.Visible;
            HandleInterrupt("fire");
        }

        private void BtnHumanEntered_Click(object sender, RoutedEventArgs e)
        {
            HandleInterrupt("human_entered");
        }

        private void BtnPatientDying_Click(object sender, RoutedEventArgs e)
        {
            HandleInterrupt("patient_dying");
        }

        private void BtnPatientConscious_Click(object sender, RoutedEventArgs e)
        {
            HandleInterrupt("patient_conscious");

        }

        private void HandleInterrupt(string name)
        {
            if (executor == null)
            {
                return;
            }

            executor.Stop();

            var program = LoadInterruptionProgram(name);
            executor.LoadProgram(program);
            executor.Start();
        }

        private string[] LoadInterruptionProgram(string name)
        {
            string str = File.ReadAllText(interruptionProgramsDir + name + ".txt");
            return str.Split('\n', '\r').Where(s => s != "").ToArray();
        }
    }
}
