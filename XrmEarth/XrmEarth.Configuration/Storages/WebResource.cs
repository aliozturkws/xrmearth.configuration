using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using XrmEarth.Configuration.Data.Core;
using XrmEarth.Configuration.Utility;
using XrmEarth.Configuration.Common;
using XrmEarth.Configuration.Query;

namespace XrmEarth.Configuration.Storages
{
    public class WebResource : ICrmStorage
    {
        internal const string LogicalName = "webresource";
        internal const string SolutionLogicalName = "solution";
        internal const string NameField = "name";
        internal const string DisplayNameField = "displayname";
        internal const string DescriptionField = "description";
        internal const string SolutionIDField = "solutionid";
        internal const string ResourceTypeField = "webresourcetype";
        internal const string ContentField = "content";
        internal const string SolutionUniqueNameParam = "SolutionUniqueName";

        public Guid ID { get; set; }
        public Guid? SolutionID { get; private set; }
        public string SolutionUniqueName { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public WebResourceType ResourceType { get; set; }
        public string RawContent { get; set; }

        public Encoding Encoding { get; set; }

        public string Content
        {
            get
            {
                if (string.IsNullOrWhiteSpace(RawContent))
                    return null;

                var binary = Encoding.GetBytes(RawContent);
                return Convert.ToBase64String(binary);
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    RawContent = null;
                    return;
                }

                var binary = Convert.FromBase64String(value);
                RawContent = Encoding.GetString(binary);
            }
        }

        public void WriteValues(IOrganizationService service, Dictionary<string, ValueContainer> values)
        {
            var jsonData = JsonSerializerUtil.Serialize(values);
            RawContent = jsonData;

            var query = BuildQuery();
            var fetchExpression = new FetchExpression(query);
            var result = service.RetrieveMultiple(fetchExpression);
            if (result.Entities.Count == 0)
            {
                var entity = ToEntity();
                var createRequest = new CreateRequest { Target = entity };
                if (!string.IsNullOrWhiteSpace(SolutionUniqueName))
                {
                    createRequest.Parameters.Add(SolutionUniqueNameParam, SolutionUniqueName);
                }
                var response = (CreateResponse)service.Execute(createRequest);
                ID = response.id;
            }
            else
            {
                Bind(result.Entities.First());
                var entity = ToEntity();
                service.Update(entity);
            }
            Publish(service);
        }

        public Dictionary<string, ValueContainer> ReadValues(IOrganizationService service, Dictionary<string, Type> keys)
        {
            var query = BuildQuery(true);
            var fetchExpression = new FetchExpression(query);
            var result = service.RetrieveMultiple(fetchExpression);
            if (result.Entities.Count == 0)
                return null;

            Bind(result.Entities.First());
            var jsonData = RawContent;
            return JsonSerializerUtil.Deserialize<Dictionary<string, ValueContainer>>(jsonData);
        }

        public void Publish(IOrganizationService service)
        {
            var request = new PublishXmlRequest
            {
                ParameterXml = "<importexportxml>" +
                               "   <webresources>" +
                               "       <webresource>{" + ID + "}</webresource>" +
                               "   </webresources>" +
                               "</importexportxml>"
            };


            service.Execute(request);
        }

        public Entity ToEntity()
        {
            var entity = new Entity(LogicalName, ID)
            {
                Attributes =
                {
                    {NameField, Name },
                    {DisplayNameField, DisplayName },
                    {DescriptionField, Description},
                    {ResourceTypeField, new OptionSetValue(ResourceType.GetHashCode())},
                    {ContentField, Content}
                }
            };

            //TODO: Check!
            //if (SolutionID.HasValue && SolutionID.Value != Guid.Empty)
            //{
            //    entity.Attributes.Add(SolutionIDField, new EntityReference(SolutionLogicalName, SolutionID.Value));
            //}

            return entity;
        }

        public void Bind(Entity entity)
        {
            ID = entity.Id;

            if (entity.Contains(ContentField))
                Content = entity.GetAttributeValue<string>(ContentField);

            if (entity.Contains(NameField))
                Name = entity.GetAttributeValue<string>(NameField);

            if (entity.Contains(DisplayNameField))
                DisplayName = entity.GetAttributeValue<string>(DisplayNameField);

            if (entity.Contains(DescriptionField))
                Description = entity.GetAttributeValue<string>(DescriptionField);

            if (entity.Contains(ResourceTypeField))
                ResourceType = (WebResourceType)entity.GetAttributeValue<OptionSetValue>(ResourceTypeField).Value;

            if (entity.Contains(SolutionIDField))
                SolutionID = entity.GetAttributeValue<EntityReference>(SolutionIDField).Id;
        }

        public string BuildQuery(bool includeContent = false, string[] otherAttributes = null)
        {
            var builder = new XmlBuilder();

            builder
                .Node(Fetch.Tags.Nodes.Fetch)
                .Attribute(Fetch.Tags.Fetch.VersionAttribute, "1.0")
                .Attribute(Fetch.Tags.Fetch.OutputFormatAttribute, "xml-platform")
                .Attribute(Fetch.Tags.Fetch.NoLockAttribute, "true")
                .Within()
                .Node(Fetch.Tags.Nodes.Entity)
                .Attribute(Fetch.Tags.Entity.NameAttribute, LogicalName)
                .Within()
                .Node(Fetch.Tags.Nodes.Attribute)
                .Attribute(Fetch.Tags.Attribute.NameAttribute, NameField);

            if (includeContent)
            {
                builder
                    .Node(Fetch.Tags.Nodes.Attribute)
                    .Attribute(Fetch.Tags.Attribute.NameAttribute, ContentField);
            }

            if (otherAttributes != null && otherAttributes.Length > 0)
            {
                foreach (var attribute in otherAttributes)
                {
                    if (attribute == ContentField && includeContent) //Already exist
                        continue;

                    builder
                        .Node(Fetch.Tags.Nodes.Attribute)
                        .Attribute(Fetch.Tags.Attribute.NameAttribute, attribute);
                }
            }

            builder
                .Node(Fetch.Tags.Nodes.Filter)
                .Attribute(Fetch.Tags.Filter.TypeAttribute, "and")
                .Within()
                .Node(Fetch.Tags.Nodes.Condition)
                .Attribute(Fetch.Tags.Condition.AttributeAttribute, NameField)
                .Attribute(Fetch.Tags.Condition.ValueAttribute, Name)
                .Attribute(Fetch.Tags.Condition.OperatorAttribute, Fetch.Values.ConditionOperators[ConditionOperator.Equal]); ;

            builder
                .EndWithin()
                .EndWithin()
                .EndWithin();

            return builder.GetOuterXml();
        }
    }
}
