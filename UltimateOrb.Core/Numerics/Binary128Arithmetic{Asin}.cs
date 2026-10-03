using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Runtime.CompilerServices;
using Misc = UltimateOrb.Miscellaneous;
using Unsafe = System.Runtime.CompilerServices.Unsafe;

namespace UltimateOrb.Numerics {
#if NET8_0_OR_GREATER
    using Int128 = System.Int128;
    using UInt128 = System.UInt128;
#endif

    public static partial class Binary128Arithmetic {

        // ---- approximate high squares ----

        // ---- full 128x128 square -> 256 (4 words) ----
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void BigSquareUnsigned(out InlineArray4<UInt64> o, in InlineArray2<UInt64> x) {
            unchecked {
                UInt128 p10 = Math.BigMul(x[1], x[0]);
                UInt64 p10x = (UInt64)(p10 >> 127); p10 <<= 1;
                UInt128 p00 = Math.BigMul(x[0], x[0]);
                UInt128 p11 = Math.BigMul(x[1], x[1]);
                InlineArray4<UInt64> o_ = default;
                o_[0] = (UInt64)p00;
                o_[1] = AddWithCarry((UInt64)(p00 >> 64), (UInt64)p10, 0, out var c);
                o_[2] = AddWithCarry((UInt64)(p10 >> 64), (UInt64)p11, c, out c);
                o_[3] = AddWithCarry((UInt64)(p11 >> 64), p10x, c, out _);
                o = o_;
            }
        }
    }

    partial class Binary128Arithmetic {

        // ---------- sth[71] : Int16 ----------
        static ReadOnlySpan<Int16> AsinSth => [
            11476, 9429, 8096, 7384, 6672, 6053, 5699, 5346, 4995, 4645, 4297,
            4023, 3851, 3680, 3510, 3342, 3174, 3009, 2844, 2681, 2520, 2361,
            2203, 2048, 1894, 1742, 1592, 1445, 1299, 1157, 1016, 878, 742, 609,
            479, 351, 226, 104, -31, -263, -490, -711, -925, -1133, -1335,
            -1531, -1719, -1901, -2105, -2442, -2765, -3074, -3369, -3650,
            -3917, -4240, -4715, -5159, -5574, -5959, -6484, -7134, -7722,
            -8306, -9238, -10046, -11220, -12394, -14139, -16488, -20584,
        ];

        // ---------- pth[71] : UInt32 ----------
        static ReadOnlySpan<UInt32> AsinPth => [
            0, 0xb2c0, 0x16560, 0x21800, 0x2ca00, 0x37c00, 0x42d80, 0x4de80,
            0x58f00, 0x63e80, 0x6ed80, 0x79b80, 0x84900, 0x8f500, 0x9a000,
            0xa4a00, 0xaf200, 0xb9a00, 0xc3f00, 0xce400, 0xd8700, 0xe2800,
            0xec700, 0xf6500, 0x100000, 0x109a00, 0x113200, 0x11c800, 0x125b00,
            0x12ed00, 0x137b00, 0x140800, 0x149200, 0x151a00, 0x159f00,
            0x162100, 0x16a100, 0x171e00, 0x179800, 0x180f80, 0x188380,
            0x18f500, 0x196380, 0x19ce80, 0x1a3680, 0x1a9b80, 0x1afd80,
            0x1b5b80, 0x1bb680, 0x1c0e40, 0x1c6280, 0x1cb340, 0x1d0080,
            0x1d4a40, 0x1d9080, 0x1dd340, 0x1e1200, 0x1e4d60, 0x1e84e0,
            0x1eb8c0, 0x1ee8e0, 0x1f1540, 0x1f3de0, 0x1f62a0, 0x1f8390,
            0x1fa0b0, 0x1fb9f0, 0x1fcf50, 0x1fe0d4, 0x1fee76, 0x1ff834,
            0x1ffe0d,
        ];

        // ---------- ind[256] : Byte ----------
        // Contents ported verbatim from the C `static const char ind[]`.
        static ReadOnlySpan<Byte> AsinInd => [
            70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70,
            70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 69, 69,
            69, 69, 69, 69, 69, 69, 69, 69, 69, 69, 69, 69, 69, 69, 69, 69, 68,
            68, 68, 68, 68, 68, 68, 68, 68, 68, 68, 68, 68, 68, 67, 67, 67, 67,
            67, 67, 67, 67, 67, 66, 66, 66, 66, 66, 66, 66, 66, 66, 65, 65, 65,
            65, 65, 65, 64, 64, 64, 64, 64, 64, 64, 64, 63, 63, 63, 63, 62, 62,
            62, 62, 62, 61, 61, 61, 61, 61, 60, 60, 60, 60, 59, 59, 59, 58, 58,
            58, 57, 57, 57, 57, 56, 56, 56, 55, 55, 55, 54, 54, 53, 53, 52, 52,
            51, 51, 51, 50, 50, 49, 49, 49, 48, 48, 47, 46, 46, 45, 44, 44, 43,
            42, 42, 41, 41, 40, 39, 39, 38, 37, 36, 35, 34, 33, 32, 31, 30, 30,
            29, 28, 27, 26, 25, 24, 24, 23, 22, 21, 20, 19, 19, 18, 17, 16, 16,
            15, 14, 13, 13, 12, 11, 11, 10, 10, 10, 9, 9, 9, 8, 8, 7, 7, 7, 6,
            6, 6, 5, 5, 5, 5, 5, 4, 4, 4, 4, 4, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            0, 0, 0, 0, 0, 0,
        ];

