using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNATetris.Tools
{
    class ArrayTools
    {
        public static int getMin(int[] array)
        {
            int min = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                if (min > array[i])
                    min = array[i];
            }
            return min;
        }
        public static int getMax(int[] array)
        {
            int max = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                if (max < array[i])
                    max = array[i];
            }
            return max;
        }

        public static bool IsInArray(int value, int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (value == array[i])
                    return true;
            }
            return false;
        }
    }
}
