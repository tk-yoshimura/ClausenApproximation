using MultiPrecision;

namespace ClausenApproximation {
    public static class Clausen2<N> where N : struct, IConstant {
        private static readonly List<MultiPrecision<N>> zetaevenm1_table = new() {
            MultiPrecision<N>.NaN
        };
        private static readonly List<MultiPrecision<N>> zeta_term_coefs = new() {
            MultiPrecision<N>.NaN
        };
        private static readonly List<MultiPrecision<N>> logloglimit_coefs = new() {
            MultiPrecision<N>.Zero
        };
        private static readonly List<MultiPrecision<N>> nearzero_coefs = new();

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

        public static MultiPrecision<N> ZetaAccelerationMk2(MultiPrecision<N> x, int max_term = 1024) {
            if (x < 0 || !(x <= 1)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x == 0 || x == 1) {
                return 0;
            }

            MultiPrecision<N> x2 = x * x;
            MultiPrecision<N> xpi = MultiPrecision<N>.PI * x;

            MultiPrecision<N> c =
                -MultiPrecision<N>.Log(xpi / MultiPrecision<N>.E * (1 - x2 / 4))
                - ((x.Exponent > -MultiPrecision<N>.Bits / 64)
                    ? (2 * (MultiPrecision<N>.Log((2 + x) / (2 - x)) / x - 1))
                    : LogLogLimit(x)
                );

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

        public static MultiPrecision<N> ZetaAccelerationMk3(MultiPrecision<N> x, int max_term = 1024) {
            if (x < 0 || !(x <= 1)) {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            if (x == 0 || x == 1) {
                return 0;
            }

            MultiPrecision<N> x2 = x * x;
            MultiPrecision<N> xpi = MultiPrecision<N>.PI * x;
            MultiPrecision<N> c = -MultiPrecision<N>.Log(xpi / MultiPrecision<N>.E * (1 - x2 / 4));

            MultiPrecision<N> ds, s = c, w = x2;

            for (int k = 1; k < max_term; k++) {
                ds = w * NearZeroCoef(k);
                s += ds;

                if (ds.Exponent <= s.Exponent - MultiPrecision<N>.Bits) {
                    return s * xpi;
                }

                w *= x2;
            }

            throw new ArithmeticException("Not convergence ZetaAcceleration.");
        }

        public static MultiPrecision<N> LogLogLimit(MultiPrecision<N> x, int terms = 32) {
            MultiPrecision<N> s = 0, x2 = x * x, w = x2;
            for (int k = 1; k <= terms; k++) {
                MultiPrecision<N> c = LogLogLimitCoef(k);

                s += w * c;
                w *= x2;
            }

            return s;
        }

        public static MultiPrecision<N> ZetaEvenM1(int n) {
            if (n >= zetaevenm1_table.Count) {
                for (int k = zetaevenm1_table.Count; k <= n; k++) {
                    MultiPrecision<Plus64<N>> zeta_even =
                        MultiPrecision<Plus64<N>>.Pow(2 * MultiPrecision<Plus64<N>>.PI, 2 * k)
                        * MultiPrecision<Plus64<N>>.Abs(MultiPrecision<Plus64<N>>.BernoulliSequence(k))
                        * MultiPrecision<Plus64<N>>.TaylorSequence[2 * k] / 2;

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

        public static MultiPrecision<N> LogLogLimitCoef(int n) {
            if (n >= logloglimit_coefs.Count) {
                for (int k = logloglimit_coefs.Count; k <= n; k++) {
                    MultiPrecision<N> c = MultiPrecision<N>.Ldexp(
                        MultiPrecision<N>.Div(1, 2 * k + 1), -2 * k + 1
                    );

                    logloglimit_coefs.Add(c);
                }
            }

            return logloglimit_coefs[n];
        }

        public static MultiPrecision<N> NearZeroCoef(int n) {
            if (n >= nearzero_coefs.Count) {
                for (int k = nearzero_coefs.Count; k <= n; k++) {
                    MultiPrecision<N> c = ZetaCoef(k) - LogLogLimitCoef(k);

                    nearzero_coefs.Add(c);
                }
            }

            return nearzero_coefs[n];
        }
    }
}
