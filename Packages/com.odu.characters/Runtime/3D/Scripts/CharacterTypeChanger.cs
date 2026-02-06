using System;
using UnityEngine;

namespace OduLib.Systems.Characters
{
    public class CharacterTypeChanger : MonoBehaviour
    {
        [SerializeField] private CharacterInputReader _characterInputReader;
        [SerializeField] private CharacterPack[] _characterMovements;

        [Serializable]
        private struct CharacterPack
        {
            public CharacterMovement Movement;
            public GameObject Camera;

            public void Enter()
            {
                Movement.Enter();
                Camera.SetActive(true);
            }

            public void Exit()
            {
                Movement.Exit();
                Camera.SetActive(false);
            }
        }

        private int _currentCharacterMovementIndex;

        private void OnEnable()
        {
            _characterInputReader.ChangeTypePerformed += SwitchCharacterMovement;
        }

        private void OnDisable()
        {
            _characterInputReader.ChangeTypePerformed -= SwitchCharacterMovement;
        }

        private void SwitchCharacterMovement()
        {
            _characterMovements[_currentCharacterMovementIndex].Exit();

            _currentCharacterMovementIndex++;
            _currentCharacterMovementIndex %= _characterMovements.Length;

            _characterMovements[_currentCharacterMovementIndex].Enter();
            _characterInputReader.CurrentCharacterMovement = _characterMovements[_currentCharacterMovementIndex].Movement;
        }
    }
}