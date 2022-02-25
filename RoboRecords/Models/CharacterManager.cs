/*
 * CharacterManager.cs
 * Copyright (C) 2022, Ors <Riku-S> and Zenya <Zeritar>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System.Collections.Generic;
using RoboRecords.DbInteraction;

namespace RoboRecords.Models
{
    public static class CharacterManager
    {
        static List<RoboCharacter> _standardCharacter;
        private static bool _inited = false;
        public static List<RoboCharacter> StandardCharacters
        {
            get
            {
                if (!_inited)
                {
                    Init();
                }
                return _standardCharacter;
            }
        }
        static void Init()
        {
            _standardCharacter = new List<RoboCharacter>();
            if (DbSelector.TryGetCharacterFromNameId("sonic", out RoboCharacter sonic))
                _standardCharacter.Add(sonic);
            if (DbSelector.TryGetCharacterFromNameId("tails", out RoboCharacter tails))
                _standardCharacter.Add(tails);
            if (DbSelector.TryGetCharacterFromNameId("knuckles", out RoboCharacter knuckles))
                _standardCharacter.Add(knuckles);
            if (DbSelector.TryGetCharacterFromNameId("amy", out RoboCharacter amy))
                _standardCharacter.Add(amy);
            if (DbSelector.TryGetCharacterFromNameId("fang", out RoboCharacter fang))
                _standardCharacter.Add(fang);
            if (DbSelector.TryGetCharacterFromNameId("metalsonic", out RoboCharacter metalsonic))
                _standardCharacter.Add(metalsonic);
            _inited = true;
        }

        public static RoboCharacter GetCharacterById(string id)
        {
            return StandardCharacters.Find(character => character.NameId == id);
        }
    }
}