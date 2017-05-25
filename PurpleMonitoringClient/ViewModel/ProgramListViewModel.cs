using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMonitoringClient.ViewModel
{
    using Model.Executing;
    using PurpleMonitoringClient.Networking;
    using System.Net.Http;
    using Windows.UI.Core;
    using Windows.UI.Popups;

    public class ProgramListViewModel
    {
        public ObservableCollection<ProgramViewModel> Programs = new ObservableCollection<ProgramViewModel>();

        public string Host { get; set; }

        CoreDispatcher dispatcher;

        public ProgramListViewModel(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        async public void FetchPrograms()
        {
            try
            {
                var programs = await Service.Instance.GetAvailablePrograms(Host);
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    Programs.Clear();
                    foreach (var p in programs)
                        Programs.Add(new ProgramViewModel(p));
                });
            } 
            catch (UriFormatException)
            {
                await new MessageDialog("Неверный формат URL", "Ошибка")
                    .ShowAsync();
            }
            catch (HttpRequestException e)
            {
                await new MessageDialog("Не удалось получить список программ\n" + e.Message, "Ошибка")
                    .ShowAsync();
            }
            catch (Exception e)
            {
                await new MessageDialog(e.Message, "Ошибка")
                    .ShowAsync();
            }
        }

    }
}
