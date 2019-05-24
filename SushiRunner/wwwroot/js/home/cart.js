$(document).ready(function () {
    $('.cart-amount-inc-button').on('click', incrementAmount);
});

$(document).ready(function () {
    $('.cart-amount-dec-button').on('click', decrementAmount);
});

function incrementAmount() {
    var amount = parseInt($(this).siblings('input').val());
    if (amount) {
        if (amount < 99) {
            amount++;
        }
    } else {
        amount = 1;
    }
    var price = $(this).closest('.cart-table-row')
        .children('.price')
        .text()
        .trim()
        .split(' ')[0];
    price = parseInt(price);
    var total = (price * amount).toString();
    $(this).closest('.cart-table-row').children('.sum').text(total + 'грн.');
    $(this).siblings('input').val(amount);
    updateTotalPrice();
}

function decrementAmount() {
    var amount = parseInt($(this).siblings('input').val());
    if (amount) {
        if (amount > 1) {
            amount--;
        }
    } else {
        amount = 1;
    }
    var price = $(this).closest('.cart-table-row')
        .children('.price')
        .text()
        .trim()
        .split(' ')[0];
    price = parseInt(price);
    var total = (price * amount).toString();
    $(this).closest('.cart-table-row').children('.sum').text(total + 'грн.');
    $(this).siblings('input').val(amount);
    updateTotalPrice();
}

function updateTotalPrice() {
    var total = 0;
    $('.cart-table-row').each(function (index) {
        var price = $(this).children('.sum')
            .text()
            .trim()
            .split(' ')[0];
        total += parseInt(price);
    });

    $('#total-sum').html("<strong>Загальна сума: </strong>" + total + " грн.");
}
