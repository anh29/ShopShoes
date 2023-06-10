$(document).ready(function () {
    ShowCount();

    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quantity = 1;
        var tQuantity = $('#quantity_value').val();
        if (tQuantity != undefined) {
            quantity = parseInt(tQuantity);
        }

        $.ajax({
            url: '/cartmanager/addtocart',
            type: 'POST',
            data: { id: id, quantity: quantity },
            success: function (rs) {
                if (rs.Success) {
                    console.log(quantity.toString());
                    alert(rs.msg);
                    ShowCount(); // Update the cart count after successful addition
                }
            }
        });
    });

    $('body').on('click', '.btnUpdate', function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        var quantity = $('#Quantity_' + id).val();
        Update(id, quantity);
    });

    $('body').on('click', '.btnDelete', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        if (!cf) {
            var conf = confirm("Bạn có chắc muốn xóa sản phẩm này khỏi giỏ hàng?");
            if (conf == true) {
                cf = true;
                $.ajax({
                    url: '/cartmanager/Delete',
                    type: 'POST',
                    data: { id: id },
                    async: true,
                    success: function (rs) {
                        if (rs.Success) {
                            $('#checkout_items').html(rs.Count);
                            $('#trow_' + id).remove();
                            LoadCart();
                            ShowCount(); // Update the cart count after successful deletion
                        }
                    }
                });
            }
        }
    });

    $('body').on('click', '.btnDeleteAll', function (e) {
        e.preventDefault();
        var conf = confirm("Bạn có chắc muốn xóa hết sản phẩm trong giỏ hàng không?");
        if (conf == true) {
            DeleteAll();
        }
    });
});

function ShowCount() {
    $.ajax({
        url: '/cartmanager/ShowCount',
        type: 'GET',
        success: function (rs) {
            if (rs.Success) {
                $('#checkout_items').html(rs.Count);
            }
        }
    });
}

function DeleteAll() {
    $.ajax({
        url: '/cartmanager/DeleteAll',
        type: 'POST',
        success: function (rs) {
            if (rs.Success) {
                LoadCart();
            }
        }
    });
}
function Update(id, quantity) {
    $.ajax({
        url: '/cartmanager/Update',
        type: 'POST',
        data: { id: id, quantity: quantity },
        success: function (rs) {
            if (rs.Success) {
                LoadCart();
            }
        }
    });
}
function LoadCart() {
    $.ajax({
        url: '/cartmanager/Partial_Item_Cart',
        type: 'GET',
        success: function (rs) {
            $('#load_data').html(rs);
        }
    });
}