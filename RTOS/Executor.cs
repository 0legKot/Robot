using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RTOS
{
    public class Executor
    {
        private string[] Commands { get; }

        private int CurrentIndex { get; set; } = 0;
        private DispatcherTimer Timer { get; }
        public Executor(string[] commands, TimeSpan interval)
        {
            Commands = (string[])commands.Clone();
            Timer = new DispatcherTimer();
            Timer.Interval = interval;
            Timer.Tick += Timer_Tick; 
        }

        public void Start()
        {
            Timer.Start();
        }

        public void Stop()
        {
            Timer.Stop();
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

            var shift = 20;

            switch (command.Trim().Substring(0, command.Length - 1))
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

                case "call_human":
                    MessageBox.Show("Human called");
                    break;
                default:
                    MessageBox.Show($"Unknown command: {command}");
                    break;
            }
        }
    }
}
