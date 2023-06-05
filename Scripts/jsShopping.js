var cf = false;
$(document).ready(function () {
    ShowCount();
    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault(); //ngăn chặn hành động mặc định của trình duyệt khi nhấn nút, như tải lại trang web.
        var id = $(this).data('id');
        var quantity = 1;
        var tQuantity = $('#quantity_value').text();
        if (tQuantity != '') {
            quantity = parseInt(tQuantity);
        }
        //ajax: bất đồng bộ, cập nhật một phần của trang web mà không cần tải lại toàn bộ trang.
        $.ajax({
            url: '/cartmanager/addtocart',
            type: 'POST',
            data: { id: id, quantity: quantity },
            success: function (rs) {
                if (rs.Success) {
                    //$('#checkout_items').html(rs.Count);
                    alert(rs.msg);
                    document.getElementById("checkout_items").innerHTML = rs.Count;
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
    $('body').off('click', '.btnDelete').on('click', '.btnDelete', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        if (!cf) {
            var conf = confirm("Bạn có chắc muốn xóa sản phẩm này khỏi giỏ hàng?");
            if (conf == true) {
                cf = true;
                //ajax: bất đồng bộ, cập nhật một phần của trang web mà không cần tải lại toàn bộ trang.
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
                        }
                    }
                });
            }
        }
    });
    $('body').off('click', '.btnDeleteAll').on('click', '.btnDeleteAll', function (e) {
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
            $('#checkout_items').html(rs.Count);
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