using JoostenProductions;
using UnityEngine;

namespace Game.InputLogic
{
    internal class InputKeys : BaseInputView
    {
        private void Start() =>
            UpdateManager.SubscribeToUpdate(Move);

        private void Move()
        {
            if (Input.GetKey(KeyCode.RightArrow))
                OnRightMove(0.02f);
            else if (Input.GetKey(KeyCode.LeftArrow))
                OnLeftMove(0.02f);
        }
    }
}
