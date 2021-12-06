using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole.Models
{
    class Menu
    {
        public string name;
        public List<MenuItem> menuItems;
        public Menu(string _name)
        {
            name = _name;
            menuItems = new List<MenuItem>();
        }

        public Menu(string _name, List<MenuItem> _items)
        {
            name = _name;
            menuItems = _items;
        }
    }
}
