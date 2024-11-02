using System;
using System.Runtime.InteropServices.JavaScript;

public partial class Program {

    public static int Main(string[] args) {
        Console.WriteLine("Hello, Browser!");
        return 0;
    }
}

public partial class MyClass {
    [JSExport]
    internal static string Greeting() {
        var text = $"Hello, World! Greetings from {GetHRef()}";
        Console.WriteLine(text);
        Console.WriteLine((UltimateOrb.UInt128)System.UInt128.Parse("123") * (UltimateOrb.UInt128)System.UInt128.Parse("100000010000001"));

        return text;
    }

    [JSImport("window.location.href", "main.js")]
    internal static partial string GetHRef();
}
