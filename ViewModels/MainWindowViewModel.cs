﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MVVMTaste.Infrastructure.Commands;
using MVVMTaste.Models;
using MVVMTaste.Models.Decanat;
using MVVMTaste.ViewModels.Base;

namespace MVVMTaste.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        /*------------------------------------------------------------------------------*/

        #region Массив разнородных данных и свойство выбранного объекта из этого массива
        
        /// <summary>Массив разнородных объектов</summary>
        public object[] CompositeCollection { get; }

        /// <summary>Выбранный объект</summary>
        private object _SelectedCompositeValue;

        /// <summary>Выбранный объект</summary>
        public object SelectedCompositeValue
        {
            get => _SelectedCompositeValue;
            set => Set(ref _SelectedCompositeValue, value);
        }

        #endregion

        #region Коллекция Групп

        public ObservableCollection<Group> Groups { get; }

        #endregion

        #region Выбранная группа (для DataGrid)

        private Group _SelectedGroup;
        public Group SelectedGroup
        {
            get => _SelectedGroup;
            set => Set(ref _SelectedGroup, value);
        }

        #endregion

        #region Индекс выбранного tabItem'a

        private int _SelectedPageIndex;
        public int SelectedPageIndex
        {
            get => _SelectedPageIndex;
            set => Set(ref _SelectedPageIndex, value);
        }

        #endregion

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

        /*------------------------------------------------------------------------------*/

        #region Комманды

        #region CloseApplicationCommand

        public ICommand CloseApplicationCommand { get; }

        private bool CanCloseApplicationCommandExecute(object p) => true;

        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region ChangeTabIndexCommand

        public ICommand ChangeTabIndexCommand { get; }
        private bool CanChangeTabIndexCommandExecute(object d) => true;// _SelectedPageIndex >= 0;
        private void OnChangeTabIndexCommandExecuted(object d)
        {
            if (d is null) return;
            int j = Convert.ToInt32(d);
            if (j == -1 && SelectedPageIndex == 0) return;
            if (j == 1 && SelectedPageIndex == 6) return;
            SelectedPageIndex += j;
        }

        #endregion

        #region CreateGroupCommand

        public ICommand CreateGroupCommand { get; }
        private bool CanCreateGroupCommandExecute(object p) => true;
        private void OnCreateGroupCommandExecuted(object p)
        {
            var group_max_index = Groups.Count > 0 ? Groups.Select(x => int.Parse(x.Name.Remove(0, 7))).Max() : 0;
            var index = 1;
            var new_group = new Group
            {
                Name = $"Группа {group_max_index + 1}",
                Students = new ObservableCollection<Student>(
                    Enumerable.Range(1,10).Select(
                        x => new Student { 
                            Surname = $"Surname {group_max_index * 10 + index}",
                            Name = $"Name {group_max_index * 10 + index}",
                            Patronymic = $"Patronymic {group_max_index * 10 + index++}",
                            Rating = 0,
                            Birthday = DateTime.Now
                        }
                    )
                )
            };
            Groups.Add(new_group);
        }

        #endregion

        #region DeleteGroupCommand

        public ICommand DeleteGroupCommand { get; }
        private bool CanDeleteGroupCommandExecute(object p) => p is Group group && Groups.Contains(group);
        private void OnDeleteGroupCommandExecuted(object p)
        {
            if (!(p is Group group)) return;
            var group_index = Groups.IndexOf(group);
            Groups.Remove(group);
            if (group_index < Groups.Count)
                SelectedGroup = Groups[group_index];
        }

        #endregion

        #endregion

        /*------------------------------------------------------------------------------*/

        public MainWindowViewModel()
        {
            #region Команды

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
            ChangeTabIndexCommand = new LambdaCommand(OnChangeTabIndexCommandExecuted, CanChangeTabIndexCommandExecute);
            CreateGroupCommand = new LambdaCommand(OnCreateGroupCommandExecuted, CanCreateGroupCommandExecute);
            DeleteGroupCommand = new LambdaCommand(OnDeleteGroupCommandExecuted, CanDeleteGroupCommandExecute);

            #endregion

            #region Генерация тестовых данных для графика

            var data_points = new List<DataPoint>((int)(360 / 0.1));
            for (var x = 0d; x <= 360; x += 0.1)
            {
                const double to_rad = Math.PI / 180;
                var y = Math.Sin(x * to_rad);

                data_points.Add(new DataPoint() { XValue = x, YValue = y });
            }

            TestDataPoint = data_points;

            #endregion

            #region Генерация тестовых данных для DataGrid

            var student_index = 1;
            var students = Enumerable.Range(1, 10).Select(i => new Student //Генерируется команда?
            {
                Name = $"Name {student_index}",
                Surname = $"Surname {student_index}",
                Patronymic = $"Patronymic {student_index++}",
                Birthday = DateTime.Now,
                Rating = 0
            });

            var groups = Enumerable.Range(1, 20).Select(i => new Group
            {
               Name = $"Группа {i}",
               Students = new ObservableCollection<Student>(students)
            });

            Groups = new ObservableCollection<Group>(groups);

            #endregion

            #region Заполнение массива разнородными данными

            var data_list = new List<object>();

            data_list.Add("Hello World!");
            data_list.Add(5);
            var group = Groups[1];
            data_list.Add(group);
            data_list.Add(group.Students.ToList()[0]);

            CompositeCollection = data_list.ToArray();

            #endregion
        }

        /*------------------------------------------------------------------------------*/
    }
}
