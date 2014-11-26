define(function (require) {
    // Load any app-specific modules
    // with a relative require call,
    // like:
    var messages = require('./depend');

    console.log(messages);

    messages.printTest();
    // Load library/vendor modules using
    // full IDs, like:
    //var print = require('print');
    //print(messages.getHello());
});