function Point(x,y) {

    this.x = x;
    this.y = y;

    this.X = ko.observable(x);
    this.Y = ko.observable(y);

}

function timeblock(startpoint, endpoint) {

    var self = this;

    this.startpoint = startpoint;
    this.endpoint = endpoint;

    this.top = endpoint.y > startpoint.y ? startpoint.y : endpoint.y;
    this.left = endpoint.x > startpoint.x ? startpoint.x : endpoint.x;

    this.height = Math.abs(endpoint.y - startpoint.y);
    this.width = Math.abs(endpoint.x - startpoint.x);

    this.top += "px";
    this.left += "px";
    this.height += "px";
    this.width += "px";

    this.selected = ko.observable(false);


}


function timeapp() {

    var self = this;

    this.startdown = ko.observable(null);

    this.selected = null;

    this.blocks = ko.observableArray([]);

    this.setDown = function (data, e) {

        e.stopPropagation();
        
        self.startdown(new Point(e.pageX, e.pageY));




    };

    this.setUp = function (data, e) {

        

        if (this.startdown() != null) {

            if (Math.abs((this.startdown().x - e.pageX) * (this.startdown().y - e.pageY)) > 25) {

                this.blocks.push(new timeblock(this.startdown(), new Point(e.pageX, e.pageY)));
            }



        }

        this.startdown(null);


    };

    this.selectBlock = function (data, e) {
        e.stopPropagation();

        if (self.selected != null) {

            self.selected.selected(false);


        }

        self.selected = this;

        this.selected(true);

    };

}