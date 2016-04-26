(function () {
    'use strict';

    angular.module('Account')
        .factory('CheckDataSvc', function () {
            return {
                emailCheck: function (email) {
                    if (!!email && email.length !== 0 &&
                    !/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                    .test(email)) {
                        return 'Incorrect email';
                    }
                },
                passwordCheck: function (password) {
                    var errorMessage = '';

                    var constraints = [
                        { name: 'digit', regexp: '[0-9]' },
                        { name: 'big letter', regexp: '[A-Z]' },
                        { name: 'little letter', regexp: '[a-z]' },
                        { name: 'special character', regexp: '[!@#$%^&*(){}\_+-]' },
                    ];

                    if (!!password && password.length !== 0) {
                        angular.forEach(constraints, function (elem) {
                            if (new RegExp(elem.regexp).exec(password) === null) {
                                errorMessage = 'Password should contain at least one ' + elem.name;
                            }
                        });
                    }

                    return errorMessage;
                },
                confirmPasswordCheck: function (confirmPassword, password) {
                    if (!!confirmPassword && confirmPassword.length !== 0
                        && confirmPassword !== password) {
                        return 'Passwords are not the same';
                    }
                }
            };
        });
})();