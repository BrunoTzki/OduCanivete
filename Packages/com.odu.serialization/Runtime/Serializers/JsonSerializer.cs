using UnityEngine;

namespace OduLib.Canivete.Serialization {
    /// <summary>
    /// Serializador JSON que implementa a interface ISerializer.
    /// Fornece suporte para serialização e desserialização de objetos usando JsonUtility do Unity.
    /// Opcionalmente, pode criptografar os dados usando XOR com uma chave pré-definida.
    /// </summary>
    public class JsonSerializer : ISerializer
    {
        /// <summary>
        /// Indica se os dados devem ser criptografados ao serializar e desserializar.
        /// </summary>
        private bool _useEncryption = false;
        
        /// <summary>
        /// A chave de criptografia usada para operações XOR de criptografia/descriptografia.
        /// </summary>
        private readonly string _encryptionCodeWord = "P,A*M+0U@,G5U-m-=h/nS}+Y@3Ln$}{JP.z1dcg:JPCu3#GxMM";

        /// <summary>
        /// Inicializa uma nova instância do JsonSerializer.
        /// </summary>
        /// <param name="encryptionCodeWord">A chave de criptografia a ser usada. Se for uma string vazia, será usada a chave padrão. (não recomendado)</param>
        /// <param name="useEncryption">Indica se os dados devem ser criptografados ao serializar.</param>
        public JsonSerializer(bool useEncryption = false, string encryptionCodeWord = "")
        {
            _useEncryption = useEncryption;
            if(encryptionCodeWord != ""){
                _encryptionCodeWord = encryptionCodeWord;
            }
        }

        public T Deserialize<T>(string data)
        {
            if(_useEncryption)
            {
                data = EncryptDecrypt(data);
            }
            return JsonUtility.FromJson<T>(data);
        }

        public string Serialize<T>(T obj)
        {
            string dataToStore = JsonUtility.ToJson(obj, true);

            if(_useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            return dataToStore;
        }

        /// <summary>
        /// Criptografa ou descriptografa uma string usando a chave de criptografia padrão.
        /// Utiliza uma operação XOR para modificar os caracteres da string com base na chave de criptografia.
        /// </summary>
        /// <param name="data">Os dados a serem criptografados ou descriptografados.</param>
        /// <returns>Os dados após a operação de criptografia/descriptografia.</returns>
        private string EncryptDecrypt(string data){
            string modifiedData = "";
            for(int i = 0; i < data.Length; i++){
                modifiedData += (char) (data[i] ^ _encryptionCodeWord[i % _encryptionCodeWord.Length]);
            }
            return modifiedData;
        }
    }
}
