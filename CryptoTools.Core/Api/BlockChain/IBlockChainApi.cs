using System.Threading.Tasks;
using CryptoTools.Core.Api.BlockChain.Models;
using Refit;

namespace CryptoTools.Core.Api.BlockChain
{
    public interface IBlockChainApi
    {
        [Get("/en/ticker")]
        Task<Prices> GetPrices();
    }
}
