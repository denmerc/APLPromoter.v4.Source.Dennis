using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLPromoter.Tests.Server.Services
{
    public enum ModeType { 
        Auto = 33, 
        Manual = 32 
    }

    public enum AnalyticTypes {
        Margin = 34,
        Markup = 35,
        Movement = 36,
        Days_On_Hand = 37,
        In_Stock_Ratio = 62,
        Supplier_performance = 38,
        Trend_Analysis = 63,
        Competitor_Analysis = 64
    }

    public enum AnalyticClientMessages
    {
        User_Identity_session_invalid = 41,
        Analytics_Identity_name_blank = 42,
        Analytics_Identity_name_duplicate = 43,
        Analytics_Identity_name_length = 44,
        Analytics_Identity_description_blank = 55,
        Analytics_Identity_owner_invalid = 56,
        Analytics_Filter_enumeration_invalid = 61,
        Analytics_Type_enumeration_invalid = 66,
        Analytics_Type_mode_invalid = 67,
        Analytics_Type_limit_invalid = 68
    }


    public enum Role
    {
        Default = 1,
        NonAdmin = 2,
        PricingAnalyst = 3,
        Admin = 4
    }
}
