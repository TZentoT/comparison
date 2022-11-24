using MtsConnect;
namespace Emulation.Code

{
    public class PresenceSensorEmulation
    {
        private ushort _id;
        private ushort _thread;
        private double _coord;
        

        public void Action(IngotsState Ingots)
        {
            var ing = Ingots.IntersectionWithPointX(_coord, _thread);

            if (ing.Count > 0)
            {
                Subs._s.SetSignal(_id, 1).Wait();
            }
            else
            {
                Subs._s.SetSignal(_id, 0).Wait();
            }
        }

        public PresenceSensorEmulation(ushort id, ushort thread, double coord)
        {
            this._id = id;
            this._thread = thread;
            this._coord = coord;
        }
    }
}