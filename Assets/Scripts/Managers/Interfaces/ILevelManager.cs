using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeConquer.Managers
{
    public interface ILevelManager : IManager
    {
        int LevelCount { get; }

        bool LoadLevel(int index);
    }
}
