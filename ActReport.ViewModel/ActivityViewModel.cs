using ActReport.Core.Contracts;
using ActReport.Core.Entities;
using ActReport.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ActReport.ViewModel
{
    public class ActivityViewModel : BaseViewModel
    {
        private Employee _employee;
        private ObservableCollection<Activity> _activities;
        private ICommand _cmdActivity;
        private Activity _selectedActivity;
        private ICommand _cmdDeleteActivity;



        public Activity SelectedActivity
        {
            get => _selectedActivity;
            set
            {
                _selectedActivity = value;
                OnPropertyChanged(nameof(SelectedActivity));
            }
        }
        public ObservableCollection<Activity> Activities
        {
            get => _activities;
            set
            {
                _activities = value;
                OnPropertyChanged(nameof(Activities));
            }
        }
        public string FullName => $"{_employee.FirstName} {_employee.LastName}";
       

        public ActivityViewModel(IController controller, Employee employee) : base(controller)
        {
            _employee = employee;
            using IUnitOfWork uow = new UnitOfWork();
            Activities = new ObservableCollection<Activity>(uow.ActivityRepository.Get(
                filter: x => x.Employee_Id == employee.Id,
                orderBy: coll => coll.OrderBy(activity => activity.Date).ThenBy(activity => activity.StartTime)));

            Activities.CollectionChanged += Activities_CollectionChanged;
        }

        private void Activities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Remove)
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    foreach(var item in e.OldItems)
                    {
                        uow.ActivityRepository.Delete((item as Activity).Id);
                    }
                    uow.Save();
                }
            }
        }

       public ICommand CmdActivity
        {
            get
            {
                if (_cmdActivity == null)
                {
                    _cmdActivity = new RelayCommand(
                       execute: _ => _controller.ShowWindow(new ActivityNewAndEditModel(_controller, SelectedActivity, _employee)),
                       canExecute: _ => true);
                }
                return _cmdActivity;
            }
            
        }
        public ICommand CmdDeleteActivity
        {
            get
            {
                if (_cmdDeleteActivity == null)
                {
                    _cmdDeleteActivity = new RelayCommand(
                       execute: _ =>
                       {
                           using IUnitOfWork uow = new UnitOfWork();

                           uow.ActivityRepository.Delete(SelectedActivity);
                           uow.Save();
                       },
                canExecute: _ => true && _cmdDeleteActivity != null);
                }
                return _cmdDeleteActivity;
            }
        }
        }
    }
 