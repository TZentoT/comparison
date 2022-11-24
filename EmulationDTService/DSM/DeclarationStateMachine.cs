using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Emulation.DSM
{
    public sealed class DeclarationStateMachine
    {
        private static DSM _instance;

        public static string AppName
        {
            get { return Instance.AppName; }
        }

        public static DeclarationSubscriber Subs
        {
            get { return Instance.DeclarationSubscriber; }
        }
        public static List<DeclarationInterval> Intervals
        {
            get { return Instance.Intervals; }
        }

        public static List<DeclarationSignal> Signals
        {
            get { return Instance.Signals; }
        }

        public static Dictionary<string, object> Variables
        {
            get { return Instance.Variables; }
        }

        private static DSM Instance
        {
            get
            {
                if (_instance == null)
                {
                    try
                    {
                        //Console.Out.WriteLine(Directory.GetCurrentDirectory());
                        XmlReaderSettings readerSettings = new XmlReaderSettings();
                        readerSettings.IgnoreComments = true;
                        XmlReader reader = XmlReader
                            .Create($"{AppDomain.CurrentDomain.BaseDirectory}\\DeclarationStateMachine.xml", readerSettings);
                        XmlSerializer serializer = new XmlSerializer(typeof(DSM));
                        _instance = (DSM)serializer.Deserialize(reader);
                        _instance.Variables = _instance.VariablesList.ToDictionary(x => x.Name, y => string.IsNullOrEmpty(y.Type) ? (object)y.Text : TypeDescriptor.GetConverter(Type.GetType(y.Type)).ConvertFromString(y.Text));
                        reader.Close();
                        //SMLogger.Logger.Info(_instance.ToString());
                    }
                    catch (Exception e)
                    { }

                }
                return _instance;
            }
        }

        private DeclarationStateMachine()
        {
        }

        public new static string  ToString() => _instance.ToString();

        [Serializable()]
        [XmlRoot("DeclarationStateMachine")]
        public class DSM
        {
            [XmlElement("AppName")]
            public string AppName { get; set; }

            [XmlElement("Subscriber")]
            public DeclarationSubscriber DeclarationSubscriber { get; set; }

            [XmlArray("Intervals")]
            [XmlArrayItem("Interval", typeof(DeclarationInterval))]
            public List<DeclarationInterval> Intervals { get; set; }



            [XmlArray("Signals")]
            [XmlArrayItem("Signal", typeof(DeclarationSignal))]
            public List<DeclarationSignal> Signals { get; set; }

            [XmlArray("Variables")]
            [XmlArrayItem("var", typeof(DeclarationVariable))]
            public List<DeclarationVariable> VariablesList { get; set; }

            [XmlIgnore]
            public Dictionary<string, object> Variables = new Dictionary<string, object>();

            public override string ToString()
            {
                string s = string.Empty;
                var pr = this.GetType().GetProperties();
                foreach (var i in pr)
                {
                    if (i.PropertyType.Name == typeof(List<>).Name)
                    {
                        IList collection = (IList)i.GetValue(this);
                        s += i.Name + "\r\n";
                        foreach (var ii in collection)
                        {
                            s += ii.ToString() + "\r\n";
                        }
                        s += "\r\n";
                    }
                    else
                        s += $"{i.Name}={i.GetValue(this).ToString()}\r\n\r\n";
                }
                return s;
            }
        }

        public class DeclarationSubscriber
        {
            [XmlAttribute("IP")]
            public string IP { get; set; }

            [XmlAttribute("Port")]
            public int Port { get; set; }

            [XmlAttribute("IDSignWatchdog")]
            public ushort IDSignWatchdog { get; set; }

            [XmlAttribute("IntervalUpdateData")]
            public double IntervalUpdateData { get; set; }

            public override string ToString()
            {
                string s = string.Empty;
                var pr = this.GetType().GetProperties();
                foreach (var i in pr)
                {
                    s += $"{i.Name}={i.GetValue(this).ToString()}; ";
                }
                return s;
            }
        }

        public class DeclarationInterval
        {
            [XmlAttribute("IngotsX1")]
            public string XmlIngotsX1 { get; set; }

            [XmlAttribute("IngotsX2")]
            public string XmlIngotsX2 { get; set; }

            [XmlAttribute("IngotsY1")]
            public string XmlIngotsY1 { get; set; }

            [XmlAttribute("IngotsY2")]
            public string XmlIngotsY2 { get; set; }

            [XmlAttribute("IngotsThreadId")]
            public string XmlIngotsThreadId { get; set; }

            [XmlIgnore]
            public double? IngotsX1
            {
                get
                {
                    if (string.IsNullOrEmpty(XmlIngotsX1)) return null;
                    return Convert.ToDouble(XmlIngotsX1);
                }
            }

            [XmlIgnore]
            public double? IngotsX2
            {
                get
                {
                    if (String.IsNullOrEmpty(XmlIngotsX2)) return null;
                    return Convert.ToDouble(XmlIngotsX2);
                }
            }

            [XmlIgnore]
            public double? IngotsY1
            {
                get
                {
                    if (String.IsNullOrEmpty(XmlIngotsY1)) return null;
                    return Convert.ToDouble(XmlIngotsY1);
                }
            }

            [XmlIgnore]
            public double? IngotsY2
            {
                get
                {
                    if (String.IsNullOrEmpty(XmlIngotsY2)) return null;
                    return Convert.ToDouble(XmlIngotsY2);
                }
            }

            [XmlIgnore]
            public ushort? IngotsThreadId
            {
                get
                {
                    if (String.IsNullOrEmpty(XmlIngotsThreadId)) return null;
                    return Convert.ToUInt16(XmlIngotsThreadId);
                }
            }

            public override string ToString()
            {
                string s = string.Empty;
                var pr = GetType().GetProperties();
                foreach (var i in pr)
                {
                    if (!i.Name.Contains("Xml") && i.GetValue(this)!=null)
                    {
                        s += $"{i.Name}={i.GetValue(this)};";
                    }
                }
                return s;
            }

        }


        public class DeclarationSignal 
        {
            [XmlAttribute("id")]
            public ushort Id { get; set; }

            [XmlAttribute("name")]
            public string Name { get; set; }

            [XmlAttribute("tag")]
            public string Tag { get; set; }

            [XmlIgnore]
            public int? NoiseFilterInterval
            {
                get
                {
                    if (String.IsNullOrEmpty(XmlNoiseFilterInterval)) return null;
                    else return Convert.ToInt32(XmlNoiseFilterInterval);
                }
            }
            [XmlAttribute("NoiseFilterInterval")]
            public string XmlNoiseFilterInterval { get; set; }


            [XmlIgnore]
            public int? BounceFilterInterval
            {
                get
                {
                    if (String.IsNullOrEmpty(XmlBounceFilterInterval)) return null;
                    else return Convert.ToInt32(XmlBounceFilterInterval);
                }
            }
            [XmlAttribute("BounceFilterInterval")]
            public string XmlBounceFilterInterval { get; set; }

            [XmlIgnore]
            public double? CutoffUpperThreshold
            {
                get
                {
                    if (String.IsNullOrEmpty(XmlCutoffUpperThreshold)) return null;
                    else return Convert.ToDouble(XmlCutoffUpperThreshold);
                }
            }
            [XmlAttribute("CutoffUpperThreshold")]
            public string XmlCutoffUpperThreshold { get; set; }

            [XmlIgnore]
            public double? CutoffLowerThreshold
            {
                get
                {
                    if (String.IsNullOrEmpty(XmlCutoffLowerThreshold)) return null;
                    else return Convert.ToDouble(XmlCutoffLowerThreshold);
                }
            }
            [XmlAttribute("CutoffLowerThreshold")]
            public string XmlCutoffLowerThreshold { get; set; }

            public override string ToString()
            {
                string s = string.Empty;
                var pr = GetType().GetProperties();
                foreach (var i in pr)
                {
                    object _val = i.GetValue(this);
                    if (_val != null)
                        if (_val is string)
                        {
                            if (!string.IsNullOrEmpty((string)_val))
                            {
                                s += $"{i.Name}={_val}; ";
                            }
                        }
                        else
                            s += $"{i.Name}={_val}; ";
                }
                return s;
            }
        }

        public class DeclarationVariable
        {
            [XmlAttribute("name")]
            public string Name { get; set; }

            [XmlAttribute("text")]
            public string Text { get; set; }

            [XmlAttribute("type")]
            public string Type { get; set; }

            public override string ToString()
            {
                string s = string.Empty;
                var pr = GetType().GetProperties();
                foreach (var i in pr)
                {
                    s += $"{i.Name}={i.GetValue(this)}; ";
                }
                return s;
            }
        }

    }


}
