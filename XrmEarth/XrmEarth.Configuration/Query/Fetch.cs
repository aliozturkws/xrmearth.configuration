using System.Collections.Generic;
using Microsoft.Xrm.Sdk.Query;

namespace XrmEarth.Configuration.Query
{
    public class Fetch
    {
        internal class Tags
        {
            private Tags() { }

            public class Nodes
            {
                public const string Fetch = "fetch";
                public const string Entity = "entity";
                public const string Attribute = "attribute";
                public const string Order = "order";
                public const string Filter = "filter";
                public const string Condition = "condition";
                public const string LinkEntity = "link-entity";
                public const string Value = "value";
                public const string AllAttributes = "all-attributes";
            }

            public class Fetch
            {
                private Fetch() { }

                public const string VersionAttribute = "version";
                public const string OutputFormatAttribute = "output-format";
                public const string MappingAttribute = "mapping";
                public const string DistinctAttribute = "distinct";
                public const string PageAttribute = "page";
                public const string CountAttribute = "count";
                public const string PagingCookieAttribute = "paging-cookie";
                public const string TopAttribute = "top";
                public const string UtcOffsetAttribute = "utc-offset";
                public const string AggregateAttribute = "aggregate";
                public const string MinActiveRowVersionAttribute = "min-active-row-version";
                public const string ReturnTotalRecordCount = "returntotalrecordcount";
                public const string NoLockAttribute = "no-lock";
            }

            public class Entity
            {
                private Entity() { }

                public const string NameAttribute = "name";
            }

            public class Attribute
            {
                private Attribute() { }

                public const string NameAttribute = "name";
                public const string AddedByAttribute = "addedby";
                public const string AggregateAttribute = "aggregate";
                public const string AliasAttribute = "alias";
                public const string BuildAttribute = "build";
                public const string DateGroupingAttribute = "dategrouping";
                public const string GroupByAttribute = "groupby";
                public const string UserTimezoneAttribute = "usertimezone";
            }

            public class Order
            {
                private Order() { }

                public const string AttributeAttribute = "attribute";
                public const string DescendingAttribute = "descending";
            }

            public class Filter
            {
                private Filter() { }

                public const string TypeAttribute = "type";
                public const string IsQuickFindFieldsAttribute = "isquickfindfields";
            }

            public class Condition
            {
                private Condition() { }

                public const string EntityNameAttribute = "entityname";
                public const string AttributeAttribute = "attribute";
                public const string OperatorAttribute = "operator";
                public const string ValueAttribute = "value";
                public const string UiTypeAttribute = "uitype";
            }

            public class Value
            {
                private Value() { }

                public const string UiTypeAttribute = "uitype";
                public const string UiNameAttribute = "uiname";
            }

            public class LinkEntity
            {
                private LinkEntity() { }

                public const string NameAttribute = "name";
                public const string FromAttribute = "from";
                public const string ToAttribute = "to";
                public const string AliasAttribute = "alias";
                public const string VisibleAttribute = "visible";
                public const string IntersectAttribute = "intersect";
                public const string LinkTypeAttribute = "link-type";
            }

            public class AllAttributes
            {

            }
        }

        internal class Values
        {
            public static readonly Dictionary<ConditionOperator, string> ConditionOperators = new Dictionary<ConditionOperator, string>
            {
                { ConditionOperator.Equal, "eq"}
            };
        }
    }
}
