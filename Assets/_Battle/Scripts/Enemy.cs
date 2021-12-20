using UnityEngine;

namespace BattleScripts
{
    internal interface IEnemy
    {
        void Update(DataPlayer dataPlayer);
    }

    internal class Enemy : IEnemy
    {
        private const float KMoney = 5f;
        private const float KPower = 1.5f;
        private const float MaxHealthPlayer = 20;

        private readonly string _name;

        private int _moneyPlayer;
        private int _healthPlayer;
        private int _powerPlayer;


        public Enemy(string name) =>
            _name = name;


        public void Update(DataPlayer dataPlayer)
        {
            switch (dataPlayer.DataType)
            {
                case DataType.Money:
                    _moneyPlayer = dataPlayer.Value;
                    break;

                case DataType.Health:
                    _healthPlayer = dataPlayer.Value;
                    break;

                case DataType.Power:
                    _powerPlayer = dataPlayer.Value;
                    break;
            }

            Debug.Log($"Notified {_name} change to {dataPlayer}");
        }

        public int CalcPower()
        {
            int kHealth = CalcKHealth();
            float moneyRatio = _moneyPlayer / KMoney;
            float powerRatio = _powerPlayer / KPower;

            return (int)(moneyRatio + kHealth + powerRatio);
        }

        private int CalcKHealth() =>
            _healthPlayer > MaxHealthPlayer ? 100 : 5;
    }
}

