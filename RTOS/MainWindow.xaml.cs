using System;
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
        const string path = ".\\Data\\Program.txt";
        const string log = ".\\Data\\Log.txt";
        const string human = ".\\Data\\Human.csv";
        const string interruptionProgramsDir = "..\\InterruptionHandling/";
        Executor executor;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                SituationInfo.mainWindow = this;
                string[] humanTxt = File.ReadLines(human).ToArray();
                for (int i = 0; i < 44; i++)
                {
                    string[] elements = humanTxt[i].Split(';');
                    for (int j = 0; j < 13; j++)
                    {
                        SituationInfo.humanMap[j, i] = int.Parse(elements[j] == "" ? "0" : elements[j]);
                        if (SituationInfo.humanMap[j, i] != 0)
                        {
                            SolidColorBrush brush = Brushes.Yellow;
                            switch (SituationInfo.humanMap[j, i])
                            {
                                case -1:
                                    brush = Brushes.Blue;
                                    break;
                                case -2:
                                    brush = Brushes.Red;
                                    break;
                                case 2:
                                    brush = Brushes.Crimson;
                                    break;
                                case 3:
                                case 4:
                                case 5:
                                case 6:
                                    brush = Brushes.Brown;
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
                            Canvas.SetLeft(myRect, i * 20);
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
                //program.Substring(1, program.Length - 2).GetBlocks(out _).Execute();
                Executed.Document.Blocks.Clear();
            }
            catch {
                MessageBox.Show("Config files are missing");
            }
        }

        private string[] GetParsedCommands()
        {
            TextRange textRange = new TextRange(
                Program.Document.ContentStart,
                Program.Document.ContentEnd
            );
            textRange.Text.Substring(1, textRange.Text.Length - 4).GetBlocks(out _).Execute();
            return File.ReadAllText(log).Split('\n').Select(x=>x.Trim()).Where(x=>x!="").ToArray();
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                executor = new Executor(GetParsedCommands(), TimeSpan.FromMilliseconds(1), Executed, this);
                SituationInfo.SetHandX(54);
                SituationInfo.SetHandY(1);
                executor.Start();
            }
            catch {
                SituationInfo.SetHandX(54);
                SituationInfo.SetHandY(1);
                MessageBox.Show("Compilation error");
            }
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
            ImgDoctor.Visibility = Visibility.Visible;
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

            var program = LoadInterruptionProgram(name);
            executor.LoadInterruptionHandlingProgram(program);
        }

        private string[] LoadInterruptionProgram(string name)
        {
            string str = File.ReadAllText(interruptionProgramsDir + name + ".txt");
            return str.Split('\n', '\r').Where(s => s != "").ToArray();
        }

        private void BtnAllClear_Click(object sender, RoutedEventArgs e)
        {
            ImgFire1.Visibility = Visibility.Hidden;
            ImgFire2.Visibility = Visibility.Hidden;
            ImgFire3.Visibility = Visibility.Hidden;
            ImgDoctor.Visibility = Visibility.Hidden;

            executor.ClearInterruptions();
        }
    }
}
