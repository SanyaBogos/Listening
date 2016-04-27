(function () {
    'use strict';

    angular.module('Account')
        .controller('LoginCtrl', function ($scope, $state, AccountDataSvc) {
            var self = this;

            self.successLogin = function (response) {
                $scope.userContext.userName = $scope.user.email;
                $scope.userContext.role = response.data.role;
                $state.go('home');
            };

            self.copyErrors = function (response) {
                $scope.errorsFromServer.slice(0, $scope.errorsFromServer.length - 1);
                angular.copy(response.data, $scope.errorsFromServer);
            };

            $scope.errorsFromServer = [];

            $scope.user = {
                email: '',
                password: '',
                rememberMe: false
            };

            $scope.login = function () {
                console.log($scope.user);
                AccountDataSvc.login($scope.user).then(self.successLogin, self.copyErrors);
            };
        });
})();