using StaserSDK;
using UnityEngine;

namespace GameCore.Scripts.Castle.Tutorial.Logic
{
    public class TutorialFinisher : MonoBehaviour
    {
        [SerializeField] private TutorialStepBase _lastStep;
        
        private void OnEnable()
        {
            _lastStep.Ended += OnStepEnded;
        }

        private void OnStepEnded(TutorialStepBase _)
        {
            Tutorial.Complete();
        }
    }
}