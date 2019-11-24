using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace RTOS
{
    public class Executor
    {
        private string[] Commands { get; set; }

        private RichTextBox Log { get; }
        private readonly MainWindow main;
        private int CurrentIndex { get; set; } = 0;
        private DispatcherTimer Timer { get; }
        public Executor(string[] commands, TimeSpan interval, RichTextBox log,MainWindow _main)
        {
            Commands = (string[])commands.Clone();
            Log = log;
            main = _main;
            Timer = new DispatcherTimer
            {
                Interval = interval
            };
            Timer.Tick += Timer_Tick; 
        }

        public void Start()
        {
            Timer.Start();
            LogText("Program started");
        }

        public void LoadProgram(string[] commands)
        {
            if (Timer.IsEnabled)
            {
                throw new Exception("Execution in process");
            }

            Commands = commands;
            CurrentIndex = 0;
        }

        public void Stop()
        {
            Timer.Stop();
            LogText("Program stopped");
        }

        private void LogText(string text)
        {
            Log.Document.Blocks.Add(new Paragraph(new Run(text)));
            Log.ScrollToEnd();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (CurrentIndex < Commands.Length)
            {
                ProcessCommand(Commands[CurrentIndex]);
                ++CurrentIndex;
            }
            else
            {
                MessageBox.Show("Program finished");
                Timer.Stop();
            }
        }

        private void ProcessCommand(string command)
        {
            if (command.Trim() == "") return;

            var shift = 1;

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
                    main.ImgHand.Height -= shift * 20;
                    main.ImgHand.Width -= shift * 20;
                    break;
                case "rise":
                    main.ImgHand.Height += shift * 20;
                    main.ImgHand.Width += shift * 20;
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
                    if (SituationInfo.InstrumentPicked == Instruments.Scalpel || true)
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
                        main.MainCanvas.Children.Add(line1);
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
                        main.MainCanvas.Children.Add(line3);
                        main.MainCanvas.Children.Add(line4);
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
                        main.MainCanvas.Children.Add(line5);
                    }
                    break;
                case "call_human":
                    MessageBox.Show("Human called");
                    break;              
             
            }

            LogText($"Executed {command}");
        }
    }
}
