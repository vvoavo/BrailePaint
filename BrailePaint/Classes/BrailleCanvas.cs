using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace BrailePaint.Classes
{
    public class BrailleCanvas
    {
        public ObservableCollection<string> canvas_display;
        public char[,] canvas;
        int w;
        int h;
        double pixel_to_char_ratio_x = 0.138;
        double pixel_to_char_ratio_y = 0.055;

        public BrailleCanvas()
        {
            canvas_display = new ObservableCollection<string>();
        }

        public void draw(int pix_x, int pix_y)
        {
            int y = (int)Math.Round(pix_y * pixel_to_char_ratio_y, 0, MidpointRounding.ToNegativeInfinity);
            int x = (int)Math.Round(pix_x * pixel_to_char_ratio_x, 0, MidpointRounding.ToNegativeInfinity);

            y--;
            x--;

            canvas[y, x] = '@';
            refresh_row(y);
        }

        public void resize(int pix_x, int pix_y)
        {
            h = (int)Math.Round(pix_y * pixel_to_char_ratio_y,0,MidpointRounding.ToNegativeInfinity);
            w = (int)Math.Round(pix_x * pixel_to_char_ratio_x,0,MidpointRounding.ToNegativeInfinity);

            canvas = new char[h, w];
            canvas_display.Clear();

            fill_on_resize();
        }

        private void fill_on_resize()
        {
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    canvas[i, j] = '#';
                }
                canvas_display.Add(new string('#', w));
            }
        }

        private void refresh_row(int row_id)
        {
            char[] chars = new char[w];
            for (int i = 0; i < w; i++)
            {
                chars[i] = canvas[row_id, i];
            }
            canvas_display[row_id] = new string(chars);
        }
    }
}
