using APLPromoter.Client.Entity;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace APLPromoter.Client.Entity
{
    [DataContract]
    public class Analytic : ObjectBase
    {
        [DataMember]
        public List<Type> Types { get; set; }
        [DataMember]
        public List<Filter> Filters { get; set; }
        [DataMember]
        public Identity Self { get; set; }
        
        //[DataContract]
        public class Identity: ObjectBase
        {
            private Int32 _id;
            private string _name;
            private string _description;

            //[DataMember]
            public Int32 Id
            {
                get
                {
                    return _id;
                }
                set
                {
                    _id = value;
                    OnPropertyChanged(() => Id);
                }
            }

            //[DataMember]
            public string Name
            {
                get
                {
                    return _name;
                }
                set
                {
                    _name = value;
                    OnPropertyChanged(() => Name);
                }
            }

            //[DataMember]
            public string Description
            {
                get
                {
                    return _description;
                }
                set
                {
                    _description = value;
                    OnPropertyChanged(() => Description);
                }
            }

            class IdentityValidator : AbstractValidator<Analytic.Identity>
            {
                public IdentityValidator()
                {
                    RuleFor(o => o.Name).NotEmpty();
                    RuleFor(o => o.Description).NotEmpty();
                    RuleFor(o => o.Name).Length(2,150);
                    RuleFor(o => o.Description).Length(2,150);
                }
            }

            protected override IValidator GetValidator()
            {
                return new IdentityValidator();
            }
        }

        [DataContract]
        public class Type
        {
            [DataMember]
            public Int32 Id { get; set; }
            [DataMember]
            public Int32 Mode { get; set; }
            [DataMember]
            public Boolean Included { get; set; }
            [DataMember]
            public List<Group> Groups { get; set; }

            [DataContract]
            public class Group
            {
                [DataMember]
                public Int32 Id { get; set; }
                [DataMember]
                public Int32 Value { get; set; } //TODO: Mode?
                [DataMember]
                public Int32 Min { get; set; }
                [DataMember]
                public Int32 Max { get; set; }


            }

        }
    }



    

}

namespace APLPromoter.Client.Entity
{
    [DataContract]
    public class Filter : ObjectBase
    {
        //[DataMember]
        //public Int32 Id { get; set; }
        [DataContract]
        public class Value
        {
            [DataMember]
            public Int32 Id { get; set; }
            [DataMember]
            public string Code { get; set; }
            [DataMember]
            public string Name { get; set; }
            [DataMember]
            public Boolean Included { get; set; }
            [DataMember]
            public Boolean Dirty { get; set; }
        }
    }
}



