﻿using System.Collections.Generic;
using System.ComponentModel;

namespace XrmEarth.Configuration.Data.Storage
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class StorageContainer
    {
        public List<StorageObjectContainer> ObjectContainers { get; set; }
    }
}
