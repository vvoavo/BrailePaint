using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrailePaint.Classes
{

    public class ConsoleGraphMaker
    {
        //⠠ ⠐ ⠈ ⠄ ⠂ ⠁

        private const int _consoleCharacters = 120;

        List<Point_d> points;
        List<Point_d> points_normalized;
        char?[,] graph;

        private bool graph_created;
        private bool extremes_found;

        private int? rows;

        private int dots_width;
        private int? dots_height;
        private double min_x;
        private double max_x;
        private double min_y;
        private double max_y;

        private double min_x_n;
        private double max_x_n;
        private double min_y_n;
        private double max_y_n;

        private void find_extremes(bool force = false)
        {
            if (extremes_found)
            {
                if (force)
                { }
                else
                { return; }
            }

            if (points == null)
                throw new NullReferenceException("'List<Point_d> points' was null, but should be defined before execution of 'find_extremes()'");
            if (points == null)
                throw new NullReferenceException("'List<Point_d> points_normalized' was null, but should be defined before execution of 'find_extremes()'");

            min_x = points.Min(x => x.x);
            min_y = points.Min(x => x.y);
            max_x = points.Max(x => x.x);
            max_y = points.Max(x => x.y);

            min_x_n = points_normalized.Min(x => x.x);
            min_y_n = points_normalized.Min(x => x.y);
            max_x_n = points_normalized.Max(x => x.x);
            max_y_n = points_normalized.Max(x => x.y);
            extremes_found = true;
        }

        private void normalize_points()
        {
            if (points == null)
                throw new NullReferenceException("'List<Point_d> points' was null, but should be defined before execution of 'find_extremes()'");

            points_normalized = new List<Point_d>();

            min_y = points.Min(x => x.y);
            min_x = points.Min(x => x.x);

            bool normalize_y = min_y < 0;
            bool normalize_x = min_x < 0;

            for (int i = 0; i < points.Count; i++)
            {
                Point_d p = new Point_d(points[i]);


                if (normalize_y)
                    p.y += Math.Abs(min_y);
                if (normalize_x)
                    p.x += Math.Abs(min_x);

                if (p.x == double.NegativeInfinity || p.x == double.PositiveInfinity || p.y == double.NegativeInfinity || p.y == double.PositiveInfinity)
                {
                    continue; // skip uncountable extremes;
                }

                points_normalized.Add(p);
            }

            find_extremes(true);

            double scale = _consoleCharacters / (max_x_n - min_x_n);
            dots_height = (int)Math.Round((max_y_n - min_y_n) * scale, 0);

            for (int i = 0; i < points_normalized.Count; i++)
            {
                points_normalized[i].scale(scale);
            }

            find_extremes(true);
        }

        private char dots_overlap(char dot_1, char dot_2)
        {
            return Braile_2by4.overlap_from_char(dot_1, dot_2);
        }

        public ConsoleGraphMaker(List<Point_d> points)
        {
            graph_created = false;
            extremes_found = false;
            dots_width = _consoleCharacters * 2; //2 points per ascii character for braille
            dots_height = null;

            this.points = points;
            normalize_points();
            find_extremes();
        }

        private bool put_dot_on_graph(int dot_x, int dot_y)
        {
            if (rows == null)
                throw new NullReferenceException("'private int rows' value is not set yet");
            int dot_symbol_id_x = dot_x % 2;
            int dot_symbol_id_y = dot_y % 4 == 0 ? 4 : dot_y % 4;


            int row = (int)Math.Round(dot_y / 4.0, 0, MidpointRounding.ToPositiveInfinity);
            int col = (int)Math.Round(dot_x / 2.0, 0, MidpointRounding.ToPositiveInfinity);

            if (row >= rows || col >= _consoleCharacters)
            {
                return false; // possible rework of logic
            }

            string binary_row_str = dot_symbol_id_x == 1 ? "10" : "01";
            string binary_str = "";
            for (int i = 3; i >= 0; i--)
            {
                if (dot_symbol_id_y == i + 1)
                    binary_str += binary_row_str;
                else
                    binary_str += "00";
            }

            char dot_char = Braile_2by4.braile_from_binary_string(binary_str);

            if (graph[row, col] != null)
            {
                dot_char = dots_overlap(dot_char, (char)graph[row, col]);
            }
            graph[row, col] = dot_char;

            return true;
        }

        private void draw_line(int a_x, int a_y, int b_x, int b_y)
        {
            int d_x = Math.Abs(b_x - a_x);
            int d_y = Math.Abs(b_y - a_y);

            int dir_x = Math.Sign(b_x - a_x);
            int dir_y = Math.Sign(b_y - a_y);

            double ratio = (double)d_x / (double)d_y;

            int x_shift = 0;
            int y_shift = 0;
            if (ratio > 1)
            {
                int counter = 0;
                while (d_x >= Math.Abs(x_shift) && d_y >= Math.Abs(y_shift))
                {
                    put_dot_on_graph(a_x + x_shift, a_y + y_shift);

                    x_shift += dir_x;
                    counter++;

                    if (counter >= ratio)
                    {
                        y_shift += dir_y;
                        counter = 0;
                        int tmp_d_x = Math.Abs(b_x - (a_x + x_shift));
                        int tmp_d_y = Math.Abs(b_y - (a_y + y_shift));
                        ratio = (double)tmp_d_x / (double)tmp_d_y;
                    }
                }
            }
            else if (ratio < 1)
            {
                int counter = 0;
                while (d_x >= Math.Abs(x_shift) && d_y >= Math.Abs(y_shift))
                {
                    put_dot_on_graph(a_x + x_shift, a_y + y_shift);

                    y_shift += dir_y;
                    counter++;

                    if (counter >= 1.0 / ratio)
                    {
                        x_shift += dir_x;
                        counter = 0;
                        int tmp_d_x = Math.Abs(b_x - (a_x + x_shift));
                        int tmp_d_y = Math.Abs(b_y - (a_y + y_shift));
                        ratio = (double)tmp_d_x / (double)tmp_d_y;
                    }
                }
            }
            else // ratio == 1
            {
                while (d_x >= Math.Abs(x_shift) && d_y >= Math.Abs(y_shift))
                {
                    put_dot_on_graph(a_x + x_shift, a_y + y_shift);

                    y_shift += dir_y;
                    x_shift += dir_x;
                }
            }
        }

        private void draw_line_midpoint(int a_x, int a_y, int b_x, int b_y)
        {
            int d_x = Math.Abs(b_x - a_x);
            int d_y = Math.Abs(b_y - a_y);

            int dots_count = (int)Math.Sqrt(d_y * d_y + d_x * d_x);

            draw_line_midpoint_recursive_func(a_x, a_y, b_x, b_y, dots_count);
        }
        private void draw_line_midpoint_recursive_func(int a_x, int a_y, int b_x, int b_y, int dots_count)
        {
            int mid_x = (a_x + b_x) / 2;
            int mid_y = (a_y + b_y) / 2;
            put_dot_on_graph(mid_x, mid_y);

            if (dots_count < 2)
                return;

            draw_line_midpoint_recursive_func(a_x, a_y, mid_x, mid_y, (dots_count) / 2);
            draw_line_midpoint_recursive_func(mid_x, mid_y, b_x, b_y, (dots_count) / 2);
        }
        private void create_graph()
        {
            rows = (int)Math.Round((decimal)(dots_height / 3.0), 0, MidpointRounding.ToPositiveInfinity);

            rows += 2;
            graph = new char?[(int)rows, _consoleCharacters];

            int? prev_dot_y = null, prev_dot_x = null;

            for (int i = 0; i < points_normalized.Count; i++)
            {
                int dot_x = (int)Math.Round(points_normalized[i].x, 0);
                int dot_y = (int)Math.Round(points_normalized[i].y, 0);

                put_dot_on_graph(dot_x, dot_y);

                //line
                if (prev_dot_y != null && prev_dot_x != null)
                {
                    draw_line_midpoint(dot_x, dot_y, (int)prev_dot_x, (int)prev_dot_y);
                }

                prev_dot_x = dot_x;
                prev_dot_y = dot_y;
            }

            for (int i = 0; i < graph.Length / _consoleCharacters; i++)
            {
                for (int j = 0; j < _consoleCharacters; j++)
                {
                    if (graph[i, j] == null)
                    {
                        graph[i, j] = ' ';
                    }
                }
            }

            graph_created = true;
        }

        public void print_graph()
        {
            if (!graph_created)
                create_graph();

            for (int i = (graph.Length / _consoleCharacters) - 1; i >= 0; i--)
            {
                for (int j = 0; j < _consoleCharacters; j++)
                {
                    Console.Write(graph[i, j]);
                }
                Console.WriteLine();
            }
        }
    }

}
