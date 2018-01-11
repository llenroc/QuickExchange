using System.Threading.Tasks;

namespace CryptoTools.Core.Interfaces
{
    public interface ITradeManager
    {
        /// <summary>
        /// Queries the persistence layer for open trades and 
        /// handles them, otherwise a new trade is created.
        /// </summary>
        /// <returns></returns>
        Task Process();
    }
}