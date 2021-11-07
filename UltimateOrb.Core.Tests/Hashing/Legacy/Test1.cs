using Xunit;
using FsCheck.Xunit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Org.BouncyCastle.Crypto.Digests;
using System.Diagnostics;
using UltimateOrb.Core.Tests.Hashing.Legacy;

namespace UltimateOrb.Hashing.Legacy.Tests {

    public static class Test1 {

        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool sdadsa0(byte[] source) {

            var sdfas = MD5Managed.Create();
            var dsafs = new MD5Digest();
            var sda = new byte[16];
            dsafs.BlockUpdate(source, 0, source.Length);
            var d = dsafs.DoFinal(sda, 0);
            Debug.Assert(d == sda.Length);

            try {
                var sdssfas = sdfas.ComputeHash(source);

                var dsa = sda.SequenceEqual(sdssfas);
                if (!dsa) {
                    return dsa;
                }
                return dsa;
            } catch (Exception ex) {

                throw;
            }
        }

        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool sdadsa(byte[] source) {

            var sdfas = MD5Managed.Create();
            var dsafs = new MD5Digest();
            var sda = new byte[16];
            dsafs.BlockUpdate(source, 0, source.Length);
            var d = dsafs.DoFinal(sda, 0);
            Debug.Assert(d == sda.Length);

            try {
                // var sdssfas = sdfas.ComputeHash(source);

                sdfas.TransformFinalBlock(source, 0, source.Length);
                var sdssfas = sdfas.Hash;

                var dsa = sda.SequenceEqual(sdssfas);
                if (!dsa) {
                    return dsa;
                }
                return dsa;
            } catch (Exception ex) {

                throw;
            }
        }


        public static string ToHexString(this byte[] source) {
            var dsa = new StringBuilder(source.Length * 2);
            foreach (var item in source) {
                dsa.Append(item.ToString("x2"));
            }
            return dsa.ToString();
        }


        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool sdadsa2(byte[] source0, byte[][] sourceRest) {
            var source = new[] { source0 }.Concat(sourceRest.Where(x => x is not null && x.Length > 0)).ToArray();
            var refImpl = new MD5Digest();

            var refResults = new List<string>();

            var refResBuffer = new byte[16];
            byte[] whole = Join(source);
            {
                refImpl.BlockUpdate(whole, 0, whole.Length);
                var d = refImpl.DoFinal(refResBuffer, 0);
                Debug.Assert(d == refResBuffer.Length);
                refResults.Add(refResBuffer.ToHexString());
            }
            for (int i = 1; i <= source.Length; i++) {
                var tt = Join(source.Take(i));
                refImpl.BlockUpdate(tt, 0, tt.Length);
                var d = refImpl.DoFinal(refResBuffer, 0);
                Debug.Assert(d == refResBuffer.Length);
                refResults.Add(refResBuffer.ToHexString());
            }
            Debug.Assert(refResults.First() == refResults.Last());


            var results = new List<string>();
            var impl = MD5Managed.Create();
            try {
                {
                    impl.TransformFinalBlock(whole, 0, whole.Length);
                    results.Add(impl.Hash.ToHexString());
                }

                for (int i = 1; i <= source.Length; i++) {
                    var sss = source.Take(i).ToArray();
                    var t = 0L;
                    foreach (var item in sss.Take(i - 1)) {
                        t += impl.TransformBlock(item, 0, item.Length, null, default);
                    }
                    t += impl.TransformFinalBlock(sss.Last(), 0, sss.Last().Length).Length;
                    results.Add(impl.Hash.ToHexString());
                    if (i == source.Length) {
                        Debug.Assert(whole.Length == t);
                    }
                }


                var r = results.SequenceEqual(refResults);
                if (!r) {
                    return r;
                }
                return r;
            } catch (Exception ex) {

                throw;
            }
        }

        private static T[] Join<T>(IEnumerable<T[]> source) {
            var c = source.Sum(x => x.Length);
            var a = new T[c];
            var p = 0;
            foreach (var item in source) {
                item.CopyTo(a, p);
                p += item.Length;
            }
            return a;
        }
    }
}
