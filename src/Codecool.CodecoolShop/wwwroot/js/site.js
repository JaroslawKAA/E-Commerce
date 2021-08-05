// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let cartButtons = document.getElementsByClassName('addToCartBtn');
let body = document.getElementById('backdropable');
var cartModal = $("#modalContainer");

for (var i=0; i < cartButtons.length; i++){
    cartButtons[i].onclick = function(){
        body.classList.toggle("backdrop");
        $.get(window.location.protocol + '//' + window.location.host + "/Shared/ModalMenu/" + $(this).attr('id'), function (data){
            cartModal.append(data);
        });}
}


// ---------- DROP MENUS IMPLEMENTATION ------------------------
function calcProductsQuantity(products){
    var result = 0;
    for (let i = 0; i < products.length; i++){
        result += products[i].quantity;
    }

    return result;
};

$(document).ready(function () {
    var pageHosting = window.location.protocol + '//' + window.location.host;

    // Load drop menus
    var categoriesMenu = $('#dropdownMenuCategory').next();
    $.get( pageHosting + "/Shared/CategoryDropmenu", function (data){
        categoriesMenu.append(data);
    });
    var suppliersMenu = $('#dropdownMenuSupplier').next();
    $.get(pageHosting + "/Shared/SuppliersDropmenu", function (data){
        suppliersMenu.append(data);
    });

    // Load amount of items in cart
    var cartBtn = $('#cartBtn');
    $.get(pageHosting + "/Shared/CartItems", function (data){
        var cartIcon = cartBtn.children(0);
        cartBtn.empty();
        cartBtn.append(cartIcon);
        if (data != null){
            cartBtn.append("Cart (" + calcProductsQuantity(data) + ")");
        }else{
            cartBtn.append("Cart");
        }
    });
});