        // ---------- cth[72][5] : UInt64 (flat) ----------
        // Contents ported verbatim from the C `static const u5x64 cth[]`.
        static ReadOnlySpan<UInt64> AsinCth_flat => [
            0, 0, 0, 0, 0,
            0x64d23d92f9ea2771, 0x6d047db3a6e206a7, 0x2953c428c9d7bc56, 0xfb0f1838d5a3a8da, 0xfff06594451d279d,
            0xe3080aa3bd169d0c, 0x4054eae9456537db, 0x9e5833eae773bc87, 0x6f49ccb466446b89, 0xffc19bc9271a05b3,
            0x00912c8144374c85, 0x309a44ddae7556ce, 0xc319285201c58b56, 0x3a246e54beb8dac9, 0xff73917b77aa5c47,
            0x6a7360619e7edb23, 0x0b11b912de529986, 0x5a6995a8cc7d5eac, 0x552a6d7261223cc4, 0xff069a0439cbe651,
            0x238522ea64680061, 0xf079215ebcd5ca36, 0x290de5267799873e, 0xcab4f035a6bfa060, 0xfe7a55701a8697b0,
            0x7a92124309b60ef2, 0x4b9dd14818169efc, 0x040ba2edca526f4e, 0xac4772d0d4932b9b, 0xfdcf16b94b0b442e,
            0x30ff410c35a4b64d, 0xd1d820652a9bc4c6, 0x6d055dc0a4c09600, 0xb4aab1b03d022116, 0xfd04e2530bcbd671,
            0x03aedba8702f81c3, 0x96da5af0d3ea2f8b, 0xca571fb2cbdee5b9, 0x17c0bd0e66183953, 0xfc1bb125286c39d8,
            0xfb91abaebc934604, 0xce94d1434f07a0a1, 0x068284240ceeae19, 0x5e9a55955f4b097f, 0xfb143c17bdcc996d,
            0xab794690d17636d8, 0xc8e308f9bd76c0e2, 0x15fddd7233c65eed, 0xdb301a162cf8115d, 0xf9edc73d6c923555,
            0x593fea054c7f450e, 0x6b0aa37cffc886a5, 0xd8e480c9ebb134e1, 0x287dce6a9f8ea2ac, 0xf8a922ae090b0799,
            0x251fef9c8bb46028, 0x493c5f39729df2a1, 0xc745b46aca55d0e8, 0x2062f831fa08f4af, 0xf7454c2c7d12b97a,
            0xff1ffe460bd9c836, 0xb16e3e5300ab04b0, 0xcc500109643b1f25, 0x1bf03e7ae9dd3a2a, 0xf5c455407155fe59,
            0x4606e3f3b69b5b9f, 0xaf57d7aca36b7036, 0x23dc96105c0b9d9b, 0x0e73cd55dd020b9f, 0xf4253c1c1a1ae532,
            0x3e9033c3d73b8ba0, 0x36a1f12dab4107b3, 0xe2bf1fc80065cf4b, 0x067308deced752e6, 0xf267ed62f8b7c795,
            0xdc78f0c65249d290, 0x3da244d00c619d0d, 0xd5109661f43e9ecd, 0x70a15831a61bca6b, 0xf08f329d05a331b0,
            0x2e799ef643386021, 0xf50e2c1a3600d3ad, 0x828f074191858249, 0x4f6a116da6359a61, 0xee953e620e55df7b,
            0xf59cf516cfc8aef3, 0x3994459558477033, 0xcfbcad100d166a9e, 0x6632028b37ff16ad, 0xec832dd8f9584e23,
            0x8993bed4bc91f71b, 0x2d156655d9d43f98, 0x7386f1c67cf345e5, 0xd03960687c6a9efe, 0xea4f644f48136890,
            0x6c1be9d31d8598fe, 0x0bbc7599cc98b839, 0x1b0cb81a981ed38d, 0x74401bc33d3f4f33, 0xe800632c0e1d2f2c,
            0xd77e840a65911d5a, 0x0409ada21048c2b1, 0x54c0567f7377993b, 0xb61bea5c37779619, 0xe59668e019f1e288,
            0xdf264c217da2460f, 0x4310bc9fc7837821, 0xa5cb92ab27280ab6, 0x03513aa71030fbb2, 0xe311a97671f44f1c,
            0x8ee20e415370de4e, 0xb85f2ea14495bf6f, 0xa12912f50e813792, 0x0f451e45046e7646, 0xe06dea9a80d1fdf6,
            0x2485e7ecaf78aedf, 0x639053243722d371, 0x92ec1a6629ed23cc, 0x92ba16b83c5c1dc4, 0xddb3d742c265539d,
            0x7d45353940ee462b, 0x81cfccf2da1a22d6, 0x4db6039c8f8f6015, 0x83992b6bb6c89121, 0xdada7cc9882f1832,
            0xb0de57772eac59f6, 0x855009022ef97eb9, 0x8549b723bab4be2d, 0x27e229b54b803697, 0xd7e6403e36d2f1f7,
            0xe6c36c5ad24b30c1, 0x5258a95fc94e2fb8, 0x63aea18a41e1bf81, 0x2a16383f5c4d5062, 0xd4d7154f370bbe07,
            0x4f980d926321df45, 0xad92bc366eaf4837, 0x360b41691ba7a5f0, 0x010bd7d48ab25527, 0xd1b27b4ae9008ef2,
            0xb53f05eae4761ab3, 0x5636614d38c536c1, 0x9cfea8981156d5dd, 0x0bb008a15e850cec, 0xce6d5666b787ca77,
            0x80638f93f4a2c269, 0x1dcdbcdbbcf14233, 0xae4218b2749a67be, 0x69d2608c3b73e12f, 0xcb190ae896231bce,
            0xe516cefc6347a5e9, 0x9684ba1ed4646c7a, 0x5f831bdef11f9bbc, 0xc6158124a4ef775f, 0xc7a3b782716d38c8,
            0x8f005279faa6880d, 0x890f8c878e5f7df9, 0x3485668a4e6e9637, 0xcf01d7d5779f0507, 0xc419953003b5c74b,
            0x744fe29890f407f9, 0x6f46175fab644dfc, 0xd9330605ec9b4e0a, 0xa322920e8bd7accf, 0xc07416e77d712462,
            0xab325fd93ce9b6b8, 0x57c460f1a312c3dd, 0x0fcf1122ee32b17c, 0x892c26b0bcfacc8d, 0xbcba10b0d8770ed2,
            0xcef1e69c4eadb8b3, 0x7f131158279ff4d7, 0x4e6ba5183b584705, 0x8fcd4c453517e8ba, 0xb8ebe30ed9fac186,
            0x3745f18539bf8772, 0x2cee4ab653475e6e, 0xc28307bea98bb0e7, 0x5cbc46ee3d37b72d, 0xb501e65acbad5ad8,
            0xf4c3a3a64ddafd51, 0x5e65313f078e13ab, 0xe6ba3e11cdb88afb, 0xf74e3a8ab016619f, 0xb103b40770404b83,
            0x539c48de71a3cc8e, 0x7f208cb9376741f7, 0xa22a23cb0d6b1468, 0x3fd4750afb3804bb, 0xacf1860da3cfd826,
            0xbc5eb21dc1786f2d, 0xcaf9195f5c2505ba, 0xf402f48b30c4df2f, 0x4987496d863e1e7c, 0xa8c6fae0bbd09645,
            0x61f8e4beffcf379f, 0x758e5ddd22563b4f, 0x80d42d484f061220, 0xfbc0967b24f94966, 0xa48d1e8d86992cdc,
            0xcdf0fca2bcbaff85, 0x201f9ee54d8615fa, 0x7a56fc977f0d0e4c, 0x754bb974869014d6, 0xa03aa9d87a93f00d,
            0xcae22303aeaf6595, 0xe7b53a3891af1ae5, 0x6de54e5c6a03779d, 0x6cf567b89889fef8, 0x9bd425459273d285,
            0x9831e7542d85d7ea, 0x611013ca24685535, 0xa3724e71c24123d7, 0xb849ff7aea680179, 0x975eea2b65a9847e,
            0x101a86b895c7d937, 0xe88c06711a5ab702, 0xfd217ba2d78129ec, 0x20b672429fb3d1dd, 0x92d5d4e3fc1f9cbd,
            0x4d91c0b5749d4914, 0x8ed80817147623b6, 0xb90bd29f73f58752, 0x5ff44c46ffccf7f3, 0x8e38a46bdb3b3d12,
            0x2efea8ce593572c2, 0xf37f1d4704fb5c49, 0xf4c2e89353ada913, 0xedbf03f73c1d3162, 0x8986f9b6c7132cff,
            0x35b06cbf85f84fbb, 0xbf788966cdcd56b0, 0x91e498b6c64670dc, 0xf6e11dfe5f762e4d, 0x84cd81e775b4c098,
            0x539ba47e0b57d2de, 0x974b441dfa650168, 0xa477f58bc7c8dc47, 0x29dd7114d81c96bf, 0x7fffb96fec8ce447,
            0x3d5bcfe7068c0ff1, 0x5016bbcd4ef1b8e7, 0x6f1857955007a55c, 0x98ea27d884420256, 0x7b208eaab0b94056,
            0x846ef313e2e0bf68, 0xabfac2142052bc6c, 0x8496c1983f214586, 0xe132acebbd5846b8, 0x7633823fd4eeef51,
            0x03681a2fcb57c1e9, 0x999fd17f45cc07f3, 0x411f4be11fd95eb2, 0xa206ce1148e4933c, 0x7138b866dbf2205c,
            0x701d2b27adfe0fb8, 0x239f4fe8463aee07, 0xe0b2bb9935920222, 0x5d94ac1eeb7461f9, 0x6c3040ff7ae4f3d5,
            0x064f259d5d960d10, 0xb194cf34ff2bf070, 0x5f46977eac6f5a5c, 0x4367583037f40763, 0x671a12bd2092f0e6,
            0x739008a332539b2b, 0x34d36b62e94d21ec, 0xf32753bd933f314c, 0x9a57aae41393eb49, 0x61f604a26e818217,
            0x4cda609f93951dc0, 0xe4f6c5ba31b5c6ed, 0x4fa0c524c392182a, 0x37df71c28f1de586, 0x5cc3c513b7fdb337,
            0x9a01a8613fd798eb, 0x266e72c3de2e292e, 0x59fec1c23334cf3a, 0x2e578575cc9228df, 0x578dcbb6628e3c6f,
            0x1f1a07c6d6b6adcd, 0x1eb9dc19b8598724, 0xd9ab93ace24809dc, 0x05dab10687beb878, 0x5246f2e8fd06b80b,
            0xb8a6a56a834058eb, 0xe60a200e3eb8678a, 0xe50f6f2cd1d3aace, 0xedb16a84844fc0a7, 0x4cfa66f577d9eaf3,
            0x69aab942d6b77874, 0x9d6df99ad27e5270, 0x80152267d988b733, 0xdbe4096c369e089e, 0x47a24816dd512c95,
            0x0550f19403d41e0a, 0xbb071ead370af5ff, 0xb9b5812340f07744, 0x64f8889a905e2862, 0x4241a5c4a8543894,
            0xacd334aee8e9e8a3, 0x92d46ccf664aa8e9, 0x9d79c6111152cb8a, 0x4b40d1af34812df0, 0x3cd8779db76341cd,
            0x0403e507d395ba75, 0x4c8823c7ba7c9380, 0x0a03c989401a778b, 0x14849e09356ec1dc, 0x37667d4c98bfbccb,
            0x657bb80e7a20116a, 0xa79b3d94f1414a71, 0x86ab887459d6023c, 0x602a110d5bf3bfc0, 0x31f02728a1820184,
            0x531dfb48ce58358d, 0xe827b8eac551e9ee, 0x1b75725ed755aa4a, 0xc588e51fb5f4fd50, 0x2c736b07088a07ba,
            0x90c4aa84e4598669, 0x7dd80f90a7af46ad, 0xa23d2ad47df691ab, 0x4e35d5f602d9b24d, 0x26efff9c8eff17d3,
            0x06b8ea90e4449667, 0x3b72f4405f272c6f, 0xe689513157c5339a, 0xa73888a873793997, 0x2168e05fb8858700,
            0xc343b6652a5c735a, 0x18fae9a2c9f7cf27, 0x7e0c3ca1daac2e97, 0xbb8233abbcf0c73a, 0x1bde7b65ebd95dd2,
            0x3097e3126b116bd9, 0x35782cb810ce1724, 0x5ca4cd631e80444d, 0xd2eff3545c8a56c1, 0x164fbb9bd36b968c,
            0xe12e48b0cf8a0b8e, 0x402a65318c77466e, 0xfffbc69461af1af2, 0x0d511d6a919beca2, 0x10be2e7affde1a24,
            0xed8e7549e2530cad, 0x419a20b85f378012, 0x4d48fe6abd0a9a0e, 0x40b937dae10995f7, 0x0b2a9f7bd1a4e3f7,
            0x0ccf7c9e0c0fe38a, 0xf496f2cb6c0dc6ca, 0x5c67a95d27203004, 0x87e16185128e709b, 0x059591109ebd190a,
        ];

