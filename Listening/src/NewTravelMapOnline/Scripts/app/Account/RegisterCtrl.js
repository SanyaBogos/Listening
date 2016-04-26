(function () {
    'use strict';

    angular.module('Account')
        .controller('RegisterCtrl', function ($scope, $state, AccountDataSvc, CheckDataSvc) {
            var self = this;

            self.copyErrors = function (response) {
                angular.copy(response.data, $scope.errorsFromServer);
            };

            self.setUserontext = function () {
                $scope.userContext.userName = $scope.user.email;
                $state.go('home');
            };

            $scope.errorsFromServer = [];

            $scope.user = {
                email: '',
                password: '',
                confirmPassword: ''
            };

            $scope.emailCheck = function () {
                return CheckDataSvc.emailCheck($scope.user.email);
            };

            $scope.passwordCheck = function () {
                return CheckDataSvc.passwordCheck($scope.user.password);
            };

            $scope.confirmPasswordCheck = function () {
                return CheckDataSvc.confirmPasswordCheck($scope.user.confirmPassword, $scope.user.password);
            };

            $scope.registryClick = function () {
                AccountDataSvc.registry($scope.user).then(self.setUserontext, self.copyErrors);
            };
        });
})();