using ActReport.Core.Contracts;
using ActReport.Core.Entities;
using ActReport.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ActReport.ViewModel
{
    public class ActivityNewAndEditModel : BaseViewModel
    {
        private Activity _activity;
        private DateTime _date;
        private DateTime _starttime;
        private DateTime _endtime;
        private string _activitytext;
        private Boolean insert;
        private Employee _employeeId;


        public Activity Activity
        {
            get => _activity;
            set
            {
                _activity = value;
                OnPropertyChanged(nameof(Activity));
            }
        }
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }
        public DateTime StartTime
        {
            get => _starttime;
            set
            {
                _starttime = value;
                OnPropertyChanged(nameof(StartTime));
            }
        }
        public DateTime EndTime
        {
            get => _endtime;
            set
            {
                _endtime = value;
                OnPropertyChanged(nameof(EndTime));
            }
        }

        public string ActivityText
        {
            get => _activitytext;
            set
            {
                _activitytext = value;
                OnPropertyChanged(nameof(ActivityText));
            }
        }
       

        public ActivityNewAndEditModel(IController controller, Activity activity, Employee employee) : base(controller)
        {
           
            if(activity == null)
            {
                insert = true;
                _activity = new Activity()
                {
                    Employee_Id = employee.Id
                };
                _date = DateTime.Today;
                _starttime = DateTime.Now;
                _endtime = DateTime.Now;
                _activitytext = null;
            }
            else if (activity != null)
            {
                _activity = activity;
                insert = false;
                _date = activity.Date;
                _starttime  = activity.StartTime;
                _endtime = activity.EndTime;
                _activitytext = activity.ActivityText;
            }
        }

        private ICommand _cmdActivites;

        public ICommand CmdNewAndEditActivites
        {
            get
            {
                if (_cmdActivites == null && insert == true)
                {
                    _cmdActivites = new RelayCommand(
                      execute: _ =>
                      {
                          using IUnitOfWork uow = new UnitOfWork();
                          _activity.Date = Date;
                          _activity.StartTime = StartTime;
                          _activity.EndTime = EndTime;
                          _activity.ActivityText = ActivityText;
                          uow.ActivityRepository.Insert(_activity);
                          uow.Save();
                          _controller.CloseWindow(this);

                      },
                      canExecute: _ => _activity != null);
                }
                else if (_cmdActivites == null && insert == false)
                {
                    _cmdActivites = new RelayCommand(
                     execute: _ =>
                     {
                         using IUnitOfWork uow = new UnitOfWork();
                         _activity.Date = Date;
                         _activity.StartTime = StartTime;
                         _activity.EndTime = EndTime;
                         _activity.ActivityText = ActivityText;
                         uow.ActivityRepository.Update(_activity);
                         uow.Save();
                         _controller.CloseWindow(this);


                     },
                     canExecute: _ => _activity != null);
                }
                return _cmdActivites;
            }
            set { _cmdActivites = value; }
        }

        private ICommand _cmdCancel;

        public ICommand CmdCancel
        {
            get
            {
                if (_cmdCancel == null )
                {
                    _cmdCancel = new RelayCommand(
                      execute: _ =>
                      {
                        
                          _controller.CloseWindow(this);

                      },
                      canExecute: _ => true);
            
                }
                return _cmdCancel;
            }
        
        }

    }
}
