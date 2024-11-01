namespace Models
{
    public class PaginatedSets
    {
        public IEnumerable<Set> nodes {get;set;}

        public PageInfo pageInfo {get;set;}
    }

    public class Set 
    {
        public int id {get;set;}
        public string winnerId {get;set;}
        public string fullRoundText {get;set;}
        public string displayScore {get;set;}
        public int? completedAt {get;set;}

        public DateTime CompletedAtDateTime { get { return UnixTimeStampToDateTime(completedAt.Value); }}

        public static DateTime UnixTimeStampToDateTime( double unixTimeStamp )
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds( unixTimeStamp ).ToLocalTime();
            return dateTime;
        }
    }
}