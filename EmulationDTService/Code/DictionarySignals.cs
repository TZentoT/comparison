using System;
using System.Collections.Generic;
using System.Linq;
using MtsConnect;

namespace Emulation.Code
{
    public class DictionarySignals
    {
        private Dictionary<string, Signal> _signals = new Dictionary<string, Signal>();
        public DateTime TrackTime { get; private set; }
        public bool Changed
        {
            get { return _signals.Select(p => p.Value.Changed).Aggregate(false, (a, b) => a || b); }
        }
        public DateTime GetTrackTime()
        {
            return _signals.First().Value.Time;
        }
        public DictionarySignals(List<ushort> list_id)
        {
            foreach (var ds in list_id)
            {
                _signals.Add(ds.ToString(), new Signal(ds, ds.ToString(), ds.ToString()));
            }
        }
        public Signal this[string s]
        {
            get { return _signals[s]; }
        }
        public Signal this[int indx]
        {
            get { return _signals.First(t => t.Value.Id == indx).Value; }
        }

        public void UpdateListSignal(SubscriptionStateEventArgs e)
        {
            TrackTime = e.State.TrackTime;
            foreach (KeyValuePair<string, Signal> sig in _signals)
            {
                if (e.State.Signals[sig.Value.Id] != null)
                sig.Value.UpdateSignal(value: e.State.Signals[sig.Value.Id].Value, time: e.State.TrackTime);
                else sig.Value.UpdateSignal(value: sig.Value.Value, time: e.State.TrackTime);
            }
        }
        public override string ToString()
        {
            string rez = _signals.Aggregate("",
                             (a, b) => string.IsNullOrEmpty(a) ? $"{b.Value.Id}={b.Value.Value}" : $"{a}"+ Environment.NewLine + $"{b.Value.Id}={b.Value.Value}");
            return rez;
        }
    }
}
