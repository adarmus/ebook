using System.Threading.Tasks;
using System.Windows.Input;

namespace ebook.Async
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
