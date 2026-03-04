using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace OduLib.Canivete.Serialization {
    /// <summary>
    /// Classe de parâmetros para configurar o serviço de dados de arquivo. Permite definir o serializador, diretório de dados, nome do arquivo, extensão do arquivo e extensão de backup.
    /// </summary>
    public class FileDataServiceParams
    {
        /// <summary>
        /// O objeto de serialização a ser usado. Por padrão, é usado o <see cref="JsonSerializer"/> sem criptografia.
        /// </summary>
        public ISerializer Serializer = new JsonSerializer();
        /// <summary>
        /// O caminho do diretório onde os dados serão armazenados. Por padrão, é usado <see cref="Application.persistentDataPath"/>.
         /// </summary>
        public string DataDirPath = Application.persistentDataPath;
        /// <summary>
        /// O nome do arquivo dos dados (sem extensão).
        /// </summary>
        public string DataFileName = "";
        /// <summary>
        /// A extensão do arquivo de dados. Por padrão, é usado ".json".
        /// </summary>
        public string DataFileExtension = ".json";
        /// <summary>
        /// A extensão usada para arquivos de backup. Por padrão, é usado ".bak".
        /// </summary>
        public string BackupExtension = ".bak";
    }
    /// <summary>
    /// Serviço genérico para serialização e deserialização de dados em arquivos de um projeto Unity.<br/>
    /// Oferece funcionalidades de salvar, carregar e deletar dados com suporte a criptografia e backup automático.
    /// </summary>
    /// <typeparam name="TData">O tipo de dados a ser serializado. Deve ser uma classe.</typeparam>
    public class FileDataService<TData> : IDataService<TData> where TData : class
    {
        private ISerializer _serializer;
        private string _dataDirPath;
        private string _dataFileName;
        
        private string _dataFileExtension;
        private readonly string _backupExtension;

#region Construtores
        /// <summary>
        /// Inicializa uma nova instância do serviço de dados de arquivo.
        /// </summary>
        /// <param name="serializer">O objeto de serialização a ser usado.</param>
        /// <param name="dataDirPath">O caminho do diretório onde os dados serão armazenados.</param>
        /// <param name="dataFileName">O nome do arquivo de dados (sem extensão).</param>
        /// <param name="dataFileExtension">A extensão do arquivo de dados (ex: .json).</param>
        /// <param name="backupExtension">A extensão usada para arquivos de backup.</param>
        public FileDataService(ISerializer serializer, string dataDirPath, string dataFileName, string dataFileExtension,  string backupExtension){
            _serializer = serializer;
            _dataDirPath = dataDirPath;
            _dataFileName = dataFileName;
            _dataFileExtension = dataFileExtension;
            _backupExtension = backupExtension;
        }
        /// <summary>
        /// Inicializa uma nova instância do serviço de dados de arquivo.
        /// </summary>
        /// <param name="parameters">Um objeto <see cref="FileDataServiceParams"/> contendo os parâmetros de configuração para o serviço de dados de arquivo.</param>
        public FileDataService(FileDataServiceParams parameters) : this(parameters.Serializer, parameters.DataDirPath, parameters.DataFileName, parameters.DataFileExtension, parameters.BackupExtension)
        {
        }
#endregion

        /// <summary>
        /// Obtém o caminho completo do arquivo de dados para um perfil específico.
        /// </summary>
        /// <param name="saveID">O identificador do perfil.</param>
        /// <param name="fileName">O nome do arquivo.</param>
        /// <returns>O caminho completo do arquivo.</returns>
        public string GetPathToFile(string saveID, string fileName){
            return Path.Combine(_dataDirPath, saveID, string.Concat(fileName,_dataFileExtension));
        }

        public TData Load(string saveID, bool allowRestoreFromBackup = true){
            if(saveID == null){
                return null;
            }

            string fullPath = GetPathToFile(saveID, _dataFileName);

            TData loadedData = null;

            if(File.Exists(fullPath)){
                try 
                {
                    string dataToLoad = ReadFromFile(fullPath);

                    loadedData = _serializer.Deserialize<TData>(dataToLoad);
                }
                catch (Exception e) 
                {
                    if(allowRestoreFromBackup){
                        Debug.LogWarning("Carregamento de dados falhou. Tentando retornar para um arquivo de backup. \n" + e);
                        bool rollbackSuccess = AttemptRollBack(fullPath);
                        if(rollbackSuccess){
                            loadedData = Load(saveID, false);
                        }
                        
                    } else {
                        Debug.LogError("Erro ocorreu ao tentar carregar dados do arquivo: " + fullPath + " e backup não funcionou.\n" + e);
                    }
                    
                }
            }
            return loadedData;
        }

        /// <summary>
        /// Lê o conteúdo de um arquivo como string.
        /// </summary>
        /// <param name="fullPath">O caminho completo do arquivo a ser lido.</param>
        /// <returns>O conteúdo do arquivo como string.</returns>
        string ReadFromFile(string fullPath){
            string dataToLoad = "";
            using (FileStream stream = new FileStream(fullPath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    dataToLoad = reader.ReadToEnd();
                }
            }
            return dataToLoad;
        }

        public bool Save(string saveID, TData data){
            if(saveID == null){
                return false;
            }

            string fullPath = GetPathToFile(saveID, _dataFileName);
            string backupFilePath = string.Concat(fullPath,_backupExtension);

            try 
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                string dataToStore = _serializer.Serialize(data);

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream)) 
                    {
                        writer.Write(dataToStore);
                    }
                }

                //Verificar se save é válido
                TData verifiedData = Load(saveID);
                if(verifiedData != null){
                    File.Copy(fullPath, backupFilePath, true);
                } else {
                    throw new FileLoadException("Arquivo de dados não pôde ser verificado e backup não pôde ser criado.");
                }

                return true;
            }
            catch (Exception e) 
            {
                Debug.LogError("Erro ocorreu ao tentar salvar dados no arquivo: " + fullPath + "\n" + e);
                return false;
            }
        }

        public void Delete(string saveID){
            if (saveID == null){
                return;
            }

            string fullPath = GetPathToFile(saveID, _dataFileName);
            try 
            {
                if (File.Exists(fullPath)) 
                {
                    Directory.Delete(Path.GetDirectoryName(fullPath), true);
                }
                else 
                {
                    Debug.LogWarning("Tentou deletar dados, mas nada foi encontrado no diretório: " + fullPath);
                }
            }
            catch (Exception e) 
            {
                Debug.LogError("Falhou em deletar dados do perfil para ProfileID: " 
                    + saveID + " no diretório: " + fullPath + "\n" + e);
            }
        }

        public Dictionary<string, TData> ListSaves(){
            Dictionary<string, TData> profileDictionary = new Dictionary<string, TData>();

            if (!Directory.Exists(_dataDirPath))
            {
                Debug.LogWarning($"Diretório {_dataDirPath} ainda não existe. Impossível carregar perfis.");
                return profileDictionary;
            }
            
            IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(_dataDirPath).EnumerateDirectories();
            foreach (DirectoryInfo dirInfo in dirInfos) 
            {
                string saveID = dirInfo.Name;

                string fullPath = GetPathToFile(saveID, _dataFileName);
                if (!File.Exists(fullPath))
                {
                    Debug.LogWarning("Ignorando diretório ao carregar todos os perfis pois este não contém dados: "
                        + saveID);
                    continue;
                }

                TData profileData = Load(saveID);
                if (profileData != null) 
                {
                    profileDictionary.Add(saveID, profileData);
                }
                else 
                {
                    Debug.LogError("Tentou carregar o perfil mas algo deu errado. ProfileId: " + saveID);
                }
            }

            return profileDictionary;
        }

        /// <summary>
        /// Tenta restaurar o arquivo de dados a partir do arquivo de backup.
        /// </summary>
        /// <param name="fullPath">O caminho completo do arquivo de dados a ser restaurado.</param>
        /// <returns>Verdadeiro se a restauração foi bem-sucedida, falso caso contrário.</returns>
        private bool AttemptRollBack(string fullPath){
            bool success = false;
            string backupFilePath = string.Concat(fullPath,_backupExtension);

            try {
                if(File.Exists(backupFilePath)){
                    File.Copy(backupFilePath, fullPath, true);
                    success = true;
                    Debug.LogWarning("Foi necessário retornar para um arquivo de backup em: " + backupFilePath);
                }
                else{
                    throw new Exception("Não existe arquivo de backup para retornar.");
                }
            } 
            catch (Exception e) {
                Debug.LogError("Ocorreu um erro ao tentar retornar ao arquivo de backup em: " + backupFilePath + "\n" + e);
            }

            return success;
        }
    }
}
