jQuery(function () {

});

function addMeasurementUnit(token) {
    class measurementUnit {
        constructor(id, name) {
            this.Id = id;
            this.Name = name;
        }
    }

    let amuvm = new measurementUnit($("#hiddenIdInput").val(), $("#measurementUnitInput").val());

    $.ajax({
        method: "POST",
        url: "/MeasurementUnits/AddUpdate",
        headers: { "RequestVerificationToken": token },
        data: {
            aMeasurementUnitVM: amuvm,
            actionType: $("#actionTypeName").text(),
            token: token
        },
        success: function (result) {
            $("#mutableBody").html(result);
            $("#actionTypeName").text("add");
            $("#addMUButton").text("Add");
        },
        error: function (req, status, error) {
            console.log(error);
        }
    })
}

function editMeasurementUnit(e) {
    $("#measurementUnitInput").val($(e).data("name"));
    $("#hiddenIdInput").val($(e).data("id"));
    $("#actionTypeName").text("edit");
    $("#addMUButton").text("Update");
};

function deleteMeasurementUnit(token, e) {
    $.ajax({
        method: "DELETE",
        url: "/MeasurementUnits/Delete",
        headers: { "RequestVerificationToken": token },
        data: {
            id: e,
            token: token
        },
        success: function (result) {
            $("#mutableBody").html(result);
            $("#actionTypeName").text("add");
            $("#addMUButton").text("Add");
        },
        error: function (req, status, error) {
            console.log(error);
        }
    })
};

function uploadProductImage(e) {
    var file = $(e)[0].files[0];

    if (file) {
        const formData = new FormData();
        formData.append("image", file);
        $.ajax({
            method: "POST",
            url: "/Products/GetImageUrl",
            data: formData,
            contentType: false,
            processData: false,
            success: function (result) {
                $("#productImageUrl").val(result);
                var reader = new FileReader();
                reader.onload = function () {

                    $("#selectedProductImage").attr("src", reader.result);
                }
                reader.readAsDataURL(file);
            }
        });

    }
}

function addProduct(token) {
    class product {
        constructor(id, name, imageUrl) {
            this.Id = id;
            this.Name = name;
            this.ImageUrl = imageUrl;
        }
    }

    let aProduct = new product($("#hiddenProductIdInput").val(), $("#productNameInput").val(), $("#productImageUrl").val());

    $.ajax({
        method: "POST",
        url: "/Products/AddUpdate",
        headers: { "RequestVerificationToken": token },
        data: {
            productVM: aProduct,
            actionType: $("#actionTypeNameForProduct").text(),
            token: token
        },
        success: function (result) {
            $("#productTableBody").html(result);
            $("#actionTypeNameForProduct").text("add");
            $("#addProductButton").text("Add");

            $("#productNameInput").val('');
            $("#inputProductImage").val('');
            $("#selectedProductImage").attr("src", "images/phone_1.jpg");
        },
        error: function (req, status, error) {
            console.log(error);
        }
    })
}

function editProduct(e) {
    $("#productNameInput").val($(e).data("name"));
    $("#hiddenProductIdInput").val($(e).data("id"));
    $("#productImageUrl").val($(e).data("image"));
    $("#actionTypeNameForProduct").text("edit");
    $("#addProductButton").text("Update");
    $("#selectedProductImage").attr("src", $(e).data("image"));
}

function deleteProduct(token, e) {
    $.ajax({
        method: "DELETE",
        url: "/Products/Delete",
        headers: { "RequestVerificationToken": token },
        data: {
            id: e,
            token: token
        },
        success: function (result) {
            $("#productTableBody").html(result);
            $("#actionTypeNameForProduct").text("add");
            $("#addProductButton").text("Add");
        },
        error: function (req, status, error) {
            console.log(error);
        }
    })
};

function calculateTotalPrice() {
    var qty = $("#quantityInput").val();
    var unitPrice = $("#unitPriceInput").val();
    if (Number(qty) != "NaN" && Number(unitPrice) != "NaN") {
        $("#totalPriceSpan").text(Number(qty) * Number(unitPrice));
    }
}

