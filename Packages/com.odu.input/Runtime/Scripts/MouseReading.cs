using UnityEngine;
using UnityEngine.InputSystem;

namespace OduLib.Canivete.Input {
    /// <summary>
    /// Utilitários relacionados à leitura do cursor do mouse.
    /// </summary>
    public static class MouseReading
    {
        /// <summary>
        /// Retorna um <see cref="UnityEngine.Ray"/> baseado na posição atual do cursor na tela,
        /// projetado pela câmera principal (`Camera.main`).
        /// </summary>
        /// <returns>Um <see cref="UnityEngine.Ray"/> que representa a posição e direção do clique/ponteiro.</returns>
        public static Ray MousePositionRay(){
            Vector3 mousePosition = Mouse.current.position.ReadValue();

            return Camera.main.ScreenPointToRay(mousePosition);
        }
    }
}
