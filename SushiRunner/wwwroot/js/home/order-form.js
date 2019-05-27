$(".menu-toggle").click(function (e) {
    e.preventDefault();
    $("#wrapper").toggleClass("toggled");
});

$("#order-button-start").click(function (e) {
    $("#total-sum").hide();
    $("#make-order-form-wrapper").show();
});
