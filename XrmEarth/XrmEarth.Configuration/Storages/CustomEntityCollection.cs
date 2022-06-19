using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using XrmEarth.Configuration.Common;
using XrmEarth.Configuration.Data.Core;
using XrmEarth.Configuration.Query;

namespace XrmEarth.Configuration.Storages
{
    public class CustomEntityCollection : List<CustomEntity>, ICrmStorage
    {
        public CustomEntityCollection(IEnumerable<Entity> entities, EntityTemplate template) : this(template)
        {
            Bind(entities);
        }

        public CustomEntityCollection(EntityTemplate template)
        {
            _entityTemplate = template;
        }

        private readonly EntityTemplate _entityTemplate;

        public void WriteValues(IOrganizationService service, Dictionary<string, ValueContainer> values)
        {
            Clear();
            Bind(values);

            var query = BuildQuery(values.Keys);
            var fetchExpression = new FetchExpression(query);
            var result = service.RetrieveMultiple(fetchExpression);
            var dataSource = new CustomEntityCollection(result.Entities, _entityTemplate);

            var requests = new List<OrganizationRequest>();
            foreach (var e in this)
            {
                var existEntity = dataSource.FirstOrDefault(ds => ds.Key == e.Key);
                if (existEntity != null)
                {
                    e.ID = existEntity.ID;
                    requests.Add(new UpdateRequest { Target = e.Create(_entityTemplate) });
                }
                else
                {
                    requests.Add(new CreateRequest { Target = e.Create(_entityTemplate) });
                }
            }
            SendRequest(service, requests);
        }

        public Dictionary<string, ValueContainer> ReadValues(IOrganizationService service, Dictionary<string, Type> keys)
        {
            var query = BuildQuery(keys.Keys, true);
            var fetchExpression = new FetchExpression(query);
            var result = service.RetrieveMultiple(fetchExpression);

            Clear();
            Bind(result.Entities);

            return ToValueContainerDictionary();
        }

        public void Bind(Dictionary<string, ValueContainer> values)
        {
            foreach (var keyValue in values)
            {
                var e = new CustomEntity
                {
                    Key = keyValue.Key,
                    Value = keyValue.Value.Value?.ToString()
                };
                Add(e);
            }
        }

        public void Bind(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var e = new CustomEntity(entity, _entityTemplate);
                Add(e);
            }
        }

        public Dictionary<string, ValueContainer> ToValueContainerDictionary()
        {
            var dic = new Dictionary<string, ValueContainer>();
            foreach (var e in this)
            {
                dic[e.Key] = new ValueContainer(e.Value);
            }
            return dic;
        }


        public string BuildQuery(IEnumerable<string> keys, bool includeValues = false)
        {
            var builder = new XmlBuilder();

            builder
                .Node(Fetch.Tags.Nodes.Fetch)
                .Attribute(Fetch.Tags.Fetch.VersionAttribute, "1.0")
                .Attribute(Fetch.Tags.Fetch.OutputFormatAttribute, "xml-platform")
                .Attribute(Fetch.Tags.Fetch.NoLockAttribute, "true")
                .Within()
                .Node(Fetch.Tags.Nodes.Entity)
                .Attribute(Fetch.Tags.Entity.NameAttribute, _entityTemplate.Name)
                .Within()
                .Node(Fetch.Tags.Nodes.Attribute)
                .Attribute(Fetch.Tags.Attribute.NameAttribute, _entityTemplate.KeyName);

            if (includeValues)
            {
                builder
                    .Node(Fetch.Tags.Nodes.Attribute)
                    .Attribute(Fetch.Tags.Attribute.NameAttribute, _entityTemplate.ValueName);
            }

            builder
                .Node(Fetch.Tags.Nodes.Filter)
                .Attribute(Fetch.Tags.Filter.TypeAttribute, "or")
                .Within();

            foreach (var key in keys)
            {
                builder
                    .Node(Fetch.Tags.Nodes.Condition)
                    .Attribute(Fetch.Tags.Condition.AttributeAttribute, _entityTemplate.KeyName)
                    .Attribute(Fetch.Tags.Condition.ValueAttribute, key)
                    .Attribute(Fetch.Tags.Condition.OperatorAttribute, Fetch.Values.ConditionOperators[ConditionOperator.Equal]);
            }

            builder
                .EndWithin()
                .EndWithin()
                .EndWithin();

            return builder.GetOuterXml();
        }

        private void SendRequest(IOrganizationService service, IEnumerable<OrganizationRequest> requests)
        {
            var multipleRequest = new ExecuteMultipleRequest
            {
                Requests = new OrganizationRequestCollection(),
                Settings = new ExecuteMultipleSettings{ ContinueOnError = true, ReturnResponses = true }
            };
            foreach (var request in requests)
            {
                multipleRequest.Requests.Add(request);
            }
            //TODO : Multiple Request Check
            service.Execute(multipleRequest);
        }
    }
}
