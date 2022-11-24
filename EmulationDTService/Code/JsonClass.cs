namespace Emulation.Code
{
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    
    public class  JsonClass
    {
        [JsonProperty("emulation")]
        public Emulation Emulation { get; set; }
    }
    
    public class Emulation
    {
        [JsonProperty("CreateIngot")]
        public CreateIngotJs[] CreateIngot { get; set; }

        [JsonProperty("IncrementSignal")]
        public IncrementSignalJs[] IncrementSignal { get; set; }

        [JsonProperty("ChangeThread")]
        public ChangeThreadJs[] ChangeThread { get; set; }
        
        [JsonProperty("EmulationMillStand")]
        public EmulationMillStandJs[] MillStandWork { get; set; }
        
        [JsonProperty("EmulationPresenceSensor")]
        public EmulationPresenceSensorJs[] PresenceSensorWork { get; set; }
    }
    
    public class ChangeThreadJs
    {
        [JsonProperty("ThreadOut")]
        public ushort ThreadOut { get; set; }

        [JsonProperty("CoordOut")]
        public double CoordOut { get; set; }

        [JsonProperty("ThreadIn")]
        public ushort ThreadIn { get; set; }

        [JsonProperty("CoordIn")]
        public double CoordIn { get; set; }

        [JsonProperty("Spin")]
        public byte Spin { get; set; }

        [JsonProperty("Length")]
        public double Length { get; set; }
    }

    public class CreateIngotJs
    {
        [JsonProperty("ThreadId")]
        public ushort ThreadId { get; set; }

        [JsonProperty("X")]
        public double X { get; set; }

        [JsonProperty("Length")]
        public double Length { get; set; }

        [JsonProperty("Width")]
        public double Width { get; set; }

        [JsonProperty("Timer")]
        public ushort Timer { get; set; }
    }

    public class IncrementSignalJs
    {
        [JsonProperty("Id")]
        public ushort Id { get; set; }

        [JsonProperty("Timer")]
        public ushort Timer { get; set; }
    }
    
    public class EmulationMillStandJs
    {
        [JsonProperty("Thread")]
        public ushort Thread { get; set; }

        [JsonProperty("Coordinate")]
        public double Coordinate { get; set; }

        [JsonProperty("WorkSignal")]
        public ushort WorkSignal { get; set; }
    }
    
    public class EmulationPresenceSensorJs
    {
        [JsonProperty("Thread")]
        public ushort Thread { get; set; }

        [JsonProperty("Coordinate")]
        public double Coordinate { get; set; }

        [JsonProperty("WorkSignal")]
        public ushort WorkSignal { get; set; }
    }
}