function getPurchaseDetailPartialView(token) {
    class PurchaseDetail {
        constructor(id, productId, quantity, measurementUnitId, unitPrice, totalPrice, purchaseHeaderId, productName, measurementUnitName) {
            this.Id = id;
            this.ProductId = productId;
            this.ProductName = productName;
            this.Quantity = quantity;
            this.MeasurementUnitId = measurementUnitId;
            this.MeasurementUnitName = measurementUnitName;
            this.UnitPrice = unitPrice;
            this.TotalPrice = totalPrice;
            this.PurchaseHeaderId = purchaseHeaderId;
        }
    }

    let aPurchaseDetail = new PurchaseDetail(0, $("#productSelect").val(), $("#quantityInput").val(), $("#measurementUnitSelect").val(), $("#unitPriceInput").val(), $("#totalPriceSpan").text(), 0, $("#productSelect").find(":selected").text(), $("#measurementUnitSelect").find(":selected").text())

    $.ajax({
        method: "POST",
        url: "/Purchases/PurchaseDetail",
        headers: { "RequestVerificationToken": token },
        data: {
            purchaseDetailVM: aPurchaseDetail
        },
        success: function (result) {
            $("#pdvm_tbody").append(result);

            var totalBillAmount = 0;
            $("input[data-tag='TotalPrice']").each(function () {
                var currVal = $(this).val();
                totalBillAmount += Number(currVal);

                $("#totalAmount").val(totalBillAmount);
            });
        },
        error: function (req, status, error) {
            console.log(error);
        }
    })
}

function submitPurchaseOrder(token) {
    class PurchaseDetail {
        constructor(id, productId, quantity, measurementUnitId, unitPrice, totalPrice, purchaseHeaderId, productName, measurementUnitName) {
            this.Id = id;
            this.ProductId = productId;
            this.ProductName = productName;
            this.Quantity = quantity;
            this.MeasurementUnitId = measurementUnitId;
            this.MeasurementUnitName = measurementUnitName;
            this.UnitPrice = unitPrice;
            this.TotalPrice = totalPrice;
            this.PurchaseHeaderId = purchaseHeaderId;
        }
    }

    class PDs {
        constructor() {
            this.purchaseDetails = [];
        }

        newPurchaseDetail(id, productId, quantity, measurementUnitId, unitPrice, totalPrice, purchaseHeaderId, productName, measurementUnitName) {
            let aPurchaseDetail = new PurchaseDetail(id, productId, quantity, measurementUnitId, unitPrice, totalPrice, purchaseHeaderId, productName, measurementUnitName);
            this.purchaseDetails.push(aPurchaseDetail);
            return aPurchaseDetail;
        }

        get allPurchaseDetails() {
            return this.purchaseDetails;
        }
    }

    class PurchaseHeader {
        constructor(id, customerName, customerPhoneNumber, customerEmailAddress, invoiceNumber, purchaseDate, totalAmount) {
            this.Id = id;
            this.CustomerName = customerName;
            this.CustomerPhoneNumber = customerPhoneNumber;
            this.CustomerEmailAddress = customerEmailAddress;
            this.InvoiceNumber = invoiceNumber;
            this.PurchaseDate = purchaseDate;
            this.TotalAmount = totalAmount;
            this.PurchaseDetails;
        }

        addPurchaseDetails(pds) {
            this.PurchaseDetails = pds.allPurchaseDetails;
        }
    }

    let purchaseDetailList = new PDs();

    $.each($("#pdvm_tbody tr"), function (index, purchaseDetailTr) {
        purchaseDetailList.newPurchaseDetail($(purchaseDetailTr).find("input[data-tag='Id']").val(), $(purchaseDetailTr).find("input[data-tag='ProductId']").val(), $(purchaseDetailTr).find("input[data-tag='Quantity']").val(), $(purchaseDetailTr).find("input[data-tag='MeasurementUnitId']").val(), $(purchaseDetailTr).find("input[data-tag='UnitPrice']").val(), $(purchaseDetailTr).find("input[data-tag='TotalPrice']").val(), $(purchaseDetailTr).find("input[data-tag='PurchaseHeaderId']").val(), $(purchaseDetailTr).find("input[data-tag='ProductName']").val(), $(purchaseDetailTr).find("input[data-tag='MeasurementUnitName']").val())
    });

    let aPurchaseHeader = new PurchaseHeader(0, $("#customerNameInput").val(), $("#customerPhoneInput").val(), $("#customerEmailInput").val(), "&nbsp;", $("#purchaseDateInput").val(), $("#totalAmount").val());
    aPurchaseHeader.addPurchaseDetails(purchaseDetailList);

    $.ajax({
        method: "POST",
        url: "/Purchases/Create",
        headers: { "RequestVerificationToken": token },
        data: {
            purchaseHeaderVM: aPurchaseHeader,
            token: token
        },
        success: function (result) {
            $("#purchaseCardsDiv").append(result);
        },
        error: function (req, status, error) {
            console.log(error);
        }
    })
}

