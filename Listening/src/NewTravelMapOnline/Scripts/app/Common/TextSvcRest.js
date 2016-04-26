(function () {
    'use strict';

    angular.module('Common')
        .factory('TextSvcRest', function ($resource) {
            //return $resource('api/Text/:parameter',
            //    { parameter: '@param' },
            //    { 'update': { method: 'PUT' } });

            return $resource('api/Text/:id',
                null,
                {
                    'update': { method: 'PUT' },
                    'insert': { method: 'POST' },
                    'remove': { method: 'DELETE' }
                });
        });
})();