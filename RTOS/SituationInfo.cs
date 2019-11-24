using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RTOS
{
    public static class SituationInfo
    {
        public static MainWindow mainWindow { get; set; }
        public static int[,] humanMap { get; set; } = new int[15, 45];
        public static int HandX { get => (int)Canvas.GetLeft(mainWindow.ImgHand) / 10; set => Canvas.SetLeft(mainWindow.ImgHand, value * 10); }
        public static int HandY { get => (int)Canvas.GetTop(mainWindow.ImgHand) / 10; set => Canvas.SetTop(mainWindow.ImgHand, value * 10); }
        public static int ScalpelX { get => (int)Canvas.GetLeft(mainWindow.ImgHand) / 10; set => Canvas.SetLeft(mainWindow.ImgHand, value * 10); }
        public static int ScalpelY { get => (int)Canvas.GetTop(mainWindow.ImgHand) / 10; set => Canvas.SetTop(mainWindow.ImgHand, value * 10); }
        public static int NeedleX { get => (int)Canvas.GetLeft(mainWindow.ImgHand) / 10; set => Canvas.SetLeft(mainWindow.ImgHand, value * 10); }
        public static int NeedleY { get => (int)Canvas.GetTop(mainWindow.ImgHand) / 10; set => Canvas.SetTop(mainWindow.ImgHand, value * 10); }
        public static int PatchX { get => (int)Canvas.GetLeft(mainWindow.ImgHand) / 10; set => Canvas.SetLeft(mainWindow.ImgHand, value * 10); }
        public static int PatchY { get => (int)Canvas.GetTop(mainWindow.ImgHand) / 10; set => Canvas.SetTop(mainWindow.ImgHand, value * 10); }
        public static bool active { get; set; } = false;
        public static Instruments instrumentPicked { get; set; } = Instruments.None;

    }
    public enum Instruments
    {
        None = 0,
        Scalpel = 1,
        Needle = 2,
        Patch=3
    }
}
