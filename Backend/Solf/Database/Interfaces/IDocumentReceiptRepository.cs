using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Interfaces
{
    public interface IDocumentReceiptRepository :  IRepository<DocumentReceipt>
    {
        Task<bool> NumberExistsAsync(int number);
    }
}
