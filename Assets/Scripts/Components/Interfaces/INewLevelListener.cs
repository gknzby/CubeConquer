using CubeConquer.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubeConquer.Components
{
    public interface INewLevelListener
    {
        void AddSelfToTheList();
        void RemoveSelfFromTheList();
        void NewLevelData(LevelData levelData);
    }
}
