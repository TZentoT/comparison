using NoStringEvaluating;
using System;

namespace EmulationDTService
{
    public class NoString
    {
        private static NoStringEvaluator.Facade facade = NoStringEvaluator.CreateFacade();
        private static NoStringEvaluator evaluator = facade.Evaluator;

        private int sign(int p1, int p2)
        {
            var direction = 0;
            switch (p1 ^ p2)
            {
                case 0: direction = 0; break;
                case 1: direction = 1; break;
            }
            if (p1 > p2) direction *= 1;
            else direction *= -1;

            return direction;
        }

        public void calc(double S1, double S2, double S3, double R = 0.2f, MtsConnect.Subscription _s = null)
        {
            var direction = sign((int)S2, (int)S3);
          
            _s.SetSignal(10006, evaluator.CalcNumber($"{direction}*2*pi*{R}*{S1}/60")).Wait();
        }
    }
}
