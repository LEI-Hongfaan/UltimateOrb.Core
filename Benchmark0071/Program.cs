using System;
using System.Diagnostics;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using UltimateOrb; // Assumed namespace for UltimateOrb.UInt128

namespace UInt128ToHalfBenchmarkApp {

    [InProcess]
    [MemoryDiagnoser]
    public class UInt128ToHalfBenchmark {
        private System.UInt128[] sysInputs;
        private UltimateOrb.UInt128[] ultInputs;

        // Input data: 0, 1, 2, ... 65536 (inclusive), then randomly shuffled.
        [GlobalSetup]
        public void Setup() {
            const int count = 65_537; // 0 to 65,536 inclusive
            int[] data = Enumerable.Range(0, count).ToArray();

            // Fisher–Yates shuffle using a fixed seed for reproducibility.
            Random rng = new Random(42);
            for (int i = data.Length - 1; i > 0; i--) {
                int swapIndex = rng.Next(i + 1);
                int temp = data[i];
                data[i] = data[swapIndex];
                data[swapIndex] = temp;
            }

            // Convert the integers to both 128-bit types.
            sysInputs = data.Select(v => (System.UInt128)v).ToArray();
            ultInputs = data.Select(v => (UltimateOrb.UInt128)v).ToArray();
        }

        // Baseline: conversion using System.UInt128.
        [Benchmark()]
        public int UInt128ToHalf_Dummy() {
            int h = 0;
            for (int i = 0; i < sysInputs.Length; i++) {
                // Assumes an explicit conversion operator exists.
                h += BitConverter.UInt16BitsToHalf(unchecked((UInt16)ultInputs[i].LoInt64Bits)).GetHashCode();
            }
            return h;
        }


        // Baseline: conversion using System.UInt128.
        [Benchmark(Baseline = true)]
        public int SystemUInt128ToHalf() {
            int h = 0;
            for (int i = 0; i < sysInputs.Length; i++) {
                // Assumes an explicit conversion operator exists.
                h += ((Half)sysInputs[i]).GetHashCode();
            }
            return h;
        }

        // Benchmark: conversion using UltimateOrb.UInt128.
        [Benchmark]
        public int UltimateOrbUInt128ToHalf() {
            int h = 0;
            for (int i = 0; i < ultInputs.Length; i++) {
                h += ((Half)ultInputs[i]).GetHashCode();
            }
            return h;
        }
    }

    public class Program {
        public static void Main(string[] args) {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            {
                var a = new UInt128ToHalfBenchmark();
                a.Setup();
                Console.WriteLine($@"AAA: {a.SystemUInt128ToHalf() == a.UltimateOrbUInt128ToHalf()}");
               
            }
            
            BenchmarkRunner.Run<UInt128ToHalfBenchmark>();
        }
    }
}
