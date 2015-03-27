using System;
using ExampleProject.Company.ExampleProject.API;

namespace HelloWorld
{
    class ChangeAPI
    {
        public void Method1(string data1)
        {
            A a = new A();
            a.aMethod();
        }

        public void Method2(string data2)
        {
            A a = new A();
            a.aMethod();
            int p = 0;
            Console.WriteLine(data2);
        }

        static void Method3(string data3)
        {
            C c = new C();
            c.cMethod();
            int p = 0;
            int j = 2;
            Console.WriteLine(data3);
        }

        static void Method4(string data4)
        {
            A a = new A();
            a.aMethod();
            int p = 0;
            int j = 2;
            Console.WriteLine(data4);
        }

        static void Method5(string data5)
        {
            D d = new D();
            d.dMethod();
            int p = 0;
            int j = 2;
            Console.WriteLine(data5);
        }

        static void Method6(string data6)
        {
            int j = 2;
            Console.WriteLine("Hello, World!");
            int i = 0;
        }

        static void Method7(string data7)
        {
            int j = 2;
            Console.WriteLine(data7);
            int i = 0;
        }

    }
}