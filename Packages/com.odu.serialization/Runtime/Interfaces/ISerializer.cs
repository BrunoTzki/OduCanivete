namespace OduLib.Canivete.Serialization {
    /// <summary>
    /// Interface que define o contrato para serializadores genéricos.
    /// Implementadores devem fornecer métodos para serializar e desserializar objetos.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Serializa um objeto de tipo genérico em uma string.
        /// </summary>
        /// <typeparam name="T">O tipo do objeto a ser serializado.</typeparam>
        /// <param name="obj">O objeto a ser serializado.</param>
        /// <returns>Uma string contendo os dados serializados.</returns>
        string Serialize<T>(T obj);
        
        /// <summary>
        /// Desserializa uma string em um objeto de tipo genérico.
        /// </summary>
        /// <typeparam name="T">O tipo do objeto a ser retornado.</typeparam>
        /// <param name="data">A string contendo os dados serializados.</param>
        /// <returns>Um objeto desserializado do tipo T.</returns>
        T Deserialize<T>(string data);
    }
}
