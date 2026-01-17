using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Mathematics.Elementary;
using UltimateOrb.Numerics;
using Math = UltimateOrb.Mathematics.Elementary.Math;

namespace UltimateOrb {

    partial struct Decimal128Bid {

        // Numerator coefficients:
        static readonly Decimal128Bid Exp10Internal_50A1_RI8_P8 = new(UInt128.FromLoHi(0X88c532fdb22a7fdfUL, 0X2ff1c33258b2a1dbUL), CtorFromBits); // 9.151355590006627443883508444594143E-7
        static readonly Decimal128Bid Exp10Internal_50A1_RI8_P7 = new(UInt128.FromLoHi(0Xee8516e730fb134cUL, 0X2ff49d54f9247e98UL), CtorFromBits); // 3.191070579311124751075853196792652E-5
        static readonly Decimal128Bid Exp10Internal_50A1_RI8_P6 = new(UInt128.FromLoHi(0Xd8c8c0a13330c515UL, 0X2ff70576af1974e0UL), CtorFromBits); // 5.303112020381575601871522876146965E-4
        static readonly Decimal128Bid Exp10Internal_50A1_RI8_P5 = new(UInt128.FromLoHi(0X044fe8ec8f2f145aUL, 0X2ff90d0b4f2091f4UL), CtorFromBits); // 0.005456864181860602821976988138280026
        static readonly Decimal128Bid Exp10Internal_50A1_RI8_P4 = new(UInt128.FromLoHi(0Xb5c5adb03ab85b6eUL, 0X2ffaba91a72759f8UL), CtorFromBits); // 0.03784068001413404664974009544760174
        static readonly Decimal128Bid Exp10Internal_50A1_RI8_P3 = new(UInt128.FromLoHi(0X1b63dd8db433cb8bUL, 0X2ffc58e1672ddf63UL), CtorFromBits); // 0.1802710314099645881474187546315659
        static readonly Decimal128Bid Exp10Internal_50A1_RI8_P2 = new(UInt128.FromLoHi(0X5575e6b1c057cb80UL, 0X2ffd1ae88f1610c8UL), CtorFromBits); // 0.5738064724965106240363869389704064
        static readonly Decimal128Bid Exp10Internal_50A1_RI8_P1 = new(UInt128.FromLoHi(0X58783ee3f794c41cUL, 0X2ffe36ce24df3e07UL), CtorFromBits); // 1.111582531418862222983057020077084
        static readonly Decimal128Bid Exp10Internal_50A1_RI8_P0 = new(UInt128.FromLoHi(0X377fdf581a855655UL, 0X2ffded09bead87c0UL), CtorFromBits); // 0.9999999999999999996148359673239125

        // Denominator coefficients:
        static readonly Decimal128Bid Exp10Internal_50A1_RI8_Q8 = new(UInt128.FromLoHi(0X281b1ba0afc39cd1UL, 0X2ff27f2a1c8d9f56UL), CtorFromBits); // 2.579202439280622390045443142950097E-6
        static readonly Decimal128Bid Exp10Internal_50A1_RI8_Q7 = new(UInt128.FromLoHi(0Xcb8d097ff7beeba5UL, 0Xaff55fdcf938aac4UL), CtorFromBits); // -7.136633096908598417733704520493989E-5
        static readonly Decimal128Bid Exp10Internal_50A1_RI8_Q6 = new(UInt128.FromLoHi(0Xe91f93e76d9cefdeUL, 0X2ff7e663ce167971UL), CtorFromBits); // 9.865158436545522848881437675679710E-4
        static readonly Decimal128Bid Exp10Internal_50A1_RI8_Q5 = new(UInt128.FromLoHi(0Xc83c0563f92cbc4dUL, 0Xaff9ae9360e9a5feUL), CtorFromBits); // -0.008733112662484364330340211114163277
        static readonly Decimal128Bid Exp10Internal_50A1_RI8_Q4 = new(UInt128.FromLoHi(0X5cc9d0700a9bc2b2UL, 0X2ffb0767cfbf60e4UL), CtorFromBits); // 0.05342498521258782839202253673054898
        static readonly Decimal128Bid Exp10Internal_50A1_RI8_Q3 = new(UInt128.FromLoHi(0X191cbfceabd45573UL, 0Xaffc70dae123b316UL), CtorFromBits); // -0.2288971292322421440565301682132339
        static readonly Decimal128Bid Exp10Internal_50A1_RI8_Q2 = new(UInt128.FromLoHi(0X48c6535055b179baUL, 0X2ffd47fd5d887492UL), CtorFromBits); // 0.6652421612580522939340938013669818
        static readonly Decimal128Bid Exp10Internal_50A1_RI8_Q1 = new(UInt128.FromLoHi(0Xbb9f263d4369db1cUL, 0Xaffe3ab890d46f9cUL), CtorFromBits); // -1.191002561575183445938114331794204
                                                                                                                                                   // static readonly Decimal128Bid Exp10Internal_50A1_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Numerator coefficients:
        static readonly Decimal128Bid Exp10Internal_40A1_RI8_P8 = new(UInt128.FromLoHi(0X09a5f45b7e57fa27UL, 0X2ff233e0a374a75bUL), CtorFromBits); // 1.052200585271740909365779683080743E-6
        static readonly Decimal128Bid Exp10Internal_40A1_RI8_P7 = new(UInt128.FromLoHi(0Xddbde6db08d52fe7UL, 0X2ff4afb69d258b8eUL), CtorFromBits); // 3.563889840752480067508092070604775E-5
        static readonly Decimal128Bid Exp10Internal_40A1_RI8_P6 = new(UInt128.FromLoHi(0Xfd8eaf27be595bb0UL, 0X2ff71d0911eca18fUL), CtorFromBits); // 5.781205337817972000659838930082736E-4
        static readonly Decimal128Bid Exp10Internal_40A1_RI8_P5 = new(UInt128.FromLoHi(0Xa122586ac3c47a73UL, 0X2ff91f65a9d86718UL), CtorFromBits); // 0.005829106165243465034379672657623667
        static readonly Decimal128Bid Exp10Internal_40A1_RI8_P4 = new(UInt128.FromLoHi(0X3b055b9c8e1936bdUL, 0X2ffac3e2098de5ffUL), CtorFromBits); // 0.03972978394350058527956282858092221
        static readonly Decimal128Bid Exp10Internal_40A1_RI8_P3 = new(UInt128.FromLoHi(0Xf5e105951a749948UL, 0X2ffc5bf1db00b003UL), CtorFromBits); // 0.1864861039146600147175120376731976
        static readonly Decimal128Bid Exp10Internal_40A1_RI8_P2 = new(UInt128.FromLoHi(0X5e76b6adb27e24a9UL, 0X2ffd20ee256c8413UL), CtorFromBits); // 0.5860201850663137573296503122699433
        static readonly Decimal128Bid Exp10Internal_40A1_RI8_P1 = new(UInt128.FromLoHi(0X56993770b2c39d07UL, 0X2ffe375920be9b93UL), CtorFromBits); // 1.122593968615516125488012805250311
        static readonly Decimal128Bid Exp10Internal_40A1_RI8_P0 = new(UInt128.FromLoHi(0X378d82e19f02e69fUL, 0X2ffded09bead87c0UL), CtorFromBits); // 0.9999999999999999999987345399146143

        // Denominator coefficients:
        static readonly Decimal128Bid Exp10Internal_40A1_RI8_Q8 = new(UInt128.FromLoHi(0Xa66d4b2e2741ac75UL, 0X2ff26da47be177fdUL), CtorFromBits); // 2.223814404681529496346789708672117E-6
        static readonly Decimal128Bid Exp10Internal_40A1_RI8_Q7 = new(UInt128.FromLoHi(0X197fcbb5e5b11ceaUL, 0Xaff53a5aa654c880UL), CtorFromBits); // -6.375858627181143811639633807219946E-5
        static readonly Decimal128Bid Exp10Internal_40A1_RI8_Q6 = new(UInt128.FromLoHi(0X101c836f949c179fUL, 0X2ff7be6adcc9c518UL), CtorFromBits); // 9.054421199082157547251381649020831E-4
        static readonly Decimal128Bid Exp10Internal_40A1_RI8_Q5 = new(UInt128.FromLoHi(0X2c629d4017fe78f6UL, 0Xaff993bb09562f54UL), CtorFromBits); // -0.008188629626218003440433111454611702
        static readonly Decimal128Bid Exp10Internal_40A1_RI8_Q4 = new(UInt128.FromLoHi(0X6d9b9a026dc184f6UL, 0X2ffafb5573b21ab3UL), CtorFromBits); // 0.05097655010421298485012619287692534
        static readonly Decimal128Bid Exp10Internal_40A1_RI8_Q3 = new(UInt128.FromLoHi(0Xf6e7e6e9e7c327e6UL, 0Xaffc6d43abcbb20eUL), CtorFromBits); // -0.2216144101875965898416066260051942
        static readonly Decimal128Bid Exp10Internal_40A1_RI8_Q2 = new(UInt128.FromLoHi(0Xd16f3a4f12e8f777UL, 0X2ffd4182bb07aa99UL), CtorFromBits); // 0.6521011026863997644080589016987511
        static readonly Decimal128Bid Exp10Internal_40A1_RI8_Q1 = new(UInt128.FromLoHi(0Xbdb390f1a0344432UL, 0Xaffe3a2d94f51210UL), CtorFromBits); // -1.179991124378529558460463538521138
                                                                                                                                                   // static readonly Decimal128Bid Exp10Internal_40A1_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Numerator coefficients:
        static readonly Decimal128Bid Exp10Internal_25A1_RI8_P8 = new(UInt128.FromLoHi(0X3e8e98fb6e01f7d9UL, 0X2ff23d73dfea0ff8UL), CtorFromBits); // 1.246407522633139041939399731443673E-6
        static readonly Decimal128Bid Exp10Internal_25A1_RI8_P7 = new(UInt128.FromLoHi(0X7800c16cdf3a3e0eUL, 0X2ff4c8c553c3caebUL), CtorFromBits); // 4.072115792700255817309719614864910E-5
        static readonly Decimal128Bid Exp10Internal_25A1_RI8_P6 = new(UInt128.FromLoHi(0Xcd564c09e4a2e831UL, 0X2ff73c2d4c8467f0UL), CtorFromBits); // 6.412830382996870122925809924761649E-4
        static readonly Decimal128Bid Exp10Internal_25A1_RI8_P5 = new(UInt128.FromLoHi(0X032621a4cff4b382UL, 0X2ff93706c5620126UL), CtorFromBits); // 0.006308365842737843784748847900963714
        static readonly Decimal128Bid Exp10Internal_25A1_RI8_P4 = new(UInt128.FromLoHi(0X3fa1878cf0a54437UL, 0X2ffacf9e968da149UL), CtorFromBits); // 0.04211023431604815511674212030170167
        static readonly Decimal128Bid Exp10Internal_25A1_RI8_P3 = new(UInt128.FromLoHi(0X19f4d6342725e2f7UL, 0X2ffc5fbc8bebda85UL), CtorFromBits); // 0.1941767110445454342727634711339767
        static readonly Decimal128Bid Exp10Internal_25A1_RI8_P2 = new(UInt128.FromLoHi(0X617fb718eebb3c16UL, 0X2ffd28445cfecd79UL), CtorFromBits); // 0.6009009538390250376863337982671894
        static readonly Decimal128Bid Exp10Internal_25A1_RI8_P1 = new(UInt128.FromLoHi(0X2a1f60a8060f19f3UL, 0X2ffe380035d5f1d7UL), CtorFromBits); // 1.135831599153273060544083391355379
        static readonly Decimal128Bid Exp10Internal_25A1_RI8_P0 = new(UInt128.FromLoHi(0X378d8e63f50a2bd2UL, 0X2ffded09bead87c0UL), CtorFromBits); // 0.9999999999999999999999999816117202

        // Denominator coefficients:
        static readonly Decimal128Bid Exp10Internal_25A1_RI8_Q8 = new(UInt128.FromLoHi(0Xf05b6c5163a21dd6UL, 0X2ff25bf29780f328UL), CtorFromBits); // 1.864919377388032156710910279425494E-6
        static readonly Decimal128Bid Exp10Internal_25A1_RI8_Q7 = new(UInt128.FromLoHi(0Xfc3963430b8aafafUL, 0Xaff512a7b538128fUL), CtorFromBits); // -5.570667419114722058177446578663343E-5
        static readonly Decimal128Bid Exp10Internal_25A1_RI8_Q6 = new(UInt128.FromLoHi(0Xcd6380839dba681bUL, 0X2ff792a183ec6ea8UL), CtorFromBits); // 8.166325223198122259969991193028635E-4
        static readonly Decimal128Bid Exp10Internal_25A1_RI8_Q5 = new(UInt128.FromLoHi(0Xc9cbac12fbddc820UL, 0Xaff97587b4bb62d5UL), CtorFromBits); // -0.007576090517939130476469424089843744
        static readonly Decimal128Bid Exp10Internal_25A1_RI8_Q4 = new(UInt128.FromLoHi(0X492dd7b9d0024a58UL, 0X2ffaed75b252c65bUL), CtorFromBits); // 0.04816255959479992716222597634869848
        static readonly Decimal128Bid Exp10Internal_25A1_RI8_Q3 = new(UInt128.FromLoHi(0Xd65647b16d919171UL, 0Xaffc69107735815aUL), CtorFromBits); // -0.2130957552383751731422100175556977
        static readonly Decimal128Bid Exp10Internal_25A1_RI8_Q2 = new(UInt128.FromLoHi(0Xa21f73a611aea3e0UL, 0X2ffd39d1bbc7cfdaUL), CtorFromBits); // 0.6365011007163091718436798400996320
        static readonly Decimal128Bid Exp10Internal_25A1_RI8_Q1 = new(UInt128.FromLoHi(0Xea2da6f31548d7ecUL, 0Xaffe39867fddbbccUL), CtorFromBits); // -1.166753493840772623473906064873452
                                                                                                                                                   // static readonly Decimal128Bid Exp10Internal_25A1_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Numerator coefficients:
        static readonly Decimal128Bid Exp10Internal_10A_RI8_P8 = new(UInt128.FromLoHi(0X328c11c856f47deeUL, 0X2ff24b0f77395094UL), CtorFromBits); // 1.522406040717052784015219629522414E-6
        static readonly Decimal128Bid Exp10Internal_10A_RI8_P7 = new(UInt128.FromLoHi(0Xefd5a3832f603e85UL, 0X2ff4eabac284b8a2UL), CtorFromBits); // 4.760880486024178663357324246138501E-5
        static readonly Decimal128Bid Exp10Internal_10A_RI8_P6 = new(UInt128.FromLoHi(0Xce9bbd0c657c696fUL, 0X2ff764d0d7fb8b24UL), CtorFromBits); // 7.237084120077541877212765650446703E-4
        static readonly Decimal128Bid Exp10Internal_10A_RI8_P5 = new(UInt128.FromLoHi(0Xf3177b8e80a2cf81UL, 0X2ff954ee2827ba01UL), CtorFromBits); // 0.006914887995346858804425571076067201
        static readonly Decimal128Bid Exp10Internal_10A_RI8_P4 = new(UInt128.FromLoHi(0X57ad3d40495c8896UL, 0X2ffade19c658dc01UL), CtorFromBits); // 0.04504737021529889585715697510287510
        static readonly Decimal128Bid Exp10Internal_10A_RI8_P3 = new(UInt128.FromLoHi(0X378ecc9a180b05d0UL, 0X2ffc6451093f661dUL), CtorFromBits); // 0.2034661303538457821759981086246352
        static readonly Decimal128Bid Exp10Internal_10A_RI8_P2 = new(UInt128.FromLoHi(0X1462509f468e1174UL, 0X2ffd30f86658e1c7UL), CtorFromBits); // 0.6185532778736324097428952456237428
        static readonly Decimal128Bid Exp10Internal_10A_RI8_P1 = new(UInt128.FromLoHi(0X0a2683cdc93afa62UL, 0X2ffe38c35ad9d6d2UL), CtorFromBits); // 1.151292546497022842008995727342178
                                                                                                                                                  // static readonly Decimal128Bid Exp10Internal_10A0_RI8_P0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Denominator coefficients:
        static readonly Decimal128Bid Exp10Internal_10A_RI8_Q8 = new(UInt128.FromLoHi(0X328c11c856f47deeUL, 0X2ff24b0f77395094UL), CtorFromBits); // 1.522406040717052784015219629522414E-6
        static readonly Decimal128Bid Exp10Internal_10A_RI8_Q7 = new(UInt128.FromLoHi(0Xefd5a3832f603e85UL, 0Xaff4eabac284b8a2UL), CtorFromBits); // -4.760880486024178663357324246138501E-5
        static readonly Decimal128Bid Exp10Internal_10A_RI8_Q6 = new(UInt128.FromLoHi(0Xce9bbd0c657c696fUL, 0X2ff764d0d7fb8b24UL), CtorFromBits); // 7.237084120077541877212765650446703E-4
        static readonly Decimal128Bid Exp10Internal_10A_RI8_Q5 = new(UInt128.FromLoHi(0Xf3177b8e80a2cf81UL, 0Xaff954ee2827ba01UL), CtorFromBits); // -0.006914887995346858804425571076067201
        static readonly Decimal128Bid Exp10Internal_10A_RI8_Q4 = new(UInt128.FromLoHi(0X57ad3d40495c8896UL, 0X2ffade19c658dc01UL), CtorFromBits); // 0.04504737021529889585715697510287510
        static readonly Decimal128Bid Exp10Internal_10A_RI8_Q3 = new(UInt128.FromLoHi(0X378ecc9a180b05d0UL, 0Xaffc6451093f661dUL), CtorFromBits); // -0.2034661303538457821759981086246352
        static readonly Decimal128Bid Exp10Internal_10A_RI8_Q2 = new(UInt128.FromLoHi(0X1462509f468e1174UL, 0X2ffd30f86658e1c7UL), CtorFromBits); // 0.6185532778736324097428952456237428
        static readonly Decimal128Bid Exp10Internal_10A_RI8_Q1 = new(UInt128.FromLoHi(0X0a2683cdc93afa62UL, 0Xaffe38c35ad9d6d2UL), CtorFromBits); // -1.151292546497022842008995727342178
                                                                                                                                                  // static readonly Decimal128Bid Exp10Internal_10A0_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Numerator coefficients:
        static readonly Decimal128Bid Exp10Internal_25A0_RI8_P8 = new(UInt128.FromLoHi(0Xf05b6c5165ad6189UL, 0X2ff25bf29780f328UL), CtorFromBits); // 1.864919377388032156710910313718153E-6
        static readonly Decimal128Bid Exp10Internal_25A0_RI8_P7 = new(UInt128.FromLoHi(0Xfc39634311a5b85eUL, 0X2ff512a7b538128fUL), CtorFromBits); // 5.570667419114722058177446681098334E-5
        static readonly Decimal128Bid Exp10Internal_25A0_RI8_P6 = new(UInt128.FromLoHi(0Xcd638083a6adbcdcUL, 0X2ff792a183ec6ea8UL), CtorFromBits); // 8.166325223198122259969991343193308E-4
        static readonly Decimal128Bid Exp10Internal_25A0_RI8_P5 = new(UInt128.FromLoHi(0Xc9cbac13042b80c8UL, 0X2ff97587b4bb62d5UL), CtorFromBits); // 0.007576090517939130476469424229155016
        static readonly Decimal128Bid Exp10Internal_25A0_RI8_P4 = new(UInt128.FromLoHi(0X492dd7b9d549a63eUL, 0X2ffaed75b252c65bUL), CtorFromBits); // 0.04816255959479992716222597723432510
        static readonly Decimal128Bid Exp10Internal_25A0_RI8_P3 = new(UInt128.FromLoHi(0Xd65647b16fe77a75UL, 0X2ffc69107735815aUL), CtorFromBits); // 0.2130957552383751731422100214741621
        static readonly Decimal128Bid Exp10Internal_25A0_RI8_P2 = new(UInt128.FromLoHi(0Xa21f73a618a88da3UL, 0X2ffd39d1bbc7cfdaUL), CtorFromBits); // 0.6365011007163091718436798518037923
        static readonly Decimal128Bid Exp10Internal_25A0_RI8_P1 = new(UInt128.FromLoHi(0Xea2da6f3169036eaUL, 0X2ffe39867fddbbccUL), CtorFromBits); // 1.166753493840772623473906086328042
        static readonly Decimal128Bid Exp10Internal_25A0_RI8_P0 = new(UInt128.FromLoHi(0X38c15b0a01189538UL, 0X2ffe314dc6448d93UL), CtorFromBits); // 1.000000000000000000000000018388280

        // Denominator coefficients:
        static readonly Decimal128Bid Exp10Internal_25A0_RI8_Q8 = new(UInt128.FromLoHi(0X3e8e98fb6f5fb054UL, 0X2ff23d73dfea0ff8UL), CtorFromBits); // 1.246407522633139041939399754362964E-6
        static readonly Decimal128Bid Exp10Internal_25A0_RI8_Q7 = new(UInt128.FromLoHi(0X7800c16ce3b0cef2UL, 0Xaff4c8c553c3caebUL), CtorFromBits); // -4.072115792700255817309719689744114E-5
        static readonly Decimal128Bid Exp10Internal_25A0_RI8_Q6 = new(UInt128.FromLoHi(0Xcd564c09ebaa3cc9UL, 0X2ff73c2d4c8467f0UL), CtorFromBits); // 6.412830382996870122925810042682569E-4
        static readonly Decimal128Bid Exp10Internal_25A0_RI8_Q5 = new(UInt128.FromLoHi(0X032621a4d6deb87eUL, 0Xaff93706c5620126UL), CtorFromBits); // -0.006308365842737843784748848016963710
        static readonly Decimal128Bid Exp10Internal_25A0_RI8_Q4 = new(UInt128.FromLoHi(0X3fa1878cf542cebcUL, 0X2ffacf9e968da149UL), CtorFromBits); // 0.04211023431604815511674212107603644
        static readonly Decimal128Bid Exp10Internal_25A0_RI8_Q3 = new(UInt128.FromLoHi(0X19f4d6342946b694UL, 0Xaffc5fbc8bebda85UL), CtorFromBits); // -0.1941767110445454342727634747045524
        static readonly Decimal128Bid Exp10Internal_25A0_RI8_Q2 = new(UInt128.FromLoHi(0X617fb718f551428bUL, 0X2ffd28445cfecd79UL), CtorFromBits); // 0.6009009538390250376863338093167243
        static readonly Decimal128Bid Exp10Internal_25A0_RI8_Q1 = new(UInt128.FromLoHi(0X2a1f60a8074dcbd9UL, 0Xaffe380035d5f1d7UL), CtorFromBits); // -1.135831599153273060544083412241369
                                                                                                                                                   // static readonly Decimal128Bid Exp10Internal_25A0_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Numerator coefficients:
        static readonly Decimal128Bid Exp10Internal_40A0_RI8_P8 = new(UInt128.FromLoHi(0Xa66d4dbd5f91fcd4UL, 0X2ff26da47be177fdUL), CtorFromBits); // 2.223814404681529496349603857038548E-6
        static readonly Decimal128Bid Exp10Internal_40A0_RI8_P7 = new(UInt128.FromLoHi(0X197fd30c777c6df2UL, 0X2ff53a5aa654c880UL), CtorFromBits); // 6.375858627181143811647702201822706E-5
        static readonly Decimal128Bid Exp10Internal_40A0_RI8_P6 = new(UInt128.FromLoHi(0X101c8ddb5b24c1fcUL, 0X2ff7be6adcc9c518UL), CtorFromBits); // 9.054421199082157547262839657644540E-4
        static readonly Decimal128Bid Exp10Internal_40A0_RI8_P5 = new(UInt128.FromLoHi(0X2c62a6acc63549f8UL, 0X2ff993bb09562f54UL), CtorFromBits); // 0.008188629626218003440443473838557688
        static readonly Decimal128Bid Exp10Internal_40A0_RI8_P4 = new(UInt128.FromLoHi(0X6d9b9fe0641a9ae9UL, 0X2ffafb5573b21ab3UL), CtorFromBits); // 0.05097655010421298485019070166637289
        static readonly Decimal128Bid Exp10Internal_40A0_RI8_P3 = new(UInt128.FromLoHi(0Xf6e7e976dd869ce9UL, 0X2ffc6d43abcbb20eUL), CtorFromBits); // 0.2216144101875965898418870701956329
        static readonly Decimal128Bid Exp10Internal_40A0_RI8_P2 = new(UInt128.FromLoHi(0Xd16f41d069285ca0UL, 0X2ffd4182bb07aa99UL), CtorFromBits); // 0.6521011026863997644088841096158368
        static readonly Decimal128Bid Exp10Internal_40A0_RI8_P1 = new(UInt128.FromLoHi(0Xbdb3924d4bbf5344UL, 0X2ffe3a2d94f51210UL), CtorFromBits); // 1.179991124378529558461956770190148
        static readonly Decimal128Bid Exp10Internal_40A0_RI8_P0 = new(UInt128.FromLoHi(0X38c15c30a34c828aUL, 0X2ffe314dc6448d93UL), CtorFromBits); // 1.000000000000000000001265460085386

        // Denominator coefficients:
        static readonly Decimal128Bid Exp10Internal_40A0_RI8_Q8 = new(UInt128.FromLoHi(0X09a5f59182fdde58UL, 0X2ff233e0a374a75bUL), CtorFromBits); // 1.052200585271740909367111200923224E-6
        static readonly Decimal128Bid Exp10Internal_40A0_RI8_Q7 = new(UInt128.FromLoHi(0Xddbdeaf5176aba8fUL, 0Xaff4afb69d258b8eUL), CtorFromBits); // -3.563889840752480067512602030946959E-5
        static readonly Decimal128Bid Exp10Internal_40A0_RI8_Q6 = new(UInt128.FromLoHi(0Xfd8eb5cf1b0d481bUL, 0X2ff71d0911eca18fUL), CtorFromBits); // 5.781205337817972000667154814683163E-4
        static readonly Decimal128Bid Exp10Internal_40A0_RI8_Q5 = new(UInt128.FromLoHi(0Xa1225f203d8016aaUL, 0Xaff91f65a9d86718UL), CtorFromBits); // -0.005829106165243465034387049158809258
        static readonly Decimal128Bid Exp10Internal_40A0_RI8_Q4 = new(UInt128.FromLoHi(0X3b05602f25209263UL, 0X2ffac3e2098de5ffUL), CtorFromBits); // 0.03972978394350058527961310503670371
        static readonly Decimal128Bid Exp10Internal_40A0_RI8_Q3 = new(UInt128.FromLoHi(0Xf5e107ba8fe2ebafUL, 0Xaffc5bf1db00b003UL), CtorFromBits); // -0.1864861039146600147177480283941807
        static readonly Decimal128Bid Exp10Internal_40A0_RI8_Q2 = new(UInt128.FromLoHi(0X5e76bd6c55b06c97UL, 0X2ffd20ee256c8413UL), CtorFromBits); // 0.5860201850663137573303918974233751
        static readonly Decimal128Bid Exp10Internal_40A0_RI8_Q1 = new(UInt128.FromLoHi(0X569938bb74feb139UL, 0Xaffe375920be9b93UL), CtorFromBits); // -1.122593968615516125489433403109689
                                                                                                                                                   // static readonly Decimal128Bid Exp10Internal_40A0_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Numerator coefficients:
        static readonly Decimal128Bid Exp10Internal_50A0_RI8_P8 = new(UInt128.FromLoHi(0X281ea322604f34bfUL, 0X2ff27f2a1c8d9f56UL), CtorFromBits); // 2.579202439280622391038859155551423E-6
        static readonly Decimal128Bid Exp10Internal_50A0_RI8_P7 = new(UInt128.FromLoHi(0Xcb96cd7ee06ea1ccUL, 0X2ff55fdcf938aac4UL), CtorFromBits); // 7.136633096908598420482478903828940E-5
        static readonly Decimal128Bid Exp10Internal_50A0_RI8_P6 = new(UInt128.FromLoHi(0Xe92d13b701166aaeUL, 0X2ff7e663ce167971UL), CtorFromBits); // 9.865158436545522852681141882088110E-4
        static readonly Decimal128Bid Exp10Internal_50A0_RI8_P5 = new(UInt128.FromLoHi(0Xc847f8a3f5d07f06UL, 0X2ff9ae9360e9a5feUL), CtorFromBits); // 0.008733112662484364333703892005060358
        static readonly Decimal128Bid Exp10Internal_50A0_RI8_P4 = new(UInt128.FromLoHi(0X5cd11ff091880864UL, 0X2ffb0767cfbf60e4UL), CtorFromBits); // 0.05342498521258782841259991948068964
        static readonly Decimal128Bid Exp10Internal_50A0_RI8_P3 = new(UInt128.FromLoHi(0X191fe1a503da385eUL, 0X2ffc70dae123b316UL), CtorFromBits); // 0.2288971292322421441446931095763038
        static readonly Decimal128Bid Exp10Internal_50A0_RI8_P2 = new(UInt128.FromLoHi(0X48cf6db0219601bdUL, 0X2ffd47fd5d887492UL), CtorFromBits); // 0.6652421612580522941903211549032893
        static readonly Decimal128Bid Exp10Internal_50A0_RI8_P1 = new(UInt128.FromLoHi(0Xbba0c773fa8ce6c4UL, 0X2ffe3ab890d46f9cUL), CtorFromBits); // 1.191002561575183446396845681338052
        static readonly Decimal128Bid Exp10Internal_50A0_RI8_P0 = new(UInt128.FromLoHi(0X38c2b957fd5910f7UL, 0X2ffe314dc6448d93UL), CtorFromBits); // 1.000000000000000000385164032676087

