using UnityEngine;
using Zenject;

namespace GameCore.Scripts.MysteryBox.Rewards
{
    public class MushroomReward : ABoxReward
    {
        [SerializeField] private GameObject _mushroomModelPrefab;
        [SerializeField] private int _weaponUpgradeValue = 2;
        
        [Inject] public ModifiersController ModifiersController { get; }
        [Inject] public DiContainer Container { get; }

        private GameObject _spawnedReward;
        public override void SpawnReward()
        {
            _spawnedReward = Instantiate(_mushroomModelPrefab, transform);
            _spawnedReward.transform.localPosition = Vector3.zero;
        }

        public override void ClaimReward()
        {
            var mushroomModifier = new MushroomModifier("MysteryBoxMushroom", Container, _weaponUpgradeValue);
            ModifiersController.UseModifier(mushroomModifier);
            Destroy(_spawnedReward.gameObject);
        }
    }
}