/*
 * MenuItem.cs
 * Copyright (C) 2021, Zenya <Zeritar>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

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
