using System.Collections.Generic;

namespace OduLib.Canivete.Serialization {
    /// <summary>
    /// Interface genérica que define o contrato para um serviço de persistência de dados.
    /// Implementadores devem fornecer operações básicas de CRUD (Create, Read, Update, Delete) para dados.
    /// </summary>
    /// <typeparam name="TData">O tipo de dado a ser persistido. Deve ser uma classe.</typeparam>
    public interface IDataService<TData> where TData : class 
    {
        /// <summary>
        /// Salva dados com um identificador único em armazenamento persistente.
        /// </summary>
        /// <param name="saveID">O identificador único para os dados a serem salvos.</param>
        /// <param name="data">Os dados a serem salvos.</param>
        /// <returns>True se o salvamento foi bem-sucedido; caso contrário, False.</returns>
        bool Save(string saveID, TData data);
        
        /// <summary>
        /// Carrega dados previamente salvos usando seu identificador único.
        /// </summary>
        /// <param name="saveID">O identificador único dos dados a serem carregados.</param>
        /// <param name="allowRestoreFromBackup">Se True, permite restaurar dados de um backup quando o arquivo principal está corrompido.</param>
        /// <returns>Os dados carregados do tipo TData, ou null se não forem encontrados.</returns>
        TData Load(string saveID, bool allowRestoreFromBackup = true);
        
        /// <summary>
        /// Deleta dados salvos através de seu identificador único.
        /// </summary>
        /// <param name="saveID">O identificador único dos dados a serem deletados.</param>
        void Delete(string saveID);
        
        /// <summary>
        /// Lista todos os dados salvos, retornando um dicionário com seus identificadores.
        /// </summary>
        /// <returns>Um dicionário onde as chaves são os identificadores únicos e os valores são os dados salvos.</returns>
        Dictionary<string, TData> ListSaves();
    }
}
