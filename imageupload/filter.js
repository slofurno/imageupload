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

function Filter(name, url, np, filter) {

    var self = this;

    this.filter = filter;

    this.list = ko.observableArray([]);

    this.nameproperty = np;

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

    this.filterarray = ko.observableArray([new Filter('Account', 'api/account', function (item, match) { return item.lastName == match; }), new Filter('User', 'api/user', function (item, match) { return item.users.indexOf(match) >= 0; }), new Filter('Topic', 'api/topic', function (item, match) { return item.topics.indexOf(match)>= 0 })]);

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

}