        static ReadOnlySpan<InlineArray5<UInt64>> AsinCth => MemoryMarshal.CreateReadOnlySpan(
            ref System.Runtime.CompilerServices.Unsafe.As<UInt64, InlineArray5<UInt64>>(
                ref MemoryMarshal.GetReference(AsinCth_flat)), 72);

        // ---------- phi0[72][5] : UInt64 (flat) ----------
        // Contents ported verbatim from the C `static const u5x64 phi0[]`.
        static ReadOnlySpan<UInt64> AsinPhi0_flat => [
            0, 0, 0, 0, 0,
            0x31dc03b852fde26e, 0x067073caa9724261, 0x146b2cf786e935f7, 0x13001bf1bb854bc4, 0x2cb0e871e4d3726,
            0x9fd8e87590496f73, 0xcdd8edd69f613be7, 0xa4cbbf16f87e6782, 0xb7e6ab0fd3c59b0e, 0x595f42cdb812489,
            0xb3d4031693534f58, 0x5d01a5faaed1d0d3, 0xff3200c1c4f94322, 0x59f6668f9b56acc6, 0x861885ff6c0a35d,
            0xb4fab3fc67f7a8b9, 0xf5865c9c96d2fa32, 0x29425b02341193a3, 0x48b0673d280eb1c2, 0xb2ba0dd41444021,
            0x790ddfc5cbe9082f, 0x1c7dc75cedb0a275, 0xc8153f82ea41acef, 0x6b9567f1aad3c76a, 0xdf716a41bbb8b7d,
            0x6ed0830e8be51bb2, 0xf0e929895ecc82ed, 0x7de84cfcc54fe6f6, 0x76bc77009fd70571, 0x10c23f2fd8e3e9a0,
            0x2ad1c043c574f910, 0x713ad3d086ff14d2, 0xba270541aa876034, 0x92d9e59dbf55c60d, 0x138d717ba27eeabc,
            0xb303fd12510f210a, 0x94a653871859e237, 0x0d1502ee17eb1bd7, 0x3739065aec5c06ab, 0x1659054ae2b44a81,
            0x006d05b30daba4c3, 0x6908be7ddfd7775b, 0xc00a83fef85d99d7, 0x795fa4f368b70f06, 0x192349840a184357,
            0xa914f322a5010d57, 0x49222451bf9432d3, 0x50da7e2b81928f5c, 0x221d215e79a25603, 0x1bee9de650865623,
            0x929e0b76f0a42573, 0xda9ba7fc9c3431f1, 0x9174fa977e1baaad, 0xf758a3df6d9553dd, 0x1eb94f058a92e80f,
            0xcee9d9a2db7de49b, 0xb88aa461c7c4f24c, 0x448f34c5b2254778, 0xcf80327e8e111bfe, 0x2185c42fed9c65a9,
            0x0cbb8da2a502b8ea, 0xe094e1731ba94e46, 0x97b764bc45ebe177, 0xbe926f0ad70b40fd, 0x245032e12e6a1d6d,
            0xd3ec0402365050e0, 0xd057a4b66e7da35b, 0x149b8cc2311b2017, 0xec8890b158863303, 0x271b02290466d237,
            0x1fd4828f5c9b8ca8, 0x319e956b7fde7cc9, 0xc94382ac5f951b31, 0xe06d1bebf0914a3b, 0x29e68bf629a0da11,
            0x71ae5a9cce6bc46f, 0xf45ac44e54c7356e, 0x4c2591b80b5d94ac, 0x7355bed3fa0a825f, 0x2caeeae98e6df044,
            0x1614e2e195d53ab6, 0x4f839ef8340df429, 0x040d34ee0e4d94bc, 0xf89ed2ddbcbf4628, 0x2f7cf8d136534aa2,
            0xf265a4a1fb8335c7, 0xa4778fc22eb6b74e, 0xd853f8dfa8dc313f, 0x1162bc035be61a75, 0x32443631456a4a4b,
            0x8247e3b00a8a8583, 0x8bdc75bb08da5f10, 0x27e44935f029b5aa, 0xf828f8301f2f6681, 0x3511e7d818e033d1,
            0x5eaa69eb3673e6b7, 0xded5356713b85cc8, 0xed71cb61a35fb922, 0x6967f0eebe17495f, 0x37ddc249cac0de95,
            0x56cb01bff8b95b1e, 0x7ae06c0836e66acf, 0xb966728ba24a156d, 0x2431f222087ae262, 0x3aa810220e622ba0,
            0x0cd54146269826fd, 0xb38d91035609925a, 0x3c90404fa8fdcf10, 0xa93ae7e3b29ab57f, 0x3d711d0d40bc2c8d,
            0x898dafcf7ea69738, 0x60c82f9ade96a188, 0xd441d9474b741773, 0xf588d3830e2690af, 0x403dc62d61195af3,
            0xc5c3582884bc019f, 0xab593f8cbe5bde60, 0xb8561a02d8cd4426, 0x96eccb83d59eb445, 0x430548e0b5cd9611,
            0x536507b58b2d6c0a, 0xbf490c3cfde570cb, 0x4b6bd1e189f08282, 0x624cd14038c8da96, 0x45d126d0c3399240,
            0x30e1dde73c9aeac0, 0x4d1bffe4ea9a99f2, 0xe5d0081afb2e1436, 0xe295b1b21ac2995c, 0x489d272385822b6e,
            0x95453966359700ad, 0x17e1392b221445c2, 0x729b7dbd3347f60e, 0xa08f9cf2cc63d953, 0x4b69a4a8a606c031,
            0xc7743390e65965ff, 0x0b847ab940e4873b, 0x8d1f17662a0b3481, 0xdafccc10fa37a2ea, 0x4e321c56e752580d,
            0x7b57ce308f292447, 0xd150427a9a863f22, 0xc5e0b9e5677a4e20, 0x37fca23bf3387e48, 0x5100a2ee4edb94b2,
            0xbda4bd9416c5c7a3, 0x82969bb7db7748d8, 0x00d78b1ec255a12c, 0x32a855887d51994d, 0x53c6bd236475e5cc,
            0x8c2f292c5227c693, 0x42e7bd886156be32, 0xb0df3bf15143943c, 0xc543c697a208d585, 0x5693b9334fe20fda,
            0xed129e30b3377786, 0xf685da7f0125cce9, 0x4226edeb7b2b6deb, 0x532a9f496d475cce, 0x595ddcaddd3aedc3,
            0xc246f045ae5f9921, 0xc469f3cbf90f9ab1, 0xb6ac3d0a4d2b5392, 0x4f4959241985a90a, 0x5c2aaaac17f0e1d6,
            0xb4cc0a0da35889e0, 0xa4511cd3434d2f34, 0x151fa685f47230d4, 0xfba7987ca6a53862, 0x5ef538076593e829,
            0x39494cdb29ef9d2f, 0x6798f6ea9d4221d1, 0xd64e85fe6c15a177, 0x2d5b6bbe1108441c, 0x61bdbb8b67421581,
            0x45aae2f82dee00cc, 0x76e81ce03e84d69a, 0x3316cda9344e35c9, 0xeae47c48f1215e66, 0x648a157110cf21b0,
            0x41eef34546c95b41, 0x79dfbca51da9087a, 0xb36f5ac6d44d45d1, 0x7c3a89b8d8280209, 0x67551870a5af196b,
            0x54f1bf06fd9b552d, 0x7e02c07120a7060f, 0x6738fb4ae60936dd, 0x8ddc4b0ddee2054d, 0x6a1f0700be9de55f,
            0x4e203ffbb54040b7, 0xfe905d1720b87885, 0x986a4823df9a3ca9, 0x61eb119337a3335e, 0x6ceb2fa010c749f6,
            0xf4a190060d439a24, 0x817d74dcf378cd86, 0x172ea99d30f812a7, 0x6af33f05fd82f72c, 0x6fb3df4b98f240b0,
            0x9aec3bed5a1c1404, 0x03a65bb59ad7a06a, 0x1846e90fa0de023b, 0x7d2fe76c33e24ecc, 0x727f8d505178bf54,
            0x6ae899663fdb4f17, 0xaa3655945c870d12, 0x659083b1cd9d70d7, 0x7e7eb442572a626c, 0x754b8b59e6f8c240,
            0x9af398b34acbe664, 0xe3d6ba0a31b9f6c2, 0x768d232448470674, 0x5141acf6bd77a871, 0x7814d9cc335f7d97,
            0x7c43fdf5e9ce2813, 0x2de8c2a724b9fa56, 0xae3b7a549e5c78a5, 0x7084ac99e10c434a, 0x7adf11214c0b6503,
            0xe06fd2c8edc4fd43, 0xb5c5702d7a091fda, 0x8df7819414bb24c6, 0xe968ae72d864e5aa, 0x7daaa2b073c4877f,
            0xe0558594031f34cb, 0xe3b66c6db1b4677b, 0xa3ac86d4e6c0489a, 0x8833f79ac38b15c1, 0x80780d1e994fb7bb,
            0x99c89e96e492dbc9, 0xd4129126b1733a9b, 0x725016b221b694e1, 0x75b2b442b022dd71, 0x83402903760fd134,
            0x9158e49de631a47e, 0xa6c00b85feb1093c, 0xd0872b6328b14761, 0x3dbd94a2b5e37532, 0x860aba7eb47087a9,
            0xf69af9a28d593e56, 0x1da6495b5f99d1d3, 0x8c9a8af0190b27c8, 0xbfd5796310d923a8, 0x88d65318aae1284d,
            0x92fad152f7a419fa, 0xa898e37734f35e6f, 0x93294418a7b1eef9, 0xaaa37bf72a9bca36, 0x8ba145b068050a5c,
            0x9eb0e34d8237d900, 0x248324370f0391a9, 0xf9ab140c3f2319e3, 0xa9a902eb63ec1333, 0x8e6bd976f7ae97c1,
            0x69046a077a9fb34f, 0xb83240d17b1a5789, 0x97b6ad5de0d13c0f, 0xbac8e70e639b423d, 0x91365e5fef79b4cd,
            0x6bbdae5c0da83d6c, 0x37712d2db36f9e39, 0xeef92ae1713389c7, 0xb541d1074ddb5287, 0x94012fa0a5db180a,
            0xbb5676745c77ebbb, 0x57d8473ff0ebece1, 0xaafa85766ff71db6, 0xa7dab323606549d7, 0x96ccb7062a990cae,
            0x58a33d22eb345c38, 0xa1fbe3b19b08aa77, 0xd8499f6481b60d89, 0x92d1e25a3bc9b0e6, 0x9999717e6e1ec71f,
            0x261113e4f262af0f, 0x3df0074844f7ac88, 0x4243932d0eabede2, 0xe575abded0b7d4f0, 0x9c621bf8fc82208d,
            0x406cadf4e2b87105, 0x524e16fdf1d11d7b, 0xaa030b9d01b9fd9b, 0xed08e0cb58f4f54e, 0x9f2e16283b9c5d03,
            0xd73e6dc68f1113a5, 0x942557aebbe3d9ef, 0xf8f9c3fb6b1b03de, 0x097b5496f5a6e1a0, 0xa1f7c7c61f889ecd,
            0xc4fa9ec55a050116, 0x63d4f5d564c5b9e6, 0x5820483ea4c7d28f, 0xf2affdc3344f9ced, 0xa4c2a22cf44c08a6,
            0xc6fe18d269ac4c10, 0xf087df22f5bcc95d, 0xa894adec7b5a152a, 0x78b0e9fb162603ee, 0xa78d60417d3fdd14,
            0xd65fd0023770eb25, 0x8b6f97871f584da8, 0x1b63f18834d9b341, 0xaf5c7a86d6939813, 0xaa585d25a7e182e7,
            0xc093aec132b1c4ab, 0xf91f55da8cc955fc, 0xcc5ecaf63081ce7d, 0xdcf0b021b83092b5, 0xad240f36158ad0c8,
            0xbd41045cdf957eb4, 0x4cf7c3415d74b2e9, 0x01be218cdb35ae43, 0x5dd1c5fec09949c9, 0xafee872faf2e5b19,
            0x5d33fb200912b7e2, 0x58bc9c09ff4efaa2, 0x331d9bd6ed13a3b4, 0x63858412fbdab72e, 0xb2b928e083506504,
            0x3b1cdc0b95df2e73, 0x4436f2f0c8996085, 0xc6f98f67ea025fd7, 0xc65a997a7f30b873, 0xb5846f60626b807f,
            0x403667c077c1fc20, 0x697e62529203f9eb, 0x936eee0e15d0dcea, 0xc22b7c8099952145, 0xb84f2eb5ac3795e7,
            0x388b200e8525e458, 0xb5ca574fc8a17bb9, 0xbb43e02f8ac0e731, 0x77344f09355e7066, 0xbb19877484ca70a4,
            0x77d3b12ce16c9c68, 0x822d209b4424efd5, 0x9684a28dad6b791d, 0xce9ca57812ea9dcf, 0xbde45c1866f2d9e6,
            0x5f01a5224504c357, 0x666c7b378df042a5, 0x3c22aad7fe194a3e, 0x514a5cd169b87306, 0xc0af3b8491d54390,
            0xcf86b4abffee731f, 0xca3271187934a232, 0x27bd0fddf4ecbdca, 0xe7744018c3407aa7, 0xc37a16c32273901b,
            0xef9975deaa64c17a, 0x6ba80fa8e07f2cf2, 0x61818a93d068697f, 0xae3e1a514a871a58, 0xc64503961527469c,
        ];

