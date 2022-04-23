using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MVVMTaste.Infrastructure.Commands;
using MVVMTaste.Models;
using MVVMTaste.ViewModels.Base;

namespace MVVMTaste.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region Тестовый набор данных для визуализации графиков

        /// <summary>Тестовый набор данных для визуализации графиков</summary>
        private IEnumerable<DataPoint> _TestDataPoint;

        /// <summary>Тестовый набор данных для визуализации графиков</summary>
        public IEnumerable<DataPoint> TestDataPoint
        {
            get => _TestDataPoint;
            set => Set(ref _TestDataPoint, value);
        }

        #endregion

        #region Заголовок окна

        private string _Title = "Анализ статистики MVVMTaste";

        /// <summary>Заголовок окна</summary>
        public string Title
        {
            get => _Title;
            //set
            //{
            //    //if (Equals(_Title, value)) return;
            //    //_Title = value;
            //    //OnPropertyChanged();
            //    Set(ref _Title, value);
            //}
            set => Set(ref _Title, value);
        }

        #endregion

        #region Status : string - Статус программы

        /// <summary>Статус программы</summary>
        private string _Status = "Готов!";

        ///<summary>Статус программы<summary>
        public string Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }

        #endregion

        #region Комманды

        #region CloseApplicationCommand

        public ICommand CloseApplicationCommand { get; }

        private bool CanCloseApplicationCommandExecute(object p) => true;

        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #endregion

        public MainWindowViewModel()
        {
            #region Команды

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);

            #endregion

            var data_points = new List<DataPoint>((int)(360 / 0.1));
            for (var x = 0d; x <= 360; x += 0.1)
            {
                const double to_rad = Math.PI / 180;
                var y = Math.Sin(x * to_rad);

                data_points.Add(new DataPoint() { XValue = x, YValue = y });
            }

            TestDataPoint = data_points;
        }
    }
}
