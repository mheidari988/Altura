namespace AlturaCMS.Web.DataModels;

public class MenuItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public List<MenuItem> Children { get; set; } = new List<MenuItem>();
}
