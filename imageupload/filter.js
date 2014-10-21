ko.observableArray.fn.pushAll = function (valuesToPush) {
    var underlyingArray = this();
    this.valueWillMutate();
    ko.utils.arrayPushAll(underlyingArray, valuesToPush);
    this.valueHasMutated();
    return this;  //optional
};

function FilterOption(filter, options) {

    var self = this;

    this.type = filter;
    this.options = options;

    this.selectedOption = filter;



}


function Filter2(somefilter) {

    var self = this;

    this.filter = somefilter;

    this.selectedOption = ko.observable();

}

function Filter(name, url, filter) {

    var self = this;

    this.filter = filter;

    this.list = ko.observableArray([]);

 

    this.name = ko.observable(name);


    this.selectedOption = ko.observable();

    $.get(url, function (data) {


        self.list.pushAll(data);



    });



}


function FilterViewModel(data) {


    var self = this;

    this.data = data;

    

    this.options = ['Account id', 'speaker id', 'topic'];

    this.chosenOption = ko.observable();

    this.filters = ko.observableArray([]);

    this.filterarray = ko.observableArray([

        new Filter('Account', 'api/account', function (item, match) {
                
            var contains = false;
 

            //$.each(item["initiativeAccount"], function (i, value) {

            console.log("match " + match["id"]);
            console.log("account " + item["initiativeAccount"]["accountId"]);

            if (item["initiativeAccount"]["accountId"] == match['id']) { contains = true;  }


            //    if (value['accountId'] == match['id']) { contains = true; return false; }
            //})


            return contains;

        }), new Filter('User', 'api/user', function (item, match) {

            var contains = false;

            

            console.log("users " + item["users"]);
//
           


               // console.log("match " + match["id"]);

                $.each(item["users"], function (i, value) {

                    console.log(value["userId"]);
                    if (value['userId'] == match['id']) { contains = true; return false; }
                })
            
        
            return contains;


         }), new Filter('Topic', 'api/topic', function (item, match) {
    

             var contains = false;


             $.each(item["topics"], function (i, value) {

                 console.log(value["topicId"]);
                 if (value['topicId'] == match['id']) { contains = true; return false; }
             })


      

            return contains;

         })

    ]);

    this.chosenOption.subscribe(function (newValue) {
        if (newValue != null) {
            console.log(newValue);
            

            //self.filters.push(new FilterOption(newValue, self.options));

            //self.filters.push(newValue)

            self.filters.push(new Filter2(newValue));

            self.chosenOption(null);

        }
    });


    this.removeFilter = function () {

        self.filters.remove(this);

    };


    this.filteredData = ko.computed(function () {

        var temp;

        return ko.utils.arrayFilter(self.data, function (item) {

            var passed = true;

            ko.utils.arrayForEach(self.filters(), function (value) {


                if (value.selectedOption() != undefined) {

                    console.log(value.selectedOption());

                    if (value.filter.filter(item, value.selectedOption())) {

                       



                    }
                    else {
                        passed = false;
                        return false;

                    }

                }
     



            });

            return passed;



        });

    });

}

