using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using XrmEarth.Configuration.Data.Core;

namespace XrmEarth.Configuration.Storages
{
    public interface ICrmStorage
    {
        void WriteValues(IOrganizationService service, Dictionary<string, ValueContainer> values);
        Dictionary<string, ValueContainer> ReadValues(IOrganizationService service, Dictionary<string, Type> keys);
    }
}
