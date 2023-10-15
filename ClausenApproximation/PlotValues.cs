using MultiPrecision;

namespace ClausenApproximation {
    internal class PlotValues {
        static void Main() {
            StreamWriter sw = new("../../../../results/clausen2_n32.csv");

            sw.WriteLine($"x,clausen2(x),clausen2(x^2),clausen2(x^2)/(x*(1-x))");

            List<MultiPrecision<Pow2.N32>> xs = new() {
                0
            };

            for (MultiPrecision<Pow2.N32> x = MultiPrecision<Pow2.N32>.Ldexp(1, -100); x < 1 / 33554432d; x *= 2) {
                xs.Add(x);
            }
            for (MultiPrecision<Pow2.N32> x = 1 / 33554432d; x < 1 / 16384d; x += 1 / 33554432d) {
                xs.Add(x);
            }
            for (MultiPrecision<Pow2.N32> x = 1 / 16384d; x < 16383 / 16384d; x += 1 / 16384d) {
                xs.Add(x);
            }
            for (MultiPrecision<Pow2.N32> x = 16383 / 16384d; x <= 1; x += 1 / 33554432d) {
                xs.Add(x);
            }

            foreach (MultiPrecision<Pow2.N32> x in xs) {
                MultiPrecision<Pow2.N32> y = Clausen2N32.Value(x);
                MultiPrecision<Pow2.N32> y2 = Clausen2N32.Value(x * x);

                MultiPrecision<Pow2.N32> y_scaled =
                    x == 0 ? 0 :
                    x == 1 ? MultiPrecision<Pow2.N32>.PI * MultiPrecision<Pow2.N32>.Log(4) :
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