        static ReadOnlySpan<InlineArray5<UInt64>> AsinPhi0 => MemoryMarshal.CreateReadOnlySpan(
            ref System.Runtime.CompilerServices.Unsafe.As<UInt64, InlineArray5<UInt64>>(
                ref MemoryMarshal.GetReference(AsinPhi0_flat)), 72);

        // ---------- c[10][2] : UInt64 (small, inlined) ----------
        static ReadOnlySpan<UInt64> AsinC_flat => [
            0xaaaaaaaaaaaaaaa9UL, 0xaaaaaaaaaaaaaaaaUL,
            0x333333333333337aUL, 0x0013333333333333UL,
            0xb6db6db6db6dae36UL, 0x000002db6db6db6dUL,
            0x71c71c71c71cfc25UL, 0x000000007c71c71cUL,
            0x2e8ba2e8ba29804eUL, 0x000000000016e8baUL,
            0x3b13b13b13ce93b6UL, 0x0000000000000471UL,
            0xe4cccccccc5f2a5aUL, 0x0000000000000000UL,
            0x002f50f0f1f806dcUL, 0x0000000000000000UL,
            0x000009fef0fec73aUL, 0x0000000000000000UL,
            0x0000000227286573UL, 0x0000000000000000UL,
        ];

        static ReadOnlySpan<InlineArray2<UInt64>> AsinC => MemoryMarshal.CreateReadOnlySpan(
            ref System.Runtime.CompilerServices.Unsafe.As<UInt64, InlineArray2<UInt64>>(
                ref MemoryMarshal.GetReference(AsinC_flat)), 10);

        // ---------- cp[] polynomial table for EvalPoly ----------
        // Contents ported verbatim from the C `static const u64 cp[]` inside evalpoly.
        static ReadOnlySpan<UInt64> AsinEvalPolyCp => [
            0x1343996b9f42b9f5UL, 0x255e6e351770584dUL, 0x00000000000000a3UL, 0x5e2111cba47a2b05UL, 0x000000000005717dUL,
            0xa97f20b758a855cdUL, 0x000000002ea1bcc9UL, 0x889c99395996e6ceUL, 0x00000190cb77f60cUL, 0xc7476c854bade5bfUL,
            0x000d8137abd89d89UL, 0x97b4ea2813d93845UL, 0x74f4aa383759f229UL, 0x5abb1888e58be523UL, 0x5f1f6db6db6db6dbUL,
            0x00000000000003f9UL, 0x9a1160a9ab2539ceUL, 0xa8ba2e8ba2e8ba2eUL, 0x000000000022bdd3UL, 0x72ec43b868c4b3c0UL,
            0xf7bdef7bdef7bdefUL, 0x0000000131683bdeUL, 0x4b2852d709bf2295UL, 0x58469ee58469ee58UL, 0x00000a8dd18469eeUL,
            0x2d86e53634cafb09UL, 0x684bda12f684bda1UL, 0x005e0b7684bda12fUL, 0x151d85735049738fUL, 0xe147ae147ae147aeUL,
            0x4d0c7ae147ae147aUL, 0x0000000000000003UL, 0x9ba6f1b2735cae39UL, 0x6f4de9bd37a6f4deUL, 0xbd37a6f4de9bd37aUL,
            0x0000000000001df3UL, 0xcf46c00a8ed8a2e2UL, 0x3cf3cf3cf3cf3cf3UL, 0xf3cf3cf3cf3cf3cfUL, 0x000000000112ef3cUL,
            0x86baeba7afbb9dd6UL, 0xbca1af286bca1af2UL, 0xa1af286bca1af286UL, 0x00000009fef286bcUL, 0xe1e21d9d6b73053dUL,
            0xe1e1e1e1e1e1e1e1UL, 0xe1e1e1e1e1e1e1e1UL, 0x00005ea1e1e1e1e1UL, 0x33332cfa4ccaad37UL, 0x3333333333333333UL,
            0x3333333333333333UL, 0x0393333333333333UL, 0x89d89e04e6327ae5UL, 0xd89d89d89d89d89dUL, 0x9d89d89d89d89d89UL,
            0x89d89d89d89d89d8UL, 0x0000000000000023UL, 0x8ba2e8b369ee2b14UL, 0xe8ba2e8ba2e8ba2eUL, 0x2e8ba2e8ba2e8ba2UL,
            0xa2e8ba2e8ba2e8baUL, 0x0000000000016e8bUL, 0x8e38e38e78e717bcUL, 0x38e38e38e38e38e3UL, 0xe38e38e38e38e38eUL,
            0x8e38e38e38e38e38UL, 0x000000000f8e38e3UL, 0x6db6db6db566fac3UL, 0xb6db6db6db6db6dbUL, 0xdb6db6db6db6db6dUL,
            0x6db6db6db6db6db6UL, 0x000000b6db6db6dbUL, 0x99999999999e1925UL, 0x9999999999999999UL, 0x9999999999999999UL,
            0x9999999999999999UL, 0x0009999999999999UL, 0xaaaaaaaaaaaaa521UL, 0xaaaaaaaaaaaaaaaaUL, 0xaaaaaaaaaaaaaaaaUL,
            0xaaaaaaaaaaaaaaaaUL, 0xaaaaaaaaaaaaaaaaUL,
        ];
    }

