using MultiPrecision;
using MultiPrecisionAlgebra;
using MultiPrecisionCurveFitting;

namespace ClausenPade {
    internal class Program {
        static void Main() {

            List<(MultiPrecision<Pow2.N32> x, MultiPrecision<Pow2.N32> y)> expecteds = new();

            using StreamReader sr = new("../../../../results/clausen2_n32.csv");

            sr.ReadLine();
            while (!sr.EndOfStream) {
                string? line = sr.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                }

                string[] line_split = line.Split(",");
                MultiPrecision<Pow2.N32> x = line_split[0], y = line_split[3];

                y /= MultiPrecision<Pow2.N32>.PI;

                expecteds.Add((x, y));
            }

            using StreamWriter sw_result = new("../../../../results_disused/clausen2_e32_pade.csv");

            Vector<Pow2.N32> xs = expecteds.Select(item => item.x).ToArray(), ys = expecteds.Select(item => item.y).ToArray();

            for (int m = 4; m <= 32; m++) {
                PadeFitter<Pow2.N32> pade = new(xs, ys, m, m, intercept: 0);

                Vector<Pow2.N32> param = pade.ExecuteFitting();
                Vector<Pow2.N32> errs = pade.Error(param);

                MultiPrecision<Pow2.N32> max_rateerr = 0;
                for (int i = 0; i < errs.Dim; i++) {
                    if (ys[i] == 0) {
                        continue;
                    }

                    max_rateerr = MultiPrecision<Pow2.N32>.Max(errs[i] / ys[i], max_rateerr);
                }

                Console.WriteLine($"m={m},n={m}");
                Console.WriteLine($"{max_rateerr:e20}");

                if (max_rateerr < 2e-32) {
                    sw_result.WriteLine($"m={m},n={m}");
                    sw_result.WriteLine("numer");
                    foreach (var (_, val) in param[..m]) {
                        sw_result.WriteLine(val);
                    }
                    sw_result.WriteLine("denom");
                    foreach (var (_, val) in param[m..]) {
                        sw_result.WriteLine(val);
                    }
                    sw_result.WriteLine("hexcode");
                    for (int i = 0; i < m; i++) {
                        sw_result.WriteLine($"({ToFP128(param[i])}, {ToFP128(param[i + m])}),");
                    }

                    sw_result.WriteLine("relative err");
                    sw_result.WriteLine($"{max_rateerr:e20}");
                    sw_result.Flush();

                    break;
                }
            }

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