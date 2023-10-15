using MultiPrecision;

namespace ClausenApproximation {
    internal class Program {
        static void Main() {
            StreamWriter sw = new("../../../../results/clausen2_n32.csv");

            sw.WriteLine($"x,clausen2(x),clausen2(x^2),clausen2(x)/(x*(1-x)),clausen2(x^2)/(x*(1-x))");

            List<MultiPrecision<Pow2.N32>> xs = new();

            for (MultiPrecision<Pow2.N32> x = 0; x < 1 / 16384d; x += 1 / 33554432d) {
                xs.Add(x);
            }
            for (MultiPrecision<Pow2.N32> x = 1 / 16384d; x <= 1; x += 1 / 16384d) {
                xs.Add(x);
            }

            foreach (MultiPrecision<Pow2.N32> x in xs) {
                MultiPrecision<Pow2.N32> y = Clausen2N32.Value(x);
                MultiPrecision<Pow2.N32> y2 = Clausen2N32.Value(x * x);

                MultiPrecision<Pow2.N32> y3 = y / (x * (1 - x));
                MultiPrecision<Pow2.N32> y4 = y2 / (x * (1 - x));

                Console.WriteLine($"{x},{y:e40},{y2:e40},{y3:e40},{y4:e40}");
                sw.WriteLine($"{x},{y},{y2},{y3},{y4}");
            }

            sw.Close();

            Console.WriteLine("END");
            Console.Read();
        }
    }
}