using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RTOS
{
    public class Hand
    {
        Rectangle HandRect { get; set; }
        Rectangle Scalpel { get; set; }
        RadioButton rbIsEmpty { get; set; }
        RadioButton rbHumanCall { get; set; }
        Canvas HandCanvas { get; set; }

        Line Incision{ get; set; }
        public Hand(Rectangle rectangle, Rectangle scalpel, RadioButton RbIsEmpty, RadioButton RbHumanCall, Canvas canvas)
        {
            HandRect = rectangle;
            Scalpel = scalpel;
            rbIsEmpty = RbIsEmpty;
            rbHumanCall = RbHumanCall;
            HandCanvas = canvas;
        }
        public void action(string command)
        {
            var x = Canvas.GetLeft(HandRect);
            var y = Canvas.GetTop(HandRect);
            var shift = 20;

            switch (command)
            {
                case "move_left":
                    x -= shift;
                    break;
                case "move_right":
                    x += shift;
                    break;
                case "move_forward":
                    y -= shift;
                    break;
                case "move_back":
                    y += shift;
                    break;
                case "down":
                    HandRect.Height -= shift*2;
                    HandRect.Width -= shift*2;
                    break;
                case "up":
                    HandRect.Height += shift*2;
                    HandRect.Width += shift*2;
                    break;
                case "pick":
                    rbIsEmpty.IsChecked = true;
                    Scalpel.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case "drop":
                    rbIsEmpty.IsChecked = false;
                    Scalpel.Visibility = System.Windows.Visibility.Visible;
                    break;
                case "cut":
                    Line l = new Line
                    {
                        StrokeThickness = 3,
                        Stroke = Brushes.DarkRed,
                        X1 = Canvas.GetLeft(HandRect) + HandRect.ActualWidth / 2,
                        X2 = Canvas.GetLeft(HandRect) + HandRect.ActualWidth / 2,
                        Y1 = Canvas.GetTop(HandRect) + HandRect.ActualHeight / 2,
                        Y2 = Canvas.GetTop(HandRect) - 20
                    };

                    Incision = l;

                    HandCanvas.Children.Add(l);
                    break;
                case "sew":
                    Incision.Stroke = Brushes.Orange;
                    break;
                case "inject":
                    Line line = new Line();
                    line.StrokeThickness = 4;
                    line.Stroke = Brushes.BlueViolet;

                    line.X1 = Canvas.GetLeft(HandRect) + HandRect.ActualWidth / 2;
                    line.X2 = Canvas.GetLeft(HandRect) + HandRect.ActualWidth / 2;
                    line.Y1 = Canvas.GetTop(HandRect) + HandRect.ActualHeight / 2;
                    line.Y2 = Canvas.GetTop(HandRect) + HandRect.ActualHeight / 2 + 2;
                    HandCanvas.Children.Add(line);

                    break;
                case "call_human":
                    rbHumanCall.IsChecked = true;
                    break;
            }
            Canvas.SetLeft(HandRect, x);
            Canvas.SetTop(HandRect, y);
        }
    }
}