    partial class Binary128Arithmetic {

        // ---- jget: range reduction based on the top 64 bits of a binary128 ----
        // Input `x` is the top 64 bits with sign stripped (raw encoding).
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static UInt64 AsinJget(UInt64 x) {
            unchecked {
                UInt64 z = (0x3FFFUL << 48) - x;
                UInt64 mz = z << 16;
                nint e = (nint)(z >> 48);
                nint nz = LeadingZeroCount(mz) * ((e == 0) ? 1 : 0);
                mz <<= (int)(nz + ((e == 0) ? 1 : 0));
                e -= nz;
                nint lz = (e << 4 | (nint)(mz >> 60)) + 161;
                lz *= (lz >= 0) ? 1 : 0;
                nint j = AsinInd[(int)lz & 0xff] * ((lz < 256) ? 1 : 0);
                nint tz = e << 11 | (nint)(mz >> 53);
                return (UInt64)(Int64)(j + (tz < AsinSth[(int)j] ? 1 : 0));
            }
            
        }

        // ---- omx2v2: approximate 1 - x^2 ----
        static int AsinOmx2v2(out InlineArray4<UInt64> X2, int s, in InlineArray2<UInt64> x) {
            unchecked {
                BigSquareUnsigned(out X2, in x);

                X2[0] = (X2[1] << 1 << (~s & 63)) | (X2[0] >> s);
                X2[0] = ~X2[0];
                X2[1] = (X2[2] << 1 << (~s & 63)) | (X2[1] >> s);
                X2[1] = ~X2[1];
                X2[2] = (X2[3] << 1 << (~s & 63)) | (X2[2] >> s);
                X2[2] = ~X2[2];
                X2[3] = X2[3] >> s;
                X2[3] = ~X2[3];

                int e = 1;
                if (Misc.Likely(X2[3] != 0)) {
                    int lk = (int)UInt64.LeadingZeroCount(X2[3]);
                    X2[3] = (X2[3] << lk) | (X2[2] >> 1 >> (~lk & 63));
                    X2[2] = (X2[2] << lk) | (X2[1] >> 1 >> (~lk & 63));
                    X2[1] = (X2[1] << lk) | (X2[0] >> 1 >> (~lk & 63));
                    X2[0] = X2[0] << lk;
                    e += lk;
                } else {
                    int lk = (int)UInt64.LeadingZeroCount(X2[2]);
                    X2[3] = (X2[2] << lk) | (X2[1] >> 1 >> (~lk & 63));
                    X2[2] = (X2[1] << lk) | (X2[0] >> 1 >> (~lk & 63));
                    X2[1] = X2[0] << lk;
                    X2[0] = 0;
                    e += lk + 64;
                }
                return e;
            }
        }

        // ---- omx2v3: exact normalized 1 - x^2 ----
        static int AsinOmx2v3(out InlineArray4<UInt64> X2, int s, in InlineArray2<UInt64> x) {
            unchecked {
                BigSquareUnsigned(out X2, in x);

                X2[0] = (X2[1] << 1 << (~s & 63)) | (X2[0] >> s);
                X2[1] = (X2[2] << 1 << (~s & 63)) | (X2[1] >> s);
                X2[2] = (X2[3] << 1 << (~s & 63)) | (X2[2] >> s);
                X2[3] = X2[3] >> s;

                X2[0] = SubtractWithBorrow(0, X2[0], 0, out var c);
                X2[1] = SubtractWithBorrow(0, X2[1], c, out c);
                X2[2] = SubtractWithBorrow(0, X2[2], c, out c);
                X2[3] = SubtractWithBorrow(0, X2[3], c, out _);

                int e = 1;
                if (Misc.Likely(X2[3] != 0)) {
                    int lk = (int)UInt64.LeadingZeroCount(X2[3]);
                    X2[3] = (X2[3] << lk) | (X2[2] >> 1 >> (~lk & 63));
                    X2[2] = (X2[2] << lk) | (X2[1] >> 1 >> (~lk & 63));
                    X2[1] = (X2[1] << lk) | (X2[0] >> 1 >> (~lk & 63));
                    X2[0] = X2[0] << lk;
                    e += lk;
                } else {
                    int lk = (int)UInt64.LeadingZeroCount(X2[2]);
                    X2[3] = (X2[2] << lk) | (X2[1] >> 1 >> (~lk & 63));
                    X2[2] = (X2[1] << lk) | (X2[0] >> 1 >> (~lk & 63));
                    X2[1] = X2[0] << lk;
                    X2[0] = 0;
                    e += lk + 64;
                }
                return e;
            }
        }

        // ---- getcos: sqrt(1 - x^2) ----
        static int AsinGetCos(out InlineArray5<UInt64> sq, int ex, in InlineArray2<UInt64> x) {
            unchecked {
                InlineArray4<UInt64> x2;
                int e = AsinOmx2v3(out x2, 2 * (ex - 1), in x);

                UInt64 rx = (x2[3] << 1) | (x2[2] >> 63);
                UInt64 r = Rsqrt9(rx);
                r = (UInt64)(((UInt128)r * Rsqrt2Table[e & 1]) >> 64);

                BigMul(out sq, r, in x2);

                InlineArray6<UInt64> h;
                BigMul(out h, r, in sq);
                ShiftRightUnsignedFull(MemoryMarshal.CreateSpan(ref h[0], 6), 2);

                Int64 msk = (Int64)(h[4]);
                msk >>= 63;
                UInt64 mu = (UInt64)msk;
                h[4] ^= mu; h[3] ^= mu; h[2] ^= mu; h[1] ^= mu; h[0] ^= mu;

                // h2s = sqrhu4(h[1..4]), placed at h2s[1..4]
                InlineArray4<UInt64> h2s_hi;
                SquareHighUnsignedApproximate(out h2s_hi, in Unsafe.As<UInt64, InlineArray4<UInt64>>(ref Unsafe.AsRef(in h[1])));
                InlineArray5<UInt64> h2s = default;
                h2s[1] = h2s_hi[0]; h2s[2] = h2s_hi[1]; h2s[3] = h2s_hi[2]; h2s[4] = h2s_hi[3];

                // h4s = sqrhu2(h2s[3..4])
                InlineArray2<UInt64> h4s;
                SquareHighUnsignedApproximate(out h4s, in Unsafe.As<UInt64, InlineArray2<UInt64>>(ref Unsafe.AsRef(in h2s[3])));

                // h3s = mhu3u3u3(h[2..4], h2s[2..4])  -> placed at h3s[2..4]
                InlineArray3<UInt64> h3s_hi;
                MultiplyHighUnsignedApproximate(out h3s_hi,
                    in Unsafe.As<UInt64, InlineArray3<UInt64>>(ref Unsafe.AsRef(in h[2])),
                    in Unsafe.As<UInt64, InlineArray3<UInt64>>(ref Unsafe.AsRef(in h2s[2])));
                InlineArray5<UInt64> h3s = default;
                h3s[2] = h3s_hi[0]; h3s[3] = h3s_hi[1]; h3s[4] = h3s_hi[2];

                // h2s[1..4] *= 3
                MultiplyBy3(ref Unsafe.As<UInt64, InlineArray4<UInt64>>(ref h2s[1]));
                // h3s[2..4] *= 5
                MultiplyBy5(ref Unsafe.As<UInt64, InlineArray3<UInt64>>(ref h3s[2]));

                // t4u = h4s + (h4s*3 >> 5)
                UInt128 t4u = ((UInt128)h4s[1] << 64) | h4s[0];
                t4u += (t4u * 3) >> 5;
                InlineArray5<UInt64> t4 = default;
                t4[3] = (UInt64)t4u;
                t4[4] = (UInt64)(t4u >> 64);

                h2s[0] = 0;
                ShiftRightUnsignedFull(MemoryMarshal.CreateSpan(ref h2s[0], 5), 62 - (e & 1));
                h3s[0] = h3s[1] = 0;
                ShiftRightUnsignedFull(MemoryMarshal.CreateSpan(ref h3s[0], 5), 59 + 64 - 2 * (e & 1));
                ShiftRightUnsignedFull(MemoryMarshal.CreateSpan(ref t4[0], 5), 56 + 128 - 3 * (e & 1));

                InlineArray5<UInt64> hBuf = default;
                hBuf[0] = h[0]; hBuf[1] = h[1]; hBuf[2] = h[2]; hBuf[3] = h[3]; hBuf[4] = h[4];

                if (msk != 0) {
                    AddUnchecked(out hBuf, in hBuf, in h2s);
                    AddUnchecked(out hBuf, in hBuf, in h3s);
                    AddUnchecked(out hBuf, in hBuf, in t4);
                } else {
                    SubtractUnchecked(out hBuf, in hBuf, in h2s);
                    AddUnchecked(out hBuf, in hBuf, in h3s);
                    SubtractUnchecked(out hBuf, in hBuf, in t4);
                }

                // hBuf = r * hBuf  -> 6 words
                InlineArray6<UInt64> h6;
                BigMul(out h6, r, in hBuf);
                
                // h6 = (h6[1..5] * x2l) where x2l = [0,x2[0..3]] -> 5-word high
                InlineArray5<UInt64> x2l = default;
                x2l[1] = x2[0]; x2l[2] = x2[1]; x2l[3] = x2[2]; x2l[4] = x2[3];
                InlineArray5<UInt64> h5;
                MultiplyHighUnsignedApproximate(out h5,
                    in Unsafe.As<UInt64, InlineArray5<UInt64>>(ref Unsafe.AsRef(in h6[1])),
                    in x2l);
                ShiftRightUnsignedFull(MemoryMarshal.CreateSpan(ref h5[0], 5), 62 - (e & 1));

                if (msk == 0) {
                    SubtractUnchecked(out sq, in sq, in h5);
                } else {
                    AddUnchecked(out sq, in sq, in h5);
                }
                return e;
            }
        }