function submitPurchaseOrderForSP(token) {
    class PurchaseDetail {
        constructor(id, productId, quantity, measurementUnitId, unitPrice, totalPrice, purchaseHeaderId, productName, measurementUnitName) {
            this.Id = id;
            this.ProductId = productId;
            this.ProductName = productName;
            this.Quantity = quantity;
            this.MeasurementUnitId = measurementUnitId;
            this.MeasurementUnitName = measurementUnitName;
            this.UnitPrice = unitPrice;
            this.TotalPrice = totalPrice;
            this.PurchaseHeaderId = purchaseHeaderId;
        }
    }

    class PDs {
        constructor() {
            this.purchaseDetails = [];
        }

        newPurchaseDetail(id, productId, quantity, measurementUnitId, unitPrice, totalPrice, purchaseHeaderId, productName, measurementUnitName) {
            let aPurchaseDetail = new PurchaseDetail(id, productId, quantity, measurementUnitId, unitPrice, totalPrice, purchaseHeaderId, productName, measurementUnitName);
            this.purchaseDetails.push(aPurchaseDetail);
            return aPurchaseDetail;
        }

        get allPurchaseDetails() {
            return this.purchaseDetails;
        }
    }

    class PurchaseHeader {
        constructor(id, customerName, customerPhoneNumber, customerEmailAddress, invoiceNumber, purchaseDate, totalAmount) {
            this.Id = id;
            this.CustomerName = customerName;
            this.CustomerPhoneNumber = customerPhoneNumber;
            this.CustomerEmailAddress = customerEmailAddress;
            this.InvoiceNumber = invoiceNumber;
            this.PurchaseDate = purchaseDate;
            this.TotalAmount = totalAmount;
            this.PurchaseDetails;
        }

        addPurchaseDetails(pds) {
            this.PurchaseDetails = pds.allPurchaseDetails;
        }
    }

    let purchaseDetailList = new PDs();

    $.each($("#pdvm_tbody tr"), function (index, purchaseDetailTr) {
        purchaseDetailList.newPurchaseDetail($(purchaseDetailTr).find("input[data-tag='Id']").val(), $(purchaseDetailTr).find("input[data-tag='ProductId']").val(), $(purchaseDetailTr).find("input[data-tag='Quantity']").val(), $(purchaseDetailTr).find("input[data-tag='MeasurementUnitId']").val(), $(purchaseDetailTr).find("input[data-tag='UnitPrice']").val(), $(purchaseDetailTr).find("input[data-tag='TotalPrice']").val(), $(purchaseDetailTr).find("input[data-tag='PurchaseHeaderId']").val(), $(purchaseDetailTr).find("input[data-tag='ProductName']").val(), $(purchaseDetailTr).find("input[data-tag='MeasurementUnitName']").val())
    });

    let aPurchaseHeader = new PurchaseHeader(0, $("#customerNameInput").val(), $("#customerPhoneInput").val(), $("#customerEmailInput").val(), "&nbsp;", $("#purchaseDateInput").val(), $("#totalAmount").val());
    aPurchaseHeader.addPurchaseDetails(purchaseDetailList);

    $.ajax({
        method: "POST",
        url: "/Purchases/CreateExecutingSP",
        headers: { "RequestVerificationToken": token },
        data: {
            purchaseHeaderVM: aPurchaseHeader,
            token: token
        },
        success: function (result) {
            window.location.href = "https://localhost:44344/Purchases";
            //$("#purchaseCardsDiv").append(result);
        },
        error: function (req, status, error) {
            console.log(error);
        }
    })
}

function removeMe(e) {
    e.parent().parent().remove();

    var totalBillAmount = 0;
    $("input[data-tag='TotalPrice']").each(function () {
        var currVal = $(this).val();
        totalBillAmount += Number(currVal);
        console.log(totalBillAmount);
        $("#totalAmount").val(totalBillAmount);
    });
}