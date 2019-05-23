$(document).ready(function() {
    $('#item-qty').on('click',
        function() {
            var qty = $(this).val();
            var price = $('#item-price').html();
            var itemSum = $('#item-sum');
            itemSum.html(parseFloat(price).toFixed(2) * parseFloat(qty).toFixed(2) + '$');
        })
});
