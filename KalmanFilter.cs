using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace x
{
    class KalmanFilter
    {
        private static double Q = 0.001;
        private static double R = 0.1;
        private static double P = 100, X = 0, K;
        public void Q_hod(double Q_hodnota)
        {
            Q = Q_hodnota;
        }
        public void R_hod(double R_hodnota)
        {
            R = R_hodnota;
        }
        public void P_hod(double P_hodnota)
        {
            P = P_hodnota;
        }
        public void X_hod(double X_hodnota)
        {
            X = X_hodnota;
        }
        private static void measurementUpdate()
        {
            K = (P + Q) / (P + Q + R);
            P = R * (P + Q) / (R + P + Q);
        }
        public static double update(double measurement)
        {
            measurementUpdate();
            double result = X + (measurement - X) * K;
            X = result;
            return result;
        }
    }
}
