using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows.Controls;
using Laendlefinder.UserControlls;
using System.Linq;
using Laendlefinder.Classes;

namespace Laendlefinder.Collections;

public class EventCollection : ObservableCollection<Event>
{
    public void Serialize(string filename)
    {
        using (StreamWriter stream = new StreamWriter(filename))
        {
            foreach (var ev in this)
            {
                string jsonStr = JsonSerializer.Serialize(ev);
                stream.WriteLine(jsonStr);
            }
        }
    }
    
    public void Draw(Panel panel)
    {
        panel.Children.Clear();
        foreach (var ev in this)
        {
            var eventControl = new EventMiniViewUserControl();
            eventControl.SetEventData(ev);
            panel.Children.Add(eventControl);
        }
    }
    
    public EventCollection Search(string searchTerm)
    {
        var results = new EventCollection();
        
        foreach (var ev in this)
        {
            if (ev == null)
            {
                continue;
            }

            bool matchFound = false;
            Console.WriteLine($"Searching in event: {ev.eid}, {ev.name}, {ev.Location?.name}, {ev.Location?.address}, {ev.type}");
            if (ev.name != null && ev.name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            {
                matchFound = true;
            }
            if (ev.Location?.name != null && ev.Location.name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            {
                matchFound = true;
                Console.WriteLine($"Found match in location name");
            }
            if (ev.Location?.address != null && ev.Location.address.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            {
                matchFound = true;
                Console.WriteLine($"Found match in location address");
            }
            else if (ev.type.HasValue && ev.type.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            {
                matchFound = true;
            }

            if (matchFound)
            {
                results.Add(ev);
            }
        }
        return results;
    }
}