using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RTOS
{
    /// <summary>
    /// Interaction logic for VisualizationWindow.xaml
    /// </summary>
    public partial class VisualizationWindow : Window
    {
        Hand hand { get; set; }
        public VisualizationWindow()
        {
            InitializeComponent();
            hand = new Hand(RobotRect, ScalpelRect, rbScalpel, rbHumanCall, MyCanvas);
        }

        private void BtnExec_Click(object sender, RoutedEventArgs e)
        {
            var comm = txtBox.Text;
            hand.action(comm);
        }
    }
}
