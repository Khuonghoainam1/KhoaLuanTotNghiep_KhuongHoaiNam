using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System;
using UnityEngine;

public static class NumberUtils
{
    public static string ToKMB(this int num)
    {
        if (num > 999999999 || num < -999999999)
        {
            return num.ToString("0,,,.###B", CultureInfo.InvariantCulture);
        }
        else if (num > 999999 || num < -999999)
        {
            return num.ToString("0,,.##M", CultureInfo.InvariantCulture);
        }
        else if (num > 999 || num < -99999)
        {
            return num.ToString("0,.#K", CultureInfo.InvariantCulture);
        }
        else
        {
            return num.ToString(CultureInfo.InvariantCulture);
        }
    }

    public static int Rounding(this int num)
    {
        if (num > 999999 || num < -999999)
        {
            double numDouble = (double)num / 1000000;
            return (int)(Math.Ceiling(numDouble) * 1000000);
        }
        else if(num > 99999 || num < -99999)
        {
            double numDouble = (double)num / 100000;
            return (int)(Math.Ceiling(numDouble) * 100000);
        }
        else if(num > 9999 || num < -9999)
        {
            double numDouble = (double)num / 10000;
            return (int)(Math.Ceiling(numDouble) * 10000);
        } 
        else 
        {
            return num; 
        }
    }
}
