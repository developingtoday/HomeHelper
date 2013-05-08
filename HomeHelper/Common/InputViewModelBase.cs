using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHelper.Repository.Abstract;

namespace HomeHelper.Common
{
    /// <summary>
    /// Enumeratie ce o folosesc pentru a stabili
    /// starea viewului
    /// </summary>
    public enum InputViewOperatiune
    {
        Adaugare,
        Modificare,
        Stergere,
    }
    public abstract class InputViewModelBase<T>:BindableBase
    {

        /// <summary>
        /// Repositoryul care il folosim pentru lucru
        /// pe obiectul care il trimite ca binding
        /// </summary>
        private IRepository<T> _repository;

        /// <summary>
        /// Obiectul care se trimite in view si apoi il preluam pentru prelucrare
        /// </summary>
        public T ObiectInBinding { get; set; }

        public InputViewModelBase(IRepository<T> repository )
        {
            _repository = repository;
            IsClosed = false;
            RefreshDataSource = false;
        }
        /// <summary>
        /// Operatiunea care este pe view
        /// in functie de ea save command 
        /// se face 
        /// </summary>
        public InputViewOperatiune Operatiune { get; set; }
        public bool ShowDelete
        {
            get { return Operatiune != InputViewOperatiune.Adaugare; }
        }
        private RelayCommand _saveCommand;
        private RelayCommand _cancelCommand;
        private RelayCommand _deleteCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(o=>Save());
                }
                return _saveCommand;
            }
            
        }

        
        public RelayCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand=new RelayCommand(o=>Cancel());
                }
                return _cancelCommand;
            }
        }

       

        public RelayCommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand=new RelayCommand(o=>Delete());
                }
                return _deleteCommand;
            }
        }


        private void Save()
        {
            switch (Operatiune)
            {
                case InputViewOperatiune.Adaugare:
                    _repository.CreateOrUpdate(ObiectInBinding);
                    break;
                case InputViewOperatiune.Modificare:
                    _repository.CreateOrUpdate(ObiectInBinding);
                    break;
            }
            IsClosed = true;
            RefreshDataSource = true;
        }

        private void Cancel()
        {
            IsClosed = true;
            RefreshDataSource = false;
        }


        private void Delete()
        {
            _repository.Delete(ObiectInBinding);
            IsClosed = true;
            RefreshDataSource = true;
        }


        private bool _isClosed;

        /// <summary>
        /// Notificarea ca un IsClosed s-a schimbat
        /// </summary>
        public event EventHandler IsClosedChanged;

        /// <summary>
        /// Il folosesc ca un flag pentru a inchide containeru ce contine viewul
        /// luat ca datacontext cand creez datacontext
        /// </summary>
        public bool IsClosed
        {
            get { return _isClosed; } 

            private set
            {
                _isClosed = value;
                if (IsClosedChanged != null)
                {
                    IsClosedChanged(this, new EventArgs());
                }
            }
        }

        public bool RefreshDataSource { get; private set; }


    }
}
