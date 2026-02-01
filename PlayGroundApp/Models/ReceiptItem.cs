using SQLite;

namespace PlayGroundApp.Models
{
    /// <summary>
    /// Represents an individual item purchased in a receipt.
    /// This model stores details about a single product on a receipt,
    /// including product name, price, category, and quantity.
    /// It has a many-to-one relationship with Receipt.
    /// </summary>
    [Table("ReceiptItems")]
    public class ReceiptItem
    {
        /// <summary>
        /// Unique identifier for the receipt item. Auto-incremented primary key.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Foreign key reference to the parent Receipt.
        /// Indexed for improved query performance when filtering items by receipt.
        /// </summary>
        [Indexed]
        public int ReceiptId { get; set; }

        /// <summary>
        /// Name of the product purchased.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Price of the product at the time of purchase.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Category classification for the product (e.g., Groceries, Electronics, etc.).
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Quantity of the product purchased. Defaults to 1.
        /// </summary>
        public int Quantity { get; set; } = 1;

        /// <summary>
        /// Navigation property for back-reference to the parent Receipt.
        /// Not persisted directly to the database (marked with [Ignore]).
        /// </summary>
        [Ignore]
        public Receipt Receipt { get; set; }
    }
}