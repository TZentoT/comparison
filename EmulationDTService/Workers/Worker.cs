using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MtsConnect;
using Emulation.Code;
using Newtonsoft.Json;

namespace EmulationDTService.Workers
{
    public class Worker
    {
        public static List<SignalInc> _signalInc = new List<SignalInc>();
        public static List<CreateIngot> _createIngot = new List<CreateIngot>();
        public static List<ChangeThread> _changeThreads = new List<ChangeThread>();
        public static List<MillStandEmulation> _millStandWork = new List<MillStandEmulation>();
        public static List<PresenceSensorEmulation> _presenceSensorWork = new List<PresenceSensorEmulation>();
        //public static DictionarySignals _signals;
        
        private readonly CancellationToken _cancellationToken = new();

        public Worker(ILogger<Worker> logger)
        {
            logger.LogInformation("Worker started");
            /*var webSocketReader = new WebSocketReader();
            new Task(async () =>  await webSocketReader.ReadWebSocketMessages(logger), _cancellationToken).Start();*/
            
            //
            
            Task.Run(() => { InitSubscr(); });
        }
        
        
        private async void InitSubscr()
        {
            Subs._c = await MtsTcpConnection.CreateAsync("127.0.0.1", 9977);
            SubscriptionConfig cfg = new SubscriptionConfig();

            JsonClass jsonClass1 = JsonConvert.DeserializeObject<JsonClass>(System.IO.File.ReadAllText(@"C:\mts\Config\Emulation.json"));
            if (jsonClass1.Emulation.ChangeThread.Length > 0)
            {
                foreach (var i in jsonClass1.Emulation.ChangeThread)
                {
                    cfg.AddAoI(thread: i.ThreadIn);
                    cfg.AddAoI(thread: i.ThreadOut);
                    _changeThreads.Add(new ChangeThread(i.ThreadOut, i.CoordOut, i.ThreadIn, i.CoordIn, i.Spin,
                        i.Length));
                }
            }

            List<ushort> lId = new List<ushort>();
            
            if (jsonClass1.Emulation.IncrementSignal.Length > 0)
            {
                foreach (var i in jsonClass1.Emulation.IncrementSignal)
                {
                    cfg.AddSignal(i.Id);
                    lId.Add(i.Id);
                    _signalInc.Add(new SignalInc(i.Id, i.Timer));
                }
            }
            cfg.AddSignal(11001);
            cfg.AddSignal(11002);
            cfg.AddSignal(11003);
            cfg.AddSignal(11004);
            cfg.AddSignal(11005);
            cfg.AddSignal(11006);
            cfg.AddSignal(11007);
            cfg.AddSignal(11008);
            cfg.AddSignal(11009);
            cfg.AddSignal(11010);
            cfg.AddSignal(11011);
            cfg.AddSignal(11012);
            _signalInc.Add(new SignalInc(11004, 5));//для идеального сигнала

            if (jsonClass1.Emulation.CreateIngot.Length > 0)
            {
                foreach (var i in jsonClass1.Emulation.CreateIngot)
                {
                    _createIngot.Add(new CreateIngot(i.ThreadId, i.X, i.Length, i.Width, i.Timer));
                }
            }

            if (jsonClass1.Emulation.MillStandWork.Length > 0)
            {
                foreach (var i in jsonClass1.Emulation.MillStandWork)
                {
                    cfg.AddAoI(thread: i.Thread);
                    _millStandWork.Add(new MillStandEmulation(i.WorkSignal, i.Thread, i.Coordinate));

                }
            }
            
            if (jsonClass1.Emulation.PresenceSensorWork.Length > 0)
            {
                foreach (var i in jsonClass1.Emulation.PresenceSensorWork)
                {
                    cfg.AddAoI(thread: i.Thread);
                    _presenceSensorWork.Add(new PresenceSensorEmulation(i.WorkSignal, i.Thread, i.Coordinate));

                }
            }

            cfg.Identity("Emulation");
            cfg.Timeout(TimeSpan.FromMilliseconds(1000));
            Subs._s = Subs._c.CreateNewSubscription(cfg);
            Subs._s.NewData += SubsNewData;
        }
        
        private static void SubsNewData(object sender, SubscriptionStateEventArgs e)
        {
            if (e.State.Signals != null)
            {
                //_signals.UpdateListSignal(e);
                if (_signalInc.Count > 0)
                {
                    foreach (SignalInc sInc in _signalInc)
                    {
                        sInc.Action(e.State.Signals);
                    }
                }
            }

            if (_createIngot.Count > 0)
            {
                foreach (CreateIngot crIngot in _createIngot)
                {
                    crIngot.Action();
                }
            }

            if (_changeThreads.Count > 0)
            {
                foreach (ChangeThread chTread in _changeThreads)
                {
                    chTread.Action(e.State.Ingots);
                }
            }
            
            if (_millStandWork.Count > 0)
            {
                foreach (MillStandEmulation mse in _millStandWork)
                {
                    mse.Action(e.State.Ingots);
                }
            }
            
            if (_presenceSensorWork.Count > 0)
            {
                foreach (PresenceSensorEmulation pse in _presenceSensorWork)
                {
                    pse.Action(e.State.Ingots);
                }
            }
        }
         
         
    }
}