        // Denominator coefficients:
        static readonly Decimal128Bid Exp10Internal_50A0_RI8_Q8 = new(UInt128.FromLoHi(0X88d1b8c0de2545ffUL, 0X2ff1c33258b2a1dbUL), CtorFromBits); // 9.151355590006627447408281468093951E-7
        static readonly Decimal128Bid Exp10Internal_50A0_RI8_Q7 = new(UInt128.FromLoHi(0Xee8974bffc55c99fUL, 0Xaff49d54f9247e98UL), CtorFromBits); // -3.191070579311124752304938809674143E-5
        static readonly Decimal128Bid Exp10Internal_50A0_RI8_Q6 = new(UInt128.FromLoHi(0Xd8d002559ff21b77UL, 0X2ff70576af1974e0UL), CtorFromBits); // 5.303112020381575603914090887650167E-4
        static readonly Decimal128Bid Exp10Internal_50A0_RI8_Q5 = new(UInt128.FromLoHi(0X0457607d2b3bd192UL, 0Xaff90d0b4f2091f4UL), CtorFromBits); // -0.005456864181860602824078775952331154
        static readonly Decimal128Bid Exp10Internal_50A0_RI8_Q4 = new(UInt128.FromLoHi(0Xb5cadb43d724bf90UL, 0X2ffaba91a72759f8UL), CtorFromBits); // 0.03784068001413404666431496436105104
        static readonly Decimal128Bid Exp10Internal_50A0_RI8_Q3 = new(UInt128.FromLoHi(0X1b66550d240aa81fUL, 0Xaffc58e1672ddf63UL), CtorFromBits); // -0.1802710314099645882168526720641055
        static readonly Decimal128Bid Exp10Internal_50A0_RI8_Q2 = new(UInt128.FromLoHi(0X557dc0c3dc3ca59dUL, 0X2ffd1ae88f1610c8UL), CtorFromBits); // 0.5738064724965106242573965538928029
        static readonly Decimal128Bid Exp10Internal_50A0_RI8_Q1 = new(UInt128.FromLoHi(0X5879c448742f8e6bUL, 0Xaffe36ce24df3e07UL), CtorFromBits); // -1.111582531418862223411198630530667
                                                                                                                                                   // static readonly Decimal128Bid Exp10Internal_50A0_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        static Decimal128Bid Exp10Internal_50A1(Decimal128Bid x) {
            Debug.Assert(-0.50M <= x && x <= -0.40M);

            // Horner for numerator
            Decimal128Bid px = Exp10Internal_50A1_RI8_P8;
            px = FusedMultiplyAdd(px, x, Exp10Internal_50A1_RI8_P7);
            px = FusedMultiplyAdd(px, x, Exp10Internal_50A1_RI8_P6);
            px = FusedMultiplyAdd(px, x, Exp10Internal_50A1_RI8_P5);
            px = FusedMultiplyAdd(px, x, Exp10Internal_50A1_RI8_P4);
            px = FusedMultiplyAdd(px, x, Exp10Internal_50A1_RI8_P3);
            px = FusedMultiplyAdd(px, x, Exp10Internal_50A1_RI8_P2);
            px = FusedMultiplyAdd(px, x, Exp10Internal_50A1_RI8_P1);
            px = FusedMultiplyAdd(px, x, Exp10Internal_50A1_RI8_P0);

