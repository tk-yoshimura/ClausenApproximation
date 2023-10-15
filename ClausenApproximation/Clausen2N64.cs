using MultiPrecision;

namespace ClausenApproximation {
    public static class Clausen2N64 {
        public static MultiPrecision<Pow2.N64> Value(MultiPrecision<Pow2.N64> x) {
            MultiPrecision<Plus8<Pow2.N64>> y = Clausen2<Plus8<Pow2.N64>>.ZetaAcceleration(x.Convert<Plus8<Pow2.N64>>());

            return y.Convert<Pow2.N64>();
        }
    }
}
