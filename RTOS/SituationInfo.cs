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
        const int shift = 20;
        public static int[,] humanMap { get; set; } = new int[15, 45];

        public static int GetHandX()
        {
            return (int)Canvas.GetLeft(mainWindow.ImgHand) / shift;
        }

        public static void SetHandX(int value)
        {
            if (InstrumentPicked==Instruments.Scalpel)
                Canvas.SetLeft(mainWindow.ImgScalpel, value * shift);
            if (InstrumentPicked == Instruments.Needle)
                Canvas.SetLeft(mainWindow.ImgNeedle, value * shift);
            if (InstrumentPicked == Instruments.Patch)
                Canvas.SetLeft(mainWindow.ImgPatch, value * shift);
            Canvas.SetLeft(mainWindow.ImgHand, value * shift);
        }

        public static int GetHandY()
        {
            return (int)Canvas.GetTop(mainWindow.ImgHand) / shift;
        }

        public static void SetHandY(int value)
        {
            Canvas.SetTop(mainWindow.ImgHand, value * shift);
        }

        public static int ScalpelX { get => (int)Canvas.GetLeft(mainWindow.ImgScalpel) / shift; set => Canvas.SetLeft(mainWindow.ImgScalpel, value * shift); }
        public static int ScalpelY { get => (int)Canvas.GetTop(mainWindow.ImgScalpel) / shift; set => Canvas.SetTop(mainWindow.ImgScalpel, value * shift); }
        public static int NeedleX { get => (int)Canvas.GetLeft(mainWindow.ImgNeedle) / shift; set => Canvas.SetLeft(mainWindow.ImgNeedle, value * shift); }
        public static int NeedleY { get => (int)Canvas.GetTop(mainWindow.ImgNeedle) / shift; set => Canvas.SetTop(mainWindow.ImgNeedle, value * shift); }
        public static int PatchX { get => (int)Canvas.GetLeft(mainWindow.ImgPatch) / shift; set => Canvas.SetLeft(mainWindow.ImgPatch, value * shift); }
        public static int PatchY { get => (int)Canvas.GetTop(mainWindow.ImgPatch) / shift; set => Canvas.SetTop(mainWindow.ImgPatch, value * shift); }
        public static bool active { get; set; } = false;
        public static Instruments InstrumentPicked { get; set; } = Instruments.None;

    }
    public enum Instruments
    {
        None = 0,
        Scalpel = 1,
        Needle = 2,
        Patch=3
    }
}