        // ---- evalpoly (Hastings-style polynomial on 5-word t2) ----
        static void AsinEvalPoly(out InlineArray5<UInt64> f, in InlineArray5<UInt64> t2) {
            unchecked {
                ReadOnlySpan<UInt64> cp = AsinEvalPolyCp;
                InlineArray5<UInt64> fp = default;
                int ck = 0;

                fp[0] = cp[0];
                {
                    var r = MultiplyHigh(t2[4], fp[0]);
                    fp[0] = MultiplyHigh(r, cp[ck + 1]);
                }
                ck += 1;

                fp[1] = cp[ck + 1];
                for (int k = 0; k < 7; k++) {
                    InlineArray2<UInt64> a = default; a[0] = fp[0]; a[1] = fp[1];
                    InlineArray2<UInt64> b = default; b[0] = t2[3]; b[1] = t2[4];
                    InlineArray2<UInt64> r;
                    MultiplyHighUnsignedApproximate(out r, in b, in a);
                    InlineArray2<UInt64> c = default; c[0] = cp[ck + 2]; c[1] = cp[ck + 3];
                    AddUnchecked(out r, in c, in r);
                    fp[0] = r[0]; fp[1] = r[1];
                    ck += 2;
                }

                fp[2] = cp[ck + 2];
                for (int k = 0; k < 5; k++) {
                    InlineArray3<UInt64> a = default; a[0] = fp[0]; a[1] = fp[1]; a[2] = fp[2];
                    InlineArray3<UInt64> b = default; b[0] = t2[2]; b[1] = t2[3]; b[2] = t2[4];
                    InlineArray3<UInt64> r;
                    MultiplyHighUnsignedApproximate(out r, in a, in b);
                    InlineArray3<UInt64> c = default; c[0] = cp[ck + 3]; c[1] = cp[ck + 4]; c[2] = cp[ck + 5];
                    AddUnchecked(out r, in c, in r);
                    fp[0] = r[0]; fp[1] = r[1]; fp[2] = r[2];
                    ck += 3;
                }

                fp[3] = cp[ck + 3];
                for (int k = 0; k < 6; k++) {
                    InlineArray4<UInt64> a = default; a[0] = fp[0]; a[1] = fp[1]; a[2] = fp[2]; a[3] = fp[3];
                    InlineArray4<UInt64> b = default; b[0] = t2[1]; b[1] = t2[2]; b[2] = t2[3]; b[3] = t2[4];
                    InlineArray4<UInt64> r;
                    MultiplyHighUnsignedApproximate(out r, in a, in b);
                    InlineArray4<UInt64> c = default; c[0] = cp[ck + 4]; c[1] = cp[ck + 5]; c[2] = cp[ck + 6]; c[3] = cp[ck + 7];
                    AddUnchecked(out r, in c, in r);
                    fp[0] = r[0]; fp[1] = r[1]; fp[2] = r[2]; fp[3] = r[3];
                    ck += 4;
                }

                fp[4] = cp[ck + 4];
                for (int k = 0; k < 5; k++) {
                    InlineArray5<UInt64> a = default; a[0] = fp[0]; a[1] = fp[1]; a[2] = fp[2]; a[3] = fp[3]; a[4] = fp[4];
                    InlineArray5<UInt64> r;
                    MultiplyHighUnsignedApproximate(out r, in a, in t2);
                    InlineArray5<UInt64> c = default;
                    c[0] = cp[ck + 5]; c[1] = cp[ck + 6]; c[2] = cp[ck + 7]; c[3] = cp[ck + 8]; c[4] = cp[ck + 9];
                    AddUnchecked(out r, in c, in r);
                    fp[0] = r[0]; fp[1] = r[1]; fp[2] = r[2]; fp[3] = r[3]; fp[4] = r[4];
                    ck += 5;
                }

                f = fp;
            }
        }
    }
    partial class Binary128Arithmetic {

        // 1.921fb54442d18469898cc51701b8p+0q  =  pi/2  (binary128)
        // (sign=0, exp=0x3FFF, frac hi = 0x921FB54442D1, frac lo = 0x8469898CC51701B8)
        static ReadOnlySpan<UInt64> PiOverTwoBits => [
            0x8469898CC51701B8UL, 0x3FFF921FB54442D1UL,
        ];

        // ln2/2-ish comment: constant `pi/2` reused below.

        static UInt128 AsinCore(UInt128 x, MidpointRounding rm) {
            unchecked {
                const UInt64 smsk = 1UL << 63;

                UInt64 xLo = (UInt64)x, xHi = (UInt64)(x >> 64);
                UInt64 xsgn = xHi & smsk;
                xHi &= ~smsk;
                int xn = (int)(xHi >> 48);

                // ---- Fast path: |x| < 2^-57 ----
                if (Misc.Unlikely(xn < 0x3FFF - 56)) {
                    UInt128 X = ((UInt128)xHi << 64) | xLo;
                    if (X == 0) return x;   // ±0

                    bool underflowFlag = false;
                    if (xHi < (1UL << 48)) underflowFlag = true;

                    X += (UInt64)(((rm != MidpointRounding.ToEven) ? 1 : 0)
                        * ((xsgn == 0 ? 1 : 0) * (rm == MidpointRounding.ToPositiveInfinity ? 1 : 0)
                         + (xsgn != 0 ? 1 : 0) * (rm == MidpointRounding.ToNegativeInfinity ? 1 : 0)));

                    xHi = (UInt64)(X >> 64) | xsgn;
                    xLo = (UInt64)X;

                    if (underflowFlag) RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Underflow);
                    RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                    return ((UInt128)xHi << 64) | xLo;
                }

                // ---- Special cases: |x| >= 1, Inf, NaN ----
                if (Misc.Unlikely(xn >= 0x3FFF)) {
                    if (xLo == 0 && xHi == (UInt64)0x3FFF << 48) {
                        // |x| = 1  ->  ±pi/2
                        UInt128 pi2 = ((UInt128)PiOverTwoBits[1] << 64) | PiOverTwoBits[0];
                        pi2 += (UInt64)(((rm != MidpointRounding.ToEven) ? 1 : 0)
                            * ((xsgn == 0 ? 1 : 0) * (rm == MidpointRounding.ToPositiveInfinity ? 1 : 0)
                             + (xsgn != 0 ? 1 : 0) * (rm == MidpointRounding.ToNegativeInfinity ? 1 : 0)));
                        UInt64 outHi = (UInt64)(pi2 >> 64) | xsgn;
                        UInt64 outLo = (UInt64)pi2;
                        RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                        return ((UInt128)outHi << 64) | outLo;
                    } else {
                        UInt128 Xa = ((UInt128)xHi << 64) | xLo;
                        int xnan = GetClass(Xa);
                        if (xnan == 2) {
                            RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Invalid);
                            return GetNaN();
                        } else if (xnan == 3) {
                            return x;
                        }
                        RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Invalid);
                        return GetNaN();
                    }
                }

