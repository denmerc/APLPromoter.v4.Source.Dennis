using APLPromoter.UI.Wpf.ViewModel;
using Promoter.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace APLPromoter.UI.Wpf.Views
{
    public class MasterDetailTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AnalyticDetailTemplate { get; set; }
        public DataTemplate PriceRoutineDetailTemplate { get; set; }
        public DataTemplate HomeDetailTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Analytic a = item as Analytic;
            if (a != null)
            {
                return this.AnalyticDetailTemplate;
            }
            PriceRoutine b = item as PriceRoutine;
            if (b != null)
            {
                return this.PriceRoutineDetailTemplate;
            }
            return base.SelectTemplate(item, container);
        }
    }

    public class TreeNodeTemplateSelector : DataTemplateSelector
    {
        public HierarchicalDataTemplate ExplorerRootNodeTemplate { get; set; }
        public HierarchicalDataTemplate ExplorerARootNodeTemplate { get; set; }
        public HierarchicalDataTemplate ExplorerPRootNodeTemplate { get; set; }
        public HierarchicalDataTemplate AnalyticTreeNodeTemplate { get; set; }
        public DataTemplate PriceRoutineTreeNodeTemplate { get; set; }



        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            base.SelectTemplate(item, container);
            if (item is ExplorerARootNode)
            {
                return this.ExplorerARootNodeTemplate;
            }
            if (item is ExplorerPRootNode)
            {
                return this.ExplorerPRootNodeTemplate;
            }
            if (item is Analytic)
            {
                return this.AnalyticTreeNodeTemplate;
            }
            if(item is PriceRoutine)
            {
                return this.PriceRoutineTreeNodeTemplate;
            }
            return null;

        }
    }
}
