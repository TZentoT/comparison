using MtsConnect;
namespace Emulation.Code
{
    public class MillStandEmulation
    {
        private ushort _id;
        private ushort _thread;
        private double _coord;
        

        public void Action(IngotsState Ingots)
        {
            var ing = Ingots.IntersectionWithAreaX(_coord-2,_coord+2, _thread);

            if (ing.Count > 0)
            {
                Subs._s.SetSignal(_id, 1).Wait();
            }
            else
            {
                Subs._s.SetSignal(_id, 0).Wait();
            }
        }

        public MillStandEmulation(ushort id, ushort thread, double coord)
        {
            this._id = id;
            this._thread = thread;
            this._coord = coord;
        }
    }
}