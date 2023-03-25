using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace TruthTable
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void onMouseHover(object sender, MouseEventArgs e)
        {
            ScaleTransform scale = new ScaleTransform(1.25, 1.25, sendButton.ActualWidth / 2, sendButton.ActualHeight / 2);
            sendButton.RenderTransform = scale;
            sendButton.Foreground = Brushes.Black;
            Cursor = Cursors.Hand;
        }
        private void onMouseLeave(object sender, MouseEventArgs e)
        {
            sendButton.RenderTransform = null;
            sendButton.Foreground = Brushes.White;
            Cursor = Cursors.Arrow;
        }
    }
}
