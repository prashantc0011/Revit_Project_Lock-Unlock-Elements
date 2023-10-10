using Autodesk.Revit.DB.Structure.StructuralSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class IfExample
{
    public static void Main(string[] args)
        {

        Console.WriteLine("Enter a number to check Grade");
        int num = Convert.ToInt32(Console.ReadLine());

        if (num < 0 || num > 100)
        {
            Console.WriteLine("Wrong Number");
        }
        else if (num >= 0 && num < 50)
        {
            Console.WriteLine("Fail");
        }
        else if (num >= 50 && num < 60)
        {
            Console.WriteLine("Grade D");
        }
        else if (num >= 60 && num < 70)
        {
            Console.WriteLine("Grade C");
        }
        else if (num >= 70 && num < 80)
        {
            Console.WriteLine("Grade B");
        }
        else if (num >= 80 && num < 90)
        {
            Console.WriteLine("Grade A");
        }
        else if (num >= 90 && num <= 100)
        {
            Console.WriteLine("Grade A+");
        }

        int x = 6, y = 7, z = 8;
        Console.WriteLine(x+y+z);

        int a, b, c;
        a = b = c= 3;
        Console.WriteLine(a+b+c);

    }
}
