using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

#if EVENTS_PRESENT
using OduLib.Canivete.Events;
#endif

namespace OduLib.Canivete.Input {
    /// <summary>
    /// Fornece métodos utilitários para controlar a vibração do gamepad (rumble).
    /// </summary>
    public static class Rumbler 
    {
        public enum VibrationMode{
            None,
            Minimal,
            VeryLow,
            Low,
            Medium,
            High,
            Max
        }
        /// <summary>
        /// Inicia a vibração do gamepad usando modos predefinidos para baixa e alta frequência.
        /// </summary>
        /// <param name="lowFrequencyMode">Modo de intensidade para a baixa frequência do motor.</param>
        /// <param name="highFrequencyMode">Modo de intensidade para a alta frequência do motor.</param>
        /// <param name="duration">Duração da vibração em segundos.</param>
        public static void GamepadRumble(VibrationMode lowFrequencyMode, VibrationMode highFrequencyMode, float duration)
        {
            float lowFrequency = GetModeValue(lowFrequencyMode);
            float highFrequency = GetModeValue(highFrequencyMode);
            GamepadRumble(lowFrequency, highFrequency, duration);
        }
        /// <summary>
        /// Inicia a vibração do gamepad fornecendo valores diretos para baixa e alta frequência.
        /// </summary>
        /// <param name="lowFrequency">Valor da baixa frequência (0.0 a 1.0).</param>
        /// <param name="highFrequency">Valor da alta frequência (0.0 a 1.0).</param>
        /// <param name="duration">Duração da vibração em segundos.</param>
        public static void GamepadRumble(float lowFrequency, float highFrequency, float duration)
        {
            Gamepad pad = Gamepad.current;

            if (pad == null) return;

            pad.SetMotorSpeeds(lowFrequency, highFrequency);
#if EVENTS_PRESENT
            CoroutineRunner.Instance.RunCoroutine(StopRumble(duration, pad));
#else
            GameObject rumblerObject = new GameObject("RumblerCoroutine");
            rumblerObject.AddComponent<RumblerCoroutine>().
            StartCoroutine(StopRumble(duration, pad));
            GameObject.Destroy(rumblerObject, duration+0.1f);
#endif
        }
        /// <summary>
        /// Interrompe a vibração atual do gamepad, pausando as haptics.
        /// </summary>
        public static void StopRumble(){
            Gamepad pad = Gamepad.current;

            if (pad == null) return;

            pad.PauseHaptics();
        }

        private static IEnumerator StopRumble(float duration, Gamepad pad)
        {
            if(duration > 5) duration = 5f;

            yield return new WaitForSeconds(duration);

            pad.SetMotorSpeeds(0f,0f);
        }

        private static float GetModeValue(VibrationMode mode){
            switch(mode){
                case VibrationMode.Minimal:
                    return 0.05f;
                case VibrationMode.VeryLow:
                    return 0.1f;
                case VibrationMode.Low:
                    return 0.25f;
                case VibrationMode.Medium:
                    return 0.5f;
                case VibrationMode.High:
                    return 0.75f;
                case VibrationMode.Max:
                    return 1f;
                default:
                    return 0f;
            }
        }
    }

#if !EVENTS_PRESENT
    /// <summary>
    /// Componente auxiliar usado para executar corrotinas de parada de vibração
    /// quando o sistema de eventos não está presente.
    /// </summary>
    public class RumblerCoroutine : MonoBehaviour
    {
        void Start()
        {
            
        }
    }
#endif
}
