using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BrailePaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int coord_x { get; set; }
        public int coord_y { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void main_canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point mouse_pos = e.GetPosition(this.main_canvas);
            coord_x = (int)Math.Round(mouse_pos.X,0);
            coord_y = (int)Math.Round(mouse_pos.Y,0);

            x_coord_label.Content = coord_x.ToString();
            y_coord_label.Content = coord_y.ToString();
        }
    }
}