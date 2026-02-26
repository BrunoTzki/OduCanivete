using System;
using System.Collections.Generic;
using UnityEngine;

namespace OduLib.Systems.StateMachine {
    /// <summary>
    /// Gerenciador genérico de máquina de estados que controla transições e atualizações de estado.
    /// </summary>
    /// <typeparam name="EState">Tipo enum que define os estados disponíveis.</typeparam>
    /// <typeparam name="Context">Tipo de contexto que será passado aos estados.</typeparam>
    public abstract class StateManager<EState,Context> : MonoBehaviour 
        where EState : Enum
        where Context : class
    {
        /// <summary>Ação invocada quando há uma transição de estado.</summary>
        public Action<EState> OnStateChanged;
        /// <summary>
        /// Dicionário que armazena todos os estados disponíveis mapeados por sua chave enum.
        /// </summary>
        protected Dictionary<EState, BaseState<EState,Context>> States = new Dictionary<EState, BaseState<EState,Context>>();
        /// <summary>Estado atual do gerenciador.</summary>
        protected BaseState<EState,Context> CurrentState;

        /// <summary>Indica se está em processo de transição entre estados.</summary>
        protected bool IsTransitioningState = false;

        /// <summary>Inicializa o estado atual ao iniciar o script.</summary>
        protected void Start(){
            CurrentState.EnterState();
            OnStateChanged?.Invoke(CurrentState.StateKey);
        }

        /// <summary>
        /// Atualiza o estado atual ou realiza transição para o próximo estado.
        /// </summary>
        protected void Update(){
            EState nextStateKey = CurrentState.GetNextState();

            if(!IsTransitioningState && nextStateKey.Equals(CurrentState.StateKey)){
                CurrentState.UpdateState();
            } else if (!IsTransitioningState) {
                TransitionToState(nextStateKey);
            }
        }

        /// <summary>Realiza a transição para um novo estado.</summary>
        /// <param name="stateKey">Chave do estado de destino.</param>
        public void TransitionToState(EState stateKey){
            IsTransitioningState = true;

            CurrentState.ExitState();
            
            EState lastStateKey = CurrentState.StateKey;
            CurrentState = States[stateKey];
            CurrentState.LastStateKey = lastStateKey; 

            CurrentState.EnterState();
            OnStateChanged?.Invoke(CurrentState.StateKey);

            IsTransitioningState = false;
        }
        
    }
}
