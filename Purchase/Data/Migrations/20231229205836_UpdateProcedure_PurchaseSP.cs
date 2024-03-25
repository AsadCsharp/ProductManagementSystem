using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purchase.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProcedure_PurchaseSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE TYPE ParamPurchaseDetail 
    AS TABLE
    (
        ProductId INT,
        Quantity DECIMAL(18,2),
        MeasurementUnitId INT,
        ProductUnitPrice DECIMAL(18,2),
        
        PurchaseHeaderId INT
    );
    GO
        CREATE OR ALTER PROCEDURE dbo.PurchaseSP
        @CustomerName NVARCHAR(MAX),
        @CustomerPhoneNumber NVARCHAR(MAX),
        @CustomerEmailAddress NVARCHAR(MAX),
        @InvoiceNumber NVARCHAR(MAX),
        @PurchaseDate DATE,
        @TVP ParamPurchaseDetail READONLY
    AS
    BEGIN
        SET NOCOUNT ON;
        BEGIN TRY
            DECLARE @LocalPurchaseDetail Table
            (
                ProductId INT,
                Quantity DECIMAL(18,2),
                MeasurementUnitId INT,
                ProductUnitPrice DECIMAL(18,2),
                ProductTotalPrice DECIMAL(18,2),
                PurchaseHeaderId INT
            );
            DECLARE @phid int;
            DECLARE @count int;
            DECLARE @SenitalValue int = 0;
            DECLARE @TotalBill DECIMAL(18,2) = 0;
            SELECT @count = COUNT(*) FROM @TVP;
            WHILE @SenitalValue < @count
            BEGIN
                SELECT @TotalBill += Quantity * ProductUnitPrice FROM @TVP ORDER BY ProductId ASC OFFSET @SenitalValue ROWS FETCH FIRST 1 ROWS ONLY;
                SET @SenitalValue += 1;
            END
            INSERT INTO @LocalPurchaseDetail (ProductId, Quantity, MeasurementUnitId, ProductUnitPrice, ProductTotalPrice, PurchaseHeaderId) SELECT ProductId, Quantity, MeasurementUnitId, ProductUnitPrice, Quantity * ProductUnitPrice, PurchaseHeaderId FROM @TVP;
            BEGIN TRANSACTION
                INSERT INTO [dbo].[PurchaseHeader] (CustomerName, CustomerPhoneNumber, CustomerEmailAddress, InvoiceNumber, PurchaseDate, TotalBill) VALUES (@CustomerName, @CustomerPhoneNumber, @CustomerEmailAddress, @InvoiceNumber, @PurchaseDate, @TotalBill);
                SELECT @phid = Id FROM [dbo].[PurchaseHeader] WHERE InvoiceNumber = @InvoiceNumber;
                UPDATE @LocalPurchaseDetail SET PurchaseHeaderId = @phid;
                INSERT INTO [dbo].[PurchaseDetail] (ProductId, Quantity, MeasurementUnitId, ProductUnitPrice, ProductTotalPrice, PurchaseHeaderId) SELECT * FROM @LocalPurchaseDetail;
            COMMIT TRANSACTION
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION
        END CATCH
    END
    GO"
);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS dbo.PurchaseSP");
        }
    }
}
