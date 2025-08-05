using Database.Interfaces;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositoryes
{
    internal class DocumentReceiptRepository : BaseRepository<DocumentReceipt>, IDocumentReceiptRepository
    {
        public DocumentReceiptRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public async Task<bool> NumberExistsAsync(int number)
        {
            return await Context.ReceiptDocuments
                .AnyAsync(d => d.Number == number);
        }
    }
}
