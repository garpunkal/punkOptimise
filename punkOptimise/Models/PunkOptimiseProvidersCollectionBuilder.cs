using punkOptimise.Interfaces;
using Umbraco.Cms.Core.Composing;

namespace punkOptimise.Models
{
    public class PunkOptimiseProvidersCollectionBuilder
        : OrderedCollectionBuilderBase<PunkOptimiseProvidersCollectionBuilder, PunkOptimiseProvidersCollection, IPunkOptimiserProvider>
    {
        protected override PunkOptimiseProvidersCollectionBuilder This => this;
    }
}