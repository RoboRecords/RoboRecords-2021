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