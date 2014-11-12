(function (_global) {
    "use strict";

    var shim = {};
    if (typeof (exports) === 'undefined') {
        if (typeof define == 'function' && typeof define.amd == 'object' && define.amd) {
            shim.exports = {};
            define(function () {
                return shim.exports;
            });
        } else {
            
            shim.exports = typeof (window) !== 'undefined' ? window : _global;
        }
    }
    else {
       
        shim.exports = exports;
    }

    (function (exports) {

       
        /**
         * @class tree
         * @name tree
         */
        var tree = tree || {}
        exports.tree = tree;


        tree.success = 1;
        tree.fail = -1;
        tree.pending = 0;

        tree.context = function (someobject) {

            this.object = someobject




        };

        tree.root = function (someobject) {

            this.context = new tree.context(someobject);


        };

        /**
         * leaf node
         *
         
         * @returns {status} out
         */
        tree.sequence = function () {

            var self = this;
            var currentindex = 0;

            var nodes = [];

            var nextNode = function () {

                if (nodes.length > currentindex) {



                }

            };

            this.tick = function () {



                var status = nodes[currentindex].tick();

                if (status == tree.success) {

                    if (nodes.length > currentindex) {
                        currentindex++;
                        return tree.pending;

                    }
                    else {
                        return tree.success;
                    }

                }
                else {

                    return status;

                }

            };





        };

        /**
         * leaf node
         *
         * @param {action} action delegate
         * @param {success} success condition
         * @param {fail} failure condition
         * @returns {vec2} out
         */

        tree.leaf = function (action, success, fail) {

            var action = action;
            var success = success;
            var fail = fail;

            this.tick = function (object) {

                object.try(action);




            };


        };


    })(shim.exports);

})(this);


function testobject() {

    
    this.try = function (action) {

        


    };


};





