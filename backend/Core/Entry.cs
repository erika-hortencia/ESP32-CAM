namespace backend
{
    public class Entry
    {
        public int type {get; set;}
        public int tokenId {get; set;}
        public string payload {get; set;}
        public string topic {get; set;}
        public long timestamp {get; set;}
    }
}