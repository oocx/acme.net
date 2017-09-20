using System.Threading.Tasks;

namespace Oocx.Acme.Console
{
    public interface IAcmeProcess
    {
        Task StartAsync();
    }
}