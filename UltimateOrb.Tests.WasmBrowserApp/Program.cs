using System;
using System.Runtime.InteropServices.JavaScript;

Console.WriteLine("Hello, Browser!");

public partial class MyClass {
    [JSExport]
    internal static string Greeting() {
        var text = $"Hello, World! Greetings from {GetHRef()}";
        Console.WriteLine(text);
        Console.WriteLine((UltimateOrb.UInt128)UInt128.Parse("123") * (UltimateOrb.UInt128)UInt128.Parse("100000010000001"));

        return text;
    }

    [JSImport("window.location.href", "main.js")]
    internal static partial string GetHRef();
}
