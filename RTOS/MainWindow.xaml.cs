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
        public MainWindow()
        {
            InitializeComponent();
            
            string[] humanTxt= File.ReadLines(human).ToArray();
            for (int i = 0; i < 44; i++)
            {
                string[] elements = humanTxt[i].Split(';');
                for (int j = 0; j < 13; j++)
                {
                    SituationInfo.humanMap[j+1, i] = int.Parse(elements[j]==""?"0":elements[j]);
                }
            }
            SituationInfo.handX = (int)Canvas.GetLeft(ImgHand) / 20;
            SituationInfo.handY = (int)Canvas.GetTop(ImgHand) / 20;
            
            var program = File.ReadAllText(path);
            File.WriteAllText(log, "");
            Program.Document.Blocks.Clear();
            Program.Document.Blocks.Add(new Paragraph(new Run(program)));
            program.Substring(1, program.Length - 2).GetBlocks(out _).Execute();
            
            Executed.Document.Blocks.Clear();
            Executed.Document.Blocks.Add(new Paragraph(new Run("Executed:")));
            Executed.Document.Blocks.Add(new Paragraph(new Run(File.ReadAllText(log))));

            VisualizationWindow v = new VisualizationWindow();
            v.Show();
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
            Executed.Document.Blocks.Add(new Paragraph(new Run(File.ReadAllText(log))));
        }
    }
}
