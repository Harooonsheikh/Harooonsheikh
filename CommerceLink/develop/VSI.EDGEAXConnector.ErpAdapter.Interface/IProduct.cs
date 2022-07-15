using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.Adapter.Interface
{
    public interface IProduct<C, A, P, PC>
    {
        List<C> GetCatagories();

        List<A> GetProductAttributesMetadata();

        List<KeyValuePair<string, IEnumerable<P>>> GetAllProducts();

        List<KeyValuePair<string, IEnumerable<P>>> GetUpdatedProducts();

        Dictionary<string, List<long>> GetCatalogsToBeDeleted(List<PC> catalogs);

        List<long> GetProductsToBeDeleted(long CatalogId, List<long> publishedIds);


        ErpLinkedProduct GetProductLinkedItemExtension(string itemId, string variantId);
    }
}
