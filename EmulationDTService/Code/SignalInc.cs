using System;
using MtsConnect;
using System.Linq;
using System.Collections.Generic;

namespace Emulation.Code
{
    public class SignalInc
    {
        public ushort _id;
        public int _timer;

        public DateTime LastTime { get; private set; }

        public void Action(SignalsState _signals)
        {
            if ((DateTime.Now - LastTime).TotalSeconds >= _timer)
            {
                LastTime = DateTime.Now;
                //Subs._s.SetSignal(_id, _signals[_id].Inc());
                
                foreach (var s in _signals)
                {
                    if (s.Key == _id)
                    {
                        Subs._s.SetSignal(_id, s.Value + 1).Wait();
                    }
                }
            }
        }

        public SignalInc(ushort id, int timer)
        {
            this._id = id;
            this._timer = timer;
        }
    }
}