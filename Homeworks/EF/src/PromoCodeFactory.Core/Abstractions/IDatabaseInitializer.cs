using System.Threading.Tasks;

namespace PromoCodeFactory.Core.Abstractions
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}
