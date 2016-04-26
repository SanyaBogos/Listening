(function () {
    'use strict';

    angular.module('AdditionFunctionality')
        .factory('GenerateIdSvc', function () {

            return {
                generateId: function () {
                    return ObjectId();
                }
            };

        });
})();