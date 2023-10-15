using MultiPrecision;

namespace ClausenApproximation {
    public static class Clausen2N32 {
        public static MultiPrecision<Pow2.N32> Value(MultiPrecision<Pow2.N32> x) {
            MultiPrecision<Plus8<Pow2.N32>> y = Clausen2<Plus8<Pow2.N32>>.ZetaAcceleration(x.Convert<Plus8<Pow2.N32>>());

            return y.Convert<Pow2.N32>();
        }
    }
}