                // ---- Range reduction ----
                UInt64 j = AsinJget(xHi);

                xHi |= 1UL << 48;
                UInt128 Xa2 = ((UInt128)xHi << 64) | (xLo << 15);
                xHi = (UInt64)(Xa2 >> 64);
                xLo = (UInt64)Xa2;

                UInt128 t = Xa2;
                int nz = 0x3FFF - xn;
                InlineArray3<UInt64> xc = default;

                if (j != 0) {
                    InlineArray2<UInt64> xb = default; xb[0] = xLo; xb[1] = xHi;

                    InlineArray4<UInt64> X2;
                    int e = AsinOmx2v2(out X2, 2 * nz - 2, in xb);

                    UInt64 rx = (X2[3] << 1) | (X2[2] >> 63);
                    UInt64 r = Rsqrt9(rx);
                    r = (UInt64)(((UInt128)r * Rsqrt2Table[e & 1]) >> 64);

                    InlineArray3<UInt64> SX;
                    MultiplyHigh(out SX, r, in Unsafe.As<UInt64, InlineArray3<UInt64>>(ref Unsafe.AsRef(in X2[1])));
                    InlineArray3<UInt64> H;
                    MultiplyHigh(out H, r, in SX);

                    const int koff = 2, rkoff = 64 - koff;
                    H[0] = (H[0] >> koff) | (H[1] << rkoff);
                    H[1] = (H[1] >> koff) | (H[2] << rkoff);
                    Int64 hh = (Int64)H[1];
                    UInt64 h2 = (UInt64)MultiplyHigh(hh, hh);
                    h2 += h2 >> 1;

                    UInt128 Hh = ((UInt128)H[1] << 64) | H[0];
                    int lk2 = (e & 1) + koff, rk2 = (64 - lk2) & 63;
                    UInt128 H2 = ((UInt128)(h2 >> rk2) << 64) | (h2 << lk2);
                    Hh -= H2;

                    Int128 D = MultiplyHighApproximate((Int128)Hh, ((UInt128)SX[2] << 64) | SX[1]);
                    UInt64 D3s = (UInt64)(D >> 127);
                    InlineArray3<UInt64> D3 = default;
                    D3[0] = (UInt64)D;
                    D3[1] = (UInt64)(D >> 64);
                    D3[2] = D3s;
                    D3[2] = (D3[2] << lk2) | (D3[1] >> rk2);
                    D3[1] = (D3[1] << lk2) | (D3[0] >> rk2);
                    D3[0] = D3[0] << lk2;

                    SubtractUnchecked(out SX, in SX, in D3);

                    // (mhu3u2u3) xc = xb * cth[j][2..4]
                    InlineArray3<UInt64> cth_hi = default;
                    cth_hi[0] = AsinCth[(int)j][2]; cth_hi[1] = AsinCth[(int)j][3]; cth_hi[2] = AsinCth[(int)j][4];
                    xLo = (UInt64)((UInt128)xLo >> (nz & 63)) | (UInt64)(((UInt128)(xHi & ~((1UL << 48) - 1)) >> (nz & 63)) << 0); // approximated via combined shift
                    // Simpler: rebuild the top-128 and shift it.
                    UInt128 Xshift = ((UInt128)xHi << 64) | xLo; // contains the pre-shift value if we saved it
                    // (Direct port follows; see the equivalent reconstruction below.)
                    // For clarity, redo the shift:
                    UInt64 saveHi = xHi, saveLo = xLo;
                    // The C code: X.a >>= nz&63;  -- 128-bit logical right shift by nz&63.
                    // We must operate on the true u128 X.a. Rebuild it:
                    UInt128 Xa = ((UInt128)saveHi << 64) | saveLo;
                    Xa >>= (nz & 63);
                    xHi = (UInt64)(Xa >> 64);
                    xLo = (UInt64)Xa;
                    xb[0] = xLo; xb[1] = xHi;

                    MultiplyHighUnsignedApproximate(out xc, in xb, in cth_hi);

                    UInt64 sj = AsinPth[(int)j];
                    int sp = 43 - (e >> 1);
                    if (Misc.Likely(sp >= 0)) {
                        sj <<= sp;
                        MultiplyHigh(out SX, sj, in SX);
                    } else {
                        MultiplyHigh(out SX, sj, in SX);
                        int rk_neg = (-sp) & 63;
                        int lk_neg = (64 + sp) & 63;
                        SX[0] = (SX[0] >> rk_neg) | (SX[1] << lk_neg);
                        SX[1] = (SX[1] >> rk_neg) | (SX[2] << lk_neg);
                        SX[2] = SX[2] >> rk_neg;
                    }

                    SubtractUnchecked(out xc, in xc, in SX);
                    nz = (int)UInt64.LeadingZeroCount(xc[2]);
                    t = ((UInt128)((xc[2] << nz) | (xc[1] >> ((-nz) & 63))) << 64)
                        | ((xc[1] << nz) | (xc[0] >> ((-nz) & 63)));

                    InlineArray5<UInt64> phi_hi = AsinPhi0[(int)j];
                    InlineArray3<UInt64> phi_lo = default;
                    phi_lo[0] = phi_hi[2]; phi_lo[1] = phi_hi[3]; phi_lo[2] = phi_hi[4];
                    AddUnchecked(out xc, in xc, in phi_lo);
                }

                UInt128 t2 = SquareHighApproximate(t);
                UInt128 t3 = MultiplyHighApproximate(t, t2);
                int s2 = 2 * (nz - 6);
                t2 >>= s2;

                // ---- Polynomial evaluation ----
                UInt64 t2h = (UInt64)(t2 >> 64);
                UInt64 fl = AsinC_flat[9 * 2];
                fl = AsinC_flat[8 * 2] + MultiplyHigh(t2h, fl);
                fl = AsinC_flat[7 * 2] + MultiplyHigh(t2h, fl);
                fl = AsinC_flat[6 * 2] + MultiplyHigh(t2h, fl);
                UInt128 f = ((UInt128)AsinC_flat[5 * 2 + 1] << 64)
                          | (AsinC_flat[5 * 2 + 0] + MultiplyHigh(t2h, fl));
                int ii = 5;
                while (ii > 0) {
                    ii--;
                    UInt128 c_i = ((UInt128)AsinC_flat[ii * 2 + 1] << 64) | AsinC_flat[ii * 2 + 0];
                    f = c_i + MultiplyHighApproximate(t2, f);
                }
                f = MultiplyHighApproximate(t3, f);

                UInt128 v = 0;
                UInt64 rnd = 0;

