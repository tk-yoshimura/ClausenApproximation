using MultiPrecision;

namespace ClausenApproximation {
    internal class PlotCoef {
        static void Main() {
            for (int k = 1; k <= 32; k++) {
                MultiPrecision<Pow2.N32> c = Clausen2<Pow2.N32>.NearZeroCoef(k);
                Console.WriteLine($"{ToFP128(c)},");
            }

            Console.WriteLine();

            MultiPrecision<Pow2.N32> pi_div_e = MultiPrecision<Pow2.N32>.PI / MultiPrecision<Pow2.N32>.E;

            Console.WriteLine($"{ToFP128(pi_div_e)},");

            Console.WriteLine("END");
            Console.Read();
        }

        public static string ToFP128(MultiPrecision<Pow2.N32> x) {
            Sign sign = x.Sign;
            long exponent = x.Exponent;
            uint[] mantissa = x.Mantissa.Reverse().ToArray();

            string code = $"({(sign == Sign.Plus ? "+1" : "-1")}, {exponent}, 0x{mantissa[0]:X8}{mantissa[1]:X8}uL, 0x{mantissa[2]:X8}{mantissa[3]:X8}uL)";

            return code;
        }
    }
}