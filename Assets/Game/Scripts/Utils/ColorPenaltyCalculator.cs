using Bird;
using ManagementUtilities;
using System.Collections.Generic;
using Timer;
using UnityEngine;
using YG;

namespace Utility
{
    public class ColorPenaltyCalculator : MonoBehaviour
    {
        [SerializeField] private RemoverFigures _remover;
        [SerializeField] private Timeline _timeline;

        private MeshRenderer _renderer;
        private Color _color;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
        }

        private void OnEnable()
        {
            _remover.Fined += CalculatePenalty;
        }

        private void OnDisable()
        {
            _remover.Fined -= CalculatePenalty;
        }

        public void SetGeese(List<Goose> geese)
        {
            foreach (var goose in geese)
                goose.Attacker.Hited += AddTime;
        }

        private void CalculatePenalty(Color color)
        {
            if (_color != color)
                AddTime();

            _color = color;
            _renderer.material.color = color;
        }

        private void AddTime()
        {
            int minChance = 0;
            int maxChance = 100;
            int multiplier = 10;
            int currentChance = YandexGame.savesData.ChanceLuck * multiplier;
            int chance = Random.Range(minChance, maxChance);

            if (chance > currentChance)
                _timeline.AddTime();
        }
    }
}
