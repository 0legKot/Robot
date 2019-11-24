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
        const string path = "..\\Program.txt";
        const string log = "..\\Log.txt";
        const string human = "..\\Human.csv";
        const int shift = 1;
        public MainWindow()
        {
            InitializeComponent();
            SituationInfo.mainWindow = this;
            string[] humanTxt= File.ReadLines(human).ToArray();
            for (int i = 0; i < 44; i++)
            {
                string[] elements = humanTxt[i].Split(';');
                for (int j = 0; j < 13; j++)
                {
                    SituationInfo.humanMap[j+1, i] = int.Parse(elements[j]==""?"0":elements[j]);
                }
            }
            SituationInfo.HandX = 108;
            SituationInfo.HandY = 1;
            
            var program = File.ReadAllText(path);
            File.WriteAllText(log, "");
            Program.Document.Blocks.Clear();
            Program.Document.Blocks.Add(new Paragraph(new Run(program)));
            program.Substring(1, program.Length - 2).GetBlocks(out _).Execute();
            
            Executed.Document.Blocks.Clear();
            //Executed.Document.Blocks.Add(new Paragraph(new Run("Executed:")));
            //Executed.Document.Blocks.Add(new Paragraph(new Run(File.ReadAllText(log))));

            Executor timer = new Executor(GetParsedCommands(), TimeSpan.FromSeconds(1));
            timer.Start();
        }

        private string[] GetParsedCommands()
        {
            var toexec = File.ReadAllText(log);
            string[] commands = toexec.Split('\n');
            return commands;
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(
                Program.Document.ContentStart,
                Program.Document.ContentEnd
            );
            textRange.Text.Substring(1, textRange.Text.Length - 4).GetBlocks(out _).Execute();

            Executed.Document.Blocks.Clear();
            Executed.Document.Blocks.Add(new Paragraph(new Run("Executed:")));
            var toexec = File.ReadAllText(log);
            string[] commands = toexec.Split('\n');
            foreach (var command in commands)
            {

                if (command.Trim() == "") continue;

                switch (command.Trim().Substring(0,command.Length-1))
                {
                    case "move_left":
                        SituationInfo.HandX -= shift;
                        break;
                    case "move_right":
                        SituationInfo.HandX += shift;
                        break;
                    case "move_up":
                        SituationInfo.HandY -= shift;
                        break;
                    case "move_down":
                        SituationInfo.HandY += shift;
                        break;
                    case "down":
                        ImgHand.Height -= shift * 20;
                        ImgHand.Width -= shift * 20;
                        break;
                    case "up":
                        ImgHand.Height += shift * 20;
                        ImgHand.Width += shift * 20;
                        break;
                    case "pick":
                        if (SituationInfo.ScalpelX == SituationInfo.HandX && SituationInfo.ScalpelY == SituationInfo.HandY)
                            SituationInfo.instrumentPicked = Instruments.Scalpel;
                        if (SituationInfo.NeedleX == SituationInfo.HandX && SituationInfo.NeedleY == SituationInfo.HandY)
                            SituationInfo.instrumentPicked = Instruments.Needle;
                        if (SituationInfo.PatchX == SituationInfo.HandX && SituationInfo.PatchY == SituationInfo.HandY)
                            SituationInfo.instrumentPicked = Instruments.Patch;
                        break;
                    case "drop":
                        SituationInfo.instrumentPicked = Instruments.None;
                        break;
                    //case "cut":
                    //    Line l = new Line
                    //    {
                    //        StrokeThickness = 3,
                    //        Stroke = Brushes.DarkRed,
                    //        X1 = Canvas.GetLeft(ImgHand) + ImgHand.ActualWidth / 2,
                    //        X2 = Canvas.GetLeft(ImgHand) + ImgHand.ActualWidth / 2,
                    //        Y1 = Canvas.GetTop(ImgHand) + ImgHand.ActualHeight / 2,
                    //        Y2 = Canvas.GetTop(ImgHand) - 20
                    //    };

                    //    //Incision = l;

                    //    //HandCanvas.Children.Add(l);
                    //    break;
                    //case "sew":
                    //    //Incision.Stroke = Brushes.Orange;
                    //    break;
                    //case "inject":
                    //    Line line = new Line();
                    //    line.StrokeThickness = 4;
                    //    line.Stroke = Brushes.BlueViolet;

                    //    line.X1 = Canvas.GetLeft(ImgHand) + ImgHand.ActualWidth / 2;
                    //    line.X2 = Canvas.GetLeft(ImgHand) + ImgHand.ActualWidth / 2;
                    //    line.Y1 = Canvas.GetTop(ImgHand) + ImgHand.ActualHeight / 2;
                    //    line.Y2 = Canvas.GetTop(ImgHand) + ImgHand.ActualHeight / 2 + 2;
                    //    MainCanvas.Children.Add(line);

                    //    break;
                    case "call_human":
                        MessageBox.Show("Human called");
                        break;
                }
                
            }

            Executed.Document.Blocks.Add(new Paragraph(new Run(toexec)));
        }
    }
}
