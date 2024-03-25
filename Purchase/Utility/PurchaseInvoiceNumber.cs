namespace Purchase.Utility
{
    public static class PurchaseInvoiceNumber
    {
        public static string Get()
        {
            return DateTime.Now.ToString("yyMMddhhmmssffff");
        }
    }
}
