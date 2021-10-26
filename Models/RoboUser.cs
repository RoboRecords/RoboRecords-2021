/*
 * RoboUser.cs: the user model as stored in the database
 * Copyright (C) 2021, Refrag <R3FR4G>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System;

namespace RoboRecords.Models
{
    public class RoboUser
    {
        public int DbId;
        
        public short Discriminator;

        public string UserNameNoDiscrim;
        public RoboUser(string userName, short discriminator)
        {
            Discriminator = discriminator;
            UserNameNoDiscrim = userName;
        }

        // Needed for the database context
        public RoboUser()
        {
            
        }
    }
}