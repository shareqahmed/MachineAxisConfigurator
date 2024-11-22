using MachineAxisConfigurator.Models;

namespace MachineAxisConfigurator.Services
{
    public interface IFileService
    {
        MachineSettings LoadXml();
    }
}