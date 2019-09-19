using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashRegistryAPR0400.Models.Handlers
{
    class TransactionComponentHandler
    {
        internal static TransactionComponent Create(Transaction transaction, Product product, double quantity)
        {
            TransactionComponent output = new TransactionComponent
            {
                /*
                 * We dont want to save the actual Product in the transaction because if we later on change the price
                 * of something that previously has been bought all the historical transactions will be modified so we "clone" the product into a component of a transaction
                 * */
                Transaction = transaction,
                ProductName = product.Name,
                ProductPrice = product.Price,
                ProductCategory = product.Category,
                Quantity = quantity,
                
            };
            return output;
        }

        internal static string Summarize(TransactionComponent tc)
        {
            return String.Format("{0} - {1} * {2} - {3}", tc.ProductName, tc.ProductPrice, tc.Quantity, tc.ProductPrice*tc.Quantity);
        }
    }
}
