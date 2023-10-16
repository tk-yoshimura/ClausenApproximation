using MultiPrecision;

namespace ClausenApproximation {
    internal class PlotValues {
        static void Main_() {
            StreamWriter sw = new("../../../../results/clausen2_n64.csv");

            sw.WriteLine($"x,clausen2(x),clausen2(x^2),clausen2(x^2)/(x*(1-x))");

            List<MultiPrecision<Pow2.N64>> xs = new() {
                0
            };

            for (MultiPrecision<Pow2.N64> x = MultiPrecision<Pow2.N64>.Ldexp(1, -100); x < 1 / 33554432d; x *= 2) {
                xs.Add(x);
            }
            for (MultiPrecision<Pow2.N64> x = 1 / 33554432d; x < 1 / 16384d; x += 1 / 33554432d) {
                xs.Add(x);
            }
            for (MultiPrecision<Pow2.N64> x = 1 / 16384d; x < 16383 / 16384d; x += 1 / 16384d) {
                xs.Add(x);
            }
            for (MultiPrecision<Pow2.N64> x = 16383 / 16384d; x <= 1; x += 1 / 33554432d) {
                xs.Add(x);
            }

            foreach (MultiPrecision<Pow2.N64> x in xs) {
                MultiPrecision<Pow2.N64> y = Clausen2N64.Value(x);
                MultiPrecision<Pow2.N64> y2 = Clausen2N64.Value(x * x);

                MultiPrecision<Pow2.N64> y_scaled =
                    x == 0 ? 0 :
                    x == 1 ? MultiPrecision<Pow2.N64>.PI * MultiPrecision<Pow2.N64>.Log(4) :
                    y2 / (x * (1 - x));

                Console.WriteLine($"{x},{y:e40},{y2:e40},{y_scaled:e40}");
                sw.WriteLine($"{x},{y},{y2},{y_scaled}");
            }

            sw.Close();

            Console.WriteLine("END");
            Console.Read();
        }
    }
}