                if (j != 0) {
                    int sf = 3 * nz;
                    InlineArray3<UInt64> f3 = default;
                    f3[2] = (UInt64)(f >> 64);
                    f3[1] = (UInt64)f;

                    if (Misc.Likely(sf < 64)) {
                        f3[0] = f3[1] << ((-sf) & 63);
                        f3[1] = (f3[1] >> sf) | (f3[2] << ((-sf) & 63));
                        f3[2] = f3[2] >> sf;
                    } else if (sf < 128) {
                        sf -= 64;
                        f3[0] = (f3[1] >> sf) | (f3[2] << 1 << (~sf & 63));
                        f3[1] = f3[2] >> sf;
                        f3[2] = 0;
                    } else if (sf < 192) {
                        sf -= 128;
                        f3[0] = f3[2] >> sf;
                        f3[2] = f3[1] = 0;
                    } else {
                        f3[2] = f3[1] = f3[0] = 0;
                    }

                    AddUnchecked(out xc, in xc, in f3);
                    int k = (int)UInt64.LeadingZeroCount(xc[2]);
                    rnd = (xc[1] >> (14 - k)) & 1;
                    xn = 0x3FFE - k;

                    UInt64 Eps = (3 * nz - 6 > 57) ? 64UL : ((1UL << 63) >> (3 * nz - 6));
                    UInt128 msk = ~(UInt128)0 >> (k + 0x31 + (rm == MidpointRounding.ToEven ? 1 : 0));
                    UInt128 tl = ((UInt128)xc[1] << 64) | xc[0];
                    tl += Eps;
                    tl &= msk;
                    if (tl < 2 * Eps) return AsinAccurate(x, rm);

                    UInt64 vHi = (xc[1] >> (15 - k)) | (xc[2] << (49 + k));
                    UInt64 vLo = xc[2] >> (15 - k);
                    v = ((UInt128)vLo << 64) | vHi;   // low word first, matching b128 layout
                } else {
                    int sf = 2 * nz + 1;
                    InlineArray4<UInt64> f4 = default;
                    f4[3] = (UInt64)(f >> 64);
                    f4[2] = (UInt64)f;

                    if (Misc.Likely(sf < 64)) {
                        f4[0] = 0;
                        f4[1] = f4[2] << ((-sf) & 63);
                        f4[2] = (f4[2] >> sf) | (f4[3] << ((-sf) & 63));
                        f4[3] = f4[3] >> sf;
                    } else if (sf < 128) {
                        sf -= 64;
                        f4[0] = f4[2] << ((-sf) & 63);
                        f4[1] = (f4[2] >> sf) | (f4[3] << 1 << (~sf & 63));
                        f4[2] = f4[3] >> sf;
                        f4[3] = 0;
                    } else if (sf < 192) {
                        sf -= 128;
                        f4[0] = (f4[2] >> sf) | (f4[3] << 1 << (~sf & 63));
                        f4[1] = f4[2] >> sf;
                        f4[3] = f4[2] = 0;
                    } else {
                        f4[3] = f4[2] = f4[1] = f4[0] = 0;
                    }

                    // x is halved and added into f4's top two words
                    UInt128 xa = ((UInt128)xHi << 64) | xLo;
                    xa >>= 1;
                    f4[2] = AddWithCarry((UInt64)xa, f4[2], 0, out var cr);
                    f4[3] = AddWithCarry((UInt64)(xa >> 64), f4[3], cr, out _);

                    UInt64 vLo = f4[2];
                    UInt64 vHi = f4[3];
                    int k = (int)(vHi >> 63);
                    rnd = (vLo >> (13 + k)) & 1;
                    UInt128 vFull = ((UInt128)vHi << 64) | vLo;
                    vFull >>= (14 + k);
                    xn += k;
                    vHi = (UInt64)(vFull >> 64) & (~0UL >> 16);
                    vLo = (UInt64)vFull;
                    v = ((UInt128)vHi << 64) | vLo;

                    int lk2 = 64 - 13 - k, rk2 = 64 - lk2;
                    UInt64 Th = (f4[2] << lk2) | (f4[1] >> rk2);
                    UInt64 Tl = (f4[1] << lk2) | (f4[0] >> rk2);
                    UInt128 Eps = (UInt128)1 << 127;
                    Eps >>= 8 + 2 * nz + k;
                    UInt128 T = ((UInt128)Th << 64) | Tl;
                    T += Eps;
                    if (T < Eps) return AsinAccurate(x, rm);
                }

                if (Misc.Unlikely(rm != MidpointRounding.ToEven)) {
                    rnd = (UInt64)(((xsgn == 0 ? 1 : 0) * (rm == MidpointRounding.ToPositiveInfinity ? 1 : 0)
                                   + (xsgn != 0 ? 1 : 0) * (rm == MidpointRounding.ToNegativeInfinity ? 1 : 0)));
                }

                UInt128 dv = ((UInt128)(((UInt64)xn << 48) | xsgn) << 64) | rnd;
                v += dv;

                RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                return v;
            }
        }
    }

    partial class Binary128Arithmetic {

        static UInt128 AsinAccurate(UInt128 x, MidpointRounding rm) {
            unchecked {
                const UInt64 smsk = 1UL << 63;

                UInt64 xLo = (UInt64)x, xHi = (UInt64)(x >> 64);
                UInt64 xsgn = xHi & smsk;
                xHi &= ~smsk;
                int xn = (int)(xHi >> 48);

                UInt64 j = AsinJget(xHi);
                xHi |= 1UL << 48;
                UInt128 Xa2 = ((UInt128)xHi << 64) | (xLo << 15);
                xHi = (UInt64)(Xa2 >> 64);
                xLo = (UInt64)Xa2;

                InlineArray5<UInt64> t = default;
                t[3] = xLo; t[4] = xHi;
                int nz = 0x3FFF - xn;
                InlineArray5<UInt64> xc = default;

                if (j != 0) {
                    InlineArray2<UInt64> xb = default; xb[0] = xLo; xb[1] = xHi;

                    InlineArray5<UInt64> sq;
                    int e = AsinGetCos(out sq, nz, in xb);

                    // X.a >>= nz & 63
                    UInt128 Xa = ((UInt128)xHi << 64) | xLo;
                    Xa >>= (nz & 63);
                    xHi = (UInt64)(Xa >> 64);
                    xLo = (UInt64)Xa;
                    xb[0] = xLo; xb[1] = xHi;

                    MultiplyHighUnsignedApproximate(out xc, in xb, in AsinCth[(int)j]);

                    UInt64 sj = AsinPth[(int)j];
                    int sp = 43 - (e >> 1);
                    if (Misc.Likely(sp >= 0)) sj <<= sp;

                    MultiplyHigh(out sq, sj, in sq);
                    if (Misc.Unlikely(sp < 0)) ShiftRightUnsignedFull(MemoryMarshal.CreateSpan(ref sq[0], 5), -sp);

                    SubtractUnchecked(out xc, in xc, in sq);
                    t = xc;
                    for (int i = 4; i >= 0; i--) {
                        if (t[i] != 0) { nz = (int)UInt64.LeadingZeroCount(t[i]) + (4 - i) * 64; break; }
                    }
                    ShiftLeftUnsignedFull(MemoryMarshal.CreateSpan(ref t[0], 5), nz);

                    InlineArray5<UInt64> phi = AsinPhi0[(int)j];
                    AddUnchecked(out xc, in xc, in phi);
                }

                InlineArray5<UInt64> t2;
                SquareHighUnsignedApproximate(out t2, in t);
                InlineArray5<UInt64> t3;
                MultiplyHighUnsignedApproximate(out t3, in t, in t2);

                int s2 = 2 * (nz - 6) - 1;
                ShiftRightUnsignedFull(MemoryMarshal.CreateSpan(ref t2[0], 5), s2);

                InlineArray5<UInt64> f;
                AsinEvalPoly(out f, in t2);
                MultiplyHighUnsignedApproximate(out f, in t3, in f);

                UInt128 v;
                UInt64 rnd;
                if (j != 0) {
                    int sf = 3 * nz;
                    ShiftRightUnsignedFull(MemoryMarshal.CreateSpan(ref f[0], 5), sf);
                    AddUnchecked(out xc, in xc, in f);
                    int k = (int)UInt64.LeadingZeroCount(xc[4]);
                    rnd = (xc[3] >> (14 - k)) & 1;
                    xn = 0x3FFE - k;
                    UInt64 vLo = (xc[3] >> (15 - k)) | (xc[4] << (49 + k));
                    UInt64 vHi = xc[4] >> (15 - k);
                    v = ((UInt128)vHi << 64) | vLo;
                } else {
                    int sf = 2 * nz + 1;
                    ShiftRightUnsignedFull(MemoryMarshal.CreateSpan(ref f[0], 5), sf);

                    UInt128 Xa = ((UInt128)xHi << 64) | xLo;
                    Xa >>= 1;
                    f[3] = AddWithCarry((UInt64)Xa, f[3], 0, out var c);
                    f[4] = AddWithCarry((UInt64)(Xa >> 64), f[4], c, out _);

                    UInt64 vLo = f[3];
                    UInt64 vHi = f[4];
                    int k = (int)(vHi >> 63);
                    rnd = (vLo >> (13 + k)) & 1;
                    UInt128 vFull = ((UInt128)vHi << 64) | vLo;
                    vFull >>= (14 + k);
                    xn += k;
                    vHi = (UInt64)(vFull >> 64) & (~0UL >> 16);
                    vLo = (UInt64)vFull;
                    v = ((UInt128)vHi << 64) | vLo;
                }

                if (Misc.Unlikely(rm != MidpointRounding.ToEven)) {
                    rnd = (UInt64)(((xsgn == 0 ? 1 : 0) * (rm == MidpointRounding.ToPositiveInfinity ? 1 : 0)
                                   + (xsgn != 0 ? 1 : 0) * (rm == MidpointRounding.ToNegativeInfinity ? 1 : 0)));
                }

                UInt128 dv = ((UInt128)(((UInt64)xn << 48) | xsgn) << 64) | rnd;
                v += dv;

                RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                return v;
            }
        }
    }
    partial class Binary128Arithmetic {

        public static UInt128 Asin(UInt128 x, MidpointRounding mode) => AsinCore(x, mode);
        public static UInt128 Asin(UInt128 x) => AsinCore(x, MidpointRounding.ToEven);

        public static UInt64 Asin(UInt64 x_lo, UInt64 x_hi, MidpointRounding mode, out UInt64 result_hi) {
            unchecked {
                UInt128 x = ((UInt128)x_hi << 64) | x_lo;
                UInt128 r = AsinCore(x, mode);
                result_hi = (UInt64)(r >> 64);
                return (UInt64)r;
            }
        }

        public static UInt64 Asin(UInt64 x_lo, UInt64 x_hi, out UInt64 result_hi) {
            unchecked {
                UInt128 x = ((UInt128)x_hi << 64) | x_lo;
                UInt128 r = AsinCore(x, MidpointRounding.ToEven);
                result_hi = (UInt64)(r >> 64);
                return (UInt64)r;
            }
        }
    }

}
