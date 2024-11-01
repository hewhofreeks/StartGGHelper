namespace Models
{
    public class BaseEntrants
    {
        public IEnumerable<Entrant> nodes {get;set;}
    }

    public class Entrant 
    {
        public int id {get;set;}
        public string name {get;set;}
        public PaginatedSets paginatedSets {get;set;}
    }
}