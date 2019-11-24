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
        Executor timer;
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
            //Executed.Document.Blocks.Add(new Paragraph(new Run("Executed:")));
            //Executed.Document.Blocks.Add(new Paragraph(new Run(File.ReadAllText(log))));

            timer = new Executor(GetParsedCommands(), TimeSpan.FromSeconds(1), Executed);
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

                switch (command.Trim().Substring(0, command.Length - 1))
                {
                    case "move_left":
                        SituationInfo.SetHandX(SituationInfo.GetHandX() - shift);
                        break;
                    case "move_right":
                        SituationInfo.SetHandX(SituationInfo.GetHandX() + shift);
                        break;
                    case "move_up":
                        SituationInfo.SetHandY(SituationInfo.GetHandY() - shift);
                        break;
                    case "move_down":
                        SituationInfo.SetHandY(SituationInfo.GetHandY() + shift);
                        break;
                    case "lower":
                        ImgHand.Height -= shift * 20;
                        ImgHand.Width -= shift * 20;
                        break;
                    case "rise":
                        ImgHand.Height += shift * 20;
                        ImgHand.Width += shift * 20;
                        break;
                    case "pick":
                        if (SituationInfo.ScalpelX == SituationInfo.GetHandX() && SituationInfo.ScalpelY == SituationInfo.GetHandY())
                            SituationInfo.InstrumentPicked = Instruments.Scalpel;
                        if (SituationInfo.NeedleX == SituationInfo.GetHandX() && SituationInfo.NeedleY == SituationInfo.GetHandY())
                            SituationInfo.InstrumentPicked = Instruments.Needle;
                        if (SituationInfo.PatchX == SituationInfo.GetHandX() && SituationInfo.PatchY == SituationInfo.GetHandY())
                            SituationInfo.InstrumentPicked = Instruments.Patch;
                        break;
                    case "drop":
                        SituationInfo.InstrumentPicked = Instruments.None;
                        break;
                    case "cut":
                        if (SituationInfo.InstrumentPicked == Instruments.Scalpel)
                        {
                            Line line1 = new Line
                            {
                                StrokeThickness = 6,
                                Stroke = Brushes.Red,
                                X1 = SituationInfo.GetHandX() * 20,
                                X2 = SituationInfo.GetHandX() * 20 + 20,
                                Y1 = SituationInfo.GetHandY() * 20,
                                Y2 = SituationInfo.GetHandY() * 20 + 20
                            };
                            //Line line2 = new Line
                            //{
                            //    StrokeThickness = 4,
                            //    Stroke = Brushes.Red,
                            //    X1 = SituationInfo.GetHandX() * 20 + 20,
                            //    X2 = SituationInfo.GetHandX() * 20,
                            //    Y1 = SituationInfo.GetHandY() * 20,
                            //    Y2 = SituationInfo.GetHandY() * 20 + 20
                            //};
                            MainCanvas.Children.Add(line1);
                            //MainCanvas.Children.Add(line2);
                        }
                        break;
                    case "sew":
                        if (SituationInfo.InstrumentPicked == Instruments.Needle || true)
                        {
                            Line line3 = new Line
                            {
                                StrokeThickness = 3,
                                Stroke = Brushes.BlueViolet,
                                X1 = SituationInfo.GetHandX() * 20,
                                X2 = SituationInfo.GetHandX() * 20 + 20,
                                Y1 = SituationInfo.GetHandY() * 20,
                                Y2 = SituationInfo.GetHandY() * 20 + 20
                            };
                            Line line4 = new Line
                            {
                                StrokeThickness = 3,
                                Stroke = Brushes.BlueViolet,
                                X1 = SituationInfo.GetHandX() * 20 + 20,
                                X2 = SituationInfo.GetHandX() * 20,
                                Y1 = SituationInfo.GetHandY() * 20,
                                Y2 = SituationInfo.GetHandY() * 20 + 20
                            };
                            MainCanvas.Children.Add(line3);
                            MainCanvas.Children.Add(line4);
                        }
                        break;
                    case "patch":
                        if (SituationInfo.InstrumentPicked == Instruments.Needle || true)
                        {
                            Line line5 = new Line
                            {
                                StrokeThickness = 6,
                                Stroke = Brushes.Orange,
                                X1 = SituationInfo.GetHandX() * 20,
                                X2 = SituationInfo.GetHandX() * 20 + 20,
                                Y1 = SituationInfo.GetHandY() * 20,
                                Y2 = SituationInfo.GetHandY() * 20 + 20
                            };
                            //Line line6 = new Line
                            //{
                            //    StrokeThickness = 3,
                            //    Stroke = Brushes.Orange,
                            //    X1 = SituationInfo.GetHandX() * 20 + 20,
                            //    X2 = SituationInfo.GetHandX() * 20,
                            //    Y1 = SituationInfo.GetHandY() * 20,
                            //    Y2 = SituationInfo.GetHandY() * 20 + 20
                            //};
                            MainCanvas.Children.Add(line5);
                            //MainCanvas.Children.Add(line6);
                        }
                        break;
                    case "call_human":
                        MessageBox.Show("Human called");
                        break;
                }

            }

            Executed.Document.Blocks.Add(new Paragraph(new Run(toexec)));
        }
    }
}
