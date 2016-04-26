(function () {
    'use strict';

    angular.module('Account')
        .factory('AccountDataSvc', function ($http, $resource) {
            return {
                login: function (login) {
                    return $http({ method: 'POST', url: 'api/Account/Login', data: login });
                },
                logout: function () {
                    return $http({ method: 'POST', url: 'api/Account/LogOff' });
                },
                registry: function (user) {
                    console.log(user);
                    return $http({ method: 'POST', url: '/api/Account/Register', data: user });
                }
            };
        });
})();