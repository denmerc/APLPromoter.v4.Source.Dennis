using Promoter.Domain;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLPromoter.UI.Wpf.ViewModel
{
    public class PriceRoutineViewModel : ReactiveObject
    {
        public PriceRoutineViewModel(PriceRoutine p)
        {
            Model = p;
            Name = Model.Name;
        }
        private Promoter.Domain.PriceRoutine _Model;
        public Promoter.Domain.PriceRoutine Model
        {
            get
            {
                return _Model;
            }
            set
            {
                if (_Model != value)
                {
                    _Model = value;
                    this.RaiseAndSetIfChanged(ref _Model, value);
                }
            }
        }
        private string _Name;
        public string Name
        {
            get
            {
                return _Model.Name;
            }
            set
            {
                if (_Model.Name != value)
                {
                    _Model.Name = value;
                    this.RaiseAndSetIfChanged(ref _Name, value);
                }
            }
        }
    }
}
