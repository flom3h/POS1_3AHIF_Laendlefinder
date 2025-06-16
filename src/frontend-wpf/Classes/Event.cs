namespace Laendlefinder.Classes;

public class Event
{
    public int eid { get; set; }
    public string name { get; set; }
    public DateTime date { get; set; }
    public TimeSpan time { get; set; }
    public string description { get; set; }
    public string picture { get; set; }
    public int type { get; set; }
    
    public Location Location { get; set; }
}