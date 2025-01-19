
using Newtonsoft.Json;

namespace Models
{
    public class BaseEventRequest 
    {
        public BaseEvent data {get;set;}
    }

    public class BaseEvent
    {
        [JsonProperty("event")]
        public Event Event { get; set; }
    }

    public class Event
    {
        public string name {get;set;}
        public int id {get;set;}

        public PaginatedSets sets {get;set;}
    
        

    }
}