            // Horner for denominator
            Decimal128Bid qx = Exp10Internal_50A1_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_50A1_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_50A1_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_50A1_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_50A1_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_50A1_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_50A1_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_50A1_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        static Decimal128Bid Exp10Internal_40A1(Decimal128Bid x) {
            Debug.Assert(-0.40M <= x && x <= -0.25M);

            // Horner for numerator
            Decimal128Bid px = Exp10Internal_40A1_RI8_P8;
            px = FusedMultiplyAdd(px, x, Exp10Internal_40A1_RI8_P7);
            px = FusedMultiplyAdd(px, x, Exp10Internal_40A1_RI8_P6);
            px = FusedMultiplyAdd(px, x, Exp10Internal_40A1_RI8_P5);
            px = FusedMultiplyAdd(px, x, Exp10Internal_40A1_RI8_P4);
            px = FusedMultiplyAdd(px, x, Exp10Internal_40A1_RI8_P3);
            px = FusedMultiplyAdd(px, x, Exp10Internal_40A1_RI8_P2);
            px = FusedMultiplyAdd(px, x, Exp10Internal_40A1_RI8_P1);
            px = FusedMultiplyAdd(px, x, Exp10Internal_40A1_RI8_P0);

            // Horner for denominator
            Decimal128Bid qx = Exp10Internal_40A1_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_40A1_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_40A1_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_40A1_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_40A1_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_40A1_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_40A1_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_40A1_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        static Decimal128Bid Exp10Internal_25A1(Decimal128Bid x) {
            Debug.Assert(-0.25M <= x && x <= -0.10M);

            // Horner for numerator
            Decimal128Bid px = Exp10Internal_25A1_RI8_P8;
            px = FusedMultiplyAdd(px, x, Exp10Internal_25A1_RI8_P7);
            px = FusedMultiplyAdd(px, x, Exp10Internal_25A1_RI8_P6);
            px = FusedMultiplyAdd(px, x, Exp10Internal_25A1_RI8_P5);
            px = FusedMultiplyAdd(px, x, Exp10Internal_25A1_RI8_P4);
            px = FusedMultiplyAdd(px, x, Exp10Internal_25A1_RI8_P3);
            px = FusedMultiplyAdd(px, x, Exp10Internal_25A1_RI8_P2);
            px = FusedMultiplyAdd(px, x, Exp10Internal_25A1_RI8_P1);
            px = FusedMultiplyAdd(px, x, Exp10Internal_25A1_RI8_P0);

            // Horner for denominator
            Decimal128Bid qx = Exp10Internal_25A1_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_25A1_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_25A1_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_25A1_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_25A1_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_25A1_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_25A1_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_25A1_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        static Decimal128Bid Exp10Internal_10A(Decimal128Bid x) {
            Debug.Assert(-0.10M <= x && x <= 0.10M);

            // Horner for numerator
            Decimal128Bid px = Exp10Internal_10A_RI8_P8;
            px = FusedMultiplyAdd(px, x, Exp10Internal_10A_RI8_P7);
            px = FusedMultiplyAdd(px, x, Exp10Internal_10A_RI8_P6);
            px = FusedMultiplyAdd(px, x, Exp10Internal_10A_RI8_P5);
            px = FusedMultiplyAdd(px, x, Exp10Internal_10A_RI8_P4);
            px = FusedMultiplyAdd(px, x, Exp10Internal_10A_RI8_P3);
            px = FusedMultiplyAdd(px, x, Exp10Internal_10A_RI8_P2);
            px = FusedMultiplyAdd(px, x, Exp10Internal_10A_RI8_P1);
            px = FusedMultiplyAdd(px, x, One);

            // Horner for denominator
            Decimal128Bid qx = Exp10Internal_10A_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_10A_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_10A_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_10A_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_10A_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_10A_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_10A_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_10A_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        static Decimal128Bid Exp10Internal_25A0(Decimal128Bid x) {
            Debug.Assert(0.10M <= x && x <= 0.25M);

            // Horner for numerator
            Decimal128Bid px = Exp10Internal_25A0_RI8_P8;
            px = FusedMultiplyAdd(px, x, Exp10Internal_25A0_RI8_P7);
            px = FusedMultiplyAdd(px, x, Exp10Internal_25A0_RI8_P6);
            px = FusedMultiplyAdd(px, x, Exp10Internal_25A0_RI8_P5);
            px = FusedMultiplyAdd(px, x, Exp10Internal_25A0_RI8_P4);
            px = FusedMultiplyAdd(px, x, Exp10Internal_25A0_RI8_P3);
            px = FusedMultiplyAdd(px, x, Exp10Internal_25A0_RI8_P2);
            px = FusedMultiplyAdd(px, x, Exp10Internal_25A0_RI8_P1);
            px = FusedMultiplyAdd(px, x, Exp10Internal_25A0_RI8_P0);

            // Horner for denominator
            Decimal128Bid qx = Exp10Internal_25A0_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_25A0_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_25A0_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_25A0_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_25A0_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_25A0_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_25A0_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_25A0_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        static Decimal128Bid Exp10Internal_40A0(Decimal128Bid x) {
            Debug.Assert(0.25M <= x && x <= 0.40M);

            // Horner for numerator
            Decimal128Bid px = Exp10Internal_40A0_RI8_P8;
            px = FusedMultiplyAdd(px, x, Exp10Internal_40A0_RI8_P7);
            px = FusedMultiplyAdd(px, x, Exp10Internal_40A0_RI8_P6);
            px = FusedMultiplyAdd(px, x, Exp10Internal_40A0_RI8_P5);
            px = FusedMultiplyAdd(px, x, Exp10Internal_40A0_RI8_P4);
            px = FusedMultiplyAdd(px, x, Exp10Internal_40A0_RI8_P3);
            px = FusedMultiplyAdd(px, x, Exp10Internal_40A0_RI8_P2);
            px = FusedMultiplyAdd(px, x, Exp10Internal_40A0_RI8_P1);
            px = FusedMultiplyAdd(px, x, Exp10Internal_40A0_RI8_P0);

            // Horner for denominator
            Decimal128Bid qx = Exp10Internal_40A0_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_40A0_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_40A0_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_40A0_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_40A0_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_40A0_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_40A0_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_40A0_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        static Decimal128Bid Exp10Internal_50A0(Decimal128Bid x) {
            Debug.Assert(0.40M <= x && x <= 0.50M);

            // Horner for numerator
            Decimal128Bid px = Exp10Internal_50A0_RI8_P8;
            px = FusedMultiplyAdd(px, x, Exp10Internal_50A0_RI8_P7);
            px = FusedMultiplyAdd(px, x, Exp10Internal_50A0_RI8_P6);
            px = FusedMultiplyAdd(px, x, Exp10Internal_50A0_RI8_P5);
            px = FusedMultiplyAdd(px, x, Exp10Internal_50A0_RI8_P4);
            px = FusedMultiplyAdd(px, x, Exp10Internal_50A0_RI8_P3);
            px = FusedMultiplyAdd(px, x, Exp10Internal_50A0_RI8_P2);
            px = FusedMultiplyAdd(px, x, Exp10Internal_50A0_RI8_P1);
            px = FusedMultiplyAdd(px, x, Exp10Internal_50A0_RI8_P0);

            // Horner for denominator
            Decimal128Bid qx = Exp10Internal_50A0_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_50A0_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_50A0_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_50A0_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_50A0_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_50A0_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_50A0_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, Exp10Internal_50A0_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        // static readonly Decimal128Bid Value_Pt50 = Decimal128Bid.ToFinestCohort(0.50M);
        static readonly Decimal128Bid Value_Pt10 = Decimal128Bid.ToFinestCohort(0.10M);
        static readonly Decimal128Bid Value_Pt25 = Decimal128Bid.ToFinestCohort(0.25M);
        static readonly Decimal128Bid Value_Pt40 = Decimal128Bid.ToFinestCohort(0.40M);

        internal static Decimal128Bid Exp10Internal_B0(Decimal128Bid x) {
            Debug.Assert(-0.5M <= x && x <= 0.5M);
            if (x < -Value_Pt10) {
                if (x >= -Value_Pt25) {
                    return Exp10Internal_25A1(x);
                } else if (x >= -Value_Pt40) {
                    return Exp10Internal_40A1(x);
                } else {
                    return Exp10Internal_50A1(x);
                }
            } else {
                if (x <= Value_Pt25) {
                    if (x <= Value_Pt10) {
                        return Exp10Internal_10A(x);
                    } else {
                        return Exp10Internal_25A0(x);
                    }
                } else {
                    if (x <= Value_Pt40) {
                        return Exp10Internal_40A0(x);
                    } else {
                        return Exp10Internal_50A0(x);
                    }
                }
            }
        }

        static readonly Decimal128Bid OneHalf_A0 = Decimal128Bid.ToFinestCohort(OneHalf);

        public static Decimal128Bid Exp10(Decimal128Bid x) {
            // Preserve NaN payload exactly
            if (Decimal128Bid.IsNaN(x)) {
                return x;
            }

            // +Inf -> +Inf, -Inf -> +0 (coarsest cohort)
            if (Decimal128Bid.IsInfinity(x)) {
                if (Decimal128Bid.IsPositiveInfinity(x)) {
                    return x;
                } else {
                    // -Inf -> Zero
                    return Decimal128Bid.Zero;
                }
            }

            // Exact zero: exp(0) == 1 but we must preserve cohort rules described:
            if (Decimal128Bid.IsZero(x)) {
                // Determine preferred q-exponent for zero input:
                var qx = ILogBQuantum(x); // q-exponent of input zero (may be negative)
                if (qx >= 0) {
                    return Decimal128Bid.One;
                } else {
                    // prefer finest cohort
                    return Decimal128Bid.ToFinestCohort(Decimal128Bid.One);
                }
            }

            // For nonzero finite x, result is inexact; we will return ToFinestCohort at the end.
            var f = x % One;
            bool expIsInt = false;

            if (IsZero(f)) {
                expIsInt = true;
            } else {
                if (f >= OneHalf_A0) {
                    f -= 1;
                } else if (f < -OneHalf_A0) {
                    f += 1;
                }
            }
            var i = x - f;

            if (i <= -6177) {
                return default(Decimal128Bid); // finest cohort of zero
            } else if (i >= 6146) {
                return Decimal128Bid.PositiveInfinity;
            }

            Decimal128Bid y;
            if (expIsInt) {
                var qx = ILogBQuantum(f); // q-exponent of input zero (may be negative)
                if (qx >= 0) {
                    y = Decimal128Bid.One;
                } else {
                    // prefer finest cohort
                    y = Decimal128Bid.ToFinestCohort(Decimal128Bid.One);
                }
            } else {
                y = Decimal128Bid.ToFinestCohort(Exp10Internal_B0(f));
            }
            // TODO
            return Decimal128Bid.Scale10(y, (int)(decimal)i);
        }
    }
}
namespace UltimateOrb {

    partial struct Decimal128Bid {

        // Numerator coefficients:
        static readonly Decimal128Bid Exp2Internal_50A1_RI8_P8 = new(UInt128.FromLoHi(0X07ba5e4694a97fd1UL, 0X2fe9b1839c921502UL), CtorFromBits); // 8.792710703934450861850065774411729E-11
        static readonly Decimal128Bid Exp2Internal_50A1_RI8_P7 = new(UInt128.FromLoHi(0X775acc4e3c7f811aUL, 0X2fedd1ea840d25bbUL), CtorFromBits); // 9.449900723641882376090647445602586E-9
        static readonly Decimal128Bid Exp2Internal_50A1_RI8_P6 = new(UInt128.FromLoHi(0X1ade32ae2be4e1afUL, 0X2ff0f1cfc67cfc96UL), CtorFromBits); // 4.904522373252056849925596812468655E-7
        static readonly Decimal128Bid Exp2Internal_50A1_RI8_P5 = new(UInt128.FromLoHi(0X6c02dc368879cb8bUL, 0X2ff44e808f70af2bUL), CtorFromBits); // 1.592213546469967504985744685517707E-5
        static readonly Decimal128Bid Exp2Internal_50A1_RI8_P4 = new(UInt128.FromLoHi(0Xc47d45b2e6c33fd5UL, 0X2ff6ad1ed93fba8fUL), CtorFromBits); // 3.511300941597636989837964751355861E-4
        static readonly Decimal128Bid Exp2Internal_50A1_RI8_P3 = new(UInt128.FromLoHi(0Xf993eb147384669dUL, 0X2ff907f1beed5c26UL), CtorFromBits); // 0.005353426802028789007763361707091613
        static readonly Decimal128Bid Exp2Internal_50A1_RI8_P2 = new(UInt128.FromLoHi(0X8fd19b81d250aa12UL, 0X2ffb0e3ea887d37fUL), CtorFromBits); // 0.05481214896747243464961097611651602
        static readonly Decimal128Bid Exp2Internal_50A1_RI8_P1 = new(UInt128.FromLoHi(0Xb29125e2bb85509cUL, 0X2ffca918fb0a7bc8UL), CtorFromBits); // 0.3429706392328751292822846573990044
        static readonly Decimal128Bid Exp2Internal_50A1_RI8_P0 = new(UInt128.FromLoHi(0X378d8e63ffb0b437UL, 0X2ffded09bead87c0UL), CtorFromBits); // 0.9999999999999999999999999994803255

        // Denominator coefficients:
        static readonly Decimal128Bid Exp2Internal_50A1_RI8_Q8 = new(UInt128.FromLoHi(0X1d5d4457598761a6UL, 0X2fea3b3842ff9faaUL), CtorFromBits); // 1.201119678756973901614447774359974E-10
        static readonly Decimal128Bid Exp2Internal_50A1_RI8_Q7 = new(UInt128.FromLoHi(0Xf7af6e37bcd24f24UL, 0Xafee3b61d715f767UL), CtorFromBits); // -1.204413864212228380369878500134692E-8
        static readonly Decimal128Bid Exp2Internal_50A1_RI8_Q6 = new(UInt128.FromLoHi(0Xd5b219c551c67d1eUL, 0X2ff123920e1c2dffUL), CtorFromBits); // 5.913752873247004944564227754327326E-7
        static readonly Decimal128Bid Exp2Internal_50A1_RI8_Q5 = new(UInt128.FromLoHi(0X5dd716957f7af124UL, 0Xaff45a75446120d9UL), CtorFromBits); // -1.834707721744417052551420973871396E-5
        static readonly Decimal128Bid Exp2Internal_50A1_RI8_Q4 = new(UInt128.FromLoHi(0X12b66670f79a91b7UL, 0X2ff6c0162d2a9b10UL), CtorFromBits); // 3.895979641809024222295196348092855E-4
        static readonly Decimal128Bid Exp2Internal_50A1_RI8_Q3 = new(UInt128.FromLoHi(0X667c9ff504822c49UL, 0Xaff91ba42e8009b3UL), CtorFromBits); // -0.005752929727584529424376493512993865
        static readonly Decimal128Bid Exp2Internal_50A1_RI8_Q2 = new(UInt128.FromLoHi(0X71624b55e68e1013UL, 0X2ffb1a8eca07d4d6UL), CtorFromBits); // 0.05730952432746358712273339677544467
        static readonly Decimal128Bid Exp2Internal_50A1_RI8_Q1 = new(UInt128.FromLoHi(0Xc1f205a75b97fb6aUL, 0Xaffcaca67e4b90f6UL), CtorFromBits); // -0.3501765413270701801349474441231210
                                                                                                                                                  // static readonly Decimal128Bid Exp2Internal_50A1_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Numerator coefficients:
        static readonly Decimal128Bid Exp2Internal_40A1_RI8_P8 = new(UInt128.FromLoHi(0X82a2a9de5115dde7UL, 0X2fe9c4876fa31bcbUL), CtorFromBits); // 9.178379492812237481043856868105703E-11
        static readonly Decimal128Bid Exp2Internal_40A1_RI8_P7 = new(UInt128.FromLoHi(0X2074f06a0a35004dUL, 0X2fede1d403715a8dUL), CtorFromBits); // 9.772636455300740907107867958771789E-9
        static readonly Decimal128Bid Exp2Internal_40A1_RI8_P6 = new(UInt128.FromLoHi(0X5d1e5f8924412e9aUL, 0X2ff0f82eeb49cca0UL), CtorFromBits); // 5.033754895376484224090040021626522E-7
        static readonly Decimal128Bid Exp2Internal_40A1_RI8_P5 = new(UInt128.FromLoHi(0X57c2c6b492126b3bUL, 0X2ff45011937904e9UL), CtorFromBits); // 1.623985287654537919886061567372091E-5
        static readonly Decimal128Bid Exp2Internal_40A1_RI8_P4 = new(UInt128.FromLoHi(0Xcdd87cdea48c1e1cUL, 0X2ff6afa6f6855049UL), CtorFromBits); // 3.562649850095111027553504647192092E-4
        static readonly Decimal128Bid Exp2Internal_40A1_RI8_P3 = new(UInt128.FromLoHi(0Xcdee9a2af3a8f011UL, 0X2ff90a9d4f18c535UL), CtorFromBits); // 0.005407584255347378104312242905870353
        static readonly Decimal128Bid Exp2Internal_40A1_RI8_P2 = new(UInt128.FromLoHi(0Xf2e17aebd9c6cb08UL, 0X2ffb0fef8ecbb290UL), CtorFromBits); // 0.05515512726557102940846234838420232
        static readonly Decimal128Bid Exp2Internal_40A1_RI8_P1 = new(UInt128.FromLoHi(0Xa8dcda9ddc8147b3UL, 0X2ffca997482c9bc0UL), CtorFromBits); // 0.3439713012405750164059954509596595
        static readonly Decimal128Bid Exp2Internal_40A1_RI8_P0 = new(UInt128.FromLoHi(0X378d8e63ffffbcd5UL, 0X2ffded09bead87c0UL), CtorFromBits); // 0.9999999999999999999999999999982805

        // Denominator coefficients:
        static readonly Decimal128Bid Exp2Internal_40A1_RI8_Q8 = new(UInt128.FromLoHi(0Xc01a9f909597c671UL, 0X2fea38afcbdd8136UL), CtorFromBits); // 1.149742959484284913723927562798705E-10
        static readonly Decimal128Bid Exp2Internal_40A1_RI8_Q7 = new(UInt128.FromLoHi(0X1e52d3761b38338bUL, 0Xafee3968c0eb687dUL), CtorFromBits); // -1.164396782022516155234703348609931E-8
        static readonly Decimal128Bid Exp2Internal_40A1_RI8_Q6 = new(UInt128.FromLoHi(0X8fc336cc290b831aUL, 0X2ff11c193c7fd8abUL), CtorFromBits); // 5.762203755157295293323152710140698E-7
        static readonly Decimal128Bid Exp2Internal_40A1_RI8_Q5 = new(UInt128.FromLoHi(0X65703387679ea263UL, 0Xaff458b38403cc4cUL), CtorFromBits); // -1.799074742824246214583423099380323E-5
        static readonly Decimal128Bid Exp2Internal_40A1_RI8_Q4 = new(UInt128.FromLoHi(0Xd00a37238dfcc6adUL, 0X2ff6bd5957f8068cUL), CtorFromBits); // 3.840453946594324691543342997882541E-4
        static readonly Decimal128Bid Exp2Internal_40A1_RI8_Q3 = new(UInt128.FromLoHi(0X08da6d9d5a03d40aUL, 0Xaff918d7282b6630UL), CtorFromBits); // -0.005696121175829805111430707497128970
        static readonly Decimal128Bid Exp2Internal_40A1_RI8_Q2 = new(UInt128.FromLoHi(0X0a08253b73425f4bUL, 0X2ffb18d43c1806f5UL), CtorFromBits); // 0.05695889657623155083794239528787787
        static readonly Decimal128Bid Exp2Internal_40A1_RI8_Q1 = new(UInt128.FromLoHi(0Xcba650ec466fd204UL, 0Xaffcac28312970feUL), CtorFromBits); // -0.3491758793193702930112366704054788
                                                                                                                                                  // static readonly Decimal128Bid Exp2Internal_40A1_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Numerator coefficients:
        static readonly Decimal128Bid Exp2Internal_25A1_RI8_P8 = new(UInt128.FromLoHi(0Xe79906a2074615e0UL, 0X2fe9dc8889f908d0UL), CtorFromBits); // 9.665244701950642223036169309263328E-11
        static readonly Decimal128Bid Exp2Internal_25A1_RI8_P7 = new(UInt128.FromLoHi(0X8942e1d899c52f32UL, 0X2fee322ab50db305UL), CtorFromBits); // 1.017504096356401666693996975828786E-8
        static readonly Decimal128Bid Exp2Internal_25A1_RI8_P6 = new(UInt128.FromLoHi(0X0af8325c5b889c1dUL, 0X2ff1000cf21ac222UL), CtorFromBits); // 5.193322524206213953480932949793821E-7
        static readonly Decimal128Bid Exp2Internal_25A1_RI8_P5 = new(UInt128.FromLoHi(0Xd268c3d30de0cb81UL, 0X2ff451fcd57c4da7UL), CtorFromBits); // 1.662906745429991483151685429283713E-5
        static readonly Decimal128Bid Exp2Internal_25A1_RI8_P4 = new(UInt128.FromLoHi(0Xf856a0f2a440e310UL, 0X2ff6b2bbc9e26dc6UL), CtorFromBits); // 3.625147056062782386484129833214736E-4
        static readonly Decimal128Bid Exp2Internal_25A1_RI8_P3 = new(UInt128.FromLoHi(0Xe84e87f748568266UL, 0X2ff90dd8c642d0a0UL), CtorFromBits); // 0.005473142825291649608806189982188134
        static readonly Decimal128Bid Exp2Internal_25A1_RI8_P2 = new(UInt128.FromLoHi(0X49eb6f9916b28387UL, 0X2ffb11f92e8f6b7bUL), CtorFromBits); // 0.05556840043957369322225331277562759
        static readonly Decimal128Bid Exp2Internal_25A1_RI8_P1 = new(UInt128.FromLoHi(0X92f0234fa19a2259UL, 0X2ffcaa2ede4f8f9eUL), CtorFromBits); // 0.3451722929951983800376851686892121
                                                                                                                                                  // static readonly Decimal128Bid Exp2Internal_25A1_RI8_P0 = new (UInt128.FromLoHi(0X38c15b0a00000000UL, 0X2ffe314dc6448d93UL), CtorFromBits); // 1.000000000000000000000000000000000

        // Denominator coefficients:
        static readonly Decimal128Bid Exp2Internal_25A1_RI8_Q8 = new(UInt128.FromLoHi(0X9d95568cdecb99b3UL, 0X2fea35cc85fe6ad0UL), CtorFromBits); // 1.091171723224332003376950552533427E-10
        static readonly Decimal128Bid Exp2Internal_25A1_RI8_Q7 = new(UInt128.FromLoHi(0X95d13f1c89089f69UL, 0Xafee372154246041UL), CtorFromBits); // -1.118173098280524111769559577960297E-8
        static readonly Decimal128Bid Exp2Internal_25A1_RI8_Q6 = new(UInt128.FromLoHi(0X7de9c1ce237570d0UL, 0X2ff113616bd4619bUL), CtorFromBits); // 5.585381144417355516721602439246032E-7
        static readonly Decimal128Bid Exp2Internal_25A1_RI8_Q5 = new(UInt128.FromLoHi(0X8300e14a23682a2aUL, 0Xaff456a29037237eUL), CtorFromBits); // -1.757166820741305388090371200264746E-5
        static readonly Decimal128Bid Exp2Internal_25A1_RI8_Q4 = new(UInt128.FromLoHi(0X055a21b33b427ccaUL, 0X2ff6ba1bbc991aeeUL), CtorFromBits); // 3.774725714941764889182323834256586E-4
        static readonly Decimal128Bid Exp2Internal_25A1_RI8_Q3 = new(UInt128.FromLoHi(0X8840db54b6579f70UL, 0Xaff91581ce2122c8UL), CtorFromBits); // -0.005628511687146687678670530244222832
        static readonly Decimal128Bid Exp2Internal_25A1_RI8_Q2 = new(UInt128.FromLoHi(0Xbbda4992e0176bb7UL, 0X2ffb16c3243795ccUL), CtorFromBits); // 0.05653970570164128848866294703287223
        static readonly Decimal128Bid Exp2Internal_25A1_RI8_Q1 = new(UInt128.FromLoHi(0Xe193083a81652991UL, 0Xaffcab909b067d20UL), CtorFromBits); // -0.3479748875647469293795469527689617
                                                                                                                                                  // static readonly Decimal128Bid Exp2Internal_25A1_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Numerator coefficients:
        static readonly Decimal128Bid Exp2Internal_10A_RI8_P8 = new(UInt128.FromLoHi(0Xc616b9f75540f8e9UL, 0X2fea32a0569f3850UL), CtorFromBits); // 1.026823794380858305981416537651433E-10
        static readonly Decimal128Bid Exp2Internal_10A_RI8_P7 = new(UInt128.FromLoHi(0Xee88c220aa4ac9e5UL, 0X2fee34968a2bfca5UL), CtorFromBits); // 1.066612285875289356389563272710629E-8
        static readonly Decimal128Bid Exp2Internal_10A_RI8_P6 = new(UInt128.FromLoHi(0X1ca30a20416a5076UL, 0X2ff1098a871e21c6UL), CtorFromBits); // 5.385813848298254223722564444377206E-7
        static readonly Decimal128Bid Exp2Internal_10A_RI8_P5 = new(UInt128.FromLoHi(0X7c2b07a0f3b9a5a7UL, 0X2ff45447f7d7864cUL), CtorFromBits); // 1.709424309595938181465565262620071E-5
        static readonly Decimal128Bid Exp2Internal_10A_RI8_P4 = new(UInt128.FromLoHi(0X7de0043c8faf9c1fUL, 0X2ff6b66365f7eeaaUL), CtorFromBits); // 3.699273693671253826915321804463135E-4
        static readonly Decimal128Bid Exp2Internal_10A_RI8_P3 = new(UInt128.FromLoHi(0X9121f90b146ab440UL, 0X2ff911a7faf56c31UL), CtorFromBits); // 0.005550406592886999240490219603670080
        static readonly Decimal128Bid Exp2Internal_10A_RI8_P2 = new(UInt128.FromLoHi(0Xe4ae1ce43e47d0c5UL, 0X2ffb145ca13227d2UL), CtorFromBits); // 0.05605283929280108160582819210842309
        static readonly Decimal128Bid Exp2Internal_10A_RI8_P1 = new(UInt128.FromLoHi(0Xba4195c5117fa603UL, 0X2ffcaadfbcab065fUL), CtorFromBits); // 0.3465735902799726547086160607290883
                                                                                                                                                 // static readonly Decimal128Bid Exp2Internal_10A_RI8_P0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Denominator coefficients:
        static readonly Decimal128Bid Exp2Internal_10A_RI8_Q8 = new(UInt128.FromLoHi(0Xc616b9f75540f8e9UL, 0X2fea32a0569f3850UL), CtorFromBits); // 1.026823794380858305981416537651433E-10
        static readonly Decimal128Bid Exp2Internal_10A_RI8_Q7 = new(UInt128.FromLoHi(0Xee88c220aa4ac9e5UL, 0Xafee34968a2bfca5UL), CtorFromBits); // -1.066612285875289356389563272710629E-8
        static readonly Decimal128Bid Exp2Internal_10A_RI8_Q6 = new(UInt128.FromLoHi(0X1ca30a20416a5076UL, 0X2ff1098a871e21c6UL), CtorFromBits); // 5.385813848298254223722564444377206E-7
        static readonly Decimal128Bid Exp2Internal_10A_RI8_Q5 = new(UInt128.FromLoHi(0X7c2b07a0f3b9a5a7UL, 0Xaff45447f7d7864cUL), CtorFromBits); // -1.709424309595938181465565262620071E-5
        static readonly Decimal128Bid Exp2Internal_10A_RI8_Q4 = new(UInt128.FromLoHi(0X7de0043c8faf9c1fUL, 0X2ff6b66365f7eeaaUL), CtorFromBits); // 3.699273693671253826915321804463135E-4
        static readonly Decimal128Bid Exp2Internal_10A_RI8_Q3 = new(UInt128.FromLoHi(0X9121f90b146ab440UL, 0Xaff911a7faf56c31UL), CtorFromBits); // -0.005550406592886999240490219603670080
        static readonly Decimal128Bid Exp2Internal_10A_RI8_Q2 = new(UInt128.FromLoHi(0Xe4ae1ce43e47d0c5UL, 0X2ffb145ca13227d2UL), CtorFromBits); // 0.05605283929280108160582819210842309
        static readonly Decimal128Bid Exp2Internal_10A_RI8_Q1 = new(UInt128.FromLoHi(0Xba4195c5117fa603UL, 0Xaffcaadfbcab065fUL), CtorFromBits); // -0.3465735902799726547086160607290883
                                                                                                                                                 // static readonly Decimal128Bid Exp2Internal_10A_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Numerator coefficients:
        static readonly Decimal128Bid Exp2Internal_25A0_RI8_P8 = new(UInt128.FromLoHi(0X9d95568cdecb99b3UL, 0X2fea35cc85fe6ad0UL), CtorFromBits); // 1.091171723224332003376950552533427E-10
        static readonly Decimal128Bid Exp2Internal_25A0_RI8_P7 = new(UInt128.FromLoHi(0X95d13f1c89089f69UL, 0X2fee372154246041UL), CtorFromBits); // 1.118173098280524111769559577960297E-8
        static readonly Decimal128Bid Exp2Internal_25A0_RI8_P6 = new(UInt128.FromLoHi(0X7de9c1ce237570d0UL, 0X2ff113616bd4619bUL), CtorFromBits); // 5.585381144417355516721602439246032E-7
        static readonly Decimal128Bid Exp2Internal_25A0_RI8_P5 = new(UInt128.FromLoHi(0X8300e14a23682a2aUL, 0X2ff456a29037237eUL), CtorFromBits); // 1.757166820741305388090371200264746E-5
        static readonly Decimal128Bid Exp2Internal_25A0_RI8_P4 = new(UInt128.FromLoHi(0X055a21b33b427ccaUL, 0X2ff6ba1bbc991aeeUL), CtorFromBits); // 3.774725714941764889182323834256586E-4
        static readonly Decimal128Bid Exp2Internal_25A0_RI8_P3 = new(UInt128.FromLoHi(0X8840db54b6579f70UL, 0X2ff91581ce2122c8UL), CtorFromBits); // 0.005628511687146687678670530244222832
        static readonly Decimal128Bid Exp2Internal_25A0_RI8_P2 = new(UInt128.FromLoHi(0Xbbda4992e0176bb7UL, 0X2ffb16c3243795ccUL), CtorFromBits); // 0.05653970570164128848866294703287223
        static readonly Decimal128Bid Exp2Internal_25A0_RI8_P1 = new(UInt128.FromLoHi(0Xe193083a81652991UL, 0X2ffcab909b067d20UL), CtorFromBits); // 0.3479748875647469293795469527689617
                                                                                                                                                  // static readonly Decimal128Bid Exp2Internal_25A0_RI8_P0 = new (UInt128.FromLoHi(0X38c15b0a00000000UL, 0X2ffe314dc6448d93UL), CtorFromBits); // 1.000000000000000000000000000000000

        // Denominator coefficients:
        static readonly Decimal128Bid Exp2Internal_25A0_RI8_Q8 = new(UInt128.FromLoHi(0Xe79906a2074615e0UL, 0X2fe9dc8889f908d0UL), CtorFromBits); // 9.665244701950642223036169309263328E-11
        static readonly Decimal128Bid Exp2Internal_25A0_RI8_Q7 = new(UInt128.FromLoHi(0X8942e1d899c52f32UL, 0Xafee322ab50db305UL), CtorFromBits); // -1.017504096356401666693996975828786E-8
        static readonly Decimal128Bid Exp2Internal_25A0_RI8_Q6 = new(UInt128.FromLoHi(0X0af8325c5b889c1dUL, 0X2ff1000cf21ac222UL), CtorFromBits); // 5.193322524206213953480932949793821E-7
        static readonly Decimal128Bid Exp2Internal_25A0_RI8_Q5 = new(UInt128.FromLoHi(0Xd268c3d30de0cb81UL, 0Xaff451fcd57c4da7UL), CtorFromBits); // -1.662906745429991483151685429283713E-5
        static readonly Decimal128Bid Exp2Internal_25A0_RI8_Q4 = new(UInt128.FromLoHi(0Xf856a0f2a440e311UL, 0X2ff6b2bbc9e26dc6UL), CtorFromBits); // 3.625147056062782386484129833214737E-4
        static readonly Decimal128Bid Exp2Internal_25A0_RI8_Q3 = new(UInt128.FromLoHi(0Xe84e87f748568266UL, 0Xaff90dd8c642d0a0UL), CtorFromBits); // -0.005473142825291649608806189982188134
        static readonly Decimal128Bid Exp2Internal_25A0_RI8_Q2 = new(UInt128.FromLoHi(0X49eb6f9916b28387UL, 0X2ffb11f92e8f6b7bUL), CtorFromBits); // 0.05556840043957369322225331277562759
        static readonly Decimal128Bid Exp2Internal_25A0_RI8_Q1 = new(UInt128.FromLoHi(0X92f0234fa19a2259UL, 0Xaffcaa2ede4f8f9eUL), CtorFromBits); // -0.3451722929951983800376851686892121
                                                                                                                                                  // static readonly Decimal128Bid Exp2Internal_25A0_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Numerator coefficients:
        static readonly Decimal128Bid Exp2Internal_40A0_RI8_P8 = new(UInt128.FromLoHi(0Xc01a9f909597ce2aUL, 0X2fea38afcbdd8136UL), CtorFromBits); // 1.149742959484284913723927562800682E-10
        static readonly Decimal128Bid Exp2Internal_40A0_RI8_P7 = new(UInt128.FromLoHi(0X1e52d3761b383b5dUL, 0X2fee3968c0eb687dUL), CtorFromBits); // 1.164396782022516155234703348611933E-8
        static readonly Decimal128Bid Exp2Internal_40A0_RI8_P6 = new(UInt128.FromLoHi(0X8fc336cc290ba9cfUL, 0X2ff11c193c7fd8abUL), CtorFromBits); // 5.762203755157295293323152710150607E-7
        static readonly Decimal128Bid Exp2Internal_40A0_RI8_P5 = new(UInt128.FromLoHi(0X65703387679eae79UL, 0X2ff458b38403cc4cUL), CtorFromBits); // 1.799074742824246214583423099383417E-5
        static readonly Decimal128Bid Exp2Internal_40A0_RI8_P4 = new(UInt128.FromLoHi(0Xd00a37238dfce079UL, 0X2ff6bd5957f8068cUL), CtorFromBits); // 3.840453946594324691543342997889145E-4
        static readonly Decimal128Bid Exp2Internal_40A0_RI8_P3 = new(UInt128.FromLoHi(0X08da6d9d5a03fa4dUL, 0X2ff918d7282b6630UL), CtorFromBits); // 0.005696121175829805111430707497138765
        static readonly Decimal128Bid Exp2Internal_40A0_RI8_P2 = new(UInt128.FromLoHi(0X0a08253b7342858eUL, 0X2ffb18d43c1806f5UL), CtorFromBits); // 0.05695889657623155083794239528797582
        static readonly Decimal128Bid Exp2Internal_40A0_RI8_P1 = new(UInt128.FromLoHi(0Xcba650ec466fe978UL, 0X2ffcac28312970feUL), CtorFromBits); // 0.3491758793193702930112366704060792
        static readonly Decimal128Bid Exp2Internal_40A0_RI8_P0 = new(UInt128.FromLoHi(0X38c15b0a000006b8UL, 0X2ffe314dc6448d93UL), CtorFromBits); // 1.000000000000000000000000000001720

        // Denominator coefficients:
        static readonly Decimal128Bid Exp2Internal_40A0_RI8_Q8 = new(UInt128.FromLoHi(0X82a2a9de51161b8dUL, 0X2fe9c4876fa31bcbUL), CtorFromBits); // 9.178379492812237481043856868121485E-11
        static readonly Decimal128Bid Exp2Internal_40A0_RI8_Q7 = new(UInt128.FromLoHi(0X2074f06a0a3541f1UL, 0Xafede1d403715a8dUL), CtorFromBits); // -9.772636455300740907107867958788593E-9
        static readonly Decimal128Bid Exp2Internal_40A0_RI8_Q6 = new(UInt128.FromLoHi(0X5d1e5f8924415069UL, 0X2ff0f82eeb49cca0UL), CtorFromBits); // 5.033754895376484224090040021635177E-7
        static readonly Decimal128Bid Exp2Internal_40A0_RI8_Q5 = new(UInt128.FromLoHi(0X57c2c6b492127624UL, 0Xaff45011937904e9UL), CtorFromBits); // -1.623985287654537919886061567374884E-5
        static readonly Decimal128Bid Exp2Internal_40A0_RI8_Q4 = new(UInt128.FromLoHi(0Xcdd87cdea48c360aUL, 0X2ff6afa6f6855049UL), CtorFromBits); // 3.562649850095111027553504647198218E-4
        static readonly Decimal128Bid Exp2Internal_40A0_RI8_Q3 = new(UInt128.FromLoHi(0Xcdee9a2af3a91463UL, 0Xaff90a9d4f18c535UL), CtorFromBits); // -0.005407584255347378104312242905879651
        static readonly Decimal128Bid Exp2Internal_40A0_RI8_Q2 = new(UInt128.FromLoHi(0Xf2e17aebd9c6f014UL, 0X2ffb0fef8ecbb290UL), CtorFromBits); // 0.05515512726557102940846234838429716
        static readonly Decimal128Bid Exp2Internal_40A0_RI8_Q1 = new(UInt128.FromLoHi(0Xa8dcda9ddc815eceUL, 0Xaffca997482c9bc0UL), CtorFromBits); // -0.3439713012405750164059954509602510
                                                                                                                                                  // static readonly Decimal128Bid Exp2Internal_40A0_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Numerator coefficients:
        static readonly Decimal128Bid Exp2Internal_50A0_RI8_P8 = new(UInt128.FromLoHi(0X1d5d44575990e7e5UL, 0X2fea3b3842ff9faaUL), CtorFromBits); // 1.201119678756973901614447774984165E-10
        static readonly Decimal128Bid Exp2Internal_50A0_RI8_P7 = new(UInt128.FromLoHi(0Xf7af6e37bcdbdc13UL, 0X2fee3b61d715f767UL), CtorFromBits); // 1.204413864212228380369878500760595E-8
        static readonly Decimal128Bid Exp2Internal_50A0_RI8_P6 = new(UInt128.FromLoHi(0Xd5b219c551f561e9UL, 0X2ff123920e1c2dffUL), CtorFromBits); // 5.913752873247004944564227757400553E-7
        static readonly Decimal128Bid Exp2Internal_50A0_RI8_P5 = new(UInt128.FromLoHi(0X5dd716957f897d8fUL, 0X2ff45a75446120d9UL), CtorFromBits); // 1.834707721744417052551420974824847E-5
        static readonly Decimal128Bid Exp2Internal_50A0_RI8_P4 = new(UInt128.FromLoHi(0X12b66670f7b97678UL, 0X2ff6c0162d2a9b10UL), CtorFromBits); // 3.895979641809024222295196350117496E-4
        static readonly Decimal128Bid Exp2Internal_50A0_RI8_P3 = new(UInt128.FromLoHi(0X667c9ff504afca9cUL, 0X2ff91ba42e8009b3UL), CtorFromBits); // 0.005752929727584529424376493515983516
        static readonly Decimal128Bid Exp2Internal_50A0_RI8_P2 = new(UInt128.FromLoHi(0X71624b55e6bb81c9UL, 0X2ffb1a8eca07d4d6UL), CtorFromBits); // 0.05730952432746358712273339680522697
        static readonly Decimal128Bid Exp2Internal_50A0_RI8_P1 = new(UInt128.FromLoHi(0Xc1f205a75bb3bfecUL, 0X2ffcaca67e4b90f6UL), CtorFromBits); // 0.3501765413270701801349474443050988
        static readonly Decimal128Bid Exp2Internal_50A0_RI8_P0 = new(UInt128.FromLoHi(0X38c15b0a0007edfaUL, 0X2ffe314dc6448d93UL), CtorFromBits); // 1.000000000000000000000000000519674

        // Denominator coefficients:
        static readonly Decimal128Bid Exp2Internal_50A0_RI8_Q8 = new(UInt128.FromLoHi(0X07ba5e4694ef38d4UL, 0X2fe9b1839c921502UL), CtorFromBits); // 8.792710703934450861850065778981076E-11
        static readonly Decimal128Bid Exp2Internal_50A0_RI8_Q7 = new(UInt128.FromLoHi(0X775acc4e3cca7032UL, 0Xafedd1ea840d25bbUL), CtorFromBits); // -9.449900723641882376090647450513458E-9
        static readonly Decimal128Bid Exp2Internal_50A0_RI8_Q6 = new(UInt128.FromLoHi(0X1ade32ae2c0bc5c2UL, 0X2ff0f1cfc67cfc96UL), CtorFromBits); // 4.904522373252056849925596815017410E-7
        static readonly Decimal128Bid Exp2Internal_50A0_RI8_Q5 = new(UInt128.FromLoHi(0X6c02dc3688866bb4UL, 0Xaff44e808f70af2bUL), CtorFromBits); // -1.592213546469967504985744686345140E-5
        static readonly Decimal128Bid Exp2Internal_50A0_RI8_Q4 = new(UInt128.FromLoHi(0Xc47d45b2e6df17b2UL, 0X2ff6ad1ed93fba8fUL), CtorFromBits); // 3.511300941597636989837964753180594E-4
        static readonly Decimal128Bid Exp2Internal_50A0_RI8_Q3 = new(UInt128.FromLoHi(0Xf993eb1473aed9f4UL, 0Xaff907f1beed5c26UL), CtorFromBits); // -0.005353426802028789007763361709873652
        static readonly Decimal128Bid Exp2Internal_50A0_RI8_Q2 = new(UInt128.FromLoHi(0X8fd19b81d27c20d1UL, 0X2ffb0e3ea887d37fUL), CtorFromBits); // 0.05481214896747243464961097614500049
        static readonly Decimal128Bid Exp2Internal_50A0_RI8_Q1 = new(UInt128.FromLoHi(0Xb29125e2bba082d7UL, 0Xaffca918fb0a7bc8UL), CtorFromBits); // -0.3429706392328751292822846575772375
                                                                                                                                                  // static readonly Decimal128Bid Exp2Internal_50A0_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        static Decimal128Bid Exp2Internal_50A1(Decimal128Bid x) {
            Debug.Assert(-0.50M <= x && x <= -0.40M);

            // Horner for numerator
            Decimal128Bid px = Exp2Internal_50A1_RI8_P8;
            px = FusedMultiplyAdd(px, x, Exp2Internal_50A1_RI8_P7);
            px = FusedMultiplyAdd(px, x, Exp2Internal_50A1_RI8_P6);
            px = FusedMultiplyAdd(px, x, Exp2Internal_50A1_RI8_P5);
            px = FusedMultiplyAdd(px, x, Exp2Internal_50A1_RI8_P4);
            px = FusedMultiplyAdd(px, x, Exp2Internal_50A1_RI8_P3);
            px = FusedMultiplyAdd(px, x, Exp2Internal_50A1_RI8_P2);
            px = FusedMultiplyAdd(px, x, Exp2Internal_50A1_RI8_P1);
            px = FusedMultiplyAdd(px, x, Exp2Internal_50A1_RI8_P0);

            // Horner for denominator
            Decimal128Bid qx = Exp2Internal_50A1_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_50A1_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_50A1_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_50A1_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_50A1_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_50A1_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_50A1_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_50A1_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        static Decimal128Bid Exp2Internal_40A1(Decimal128Bid x) {
            Debug.Assert(-0.40M <= x && x <= -0.25M);

            // Horner for numerator
            Decimal128Bid px = Exp2Internal_40A1_RI8_P8;
            px = FusedMultiplyAdd(px, x, Exp2Internal_40A1_RI8_P7);
            px = FusedMultiplyAdd(px, x, Exp2Internal_40A1_RI8_P6);
            px = FusedMultiplyAdd(px, x, Exp2Internal_40A1_RI8_P5);
            px = FusedMultiplyAdd(px, x, Exp2Internal_40A1_RI8_P4);
            px = FusedMultiplyAdd(px, x, Exp2Internal_40A1_RI8_P3);
            px = FusedMultiplyAdd(px, x, Exp2Internal_40A1_RI8_P2);
            px = FusedMultiplyAdd(px, x, Exp2Internal_40A1_RI8_P1);
            px = FusedMultiplyAdd(px, x, Exp2Internal_40A1_RI8_P0);

            // Horner for denominator
            Decimal128Bid qx = Exp2Internal_40A1_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_40A1_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_40A1_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_40A1_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_40A1_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_40A1_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_40A1_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_40A1_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        static Decimal128Bid Exp2Internal_25A1(Decimal128Bid x) {
            Debug.Assert(-0.25M <= x && x <= -0.10M);

            // Horner for numerator
            Decimal128Bid px = Exp2Internal_25A1_RI8_P8;
            px = FusedMultiplyAdd(px, x, Exp2Internal_25A1_RI8_P7);
            px = FusedMultiplyAdd(px, x, Exp2Internal_25A1_RI8_P6);
            px = FusedMultiplyAdd(px, x, Exp2Internal_25A1_RI8_P5);
            px = FusedMultiplyAdd(px, x, Exp2Internal_25A1_RI8_P4);
            px = FusedMultiplyAdd(px, x, Exp2Internal_25A1_RI8_P3);
            px = FusedMultiplyAdd(px, x, Exp2Internal_25A1_RI8_P2);
            px = FusedMultiplyAdd(px, x, Exp2Internal_25A1_RI8_P1);
            px = FusedMultiplyAdd(px, x, One);

            // Horner for denominator
            Decimal128Bid qx = Exp2Internal_25A1_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_25A1_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_25A1_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_25A1_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_25A1_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_25A1_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_25A1_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_25A1_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        static Decimal128Bid Exp2Internal_10A(Decimal128Bid x) {
            Debug.Assert(-0.10M <= x && x <= 0.10M);

            // Horner for numerator
            Decimal128Bid px = Exp2Internal_10A_RI8_P8;
            px = FusedMultiplyAdd(px, x, Exp2Internal_10A_RI8_P7);
            px = FusedMultiplyAdd(px, x, Exp2Internal_10A_RI8_P6);
            px = FusedMultiplyAdd(px, x, Exp2Internal_10A_RI8_P5);
            px = FusedMultiplyAdd(px, x, Exp2Internal_10A_RI8_P4);
            px = FusedMultiplyAdd(px, x, Exp2Internal_10A_RI8_P3);
            px = FusedMultiplyAdd(px, x, Exp2Internal_10A_RI8_P2);
            px = FusedMultiplyAdd(px, x, Exp2Internal_10A_RI8_P1);
            px = FusedMultiplyAdd(px, x, One);

            // Horner for denominator
            Decimal128Bid qx = Exp2Internal_10A_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_10A_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_10A_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_10A_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_10A_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_10A_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_10A_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_10A_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        static Decimal128Bid Exp2Internal_25A0(Decimal128Bid x) {
            Debug.Assert(0.10M <= x && x <= 0.25M);

            // Horner for numerator
            Decimal128Bid px = Exp2Internal_25A0_RI8_P8;
            px = FusedMultiplyAdd(px, x, Exp2Internal_25A0_RI8_P7);
            px = FusedMultiplyAdd(px, x, Exp2Internal_25A0_RI8_P6);
            px = FusedMultiplyAdd(px, x, Exp2Internal_25A0_RI8_P5);
            px = FusedMultiplyAdd(px, x, Exp2Internal_25A0_RI8_P4);
            px = FusedMultiplyAdd(px, x, Exp2Internal_25A0_RI8_P3);
            px = FusedMultiplyAdd(px, x, Exp2Internal_25A0_RI8_P2);
            px = FusedMultiplyAdd(px, x, Exp2Internal_25A0_RI8_P1);
            px = FusedMultiplyAdd(px, x, One);

            // Horner for denominator
            Decimal128Bid qx = Exp2Internal_25A0_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_25A0_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_25A0_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_25A0_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_25A0_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_25A0_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_25A0_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_25A0_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        static Decimal128Bid Exp2Internal_40A0(Decimal128Bid x) {
            Debug.Assert(0.25M <= x && x <= 0.40M);

            // Horner for numerator
            Decimal128Bid px = Exp2Internal_40A0_RI8_P8;
            px = FusedMultiplyAdd(px, x, Exp2Internal_40A0_RI8_P7);
            px = FusedMultiplyAdd(px, x, Exp2Internal_40A0_RI8_P6);
            px = FusedMultiplyAdd(px, x, Exp2Internal_40A0_RI8_P5);
            px = FusedMultiplyAdd(px, x, Exp2Internal_40A0_RI8_P4);
            px = FusedMultiplyAdd(px, x, Exp2Internal_40A0_RI8_P3);
            px = FusedMultiplyAdd(px, x, Exp2Internal_40A0_RI8_P2);
            px = FusedMultiplyAdd(px, x, Exp2Internal_40A0_RI8_P1);
            px = FusedMultiplyAdd(px, x, Exp2Internal_40A0_RI8_P0);

            // Horner for denominator
            Decimal128Bid qx = Exp2Internal_40A0_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_40A0_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_40A0_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_40A0_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_40A0_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_40A0_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_40A0_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_40A0_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        static Decimal128Bid Exp2Internal_50A0(Decimal128Bid x) {
            Debug.Assert(0.40M <= x && x <= 0.50M);

            // Horner for numerator
            Decimal128Bid px = Exp2Internal_50A0_RI8_P8;
            px = FusedMultiplyAdd(px, x, Exp2Internal_50A0_RI8_P7);
            px = FusedMultiplyAdd(px, x, Exp2Internal_50A0_RI8_P6);
            px = FusedMultiplyAdd(px, x, Exp2Internal_50A0_RI8_P5);
            px = FusedMultiplyAdd(px, x, Exp2Internal_50A0_RI8_P4);
            px = FusedMultiplyAdd(px, x, Exp2Internal_50A0_RI8_P3);
            px = FusedMultiplyAdd(px, x, Exp2Internal_50A0_RI8_P2);
            px = FusedMultiplyAdd(px, x, Exp2Internal_50A0_RI8_P1);
            px = FusedMultiplyAdd(px, x, Exp2Internal_50A0_RI8_P0);

            // Horner for denominator
            Decimal128Bid qx = Exp2Internal_50A0_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_50A0_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_50A0_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_50A0_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_50A0_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_50A0_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_50A0_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, Exp2Internal_50A0_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        internal static Decimal128Bid Exp2Internal_B0(Decimal128Bid x) {
            Debug.Assert(-0.5M <= x && x <= 0.5M);
            if (x < -Value_Pt10) {
                if (x >= -Value_Pt25) {
                    return Exp2Internal_25A1(x);
                } else if (x >= -Value_Pt40) {
                    return Exp2Internal_40A1(x);
                } else {
                    return Exp2Internal_50A1(x);
                }
            } else {
                if (x <= Value_Pt25) {
                    if (x <= Value_Pt10) {
                        return Exp2Internal_10A(x);
                    } else {
                        return Exp2Internal_25A0(x);
                    }
                } else {
                    if (x <= Value_Pt40) {
                        return Exp2Internal_40A0(x);
                    } else {
                        return Exp2Internal_50A0(x);
                    }
                }
            }
        }

        public static Decimal128Bid Exp2(Decimal128Bid x) {
            // Preserve NaN payload exactly
            if (Decimal128Bid.IsNaN(x)) {
                return x;
            }

            // +Inf -> +Inf, -Inf -> +0 (coarsest cohort)
            if (Decimal128Bid.IsInfinity(x)) {
                if (Decimal128Bid.IsPositiveInfinity(x)) {
                    return x;
                } else {
                    // -Inf -> Zero
                    return Decimal128Bid.Zero;
                }
            }

            // Exact zero: exp(0) == 1 but we must preserve cohort rules described:
            if (Decimal128Bid.IsZero(x)) {
                // Determine preferred q-exponent for zero input:
                var qx = ILogBQuantum(x); // q-exponent of input zero (may be negative)
                if (qx >= 0) {
                    return Decimal128Bid.One;
                } else {
                    // prefer finest cohort
                    return Decimal128Bid.ToFinestCohort(Decimal128Bid.One);
                }
            }

            // For nonzero finite x, result is inexact; we will return ToFinestCohort at the end.
            var f = x % One;
            bool expIsInt = false;

            if (IsZero(f)) {
                expIsInt = true;
            } else {
                if (f >= OneHalf_A0) {
                    f -= 1;
                } else if (f < -OneHalf_A0) {
                    f += 1;
                }
            }
            var i = x - f;

            // TODO
            if (i <= -21000) {
                return default(Decimal128Bid); // finest cohort of zero
            } else if (i >= 21000) {
                return Decimal128Bid.PositiveInfinity;
            }

            var ii = (int)(decimal)i;

            Decimal128Bid y;
            if (expIsInt) {
                var qx = ILogBQuantum(f); // q-exponent of input zero (may be negative)
                if (qx >= 0) {
                    return (Decimal128Bid)BigRational.Exp2(ii);
                } else {
                    // prefer finest cohort
                    y = Decimal128Bid.One;
                }
            } else {
                y = Exp2Internal_B0(f);
            }
            // TODO
            return Decimal128Bid.ToFinestCohort((Decimal128Bid)((BigRational)y * BigRational.Exp2(ii)));
        }
    }
}

namespace UltimateOrb {

    partial struct Decimal128Bid {

        // TODO: when roslyn has fixed the ref analysis bug
        static ReadOnlySpan<Decimal128Bid> ExpTable0 => ExpTable0_;
        static readonly Decimal128Bid[] ExpTable0_ = [
            // 1
            new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits),
            // 2.718281828459045235360287471352662
            new (UInt128.FromLoHi(0X4e906accb26abb56UL, 0X2ffe86058a4bf4deUL), CtorFromBits),
            // 7.389056098930650227230427460575008
            new (UInt128.FromLoHi(0Xde1df85c161a9f20UL, 0X2fff6c4effee1a25UL), CtorFromBits),
            // 20.08553692318766774092852965458172
            new (UInt128.FromLoHi(0X6b37c600f824c0fcUL, 0X3000630783018ed1UL), CtorFromBits),
            // 54.59815003314423907811026120286088
            new (UInt128.FromLoHi(0X7fe83bc068911f88UL, 0X30010d308dbede2aUL), CtorFromBits),
            // 148.4131591025766034211155800405523
            new (UInt128.FromLoHi(0X2f7e126b6eaeb613UL, 0X3002492c5fce92f1UL), CtorFromBits),
            // 403.4287934927351226083871805433883
            new (UInt128.FromLoHi(0Xad566a6f09e4681bUL, 0X3002c6e7df5d3a7eUL), CtorFromBits),
            // 1096.633158428458599263720238288121
            new (UInt128.FromLoHi(0Xc2508b8c65fd0cf9UL, 0X3004361174d7bc1cUL), CtorFromBits),
            // 2980.957987041728274743592099452889
            new (UInt128.FromLoHi(0Xe6057018126717d9UL, 0X300492f8fabdb74cUL), CtorFromBits),
            // 8103.083927575384007709996689432760
            new (UInt128.FromLoHi(0X42b2f6e69444a0b8UL, 0X30058f834c46c7c8UL), CtorFromBits),
            // 22026.46579480671651695790064528424
            new (UInt128.FromLoHi(0X79d385d28285ec28UL, 0X30066c994ef2da47UL), CtorFromBits),
            // 59874.14171519781845532648579225778
            new (UInt128.FromLoHi(0X3512f0434a0358b2UL, 0X30072733ca9a01a4UL), CtorFromBits),
            // 162754.7914190039208080052048984868
            new (UInt128.FromLoHi(0X97cbc83f78357b24UL, 0X3008503e8af09e7eUL), CtorFromBits),
            // 442413.3920089205033261027759490883
            new (UInt128.FromLoHi(0X5bb87023112c0743UL, 0X3008da206bae0938UL), CtorFromBits),
            // 1202604.284164776777749236770767859
            new (UInt128.FromLoHi(0X4b873f57e7ad83f3UL, 0X300a3b4b00046f2fUL), CtorFromBits),
            // 3269017.372472110639301855046091721
            new (UInt128.FromLoHi(0Xe3c28547663d43c9UL, 0X300aa12cccd09ba3UL), CtorFromBits),
            // 8886110.520507872636763023740781450
            new (UInt128.FromLoHi(0X9b11554ef7ac438aUL, 0X300bb61e7ba7914eUL), CtorFromBits),
            // 24154952.75357529821477543518038582
            new (UInt128.FromLoHi(0X475dd57fda1e6a36UL, 0X300c7717d62d7d3bUL), CtorFromBits),
            // 65659969.13733051113878650325906003
            new (UInt128.FromLoHi(0X90248a0d4e393253UL, 0X300d43ba88451f7cUL), CtorFromBits),
            // 178482300.9631872608449100337887227
            new (UInt128.FromLoHi(0Xa266bc7c138bb7fbUL, 0X300e57ffa22e67f1UL), CtorFromBits),
            // 485165195.4097902779691068305415406
            new (UInt128.FromLoHi(0X7396b0251fdb9ceeUL, 0X300eef3474f348e8UL), CtorFromBits),
            // 1318815734.483214697209998883745303
            new (UInt128.FromLoHi(0X60c14a8d78c8ee17UL, 0X30104105cb77182aUL), CtorFromBits),
            // 3584912846.131591561681159945978421
            new (UInt128.FromLoHi(0X38fa14cee9580235UL, 0X3010b0bff6240329UL), CtorFromBits),
            // 9744803446.248902600034632684822975
            new (UInt128.FromLoHi(0X8962be2f697461bfUL, 0X3011e074b623e084UL), CtorFromBits),
            // 26489122129.84347229413916215281188
            new (UInt128.FromLoHi(0Xb9821d9b36170624UL, 0X30128299f8faef82UL), CtorFromBits),
            // 72004899337.38587252416135146612616
            new (UInt128.FromLoHi(0X1bc3d42abf89a788UL, 0X30136302f5ca1261UL), CtorFromBits),
            // 195729609428.8387642697763978760953
            new (UInt128.FromLoHi(0X216547cf2e1052f9UL, 0X301460808cc6389bUL), CtorFromBits),
            // 532048240601.7986166837473043411774
            new (UInt128.FromLoHi(0X36af936ee1c1f33eUL, 0X30150651edd7b004UL), CtorFromBits),
            // 1446257064291.475173677047422996929
            new (UInt128.FromLoHi(0X43185f510bb551c1UL, 0X3016474e549c598aUL), CtorFromBits),
            // 3931334297144.042074388620580843528
            new (UInt128.FromLoHi(0X33cc5c0052757008UL, 0X3016c1d46a380ff5UL), CtorFromBits),
            // 10686474581524.46214699046865074140
            new (UInt128.FromLoHi(0Xb7f786727739f7dcUL, 0X301834b03a2b09abUL), CtorFromBits),
            // 29048849665247.42523108568211167983
            new (UInt128.FromLoHi(0X6222a67824889aefUL, 0X30188f38cd8e018aUL), CtorFromBits),
            // 78962960182680.69516097802263510822
            new (UInt128.FromLoHi(0Xf80596103cbabf26UL, 0X3019855144814a7fUL), CtorFromBits),
            // 214643579785916.0646242977615312609
            new (UInt128.FromLoHi(0Xf3747ec405b9f6e1UL, 0X301a69d3d41e1431UL), CtorFromBits),
            // 583461742527454.8814029027346103910
            new (UInt128.FromLoHi(0X4ebc3f12ee223e66UL, 0X301b1fab39afd9e6UL), CtorFromBits),
            // 1586013452313430.728129644625774660
            new (UInt128.FromLoHi(0Xd4457aa6b9f1c844UL, 0X301c4e324ddb0080UL), CtorFromBits),
            // 4311231547115195.227113422292856925
            new (UInt128.FromLoHi(0Xda01239fe989305dUL, 0X301cd48f6470002cUL), CtorFromBits),
            // 11719142372802611.30877293979119019
            new (UInt128.FromLoHi(0X9a3b25beddbe2dabUL, 0X301e39c7a320a731UL), CtorFromBits),
            // 31855931757113756.22032867170129865
            new (UInt128.FromLoHi(0X890a4c3d0f2717c9UL, 0X301e9d0fd6b26dffUL), CtorFromBits),
            // 86593400423993746.95360693271926493
            new (UInt128.FromLoHi(0X1b2a66ed9ab616ddUL, 0X301faaf03cb893aeUL), CtorFromBits),
            // 235385266837019985.4078999107490348
            new (UInt128.FromLoHi(0X9d4dc6d075e8522cUL, 0X3020740dcc2c1500UL), CtorFromBits),
            // 639843493530054949.2226634035155708
            new (UInt128.FromLoHi(0X302e143eba9ad2fcUL, 0X30213b779a5129a0UL), CtorFromBits),
            // 1739274941520501047.394681303611235
            new (UInt128.FromLoHi(0X4bb395a2d00c3763UL, 0X302255c0bc6f866aUL), CtorFromBits),
            // 4727839468229346561.474457562744280
            new (UInt128.FromLoHi(0X7a621d4627c385d8UL, 0X3022e919b93b61e0UL), CtorFromBits),
            // 12851600114359308275.80929963214310
            new (UInt128.FromLoHi(0X98a75572278045e6UL, 0X30243f5cfff57cfcUL), CtorFromBits),
            // 34934271057485095348.03479723340610
            new (UInt128.FromLoHi(0X1a893291f7e17342UL, 0X3024ac3d3fc53e0eUL), CtorFromBits),
            // 94961194206024488745.13364911711832
            new (UInt128.FromLoHi(0X3c323e5a1224e658UL, 0X3025d431e0b735ecUL), CtorFromBits),
            // 258131288619006739623.2858002152734
            new (UInt128.FromLoHi(0Xefb6b0ec9d7a5d1eUL, 0X30267f44bfc663c4UL), CtorFromBits),
            // 701673591209763173865.4715998861174
            new (UInt128.FromLoHi(0X7d8000ad90e37776UL, 0X302759f3a885e449UL), CtorFromBits),
            // 1907346572495099690525.099840953848
            new (UInt128.FromLoHi(0X635a633eb38eddf8UL, 0X30285e0a192a2ffaUL), CtorFromBits),
            // 5184705528587072464087.453322933485
            new (UInt128.FromLoHi(0X7a9f35b7fd0774edUL, 0X3028ffa02f170b31UL), CtorFromBits),
            // 14093490824269387964492.14331237017
            new (UInt128.FromLoHi(0Xf714154f1a823299UL, 0X302a457c7c7d4d8eUL), CtorFromBits),
            // 38310080007165768493035.69548786199
            new (UInt128.FromLoHi(0X13cecd2709ed3a17UL, 0X302abce21e78ba71UL), CtorFromBits),
            // 104137594330290877971834.7293349380
            new (UInt128.FromLoHi(0Xc271507129b65604UL, 0X302c33580326ade5UL), CtorFromBits),
            // 283075330327469390044206.3548014075
            new (UInt128.FromLoHi(0X0e55695d1a8d5dfbUL, 0X302c8b91213b63a7UL), CtorFromBits),
            // 769478526514201713818274.5590129394
            new (UInt128.FromLoHi(0X29674537c460e2f2UL, 0X302d7b61d825158dUL), CtorFromBits),
            // 2091659496012996153907071.157214674
            new (UInt128.FromLoHi(0X49583372094f1dd2UL, 0X302e672074572a15UL), CtorFromBits),
            // 5685719999335932222640348.820633253
            new (UInt128.FromLoHi(0Xed5bc791928f76a5UL, 0X302f1853e027db0eUL), CtorFromBits),
            // 15455389355901039303530766.91117462
            new (UInt128.FromLoHi(0X3d4d1a23f0d9a596UL, 0X30304c3371a3774aUL), CtorFromBits),
            // 42012104037905142549565934.30719162
            new (UInt128.FromLoHi(0Xf180e41ddc1ceabaUL, 0X3030cf22baf34586UL), CtorFromBits),
            // 114200738981568428366295718.3144766
            new (UInt128.FromLoHi(0X42f660a16266933eUL, 0X3032384e28e43f71UL), CtorFromBits),
            // 310429793570191990870734214.1107100
            new (UInt128.FromLoHi(0X6a850d1c10382b9cUL, 0X3032990dbf9c0042UL), CtorFromBits),
            // 843835666874145448907332948.0373118
            new (UInt128.FromLoHi(0X73bc81ce9543937eUL, 0X3033a00b08ee30c3UL), CtorFromBits),
            // 2293783159469609879099352840.268614
            new (UInt128.FromLoHi(0Xc3c1b2260f55eb46UL, 0X303471179d1ee9f4UL), CtorFromBits),
            // 6235149080811616882909238708.928470
            new (UInt128.FromLoHi(0X8dbb11637ba7ffd6UL, 0X3035336aa5440673UL), CtorFromBits),
            // 16948892444103337141417836114.37197
            new (UInt128.FromLoHi(0Xf21e14d508c0708dUL, 0X303653908283669fUL), CtorFromBits),
            // 46071866343312915426773184428.06009
            new (UInt128.FromLoHi(0X47a5928a82fdeaf9UL, 0X3036e326dee65510UL), CtorFromBits),
            // 125236317084221378051352196074.4366
            new (UInt128.FromLoHi(0X27f7fafd154ff5aeUL, 0X30383dbf0ba6cbceUL), CtorFromBits),
            // 340427604993174052137690718700.4351
            new (UInt128.FromLoHi(0X3b6a656250e923bfUL, 0X3038a7d8012c483aUL), CtorFromBits),
            // 925378172558778760024239791668.7346
            new (UInt128.FromLoHi(0Xf7b486ece93febf2UL, 0X3039c83f25643426UL), CtorFromBits),
            // 2515438670919167006265781174252.113
            new (UInt128.FromLoHi(0X896155a94dbdb251UL, 0X303a7c054cb4a686UL), CtorFromBits),
            // 6837671229762743866755892826677.711
            new (UInt128.FromLoHi(0Xe1dc499c436521cfUL, 0X303b511f8b544577UL), CtorFromBits),
            // 18586717452841279803403701812545.41
            new (UInt128.FromLoHi(0Xc8cb865488bced8dUL, 0X303c5ba3bc50cc93UL), CtorFromBits),
            // 50523936302761041945570383321857.65
            new (UInt128.FromLoHi(0Xd4f50ff9d9778ca5UL, 0X303cf91a2c49f367UL), CtorFromBits),
            // 137338297954017618778418852980853.9
            new (UInt128.FromLoHi(0Xb49c5a984d01149bUL, 0X303e43b687be4933UL), CtorFromBits),
            // 373324199679900164025490831726470.0
            new (UInt128.FromLoHi(0X0228066bf0b76b3cUL, 0X303eb8102379b88cUL), CtorFromBits),
            // 1014800388113888727832461784131717
            new (UInt128.FromLoHi(0X8fbeef9ef8242885UL, 0X3040320894e6a8baUL), CtorFromBits),
            // 2758513454523170206286469819902662
            new (UInt128.FromLoHi(0Xbe67c82472ddaac6UL, 0X3040880155b10c59UL), CtorFromBits),
            // 7498416996990120434675630591224060
            new (UInt128.FromLoHi(0X0971ed0e2388a8fcUL, 0X304171b3540cbf92UL), CtorFromBits),
            // 2.038281066512668766832313753717263E+34
            new (UInt128.FromLoHi(0X52683d6ae284ca0fUL, 0X3042647eb9552737UL), CtorFromBits),
            // 5.540622384393510052571173395831661E+34
            new (UInt128.FromLoHi(0Xf63a1d9c90871b6dUL, 0X3043112c7c79d8feUL), CtorFromBits),
            // 1.506097314585030548352594130167675E+35
            new (UInt128.FromLoHi(0Xb7573496ac22e77bUL, 0X30444a419ee33e2fUL), CtorFromBits),
            // 4.093996962127454696660914229327829E+35
            new (UInt128.FromLoHi(0X170d1d5c4041b7d5UL, 0X3044c9d981a2ec3eUL), CtorFromBits),
            // 1.112863754791759412087071478183941E+36
            new (UInt128.FromLoHi(0X0ffef6c68bd8f805UL, 0X304636de50ba6070UL), CtorFromBits),
            // 3.025077322201142338266566396443429E+36
            new (UInt128.FromLoHi(0X8a342ddd908b7b25UL, 0X30469525d800660bUL), CtorFromBits),
            // 8.223012714622913510304328016407775E+36
            new (UInt128.FromLoHi(0X67f86d8bb5272cdfUL, 0X3047956d03165028UL), CtorFromBits),
            // 2.235246603734715047443065732332715E+37
            new (UInt128.FromLoHi(0X2cd81cae4613fcabUL, 0X30486e34c74ee265UL), CtorFromBits),
            // 6.076030225056872149522328938130276E+37
            new (UInt128.FromLoHi(0X760f74c3a6f8ab64UL, 0X30492b9248837408UL), CtorFromBits),
            // 1.651636254994001855528329796264859E+38
            new (UInt128.FromLoHi(0Xf81d2c4a9f6e239bUL, 0X304a516e94911ee7UL), CtorFromBits),
            // 4.489612819174345246284245579645316E+38
            new (UInt128.FromLoHi(0Xc3c15d424510c184UL, 0X304add5ae169e3fbUL), CtorFromBits),
            // 1.220403294317840802002710035136370E+39
            new (UInt128.FromLoHi(0X792bfdda3acb7f72UL, 0X304c3c2ba7b88e39UL), CtorFromBits),
            // 3.317400098335742625755516107852592E+39
            new (UInt128.FromLoHi(0Xe8ec924a77118b30UL, 0X304ca38f79d56cc1UL), CtorFromBits),
            // 9.017628405034298931400995982170905E+39
            new (UInt128.FromLoHi(0X0f7f8aeee59e5b19UL, 0X304dbc9a78dbda13UL), CtorFromBits),
            // 2.451245542920085785552772943110915E+40
            new (UInt128.FromLoHi(0X572e73fc08cce303UL, 0X304e78db1185b5e4UL), CtorFromBits),
            // 6.663176216410895834244814050240873E+40
            new (UInt128.FromLoHi(0Xd8644080e0092969UL, 0X304f48851b883d51UL), CtorFromBits),
            // 1.811239082889023282193798758098816E+41
            new (UInt128.FromLoHi(0X958bd926da676780UL, 0X3050594d0d1e106aUL), CtorFromBits),
            // 4.923458286012058399754862059113304E+41
            new (UInt128.FromLoHi(0X96935715e06ccb58UL, 0X3050f2bec7b8e719UL), CtorFromBits),
            // 1.338334719204269500461736408706115E+42
            new (UInt128.FromLoHi(0X53c4ccf0e0e4ec43UL, 0X305241fc28b4d516UL), CtorFromBits),
            // 3.637970947608804579287743826760186E+42
            new (UInt128.FromLoHi(0Xc202981d969811faUL, 0X3052b35da61ce4a6UL), CtorFromBits),
            // 9.889030319346946770560030967138037E+42
            new (UInt128.FromLoHi(0X11a45ea47eb106f5UL, 0X3053e7911c4d1ce5UL), CtorFromBits),
            // 2.688117141816135448412625551580014E+43
            new (UInt128.FromLoHi(0Xf186eab2d3895b6eUL, 0X30548488ceeffb54UL), CtorFromBits),
            // 7.307059979368067272647682634061514E+43
            new (UInt128.FromLoHi(0X2fec4d0d306896caUL, 0X30556844102db205UL), CtorFromBits),
            // 1.986264836137654325874046890613771E+44
            new (UInt128.FromLoHi(0X76fda4a75f871c0bUL, 0X305661ee2fd3d950UL), CtorFromBits),
            // 5.399227610580168869761684237193682E+44
            new (UInt128.FromLoHi(0Xe923395e1a5411d2UL, 0X30570a33d5523ba0UL), CtorFromBits),
            // 1.467662230155442328510702112087047E+45
            new (UInt128.FromLoHi(0X8caddc165bd61c07UL, 0X3058485c806e8f98UL), CtorFromBits),
            // 3.989519570547215850763757278730095E+45
            new (UInt128.FromLoHi(0X679dbb778b3c176fUL, 0X3058c4b2d0fa2854UL), CtorFromBits),
            // 1.084463855290023081336100102856874E+46
            new (UInt128.FromLoHi(0X6fae02be1447e0aaUL, 0X305a3577dbb41c44UL), CtorFromBits),
            // 2.947878391455509377387820248707928E+46
            new (UInt128.FromLoHi(0Xe07d7d8eefafb358UL, 0X305a915774cd9374UL), CtorFromBits),
            // 8.013164264000591141056105836293556E+46
            new (UInt128.FromLoHi(0X748b53cf41a1adb4UL, 0X305b8b145a2c6250UL), CtorFromBits),
            // 2.178203880729020635553939331393682E+47
            new (UInt128.FromLoHi(0X09b6538ab6e2a892UL, 0X305c6b64cc5453aaUL), CtorFromBits),
            // 5.920972027664670298955228815588040E+47
            new (UInt128.FromLoHi(0Xe3d34af4bb047ac8UL, 0X305d23ed2c74c0c4UL), CtorFromBits),
            // 1.609487066961518054926233299337351E+48
            new (UInt128.FromLoHi(0X056a44d8c5a04087UL, 0X305e4f5a9531f206UL), CtorFromBits),
            // 4.375039447261341073462574675087939E+48
            new (UInt128.FromLoHi(0X531fd05bb41f5243UL, 0X305ed7b4c2e1ede6UL), CtorFromBits),
            // 1.189259022828200881968195409638927E+49
            new (UInt128.FromLoHi(0X2ce684c34035760fUL, 0X30603aa28f26fcbcUL), CtorFromBits),
            // 3.232741191084859311426235420582919E+49
            new (UInt128.FromLoHi(0X158e3d4138072407UL, 0X30609f62ee26aabfUL), CtorFromBits),
            // 8.787501635837023113106973803049638E+49
            new (UInt128.FromLoHi(0X39aaf059914272a6UL, 0X3061b141dd29f141UL), CtorFromBits),
            // 2.388690601424991425462639294944161E+50
            new (UInt128.FromLoHi(0X1806a857267c13a1UL, 0X306275c5839ae2eaUL), CtorFromBits),
            // 6.493134255664462136224950708771209E+50
            new (UInt128.FromLoHi(0Xee5ff83e89029989UL, 0X30634022e052fd54UL), CtorFromBits),
            // 1.765016885691765583291178205644718E+51
            new (UInt128.FromLoHi(0X93190d86ea4507aeUL, 0X30645705a524ac46UL), CtorFromBits),
            // 4.797813327299302186003488289501133E+51
            new (UInt128.FromLoHi(0X7f9772a11252afcdUL, 0X3064ec8ceaf6fd92UL), CtorFromBits),
            // 1.304180878393632279733879028098649E+52
            new (UInt128.FromLoHi(0X7ab983ca6b9ca659UL, 0X3066404d13b1de9dUL), CtorFromBits),
            // 3.545131182761166475189407421247819E+52
            new (UInt128.FromLoHi(0X20be20dc58d7314bUL, 0X3066aec9d8a75654UL), CtorFromBits),
            // 9.636665673603201271763873014194224E+52
            new (UInt128.FromLoHi(0Xe3d2642b332fa030UL, 0X3067db1fd226dc1dUL), CtorFromBits),
            // 2.619517318749062676188981025374639E+53
            new (UInt128.FromLoHi(0Xde137f65d96501afUL, 0X30688126f4f450ecUL), CtorFromBits),
            // 7.120586326889337708833068068270194E+53
            new (UInt128.FromLoHi(0Xe8bac3a705867072UL, 0X30695f126f4f9cefUL), CtorFromBits),
            // 1.935576042035722568720624490527487E+54
            new (UInt128.FromLoHi(0X391afec0c11a3affUL, 0X306a5f6e67801815UL), CtorFromBits),
            // 5.261441182666385745176776704161635E+54
            new (UInt128.FromLoHi(0Xdc283029d5f67763UL, 0X306b0368b95c63c3UL), CtorFromBits),
            // 1.430207995834810446358367107290526E+55
            new (UInt128.FromLoHi(0X09c326faac9c159eUL, 0X306c4683c34612a8UL), CtorFromBits),
        ];

        static ReadOnlySpan<Decimal128Bid> ExpTable7 => ExpTable7_;
        static readonly Decimal128Bid[] ExpTable7_ = [
            // 1
            new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits),
            // 1.430207995834810446358367107290526E+55
            new (UInt128.FromLoHi(0X09c326faac9c159eUL, 0X306c4683c34612a8UL), CtorFromBits),
            // 2.045494911349825175079419025332923E+110
            new (UInt128.FromLoHi(0X3cdff0eda2e09abbUL, 0X30da64d9c685be94UL), CtorFromBits),
            // 2.925483177651936727218575960774259E+165
            new (UInt128.FromLoHi(0X55f0812d8db51673UL, 0X3148903cc9f6e759UL), CtorFromBits),
            // 4.184049432358029151851648927429493E+220
            new (UInt128.FromLoHi(0Xd78bfa828af47375UL, 0X31b6ce4a20dc68f9UL), CtorFromBits),
            // 5.984060953126553169633576189715827E+275
            new (UInt128.FromLoHi(0X5cf90cc99d886573UL, 0X3225270977c4c30aUL), CtorFromBits),
            // 8.558451822724473185317785446579883E+330
            new (UInt128.FromLoHi(0Xf464c24c565efeabUL, 0X3293a5f6d94d3a0fUL), CtorFromBits),
            // 1.224036622882754921806981137893847E+386
            new (UInt128.FromLoHi(0X2657355c93fb09d7UL, 0X33023c5983a380e5UL), CtorFromBits),
            // 1.750626965241554596301685175972517E+441
            new (UInt128.FromLoHi(0Xf524e548b3e972a5UL, 0X3370565004cd75d9UL), CtorFromBits),
            // 2.503760683412500168119963382399243E+496
            new (UInt128.FromLoHi(0X32c969b18cdf410bUL, 0X33de7b71e717ca95UL), CtorFromBits),
            // 3.580898549073387197078135289400230E+551
            new (UInt128.FromLoHi(0Xdb014f9eb3b7cba6UL, 0X344cb08d4b3f5011UL), CtorFromBits),
            // 5.121429737158029727107773237847608E+606
            new (UInt128.FromLoHi(0X16d3403bbf586638UL, 0X34bafc8187fb2132UL), CtorFromBits),
            // 7.324709760189585739027115040481786E+661
            new (UInt128.FromLoHi(0X5a527cc13f02a1faUL, 0X35296922d5b234eeUL), CtorFromBits),
            // 1.047585846619242246404391961653609E+717
            new (UInt128.FromLoHi(0X71969846ad607169UL, 0X359833a6646e4ef2UL), CtorFromBits),
            // 1.498265654158219589864238908065052E+772
            new (UInt128.FromLoHi(0Xdb419d0df91e451cUL, 0X360649dec56d92e1UL), CtorFromBits),
            // 2.142831518461758471900689629718639E+827
            new (UInt128.FromLoHi(0X8ab908fd34dc286fUL, 0X367469a656100a04UL), CtorFromBits),
            // 3.064694771430855204669187310990986E+882
            new (UInt128.FromLoHi(0Xfba9966170dfb68aUL, 0X36e29719e2e251faUL), CtorFromBits),
            // 4.383150966893545913495579113869240E+937
            new (UInt128.FromLoHi(0X170f4d0fcefccfb8UL, 0X3750d81b249db518UL), CtorFromBits),
            // 6.268817559802229894509465077200837E+992
            new (UInt128.FromLoHi(0Xd02bb4f0a6dab7c5UL, 0X37bf351399fd19b2UL), CtorFromBits),
            // 8.965712998458814199193112781654289E+1047
            new (UInt128.FromLoHi(0X9f00bc6d59460911UL, 0X382dba0b35220a30UL), CtorFromBits),
            // 1.282283441875588961606992907557294E+1103
            new (UInt128.FromLoHi(0X2e1d202ed15959aeUL, 0X389c3f38b142ead1UL), CtorFromBits),
            // 1.833932031497048740694173009640043E+1158
            new (UInt128.FromLoHi(0Xbd4d490903d0fa6bUL, 0X390a5a6b79fd31c0UL), CtorFromBits),
            // 2.622904255264656545650446312298078E+1213
            new (UInt128.FromLoHi(0Xbc41a79d443a565eUL, 0X39788151b4bc89aaUL), CtorFromBits),
            // 3.751298638188660504611099180737453E+1268
            new (UInt128.FromLoHi(0Xffd564071db323adUL, 0X39e6b8f40ba167feUL), CtorFromBits),
            // 5.365137307101657862605338763576192E+1323
            new (UInt128.FromLoHi(0X525168dd261fc380UL, 0X3a5508858d9c1e94UL), CtorFromBits),
            // 7.673262275368434022968229733933559E+1378
            new (UInt128.FromLoHi(0X0e69762f26f121f7UL, 0X3ac37a522f9d3824UL), CtorFromBits),
            // 1.097436106036954541540431782260949E+1434
            new (UInt128.FromLoHi(0Xed13fc646f0cb0d5UL, 0X3b32361b974e6df0UL), CtorFromBits),
            // 1.569561893771871276293183466803444E+1489
            new (UInt128.FromLoHi(0X44b99671048290f4UL, 0X3ba04d62a800bae8UL), CtorFromBits),
            // 2.244799970430157670592144796225470E+1544
            new (UInt128.FromLoHi(0Xa72a8e5f6ed30fbeUL, 0X3c0e6ead5be69974UL), CtorFromBits),
            // 3.210530866758957554891511080445576E+1599
            new (UInt128.FromLoHi(0X9150d9970f686688UL, 0X3c7c9e4a98b8fc6fUL), CtorFromBits),
            // 4.591726916513125538797851095365756E+1654
            new (UInt128.FromLoHi(0Xdb903bf616bc787cUL, 0X3ceae263bddb7a40UL), CtorFromBits),
            // 6.567124550686991264757420313848293E+1709
            new (UInt128.FromLoHi(0X67ccfd402850a9e5UL, 0X3d5943c8c3dc8c9eUL), CtorFromBits),
            // 9.392354042035621826801222481017776E+1764
            new (UInt128.FromLoHi(0X803ecc70854afbb0UL, 0X3dc7cf142cb1ce51UL), CtorFromBits),
            // 1.343301985063074768167518462193741E+1820
            new (UInt128.FromLoHi(0Xd0850dbcef36b04dUL, 0X3e36423adaced921UL), CtorFromBits),
            // 1.921201239857982642460542435889542E+1875
            new (UInt128.FromLoHi(0X79415775b4509186UL, 0X3ea45eb8f8026c61UL), CtorFromBits),
            // 2.747717374852638304408385061109619E+1930
            new (UInt128.FromLoHi(0Xe142a2700fefbf73UL, 0X3f12877911ad40d1UL), CtorFromBits),
            // 3.929807359808478417975487753895247E+1985
            new (UInt128.FromLoHi(0Xd5cb9fe36d1c3d4fUL, 0X3f80c1c1246b1a0dUL), CtorFromBits),
            // 5.620441908088571738317606338080649E+2040
            new (UInt128.FromLoHi(0X2a6d04f6183eaf89UL, 0X3fef151bf34459f6UL), CtorFromBits),
            // 8.038400957073334086295581522649681E+2095
            new (UInt128.FromLoHi(0X58261dc3876fe251UL, 0X405d8c52e2539e67UL), CtorFromBits),
            // 1.149658532253247530250787600838303E+2151
            new (UInt128.FromLoHi(0X5df826ebd66dae9fUL, 0X40cc38aebb10efa1UL), CtorFromBits),
            // 1.644250825308306934948719540018337E+2206
            new (UInt128.FromLoHi(0X8030914f7e6b9ca1UL, 0X413a51115cf4de8dUL), CtorFromBits),
            // 2.351620677513926683678344812942179E+2261
            new (UInt128.FromLoHi(0Xa8266f5e128bd363UL, 0X41a873f1a03884b1UL), CtorFromBits),
            // 3.363306696150892174301571565494757E+2316
            new (UInt128.FromLoHi(0X151b8568932421e5UL, 0X4216a5d2e60d2034UL), CtorFromBits),
            // 4.810228129279765278322852464992324E+2371
            new (UInt128.FromLoHi(0X4510d3fcf940a044UL, 0X4284ed299d5a7f58UL), CtorFromBits),
            // 6.879626732285442584470357837507002E+2426
            new (UInt128.FromLoHi(0X6ecef21ee36e69baUL, 0X42f3533118de8c4dUL), CtorFromBits),
            // 9.839297160873548869581133474250227E+2481
            new (UInt128.FromLoHi(0Xe002d9050dcd3df3UL, 0X4361e51d63cc9f65UL), CtorFromBits),
            // 1.407224147287609883201961093876676E+2537
            new (UInt128.FromLoHi(0X67c5b1eb1131cbc4UL, 0X43d04561aa74271cUL), CtorFromBits),
            // 2.012623227382562637919903099984043E+2592
            new (UInt128.FromLoHi(0X16e2646aaab518abUL, 0X443e633ae0621094UL), CtorFromBits),
            // 2.878469832405402903143790075670125E+2647
            new (UInt128.FromLoHi(0X2f170fffcab76e6dUL, 0X44ac8deb65a72f3cUL), CtorFromBits),
            // 4.116810570075493998891907758470400E+2702
            new (UInt128.FromLoHi(0Xedddcbf2f168b500UL, 0X451acaf974611eacUL), CtorFromBits),
            // 5.887895394659235740367294511113189E+2757
            new (UInt128.FromLoHi(0Xfedb33e71952e3e5UL, 0X4589224bb01733ceUL), CtorFromBits),
            // 8.420915072080595838989899493362101E+2812
            new (UInt128.FromLoHi(0X8238cb6970dd49b5UL, 0X45f79f2ee41794a5UL), CtorFromBits),
            // 1.204366006833553732335659031506654E+2868
            new (UInt128.FromLoHi(0X8ab1f8af5090aedeUL, 0X46663b613c734547UL), CtorFromBits),
            // 1.722493892884990505992112771498853E+2923
            new (UInt128.FromLoHi(0X06159a25a61d0365UL, 0X46d454ecedf16f7aUL), CtorFromBits),
            // 2.463524538380742932737895397471100E+2978
            new (UInt128.FromLoHi(0X8bff6767270e337cUL, 0X474279760d18b27bUL), CtorFromBits),
            // 3.523352492727398915941903524985876E+3033
            new (UInt128.FromLoHi(0X2b16e7a794587c14UL, 0X47b0adb6f5e92cabUL), CtorFromBits),
            // 5.039126907243236752266068663174442E+3088
            new (UInt128.FromLoHi(0Xe9ef2836f44f852aUL, 0X481ef872b931ddabUL), CtorFromBits),
            // 7.206999594765616395460950205952491E+3143
            new (UInt128.FromLoHi(0X91bdbb0b0a5071ebUL, 0X488d63551fa377b7UL), CtorFromBits),
            // 1.030750844641202326850218778042996E+3199
            new (UInt128.FromLoHi(0X02ee091b508e2274UL, 0X48fc32d1e79b0f53UL), CtorFromBits),
            // 1.474188099719332046972972086841627E+3254
            new (UInt128.FromLoHi(0Xdcdc2cc8cf57d11bUL, 0X496a48aedea7581eUL), CtorFromBits),
            // 2.108395607583113575183348813366864E+3309
            new (UInt128.FromLoHi(0X575bb371eb418650UL, 0X49d867f3b1a22f49UL), CtorFromBits),
            // 3.015444256348362340486738724432129E+3364
            new (UInt128.FromLoHi(0X8a2d9302fd4ea901UL, 0X4a4694ac41e315f3UL), CtorFromBits),
            // 4.312712486423581690131996647333665E+3419
            new (UInt128.FromLoHi(0X70d5b53da29f9f21UL, 0X4ab4d4a2159c48f2UL), CtorFromBits),
            // 6.168075881819632925568967684321681E+3474
            new (UInt128.FromLoHi(0X79db5733ffb2f191UL, 0X4b23301c10123276UL), CtorFromBits),
            // 8.821631445094288338188104071480182E+3529
            new (UInt128.FromLoHi(0X5dfcfccdf6b18376UL, 0X4b91b2f0a486785bUL), CtorFromBits),
            // 1.261676782908164479440006015914000E+3585
            new (UInt128.FromLoHi(0Xb4313ea1bb409010UL, 0X4c003e34998e2141UL), CtorFromBits),
            // 1.804460223074397147569663151957385E+3640
            new (UInt128.FromLoHi(0Xf79b54cbb71fe589UL, 0X4c6e58f77d70a676UL), CtorFromBits),
            // 2.580753439206868524519983223694100E+3695
            new (UInt128.FromLoHi(0X33d521f480746314UL, 0X4cdc7f3db01ab104UL), CtorFromBits),
            // 3.691014204031849753207510487649706E+3750
            new (UInt128.FromLoHi(0X5232741b4cc9cdaaUL, 0X4d4ab5fb261e3ba5UL), CtorFromBits),
            // 5.278918027346209966859059967430248E+3805
            new (UInt128.FromLoHi(0Xb7b19a57391e0e68UL, 0X4db9044550177a3aUL), CtorFromBits),
            // 7.549950772067074042248632925530310E+3860
            new (UInt128.FromLoHi(0Xa8bcf6dfc08da8c6UL, 0X4e27743dc6a86236UL), CtorFromBits),
            // 1.079799996236952974549397502940984E+3916
            new (UInt128.FromLoHi(0X32463af5719bd738UL, 0X4e96353cfdf65271UL), CtorFromBits),
            // 1.544338588520488375475913666607599E+3971
            new (UInt128.FromLoHi(0X8cbe24c80f7039efUL, 0X4f044c244b1ba443UL), CtorFromBits),
            // 2.208725397578247682321453189426037E+4026
            new (UInt128.FromLoHi(0X4855b53dfbb7eb75UL, 0X4f726ce608ab5488UL), CtorFromBits),
            // 3.158936724219830508381721310641840E+4081
            new (UInt128.FromLoHi(0X26c57ba6a21dbeb0UL, 0X4fe09bbf630e8e71UL), CtorFromBits),
            // 4.517936561315425107396247055060861E+4136
            new (UInt128.FromLoHi(0Xaa6388b478d14f7dUL, 0X504edec0605c43bdUL), CtorFromBits),
            // 6.461588994667749342792362408129023E+4191
            new (UInt128.FromLoHi(0X04a6f5a36d81b1ffUL, 0X50bd3e94b816acd8UL), CtorFromBits),
            // 9.241416245972029471540528748235660E+4246
            new (UInt128.FromLoHi(0X755a6c8464110b8cUL, 0X512bc7a3125f335dUL), CtorFromBits),
            // 1.321714740782691391785626441625255E+4302
            new (UInt128.FromLoHi(0X77bb96d727d42ea7UL, 0X519a412a62a8e6a7UL), CtorFromBits),
            // 1.890326990480139058860712341070300E+4357
            new (UInt128.FromLoHi(0X9c7e6bff679ce1dcUL, 0X52085d3347ede92fUL), CtorFromBits),
            // 2.703560776527048489380360038875015E+4412
            new (UInt128.FromLoHi(0X2aab5ec22e4b7f87UL, 0X5276854bbc0333b5UL), CtorFromBits),
            // 3.866654239814353861873982458928929E+4467
            new (UInt128.FromLoHi(0X74a247b246419f21UL, 0X52e4bea409ae9ed2UL), CtorFromBits),
            // 5.530119810911059560864188794697673E+4522
            new (UInt128.FromLoHi(0Xe35f6fe6a5c477c9UL, 0X535310a7ecd4ae58UL), CtorFromBits),
            // 7.909221571489487405631062261309850E+4577
            new (UInt128.FromLoHi(0Xe215a6aca1cf219aUL, 0X53c185f469385281UL), CtorFromBits),
            // 1.131183193237342973642771798736889E+4633
            new (UInt128.FromLoHi(0Xdef84bbaf1e8bff9UL, 0X543037c58a06b116UL), CtorFromBits),
            // 1.617827247722001399920515820971845E+4688
            new (UInt128.FromLoHi(0Xe9e25efeb18a6745UL, 0X549e4fc3d9c5c691UL), CtorFromBits),
            // 2.313829465571431026369046635220119E+4743
            new (UInt128.FromLoHi(0Xf5d2f9a972fe0c97UL, 0X550c7214a23aacb3UL), CtorFromBits),
            // 3.309257402658446906454894670541463E+4798
            new (UInt128.FromLoHi(0X4ae91aa963387297UL, 0X557aa328b35d6d39UL), CtorFromBits),
            // 4.732926397557647669322836115006689E+4853
            new (UInt128.FromLoHi(0Xb1481fed0b8d24e1UL, 0X55e8e959edfcb33bUL), CtorFromBits),
            // 6.769069177484592568585034751407268E+4908
            new (UInt128.FromLoHi(0X17cc1e3cb3b5f4a4UL, 0X56574dbdaa24a541UL), CtorFromBits),
            // 9.681176861997427942394480159308885E+4963
            new (UInt128.FromLoHi(0X7f0f0ca1da1b3c55UL, 0X56c5dd51a18ef0f4UL), CtorFromBits),
            // 1.384609655711968069008735358635763E+5019
            new (UInt128.FromLoHi(0X1b0f423f5cf34ef3UL, 0X573444443b17289eUL), CtorFromBits),
            // 1.980279800709340754198018103031031E+5074
            new (UInt128.FromLoHi(0X7354a774d945b4f7UL, 0X57a261a2a52293bfUL), CtorFromBits),
            // 2.832212004964664082177958319293804E+5129
            new (UInt128.FromLoHi(0Xc0dd890a0fdd056cUL, 0X58108ba38a8d258eUL), CtorFromBits),
            // 4.050652255399802430866330061166611E+5184
            new (UInt128.FromLoHi(0Xb52d949816a0d813UL, 0X587ec7b66b55dc64UL), CtorFromBits),
            // 5.793275244019106175418295536820204E+5239
            new (UInt128.FromLoHi(0X498397d269db9fecUL, 0X58ed1da169e50938UL), CtorFromBits),
            // 8.285588576067988277173586895792318E+5294
            new (UInt128.FromLoHi(0X814da8b44ddb68beUL, 0X595b9882d499b6dbUL), CtorFromBits),
            // 1.185011503168999839510904461138437E+5350
            new (UInt128.FromLoHi(0X2474fe7c1cb51a05UL, 0X59ca3a6cf2ad12dfUL), CtorFromBits),
            // 1.694812926988531388522524763894059E+5405
            new (UInt128.FromLoHi(0X1df84648a7b6312bUL, 0X5a38538f8beb1bf2UL), CtorFromBits),
            // 2.423934999623196401246592686287728E+5460
            new (UInt128.FromLoHi(0Xfed8af64d756eb70UL, 0X5aa677825c65c6ebUL), CtorFromBits),
            // 3.466731217844943739497213820182726E+5515
            new (UInt128.FromLoHi(0X53b429ca4800a0c6UL, 0X5b14aaec4cb4556cUL), CtorFromBits),
            // 4.958146707171988641927735566157260E+5570
            new (UInt128.FromLoHi(0X03819c2907b445ccUL, 0X5b82f4749c0e5dc6UL), CtorFromBits),
            // 7.091181065119414661458755548809864E+5625
            new (UInt128.FromLoHi(0X221bf9afa7270a88UL, 0X5bf15d9f49c92b7fUL), CtorFromBits),
            // 1.014186385924619450871887019103000E+5681
            new (UInt128.FromLoHi(0Xbb3087a62247d318UL, 0X5c603200d4f3f3baUL), CtorFromBits),
            // 1.450497478416199595493793262997702E+5736
            new (UInt128.FromLoHi(0X7be33063962010c6UL, 0X5cce4783da20b3f3UL), CtorFromBits),
            // 2.074513091569079046389302641027861E+5791
            new (UInt128.FromLoHi(0Xc8e3bb5f4dc7eb15UL, 0X5d3c66480951d558UL), CtorFromBits),
            // 2.966985211026089146882015493860107E+5846
            new (UInt128.FromLoHi(0X6f11c92dde6bab0bUL, 0X5daa92489e444ad6UL), CtorFromBits),
            // 4.243405972333145099808751386826124E+5901
            new (UInt128.FromLoHi(0X031155c94da7158cUL, 0X5e18d137502f8176UL), CtorFromBits),
            // 6.068953151204052559084742849124490E+5956
            new (UInt128.FromLoHi(0X17dd72228facd88aUL, 0X5e872b38f5412b29UL), CtorFromBits),
            // 8.679865323198905335487882844272619E+6011
            new (UInt128.FromLoHi(0X69a5ead66ea577ebUL, 0X5ef5abf34d7aebe4UL), CtorFromBits),
            // 1.241401278800837563085311780687860E+6067
            new (UInt128.FromLoHi(0X92c59cda0ee18ff4UL, 0X5f643d34afde38fbUL), CtorFromBits),
            // 1.775462034980516651086504712290882E+6122
            new (UInt128.FromLoHi(0X8a82dbfb6b7d4242UL, 0X5fd257897b3da8eaUL), CtorFromBits),
            // ∞
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X7800000000000000UL), CtorFromBits),
            // ∞
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X7800000000000000UL), CtorFromBits),
            // ∞
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X7800000000000000UL), CtorFromBits),
            // ∞
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X7800000000000000UL), CtorFromBits),
            // ∞
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X7800000000000000UL), CtorFromBits),
            // ∞
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X7800000000000000UL), CtorFromBits),
            // ∞
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X7800000000000000UL), CtorFromBits),
            // ∞
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X7800000000000000UL), CtorFromBits),
            // ∞
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X7800000000000000UL), CtorFromBits),
            // ∞
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X7800000000000000UL), CtorFromBits),
            // ∞
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X7800000000000000UL), CtorFromBits),
            // ∞
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X7800000000000000UL), CtorFromBits),
            // ∞
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X7800000000000000UL), CtorFromBits),
            // ∞
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X7800000000000000UL), CtorFromBits),
            // ∞
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X7800000000000000UL), CtorFromBits),
            // ∞
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X7800000000000000UL), CtorFromBits),
        ];

        static ReadOnlySpan<Decimal128Bid> ExpMTable0 => ExpMTable0_;
        static readonly Decimal128Bid[] ExpMTable0_ = [
            // 1
            new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits),
            // 0.3678794411714423215955237701614609
            new (UInt128.FromLoHi(0X8b6eb0ca0a12fc11UL, 0X2ffcb560e9d6f047UL), CtorFromBits),
            // 0.1353352832366126918939994949724844
            new (UInt128.FromLoHi(0X4678db75acd8aaacUL, 0X2ffc42b9b6d8e082UL), CtorFromBits),
            // 0.04978706836786394297934241565006178
            new (UInt128.FromLoHi(0X1f4468ac76105962UL, 0X2ffaf5781d6af994UL), CtorFromBits),
            // 0.01831563888873418029371802127324124
            new (UInt128.FromLoHi(0Xf146c743891e53dcUL, 0X2ffa5a4d961b873fUL), CtorFromBits),
            // 0.006737946999085467096636048423148424
            new (UInt128.FromLoHi(0X6ddff465784e5b88UL, 0X2ff94c34d8f60da8UL), CtorFromBits),
            // 0.002478752176666358423045167430816668
            new (UInt128.FromLoHi(0Xef49111792c0d39cUL, 0X2ff87a36403fdcadUL), CtorFromBits),
            // 9.118819655545162080031360844092826E-4
            new (UInt128.FromLoHi(0X32855834c8e8d59aUL, 0X2ff7c197af6c6f37UL), CtorFromBits),
            // 3.354626279025118388213891257808610E-4
            new (UInt128.FromLoHi(0Xddd960905a0336e2UL, 0X2ff6a565561c5ed7UL), CtorFromBits),
            // 1.234098040866795494976366907300338E-4
            new (UInt128.FromLoHi(0X39b7a9407e8519f2UL, 0X2ff63cd881d5650aUL), CtorFromBits),
            // 4.539992976248485153559151556055061E-5
            new (UInt128.FromLoHi(0Xefd0959d73f65015UL, 0X2ff4dfd6c47b1464UL), CtorFromBits),
            // 1.670170079024565931263551736058088E-5
            new (UInt128.FromLoHi(0X0cd67435b73310e8UL, 0X2ff4525882950908UL), CtorFromBits),
            // 6.144212353328209758682308178805532E-6
            new (UInt128.FromLoHi(0Xf083ebd62b03031cUL, 0X2ff32eeedcda14f7UL), CtorFromBits),
            // 2.260329406981054325785277290538689E-6
            new (UInt128.FromLoHi(0X8cf5043fb62c66c1UL, 0X2ff26f715e37702eUL), CtorFromBits),
            // 8.315287191035678840639851425652623E-7
            new (UInt128.FromLoHi(0X8957707869fb5f8fUL, 0X2ff199f9ae003c38UL), CtorFromBits),
            // 3.059023205018257883714794977022896E-7
            new (UInt128.FromLoHi(0X4a95be0ebd00d3b0UL, 0X2ff096d24d108a7aUL), CtorFromBits),
            // 1.125351747192591145137751790601272E-7
            new (UInt128.FromLoHi(0X4635127cb5d64438UL, 0X2ff0377bef9b9494UL), CtorFromBits),
            // 4.139937718785166659651027718955281E-8
            new (UInt128.FromLoHi(0X0cf69d1b0860e111UL, 0X2feecc1d5c3a27f6UL), CtorFromBits),
            // 1.522997974471262843613662923351743E-8
            new (UInt128.FromLoHi(0X4cc06910a0ee66bfUL, 0X2fee4b16efdd70aeUL), CtorFromBits),
            // 5.602796437537267540012982816206463E-9
            new (UInt128.FromLoHi(0X83fb6373a6c7ee7fUL, 0X2fed143d3bad3645UL), CtorFromBits),
            // 2.061153622438557827965940380155821E-9
            new (UInt128.FromLoHi(0X93ec50ba02cf3fadUL, 0X2fec659f6a8bf980UL), CtorFromBits),
            // 7.582560427911906727941743241268126E-10
            new (UInt128.FromLoHi(0X4538db84cd5b0b9eUL, 0X2feb75d95e237b03UL), CtorFromBits),
            // 2.789468092868924807718913030644293E-10
            new (UInt128.FromLoHi(0X417a4932a8fe6245UL, 0X2fea89880985c1a6UL), CtorFromBits),
            // 1.026187963170189030392752784061250E-10
            new (UInt128.FromLoHi(0Xb01c554b3784b342UL, 0X2fea32985023f83bUL), CtorFromBits),
            // 3.775134544279097751644969547523407E-11
            new (UInt128.FromLoHi(0Xa036e7f36c5ae54fUL, 0X2fe8ba20e598ce8fUL), CtorFromBits),
            // 1.388794386496402059466176374608686E-11
            new (UInt128.FromLoHi(0X9eeb86efa1fd932eUL, 0X2fe844790caf5511UL), CtorFromBits),
            // 5.109089028063324719874400193479216E-12
            new (UInt128.FromLoHi(0X8cb1941f1a5e0230UL, 0X2fe6fbe5c4ffd245UL), CtorFromBits),
            // 1.879528816539083294758270418422193E-12
            new (UInt128.FromLoHi(0X2907425c433209b1UL, 0X2fe65caafd25c69fUL), CtorFromBits),
            // 6.914400106940203009412584658741409E-13
            new (UInt128.FromLoHi(0X9d472dd8fecca8a1UL, 0X2fe554e7ffb3e540UL), CtorFromBits),
            // 2.543665647376922910303385614857682E-13
            new (UInt128.FromLoHi(0X57f7ae7d5990b5d2UL, 0X2fe47d6992fc53ccUL), CtorFromBits),
            // 9.357622968840174604915832223378707E-14
            new (UInt128.FromLoHi(0Xa74de8a68da72d13UL, 0X2fe3cd5dce8b67ceUL), CtorFromBits),
            // 3.442477108469976458392389332851557E-14
            new (UInt128.FromLoHi(0X4c74af2eca4d5b65UL, 0X2fe2a9ba2b7348b2UL), CtorFromBits),
            // 1.266416554909417572312090415596510E-14
            new (UInt128.FromLoHi(0Xeeb152b373e603deUL, 0X2fe23e706c95a323UL), CtorFromBits),
            // 4.658886145103397364184245543610168E-15
            new (UInt128.FromLoHi(0X05fcec7f15dfa738UL, 0X2fe0e5b36907524aUL), CtorFromBits),
            // 1.713908431542012966302720342576049E-15
            new (UInt128.FromLoHi(0Xb20d511cb5f34bb1UL, 0X2fe0548090d24341UL), CtorFromBits),
            // 6.305116760146989385639021192246543E-16
            new (UInt128.FromLoHi(0X8dc312ed8245750fUL, 0X2fdf36ddc307e072UL), CtorFromBits),
            // 2.319522830243569388312263609738080E-16
            new (UInt128.FromLoHi(0X3122abae7386e360UL, 0X2fde725c7e7b8d36UL), CtorFromBits),
            // 8.533047625744065794278049822941244E-17
            new (UInt128.FromLoHi(0X95dd4eb5fefa203cUL, 0X2fdda4b633ea01d3UL), CtorFromBits),
            // 3.139132792048029628708964652231920E-17
            new (UInt128.FromLoHi(0X0c0c44cd15872cf0UL, 0X2fdc9ac56d19cfc4UL), CtorFromBits),
            // 1.154822417301578598626244206332387E-17
            new (UInt128.FromLoHi(0Xaa3c9f45220d09e3UL, 0X2fdc38efe87a8892UL), CtorFromBits),
            // 4.248354255291588995329234782858658E-18
            new (UInt128.FromLoHi(0Xe7cfa73d1a5b2da2UL, 0X2fdad175c4f33bfeUL), CtorFromBits),
            // 1.562882189334988768090882995105834E-18
            new (UInt128.FromLoHi(0Xd08568aad3d3b02aUL, 0X2fda4d0e58b6b8eaUL), CtorFromBits),
            // 5.749522264293559806664380880573423E-19
            new (UInt128.FromLoHi(0Xb5005d8babd4d3efUL, 0X2fd91b792c647732UL), CtorFromBits),
            // 2.115131037591080486631401007022651E-19
            new (UInt128.FromLoHi(0X6e30f4715133023bUL, 0X2fd86848b4fb4df4UL), CtorFromBits),
            // 7.781132241133796515713316729279898E-20
            new (UInt128.FromLoHi(0X34dbe8f80ea6559aUL, 0X2fd77fa3b24593c1UL), CtorFromBits),
            // 2.862518580549393644470121629183937E-20
            new (UInt128.FromLoHi(0Xc01357c155b36fc1UL, 0X2fd68d221060e73cUL), CtorFromBits),
            // 1.053061735755381237876332444942811E-20
            new (UInt128.FromLoHi(0X06ec0c81108e81dbUL, 0X2fd633eb81fba367UL), CtorFromBits),
            // 3.873997628687187112931477497269128E-21
            new (UInt128.FromLoHi(0X0a458ae5099b4b88UL, 0X2fd4bf00b97386c5UL), CtorFromBits),
            // 1.425164082740935106285321028034060E-21
            new (UInt128.FromLoHi(0Xa6df54200b72de0cUL, 0X2fd446441982f341UL), CtorFromBits),
            // 5.242885663363463937171805302832344E-22
            new (UInt128.FromLoHi(0Xa21513e58e523cd8UL, 0X2fd3027e853e9104UL), CtorFromBits),
            // 1.928749847963917783017342816527013E-22
            new (UInt128.FromLoHi(0X4d53fb976fa0a2a5UL, 0X2fd25f183ee0b34aUL), CtorFromBits),
            // 7.095474162284704138983269387808073E-23
            new (UInt128.FromLoHi(0X0b39e07490152d49UL, 0X2fd15dd57987e9b0UL), CtorFromBits),
            // 2.610279069667704804702695315331865E-23
            new (UInt128.FromLoHi(0X58b3ae1b9af1db19UL, 0X2fd080b25a8fc836UL), CtorFromBits),
            // 9.602680054508676030230769670007491E-24
            new (UInt128.FromLoHi(0X6f3a35e20f07aac3UL, 0X2fcfd972dcb1a3a4UL), CtorFromBits),
            // 3.532628572200807029735392810177209E-24
            new (UInt128.FromLoHi(0X08284b611af112b9UL, 0X2fceae2c0a8a4680UL), CtorFromBits),
            // 1.299581425007503073600713406071486E-24
            new (UInt128.FromLoHi(0X1fe8b33a06a4c2beUL, 0X2fce4013060f3343UL), CtorFromBits),
            // 4.780892883885469081277177042317963E-25
            new (UInt128.FromLoHi(0Xb41b33136e48328bUL, 0X2fccebb75a103e27UL), CtorFromBits),
            // 1.758792202424311648955875128803436E-25
            new (UInt128.FromLoHi(0X303b90797970806cUL, 0X2fcc56b7141b5ad6UL), CtorFromBits),
            // 6.470234925645460326154039552926489E-26
            new (UInt128.FromLoHi(0Xb7767651f36c6319UL, 0X2fcb3f01d899284bUL), CtorFromBits),
            // 2.380266408694400605894324588802496E-26
            new (UInt128.FromLoHi(0Xef92d8994e8a71c0UL, 0X2fca755b2f91fbb4UL), CtorFromBits),
            // 8.756510762696520338488732800739166E-27
            new (UInt128.FromLoHi(0X6fbd0153372f675eUL, 0X2fc9afbab44083b5UL), CtorFromBits),
            // 3.221340285992516089001247775848944E-27
            new (UInt128.FromLoHi(0X1b07bccd7ccef9f0UL, 0X2fc89ed307d70250UL), CtorFromBits),
            // 1.185064864233981006285030739097281E-27
            new (UInt128.FromLoHi(0X14169647726f36c1UL, 0X2fc83a6d9f184fe8UL), CtorFromBits),
            // 4.359610000063080973623124815888460E-28
            new (UInt128.FromLoHi(0X202e7db17c61c04cUL, 0X2fc6d6f203a65830UL), CtorFromBits),
            // 1.603810890548637852976087034142335E-28
            new (UInt128.FromLoHi(0X3c8cee79f9e69e7fUL, 0X2fc64f12f07adcfdUL), CtorFromBits),
            // 5.900090541597061391401260295558423E-29
            new (UInt128.FromLoHi(0X857baaf037829517UL, 0X2fc522e59cbc510bUL), CtorFromBits),
            // 2.170522011303639411986569259572706E-29
            new (UInt128.FromLoHi(0X9f856144e5eedde2UL, 0X2fc46b03d6dedeb6UL), CtorFromBits),
            // 7.984904245686978808392694266474241E-30
            new (UInt128.FromLoHi(0X860c8f99b68e2f01UL, 0X2fc389afa9211fe7UL), CtorFromBits),
            // 2.937482111710802946608880642392872E-30
            new (UInt128.FromLoHi(0Xfd312b8aadf38328UL, 0X2fc290d43c9c874bUL), CtorFromBits),
            // 1.080639277707278494536649616247343E-30
            new (UInt128.FromLoHi(0Xe25dcc685841ca2fUL, 0X2fc2354795d3aec9UL), CtorFromBits),
            // 3.975449735908646807789099753794825E-31
            new (UInt128.FromLoHi(0Xfca0b46df519ed09UL, 0X2fc0c4013ae3b901UL), CtorFromBits),
            // 1.462486227251230946826378730831583E-31
            new (UInt128.FromLoHi(0Xafbf361c8df07edfUL, 0X2fc0481b2bdd4edbUL), CtorFromBits),
            // 5.380186160021138413818187270454184E-32
            new (UInt128.FromLoHi(0X1a811b3494e1fba8UL, 0X2fbf09437f137ee0UL), CtorFromBits),
            // 1.979259877946904553749191533601551E-32
            new (UInt128.FromLoHi(0X69c999187965230fUL, 0X2fbe6195c5963de3UL), CtorFromBits),
            // 7.281290178321643834296973716877553E-33
            new (UInt128.FromLoHi(0X9c8034a5fc5ff0f1UL, 0X2fbd66fecd75e164UL), CtorFromBits),
            // 2.678636961808077944344415201077540E-33
            new (UInt128.FromLoHi(0X76bd18a148c32924UL, 0X2fbc841126d2e026UL), CtorFromBits),
            // 9.854154686111258028938097973614111E-34
            new (UInt128.FromLoHi(0Xffc5711ab259561fUL, 0X2fbbe5d8eb0d7325UL), CtorFromBits),
            // 3.625140919143559224240833197274628E-34
            new (UInt128.FromLoHi(0X4398620b7c987604UL, 0X2fbab2bbb60e1695UL), CtorFromBits),
            // 1.333614815502261341453014079127397E-34
            new (UInt128.FromLoHi(0X589eec3881603f65UL, 0X2fba41c095dfff38UL), CtorFromBits),
            // 4.906094730649280566135387351928824E-35
            new (UInt128.FromLoHi(0Xc9078f6be4e3cff8UL, 0X2fb8f1e39f0c8aa9UL), CtorFromBits),
            // 1.804851387845415172312128357350027E-35
            new (UInt128.FromLoHi(0Xd81594c08d773a8bUL, 0X2fb858fc6d5c90adUL), CtorFromBits),
            // 6.639677199580734400702255270428292E-36
            new (UInt128.FromLoHi(0X1f538bb5c44f7e84UL, 0X2fb7475c821d1a83UL), CtorFromBits),
            // 2.442600737740527679440802607414812E-36
            new (UInt128.FromLoHi(0Xcc6e4aff43ae3e1cUL, 0X2fb6786df4a67778UL), CtorFromBits),
            // 8.985825944049380669668848172735835E-37
            new (UInt128.FromLoHi(0X5de7ccfaa18b1d5bUL, 0X2fb5bb091190b193UL), CtorFromBits),
            // 3.305700626760734298455096425735447E-37
            new (UInt128.FromLoHi(0X168f56ad61946517UL, 0X2fb4a2fbcecd5c62UL), CtorFromBits),
            // 1.216099299252825564416826336765760E-37
            new (UInt128.FromLoHi(0Xa71e17a350d26740UL, 0X2fb43bf554c34800UL), CtorFromBits),
            // 4.473779306181120734627655839169744E-38
            new (UInt128.FromLoHi(0X30d79c6b7c78bcd0UL, 0X2fb2dc930892e9f9UL), CtorFromBits),
            // 1.645811431082273651166034277667247E-38
            new (UInt128.FromLoHi(0Xe853f647facf91afUL, 0X2fb251250f8bb59bUL), CtorFromBits),
            // 6.054601895401185884531860533810599E-39
            new (UInt128.FromLoHi(0Xee418b38228615a7UL, 0X2fb12a83d1d89a48UL), CtorFromBits),
            // 2.227363561795743739222909281640826E-39
            new (UInt128.FromLoHi(0Xd2c9c01c7cac217aUL, 0X2fb06dd147d36cabUL), CtorFromBits),
            // 8.194012623990515430361108213381943E-40
            new (UInt128.FromLoHi(0X47a85ebcb01fbb37UL, 0X2faf93fefabd9468UL), CtorFromBits),
            // 3.014408785065374553263082455636317E-40
            new (UInt128.FromLoHi(0X744579359cd3195dUL, 0X2fae949f301950cbUL), CtorFromBits),
            // 1.108939019312136379459597534352117E-40
            new (UInt128.FromLoHi(0X26a643fd80c5d2f5UL, 0X2fae36acc7394750UL), CtorFromBits),
            // 4.079558667177560157700966530454418E-41
            new (UInt128.FromLoHi(0Xfb935e548d289392UL, 0X2facc92344fd430eUL), CtorFromBits),
            // 1.500785762707394887544965003009105E-41
            new (UInt128.FromLoHi(0X9d2e8a0a8ef8dc51UL, 0X2fac49fe945681d5UL), CtorFromBits),
            // 5.521082277028532731723642066679472E-42
            new (UInt128.FromLoHi(0X8a7e13528973b6b0UL, 0X2fab1035dafbde3dUL), CtorFromBits),
            // 2.031092662734810925690368777712955E-42
            new (UInt128.FromLoHi(0X7f7a9213ca26f53bUL, 0X2faa6423fe58e9b6UL), CtorFromBits),
            // 7.471972337342990160585555576845257E-43
            new (UInt128.FromLoHi(0X1df9182293af73c9UL, 0X2fa970658cbefb8fUL), CtorFromBits),
            // 2.748785007910214929956328761371857E-43
            new (UInt128.FromLoHi(0Xf9d301afb1caacd1UL, 0X2fa887868b629ea7UL), CtorFromBits),
            // 1.011221492610448529945285797620259E-43
            new (UInt128.FromLoHi(0X2362abafda9c2a23UL, 0X2fa831db68ddaf2eUL), CtorFromBits),
            // 3.720075976020835962959695803863118E-44
            new (UInt128.FromLoHi(0X2eb6adedf114504eUL, 0X2fa6b769f5c496b6UL), CtorFromBits),
            // 1.368539471173853000247055730232294E-44
            new (UInt128.FromLoHi(0Xbd5c793c065e4be6UL, 0X2fa643796586118eUL), CtorFromBits),
            // 5.034575358764982396802154622970969E-45
            new (UInt128.FromLoHi(0X173226884d816c59UL, 0X2fa4f83946593525UL), CtorFromBits),
            // 1.852116769517975462264702404739742E-45
            new (UInt128.FromLoHi(0X2a26ee321305429eUL, 0X2fa45b51000bedfbUL), CtorFromBits),
            // 6.813556821545298513418186405213307E-46
            new (UInt128.FromLoHi(0X0dfd14050ff0747bUL, 0X2fa34fef2d794f8aUL), CtorFromBits),
            // 2.506567475899953173103157244337931E-46
            new (UInt128.FromLoHi(0Xd34f97f487301f0bUL, 0X2fa27b9554541ed3UL), CtorFromBits),
            // 9.221146422925874925514857715822952E-47
            new (UInt128.FromLoHi(0X954d4531e2077168UL, 0X2fa1c6a33b0a8e96UL), CtorFromBits),
            // 3.392270193026015202552993984869140E-47
            new (UInt128.FromLoHi(0X5b9616b272c4f314UL, 0X2fa0a7407828366fUL), CtorFromBits),
            // 1.247946462912951248544372794494191E-47
            new (UInt128.FromLoHi(0X22f7660dd7d28cefUL, 0X2fa03d874c7fa02fUL), CtorFromBits),
            // 4.590938473882945758034744928980065E-48
            new (UInt128.FromLoHi(0Xcd7075bb87494c61UL, 0X2f9ee259ca43050eUL), CtorFromBits),
            // 1.688911880224532335163846314671427E-48
            new (UInt128.FromLoHi(0Xccf40265c0f3bd43UL, 0X2f9e5345109ba1d8UL), CtorFromBits),
            // 6.213159586848108836624554207130696E-49
            new (UInt128.FromLoHi(0Xf02bd0bf6612b448UL, 0X2f9d32551960f050UL), CtorFromBits),
            // 2.285693676718671734737326403779573E-49
            new (UInt128.FromLoHi(0X35066dc92f607bf5UL, 0X2f9c70b182979de2UL), CtorFromBits),
            // 8.408597124803643024305255812911477E-50
            new (UInt128.FromLoHi(0Xcd29a9301ef35175UL, 0X2f9b9e936aa8681dUL), CtorFromBits),
            // 3.093350011308560842982749612690113E-50
            new (UInt128.FromLoHi(0X11983c188cb24ac1UL, 0X2f9a988390f531f4UL), CtorFromBits),
            // 1.137979873507868148877262079413556E-50
            new (UInt128.FromLoHi(0X824f77c3b8504d34UL, 0X2f9a381b5348d75cUL), CtorFromBits),
            // 4.186393999304231551538413432299159E-51
            new (UInt128.FromLoHi(0X5bfb858224d95e97UL, 0X2f98ce67b890bb98UL), CtorFromBits),
            // 1.540088284987520198468801203665255E-51
            new (UInt128.FromLoHi(0X3443d2d76cbdf967UL, 0X2f984beea5a2f633UL), CtorFromBits),
            // 5.665668176358939336292407434616395E-52
            new (UInt128.FromLoHi(0X67b236d44d591e4bUL, 0X2f971756c938a48dUL), CtorFromBits),
            // 2.084282842581751323933015821372177E-52
            new (UInt128.FromLoHi(0Xdf2a4212fc89cb11UL, 0X2f9666c3591675e5UL), CtorFromBits),
            // 7.667648073721999632434208375275828E-53
            new (UInt128.FromLoHi(0X4360a572eedf8134UL, 0X2f957a0b53267660UL), CtorFromBits),
            // 2.820770088460135401118447461253846E-53
            new (UInt128.FromLoHi(0X5dba98adc304c2d6UL, 0X2f948b131fb9a4caUL), CtorFromBits),
            // 1.037703323815834534800581793814404E-53
            new (UInt128.FromLoHi(0Xd5b3e17b3da61784UL, 0X2f943329a8470ad3UL), CtorFromBits),
            // 3.817497188671174625733113216892722E-54
            new (UInt128.FromLoHi(0Xa8208326b0d5ff32UL, 0X2f92bc3796aef1c9UL), CtorFromBits),
            // 1.404378732441903834838253307430433E-54
            new (UInt128.FromLoHi(0X8e1ee0a5bca80621UL, 0X2f92453dc06c2861UL), CtorFromBits),
            // 5.166420632837860980252718607357558E-55
            new (UInt128.FromLoHi(0X2d1ad5c837c3d676UL, 0X2f90feb96567c705UL), CtorFromBits),
            // 1.900619935265001688683820156642422E-55
            new (UInt128.FromLoHi(0Xae7e9c8f3902e476UL, 0X2f905db5323a4872UL), CtorFromBits),
            // 6.991989996645917022696257740400656E-56
            new (UInt128.FromLoHi(0Xe7a3bf0d3aace810UL, 0X2f8f58bb522863e4UL), CtorFromBits),
        ];

        static ReadOnlySpan<Decimal128Bid> ExpMTable7 => ExpMTable7_;
        static readonly Decimal128Bid[] ExpMTable7_ = [
            // 1
            new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits),
            // 6.991989996645917022696257740400656E-56
            new (UInt128.FromLoHi(0Xe7a3bf0d3aace810UL, 0X2f8f58bb522863e4UL), CtorFromBits),
            // 4.888792411319657073829388693524670E-111
            new (UInt128.FromLoHi(0Xffb926b65313d0beUL, 0X2f20f1093c3d70a2UL), CtorFromBits),
            // 3.418238763562551365726537070070241E-166
            new (UInt128.FromLoHi(0X22e095e2157ac9e1UL, 0X2eb2a8883d224bd2UL), CtorFromBits),
            // 2.390029124097666707442134495219439E-221
            new (UInt128.FromLoHi(0Xe046712c1e834eefUL, 0X2e4475d6689ae403UL), CtorFromBits),
            // 1.671105972738328864137217273371639E-276
            new (UInt128.FromLoHi(0Xea42b614f6261bf7UL, 0X2dd65264529e2bb3UL), CtorFromBits),
            // 1.168435624472163993832654138042188E-331
            new (UInt128.FromLoHi(0Xdc926e393fc0e74cUL, 0X2d68399bbb1f9464UL), CtorFromBits),
            // 8.169690198034095885120626267809616E-387
            new (UInt128.FromLoHi(0Xe29d815fe56c6750UL, 0X2cf992cbfcbe60d8UL), CtorFromBits),
            // 5.712239214035059926473316762341583E-442
            new (UInt128.FromLoHi(0X1dbbde3cdd6b50cfUL, 0X2c8b19a2985c1d6dUL), CtorFromBits),
            // 3.993991944298167434522026364232271E-497
            new (UInt128.FromLoHi(0Xd8ea35ca7501e24fUL, 0X2c1cc4eb43ff0433UL), CtorFromBits),
            // 2.792595172121716332864490337603960E-552
            new (UInt128.FromLoHi(0Xcd3f337d523a1178UL, 0X2bae89af81a91698UL), CtorFromBits),
            // 1.952579750815672345291112490381339E-607
            new (UInt128.FromLoHi(0Xd7bf0d0d83f6b01bUL, 0X2b4060450572569dUL), CtorFromBits),
            // 1.365241808535655837751317615791015E-662
            new (UInt128.FromLoHi(0Xdb8601f1d9d6cba7UL, 0X2ad2434fc63377afUL), CtorFromBits),
            // 9.545757068284085951086017081678138E-718
            new (UInt128.FromLoHi(0X48c0b72020b2993aUL, 0X2a63d6a4649b068fUL), CtorFromBits),
            // 6.674383793185438484092539146487324E-773
            new (UInt128.FromLoHi(0Xb4da2f2711f6461cUL, 0X29f54912912a760cUL), CtorFromBits),
            // 4.666722471572821696167493254829705E-828
            new (UInt128.FromLoHi(0Xccb09176d3d4d689UL, 0X2986e6165190a68cUL), CtorFromBits),
            // 3.262967683835987916968290266742653E-883
            new (UInt128.FromLoHi(0Xa22c9fc121117f7dUL, 0X2918a0e0713774c8UL), CtorFromBits),
            // 2.281463740476012479171660955259295E-938
            new (UInt128.FromLoHi(0Xe3ea31ad43dc119fUL, 0X28aa707c1eee5d4dUL), CtorFromBits),
            // 1.595197165111865579897714539760804E-993
            new (UInt128.FromLoHi(0X210afa3e4e780ca4UL, 0X283c4ea638082e1aUL), CtorFromBits),
            // 1.115360262114008935904412565117744E-1048
            new (UInt128.FromLoHi(0X81f463f2eb0a1b30UL, 0X27ce36fdd360d530UL), CtorFromBits),
            // 7.798587795357518471036656159752917E-1104
            new (UInt128.FromLoHi(0X198e08e0730f02d5UL, 0X275f808004359cc6UL), CtorFromBits),
            // 5.452764785310470500288621649111792E-1159
            new (UInt128.FromLoHi(0X5d553084c2e8aaf0UL, 0X26f10cd79143387fUL), CtorFromBits),
            // 3.812567683295393108766189582254048E-1214
            new (UInt128.FromLoHi(0X97919c18d3837fe0UL, 0X2682bbf95e97ab0cUL), CtorFromBits),
            // 2.665743510313688729620266668558912E-1269
            new (UInt128.FromLoHi(0Xcd01c943b4148a40UL, 0X2614836e69d5c541UL), CtorFromBits),
            // 1.863885195773708353081654208846617E-1324
            new (UInt128.FromLoHi(0X379a348064835b19UL, 0X25a65be589e1f7f7UL), CtorFromBits),
            // 1.303226664374618546087034717948150E-1379
            new (UInt128.FromLoHi(0Xf04cb57345f748f6UL, 0X2538404108768867UL), CtorFromBits),
            // 9.112147800669558757507840449440009E-1435
            new (UInt128.FromLoHi(0X574ae2720ca7e909UL, 0X24c9c143797f6e7bUL), CtorFromBits),
            // 6.371204627024064831200574475064317E-1490
            new (UInt128.FromLoHi(0X7442033c6dafd3fdUL, 0X245b3a1fe872151bUL), CtorFromBits),
            // 4.454739901873644207468585061444864E-1545
            new (UInt128.FromLoHi(0X650b110856d3ed00UL, 0X23ecdba2b8f0cbd7UL), CtorFromBits),
            // 3.114749683155993428898813885882437E-1600
            new (UInt128.FromLoHi(0X3bce9a7acf052045UL, 0X237e9991ab06dcc1UL), CtorFromBits),
            // 2.177829862668274560409047539970963E-1655
            new (UInt128.FromLoHi(0X8d6c7812f810f393UL, 0X23106b6013cfd695UL), CtorFromBits),
            // 1.522736461417332697379504984446164E-1710
            new (UInt128.FromLoHi(0X14a4c8fa298c54d4UL, 0X22a24b13a2dee97aUL), CtorFromBits),
            // 1.064695810575799160259091274960643E-1765
            new (UInt128.FromLoHi(0Xd3ee47c51bf48f03UL, 0X2234347e59b4c654UL), CtorFromBits),
            // 7.444342457016803876281065960725906E-1821
            new (UInt128.FromLoHi(0Xd56e63ffeda95d92UL, 0X21c56f08cfc9a650UL), CtorFromBits),
            // 5.205076799106798022261887290322837E-1876
            new (UInt128.FromLoHi(0X4c0b868922f02f95UL, 0X215700a14e37121aUL), CtorFromBits),
            // 3.639384491112848121623213147526969E-1931
            new (UInt128.FromLoHi(0X18cea54a6c599739UL, 0X20e8b36f7d84acbaUL), CtorFromBits),
            // 2.544653995580932536840344355577282E-1986
            new (UInt128.FromLoHi(0X5013fa9d3ff99dc2UL, 0X207a7d760c82ccd6UL), CtorFromBits),
            // 1.779219528202694383855636132863492E-2041
            new (UInt128.FromLoHi(0Xc61bd4e590131e04UL, 0X200c57b8e85b347bUL), CtorFromBits),
            // 1.244028514303030717254371648637695E-2096
            new (UInt128.FromLoHi(0X0437e362c2ce8affUL, 0X1f9e3d55d8ec7e9fUL), CtorFromBits),
            // 8.698234927549072881631133643366872E-2152
            new (UInt128.FromLoHi(0Xf43a0bde5ffce1d8UL, 0X1f2facdb28df800bUL), CtorFromBits),
            // 6.081797160189924039455487490725372E-2207
            new (UInt128.FromLoHi(0X5a6ba3913d6315fcUL, 0X1ec12bdb127c63a0UL), CtorFromBits),
            // 4.252386490567749465822510886075521E-2262
            new (UInt128.FromLoHi(0X32f29566d7b2bc81UL, 0X1e52d1a8a9ce147eUL), CtorFromBits),
            // 2.973264380392194144660664886048028E-2317
            new (UInt128.FromLoHi(0X3509e430f249351cUL, 0X1de49297df5b7475UL), CtorFromBits),
            // 2.078903480508584209222890527102697E-2372
            new (UInt128.FromLoHi(0X2d14b56f9e5f96e9UL, 0X1d76667f736e721aUL), CtorFromBits),
            // 1.453567233970840092961301389741266E-2427
            new (UInt128.FromLoHi(0X847d16c50e918cd2UL, 0X1d0847aa990afba6UL), CtorFromBits),
            // 1.016332755937638910575640903774057E-2482
            new (UInt128.FromLoHi(0X67344a2c47096f69UL, 0X1c9a321bec3f9d3cUL), CtorFromBits),
            // 7.106188462779547490389141891451257E-2538
            new (UInt128.FromLoHi(0Xa0b0632615b20979UL, 0X1c2b5e5cb54d8f2aUL), CtorFromBits),
            // 4.968639864603522250080484813150753E-2593
            new (UInt128.FromLoHi(0X124616376b539a21UL, 0X1bbcf4f90d46c215UL), CtorFromBits),
            // 3.474068023024395114712080427585619E-2648
            new (UInt128.FromLoHi(0X87f4ca2742a11053UL, 0X1b4eab48e7335611UL), CtorFromBits),
            // 2.429064886465402798009433024758136E-2703
            new (UInt128.FromLoHi(0X3e955926a0820178UL, 0X1ae077c31bf4b7aeUL), CtorFromBits),
            // 1.698399738736994652319412073319861E-2758
            new (UInt128.FromLoHi(0X231b2f30cd00cdb5UL, 0X1a7253bcd1883f95UL), CtorFromBits),
            // 1.187519398355510558671592900470025E-2813
            new (UInt128.FromLoHi(0X2f7a7a3728072509UL, 0X1a043a8c9a1f59aeUL), CtorFromBits),
            // 8.303123754124707671883991259798703E-2869
            new (UInt128.FromLoHi(0X455162a6adfbdcafUL, 0X1995996027d10a5cUL), CtorFromBits),
            // 5.805535822975304875258012294042171E-2924
            new (UInt128.FromLoHi(0X52543f9115b9d23bUL, 0X19271e3c29f63f4cUL), CtorFromBits),
            // 4.059224839941285305678672723727645E-2979
            new (UInt128.FromLoHi(0X46e738b13290551dUL, 0X18b8c8229ed997d5UL), CtorFromBits),
            // 2.838205947500609050775665004919383E-3034
            new (UInt128.FromLoHi(0Xfe6ab2bca43e3a57UL, 0X184a8bef32063487UL), CtorFromBits),
            // 1.984470759334520522233891035570536E-3089
            new (UInt128.FromLoHi(0X49f409d3bbb86168UL, 0X17dc61d78ada4cc6UL), CtorFromBits),
            // 1.387539969790329455341319701716789E-3144
            new (UInt128.FromLoHi(0X51c373df4ea4df35UL, 0X176e446937722729UL), CtorFromBits),
            // 9.701665588720361455449661130116250E-3200
            new (UInt128.FromLoHi(0Xd003f2b6a2bf8c9aUL, 0X16ffde543c34765bUL), CtorFromBits),
            // 6.783394874713668869026902272635574E-3255
            new (UInt128.FromLoHi(0Xcc39f2f950c7f2b6UL, 0X16914e727af7a79bUL), CtorFromBits),
            // 4.742942910729715631794084223142709E-3310
            new (UInt128.FromLoHi(0Xfaf60b88413c4b35UL, 0X1622e9d85b163391UL), CtorFromBits),
            // 3.316260938648484032114591690186452E-3365
            new (UInt128.FromLoHi(0X39e23b6195dcded4UL, 0X15b4a38119029defUL), CtorFromBits),
            // 2.318726330929779950508669368196876E-3420
            new (UInt128.FromLoHi(0X817e886f838adf0cUL, 0X1546725270dac2e6UL), CtorFromBits),
            // 1.621251131082051160055957244623274E-3475
            new (UInt128.FromLoHi(0X0f3dc6cf4eec39aaUL, 0X14d84fef10efb5f3UL), CtorFromBits),
            // 1.133577169057658006990290662671384E-3530
            new (UInt128.FromLoHi(0X098d6858217b4418UL, 0X146a37e3c16113c6UL), CtorFromBits),
            // 7.925960226477342322099585266998009E-3586
            new (UInt128.FromLoHi(0Xc3b0246c93dbdaf9UL, 0X13fb86c7aebb355dUL), CtorFromBits),
            // 5.541823461734298446828291156355678E-3641
            new (UInt128.FromLoHi(0X419d395fd0202a5eUL, 0X138d113ba55dbcf5UL), CtorFromBits),
            // 3.874837420762386166101343618421519E-3696
            new (UInt128.FromLoHi(0X62e4a600f81f530fUL, 0X131ebf0b52f73fe7UL), CtorFromBits),
            // 2.709282448459987021672027589118380E-3751
            new (UInt128.FromLoHi(0X3b3579cd396f09acUL, 0X12b08593f3bb4bdaUL), CtorFromBits),
            // 1.894327577772058651457315427502067E-3806
            new (UInt128.FromLoHi(0X77abc2db108ac7f3UL, 0X12425d65c6862857UL), CtorFromBits),
            // 1.324511947415272448836074204794778E-3861
            new (UInt128.FromLoHi(0X3adbecce77fb8f9aUL, 0X11d4414db0ebff2cUL), CtorFromBits),
            // 9.260974286765587833476347756950616E-3917
            new (UInt128.FromLoHi(0X435eff05292a3458UL, 0X1165c899edcf5d56UL), CtorFromBits),
            // 6.475263957226004626730016495550621E-3972
            new (UInt128.FromLoHi(0Xef0641e43f1ac09dUL, 0X10f73f41524694abUL), CtorFromBits),
            // 4.527498081456607947756979847061658E-4027
            new (UInt128.FromLoHi(0X61f9576f194a609aUL, 0X1088df390f4c5a18UL), CtorFromBits),
            // 3.165622129537818395984836494152872E-4082
            new (UInt128.FromLoHi(0X9502610c47d4d4a8UL, 0X101a9c13c4c438f6UL), CtorFromBits),
            // 2.213399826288937154928319814039825E-4137
            new (UInt128.FromLoHi(0X308c6048df2df911UL, 0X0fac6d210890226bUL), CtorFromBits),
            // 1.547606944399005901856447859716663E-4192
            new (UInt128.FromLoHi(0Xc5fa65911d66f237UL, 0X0f3e4c4d8bbc4260UL), CtorFromBits),
            // 1.082085227397760316795550004925768E-4247
            new (UInt128.FromLoHi(0X241638144a77d548UL, 0X0ed03559d5f13bc4UL), CtorFromBits),
            // 7.565929085483462516224640780401970E-4303
            new (UInt128.FromLoHi(0Xcadc8f501d5cfd32UL, 0X0e617507735f7685UL), CtorFromBits),
            // 5.290090048103276112571017425308297E-4358
            new (UInt128.FromLoHi(0Xee499f91bbd3ca89UL, 0X0df304d252d67363UL), CtorFromBits),
            // 3.698825669769422456758829467578128E-4413
            new (UInt128.FromLoHi(0Xf1067659d4e82f10UL, 0X0d84b65dbe533f29UL), CtorFromBits),
            // 2.586215208236493590844543500487546E-4468
            new (UInt128.FromLoHi(0Xe12ca8e21be70b7aUL, 0X0d167f82a008249cUL), CtorFromBits),
            // 1.808279086516310041665906351559269E-4523
            new (UInt128.FromLoHi(0X56b867e626ec5265UL, 0X0ca85927b0da738eUL), CtorFromBits),
            // 1.264346928406605654592888403170842E-4578
            new (UInt128.FromLoHi(0Xd1c9d77a62d8961aUL, 0X0c3a3e564d42b80aUL), CtorFromBits),
            // 8.840301075708978160882060432712831E-4634
            new (UInt128.FromLoHi(0Xcbee751fd05b247fUL, 0X0bcbb3dc495af548UL), CtorFromBits),
            // 6.181129668869531485883031284517014E-4689
            new (UInt128.FromLoHi(0Xa257bef817a68896UL, 0X0b5d30c0d321c3e3UL), CtorFromBits),
            // 4.321839681270705365111688322776535E-4744
            new (UInt128.FromLoHi(0Xf0b4b9360df67dd7UL, 0X0aeed515492ae8d3UL), CtorFromBits),
            // 3.021825981855215030022136969165135E-4799
            new (UInt128.FromLoHi(0X54f79dec73cdf94fUL, 0X0a8094fcce5ab31cUL), CtorFromBits),
            // 2.112857703673638985164973760431643E-4854
            new (UInt128.FromLoHi(0Xad72043e26fdc61bUL, 0X0a12682c03718fe8UL), CtorFromBits),
            // 1.477307992842234699052681782628933E-4909
            new (UInt128.FromLoHi(0X8b74a0186c981245UL, 0X09a448d63f9272dfUL), CtorFromBits),
            // 1.032932270791796300240246237872229E-4964
            new (UInt128.FromLoHi(0X8b98c8979198c065UL, 0X093632ed702c7ab6UL), CtorFromBits),
            // 7.222252104588991267146335828909470E-5020
            new (UInt128.FromLoHi(0X8bc4c704edbd1d9eUL, 0X08c76415a327e2e5UL), CtorFromBits),
            // 5.049791446854114820817644628799556E-5075
            new (UInt128.FromLoHi(0X7fecc2994fc0f044UL, 0X0858f8f9542e4619UL), CtorFromBits),
            // 3.530809128155208275518134128063943E-5130
            new (UInt128.FromLoHi(0X5527d66e7601b5c7UL, 0X07eaae1513996f6cUL), CtorFromBits),
            // 2.468738210412730791748598720847706E-5185
            new (UInt128.FromLoHi(0Xf454f2b958d35f5aUL, 0X077c79b7db611f4cUL), CtorFromBits),
            // 1.726139287154335676163364015510224E-5240
            new (UInt128.FromLoHi(0X892d45f4316312d0UL, 0X070e551af0d8e671UL), CtorFromBits),
            // 1.206914862860062910487790605218055E-5295
            new (UInt128.FromLoHi(0X3e2b672ead12b107UL, 0X06a03b81683f8f58UL), CtorFromBits),
            // 8.438736647920838672927709444973265E-5351
            new (UInt128.FromLoHi(0Xd4e7ca24205422d1UL, 0X0631a00fd4b599d0UL), CtorFromBits),
            // 5.900356222659180185198328678812524E-5406
            new (UInt128.FromLoHi(0X64dffd1318158b6cUL, 0X05c322e8f7328a50UL), CtorFromBits),
            // 4.125523168548047689665549667550941E-5461
            new (UInt128.FromLoHi(0Xc255c59eff9e46ddUL, 0X0554cb676c4da949UL), CtorFromBits),
            // 2.884561672541891693356542232828387E-5516
            new (UInt128.FromLoHi(0Xf5faa2c11ba719e3UL, 0X04e68e3849733d7cUL), CtorFromBits),
            // 2.016882635912112209817643919807734E-5571
            new (UInt128.FromLoHi(0X22d62389163314f6UL, 0X04786370a34638aaUL), CtorFromBits),
            // 1.410202321470633773359055119061464E-5626
            new (UInt128.FromLoHi(0X247edbb0a53811d8UL, 0X040a458741741908UL), CtorFromBits),
            // 9.860120524969521035989907417685950E-5682
            new (UInt128.FromLoHi(0X5bd97773f1455fbeUL, 0X039be62437b7b0e3UL), CtorFromBits),
            // 6.894186407630997898146756352655272E-5737
            new (UInt128.FromLoHi(0X2b13fea72dc5bfa8UL, 0X032d53e8ddb7b80eUL), CtorFromBits),
            // 4.820408239716818772166917024495239E-5792
            new (UInt128.FromLoHi(0X0ebe4a97e095b687UL, 0X02beedaa1b107c10UL), CtorFromBits),
            // 3.370424619184955046631448333085849E-5847
            new (UInt128.FromLoHi(0Xdc85f69aa14d6c99UL, 0X0250a62cbd4d0fabUL), CtorFromBits),
            // 2.356597522179032999500291470439564E-5902
            new (UInt128.FromLoHi(0X3700c2701a5eb48cUL, 0X01e274307145efdfUL), CtorFromBits),
            // 1.647730630119635330867917017210068E-5957
            new (UInt128.FromLoHi(0Xe37d15c24cce5cd4UL, 0X0174513d48cff1daUL), CtorFromBits),
            // 1.152091608296356377940892934364719E-6012
            new (UInt128.FromLoHi(0X6b609e0065825e2fUL, 0X010638cd70c2448dUL), CtorFromBits),
            // 8.055413000427829979317644181402465E-6068
            new (UInt128.FromLoHi(0X723490eadb687361UL, 0X00978d299b340fbfUL), CtorFromBits),
            // 5.632336711784285931720292734248877E-6123
            new (UInt128.FromLoHi(0X4333501141c6a7adUL, 0X002915b215735bcfUL), CtorFromBits),
            // 0E-6176
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X0000000000000000UL), CtorFromBits),
            // 0E-6176
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X0000000000000000UL), CtorFromBits),
            // 0E-6176
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X0000000000000000UL), CtorFromBits),
            // 0E-6176
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X0000000000000000UL), CtorFromBits),
            // 0E-6176
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X0000000000000000UL), CtorFromBits),
            // 0E-6176
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X0000000000000000UL), CtorFromBits),
            // 0E-6176
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X0000000000000000UL), CtorFromBits),
            // 0E-6176
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X0000000000000000UL), CtorFromBits),
            // 0E-6176
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X0000000000000000UL), CtorFromBits),
            // 0E-6176
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X0000000000000000UL), CtorFromBits),
            // 0E-6176
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X0000000000000000UL), CtorFromBits),
            // 0E-6176
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X0000000000000000UL), CtorFromBits),
            // 0E-6176
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X0000000000000000UL), CtorFromBits),
            // 0E-6176
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X0000000000000000UL), CtorFromBits),
            // 0E-6176
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X0000000000000000UL), CtorFromBits),
            // 0E-6176
            new (UInt128.FromLoHi(0X0000000000000000UL, 0X0000000000000000UL), CtorFromBits),
        ];

        static Decimal128Bid ExpInternal1(int exponent) {
            Debug.Assert(0 <= exponent && exponent < 128);
            return ExpTable0[exponent];
        }

        static Decimal128Bid ExpInternal2(int exponent) {
            Debug.Assert(0 <= exponent && exponent < 128);
            return ExpTable7[exponent];
        }

        static Decimal128Bid ExpMInternal1(int exponent) {
            Debug.Assert(0 <= exponent && exponent < 128);
            return ExpMTable0[exponent];
        }

        static Decimal128Bid ExpMInternal2(int exponent) {
            Debug.Assert(0 <= exponent && exponent < 128);
            return ExpMTable7[exponent];
        }

        // Numerator coefficients:
        static readonly Decimal128Bid ExpInternal_50A1_RI8_P8 = new(UInt128.FromLoHi(0X8c896210c4cb898aUL, 0X2fec4bff18282f1dUL), CtorFromBits); // 1.541391377934777491755286118762890E-9
        static readonly Decimal128Bid ExpInternal_50A1_RI8_P7 = new(UInt128.FromLoHi(0X8a7f1e50a1db981aUL, 0X2ff039741cb4ec9dUL), CtorFromBits); // 1.165296698564107014126459770607642E-7
        static readonly Decimal128Bid ExpInternal_50A1_RI8_P6 = new(UInt128.FromLoHi(0Xc7be32d7037b5a47UL, 0X2ff2d12dec56af7dUL), CtorFromBits); // 4.242662017735013708599772856408647E-6
        static readonly Decimal128Bid ExpInternal_50A1_RI8_P5 = new(UInt128.FromLoHi(0X3559a5bd5c4e424fUL, 0X2ff5db66e83a0417UL), CtorFromBits); // 9.642297704970292088845085679960655E-5
        static readonly Decimal128Bid ExpInternal_50A1_RI8_P4 = new(UInt128.FromLoHi(0X63b0811c0b401e7eUL, 0X2ff849474d6525ddUL), CtorFromBits); // 0.001486265053231160207254959205391998
        static readonly Decimal128Bid ExpInternal_50A1_RI8_P3 = new(UInt128.FromLoHi(0X9f9f15476d5589bcUL, 0X2ffa4dfdf4a7af5aUL), CtorFromBits); // 0.01581865981658381627299137611729340
        static readonly Decimal128Bid ExpInternal_50A1_RI8_P2 = new(UInt128.FromLoHi(0X2be474053afe6afeUL, 0X2ffc37b092e463f9UL), CtorFromBits); // 0.1129522145721983869107900895881982
        static readonly Decimal128Bid ExpInternal_50A1_RI8_P1 = new(UInt128.FromLoHi(0X4fbd308ec19c6101UL, 0X2ffcf2d27bc1cfa7UL), CtorFromBits); // 0.4925019339171207178573178247864577
        static readonly Decimal128Bid ExpInternal_50A1_RI8_P0 = new(UInt128.FromLoHi(0X378d8e63625d2789UL, 0X2ffded09bead87c0UL), CtorFromBits); // 0.9999999999999999999999997355304841

        // Denominator coefficients:
        static readonly Decimal128Bid ExpInternal_50A1_RI8_Q8 = new(UInt128.FromLoHi(0X902f641d6d44be10UL, 0X2fec772fa95bf862UL), CtorFromBits); // 2.417382880624593654868327321091600E-9
        static readonly Decimal128Bid ExpInternal_50A1_RI8_Q7 = new(UInt128.FromLoHi(0X84d5b23314cb7b60UL, 0Xaff05186126521ecUL), CtorFromBits); // -1.653497444664582388154655188941664E-7
        static readonly Decimal128Bid ExpInternal_50A1_RI8_Q6 = new(UInt128.FromLoHi(0X70536f84dbd78034UL, 0X2ff311fed8ea9b21UL), CtorFromBits); // 5.557288907458876009179230351360052E-6
        static readonly Decimal128Bid ExpInternal_50A1_RI8_Q5 = new(UInt128.FromLoHi(0X7db29f464679ad87UL, 0Xaff63a53a53d6740UL), CtorFromBits); // -1.183006833759169739871139611127175E-4
        static readonly Decimal128Bid ExpInternal_50A1_RI8_Q4 = new(UInt128.FromLoHi(0Xc61ea75dede2518bUL, 0X2ff8552250a8ae64UL), CtorFromBits); // 0.001726723536559751094835476084969867
        static readonly Decimal128Bid ExpInternal_50A1_RI8_Q3 = new(UInt128.FromLoHi(0X8fdc14170630f61eUL, 0Xaffa568645f1b707UL), CtorFromBits); // -0.01754925446372087837388564075705886
        static readonly Decimal128Bid ExpInternal_50A1_RI8_Q2 = new(UInt128.FromLoHi(0Xf7ee0c3f3d02f6cbUL, 0X2ffc3b62f6795831UL), CtorFromBits); // 0.1204502806550776690536469686843083
        static readonly Decimal128Bid ExpInternal_50A1_RI8_Q1 = new(UInt128.FromLoHi(0Xe7d05dbd866442b5UL, 0Xaffcfa3742ebb818UL), CtorFromBits); // -0.5074980660828792821426719880921781
                                                                                                                                                 // static readonly Decimal128Bid ExpInternal_50A1_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Numerator coefficients:
        static readonly Decimal128Bid ExpInternal_40A1_RI8_P8 = new(UInt128.FromLoHi(0X38c4107ac1da6c92UL, 0X2fec50d4dca431fcUL), CtorFromBits); // 1.639457423947201413779583930100882E-9
        static readonly Decimal128Bid ExpInternal_40A1_RI8_P7 = new(UInt128.FromLoHi(0X45d83b9db4cab355UL, 0X2ff03c4d053cd216UL), CtorFromBits); // 1.223046765685404685645011288240981E-7
        static readonly Decimal128Bid ExpInternal_40A1_RI8_P6 = new(UInt128.FromLoHi(0Xd27eb84841cfd2fdUL, 0X2ff2d92d8cbbb7bcUL), CtorFromBits); // 4.404891706143733855447758127747837E-6
        static readonly Decimal128Bid ExpInternal_40A1_RI8_P5 = new(UInt128.FromLoHi(0X637d396cbc0b4662UL, 0X2ff5e92a83909daeUL), CtorFromBits); // 9.921466596377499000428419607119458E-5
        static readonly Decimal128Bid ExpInternal_40A1_RI8_P4 = new(UInt128.FromLoHi(0X987d78b89fc47db2UL, 0X2ff84ad53b8f12b6UL), CtorFromBits); // 0.001517792341866104471715017157541298
        static readonly Decimal128Bid ExpInternal_40A1_RI8_P3 = new(UInt128.FromLoHi(0Xbc7e03775061f603UL, 0X2ffa4f22d7744efaUL), CtorFromBits); // 0.01605070796099443469436370334905859
        static readonly Decimal128Bid ExpInternal_40A1_RI8_P2 = new(UInt128.FromLoHi(0X2f81cac6d573a370UL, 0X2ffc3831e220cf02UL), CtorFromBits); // 0.1139767101043108526619106206786416
        static readonly Decimal128Bid ExpInternal_40A1_RI8_P1 = new(UInt128.FromLoHi(0X3b3e4a5be5f6b280UL, 0X2ffcf3d948764822UL), CtorFromBits); // 0.4945840470867546970644547833868928
        static readonly Decimal128Bid ExpInternal_40A1_RI8_P0 = new(UInt128.FromLoHi(0X378d8e63ff7a931dUL, 0X2ffded09bead87c0UL), CtorFromBits); // 0.9999999999999999999999999991255837

        // Denominator coefficients:
        static readonly Decimal128Bid ExpInternal_40A1_RI8_Q8 = new(UInt128.FromLoHi(0X479f754e4bef0399UL, 0X2fec6fdf8e143035UL), CtorFromBits); // 2.269059317523583817220174314931097E-9
        static readonly Decimal128Bid ExpInternal_40A1_RI8_Q7 = new(UInt128.FromLoHi(0X7f71dde70a827ba5UL, 0Xaff04da40d732a54UL), CtorFromBits); // -1.574743120665013009325344340212645E-7
        static readonly Decimal128Bid ExpInternal_40A1_RI8_Q6 = new(UInt128.FromLoHi(0Xdf078e2f50c2647fUL, 0X2ff307ee4d86ed24UL), CtorFromBits); // 5.353154021900481315972880391103615E-6
        static readonly Decimal128Bid ExpInternal_40A1_RI8_Q5 = new(UInt128.FromLoHi(0Xa9e3ede7537a01f5UL, 0Xaff638b3e48a17e6UL), CtorFromBits); // -1.150067508421418655797530752319989E-4
        static readonly Decimal128Bid ExpInternal_40A1_RI8_Q4 = new(UInt128.FromLoHi(0X39ab488fa81ff823UL, 0X2ff85364de36c414UL), CtorFromBits); // 0.001691431585234646597760996257822755
        static readonly Decimal128Bid ExpInternal_40A1_RI8_Q3 = new(UInt128.FromLoHi(0Xfd9db5823ce951d3UL, 0Xaffa554c7bfaeb5aUL), CtorFromBits); // -0.01730064526660573610196806920655315
        static readonly Decimal128Bid ExpInternal_40A1_RI8_Q2 = new(UInt128.FromLoHi(0X100a479fa4ffa4b6UL, 0X2ffc3add79014ac0UL), CtorFromBits); // 0.1193926630175561555974570008093878
        static readonly Decimal128Bid ExpInternal_40A1_RI8_Q1 = new(UInt128.FromLoHi(0Xfc4f4407fdc13e6dUL, 0Xaffcf93076373f9dUL), CtorFromBits); // -0.5054159529132453029355451691646573
                                                                                                                                                 // static readonly Decimal128Bid ExpInternal_40A1_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Numerator coefficients:
        static readonly Decimal128Bid ExpInternal_25A1_RI8_P8 = new(UInt128.FromLoHi(0Xde608778e29f97c1UL, 0X2fec571292b42b7cUL), CtorFromBits); // 1.766041145066398710958342098884545E-9
        static readonly Decimal128Bid ExpInternal_25A1_RI8_P7 = new(UInt128.FromLoHi(0Xb1ffbcb926cd88c6UL, 0X2ff03fe96ff6ac0dUL), CtorFromBits); // 1.296286617940220290773682381228230E-7
        static readonly Decimal128Bid ExpInternal_25A1_RI8_P6 = new(UInt128.FromLoHi(0X7bc5d1730c654a2dUL, 0X2ff2e32e48a1eb95UL), CtorFromBits); // 4.607773954174863047671852860525101E-6
        static readonly Decimal128Bid ExpInternal_25A1_RI8_P5 = new(UInt128.FromLoHi(0X81877dafa8fc670bUL, 0X2ff6329e564d0c5dUL), CtorFromBits); // 1.026665238716363311099736179369739E-4
        static readonly Decimal128Bid ExpInternal_25A1_RI8_P4 = new(UInt128.FromLoHi(0Xee5e460add572c93UL, 0X2ff84cbcadd2f49cUL), CtorFromBits); // 0.001556411820366482149595326872759443
        static readonly Decimal128Bid ExpInternal_25A1_RI8_P3 = new(UInt128.FromLoHi(0X639d7f5544dfe3b1UL, 0X2ffa5086cf2e31c8UL), CtorFromBits); // 0.01633273461311758260364222887093169
        static readonly Decimal128Bid ExpInternal_25A1_RI8_P2 = new(UInt128.FromLoHi(0X95fecb50f212e2f6UL, 0X2ffc38cdffde8e09UL), CtorFromBits); // 0.1152135898849706999951781447000822
        static readonly Decimal128Bid ExpInternal_25A1_RI8_P1 = new(UInt128.FromLoHi(0X70fb3dfb179749c1UL, 0X2ffcf514c01dc254UL), CtorFromBits); // 0.4970834373243375773044191486495169
        static readonly Decimal128Bid ExpInternal_25A1_RI8_P0 = new(UInt128.FromLoHi(0X378d8e63ffffff80UL, 0X2ffded09bead87c0UL), CtorFromBits); // 0.9999999999999999999999999999999872

        // Denominator coefficients:
        static readonly Decimal128Bid ExpInternal_25A1_RI8_Q8 = new(UInt128.FromLoHi(0X3c28fb4d0981b207UL, 0X2fec67b98f924844UL), CtorFromBits); // 2.103789832442104172556636003611143E-9
        static readonly Decimal128Bid ExpInternal_25A1_RI8_Q7 = new(UInt128.FromLoHi(0X5cfc40f3f9910dccUL, 0Xaff0493b002a6fe1UL), CtorFromBits); // -1.485290413958137857997471271161292E-7
        static readonly Decimal128Bid ExpInternal_25A1_RI8_Q6 = new(UInt128.FromLoHi(0Xd37eda95b09d9cf0UL, 0X2ff2fc54bdd7d699UL), CtorFromBits); // 5.117881139370750621591733714918640E-6
        static readonly Decimal128Bid ExpInternal_25A1_RI8_Q5 = new(UInt128.FromLoHi(0X16a7ee3eb31c163aUL, 0Xaff636cf2c9d610cUL), CtorFromBits); // -1.111664155837721281757402592187962E-4
        static readonly Decimal128Bid ExpInternal_25A1_RI8_Q4 = new(UInt128.FromLoHi(0X37c2cad7e0f554f5UL, 0X2ff85158a80a7a1fUL), CtorFromBits); // 0.001649899262344653326139024553104629
        static readonly Decimal128Bid ExpInternal_25A1_RI8_Q3 = new(UInt128.FromLoHi(0X4aaf7e9357f29c36UL, 0Xaffa53d857651bdcUL), CtorFromBits); // -0.01700580327635099540599300605254710
        static readonly Decimal128Bid ExpInternal_25A1_RI8_Q2 = new(UInt128.FromLoHi(0X40ca5487da860548UL, 0X2ffc3a3e1f178f95UL), CtorFromBits); // 0.1181301525606331226907589961188680
        static readonly Decimal128Bid ExpInternal_25A1_RI8_Q1 = new(UInt128.FromLoHi(0Xc6925068e8688060UL, 0Xaffcf7f4fe8fc56bUL), CtorFromBits); // -0.5029165626756624226955808513491040
                                                                                                                                                 // static readonly Decimal128Bid ExpInternal_25A1_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Numerator coefficients:
        static readonly Decimal128Bid ExpInternal_10A_RI8_P8 = new(UInt128.FromLoHi(0Xecc597c515d0c459UL, 0X2fec5f0238dcede5UL), CtorFromBits); // 1.927004966919609751535064479482969E-9
        static readonly Decimal128Bid ExpInternal_10A_RI8_P7 = new(UInt128.FromLoHi(0X2cf6f34ad7701fc6UL, 0X2ff044684dcff323UL), CtorFromBits); // 1.387467663691381915161060959068102E-7
        static readonly Decimal128Bid ExpInternal_10A_RI8_P6 = new(UInt128.FromLoHi(0X6cd0a536cf9665a9UL, 0X2ff2ef6db3cae65aUL), CtorFromBits); // 4.856187408094389966018742714787241E-6
        static readonly Decimal128Bid ExpInternal_10A_RI8_P5 = new(UInt128.FromLoHi(0X330f333bcdeb2d14UL, 0X2ff634acb31ba1d8UL), CtorFromBits); // 1.068367974564384658151322757573908E-4
        static readonly Decimal128Bid ExpInternal_10A_RI8_P4 = new(UInt128.FromLoHi(0Xf246559c794e418eUL, 0X2ff84f032046b8d4UL), CtorFromBits); // 0.001602558032193990012897632136872334
        static readonly Decimal128Bid ExpInternal_10A_RI8_P3 = new(UInt128.FromLoHi(0X0e291464293ec1daUL, 0X2ffa522c41d0e286UL), CtorFromBits); // 0.01666663995702001182093753884525018
        static readonly Decimal128Bid ExpInternal_10A_RI8_P2 = new(UInt128.FromLoHi(0X299847692a61e281UL, 0X2ffc398565961ac0UL), CtorFromBits); // 0.1166666132473733569752084110238337
        static readonly Decimal128Bid ExpInternal_10A_RI8_P1 = new(UInt128.FromLoHi(0X1bc6c73200000000UL, 0X2ffcf684df56c3e0UL), CtorFromBits); // 0.5000000000000000000000000000000000
                                                                                                                                                // static readonly Decimal128Bid ExpInternal_10A_RI8_P0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Denominator coefficients:
        static readonly Decimal128Bid ExpInternal_10A_RI8_Q8 = new(UInt128.FromLoHi(0Xecc597c515d0c459UL, 0X2fec5f0238dcede5UL), CtorFromBits); // 1.927004966919609751535064479482969E-9
        static readonly Decimal128Bid ExpInternal_10A_RI8_Q7 = new(UInt128.FromLoHi(0X2cf6f34ad7701fc6UL, 0Xaff044684dcff323UL), CtorFromBits); // -1.387467663691381915161060959068102E-7
        static readonly Decimal128Bid ExpInternal_10A_RI8_Q6 = new(UInt128.FromLoHi(0X6cd0a536cf9665a9UL, 0X2ff2ef6db3cae65aUL), CtorFromBits); // 4.856187408094389966018742714787241E-6
        static readonly Decimal128Bid ExpInternal_10A_RI8_Q5 = new(UInt128.FromLoHi(0X330f333bcdeb2d14UL, 0Xaff634acb31ba1d8UL), CtorFromBits); // -1.068367974564384658151322757573908E-4
        static readonly Decimal128Bid ExpInternal_10A_RI8_Q4 = new(UInt128.FromLoHi(0Xf246559c794e418eUL, 0X2ff84f032046b8d4UL), CtorFromBits); // 0.001602558032193990012897632136872334
        static readonly Decimal128Bid ExpInternal_10A_RI8_Q3 = new(UInt128.FromLoHi(0X0e291464293ec1daUL, 0Xaffa522c41d0e286UL), CtorFromBits); // -0.01666663995702001182093753884525018
        static readonly Decimal128Bid ExpInternal_10A_RI8_Q2 = new(UInt128.FromLoHi(0X299847692a61e281UL, 0X2ffc398565961ac0UL), CtorFromBits); // 0.1166666132473733569752084110238337
        static readonly Decimal128Bid ExpInternal_10A_RI8_Q1 = new(UInt128.FromLoHi(0X1bc6c73200000000UL, 0Xaffcf684df56c3e0UL), CtorFromBits); // -0.5000000000000000000000000000000000
                                                                                                                                                // static readonly Decimal128Bid ExpInternal_10A_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Numerator coefficients:
        static readonly Decimal128Bid ExpInternal_25A0_RI8_P8 = new(UInt128.FromLoHi(0X3c28fb4d0981b222UL, 0X2fec67b98f924844UL), CtorFromBits); // 2.103789832442104172556636003611170E-9
        static readonly Decimal128Bid ExpInternal_25A0_RI8_P7 = new(UInt128.FromLoHi(0X5cfc40f3f9910ddfUL, 0X2ff0493b002a6fe1UL), CtorFromBits); // 1.485290413958137857997471271161311E-7
        static readonly Decimal128Bid ExpInternal_25A0_RI8_P6 = new(UInt128.FromLoHi(0Xd37eda95b09d9d32UL, 0X2ff2fc54bdd7d699UL), CtorFromBits); // 5.117881139370750621591733714918706E-6
        static readonly Decimal128Bid ExpInternal_25A0_RI8_P5 = new(UInt128.FromLoHi(0X16a7ee3eb31c1648UL, 0X2ff636cf2c9d610cUL), CtorFromBits); // 1.111664155837721281757402592187976E-4
        static readonly Decimal128Bid ExpInternal_25A0_RI8_P4 = new(UInt128.FromLoHi(0X37c2cad7e0f5550aUL, 0X2ff85158a80a7a1fUL), CtorFromBits); // 0.001649899262344653326139024553104650
        static readonly Decimal128Bid ExpInternal_25A0_RI8_P3 = new(UInt128.FromLoHi(0X4aaf7e9357f29c4bUL, 0X2ffa53d857651bdcUL), CtorFromBits); // 0.01700580327635099540599300605254731
        static readonly Decimal128Bid ExpInternal_25A0_RI8_P2 = new(UInt128.FromLoHi(0X40ca5487da860557UL, 0X2ffc3a3e1f178f95UL), CtorFromBits); // 0.1181301525606331226907589961188695
        static readonly Decimal128Bid ExpInternal_25A0_RI8_P1 = new(UInt128.FromLoHi(0Xc6925068e86880a0UL, 0X2ffcf7f4fe8fc56bUL), CtorFromBits); // 0.5029165626756624226955808513491104
        static readonly Decimal128Bid ExpInternal_25A0_RI8_P0 = new(UInt128.FromLoHi(0X38c15b0a0000000dUL, 0X2ffe314dc6448d93UL), CtorFromBits); // 1.000000000000000000000000000000013

        // Denominator coefficients:
        static readonly Decimal128Bid ExpInternal_25A0_RI8_Q8 = new(UInt128.FromLoHi(0Xde608778e29f97d7UL, 0X2fec571292b42b7cUL), CtorFromBits); // 1.766041145066398710958342098884567E-9
        static readonly Decimal128Bid ExpInternal_25A0_RI8_Q7 = new(UInt128.FromLoHi(0Xb1ffbcb926cd88d6UL, 0Xaff03fe96ff6ac0dUL), CtorFromBits); // -1.296286617940220290773682381228246E-7
        static readonly Decimal128Bid ExpInternal_25A0_RI8_Q6 = new(UInt128.FromLoHi(0X7bc5d1730c654a68UL, 0X2ff2e32e48a1eb95UL), CtorFromBits); // 4.607773954174863047671852860525160E-6
        static readonly Decimal128Bid ExpInternal_25A0_RI8_Q5 = new(UInt128.FromLoHi(0X81877dafa8fc6718UL, 0Xaff6329e564d0c5dUL), CtorFromBits); // -1.026665238716363311099736179369752E-4
        static readonly Decimal128Bid ExpInternal_25A0_RI8_Q4 = new(UInt128.FromLoHi(0Xee5e460add572ca7UL, 0X2ff84cbcadd2f49cUL), CtorFromBits); // 0.001556411820366482149595326872759463
        static readonly Decimal128Bid ExpInternal_25A0_RI8_Q3 = new(UInt128.FromLoHi(0X639d7f5544dfe3c6UL, 0Xaffa5086cf2e31c8UL), CtorFromBits); // -0.01633273461311758260364222887093190
        static readonly Decimal128Bid ExpInternal_25A0_RI8_Q2 = new(UInt128.FromLoHi(0X95fecb50f212e305UL, 0X2ffc38cdffde8e09UL), CtorFromBits); // 0.1152135898849706999951781447000837
        static readonly Decimal128Bid ExpInternal_25A0_RI8_Q1 = new(UInt128.FromLoHi(0X70fb3dfb17974a00UL, 0Xaffcf514c01dc254UL), CtorFromBits); // -0.4970834373243375773044191486495232
                                                                                                                                                 // static readonly Decimal128Bid ExpInternal_25A0_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Numerator coefficients:
        static readonly Decimal128Bid ExpInternal_40A0_RI8_P8 = new(UInt128.FromLoHi(0X479f754e4c0d49ffUL, 0X2fec6fdf8e143035UL), CtorFromBits); // 2.269059317523583817220174316915199E-9
        static readonly Decimal128Bid ExpInternal_40A0_RI8_P7 = new(UInt128.FromLoHi(0X7f71dde70a977e7aUL, 0X2ff04da40d732a54UL), CtorFromBits); // 1.574743120665013009325344341589626E-7
        static readonly Decimal128Bid ExpInternal_40A0_RI8_P6 = new(UInt128.FromLoHi(0Xdf078e2f5109d135UL, 0X2ff307ee4d86ed24UL), CtorFromBits); // 5.353154021900481315972880395784501E-6
        static readonly Decimal128Bid ExpInternal_40A0_RI8_P5 = new(UInt128.FromLoHi(0Xa9e3ede753895a3bUL, 0X2ff638b3e48a17e6UL), CtorFromBits); // 1.150067508421418655797530753325627E-4
        static readonly Decimal128Bid ExpInternal_40A0_RI8_P4 = new(UInt128.FromLoHi(0X39ab488fa836898bUL, 0X2ff85364de36c414UL), CtorFromBits); // 0.001691431585234646597760996259301771
        static readonly Decimal128Bid ExpInternal_40A0_RI8_P3 = new(UInt128.FromLoHi(0Xfd9db5823d00672fUL, 0X2ffa554c7bfaeb5aUL), CtorFromBits); // 0.01730064526660573610196806922168111
        static readonly Decimal128Bid ExpInternal_40A0_RI8_P2 = new(UInt128.FromLoHi(0X100a479fa50f92caUL, 0X2ffc3add79014ac0UL), CtorFromBits); // 0.1193926630175561555974570009137866
        static readonly Decimal128Bid ExpInternal_40A0_RI8_P1 = new(UInt128.FromLoHi(0Xfc4f4407fe04adddUL, 0X2ffcf93076373f9dUL), CtorFromBits); // 0.5054159529132453029355451696066013
        static readonly Decimal128Bid ExpInternal_40A0_RI8_P0 = new(UInt128.FromLoHi(0X38c15b0a000d57b0UL, 0X2ffe314dc6448d93UL), CtorFromBits); // 1.000000000000000000000000000874416

        // Denominator coefficients:
        static readonly Decimal128Bid ExpInternal_40A0_RI8_Q8 = new(UInt128.FromLoHi(0X38c4107ac1f04c72UL, 0X2fec50d4dca431fcUL), CtorFromBits); // 1.639457423947201413779583931534450E-9
        static readonly Decimal128Bid ExpInternal_40A0_RI8_Q7 = new(UInt128.FromLoHi(0X45d83b9db4db04e1UL, 0Xaff03c4d053cd216UL), CtorFromBits); // -1.223046765685404685645011289310433E-7
        static readonly Decimal128Bid ExpInternal_40A0_RI8_Q6 = new(UInt128.FromLoHi(0Xd27eb848420a98baUL, 0X2ff2d92d8cbbb7bcUL), CtorFromBits); // 4.404891706143733855447758131599546E-6
        static readonly Decimal128Bid ExpInternal_40A0_RI8_Q5 = new(UInt128.FromLoHi(0X637d396cbc8fa706UL, 0Xaff5e92a83909daeUL), CtorFromBits); // -9.921466596377499000428419615794950E-5
        static readonly Decimal128Bid ExpInternal_40A0_RI8_Q4 = new(UInt128.FromLoHi(0X987d78b89fd8be01UL, 0X2ff84ad53b8f12b6UL), CtorFromBits); // 0.001517792341866104471715017158868481
        static readonly Decimal128Bid ExpInternal_40A0_RI8_Q3 = new(UInt128.FromLoHi(0Xbc7e03775077606fUL, 0Xaffa4f22d7744efaUL), CtorFromBits); // -0.01605070796099443469436370336309359
        static readonly Decimal128Bid ExpInternal_40A0_RI8_Q2 = new(UInt128.FromLoHi(0X2f81cac6d582d887UL, 0X2ffc3831e220cf02UL), CtorFromBits); // 0.1139767101043108526619106207783047
        static readonly Decimal128Bid ExpInternal_40A0_RI8_Q1 = new(UInt128.FromLoHi(0X3b3e4a5be638aff4UL, 0Xaffcf3d948764822UL), CtorFromBits); // -0.4945840470867546970644547838193652
                                                                                                                                                 // static readonly Decimal128Bid ExpInternal_40A0_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Numerator coefficients:
        static readonly Decimal128Bid ExpInternal_50A0_RI8_P8 = new(UInt128.FromLoHi(0X902f641d93600dc0UL, 0X2fec772fa95bf862UL), CtorFromBits); // 2.417382880624593654868327960415680E-9
        static readonly Decimal128Bid ExpInternal_50A0_RI8_P7 = new(UInt128.FromLoHi(0X84d5b2332edc2534UL, 0X2ff05186126521ecUL), CtorFromBits); // 1.653497444664582388154655626241332E-7
        static readonly Decimal128Bid ExpInternal_50A0_RI8_P6 = new(UInt128.FromLoHi(0X70536f853371dab8UL, 0X2ff311fed8ea9b21UL), CtorFromBits); // 5.557288907458876009179231821093560E-6
        static readonly Decimal128Bid ExpInternal_50A0_RI8_P5 = new(UInt128.FromLoHi(0X7db29f46591faf04UL, 0X2ff63a53a53d6740UL), CtorFromBits); // 1.183006833759169739871139923996420E-4
        static readonly Decimal128Bid ExpInternal_50A0_RI8_P4 = new(UInt128.FromLoHi(0Xc61ea75e091a7c15UL, 0X2ff8552250a8ae64UL), CtorFromBits); // 0.001726723536559751094835476541635605
        static readonly Decimal128Bid ExpInternal_50A0_RI8_P3 = new(UInt128.FromLoHi(0X8fdc141721daef99UL, 0X2ffa568645f1b707UL), CtorFromBits); // 0.01754925446372087837388564539830169
        static readonly Decimal128Bid ExpInternal_50A0_RI8_P2 = new(UInt128.FromLoHi(0Xf7ee0c3f4fffb76dUL, 0X2ffc3b62f6795831UL), CtorFromBits); // 0.1204502806550776690536470005397357
        static readonly Decimal128Bid ExpInternal_50A0_RI8_P1 = new(UInt128.FromLoHi(0Xe7d05dbdd6644444UL, 0X2ffcfa3742ebb818UL), CtorFromBits); // 0.5074980660828792821426721223099460
        static readonly Decimal128Bid ExpInternal_50A0_RI8_P0 = new(UInt128.FromLoHi(0X38c15b0a0fc37c0cUL, 0X2ffe314dc6448d93UL), CtorFromBits); // 1.000000000000000000000000264469516

        // Denominator coefficients:
        static readonly Decimal128Bid ExpInternal_50A0_RI8_Q8 = new(UInt128.FromLoHi(0X8c896210dd17cc62UL, 0X2fec4bff18282f1dUL), CtorFromBits); // 1.541391377934777491755286526413922E-9
        static readonly Decimal128Bid ExpInternal_50A0_RI8_Q7 = new(UInt128.FromLoHi(0X8a7f1e50b43a2188UL, 0Xaff039741cb4ec9dUL), CtorFromBits); // -1.165296698564107014126460078793096E-7
        static readonly Decimal128Bid ExpInternal_50A0_RI8_Q6 = new(UInt128.FromLoHi(0Xc7be32d7465c8cb9UL, 0X2ff2d12dec56af7dUL), CtorFromBits); // 4.242662017735013708599773978463417E-6
        static readonly Decimal128Bid ExpInternal_50A0_RI8_Q5 = new(UInt128.FromLoHi(0X3559a5bdf44d9a3eUL, 0Xaff5db66e83a0417UL), CtorFromBits); // -9.642297704970292088845088230054462E-5
        static readonly Decimal128Bid ExpInternal_50A0_RI8_Q4 = new(UInt128.FromLoHi(0X63b0811c22adeb35UL, 0X2ff849474d6525ddUL), CtorFromBits); // 0.001486265053231160207254959598463797
        static readonly Decimal128Bid ExpInternal_50A0_RI8_Q3 = new(UInt128.FromLoHi(0X9f9f15478645223fUL, 0Xaffa4dfdf4a7af5aUL), CtorFromBits); // -0.01581865981658381627299138030084671
        static readonly Decimal128Bid ExpInternal_50A0_RI8_Q2 = new(UInt128.FromLoHi(0X2be474054ccc964dUL, 0X2ffc37b092e463f9UL), CtorFromBits); // 0.1129522145721983869107901194606157
        static readonly Decimal128Bid ExpInternal_50A0_RI8_Q1 = new(UInt128.FromLoHi(0X4fbd308f0f3f37e9UL, 0Xaffcf2d27bc1cfa7UL), CtorFromBits); // -0.4925019339171207178573179550382057
                                                                                                                                                 // static readonly Decimal128Bid ExpInternal_50A0_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        static Decimal128Bid ExpInternal_50A1(Decimal128Bid x) {
            Debug.Assert(-0.50M <= x && x <= -0.40M);

            // Horner for numerator
            Decimal128Bid px = ExpInternal_50A1_RI8_P8;
            px = FusedMultiplyAdd(px, x, ExpInternal_50A1_RI8_P7);
            px = FusedMultiplyAdd(px, x, ExpInternal_50A1_RI8_P6);
            px = FusedMultiplyAdd(px, x, ExpInternal_50A1_RI8_P5);
            px = FusedMultiplyAdd(px, x, ExpInternal_50A1_RI8_P4);
            px = FusedMultiplyAdd(px, x, ExpInternal_50A1_RI8_P3);
            px = FusedMultiplyAdd(px, x, ExpInternal_50A1_RI8_P2);
            px = FusedMultiplyAdd(px, x, ExpInternal_50A1_RI8_P1);
            px = FusedMultiplyAdd(px, x, ExpInternal_50A1_RI8_P0);

            // Horner for denominator
            Decimal128Bid qx = ExpInternal_50A1_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, ExpInternal_50A1_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_50A1_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_50A1_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_50A1_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_50A1_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_50A1_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_50A1_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        static Decimal128Bid ExpInternal_40A1(Decimal128Bid x) {
            Debug.Assert(-0.40M <= x && x <= -0.25M);

            // Horner for numerator
            Decimal128Bid px = ExpInternal_40A1_RI8_P8;
            px = FusedMultiplyAdd(px, x, ExpInternal_40A1_RI8_P7);
            px = FusedMultiplyAdd(px, x, ExpInternal_40A1_RI8_P6);
            px = FusedMultiplyAdd(px, x, ExpInternal_40A1_RI8_P5);
            px = FusedMultiplyAdd(px, x, ExpInternal_40A1_RI8_P4);
            px = FusedMultiplyAdd(px, x, ExpInternal_40A1_RI8_P3);
            px = FusedMultiplyAdd(px, x, ExpInternal_40A1_RI8_P2);
            px = FusedMultiplyAdd(px, x, ExpInternal_40A1_RI8_P1);
            px = FusedMultiplyAdd(px, x, ExpInternal_40A1_RI8_P0);

            // Horner for denominator
            Decimal128Bid qx = ExpInternal_40A1_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, ExpInternal_40A1_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_40A1_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_40A1_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_40A1_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_40A1_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_40A1_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_40A1_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        static Decimal128Bid ExpInternal_25A1(Decimal128Bid x) {
            Debug.Assert(-0.25M <= x && x <= -0.10M);

            // Horner for numerator
            Decimal128Bid px = ExpInternal_25A1_RI8_P8;
            px = FusedMultiplyAdd(px, x, ExpInternal_25A1_RI8_P7);
            px = FusedMultiplyAdd(px, x, ExpInternal_25A1_RI8_P6);
            px = FusedMultiplyAdd(px, x, ExpInternal_25A1_RI8_P5);
            px = FusedMultiplyAdd(px, x, ExpInternal_25A1_RI8_P4);
            px = FusedMultiplyAdd(px, x, ExpInternal_25A1_RI8_P3);
            px = FusedMultiplyAdd(px, x, ExpInternal_25A1_RI8_P2);
            px = FusedMultiplyAdd(px, x, ExpInternal_25A1_RI8_P1);
            px = FusedMultiplyAdd(px, x, One);

            // Horner for denominator
            Decimal128Bid qx = ExpInternal_25A1_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, ExpInternal_25A1_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_25A1_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_25A1_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_25A1_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_25A1_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_25A1_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_25A1_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        static Decimal128Bid ExpInternal_10A(Decimal128Bid x) {
            Debug.Assert(-0.10M <= x && x <= 0.10M);

            // Horner for numerator
            Decimal128Bid px = ExpInternal_10A_RI8_P8;
            px = FusedMultiplyAdd(px, x, ExpInternal_10A_RI8_P7);
            px = FusedMultiplyAdd(px, x, ExpInternal_10A_RI8_P6);
            px = FusedMultiplyAdd(px, x, ExpInternal_10A_RI8_P5);
            px = FusedMultiplyAdd(px, x, ExpInternal_10A_RI8_P4);
            px = FusedMultiplyAdd(px, x, ExpInternal_10A_RI8_P3);
            px = FusedMultiplyAdd(px, x, ExpInternal_10A_RI8_P2);
            px = FusedMultiplyAdd(px, x, ExpInternal_10A_RI8_P1);
            px = FusedMultiplyAdd(px, x, One);

            // Horner for denominator
            Decimal128Bid qx = ExpInternal_10A_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, ExpInternal_10A_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_10A_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_10A_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_10A_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_10A_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_10A_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_10A_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        static Decimal128Bid ExpInternal_25A0(Decimal128Bid x) {
            Debug.Assert(0.10M <= x && x <= 0.25M);

            // Horner for numerator
            Decimal128Bid px = ExpInternal_25A0_RI8_P8;
            px = FusedMultiplyAdd(px, x, ExpInternal_25A0_RI8_P7);
            px = FusedMultiplyAdd(px, x, ExpInternal_25A0_RI8_P6);
            px = FusedMultiplyAdd(px, x, ExpInternal_25A0_RI8_P5);
            px = FusedMultiplyAdd(px, x, ExpInternal_25A0_RI8_P4);
            px = FusedMultiplyAdd(px, x, ExpInternal_25A0_RI8_P3);
            px = FusedMultiplyAdd(px, x, ExpInternal_25A0_RI8_P2);
            px = FusedMultiplyAdd(px, x, ExpInternal_25A0_RI8_P1);
            px = FusedMultiplyAdd(px, x, One);

            // Horner for denominator
            Decimal128Bid qx = ExpInternal_25A0_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, ExpInternal_25A0_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_25A0_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_25A0_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_25A0_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_25A0_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_25A0_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_25A0_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        static Decimal128Bid ExpInternal_40A0(Decimal128Bid x) {
            Debug.Assert(0.25M <= x && x <= 0.40M);

            // Horner for numerator
            Decimal128Bid px = ExpInternal_40A0_RI8_P8;
            px = FusedMultiplyAdd(px, x, ExpInternal_40A0_RI8_P7);
            px = FusedMultiplyAdd(px, x, ExpInternal_40A0_RI8_P6);
            px = FusedMultiplyAdd(px, x, ExpInternal_40A0_RI8_P5);
            px = FusedMultiplyAdd(px, x, ExpInternal_40A0_RI8_P4);
            px = FusedMultiplyAdd(px, x, ExpInternal_40A0_RI8_P3);
            px = FusedMultiplyAdd(px, x, ExpInternal_40A0_RI8_P2);
            px = FusedMultiplyAdd(px, x, ExpInternal_40A0_RI8_P1);
            px = FusedMultiplyAdd(px, x, ExpInternal_40A0_RI8_P0);

            // Horner for denominator
            Decimal128Bid qx = ExpInternal_40A0_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, ExpInternal_40A0_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_40A0_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_40A0_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_40A0_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_40A0_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_40A0_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_40A0_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        static Decimal128Bid ExpInternal_50A0(Decimal128Bid x) {
            Debug.Assert(0.40M <= x && x <= 0.50M);

            // Horner for numerator
            Decimal128Bid px = ExpInternal_50A0_RI8_P8;
            px = FusedMultiplyAdd(px, x, ExpInternal_50A0_RI8_P7);
            px = FusedMultiplyAdd(px, x, ExpInternal_50A0_RI8_P6);
            px = FusedMultiplyAdd(px, x, ExpInternal_50A0_RI8_P5);
            px = FusedMultiplyAdd(px, x, ExpInternal_50A0_RI8_P4);
            px = FusedMultiplyAdd(px, x, ExpInternal_50A0_RI8_P3);
            px = FusedMultiplyAdd(px, x, ExpInternal_50A0_RI8_P2);
            px = FusedMultiplyAdd(px, x, ExpInternal_50A0_RI8_P1);
            px = FusedMultiplyAdd(px, x, ExpInternal_50A0_RI8_P0);

            // Horner for denominator
            Decimal128Bid qx = ExpInternal_50A0_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, ExpInternal_50A0_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_50A0_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_50A0_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_50A0_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_50A0_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_50A0_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, ExpInternal_50A0_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        internal static Decimal128Bid ExpInternal_B0(Decimal128Bid x) {
            Debug.Assert(-0.5M <= x && x <= 0.5M);
            if (x < -Value_Pt10) {
                if (x >= -Value_Pt25) {
                    return ExpInternal_25A1(x);
                } else if (x >= -Value_Pt40) {
                    return ExpInternal_40A1(x);
                } else {
                    return ExpInternal_50A1(x);
                }
            } else {
                if (x <= Value_Pt25) {
                    if (x <= Value_Pt10) {
                        return ExpInternal_10A(x);
                    } else {
                        return ExpInternal_25A0(x);
                    }
                } else {
                    if (x <= Value_Pt40) {
                        return ExpInternal_40A0(x);
                    } else {
                        return ExpInternal_50A0(x);
                    }
                }
            }
        }

        public static Decimal128Bid Exp(Decimal128Bid x) {
            // Preserve NaN payload exactly
            if (Decimal128Bid.IsNaN(x)) {
                return x;
            }

            // +Inf -> +Inf, -Inf -> +0 (coarsest cohort)
            if (Decimal128Bid.IsInfinity(x)) {
                if (Decimal128Bid.IsPositiveInfinity(x)) {
                    return x;
                } else {
                    // -Inf -> Zero
                    return Decimal128Bid.Zero;
                }
            }

            // Exact zero: exp(0) == 1 but we must preserve cohort rules described:
            if (Decimal128Bid.IsZero(x)) {
                // Determine preferred q-exponent for zero input:
                var qx = ILogBQuantum(x); // q-exponent of input zero (may be negative)
                if (qx >= 0) {
                    return Decimal128Bid.One;
                } else {
                    // prefer finest cohort
                    return Decimal128Bid.ToFinestCohort(Decimal128Bid.One);
                }
            }

            // For nonzero finite x, result is inexact; we will return ToFinestCohort at the end.
            var f = x % One;
            //bool expIsInt = false;

            if (IsZero(f)) {
                //expIsInt = true;
            } else {
                if (f >= OneHalf_A0) {
                    f -= 1;
                } else if (f < -OneHalf_A0) {
                    f += 1;
                }
            }
            var i = x - f;

            if (i <= -14222) {
                return default(Decimal128Bid); // finest cohort of zero
            } else if (i >= 14150) {
                return Decimal128Bid.PositiveInfinity;
            }

            var ii = (int)(decimal)i;
            var nE = 0 > ii ? unchecked(-ii) : ii;
            var lE = nE & 127;
            var hE = nE >> 7;
            Decimal128Bid y = ExpInternal_B0(f) * (0 > ii ? ExpMInternal1(lE) : ExpInternal1(lE));
            y *= (0 > ii ? ExpMInternal2(hE) : ExpInternal2(hE));

            return Decimal128Bid.ToFinestCohort(y);
        }
    }
}
