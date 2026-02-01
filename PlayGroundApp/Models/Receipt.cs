using SQLite;
using System;
using System.Collections.Generic;

namespace PlayGroundApp.Models
{
    /// <summary>
    /// Represents a receipt entity in the application.
    /// This model stores information about a purchase receipt including store details,
    /// purchase date, and an associated image. It has a one-to-many relationship with ReceiptItem.
    /// </summary>
    [Table("Receipts")]
    public class Receipt
    {
        /// <summary>
        /// Unique identifier for the receipt. Auto-incremented primary key.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Name of the store where the purchase was made.
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// The date and time when the purchase was made.
        /// </summary>
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Path to the image file of the receipt (e.g., photo of the receipt).
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Timestamp when the receipt record was created in UTC.
        /// Defaults to current UTC time.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property for the one-to-many relationship.
        /// Contains all ReceiptItem entries associated with this receipt.
        /// Not persisted directly to the database (marked with [Ignore]).
        /// </summary>
        [Ignore]
        public List<ReceiptItem> Items { get; set; } = new List<ReceiptItem>();
    }
}