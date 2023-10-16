using MultiPrecision;

namespace ClausenApproximation {
    internal class Program {
        static void Main() {
            MultiPrecision<Pow2.N64> x = MultiPrecision<Pow2.N64>.Ldexp(1, -32);

            MultiPrecision<Pow2.N64> y1 = Clausen2<Pow2.N64>.ZetaAcceleration(x);
            MultiPrecision<Pow2.N64> y2 = Clausen2<Pow2.N64>.ZetaAccelerationMk2(x);

            Console.WriteLine(y1);
            Console.WriteLine(y2);

            Console.WriteLine("END");
            Console.Read();
        }
    }
}