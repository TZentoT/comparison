using Jace;
using System;
using System.Collections.Generic;

namespace EmulationDTService
{
    public class Jace
    {
        public const double R = 0.2f;


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
            Dictionary<string, double> variables = new Dictionary<string, double>();
            variables.Add("sign", direction);
            variables.Add("Pi", Math.PI);
            variables.Add("S1", S1);
            variables.Add("R", R);

            CalculationEngine engine = new CalculationEngine();
           /* return engine.Calculate("sign*2*Pi*R*S1/60", variables);*/
            _s.SetSignal(10006, engine.Calculate("sign*2*Pi*R*S1/60", variables)).Wait();
        }
    }
}
