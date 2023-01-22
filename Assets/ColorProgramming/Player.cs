using ColorProgramming;
using System.Collections;
using UnityEngine;

namespace ColorProgramming
{
    public class Player
    {
        public Pattern CurrentPattern { get; private set; }

        public Player() {
            CurrentPattern = Pattern.CLEAR;
        }
        public Player(Pattern currentPattern) { 
            CurrentPattern = currentPattern;
        }

        public void SetPattern(Pattern pattern)
        {
            CurrentPattern = pattern;
        }

    }
}