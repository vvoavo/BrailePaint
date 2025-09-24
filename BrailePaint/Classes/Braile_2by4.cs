using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BrailePaint.Classes
{
    public static class Braile_2by4
    {
        const string unicode_base = @"\u28";


        /// <summary>
        /// accepts string of 1 and 0 that represent dots_filled in order by rows 
        /// ex: 11000011 will have corners filled and middle empty
        /// 
        /// program order is different from order by rows
        /// </summary>
        /// <returns>
        /// unicode braile 2x4 character
        /// </returns>
        public static char braile_from_binary_string(string binary, bool is_string_in_program_order = false)
        {
            if (binary.Count() > 8)
            {
                throw new Exception("length of binary string should be 8");
            }
            else if (binary.Count() < 8)
            {
                binary = binary.Insert(0, new string('0', 8 - binary.Count()));
            }

            string binary_program_str;
            if (is_string_in_program_order)
                binary_program_str = binary;
            else
                binary_program_str = from_normal_to_program_order(binary);

            byte tmp = Convert.ToByte(binary_program_str, 2);
            string unicode_end = tmp.ToString("X2");
            string res = Regex.Unescape(unicode_base + unicode_end);

            return res.ToCharArray()[0];
        }

        /// <summary>
        /// returns binary string from braile symbol, ordered for converting to hex and using with unicode
        /// </summary>
        public static string binary_string_from_braile(char braile)
        {
            int unicode_val = (int)braile;
            string hex_str = unicode_val.ToString("X4");
            hex_str = hex_str.Substring(2, 2);
            string binary = Convert.ToString(Convert.ToInt32(hex_str, 16), 2);
            return binary;
        }

        public static char overlap_from_char(char char1, char char2)
        {
            string char_1_binary = binary_string_from_braile(char1);
            string char_2_binary = binary_string_from_braile(char2);

            string res_binary = overlap_from_binary(char_1_binary, char_2_binary);

            return braile_from_binary_string(res_binary, true);
        }

        public static string overlap_from_binary(string b1, string b2)
        {
            if (b1.Count() < 8)
            {
                b1 = b1.Insert(0, new string('0', 8 - b1.Count()));
            }
            if (b2.Count() < 8)
            {
                b2 = b2.Insert(0, new string('0', 8 - b2.Count()));
            }

            char[] char_1_arr = b1.ToCharArray();
            char[] char_2_arr = b2.ToCharArray();

            char[] res_array = new char[8];
            for (int i = 0; i < 8; i++)
            {
                if (char_1_arr[i] == '1' || char_2_arr[i] == '1')
                    res_array[i] = '1';
                else
                    res_array[i] = '0';
            }

            return new string(res_array);
        }

        private static string from_normal_to_program_order(string binary_str)
        {
            char[] result = new char[8];
            char[] binary = binary_str.ToCharArray();
            result[0] = binary[7];
            result[1] = binary[6];
            result[2] = binary[5];
            result[3] = binary[3];
            result[4] = binary[1];
            result[5] = binary[4];
            result[6] = binary[2];
            result[7] = binary[0];

            string res_str = new string(result);

            return res_str;
        }
    }
}
