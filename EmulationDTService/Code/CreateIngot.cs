using System;

namespace Emulation.Code
{
    public class CreateIngot
    {
        public ushort _thread;
        public double _coord;
        public double _length;
        public double _width;
        public int _timer;

        public DateTime LastTime { get; private set; }

        public void Action()
        {
            if ((DateTime.Now - LastTime).TotalSeconds >= _timer)
            {
                LastTime = DateTime.Now;
                Subs._s.AddIngot(x1: _coord, x2: _coord + _length, y1: 0, y2: _width, thread: _thread, level3Id: 0,
                    baseId: 1, rollingId: 0).Wait();
            }
        }

        public CreateIngot(ushort thread, double coord, double length, double width, int timer)
        {
            _thread = thread;
            _timer = timer;
            _coord = coord;
            _length = length;
            _width = width;
        }
    }
}