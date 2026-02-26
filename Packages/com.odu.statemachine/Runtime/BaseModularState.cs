using System;

namespace OduLib.Systems.StateMachine {
    /// <summary>
    /// Implementação concreta de um estado que delega sua lógica para dados modulares (ScriptableObject).
    /// Permite reutilizar lógica de estado sem criar novas classes, usando composição.
    /// </summary>
    /// <typeparam name="EState">Tipo enum que define os estados disponíveis.</typeparam>
    /// <typeparam name="Context">Tipo de contexto que será passado aos dados do estado.</typeparam>
    public class BaseModularState<EState, Context> : BaseState<EState, Context>
        where EState : Enum
        where Context : class
    {
        /// <summary>Inicializa um novo estado modular com seus dados.</summary>
        /// <param name="key">Identificador único do estado (enum).</param>
        /// <param name="context">Contexto compartilhado para uso do estado.</param>
        /// <param name="data">Dados modulares que contêm a lógica do estado.</param>
        public BaseModularState(EState key, Context context, BaseModularStateData<EState, Context> data) : base(key, context)
        {
            Data = data;
            Data.Init(this, context);
        }
        
        /// <summary>Dados modulares que contêm a lógica deste estado.</summary>
        protected BaseModularStateData<EState, Context> Data;

        /// <summary>Chamado quando o estado é ativado, delegando para os dados modulares.</summary>
        public override void EnterState()
        {
            Data.EnterState();
        }

        /// <summary>Chamado quando o estado é desativado, delegando para os dados modulares.</summary>
        public override void ExitState()
        {
            Data.ExitState();
        }

        /// <summary>Retorna a próxima chave de estado, delegando para os dados modulares.</summary>
        /// <returns>Chave do próximo estado retornada pelos dados modulares.</returns>
        public override EState GetNextState()
        {
            return Data.GetNextState();
        }

        /// <summary>Atualiza o estado a cada frame, delegando para os dados modulares.</summary>
        public override void UpdateState()
        {
            Data.UpdateState();
        }
    }
}
