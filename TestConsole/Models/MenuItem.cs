using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole.Models
{
    class MenuItem
    {
        public string name;
        public MenuAction action;
        public string parameter;
        public Menu subMenu;

        public MenuItem(string _name, MenuAction _action)
        {
            name = _name;
            action = _action;
            parameter = "";
            subMenu = new Menu("Empty Menu");
        }

        public MenuItem(string _name, MenuAction _action, string _parameter)
        {
            name = _name;
            action = _action;
            parameter = _parameter;
            subMenu = new Menu("Empty Menu");
        }

        public MenuItem(string _name, MenuAction _action, string _parameter, Menu _subMenu)
        {
            name = _name;
            action = _action;
            parameter = _parameter;
            subMenu = _subMenu;
        }

        public MenuItem(string _name, MenuAction _action, Menu _subMenu)
        {
            name = _name;
            action = _action;
            parameter = "";
            subMenu = _subMenu;
        }
    }
}
