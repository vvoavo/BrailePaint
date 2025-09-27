using BrailePaint.Classes;
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
        public bool drawing;
        public BrailleCanvas BrailleCanvas { get; set; }
        public MainWindow()
        {
            drawing = false;
            BrailleCanvas = new BrailleCanvas();
            InitializeComponent();
            listbox_canvas.ItemsSource = BrailleCanvas.canvas_display;
        }

        private void main_canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point mouse_pos = e.GetPosition(this.main_canvas);
            coord_x = (int)Math.Round(mouse_pos.X, 0);
            coord_y = (int)Math.Round(mouse_pos.Y, 0);

            if(drawing)
            {
                BrailleCanvas.draw(coord_x, coord_y);
            }

            x_coord_label.Content = coord_x.ToString();
            y_coord_label.Content = coord_y.ToString();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            listbox_canvas.Height = main_canvas.ActualHeight;
            listbox_canvas.Width = main_canvas.ActualWidth;
            BrailleCanvas.resize((int)main_canvas.ActualWidth, (int)main_canvas.ActualHeight);
        }

        private void listbox_canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            log.Items.Add("started drawing");
            drawing = true;
        }

        private void listbox_canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            log.Items.Add("stopped drawing");
            drawing = false;
        }
    }
}