using System;
using System.Collections.Generic;
using System.Linq;
using MtsConnect;

namespace Emulation.Code
{
    public class ChangeThread
    {
        private ushort _threadout;
        private double _coordout;
        private ushort _threadin;
        private double _coordin;
        private byte _spin;
        private double _length;
        public void Action(IngotsState Ingots)
        {
            List<Ingot> ing = new List<Ingot>();
            
            foreach (var ing2 in Ingots)
            {
                if (ing2.Thread == _threadout && ing2.X2 > _coordout)
                {
                    ing.Add(ing2);
                }
            }

            //var ing = Ingots.IntersectionWithPointX(_coordout, _threadout);
            if (ing.Count != 0)
            {
                foreach (var i in ing)
                {
                    if (_spin == 0)
                    {
                        if (_length == 0)
                            Subs._s.ModifyIngot(ingotId: i.Id, _coordin, _coordin + i.Length, thread: _threadin).Wait();
                        else Subs._s.ModifyIngot(ingotId: i.Id, _coordin, _coordin + _length, thread: _threadin).Wait();
                    }
                    else
                    {
                        if (_length != 0)
                            Subs._s.ModifyIngot(ingotId: i.Id, x1: _coordin, _coordin + _length, 0, y2: i.Length,
                                thread: _threadin).Wait();
                        else
                            Subs._s.ModifyIngot(ingotId: i.Id, x1: _coordin, _coordin + i.Width, 0, y2: i.Length,
                                thread: _threadin).Wait();
                    }
                }
            }
        }
        
        public ChangeThread(ushort threadout, double coordout, ushort threadin, double coordin, byte spin, double length) 
        {
            _threadout = threadout;
            _coordout = coordout;
            _threadin = threadin;
            _coordin = coordin;
            _length = length;
            _spin = spin;
        }
    }
}