using APLPromoter.Client.Entity;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APLPromoter.Client.ViewModels
{
    public class ViewModelBase : ObjectBase
    {
        List<ObjectBase> _Models;

        protected virtual void OnViewLoaded(){}

        protected  void UsingProxy<T>(T proxy, Action<T> action){
            action.Invoke(proxy);
            IDisposable disposableProxy = proxy as IDisposable;
            if(disposableProxy != null){
                disposableProxy.Dispose();
            }

        }

        public object ViewLoaded
        {
            get
            {
                OnViewLoaded();
                return null;
            }
        }

        protected virtual void AddModels(List<ObjectBase> models) { }


        protected void ValidateModel()
        {
            if (_Models == null)
            {
                _Models = new List<ObjectBase>();
                AddModels(_Models);
            }

            _ValidationErrors = new List<ValidationFailure>();

            if(_Models.Count > 0)
            {
                foreach (ObjectBase modelObject in _Models)
                {
                    if (modelObject != null)
                    {
                        modelObject.Validate();
                        _ValidationErrors = _ValidationErrors.Union(modelObject.ValidationErrors).ToList();
                    }
                }
                OnPropertyChanged(() => ValidationErrors, false);
            }
        }


    }

    
}
