using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Numerics;
using UltimateOrb.Unmanaged;
using Misc = UltimateOrb.Miscellaneous;

namespace UltimateOrb {

    public static partial class CorrectRoundingMath {


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double RoundPartial(double x) {
            Debug.Assert(double.IsFinite(x));
            return Math.Round(x); // tiesToEven
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double BigAddDDPartial(double x, double y, out double e) {
            e = DoubleArithmeticF.BigAddPartial(x, y, out var r);
            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double AddDDPartial(double xh, double xl, double yh, double yl, out double e) {
            var sh = BigAddDDPartial(xh, yh, out var sl);
            e = (xl + yl) + sl;
            return sh;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double MultiplyDD(double xh, double xl, double ch, double cl, out double l) {
            double ahhh = ch * xh;
            l = (ch * xl + cl * xh) + double.FusedMultiplyAdd(ch, xh, -ahhh);
            return ahhh;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double MultiplyHighDD(double xh, double xl, double c, out double l) {
            double h = c * xh;
            l = c * xl + double.FusedMultiplyAdd(c, xh, -h);
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double EvalPolyOddDD(double xh, double xl, int n, ReadOnlyManagedPtr<InlineArray2<double>> c, out double l) {
            unchecked {
                int i = n - 1;
                double ch = c[i][0], cl = c[i][1];
                while (--i >= 0) {
                    ch = MultiplyDD(xh, xl, ch, cl, out cl);
                    ch = AddDDPartial(c[i][0], c[i][1], ch, cl, out cl);
                }
                l = cl;
                return ch;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double EvalPolyOddDD(double x, int n, ReadOnlyManagedPtr<InlineArray2<double>> c, out double l) {
            unchecked {
                int i = n - 1;
                double ch = RoundPartial(c[i][0]), cl = RoundPartial(c[i][1]);
                while (--i >= 0) {
                    ch = MultiplyDD(x, 0, ch, cl, out cl);
                    ch = AddDDPartial(c[i][0], c[i][1], ch, cl, out cl);
                }
                l = cl;
                return ch;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double AsExpLD(double x, Int64 i) {
            unchecked {
                if (Vector128.IsHardwareAccelerated) {
                    Vector128<Int64> sb = Vector128.CreateScalarUnsafe(i << 52);
                    Vector128<double> r = Vector128.CreateScalarUnsafe(x);
                    return (sb + r.AsInt64()).AsDouble()[0];
                } else {
                    Int64 bits = BitConverter.DoubleToInt64Bits(x);
                    bits += (Int64)i << 52;
                    return BitConverter.Int64BitsToDouble(bits);
                }
            }
        }

        static ReadOnlySpan<double> ExpM1Db => [
            0.11941886519949067, 0.27540600958627748, 0.27755765846625235, 0.28901898938445919,
            0.37641383522340355, 0.38646769083336091, 0.43424665650553418, 0.50069332895087848,
            0.83752245534057401, 0.90727484861634911, 4.6130429605534751, 6.0598594832526835,
            16.559833721798679, 17.835004503812709, 28.268769111813619, 37.862797029174125,
            49.229206176268114, 94.594658224139906, 143.52668152238471, 170.40063412375622,
            170.71172754077605, 301.66366426251898, 414.48199762232366, 470.27976575414419,
            535.50228711246621, -0.10437659580587229, -0.26719162429499793, -0.27279120126225909,
            -0.31963083006878507, -0.36778644247009107, -0.37764691837076181, -0.40383574410560363,
            -0.53826659409403055, -0.56813286662045848, -0.93001836374961722, -1.0403571407112755,
            -1.1664532184587633, -1.8912684161004483,
        ];

        static ReadOnlySpan<UInt64> ExpM1DbS2 => [
            0x76f58b0d65bd5553UL, 0xc06UL,
        ];

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
        static double AsExpM1Db(double x, double f) {
            unchecked {
                int a = 0, b = ExpM1Db.Length - 1, m = (a + b) / 2;
                var c = ExpM1Db;
                while (a <= b) {
                    if (BitConverter.DoubleToUInt64Bits(c[m]) < BitConverter.DoubleToUInt64Bits(x)) {
                        a = m + 1;
                    } else if (Misc.Unlikely(BitConverter.DoubleToUInt64Bits(c[m]) == BitConverter.DoubleToUInt64Bits(x))) {
                        const UInt64 s = 0X300e81651cUL;
                        var dr = BitConverter.UInt64BitsToDouble(((s >> m) << 63) | (((BitConverter.DoubleToUInt64Bits(f) >> 52) & 0x7ff) - 54) << 52);
                        UInt64 t = (ExpM1DbS2[m >> 5] >> ((m << 1) & 63)) & 3;
                        for (Int64 k = -1; k <= 1; k++) {
                            var r = BitConverter.DoubleToUInt64Bits(f) + (UInt64)k;
                            if ((r & 3) == t) return BitConverter.UInt64BitsToDouble(r) + dr;
                        }
                        break;
                    } else {
                        b = m - 1;
                    }
                    m = (a + b) >> 1;
                }
                return f;
            }
        }

        static ReadOnlySpan<double> ExpM1T0_flat => [
            0, 1, -1.523477860336858E-17, 1.0108892860517005,
            5.1092250289734445E-17, 1.0218971486541166, 7.60083887402709E-18, 1.0330248790212284,
            8.5518897055379637E-17, 1.0442737824274138, 1.7593257387720916E-18, 1.0556451783605572,
            -7.8998539668415821E-17, 1.0671404006768237, -6.6566604360565926E-17, 1.0787607977571199,
            -3.0467820798124711E-17, 1.0905077326652577, 5.2660368715706944E-17, 1.1023825833078409,
            1.0410278456845571E-16, 1.1143867425958924, 5.1658567587954561E-17, 1.1265216186082418,
            8.9128126760254065E-17, 1.1387886347566916, 3.2507102188638278E-17, 1.1511892299529827,
            3.8292048369240941E-17, 1.1637248587775775, 5.5542032542180777E-17, 1.1763969916502812,
            3.9820152314656461E-17, 1.189207115002721, 6.6449814992523012E-17, 1.2021567314527031,
            -7.7126306926814881E-17, 1.215247359980469, -1.8987816313025296E-17, 1.22848053610687,
            4.6580275918369368E-17, 1.241857812073484, -6.7113898212968784E-18, 1.2553807570246911,
            2.6679321313421861E-18, 1.2690509571917332, 1.713594918243561E-17, 1.2828700160787783,
            2.5382502794888315E-17, 1.2968395546510096, -7.1815361355194551E-17, 1.3109612115247644,
            -2.8587312100388608E-17, 1.3252366431597413, 8.9272825948317308E-17, 1.3396675240533029,
            7.7009483798029882E-17, 1.3542555469368927, 9.5937979191188488E-17, 1.3690024229745905,
            -6.7705116587947851E-17, 1.383909881963832, -9.6142132090513231E-17, 1.3989796725383112,
            -9.6672933134529135E-17, 1.4142135623730951, -1.2031642489053654E-17, 1.42961333839197,
            -3.0237581349939879E-17, 1.4451808069770467, -5.600377186075217E-17, 1.460917794180647,
            -3.4839945568927964E-17, 1.4768261459394993, 1.4192920154284036E-17, 1.4929077282912648,
            -1.016455327754295E-16, 1.5091644275934228, -1.1024941712342561E-16, 1.5255981507445384,
            7.9498348096976196E-17, 1.5422108254079407, 3.7812070533575275E-17, 1.5590044002378369,
            -1.0136916471278304E-17, 1.5759808451078865, -1.0094406542311962E-16, 1.593142151342267,
            2.4707192569797888E-17, 1.6104903319492543, -6.7129550847070829E-17, 1.6280274218573478,
            -1.0125679913674774E-16, 1.6457554781539649, 5.8909926967131009E-17, 1.6636765803267364,
            8.1990100205814978E-17, 1.681792830507429, -8.0237193703977002E-18, 1.7001063537185235,
            -1.8513804182631107E-17, 1.7186192981224779, 3.1643892992929569E-17, 1.7373338352737062,
            2.9601406954488739E-17, 1.7562521603732995, 6.4297317965565708E-17, 1.7753764925265212,
            1.822745842791209E-17, 1.7947090750031072, -9.9695315389203488E-17, 1.8142521755003989,
            3.2831072242456266E-17, 1.8340080864093424, 9.7618874907275935E-17, 1.8539791250833855,
            -6.1227634130041426E-17, 1.8741676341103, 3.4034035352165303E-17, 1.8945759815869656,
            -1.0619946056195964E-16, 1.9152065613971474, 1.0332385960676326E-16, 1.9360617934922943,
            8.9607677910366665E-17, 1.9571441241754002, 4.0388753109278167E-17, 1.9784560263879509,
        ];

        static ReadOnlySpan<InlineArray2<double>> ExpM1T0 {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<double, InlineArray2<double>>(
                ref MemoryMarshal.GetReference(ExpM1T0_flat)), 64);
        }

        static ReadOnlySpan<double> ExpM1T1_flat => [
            0, 1, 9.336185335478462E-17, 1.0001692397053021,
            -5.1413339313189571E-18, 1.0003385080526823, 6.9624240220205726E-17, 1.0005078050469876,
            -5.115123297685667E-17, 1.0006771306930664, 8.4229900245864878E-17, 1.0008464849957674,
            -2.8245220747761684E-17, 1.001015867959941, -7.1804245655921329E-17, 1.0011852795904375,
            -1.8973728416792996E-17, 1.0013547198921082, 9.0604410672691205E-17, 1.0015241888698057,
            -7.17327634990032E-17, 1.0016936865283832, -1.3307196246722662E-17, 1.0018632128726943,
            2.5726925943221121E-17, 1.002032767907594, -3.9299377854845172E-17, 1.0022023516379379,
            8.4613772479947175E-17, 1.0023719640685822, -4.1948832416399403E-17, 1.0025416052043845,
            -3.6366159286922646E-17, 1.0027112750502025, -2.610944063243938E-17, 1.0028809736108952,
            1.7530784779823324E-17, 1.0030507008913223, 5.7539235256282674E-17, 1.0032204568963443,
            -8.6849220051179577E-18, 1.0033902416308227, 9.4900354309817764E-17, 1.0035600550996193,
            -8.7103806058184211E-17, 1.0037298973075977, 3.4958916958571545E-17, 1.0038997682596209,
            9.753787549840241E-17, 1.0040696679605541, -1.0576221196292857E-16, 1.0042395964152628,
            4.2091887381271259E-17, 1.0044095536286128, -1.6700166857554785E-17, 1.0045795396054717,
            -1.6231463554124514E-17, 1.0047495543507072, 2.3028539278028114E-17, 1.0049195978691881,
            1.6418046976773032E-17, 1.0050896701657839, 3.7266984318284131E-17, 1.005259771245365,
            9.499186535455033E-17, 1.0054299011128027, -8.6809313144445816E-17, 1.0056000597729693,
            4.0005474910301175E-17, 1.005770247230737, 7.190499111509974E-17, 1.0059404634909801,
            -1.3908068671065786E-17, 1.006110708558573, -8.1402086425730496E-17, 1.0062809824383909,
            -5.7621510437495342E-17, 1.00645128513531, 6.745278477310458E-17, 1.0066216166542072,
            1.8998557240346293E-17, 1.0067919769999607, -9.6374300323164059E-17, 1.0069623661774489,
            -1.2528654462453979E-17, 1.0071327841915512, 3.0205788878436942E-17, 1.0073032310471479,
            -4.8693942586085655E-17, 1.0074737067491204, 5.2240299376874538E-17, 1.0076442113023503,
            -9.3615435514784559E-17, 1.0078147447117207, -8.6525132330619496E-17, 1.007985306982115,
            -3.2520587560843081E-17, 1.0081558981184175, -9.9172322680609155E-17, 1.0083265181255139,
            -7.1360474041625215E-17, 1.0084971670082898, -1.7268683712243217E-17, 1.0086678447716324,
            -6.6199546936739413E-17, 1.0088385514204294, 3.5654569015130198E-17, 1.0090092869595693,
            3.7173100137088179E-17, 1.0091800513939415, 7.0625724068255265E-17, 1.0093508447284363,
            -1.4321412303428819E-17, 1.0095216669679448, 1.566818801313411E-17, 1.0096925181173586,
            -1.1043695780393687E-16, 1.0098633981815708, -5.767317427160398E-17, 1.0100343071654745,
            4.8354849784403835E-18, 1.0102052450739643, 7.0151212897154409E-17, 1.0103762119119353,
            7.1618028736195726E-17, 1.0105472076842836, 1.0504659134084051E-16, 1.0107182323959061,
        ];

        static ReadOnlySpan<InlineArray2<double>> ExpM1T1 {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<double, InlineArray2<double>>(
                ref MemoryMarshal.GetReference(ExpM1T1_flat)), 64);
        }

        static ReadOnlySpan<double> ExpM1TZ_flat => [
            -1.0231869534531498E-17, -0.22119921692859512, -5.3138072323012962E-17, -0.21509100668250825,
            5.4265860447649417E-17, -0.20893488914970404, 2.6020337682703143E-17, -0.20273048858867559,
            -3.6618868309204172E-17, -0.19647742631093923, 5.0008114075382227E-17, -0.19017532065792075,
            6.5546978087008111E-18, -0.18382378697766022, -5.1493961899974031E-17, -0.17742243760133536,
            -2.7604408719539223E-17, -0.17097088181959963, 2.561592821517568E-17, -0.16446872585873495,
            -3.8967887440685524E-17, -0.15791557285661761, 2.2088650680117402E-17, -0.15131102283849607,
            1.7204900005057594E-17, -0.14465467269257748, 5.7637851580401739E-18, -0.13794611614542429,
            6.1465980117146972E-19, -0.13118494373715683, -2.1452399010158893E-17, -0.12437074279646176,
            -5.224526916735663E-17, -0.11750309741540454, 5.5302409450097923E-17, -0.11058158842404442,
            -4.7460497709066285E-17, -0.10360579336484954, -2.0811956998712977E-17, -0.096575286466913268,
            -3.3250483245775637E-17, -0.089489638619965839, 2.2920689673580445E-17, -0.082348417348184211,
            1.0614261758612887E-17, -0.075151186783795176, -1.167464604196626E-18, -0.067897507640472421,
            -2.1524470434470569E-17, -0.060586937186524192, 3.5480066918496995E-17, -0.053219029217871139,
            -3.3924571641036719E-17, -0.04579333403081165, -3.5877873473605866E-18, -0.038309398394574701,
            -4.8011517070832187E-17, -0.03076676552365587, 8.5965027336832306E-18, -0.023164975049937975,
            -4.7493026566356186E-17, -0.015503562994591547, -2.8192701381719798E-18, -0.0077820617397564851,
            0, 0,
            4.2294554952087231E-17, 0.0078430972064479354, 2.0530467874932267E-17, 0.015747708586685727,
            1.8124461803844703E-17, 0.023714316602357899, 2.1578125052084337E-17, 0.031743407499102649,
            1.1038442468719412E-18, 0.039835471336229999, -5.3279008988776141E-17, 0.047991002016632756,
            2.011958971782554E-17, 0.056210497316931951, -2.2934210303960824E-18, 0.064494458917859432,
            -2.9761749354735218E-17, 0.072843392434877474, -5.0883253042401801E-17, 0.081257807449039654,
            4.0889548002981385E-17, 0.08973821753809319, -2.0315852964578147E-17, 0.098285140307825869,
            4.1845877976825521E-17, 0.10689909742365744, 5.2982113181689629E-17, 0.11558061464248071,
            -1.4900036878700333E-17, 0.12433022184475073, -5.3707377085580312E-18, 0.13314845306682632,
            -1.2069701773647767E-17, 0.1420358465335656, 3.7613173622701076E-17, 0.15099294469117641,
            -9.0029412145154113E-18, 0.16002029424032516, -4.1567420789311541E-17, 0.16911844616950444,
            5.4390762841681123E-17, 0.17828795578866319, 6.4158162077592173E-19, 0.1875293827631006,
            -5.89991778046089E-18, 0.19684329114762478, 3.9295715071105525E-17, 0.20623024942098067,
            -4.2223555445971666E-17, 0.21569083052054749, -2.8228512450696971E-17, 0.22522561187730761,
            -3.104366491258746E-19, 0.234835175451091, 3.66171795099051E-17, 0.24452010776609512,
            1.3050032175111173E-17, 0.25428099994668374, -1.5414979336037951E-17, 0.26411844775346638,
            -3.1874729281059959E-17, 0.27403305161966096, -2.1332574644578409E-17, 0.28402541668774151,
        ];

        static ReadOnlySpan<InlineArray2<double>> ExpM1TZ {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<double, InlineArray2<double>>(
                ref MemoryMarshal.GetReference(ExpM1TZ_flat)), 65);
        }

        static ReadOnlySpan<double> ExpM1AccurateCL => [
            1.1470745598771441E-11, 7.6471637304014369E-13, 4.7794758077432926E-14, 2.811458162324019E-15,
            1.5631408654506689E-16, 8.2206346583945647E-18,
        ];

        static ReadOnlySpan<double> ExpM1AccurateCHa_flat => [
            0.16666666666666666, 9.2518585385429691E-18, 0.041666666666666664, 2.312964634635329E-18,
            0.0083333333333333332, 1.1564823173247946E-19, 0.0013888888888888889, -5.3005439427021786E-20,
            0.00019841269841269841, 1.7209547147978358E-22, 2.4801587301587302E-05, 2.1496993363906636E-23,
            2.7557319223985893E-06, -1.8583040755160046E-22, 2.7557319223985888E-07, 2.4805440528173178E-23,
            2.5052108385441717E-08, 1.4639725357390741E-24, 2.0876756987867674E-09, 1.8498580855208408E-25,
            1.6059043836822621E-10, 1.2583393781282993E-26,
        ];

        static ReadOnlySpan<InlineArray2<double>> ExpM1AccurateCHa {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<double, InlineArray2<double>>(
                ref MemoryMarshal.GetReference(ExpM1AccurateCHa_flat)), 11);
        }

        static ReadOnlySpan<double> ExpM1AccurateCHb_flat => [
            1, 0, 0.5, 2.2752803697148587E-30, 0.16666666666666666, 9.251858538539695E-18,
            0.041666666666666664, 2.3113756024311491E-18, 0.0083333333333333332, 1.1645814375470794E-19,
            0.0013888888892440137, -7.0227927062705387E-20, 0.0001984126983733902, -1.0602991841308864E-20,
        ];

        static ReadOnlySpan<InlineArray2<double>> ExpM1AccurateCHb {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<double, InlineArray2<double>>(
                ref MemoryMarshal.GetReference(ExpM1AccurateCHb_flat)), 7);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
        static double AsExpM1Accurate(double x) {
            unchecked {
                if (Misc.Unlikely(double.Abs(x) < 0.25)) {
                    var cl = ExpM1AccurateCL;
                    var ch = ExpM1AccurateCHa;

                    double fl = x * (cl[0] + x * (cl[1] + x * (cl[2] + x * (cl[3] + x * (cl[4] + x * (cl[5]))))));
                    double fh = EvalPolyOddDD(x, 11, new(ref MemoryMarshal.GetReference(ch)), out fl);
                    fh = MultiplyHighDD(fh, fl, x, out fl);
                    fh = MultiplyHighDD(fh, fl, x, out fl);
                    fh = MultiplyHighDD(fh, fl, x, out fl);
                    double hx = 0.5 * x, x2h = x * hx, x2l = double.FusedMultiplyAdd(x, hx, -x2h);
                    fh = AddDDPartial(x2h, x2l, fh, fl, out fl);
                    double v2, v0 = BigAddDDPartial(x, fh, out v2), v1 = BigAddDDPartial(v2, fl, out v2);
                    v0 = BigAddDDPartial(v0, v1, out v1);
                    v1 = BigAddDDPartial(v1, v2, out v2);
                    UInt64 ix = BitConverter.DoubleToUInt64Bits(v1);
                    if ((ix & (~(UInt64)0 >> 12)) == 0) {
                        if (0 == (ix << 1)) return AsExpM1Db(x, v0);
                        Int64 d = ((((Int64)ix >> 63) ^ (BitConverter.DoubleToInt64Bits(v2) >> 63)) << 1) + 1;
                        ix += (UInt64)d;
                        v1 = BitConverter.UInt64BitsToDouble(ix);
                    }
                    return v0 + v1;
                } else {
                    var ch = ExpM1AccurateCHb;
                    const double s = 5909.278887481194;
                    double t = RoundPartial(x * s);
                    Int64 jt = (Int64)t;
                    int i0 = ((int)jt >> 6) & 0x3f, i1 = (int)jt & 0x3f;
                    Int64 ie = jt >> 12;
                    double t0h = ExpM1T0[i0][1], t0l = ExpM1T0[i0][0];
                    double t1h = ExpM1T1[i1][1], t1l = ExpM1T1[i1][0];
                    double tl, th = MultiplyDD(t0h, t0l, t1h, t1l, out tl);

                    const double l2h = 0.00016922538588914904, l2l = 1.0256140314162804E-14, l2ll = 3.2042720746546034E-31;
                    double dx = x - l2h * t, dxl = l2l * t, dxll = l2ll * t + double.FusedMultiplyAdd(l2l, t, -dxl);
                    double dxh = dx + dxl; dxl = (dx - dxh) + dxl + dxll;
                    double fl, fh = EvalPolyOddDD(dxh, dxl, 7, new(ref MemoryMarshal.GetReference(ch)), out fl);
                    fh = MultiplyDD(dxh, dxl, fh, fl, out fl);
                    fh = MultiplyDD(fh, fl, th, tl, out fl);
                    fh = AddDDPartial(th, tl, fh, fl, out fl);
                    var off = BitConverter.UInt64BitsToDouble((UInt64)(2048 + 1023 - ie) << 52);
                    double e;
                    if (Misc.Likely(ie < 53))
                        fh = BigAddDDPartial(off, fh, out e);
                    else {
                        if (ie < 104)
                            fh = BigAddDDPartial(fh, off, out e);
                        else
                            e = 0;
                    }
                    fl += e;
                    fh = BigAddDDPartial(fh, fl, out fl);
                    UInt64 d = (BitConverter.DoubleToUInt64Bits(fl) + 8) & (~(UInt64)0 >> 12);
                    if (Misc.Unlikely(d <= 8)) fh = AsExpM1Db(x, fh);
                    fh = AsExpLD(fh, ie);
                    return fh;
                }
            }
        }

        public static double ExpM1(double x) {
            unchecked {
                UInt64 ix = BitConverter.DoubleToUInt64Bits(x);
                UInt64 aix = ix & (~(UInt64)0 >> 1);
                if (Misc.Likely(aix < 0x3fd0000000000000UL)) { // |x| < 0.25
                    if (Misc.Unlikely(aix < 0x3ca0000000000000UL)) { // |x| < 2^-53
                        if (aix == 0) return x;
                        double res = double.FusedMultiplyAdd(5.5511151231257827E-17, double.Abs(x), x);
                        /* we have underflow for |x| < 2^-1022 and for x=-2.2250738585072014E-308 and
                           rounding towards zero */
                        if (aix < 0x10000000000000UL || double.Abs(res) < 2.2250738585072014E-308)
                            RaiseFloatingPointExceptionFlagsDummy(FloatingPointExceptionFlags.Underflow);
                        return res;
                    }
                    double sx = 128 * x, fx = RoundPartial(sx), z = sx - fx, z2 = z * z;
                    int i = (int)(Int64)fx;
                    var tz = ExpM1TZ;
                    double th = tz[i + 32][1], tl = tz[i + 32][0];
                    double fh = z * 0.0078125;
                    double fl = z2 * ((3.0517578125E-05 + z * 7.9472859700508441E-08)
                                      + z2 * (1.5522042910260805E-10
                                              + z * (2.4253205264225237E-13
                                                     + z * 3.1579677127348593E-16)));
                    double e0 = 4.4045713257223618E-20, eps = z2 * e0 + 4.9303806576313238E-32;
                    double rl;
                    double rh = BigAddDDPartial(th, fh, out rl);
                    rl += tl + fl;
                    fh = MultiplyDD(th, tl, fh, fl, out fl);
                    fh = AddDDPartial(rh, rl, fh, fl, out fl);
                    double ub = fh + (fl + eps), lb = fh + (fl - eps);
                    if (Misc.Unlikely(ub != lb)) return AsExpM1Accurate(x);
                    return lb;
                } else { // |x| >= 0.25
                    if (Misc.Unlikely(aix >= 0x40862e42fefa39f0UL)) {
                        // |x| >= 709.78271289338409
                        if (aix > 0x7ff0000000000000UL) return x + x; // nan
                        if (aix == 0x7ff0000000000000UL) { // +/-inf
                            if ((ix >> 63) != 0) // -inf
                                return -1.0;
                            else
                                return x; // +inf
                        }
                        if ((ix >> 63) == 0) { // x >= 709.78271289338409
                            RaiseFloatingPointExceptionFlagsDummy(FloatingPointExceptionFlags.Overflow);
                            double z = 8.9884656743115795E+307;
                            return z * z;
                        }
                    }
                    if (Misc.Unlikely(ix >= 0xc0425e4f7b2737faUL)) {
                        // x <= -36.736800569677101
                        if (ix >= 0xc042b708872320e2UL) // x <= -37.429947750237048
                            return -1.0 + 2.7755575615628914E-17;
                        return (36.736800569677101 + x + 6.7398329902596056E-16) * 8.0085662595372941E-17
                               - 0.99999999999999989;
                    }

                    const double s = 5909.278887481194; // s approximates 2^12/log(2)
                    double t = RoundPartial(x * s);
                    Int64 jt = (Int64)t;
                    int i0 = ((int)(jt >> 6)) & 0x3f;
                    int i1 = ((int)jt) & 0x3f;
                    Int64 ie = jt >> 12;
                    var t0s = ExpM1T0;
                    var t1s = ExpM1T1;
                    double t0h = t0s[i0][1], t0l = t0s[i0][0];
                    double t1h = t1s[i1][1], t1l = t1s[i1][0];
                    double tl;
                    double th = MultiplyDD(t0h, t0l, t1h, t1l, out tl);
                    const double l2h = 0.00016922538588914904, l2l = 1.0256140314162804E-14;
                    double dx = (x - l2h * t) + l2l * t, dx2 = dx * dx;
                    double p = (1 + dx * 0.5) + dx2 * (0.16666666674124284 + dx * 0.041666666654270573);
                    double fh = th, tx = th * dx, fl = tl + tx * p;
                    double eps = 1.64e-19 * th;
                    double off = BitConverter.UInt64BitsToDouble((UInt64)(2048 + 1023 - ie) << 52);
                    double e;
                    if (Misc.Likely(ie < 53)) {
                        fh = BigAddDDPartial(off, fh, out e);
                    } else if (ie < 75) {
                        fh = BigAddDDPartial(fh, off, out e);
                    } else {
                        e = 0;
                    }
                    fl += e;
                    double ub = fh + (fl + eps), lb = fh + (fl - eps);
                    if (Misc.Unlikely(ub != lb)) return AsExpM1Accurate(x);
                    return AsExpLD(lb, ie);
                }
            }
        }

        private static void RaiseFloatingPointExceptionFlagsDummy(FloatingPointExceptionFlags flags) {
        }
    }
}
