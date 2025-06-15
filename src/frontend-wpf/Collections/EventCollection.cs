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
            
            if (ev.name != null && ev.name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            {
                matchFound = true;
            }
            if (ev.Location?.name != null && ev.Location.name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            {
                matchFound = true;
            }
            if (ev.Location?.address != null && ev.Location.address.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            {
                matchFound = true;
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