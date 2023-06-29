using System.Collections.Generic;
using System;
using Umbraco.Cms.Core.Composing;
using punkOptimise.Interfaces;

namespace punkOptimise.Models
{
    public class PunkOptimiseProvidersCollection : BuilderCollectionBase<IPunkOptimiserProvider>
    {
        public PunkOptimiseProvidersCollection(Func<IEnumerable<IPunkOptimiserProvider>> items)
            : base(items)
        { }
    }
}