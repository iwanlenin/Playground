using SQLite;
using PlayGroundApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayGroundApp.Services
{
    /// <summary>
    /// Provides database access and operations for Receipt and ReceiptItem entities.
    /// This service encapsulates all database interactions using SQLiteAsyncConnection,
    /// providing CRUD operations and transactional support for maintaining data consistency.
    /// Implements async/await patterns for all database operations.
    /// </summary>
    public class DatabaseService
    {
        /// <summary>
        /// The SQLite async connection instance used for all database operations.
        /// </summary>
        private readonly SQLiteAsyncConnection database;

        /// <summary>
        /// Initializes a new instance of the DatabaseService class.
        /// Creates a new SQLiteAsyncConnection and initializes database tables.
        /// </summary>
        /// <param name="databasePath">The file path where the SQLite database will be stored.</param>
        public DatabaseService(string databasePath)
        {
            this.database = new SQLiteAsyncConnection(databasePath);
            InitializeDatabaseAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Initializes the database by creating tables for Receipt and ReceiptItem if they don't exist.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task InitializeDatabaseAsync()
        {
            await this.database.CreateTableAsync<Receipt>();
            await this.database.CreateTableAsync<ReceiptItem>();
        }

        /// <summary>
        /// Retrieves all receipts from the database, ordered by creation date in descending order (newest first).
        /// </summary>
        /// <returns>A list of Receipt objects ordered by CreatedAt descending.</returns>
        public async Task<List<Receipt>> GetReceiptsAsync()
        {
            return await this.database.Table<Receipt>()
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a single receipt by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the receipt to retrieve.</param>
        /// <returns>The Receipt object if found; otherwise null.</returns>
        public async Task<Receipt> GetReceiptAsync(int id)
        {
            return await this.database.Table<Receipt>()
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves a single receipt by its ID and loads all associated ReceiptItem entries.
        /// Populates the Items navigation property with related items.
        /// </summary>
        /// <param name="id">The unique identifier of the receipt to retrieve.</param>
        /// <returns>The Receipt object with Items populated if found; otherwise null.</returns>
        public async Task<Receipt> GetReceiptWithItemsAsync(int id)
        {
            var receipt = await GetReceiptAsync(id);
            if (receipt != null)
            {
                receipt.Items = await GetReceiptItemsAsync(id);
            }
            return receipt;
        }

        /// <summary>
        /// Saves a receipt to the database. Inserts a new receipt if Id is 0,
        /// otherwise updates the existing receipt.
        /// </summary>
        /// <param name="receipt">The Receipt object to save.</param>
        /// <returns>The ID of the saved receipt.</returns>
        public async Task<int> SaveReceiptAsync(Receipt receipt)
        {
            if (receipt.Id == 0)
            {
                return await this.database.InsertAsync(receipt);
            }
            else
            {
                await this.database.UpdateAsync(receipt);
                return receipt.Id;
            }
        }

        /// <summary>
        /// Deletes a receipt from the database by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the receipt to delete.</param>
        /// <returns>The number of rows deleted (0 if receipt not found, 1 if successful).</returns>
        public async Task<int> DeleteReceiptAsync(int id)
        {
            return await this.database.DeleteAsync<Receipt>(id);
        }

        /// <summary>
        /// Retrieves all ReceiptItem entries associated with a specific receipt.
        /// </summary>
        /// <param name="receiptId">The ID of the parent receipt.</param>
        /// <returns>A list of ReceiptItem objects for the specified receipt.</returns>
        public async Task<List<ReceiptItem>> GetReceiptItemsAsync(int receiptId)
        {
            return await this.database.Table<ReceiptItem>()
                .Where(ri => ri.ReceiptId == receiptId)
                .ToListAsync();
        }

        /// <summary>
        /// Saves a receipt item to the database. Inserts a new item if Id is 0,
        /// otherwise updates the existing item.
        /// </summary>
        /// <param name="receiptItem">The ReceiptItem object to save.</param>
        /// <returns>The ID of the saved receipt item.</returns>
        public async Task<int> SaveReceiptItemAsync(ReceiptItem receiptItem)
        {
            if (receiptItem.Id == 0)
            {
                return await this.database.InsertAsync(receiptItem);
            }
            else
            {
                await this.database.UpdateAsync(receiptItem);
                return receiptItem.Id;
            }
        }

        /// <summary>
        /// Deletes a receipt item from the database by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the receipt item to delete.</param>
        /// <returns>The number of rows deleted (0 if item not found, 1 if successful).</returns>
        public async Task<int> DeleteReceiptItemAsync(int id)
        {
            return await this.database.DeleteAsync<ReceiptItem>(id);
        }

        /// <summary>
        /// Saves a receipt and all its associated items in a single transactional operation.
        /// Ensures data consistency by either committing all changes or rolling back on failure.
        /// Automatically sets the ReceiptId on all items after saving the receipt.
        /// </summary>
        /// <param name="receipt">The Receipt object to save.</param>
        /// <param name="items">The list of ReceiptItem objects to associate with the receipt.</param>
        /// <returns>The ID of the saved receipt.</returns>
        public async Task<int> SaveReceiptWithItemsAsync(Receipt receipt, List<ReceiptItem> items)
        {
            await this.database.RunInTransactionAsync(async conn =>
            {
                // Save the receipt first to get its ID
                var receiptId = await SaveReceiptAsync(receipt);

                // Set ReceiptId for all items
                foreach (var item in items)
                {
                    item.ReceiptId = receiptId;
                }

                // Save all receipt items
                foreach (var item in items)
                {
                    await SaveReceiptItemAsync(item);
                }
            });

            return receipt.Id;
        }
    }
}
