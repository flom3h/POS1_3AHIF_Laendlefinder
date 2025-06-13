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
}