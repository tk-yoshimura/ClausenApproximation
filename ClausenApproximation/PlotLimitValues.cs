using MultiPrecision;

namespace ClausenApproximation {
    internal class PlotLimitValues {
        static void Main_() {
            StreamWriter sw = new("../../../../results/clausen2_n32_limit_2.csv");

            sw.WriteLine($"x,clausen2(x),clausen2(x^2),clausen2(x^2)/(x*(1-x))");

            List<MultiPrecision<Pow2.N32>> xs = new();

            for (MultiPrecision<Pow2.N32> x = 16383 / 16384d; x <= 1 - MultiPrecision<Pow2.N32>.Ldexp(1, -256); x = 1 - (1 - x) / 2) {
                xs.Add(x);
            }

            foreach (MultiPrecision<Pow2.N32> x in xs) {
                MultiPrecision<Pow2.N32> y = Clausen2N32.Value(x);
                MultiPrecision<Pow2.N32> y2 = Clausen2N32.Value(x * x);

                MultiPrecision<Pow2.N32> y_scaled = y2 / (x * (1 - x));

                Console.WriteLine($"{x},{y:e40},{y2:e40},{y_scaled:e40}");
                sw.WriteLine($"{x},{y},{y2},{y_scaled}");
            }

            sw.Close();

            Console.WriteLine("END");
            Console.Read();
        }
    }
}