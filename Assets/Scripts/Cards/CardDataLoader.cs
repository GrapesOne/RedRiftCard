using System.Threading;
using System.Threading.Tasks;

public interface ICardDataLoader
{
   Task<CardData> LoadCardData(CancellationToken token);
   void Init();
}
