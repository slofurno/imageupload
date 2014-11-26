define(function (require) {
    // Ensure jQuery is required as it will likely be used in the carousel
   // var $ = require('jquery');

    // jQuery plugins just augment the $.prototype and therefore
    // don't need to return anything. So these don't need to be assigned
    // to a variable

    // Define an object and then return it for instantiation later
    var SelectMap = function (element) {

        //var AF = window.requestAnimationFrame;
        var canvas = element;
        //var canvas = document.getElementById("ctx");
        var width = 800;
        var height = 600;
        var ctx = canvas.getContext("2d");

        function drawline(x1, y1, x2, y2) {

            ctx.moveTo(x1, y1);
            ctx.lineTo(x2, y2);

            ctx.strokeStyle = "blue";
            ctx.stroke();

        };

        function drawrect(x1, y1, x2, y2) {

            ctx.rect(x1, y1, x2 - x1, y2 - y1);
            ctx.stroke();

        };

        function clearrect() {

            ctx.clearRect(0, 0, width, height);
        };


        function draw() {
            clearrect();


            drawline(20, 20, 400, 400);
            drawrect(20, 20, 400, 400);


           // AF(draw);
        };

        draw();
        //element.append('Carousel module loaded!');




    };

   // SelectMap.prototype = {



   // };

    return SelectMap;
});