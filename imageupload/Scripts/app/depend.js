define(["./app"], function (app) {
    //return an object to define the "my/shirt" module.
    return {
        color: "blue",
        size: "large",
        printTest: function () {
            
            app.print(this);
        }
    }
}
);