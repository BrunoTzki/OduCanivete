using System;
using UnityEngine;

namespace OduLib.Systems.StateMachine {
    /// <summary>
    /// Classe base abstrata para dados modulares de estado armazenados como ScriptableObject.
    /// Permite separar a lógica do estado de sua implementação através do padrão de composição.
    /// </summary>
    /// <typeparam name="EState">Tipo enum que define os estados disponíveis.</typeparam>
    /// <typeparam name="Context">Tipo de contexto que será acessível aos dados do estado.</typeparam>
    public abstract class BaseModularStateData<EState, Context> : ScriptableObject 
        where EState : Enum
        where Context : class
    {
        /// <summary>Estado pai que executa as funções de lógica do estado.</summary>
        protected BaseState<EState, Context> SuperState;
        /// <summary>Contexto compartilhado entre todos os estados.</summary>
        protected Context Ctx {get; private set;}

        /// <summary>Fução para inicializar os dados do estado.</summary>
        /// <param name="super">Estado pai que executa as funções de lógica do estado.</param>
        /// <param name="context">Contexto compartilhado entre todos os estados.</param>
        public virtual void Init(BaseState<EState, Context> super, Context context){
            SuperState = super;
            Ctx = context;
        }

        /// <summary>Chamado quando o estado é ativado.</summary>
        public abstract void EnterState();
        /// <summary>Chamado quando o estado é desativado.</summary>
        public abstract void ExitState();
        /// <summary>Chamado a cada frame enquanto o estado está ativo.</summary>
        public abstract void UpdateState();
        /// <summary>Retorna a próxima chave de estado para transição.</summary>
        /// <returns>Chave do próximo estado ou a chave atual se não houver transição.</returns>
        public abstract EState GetNextState();
    }
}