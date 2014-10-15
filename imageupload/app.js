function color(r, g, b) {

    var self = this;

    self.r = r;
    self.g = g;
    self.b = b;

    self.getColor = function () {

        return 'rgb(' + self.r + ',' + self.g + ',' + self.b + ')';

    };


}


function pixel(i) {

    var self = this;
    self.colorindex = ko.observable(i);

    self.myColor = ko.observable(i);

    self.changeColor = function (d) {

        console.log(d);

        self.myColor(d);

    };




}



function app(mousehandler) {

    var self = this;

    self.mousestate = mousehandler;

    self.title = ko.observable("first");

    self.selectedColor = ko.observable(new color(0, 0, 0));
    //self.colorPalette = ['Gray', 'DarkSlateBlue', 'red', 'ForestGreen', 'SandyBrown'];

    self.colorPalette = [new color(0, 0, 0), new color(208, 0, 0), new color(127, 255, 212), new color(0, 208, 0), new color(0, 0, 208), new color(106, 90, 205)];

   

    self.width = 10;
    self.height = 10;

    var length = 100;

    self.pixels = [];

    for (var i = 0; i < length; i++) {

        //self.pixels[i] = new pixel((i % self.colorPalette.length));

        self.pixels[i] = new pixel(self.colorPalette[(i % self.colorPalette.length)]);


    }

    self.setColor = function (i) {



        //self.selectedColor(self.colorPalette.indexOf(i));

        console.log(i);

        self.selectedColor(i);


    };

    self.setPixelColor = function (pixel) {

        if (self.mousestate.mousedown) {

            pixel.changeColor(self.selectedColor());

        }

    };

    self.submitImage = function () {

        var temparray = [];

        for (var i = 0; i < self.pixels.length; i++) {

            console.log(self.pixels[i]);

            
            temparray.push(parseInt(self.pixels[i].myColor().b));
            temparray.push(parseInt(self.pixels[i].myColor().g));
            temparray.push(parseInt(self.pixels[i].myColor().r));
            temparray.push(parseInt(255));

        }

        var pdata = { "data": temparray, "title":self.title() };

        console.log(temparray);

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/imageupload.asmx/UploadImage",
            data: JSON.stringify(pdata),
            dataType: "json",
            async: true,
            success: function (data, textStatus) {

                var msg;

                if (textStatus == "success") {
                    if (data.hasOwnProperty('d')) {
                        msg = data.d;
                    } else {
                        msg = data;
                    }
                    console.log(msg);

                }
            },
            error: function (data, status, error) {


                console.log(error);
            }
        });



    };

    // ko.observableArray([0, 1, 2, 3, 2, 3, 2, 3, 1, 3, 1, 2, 3, 0, 2, 1, 2]);





    //return {selectedColor: selectedColor, colorPalette:colorPalette, pixels:pixels};

}