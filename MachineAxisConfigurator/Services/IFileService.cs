namespace MachineAxisConfigurator.Services
{
    public interface IFileService
    {
        T DeserializeXml<T>(string filePath) where T : class;
        string GetFilePath();
        void SerializeXml<T>(T data, string filePath) where T : class;
    }
}