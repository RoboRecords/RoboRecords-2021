/*
 * Menu.cs
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
