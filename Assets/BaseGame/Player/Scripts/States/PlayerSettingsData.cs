using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCPS.SlipForge.Player
{
    [CreateAssetMenu(fileName = "PlayerSettingsData", menuName = "BaseGame/PlayerSettings")]
    public class PlayerSettingsData : ScriptableObject
    {
        public float DodgeSpeed;
        public float DodgeSeconds;
        public float WalkSpeed;
        public int IFrames;
    }
}

