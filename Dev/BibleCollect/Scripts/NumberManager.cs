using System;

public static class NumberManager
{
    public static string NtoS(long n)
    {
        bool isMinus = false;
        decimal r = n;
        byte c = 0;
        string m = "";
        if (r > -1000&& r < 1000)
            return n+m;

        if (r < 0)
        {
            isMinus = true;
            r = Math.Abs(r);
        }
        while(r>999)
        {
            r= r / 1000;
            c++;
        }

        switch(c)
        {
            case 1:
                m = "A";
                break;
            case 2:
                m = "B";
                break;
            case 3:
                m = "C";
                break;
            case 4:
                m = "D";
                break;
            case 5:
                m = "E";
                break;
            case 6:
                m = "F";
                break;
            default:
                break;
        }

        r = Math.Round(r,1);

        if (isMinus) return String.Format("{0:0.0}",r * -1) + m;
        return String.Format("{0:0.0}", r) + m;
    }
}
