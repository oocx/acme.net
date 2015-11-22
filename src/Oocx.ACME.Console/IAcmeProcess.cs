using System.Threading.Tasks;

namespace Oocx.ACME.Console
{
    public interface IAcmeProcess
    {
        Task StartAsync();
    }
}