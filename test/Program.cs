using System;

namespace test
{
    class A
    {
        public A(string a)
        {
            Console.WriteLine("new A: " + a);
        }
    }

    class B
    {
        public A a { get; } = new A("a");
        public A b { get => new A("b"); }
        public A c => new A("c");

        public B()
        {
            Console.WriteLine("new B");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            B b = new B();
            Console.WriteLine("Access tests");
            A a = b.a;
            a = b.b;
            a = b.c;

            Console.WriteLine("Access tests");
            a = b.a;
            a = b.b;
            a = b.c;
        }
    }
}
