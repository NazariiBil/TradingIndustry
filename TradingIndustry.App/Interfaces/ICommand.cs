
namespace TradingIndustry.App.Interfaces
{
    public interface ICommand
    {
        string Description { get; }
        void Execute();
    }
}