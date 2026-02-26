using System;

namespace OduLib.Systems.StateMachine {
    /// <summary>
    /// Classe base abstrata que define a estrutura de um estado na máquina de estados.
    /// </summary>
    /// <typeparam name="EState">Tipo enum que define os estados disponíveis.</typeparam>
    /// <typeparam name="Context">Tipo de contexto que será acessível aos estados.</typeparam>
    public abstract class BaseState<EState, Context>
        where EState : Enum
        where Context : class
    {
        /// <summary>Inicializa um novo estado com sua chave e contexto.</summary>
        /// <param name="key">Identificador único do estado (enum).</param>
        /// <param name="context">Contexto compartilhado para uso do estado.</param>
        public BaseState(EState key, Context context){
            StateKey = key;
            Ctx = context;
        }

        /// <summary>Chave identificadora deste estado.</summary>
        public EState StateKey {get; private set;}
        /// <summary>Chave do estado anterior.</summary>
        public EState LastStateKey {get; set;}
        /// <summary>Contexto compartilhado entre todos os estados.</summary>
        protected Context Ctx {get; private set;}

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
