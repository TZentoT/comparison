using System;
using Emulation.DSM;

namespace Emulation.Code
{
    public class Signal
    {
        private DeclarationStateMachine.DeclarationSignal _descr;
        public ushort Id => _descr.Id;
        public string Name => _descr.Name;
        public string Tag => _descr.Tag;
        public double LastValue { get; private set; }
        public double Value { get; private set; }
        public DateTime LastTime { get; private set; }
        public DateTime Time { get; private set; }
        public TimeSpan IntervalNotZeroValue { get; private set; }
        public TimeSpan IntervalZeroValue { get; private set; }
        public bool Front { get; private set; }
        public bool Backfront { get; private set; }
        public bool Changed { get; private set; }
        public bool BoolValue => Value != 0;
        public bool NoiseFilterVal { get; private set; }
        private DateTime _noiseFilterTimer;
        private bool _noiseFilterOnCheck;
        private double _incValue;

        public double Inc()
        {
            _incValue = _incValue < 65536 ? _incValue + 1 : 0;
            return this._incValue;
        }

        public double Dec()
        {
            _incValue = _incValue > 0 ? _incValue - 1 : 65536;
            return this._incValue;
        }

        private void NoiseFilter()
        {
            if (_descr.NoiseFilterInterval == null) return;
            if (Value == 0)
            {
                NoiseFilterVal = false;
                _noiseFilterOnCheck = false;
                return;
            }
            if (Front)
            {
                _noiseFilterOnCheck = true;
                _noiseFilterTimer = Time;
            }
            if (_noiseFilterOnCheck && (Time - _noiseFilterTimer).Milliseconds >= _descr.NoiseFilterInterval)
            {
                NoiseFilterVal = true;
            }
        }
        public bool BounceFilterVal;
        private DateTime _bounceFilterTimer;
        private bool _bounceFilterOnCheck;
        private void BounceFilter()
        {
            if (_descr.BounceFilterInterval == null) return;
            if (Value != 0)
            {
                BounceFilterVal = true;
                _bounceFilterOnCheck = false;
                return;
            }
            if (Backfront)
            {
                _bounceFilterOnCheck = true;
                _bounceFilterTimer = Time;
            }
            if (_bounceFilterOnCheck && (Time - _bounceFilterTimer).Milliseconds >= _descr.BounceFilterInterval)
            {
                BounceFilterVal = false;
            }
        }

        public double CutoffVal = 0;
        private bool _checkCutoff = false;
        private void Cutoff()
        {
            if (_descr.CutoffUpperThreshold == null) return;
            if (LastValue <= _descr.CutoffUpperThreshold && Value > _descr.CutoffUpperThreshold)
            {
                _checkCutoff = true;
                CutoffVal = Value;
            }
            if (LastValue >= _descr.CutoffLowerThreshold && Value < _descr.CutoffLowerThreshold)
            {
                _checkCutoff = false;
            }
            CutoffVal = _checkCutoff ? Value : 0;
        }

        public Signal(ushort id, string name, string tag) { this._descr.Id = id; this._descr.Name = name; this._descr.Tag = tag; }
        public Signal(DeclarationStateMachine.DeclarationSignal descrSignal) { this._descr = descrSignal; }


        public void UpdateSignal(double value, DateTime time)
        {
            if (LastTime == null || LastTime == DateTime.MinValue) //получаем значение сигнала впервый раз (на 1 цикле опроса MTS)
            {
                LastValue = value; this.Value = value;
                this.Time = time; LastTime = time;
                IntervalNotZeroValue = TimeSpan.Zero;
                IntervalZeroValue = TimeSpan.Zero;
                Changed = false;
                if (value != 0)
                {
                    _noiseFilterOnCheck = true;
                    _noiseFilterTimer = time;
                    BounceFilterVal = true;
                    if (_descr.CutoffUpperThreshold != null && Value > _descr.CutoffUpperThreshold)
                        _checkCutoff = true;
                }
            }
            else
            {
                if (Front) { IntervalZeroValue = TimeSpan.Zero; Front = false; }
                if (Backfront) { IntervalNotZeroValue = TimeSpan.Zero; Backfront = false; }
                LastTime = this.Time; this.Time = time;
                LastValue = this.Value; this.Value = value;

                if (LastValue == 0 && this.Value == 0) //если держится значение сигнала равным 0 
                { IntervalZeroValue = IntervalZeroValue + (this.Time - LastTime); Front = false; Backfront = false; }
                if (LastValue != 0 && this.Value != 0) //если держится значение сигнала не равным 0 
                { IntervalNotZeroValue = IntervalNotZeroValue + (this.Time - LastTime); Front = false; Backfront = false; }
                if (LastValue == 0 && this.Value != 0) //если значение сигнала изменилось с "0" на "не 0"
                { IntervalNotZeroValue = this.Time - LastTime; Front = true; }
                if (LastValue != 0 && this.Value == 0) //если значение сигнала изменилось с "не 0" на "0"
                { IntervalZeroValue = this.Time - LastTime; Backfront = true; }
                Changed = (value != LastValue);
            }
            NoiseFilter();
            BounceFilter();
            Cutoff();
            _incValue = Value;
        }


        public override string ToString()
        {
            return
                $"{this.Id}={Value}";
        }
    }
}
