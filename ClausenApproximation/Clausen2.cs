using MultiPrecision;

namespace ClausenApproximation {
    public static class Clausen2<N> where N : struct, IConstant {
        private static readonly List<MultiPrecision<N>> zetaevenm1_table = new() {
            MultiPrecision<N>.NaN
        };
        private static readonly List<MultiPrecision<N>> zeta_term_coefs = new() {
            MultiPrecision<N>.NaN
        };

        public static MultiPrecision<N> Fourier(MultiPrecision<N> x, long max_term = 65536) {
            if (x < 0 || !(x <= MultiPrecision<N>.PI)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            MultiPrecision<N> ds, s = 0;

            for (long k = 1; k < max_term; k += 2) {
                ds =
                    MultiPrecision<N>.Sin(x * k) / (k * k)
                    + MultiPrecision<N>.Sin(x * k + x) / ((k + 1) * (k + 1));

                s += ds;

                if (ds.Exponent <= s.Exponent - MultiPrecision<N>.Bits) {
                    return s;
                }
            }

            throw new ArithmeticException("Not convergence Fourier.");
        }

        public static MultiPrecision<N> FourierPI(MultiPrecision<N> x, long max_term = 65536) {
            if (x < 0 || !(x <= 1)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            MultiPrecision<N> ds, s = 0;

            for (long k = 1; k < max_term; k += 2) {
                ds =
                    MultiPrecision<N>.SinPI(x * k) / (k * k)
                    + MultiPrecision<N>.SinPI(x * k + x) / ((k + 1) * (k + 1));

                s += ds;

                if (ds.Exponent <= s.Exponent - MultiPrecision<N>.Bits) {
                    return s;
                }
            }

            throw new ArithmeticException("Not convergence Fourier.");
        }

        public static MultiPrecision<N> ZetaAcceleration(MultiPrecision<N> x, int max_term = 1024) {
            if (x < 0 || !(x <= 1)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x == 0 || x == 1) {
                return 0;
            }

            MultiPrecision<N> x2 = x * x;
            MultiPrecision<N> xpi = MultiPrecision<N>.PI * x;

            MultiPrecision<N> c = 3
                - MultiPrecision<N>.Log(xpi * (1 - x2 / 4))
                - 2 / x * MultiPrecision<N>.Log((2 + x) / (2 - x));

            MultiPrecision<N> ds, s = c, w = x2;

            for (int k = 1; k < max_term; k++) {
                ds = w * ZetaCoef(k);
                s += ds;

                if (ds.Exponent <= s.Exponent - MultiPrecision<N>.Bits) {
                    return s * xpi;
                }

                w *= x2;
            }

            throw new ArithmeticException("Not convergence ZetaAcceleration.");
        }

        public static MultiPrecision<N> ZetaEvenM1(int n) {
            if (n >= zetaevenm1_table.Count) {
                for (int k = zetaevenm1_table.Count; k <= n; k++) {
                    MultiPrecision<Plus8<N>> zeta_even =
                        MultiPrecision<Plus8<N>>.Pow(2 * MultiPrecision<Plus8<N>>.PI, 2 * k)
                        * MultiPrecision<Plus8<N>>.Abs(MultiPrecision<Plus8<N>>.BernoulliSequence(k))
                        * MultiPrecision<Plus8<N>>.TaylorSequence[2 * k] / 2;

                    zetaevenm1_table.Add((zeta_even - 1).Convert<N>());
                }
            }

            return zetaevenm1_table[n];
        }

        public static MultiPrecision<N> ZetaCoef(int n) {
            if (n >= zeta_term_coefs.Count) {
                for (int k = zeta_term_coefs.Count; k <= n; k++) {
                    MultiPrecision<N> c = MultiPrecision<N>.Ldexp(
                        ZetaEvenM1(n) / (n * (2 * n + 1)), -2 * n
                    );

                    zeta_term_coefs.Add(c);
                }
            }

            return zeta_term_coefs[n];
        }
    }
}
