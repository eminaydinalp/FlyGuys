using _GAME.__Scripts.Obstacle;
using Rentire.Core;
using UnityEngine;

namespace _GAME.__Scripts.Controller
{
    public class FinalController : Singleton<FinalController>
    {
        public FinalObstacleParent finalObstacleParent;

        public void DoFinalTextFade()
        {
            finalObstacleParent.FinalTextFade();
        }
    }
}