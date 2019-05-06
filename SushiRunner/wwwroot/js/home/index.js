$(".menu-toggle").click(function (e) {
    e.preventDefault();
    $("#wrapper").toggleClass("toggled");
});

var itemCount = 0;

$('.add-to-card-btn').click(function (){
    itemCount ++;
    $('#itemCount').html(itemCount).css('display', 'flex');
});
