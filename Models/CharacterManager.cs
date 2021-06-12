using System.Collections.Generic;

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
            _standardCharacter.Add(new RoboCharacter("Sonic"));
            _standardCharacter.Add(new RoboCharacter("Tails"));
            _standardCharacter.Add(new RoboCharacter("Knuckles"));
            _standardCharacter.Add(new RoboCharacter("Amy"));
            _standardCharacter.Add(new RoboCharacter("Fang"));
            _standardCharacter.Add(new RoboCharacter("Metal Sonic"));
            _inited = true;
        }

        public static RoboCharacter GetCharacterById(string id)
        {
            return StandardCharacters.Find(character => character.NameId == id);
        }
    }
}