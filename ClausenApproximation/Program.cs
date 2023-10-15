using MultiPrecision;

namespace ClausenApproximation {
    internal class Program {
        static void Main() {
            MultiPrecision<Pow2.N32> y1 = Clausen2<Pow2.N32>.ZetaAcceleration(0.5d);
            MultiPrecision<Pow2.N32> y2 = Clausen2<Pow2.N32>.ZetaAcceleration(MultiPrecision<Pow2.N32>.Div(1, 3));

            Console.WriteLine(y1);
            Console.WriteLine(y2);

            Console.WriteLine("END");
            Console.Read();
        }
    }
}